using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Luminate
{
    /// <summary>
    /// This class is used parse the output file and create a virtual mapping of the JS structure and its comments
    /// </summary>
    public class DocumentLexer
    {
        public List<Token> tokens = new List<Token>();


        /// <summary>
        /// Use this function to parse a string and create program Tokens
        /// </summary>
        /// <param name="text"></param>
        public void parse(string text)
        {
            // First match all this. variables
            MatchCollection matches = Regex.Matches(text, @"@type([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*\/+", RegexOptions.IgnoreCase);
            Token token = null;

            List<Token> tempTokens = new List<Token>();

            int count = matches.Count;
            foreach (Match m in matches)
            {
                if (m.Success)
                {
                    string blockComment = m.Value;

                    blockComment = Regex.Replace(blockComment, @"\*|/", "");
                    
                    string[] parts = blockComment.Split('@');

                    if (parts.Length >= 4 )
                    {
                        Dictionary<string, string> commentSections = new Dictionary<string,string>();
                        int paramCount = 1;

                        foreach (string s in parts)
                        {
                            if (s != "")
                            {
                                string typeData = Regex.Replace(s, @"\s\s+", " ");
                                string[] lineSplit = typeData.Split(' ');

                                if (lineSplit.Length > 1)
                                {
                                    if ( lineSplit[0] == "param" )
                                    {
                                        lineSplit[0] = "param" + paramCount.ToString();
                                        paramCount++;
                                    }


                                    if (commentSections.ContainsKey(lineSplit[0]) == false)
                                        commentSections.Add(lineSplit[0], s.Substring(lineSplit[0].Length + 1));
                                }
                            }
                        }

                        if (commentSections.ContainsKey("type"))
                        {
                            token = new Token();
                            tempTokens.Add(token);
                            foreach (KeyValuePair<string, string> pair in commentSections)
                            {
                                switch (pair.Key)
                                {
                                    case "type":

                                        string[] typeSplit = pair.Value.Replace("\r\n", "").Split(' ');
                                        if (typeSplit.Length > 0)
                                            token.visibility = typeSplit[0];
                                        if (typeSplit.Length > 1)
                                            token.type = typeSplit[1];
                                        if (typeSplit.Length > 2)
                                            token.name = Regex.Replace(typeSplit[2], @"<|>", "");
 
                                        break;
                                    case "description":
                                        token.desc = pair.Value.Replace("\r\n", " ");
                                        break;
                                    case "name":
                                        token.name = pair.Value;
                                        break;
                                    case "author":
                                        token.author = pair.Value;
                                        break;
                                    case "created":
                                        token.created = pair.Value;
                                        break;
                                    case "extends":
                                        string extends = pair.Value.Replace("\r\n", "");
                                        token.extends = Regex.Replace(extends, @"<|>", "").Trim();
                                        break;
                                   case "returns":
                                        string[] returnSplit = pair.Value.Replace("\r\n", "").Split(' ');

                                        if ( returnSplit.Length > 1 )
                                        {
                                            string returnsType = Regex.Replace(returnSplit[0], @"<|>", "");

                                            
                                            token.returnType = returnsType;

                                            string rdesc = pair.Value.Substring(returnsType.Length + 3);
                                            token.returnDesc = rdesc;
                                        }

                                        break;
                                    default:
                                        if ( pair.Key.Contains("param") )
                                        {
                                            string[] paramParts = pair.Value.Split(' ');
                                            if (paramParts.Length > 2)
                                            {
                                                string ptype = Regex.Replace(paramParts[0], @"<|>", "");
                                                string pname = paramParts[1].Replace("\r\n", "");
                                                string pdesc = pair.Value.Substring(ptype.Length + pname.Length + 3);
                                                
                                                Token paramToken = new Token();
                                                paramToken.name = pname;
                                                paramToken.desc = pdesc;
                                                paramToken.type = ptype;

                                                if ( token.parameters.ContainsKey( paramToken.name ) == false )
                                                    token.parameters.Add(paramToken.name, paramToken);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            //Now that all the tokens are created - lets link them up
            tokens.Clear();
            foreach (Token t in tempTokens)
            {
                if (t.type == "class")
                {
                    tokens.Add(t);
                }
                //Add the variables to a class
                else
                {
                    bool addedToClass = false;
                    foreach (Token classToken in tempTokens)
                    {
                        if (classToken.type == "class" && t.extends != null && t.extends.Trim() == classToken.name.Trim())
                        {
                            addedToClass = true;

                          

                            if (t.type == "mvar" && !classToken.mVariables.ContainsKey(t.name.Trim()))
                                classToken.mVariables.Add(t.name.Trim(), t);
                            else if (t.type == "svar" && !classToken.sVariables.ContainsKey(t.name.Trim()))
                                classToken.sVariables.Add(t.name.Trim(), t);
                            else if (t.type == "mfunc" && !classToken.mFunctions.ContainsKey(t.name.Trim()))
                                classToken.mFunctions.Add(t.name.Trim(), t);
                            else if (t.type == "sfunc" && !classToken.sFunctions.ContainsKey(t.name.Trim()))
                                classToken.sFunctions.Add(t.name.Trim(), t);
                            else if (t.type == "enum" && !classToken.enums.ContainsKey(t.name.Trim()))
                                classToken.enums.Add(t.name.Trim(), t);
                            break;
                        }
                    }

                    if (addedToClass == false)
                        tokens.Add(t);
                        
                }
            }

            //Setup all inherrited functions and variables
            foreach (Token t in tempTokens)
                if (t.type == "class")
                    ImplementSubClasses(t, t.extends, tempTokens);
        }

        /// <summary>
        /// Recursively fills the functions and variables for a class and its parents
        /// </summary>
        /// <param name="mainClass"></param>
        /// <param name="subclass"></param>
        /// <param name="tokens"></param>
        private void ImplementSubClasses(Token mainClass, string subclass, List<Token> tokens)
        {
            if (subclass == null)
                return;

            foreach (Token t in tokens)
                if (t.type == "class" && subclass.Trim() == t.name.Trim() && t != mainClass)
                {
                    //member functions
                    foreach (KeyValuePair<string, Token> func in t.mFunctions)
                        if (mainClass.mFunctions.ContainsKey(func.Key) == false)
                            mainClass.mFunctions.Add(func.Key, func.Value);

                    //static funcions
                    foreach (KeyValuePair<string, Token> func in t.sFunctions)
                        if (mainClass.sFunctions.ContainsKey(func.Key) == false)
                            mainClass.sFunctions.Add(func.Key, func.Value);

                    //static vars
                    foreach (KeyValuePair<string, Token> func in t.sVariables)
                        if (mainClass.sVariables.ContainsKey(func.Key) == false)
                            mainClass.sVariables.Add(func.Key, func.Value);

                    //member vars
                    foreach (KeyValuePair<string, Token> func in t.mVariables)
                        if (mainClass.mVariables.ContainsKey(func.Key) == false)
                            mainClass.mVariables.Add(func.Key, func.Value);

                    //Enums
                    foreach (KeyValuePair<string, Token> func in t.enums)
                        if (mainClass.enums.ContainsKey(func.Key) == false)
                            mainClass.enums.Add(func.Key, func.Value);


                    if (mainClass.mFunctions.ContainsKey(t.name.Trim()) == false)
                        mainClass.mFunctions.Add(t.name.Trim(), t);

                    ImplementSubClasses(mainClass, t.extends, tokens);
                }
        }
    }
}
