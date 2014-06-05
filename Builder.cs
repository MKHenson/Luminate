using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Yahoo.Yui.Compressor;
using System.Text.RegularExpressions;
using System.Net;
using System.Threading;

namespace Luminate
{
    class Builder
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        private delegate void SetTextCallback(string text, Logger.MessageType type);

        public static List<string> ignoreList = new List<string>();

        /// <summary>
        /// Builds the files into a single file
        /// </summary>
        public static void Build( Project p, string prepend )
        {
            DirectoryInfo info = new DirectoryInfo(p.directory);
            FileInfo[] files = info.GetFiles("*.js", SearchOption.AllDirectories);
            Dictionary<string, string> fileContents = new Dictionary<string, string>();
            StreamReader reader = null;
            string endString = "";

            string baseDir = p.directory.Replace('\\', '/').Trim();
            string startFile = p.First.Replace('\\', '/').Trim();
            string outputFile = p.OutputFile.Replace('\\', '/').Trim();
            string endFile = p.Last.Replace('\\', '/').Trim();

            if (outputFile == "" || outputFile == null)
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (Logger.Singleton.mList.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Log);
                    Logger.Singleton.mList.Invoke(d, new object[] { "You have not specified an output file!", Logger.MessageType.ERROR });
                }
                else
                    Log("You have not specified an output file!", Logger.MessageType.ERROR);

                return;
            }

            //Look at all the files
            for (int i = 0; i < files.Length; i++ )
            {
                if (files[i].FullName.Replace('\\', '/').Trim() == startFile.Trim() ||
                    files[i].FullName.Replace('\\', '/').Trim() == endFile.Trim() ||
                    files[i].FullName.Replace('\\', '/').Trim() == outputFile.Trim())
                    continue;

                try
                {
                    bool ignore = false;
                    //Remove all the excluded objects
                    foreach (string excludeStr in ignoreList)
                        if (files[i].FullName.Replace('\\', '/').Trim() == excludeStr.Replace('\\', '/').Trim())
                        {
                            ignore = true;
                            break;
                        }

                    if (ignore)
                        continue;

                    reader = new StreamReader(files[i].FullName);
                    fileContents.Add(files[i].FullName.Replace('\\', '/').Trim(), reader.ReadToEnd() + "\n\n");
                    reader.Close();

                    Luminate.Singleton.ProgressPercent( (int)(((float)i / (float)files.Length) * 100) );
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                    if (reader != null)
                        reader.Close();
                }
            }

            //Build a sort array
            string[] fileContentsArr = new string[fileContents.Values.Count];
            int counter = 0;
            foreach (string value in fileContents.Values)
            {
                fileContentsArr[counter] = value;
                counter++;
            }

            //Sort
            //Array.Sort(fileContentsArr, new FileSorter(fileContents, mTextboxFolder.Text));
            Sort(fileContentsArr, fileContents, baseDir);

           

            try
            {
                //First do first file
                if ( startFile != null && startFile.Trim() != "" && !File.Exists( startFile.Trim() ) )
                    Logger.Singleton.Log("The start file for project " + p.Name + " cannot be found.", Logger.MessageType.ERROR);
                else if ( startFile.Trim() != "")
                {
                    reader = new StreamReader(startFile);
                    endString = reader.ReadToEnd() + "\n\n";
                    reader.Close();

                    
                }
            }
            catch (Exception ex)
            {
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                if (reader != null)
                    reader.Close();
            }

            foreach (string s in fileContentsArr)
                endString += s;

            //Now the final string
            if (endFile != null && endFile.Trim() != "" && !File.Exists(endFile.Trim()))
                Logger.Singleton.Log("The end file for project " + p.Name + " cannot be found.", Logger.MessageType.ERROR);
            else if ( endFile.Trim() != "")
            {
                try
                {
                    reader = new StreamReader(endFile);
                    endString += reader.ReadToEnd();
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                    if (reader != null)
                        reader.Close();
                }
            }

            //Minify
            if (p.Minify)
            {
                try
                {
                    JavaScriptCompressor comp = new JavaScriptCompressor(endString, true);
                    endString = comp.Compress();
                }
                catch (Exception ex)
                {
                    // First match all this. variables
                    MatchCollection matches = Regex.Matches(ex.Message, @"(\b)Line:(\W)*(\w)*", RegexOptions.IgnoreCase);

                    foreach (Match m in matches)
                        if (m.Success)
                        {
                            string v = m.Value;
                            string[] data = v.Split(':');
                            v = data[1].Trim();

                            Logger.Singleton.Log("Javascript Error: " + ex.Message + " on line " + v + ". ",
                                Logger.MessageType.ERROR, new string[] { p.OutputFile, v });
                        }
                    
                }
            }

            if (prepend != null && prepend != "")
                endString = prepend + endString;

            p.CompiledJS = endString;

            //Finally write
            try
            {
                StreamWriter writer = new StreamWriter(outputFile);
                writer.Write(endString);
                writer.Close();

                if (p.UseFTP)
                {
                    Thread t = new Thread (StartFTP);          // Kick off a new thread
                    t.Start(p);
                }
            }
            catch (Exception ex)
            {
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
            }
        }

        /// <summary>
        /// Tries to send the compiled file to an FTP location
        /// </summary>
        private static void StartFTP(object proj)
        {
            Project p = proj as Project;

            try
            {
                //Now check for FTP
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(p.FTPLocation);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(p.FTPUser, p.FTPPassword);

                // Get the data in bytes
                byte[] ftpData = Encoding.UTF8.GetBytes(p.CompiledJS);

                request.ContentLength = ftpData.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(ftpData, 0, ftpData.Length);                
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (Logger.Singleton.mList.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Log);
                    Logger.Singleton.mList.Invoke(d, new object[] { "Upload Completed: " + response.StatusDescription, Logger.MessageType.MESSAGE });
                }
                else
                    Log(response.StatusDescription, Logger.MessageType.MESSAGE);

                response.Close();
            }
            catch (Exception ex)
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (Logger.Singleton.mList.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Log);
                    Logger.Singleton.mList.Invoke(d, new object[] { ex.Message, Logger.MessageType.ERROR });
                }
                else
                    Log(ex.Message, Logger.MessageType.ERROR);
            }
        }

        /// <summary>
        /// Makes a thread safe log message
        /// </summary>
        /// <param name="message"></param>
        private static void Log(string message, Logger.MessageType type)
        {
            Logger.Singleton.Log(message, type);
        }

        private static void Sort(string[] arrayToSort, Dictionary<string, string> files, string folderDir)
        {
            if (files.Count == 0)
                return;

            int n = arrayToSort.Length;
            bool changeMade = true;

            int count = 0;

            while (changeMade)
            {
                count++;
                List<object> foundObjs = new List<object>();
                Console.WriteLine("=================================================");
                changeMade = false;

                for (int i = 0; i < n; i++)
                {
                    for (int ii = 0; ii < n; ii++)
                    {
                        if (arrayToSort[i] == arrayToSort[ii])
                            continue;

                        int comparison = Compare(arrayToSort[i], arrayToSort[ii], files, folderDir, i);
                        if (comparison > 0 && ii > i)
                        {
                            string temp = arrayToSort[i];
                            arrayToSort[i] = arrayToSort[ii];
                            arrayToSort[ii] = temp;
                            changeMade = true;
                        }
                    }
                }

                if (count > files.Count)
                {
                    Logger.Singleton.Log("A Cyclic Dependency occurred, please make sure that no two files are referrencing each other", Logger.MessageType.ERROR); 
                    break;
                }
            }

        }

        private static int Compare(string x, string y, Dictionary<string, string> files, string folderDir, int index)
        {
            string mBaseDir = folderDir;

            DirectoryInfo info = new DirectoryInfo(mBaseDir);
            mBaseDir = info.FullName;

            if (mBaseDir[mBaseDir.Length - 1] != '/')
                mBaseDir += '/';

            mBaseDir = mBaseDir.Replace('\\', '/');

            // Here we call Regex.Match.
            MatchCollection matches = Regex.Matches(x, @"@import\((.*?)\)", RegexOptions.IgnoreCase);

           
            //LOGGING
            //=====================================================
            //foreach (KeyValuePair<string, string> pair in files)
            //    if (pair.Value.Length == x.Length && pair.Value == x)
            //        Console.Write("x: " + pair.Key.Split('/')[pair.Key.Split('/').Length - 1] + " ");
           // foreach (KeyValuePair<string, string> pair in files)
            //    if (pair.Value.Length == y.Length && pair.Value == y)
            //        Console.Write("y: " + pair.Key.Split('/')[pair.Key.Split('/').Length - 1] + " ");
            //=====================================================

            if (x == y)
                return 0;

            // X is referencing files
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                    if (m.Success)
                    {
                        string fileRequired = m.Groups[1].Value.Trim();
                        fileRequired = fileRequired.Replace('\\', '/');
                        fileRequired = fileRequired.Replace("./", "/");

                        while (fileRequired[0] == '/')
                            fileRequired = fileRequired.Substring(1, fileRequired.Length - 1);

                        fileRequired = mBaseDir + fileRequired;

                        //Check if a file
                        if (File.Exists(fileRequired))
                        {
                            foreach (KeyValuePair<string, string> pair in files)
                                if (pair.Value.Length == y.Length && pair.Value == y)
                                {
                                    //If the filerequired is y - then y needs to come after x
                                    if (fileRequired == pair.Key)
                                    {
                                        //Console.WriteLine("1");
                                        return 1;
                                    }
                                }

                            //Console.WriteLine("0");
                            //return 0;
                        }
                        else
                        {
                            int counter = 0;
                            foreach (KeyValuePair<string, string> pair in files)
                            {
                                if (counter == index)
                                {
                                    Logger.Singleton.Log("'" + pair.Key + "' tried to import the file " + fileRequired + "', but failed because the file cannot be found.", Logger.MessageType.ERROR);
                                    break;
                                }
                                else
                                    counter++;
                            }
                            
                        }
                    }
            }

           // Console.WriteLine("0");
            return 0;
        }
    }
}
