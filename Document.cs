using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using ScintillaNET;
using System.Text.RegularExpressions;
using System.Timers;

namespace Luminate
{
    /// <summary>
    /// A document class to edit text files
    /// </summary>
    public partial class Document : DockContent
    {
        // Delegates for cross threaded calls
        private delegate void ImageUpdate();
        private delegate void TextUpdate( string content );
        private delegate void TimerCalled(object sender, ElapsedEventArgs e);

        private Scintilla mDoc;
        private bool mSaved;
        public string file;
        public bool forceRemove;
        public bool canSave;
        private List<string> mKeywords;
        private Dictionary<string, Token> mKeywordInfo = new Dictionary<string, Token>();
        private string[] mKeywordsToAdd;
        private PictureBox mPicBox;

        //Word suggestion
        private Dictionary<string, string[]> mFunctionParams;
        private string mSuggestWord;
        private ArgumentsControl mArgControl;
        private ClassDescriptor mDescriptor;

        public Document() : base() { }

        private bool mUpdateOnActive;

        private bool mOEMPeriodPushed;
        private bool mPerformKeyUpChecks;

        private string mLastHoverWord;
        private System.Timers.Timer mTimer;

        /// <summary>
        /// Simple constructor
        /// </summary>
        public Document( string file = "", string content = "" ) : base()
        {
            InitializeComponent();

            mFunctionParams = new Dictionary<string, string[]>();
            mSuggestWord = "";
            mLastHoverWord = "";
            canSave = true;
            this.file = file;
            forceRemove = false;
            mKeywords = new List<string>();
            mDoc = new Scintilla();
            Controls.Add(mDoc);
            mDoc.Dock = DockStyle.Fill;
            mTimer = new System.Timers.Timer( 5000 );
           

            mDoc.KeyDown += new KeyEventHandler(OnKeyDown);
            mDoc.KeyUp += new KeyEventHandler(OnKeyUp);
            mDoc.TextChanged += new EventHandler(OnTextChanged);
            mDoc.Margins[0].Width = 20;
            mDoc.ContextMenuStrip = contextMenuStrip;
            mDoc.DoubleClick += new EventHandler(OnDocDoubleClick);
            mDoc.Click += new EventHandler(OnClick);
            mDoc.Commands.RemoveBinding('f', Keys.Control, BindableCommand.ShowFind);

            mDoc.Commands.RemoveBinding('d', Keys.Control);
            mDoc.Commands.AddBinding('d', Keys.Control, BindableCommand.LineDelete);

            mDoc.GotFocus += new EventHandler(OnGotFocus);
            mDoc.Click += new EventHandler(OnDocumentClick);
            

            mDoc.KeyUp += new KeyEventHandler(mDoc_KeyUp);

            mDoc.MouseMove += new MouseEventHandler( OnMouseMove );

            mPerformKeyUpChecks = true;

            string lowerCase = file.ToLower();
            if (lowerCase.Contains(".js"))
            {
                //Set the syntax highlighting to JS
                mDoc.ConfigurationManager.Language = "js";

                mKeywords.AddRange( new string[] {"function","if","subclass","var","prototype","break","const",
                    "continue","delete","do","while","export","for","in","else","import","instanceof","label",
                    "let","new","return","switch","this","throw","try","catch","typeof","void","while","with",
                    "yield", "alert","setTimeout"});

                mDoc.Styles["COMMENTDOC"].ForeColor = System.Drawing.Color.FromArgb(0x3284BF);

            }
            else if (lowerCase.Contains(".html"))
            {
                //Set the syntax highlighting to HTML
                mDoc.ConfigurationManager.Language = "html";

                //JS keywords
                mKeywords.AddRange(new string[] {"function","if","subclass","var","prototype","break","const",
                    "continue","delete","do","while","export","for","in","else","import","instanceof","label",
                    "let","new","return","switch","this","throw","try","catch","typeof","void","while","with",
                    "yield","alert","setTimeout", "jQuery"});

                //HTML keywords
                mKeywords.AddRange(new string[] {"html","body","link","head","style","class",
                    "span","h1","h2","h3","div","checkbox","text","javascript","script","link","src",
                    "rel","href","frame","table","th","td","tr","object","textarea","strike","center","select",
                    "ul","li","noscript", "meta", "img", "form", "submit", "option", "checked", "font", "color", "arial", "canvas"});
            }
            else if (lowerCase.Contains(".php"))
            {
                //Set the syntax highlighting to HTML
                mDoc.ConfigurationManager.Language = "html";
            }
            else if (lowerCase.Contains(".css"))
            {
                //Set the syntax highlighting to CSS
                mDoc.ConfigurationManager.CustomLocation = Application.StartupPath + "\\" + "css.xml";
                mDoc.ConfigurationManager.Language = "css";

                //HTML keywords
                mKeywords.AddRange(new string[] {"html","body","link","head","style","class", "left", "right", "top", "bottom",
                    "margin", "border", "float", "italic", "bold", "text-align", "margin-bottom", "margin-top",
                    "margin-left", "margin-right", "border-top", "border-bottom", "border-left", "border-right",
                    "clear", "both", "inherrited", "important", "center", "font", "font-weight", "font-size", "padding", "padding-left",
                    "padding-right", "padding-top", "padding-bottom", "display", "inline", "block", "visibility", "hidden", "width", "height",
                    "color", "none", "list-style", "background-color", "border-collapse", "table-layout", "collapse", "overflow",
                    "span","h1","h2","h3","div","checkbox","text","javascript","script","link","src", "fixed", "border-collapse",
                    "rel","href","frame","table","th","td","tr","object","textarea","strike","center","select", "border-style", "solid",
                    "dashed", "dotted", "ThreeDShadow", "ThreeDLightShadow", "ThreeDHighlight", "ThreeDFace", "ThreeDDarkShadow", 
                    "Scrollbar", "cursor", "pointer", "position", "absolute", "relative", "auto", "z-index", "text-decoration",
                    "underline", "overline", "blink", "line-through", "white-space", "nowrap", "pre", "prewrap", "pre-line", "normal",
                    "url", "transparent", "background", "vertical-align", "background-image", 
                    "ul","li","noscript", "meta", "img", "form", "submit", "option", "checked", "font", "color", "arial", "canvas"});
            }
            else if (lowerCase.Contains(".xml"))
                //Set the syntax highlighting to XML
                mDoc.ConfigurationManager.Language = "xml";
            else if (lowerCase.Contains(".jpg") || lowerCase.Contains(".jpeg") || lowerCase.Contains(".png") || lowerCase.Contains(".bmp") || lowerCase.Contains(".gif"))
            {
                //Set the syntax highlighting to XML
                mPicBox = new PictureBox();
                Controls.Add(mPicBox);
                try
                {
                    mPicBox.ImageLocation = file;
                    mPicBox.Load();
                }
                catch (Exception ex)
                {
                    Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                }

                mPicBox.Dock = DockStyle.Fill;
                Controls.Remove(mDoc);
            }
            
            mKeywords.Sort();
            mUpdateOnActive = true;
            if (content == null)
                UpdateContents(false);
            else
                mDoc.Text = content;

            //Must come just after the above code
            mUpdateOnActive = false;
            mDoc.UndoRedo.EmptyUndoBuffer();           

            string[] split = file.Split('\\');
            Text = split[ split.Length - 1 ];

            mDoc.Folding.IsEnabled = true;
            mDoc.Folding.MarkerScheme = FoldMarkerScheme.BoxPlusMinus;
            mDoc.Folding.Flags = FoldFlag.LineAfterContracted;

            mDoc.Folding.UseCompactFolding = true;
            mDoc.Margins.Margin1.IsClickable = true;
            mDoc.Margins.Margin1.IsFoldMargin = true;
            mDoc.Margins.Margin1.Width = 20;
            mDoc.AutoComplete.IsCaseSensitive = false;
            mDoc.AutoComplete.RegisterImages(imageList);
            
            mDoc.Commands.RemoveBinding(Keys.Up, BindableCommand.LineUp);
            mDoc.Commands.RemoveBinding(Keys.Down, BindableCommand.LineDown);
            mDoc.Commands.RemoveBinding(Keys.Enter, BindableCommand.AutoCComplete);
            mDoc.Commands.RemoveBinding(Keys.OemPeriod);
            
            mDoc.AutoCompleteAccepted += new EventHandler<AutoCompleteAcceptedEventArgs>(OnAutoCompleteAccepted);
            mDoc.GotFocus += new EventHandler(OnGotFormFocus);          
            mDoc.TextChanged += new EventHandler(OnTextChange);
            mDoc.Scroll += new EventHandler<ScrollEventArgs>(OnDocScroll);
            Activated += new EventHandler(OnDocActivated);
            mDoc.AutoComplete.FillUpCharacters = "";
            mDoc.KeyPress += new KeyPressEventHandler(OnKeyPress);

            mSaved = true;
            mArgControl = new ArgumentsControl();
            mDescriptor = new ClassDescriptor();
            mDescriptor.listView.MouseEnter += new EventHandler(OnDescriptorMouseEnter);
            mDescriptor.listView.MouseLeave += new EventHandler(OnDescriptorMouseLeave);
            mDescriptor.listView.LostFocus += new EventHandler(OnDescriptorLostFocus);

            mDoc.Controls.Add(mArgControl);
            mDoc.Controls.Add(mDescriptor);
            mArgControl.Visible = false;
            mDescriptor.Visible = false;

            //List<MenuItem> items = new List<MenuItem>();
            //foreach (MenuItem item in contextMenuStrip.Items)
            //    items.Add(item);

            TabPageContextMenuStrip = contextMenuStrip;

        }

        /// <summary>
        /// Resume the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDescriptorLostFocus(object sender, EventArgs e)
        {
            mTimer.Start();
        }

        /// <summary>
        /// Resume the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDescriptorMouseLeave(object sender, EventArgs e)
        {
            if (mDescriptor.listView.Focused)
                return;

            mTimer.Start();
        }

        /// <summary>
        /// Pause the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDescriptorMouseEnter(object sender, EventArgs e)
        {
            mTimer.Stop();
        }

        /// <summary>
        /// Called when the timer is up. We check what word we are over and see if there is any info of a class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTimer(object sender, ElapsedEventArgs e)
        {
            if (mDoc.IsDisposed)
            {
                mTimer.Elapsed -= new ElapsedEventHandler(OnTimer);
                mTimer.Stop();
                mTimer = null;
                return;
            }

            // If the text has changed we call an invoke to re-update it in a thread safe way
            if (this.InvokeRequired)
            {
                TimerCalled d = new TimerCalled(OnTimer);
                this.Invoke(d, new object[]{ sender, e } );
                return;
            }

          

            System.Drawing.Point local = mDoc.PointToClient(new System.Drawing.Point(Cursor.Position.X, Cursor.Position.Y));
            int position = mDoc.PositionFromPoint(local.X, local.Y);
            string word = mDoc.GetWordFromPosition(position);

            if (word != null && word != "" && word != mLastHoverWord && mArgControl.Visible == false )
            {

                if (mKeywordInfo.ContainsKey(word.Trim()))
                {
                    Token t = mKeywordInfo[word.Trim()];

                    if (t.type != "class")
                        return;

                    mDescriptor.ImageList = this.imageList;
                    mDescriptor.Visible = true;
                    mDescriptor.FillData(t);


                    mDescriptor.Location = new System.Drawing.Point(
                        mDoc.PointXFromPosition(position),
                        mDoc.PointYFromPosition(position) - mDescriptor.Size.Height - 5);

                    if (mDescriptor.Location.Y < 0)
                        mDescriptor.Location = new System.Drawing.Point(mDescriptor.Location.X, 0);

                    if (mDescriptor.Location.X + mDescriptor.Size.Width > Size.Width)
                        mDescriptor.Location = new System.Drawing.Point(Size.Width - mDescriptor.Size.Width, mDescriptor.Location.Y);
                }
                else
                    mDescriptor.Visible = false;
            }
            else
                mDescriptor.Visible = false;

           
        }

        /// <summary>
        /// Reset the timer when the mouse moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            mTimer.Elapsed -= new ElapsedEventHandler(OnTimer);
            mTimer.Stop();
            mTimer = new System.Timers.Timer(1500);
            mTimer.Elapsed += new ElapsedEventHandler(OnTimer);
            mTimer.AutoReset = true;
            mTimer.Start();           
        }

        void mDoc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {
                try
                {
                    this.ShowSuggestion(mDoc.AutoComplete.SelectedText);
                }
                catch { }
            }

            if (e.KeyCode == Keys.OemPeriod)
                this.CreateIntellisenseKeywords();
        }

        void OnDocumentClick(object sender, EventArgs e)
        {
            this.OnGotFormFocus(this, null);
        }

        /// <summary>
        /// When we press a key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            mPerformKeyUpChecks = true;

            string styleName = Scintilla.Styles.GetStyleNameAt(Scintilla.CurrentPos);

            //Get the next line of text
            Line line = Scintilla.Lines.FromPosition( Scintilla.CurrentPos );
            line = line.Next;

            //Now see if the next line is a function. If it is, then get its parameters
            if (line != null)
            {         
                if ( Scintilla.Text.Length > 3 && e.KeyChar == '*' && Scintilla.Text[Scintilla.CurrentPos - 1] == '*' &&
                    Scintilla.Text[Scintilla.CurrentPos - 2] == '/' )
                {
                    ////The line below is a function or variable - show the comment context
                    //if (GetFunctionMatched(line.Text, true, true).Count > 0 ||
                    //    GetVariableMatched(line.Text).Count > 0)
                    //{
                        e.Handled = true;

                        contextComment.Show(
                            mDoc.PointToScreen(new System.Drawing.Point(
                            mDoc.PointXFromPosition(mDoc.CurrentPos), mDoc.PointYFromPosition(mDoc.CurrentPos))));
                        contextComment.Focus();
                        return;
                    //}

            //        string function = line.Text;
            //        string[] parts = function.Split('(');


            //        if (parts.Length > 1)
            //        {
            //            parts = parts[1].Split(')');
            //            parts = parts[0].Split(',');

            //            line = Scintilla.Lines.FromPosition(Scintilla.CurrentPos);
            //            string tabStr = "";
            //            foreach (char c in line.Text)
            //            {
            //                if (c == '\t')
            //                    tabStr += "\t";
            //                else
            //                    break;
            //            }

            //            int descriptionIndex = Scintilla.CurrentPos;
            //            e.Handled = true;
            //            string newText = "";
            //            newText = "*\r\n";
            //            newText += tabStr + "* @description \r\n";

            //            descriptionIndex += newText.Length - 2;

            //            foreach (string paramName in parts)
            //                if (paramName.Trim() != "")
            //                    newText += tabStr + "* @param " + paramName.Trim() + " \r\n";

            //            newText += tabStr + "*/";


            //            Scintilla.InsertText(Scintilla.CurrentPos, newText);

            //            mDoc.CurrentPos = descriptionIndex;
            //        }
                }
            }
           
            if (Luminate.Singleton.Preferences.autoBraceClosure)
            {
                if ( Scintilla.CurrentPos < Scintilla.Text.Length )
                    if (Scintilla.Text[Scintilla.CurrentPos ] != ' ' &&
                        Scintilla.Text[Scintilla.CurrentPos ] != '\t' &&
                        Scintilla.Text[Scintilla.CurrentPos] != '\r' &&
                        Scintilla.Text[Scintilla.CurrentPos ] != '\n')
                    {
                        return;
                    }

                //If we use '[' we close it
                if (e.KeyChar == '[')
                {
                   this.Scintilla.InsertText(this.Scintilla.CurrentPos, "]");
                    return;
                }

                //If we use '(' we close it
                if (e.KeyChar == '(')
                {
                    this.Scintilla.InsertText(this.Scintilla.CurrentPos, ")");
                    return;
                }

                //If we use '"' we close it
                if ( styleName != "STRINGEOL" && e.KeyChar == '"')
                {
                    this.Scintilla.InsertText(this.Scintilla.CurrentPos, "\"");
                    return;
                }

                if (styleName != "STRINGEOL" && e.KeyChar == '\'')
                {
                    this.Scintilla.InsertText(this.Scintilla.CurrentPos, "'");
                    return;
                }
            }

            
        }

        void OnDocScroll(object sender, ScrollEventArgs e)
        {
            mDescriptor.Visible = false;

            mArgControl.Location = new System.Drawing.Point(
                 mDoc.PointXFromPosition(mDoc.CurrentPos),
                 mDoc.PointYFromPosition(mDoc.CurrentPos) - mArgControl.Size.Height - 5);

            if (mArgControl.Location.Y < 0)
                mArgControl.Location = new System.Drawing.Point(mArgControl.Location.X, 0);

            if (mArgControl.Location.X + mArgControl.Size.Width > Size.Width)
                mArgControl.Location = new System.Drawing.Point(Size.Width - mArgControl.Size.Width, mArgControl.Location.Y);

            mArgControl.Invalidate();
        }

        /// <summary>
        /// When the doc gets focus we re-build the intellisense.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGotFormFocus(object sender, EventArgs e)
        {
            CreateIntellisenseKeywords();
        }

        /// <summary>
        /// When auto complete is accepted we make sure there is no funny behaviour with more than 1 period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAutoCompleteAccepted(object sender, AutoCompleteAcceptedEventArgs e)
        {
            if (mOEMPeriodPushed)
            {
                e.Cancel = true;
                return;
            }

            if (mDoc.CurrentPos > 2 && mDoc.Text[mDoc.CurrentPos - 1] == '.' && mOEMPeriodPushed)
                e.Cancel = true;
            else
            {
                e.Cancel = false;
                mSuggestWord = e.Text + "?0";
                ShowSuggestion(e.Text);
            }
        }

        /// <summary>
        /// Check for any external updates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnGotFocus(object sender, EventArgs e)
        {
            UpdateContents();
        }

        /// <summary>
        /// Get this doc to update its elements
        /// </summary>
        private void UpdateContents(bool showMessage = true )
        {
            if (mUpdateOnActive)
            {
                mUpdateOnActive = false;
                if ( showMessage == true && MessageBox.Show("The file " + file + " has been modified outside of this application. Would you like to update the contents?",
                        "File modified", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.No)
                    return;

                string fileContent = null;

                //Check if its an image
                string lowerCase = file.ToLower();
                if (lowerCase.Contains(".jpg") || lowerCase.Contains(".jpeg") || lowerCase.Contains(".png") || lowerCase.Contains(".bmp") || lowerCase.Contains(".gif"))
                {
                    // If the image has changed we call an invoke to re-update it in a thread safe way
                    if (mPicBox.InvokeRequired)
                        mPicBox.Invoke(new ImageUpdate(UpdateImage));
                    else
                        UpdateImage();
                }
                else
                {
                    try
                    {
                        // Create an instance of StreamReader to read from a file.
                        // The using statement also closes the StreamReader.
                        using (StreamReader sr = new StreamReader(file))
                            fileContent = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
                        fileContent = null;
                    }

                    if (fileContent != null)
                        // If the text has changed we call an invoke to re-update it in a thread safe way
                        if (mDoc.InvokeRequired)
                        {
                            TextUpdate d = new TextUpdate(UpdateText);
                            //mDoc.Invoke(d, new object[] { fileContent });
                        }
                        else
                            UpdateText(fileContent);
                }
            }
        }


        /// <summary>
        /// Turn the flag on or off to update the doc
        /// </summary>
        public bool UpdateOnActive
        {
            get { return mUpdateOnActive; }
            set { mUpdateOnActive = value; }
        }


        /// <summary>
        /// Called when the image has updated outside the app and we need a thread safe way of updating the contents
        /// </summary>
        private void UpdateImage()
        {
            try
            {
                mPicBox.ImageLocation = Tag.ToString();
                mPicBox.Load();
            }
            catch (Exception ex)
            {
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
            }
        }

        /// <summary>
        ///  Called when the text has updated outside the app and we need a thread safe way of updating the contents
        /// </summary>
        /// <param name="content"></param>
        private void UpdateText( string content )
        {
            mDoc.Text = content;
        }

        /// <summary>
        /// When the doc is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDocActivated(object sender, EventArgs e)
        {
            //Set the current document
            FindReplace.get.document = this;

            if (mUpdateOnActive)
                UpdateContents();

            CreateIntellisenseKeywords();

            OnMouseMove(null);
        }
        
        /// <summary>
        /// Reset all highlights
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClick(object sender, EventArgs e)
        {
            HideSuggestion();

            if (mPicBox != null)
                return;

            mDoc.FindReplace.ClearAllHighlights();
        }

        /// <summary>
        /// Find all highlights
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDocDoubleClick(object sender, EventArgs e)
        {
            if (mPicBox != null)
                return;

            mDoc.FindReplace.ClearAllHighlights();

            if (mDoc.Selection.Text != null && mDoc.Selection.Text != "")
            {
                mDoc.FindReplace.HighlightAll(mDoc.FindReplace.FindAll(mDoc.Selection.Text, SearchFlags.WholeWord));

                ShowSuggestion(mDoc.Selection.Text);
            }
        }

        /// <summary>
        /// Calculate the margin width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTextChanged(object sender, EventArgs e)
        {
            if (canSave == false)
                return;

            mDoc.Margins[0].Width = mDoc.Lines.Count.ToString().Length * 10;
        }

        /// <summary>
        /// This is a small fix to make sure the doc is saved before it closes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.MdiFormClosing)
            {
                e.Cancel = true;
                return;
            }

            base.OnFormClosing(e);
        }

        /// <summary>
        /// When the doc is closing remove the reference
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            
            if (forceRemove == false && Saved == false)
            {
                 DialogResult message = MessageBox.Show(Text + " has not been saved, do you want to save it before closing?", "Save file", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                 if ( message == System.Windows.Forms.DialogResult.Cancel )
                 {
                     e.Cancel = true;
                     return;
                 }
                 else if ( message == System.Windows.Forms.DialogResult.Yes )
                     Save();
            }

            //Cleanup listeners
            mDoc.TextChanged -= new EventHandler(OnTextChange);
            mDoc.KeyDown -= new KeyEventHandler(OnKeyDown);
            mDoc.TextChanged -= new EventHandler(OnTextChanged);
            mDoc.ContextMenuStrip = contextMenuStrip;
            mDoc.DoubleClick -= new EventHandler(OnDocDoubleClick);
            mDoc.Click -= new EventHandler(OnClick);
            mDoc.FindReplace.Window.Close();

            Luminate.Singleton.RemoveFile( file, true );
        }

        /// <summary>
        /// When the text changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnTextChange(object sender, EventArgs e)
        {
            if (canSave == false)
                return;

            if (mDoc.UndoRedo.CanUndo)
            {
                mSaved = false;

                string[] split = file.Split('\\');
                Text = "*" + split[split.Length - 1];
            }
            else
            {
                mSaved = true;

                string[] split = file.Split('\\');
                Text = split[split.Length - 1];
            }
        }

        /// <summary>
        /// Get or set the doc text
        /// </summary>
        public string DocText
        {
            get 
            {
                if (mDoc != null)
                    return mDoc.Text;
                else
                    return base.Text;
            }
            set { mDoc.Text = value; }
        }

        /// <summary>
        /// Get if the doc is saved or not
        /// </summary>
        public bool Saved { get { return mSaved; } }

        /// <summary>
        /// Saves the text contents into the file
        /// </summary>
        public void Save()
        {
            if (canSave == false)
                return;

            bool listenBefore = Luminate.Singleton.listenForChanges;
            Luminate.Singleton.listenForChanges = false;

            try
            {
                if (mPicBox != null)
                {
                    mSaved = true;
                    string[] split = file.Split('\\');
                    Text = split[split.Length - 1];
                    return;
                }

                //Write to file
                using (StreamWriter outfile = new StreamWriter(file))
                {
                    outfile.Write(DocText);
                }

                mSaved = true;
                string[] split2 = file.Split('\\');
                Text = split2[split2.Length - 1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.Singleton.Log(ex.Message, Logger.MessageType.ERROR);
            }

            Luminate.Singleton.listenForChanges = listenBefore;

            Luminate.Singleton.CallBuild();
        }

        /// <summary>
        /// Shows the function parameter definitions
        /// </summary>
        private void ShowSuggestion( string suggestion )
        {
            //if (!mFunctionParams.ContainsKey(mSuggestWord))
            //    return;

            //string text = "function " + mSuggestWord.Split('?')[0] + "( ";
            //foreach (string s in mFunctionParams[mSuggestWord])
            //    text += s + ", ";

            //if (mFunctionParams[mSuggestWord].Length > 0)
            //    text = text.Substring(0, text.Length - 2);

            //text += " )";

            suggestion = suggestion.Split('?')[0].Trim();

            if (mKeywordInfo.ContainsKey(suggestion) )
            {
                mArgControl.Visible = true;
                mDescriptor.Visible = false;
                 mArgControl.FillData( mKeywordInfo[suggestion] );
                

                mArgControl.Location = new System.Drawing.Point(
                    mDoc.PointXFromPosition(mDoc.CurrentPos),
                    mDoc.PointYFromPosition(mDoc.CurrentPos) - mArgControl.Size.Height - 5);

                if (mArgControl.Location.Y < 0)
                    mArgControl.Location = new System.Drawing.Point(mArgControl.Location.X, 0);

                if (mArgControl.Location.X + mArgControl.Size.Width > Size.Width)
                    mArgControl.Location = new System.Drawing.Point(Size.Width - mArgControl.Size.Width, mArgControl.Location.Y);
            }
            else
                mArgControl.Visible = false;
        }
        
        /// <summary>
        /// Hides the function parameter definitions
        /// </summary>
        private void HideSuggestion()
        {
            mArgControl.Visible = false;
            mDescriptor.Visible = false;
        }

        private bool DoesLineHaveWords( int line )
        {
            if (Scintilla.Lines.Count > line)
            {
                Line l = Scintilla.Lines[line];
                string test = l.Text.Trim();
                if (test == "")
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// When the key is up on this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (!mPerformKeyUpChecks)
                return;

            if (mPicBox != null)
                return;

            //Show the find dialogue
            
            if (e.KeyCode == Keys.D && e.Shift && e.Control)
            {
                e.Handled = true;
                mDoc.Commands.Execute(BindableCommand.LineDuplicate);
                return;
            }

            if (e.Control && e.KeyCode == Keys.Space)
            {
                mDoc.AutoComplete.Show(0, mKeywordsToAdd);
                ShowSuggestion(mDoc.AutoComplete.SelectedText);
            }
            
            //If we are in a doc comment and hit enter, then it ceates a new line with a helpful '*' formatter.
            string styleName = Scintilla.Styles.GetStyleNameAt(Scintilla.CurrentPos);
            if ( styleName == "COMMENTDOC" && e.KeyCode == Keys.Enter )
            {
                Scintilla.InsertText(Scintilla.CurrentPos, "* ");
                Scintilla.Commands.Execute(BindableCommand.CharRight);
                Scintilla.Commands.Execute(BindableCommand.CharRight);

            }

            //If we are in a comment then we do not show the auto complete
            if (styleName == "COMMENTDOC" || styleName == "COMMENTLINEDOC")
                return;

            //if (Luminate.Singleton.Preferences.autoBraceClosure)
            //{
                //If we hit enter and the previous key was a {. Then we create a function closure.
                if (e.KeyCode == Keys.Return)
                {
                    int prevLineNumber = Scintilla.Lines.FromPosition(Scintilla.CurrentPos).Number - 1;
                    if (Scintilla.Lines.Count > prevLineNumber)
                    {
                        string prevLine = Scintilla.Lines[prevLineNumber].Text.Trim();

                        if (prevLine.Length > 0 && prevLine[prevLine.Length - 1] == '{' && !DoesLineHaveWords(Scintilla.Lines.FromPosition(Scintilla.CurrentPos).Number + 1))
                        {
                            this.Scintilla.Commands.Execute(BindableCommand.NewLine);
                            this.Scintilla.Commands.Execute(BindableCommand.BackTab);
                            this.Scintilla.InsertText(Scintilla.CurrentPos, "}");
                            this.Scintilla.Commands.Execute(BindableCommand.LineUp);
                            this.Scintilla.Commands.Execute(BindableCommand.LineEnd);

                            return;
                        }
                    }
                }
            //}
            
            if ( e.KeyCode == Keys.Escape || (e.KeyCode == Keys.D0 && e.Shift) || e.KeyCode == Keys.OemSemicolon )
                HideSuggestion();

            //Call save ctrl + s
            if (e.Control && e.KeyCode == Keys.S)
            {
                Save();
                return;
            }

            //If any of the keys below - we let scintilla act as normal
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Escape )
                return;

            //Check for auto complete
            SolutionPreferences prefs = Luminate.Singleton.Preferences;
            if (prefs.useAutoComplete && e.Control == false &&
                (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Right || e.KeyCode == Keys.Left) == false)
            {
                    string word = mDoc.GetWordFromPosition(mDoc.CurrentPos);

                    //We dont want the next letter to be filled
                    char nextLetter = ' ';
                    char pevLetter = ' ';

                    if (mDoc.Text.Length > mDoc.CurrentPos)
                        nextLetter = mDoc.Text[mDoc.CurrentPos];

                    if (mDoc.CurrentPos - 2 > 0 && mDoc.CurrentPos - 2 < mDoc.Text.Length)
                        pevLetter = mDoc.Text[mDoc.CurrentPos - 2];

                    //If the next or previous character is not blank - then show the auto complete
                    if (word != "" && ( nextLetter == ' ' || nextLetter == '\r' || nextLetter == '\n' || nextLetter == '\t' ) &&
                        (pevLetter == ' ' || pevLetter == '\r' || pevLetter == '\n' || pevLetter == '\t'))
                    {
                        foreach (string s in mKeywordsToAdd)
                            if (s.Length >= word.Length && s.Substring(0, word.Length).ToLower() == word.ToLower())
                            {
                              mDoc.AutoComplete.Show(0, mKeywordsToAdd);
                               ShowSuggestion(mDoc.AutoComplete.SelectedText);
                                return;
                            }

                        mDoc.AutoComplete.Cancel();
                        return;
                    }
            }

            //If we hit . then we show the auto compelte
            if (prefs.useAutoComplete && e.KeyCode == Keys.OemPeriod)
            {
                CreateIntellisenseKeywords();

                mDoc.AutoComplete.Show(0, mKeywordsToAdd);
                if ( mDoc.AutoComplete.SelectedIndex != -1 )
                    ShowSuggestion(mDoc.AutoComplete.SelectedText);
            }

            //Call Run ctrl + f5
            if (e.KeyCode == Keys.F5)
                Luminate.Singleton.CallRun();
            //Call Run ctrl + f5
            else if (e.KeyCode == Keys.F6)
                Luminate.Singleton.CallBuild();

           
              
        }

        /// <summary>
        /// Checks if a selection ends in px
        /// </summary>
        /// <returns></returns>
        private bool EndsInPx()
        {
            if (mDoc.Selection.Text.Length > 2 && mDoc.Selection.Text[mDoc.Selection.Text.Length - 1] == 'x' &&
               mDoc.Selection.Text[mDoc.Selection.Text.Length - 2] == 'p')
                return true;

            return false;

        }

        /// <summary>
        /// Creates the intellisense words
        /// </summary>
        private void CreateIntellisenseKeywords()
        {
            //First lets figure out the below...
            bool isThisCmd = false;


            if (mDoc.CurrentPos > 5 && mDoc.Text.Length > 5 && mDoc.CurrentPos - 5 < mDoc.Text.Length && mDoc.CurrentPos < 5)
            {
                string lastBitOfText = mDoc.Text.Substring(mDoc.CurrentPos - 5, 5);
                if (lastBitOfText.Trim() == "this." || lastBitOfText.Trim() == "this")
                    isThisCmd = true;
            }

            mFunctionParams.Clear();

            SolutionPreferences prefs = Luminate.Singleton.Preferences;
            List<string> keyWords = new List<string>();
           
            keyWords.AddRange(prefs.additionalKeywords.ToArray());
            
           


            //Add icons for custom
            for (int i = 0; i < keyWords.Count; i++)
                if ( keyWords[i] != "" )
                    keyWords[i] += "?1";

            if (isThisCmd == false)
                foreach (string s in mKeywords )
                    keyWords.Add(s + "?5");

            //Get the variables
            List<Match> vars = GetVariableMatched(mDoc.Text);
            foreach (Match m in vars)
            {
                string name = m.Value.ToString();
                name = name.Replace("this.", "").Replace(".", "").Replace(" ", "").Replace("=", "").Replace("var", "");
               
                if ( keyWords.Contains( name + "?3" ) == false )
                    keyWords.Add(name + "?3");
            }

            string className = GetClassFromPosition();
            mKeywordInfo.Clear();

            //Fill intellisense based on the class we are in
            foreach (Token t in Luminate.Singleton.lexer.tokens)
            {
                if (t.type == "class")
                    keyWords.Add(t.name.Trim() + "?2");
                else if (t.type == "sfunc")
                    keyWords.Add(t.name.Trim() + "?0");
                else
                    keyWords.Add(t.name.Trim() + "?3");

                if (mKeywordInfo.ContainsKey(t.name.Trim()) == false)
                    mKeywordInfo.Add( t.name.Trim(), t );

                if (t.type == "class" && t.name.Trim() == className.Trim())
                {
                    if ( isThisCmd )
                        foreach (KeyValuePair<string, Token> func in t.mFunctions)
                        {
                            keyWords.Add(func.Key.Trim() + "?0");
                            if (mKeywordInfo.ContainsKey(func.Key) == false)
                                mKeywordInfo.Add(func.Key.Trim(), func.Value);
                        }
                   // if (isThisCmd == false)
                        foreach (KeyValuePair<string, Token> func in t.sFunctions)
                        {
                            keyWords.Add(func.Key.Trim() + "?0");
                            if (mKeywordInfo.ContainsKey(func.Key) == false)
                                mKeywordInfo.Add(func.Key.Trim(), func.Value);
                        }
                    if (isThisCmd )
                        foreach (KeyValuePair<string, Token> var in t.mVariables)
                        {
                            keyWords.Add(var.Key.Trim() + "?3");
                            if (mKeywordInfo.ContainsKey(var.Key) == false)
                                mKeywordInfo.Add(var.Key.Trim(), var.Value);
                        }
                  //  if (isThisCmd == false)
                        foreach (KeyValuePair<string, Token> var in t.sVariables)
                        {
                            keyWords.Add(var.Key.Trim() + "?3");
                            if (mKeywordInfo.ContainsKey(var.Key) == false)
                                mKeywordInfo.Add(var.Key.Trim(), var.Value);
                        }
                 //   if ( isThisCmd == false )
                        
                }

                //Add the enums
                if (isThisCmd == false)
                    foreach (KeyValuePair<string, Token> en in t.enums)
                    {
                        keyWords.Add(t.name + "." + en.Key.Trim() + "?4");
                        if (mKeywordInfo.ContainsKey(en.Key) == false)
                            mKeywordInfo.Add(t.name + "." + en.Key.Trim(), en.Value);
                    }
            }

            keyWords.Sort();
            mKeywordsToAdd = keyWords.ToArray();

            //Try and figure out what function you are in
            //string functionName = GetClassFromPosition();
        }

        /// <summary>
        /// Use this function to identify if a string has a function
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private MatchCollection GetFunctionMatched(string text, bool matchGlobal, bool matchLocal)
        {

            MatchCollection functionMatches = null;
            
            if ( matchGlobal )
                functionMatches = Regex.Matches(text, @"function(\s+)([a-zA-Z0-9]+)(\s*)\(", RegexOptions.IgnoreCase);

            if ((matchLocal && matchGlobal && functionMatches.Count == 0) || (matchLocal && !matchGlobal) )
                functionMatches = Regex.Matches(text, @"\.\S([a-zA-Z0-9]+)\s=\sfunction\(", RegexOptions.IgnoreCase);

            return functionMatches;
        }

        
        /// <summary>
        /// Use this function to identify if a string has a variable
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private List<Match> GetVariableMatched(string text)
        {
            List<Match> matches = new List<Match>();

            MatchCollection variableMatches = Regex.Matches(text, @"this.(\w*)\s=", RegexOptions.IgnoreCase);
            foreach (Match m in variableMatches)
                matches.Add(m);


            variableMatches = Regex.Matches(text, @".(\w*)\s=", RegexOptions.IgnoreCase);
            foreach (Match m in variableMatches)
                matches.Add(m);

            variableMatches = Regex.Matches(text, @"var(\s*)([a-zA-Z0-9]+)(\s*)=", RegexOptions.IgnoreCase);
            foreach (Match m in variableMatches)
                matches.Add(m);

            return matches;
        }

        /// <summary>
        /// When the key is up on this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (mPicBox != null)
                return;

            //Show the find dialogue
            if (e.KeyCode == Keys.F && e.Control)
            {
                ShowFind();
                return;
            }
            
            CreateIntellisenseKeywords();
           

            if (e.Shift && e.KeyCode == Keys.Up)
                return;

            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && EndsInPx())
            {
                string value = mDoc.Selection.Text.Remove(mDoc.Selection.Text.IndexOf("px"), 2).Trim();
                try
                {
                    float num = float.Parse(value);

                    if (((e.KeyCode & Keys.Up) == Keys.Up))
                        num++;
                    else if (((e.KeyCode & Keys.Down) == Keys.Down))
                        num--;


                    int start = mDoc.Selection.Start;
                    int length = mDoc.Selection.Length;
                    mDoc.Text = mDoc.Text.Remove(start, length);
                    mDoc.Text = mDoc.Text.Insert(start, num.ToString() + "px");
                    mDoc.Selection.Start = start;
                    mDoc.Selection.Length = (num.ToString() + "px").Length;
                    mDoc.Scrolling.ScrollToCaret();
                }
                catch
                {
                }
            }

            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down && EndsInPx() == false && e.Shift == false)
            {
                if (e.KeyCode == Keys.Up)
                    mDoc.Commands.Execute(BindableCommand.LineUp);
                else if (e.KeyCode == Keys.Down)
                    mDoc.Commands.Execute(BindableCommand.LineDown);
                return;
            }

            if (e.KeyCode == Keys.OemPeriod)
                mOEMPeriodPushed = true;
            else
                mOEMPeriodPushed = false;

            if (e.KeyCode == Keys.Up && e.Control)
                LineUp();
            else if (e.KeyCode == Keys.Down && e.Control)
                LineDown();
        }

        public void LineUp()
        {
            mDoc.Indentation.TabIndents = false;
            mDoc.Indentation.SmartIndentType = SmartIndent.None;
            mDoc.Commands.Execute(BindableCommand.Cut);
            mDoc.Commands.Execute(BindableCommand.LineUp);
            mDoc.Commands.Execute(BindableCommand.Home);
            mDoc.Commands.Execute(BindableCommand.Paste);
            mDoc.Commands.Execute(BindableCommand.NewLine);
            mDoc.Commands.Execute(BindableCommand.LineEnd);
            mDoc.Commands.Execute(BindableCommand.LineDown);
            mDoc.Commands.Execute(BindableCommand.LineDelete);
            mDoc.Commands.Execute(BindableCommand.LineUp);
            mDoc.Commands.Execute(BindableCommand.LineUp);
            Line l = mDoc.Lines.FromPosition(mDoc.CurrentPos);
            mDoc.Selection.Start = l.StartPosition;
            mDoc.Selection.End = l.EndPosition;
            mDoc.Indentation.SmartIndentType = SmartIndent.CPP;
            mDoc.Indentation.TabIndents = true;
        }

        public void LineDown()
        {
            mDoc.Indentation.TabIndents = false;
            mDoc.Indentation.SmartIndentType = SmartIndent.None;

            mDoc.Commands.Execute(BindableCommand.LineDown);
            System.Windows.Forms.Clipboard.Clear();

            Line l = mDoc.Lines.FromPosition(mDoc.CurrentPos);
            mDoc.Selection.Start = l.StartPosition;
            mDoc.Selection.End = l.EndPosition;

            mDoc.Commands.Execute(BindableCommand.Cut);
            mDoc.Commands.Execute(BindableCommand.LineUp);
            mDoc.Commands.Execute(BindableCommand.Home);
            mDoc.Commands.Execute(BindableCommand.Paste);
            mDoc.Commands.Execute(BindableCommand.NewLine);
            mDoc.Commands.Execute(BindableCommand.LineDown);
            mDoc.Commands.Execute(BindableCommand.LineDelete);
            mDoc.Commands.Execute(BindableCommand.LineUp);
            l = mDoc.Lines.FromPosition(mDoc.CurrentPos);
            mDoc.Selection.Start = l.StartPosition;
            mDoc.Selection.End = l.EndPosition;

            mDoc.Indentation.SmartIndentType = SmartIndent.CPP;
            mDoc.Indentation.TabIndents = true;
        }

        /// <summary>
        /// Get the scintilla to do a command
        /// </summary>
        /// <param name="command"></param>
        public void DoCommand( BindableCommand command )
        {
            mDoc.Commands.Execute(command);
        }

        /// <summary>
        /// Gets the scintilla object
        /// </summary>
        public Scintilla Scintilla { get { return mDoc; } }

        /// <summary>
        /// Shortcut menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyClick(object sender, EventArgs e)
        {
            Luminate.Singleton.copyToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// Shortcut menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCutClick(object sender, EventArgs e)
        {
            Luminate.Singleton.cutToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// Shortcut menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPasteClick(object sender, EventArgs e)
        {
            Luminate.Singleton.pasteToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// Shortcut menu click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSaveClick(object sender, EventArgs e)
        {
            Luminate.Singleton.saveToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseClick(object sender, EventArgs e)
        {
            Luminate.Singleton.RemoveFile(file);
        }

        /// <summary>
        /// Removes all the files from the panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseAllClick(object sender, EventArgs e)
        {
            Luminate.Singleton.RemoveAll("");
        }

        /// <summary>
        /// When we click remove all
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveAllButClick(object sender, EventArgs e)
        {
            Luminate.Singleton.RemoveAll(file);
        }

        private void OnContextOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mPicBox != null)
            {
                e.Cancel = true;
                return;
            }

            wordWrapToolStripMenuItem.Checked = (mDoc.LineWrapping.Mode == LineWrappingMode.Word ? true : false);
        }

        /// <summary>
        /// Click word wrap on / off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWordWrapClick(object sender, EventArgs e)
        {
            mDoc.LineWrapping.Mode = (wordWrapToolStripMenuItem.Checked ? LineWrappingMode.Word : LineWrappingMode.None);
        }

        /// <summary>
        /// Show the find doc
        /// </summary>
        public void ShowFind()
        {
            FindReplace.get.document = this;
            FindReplace.get.Show();
            FindReplace.get.textBoxFind.Text = mDoc.Selection.Text;
            
            
            FindReplace.get.textBoxFind.SelectAll();
            FindReplace.get.textBoxFind.Focus();
        }

        /// <summary>
        /// This is saved in the dock panel export
        /// </summary>
        /// <returns></returns>
        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return file + "|||DOCKSPLIT|||";
        }

        /// <summary>
        /// This will try and determine the name of a JS class from the position of the scintilla cursor.
        /// </summary>
        /// <returns></returns>
        private string GetClassFromPosition()
        {
            string text = Scintilla.Text;//.Substring(0, mDoc.CurrentPos);
            
            MatchCollection matches = Regex.Matches(text, @"(public|private)(\s*)class(\s*)(\w*)+");
            
            foreach ( Match m in matches )
                if (m.Success)
                {
                    int len = text.Length;
                    string functionText = text.Substring(m.Index, m.Length);

                    string[] parts = functionText.Split(' ');
                    if (parts.Length > 2)
                    {
                        return parts[parts.Length - 1];
                    }
                }

            return "";
        }

        /// <summary>
        /// This function is used to create the comment based on the decision that was made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommentItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //Get the next line of text
            Line line = Scintilla.Lines.FromPosition( Scintilla.CurrentPos );
            line = line.Next;

            string className = GetClassFromPosition();

            //Now see if the next line is a function. If it is, then get its parameters
            if (line != null)
            {
                //Get the number of tabs
                Line currentLine = Scintilla.Lines.FromPosition(Scintilla.CurrentPos);
                string tabStr = "";
                foreach (char c in currentLine.Text)
                {
                    if (c == '\t')
                        tabStr += "\t";
                    else
                        break;
                }

                //If a class / function
                if (e.ClickedItem == commentClass ||
                    e.ClickedItem == commentMemberFunction || e.ClickedItem == commentStaticFunction)
                {
                    MatchCollection functionMatches = GetFunctionMatched(line.Text, true, true);

                    //Is the line a function?
                    if (functionMatches.Count > 0)
                    {
                        string function = line.Text;
                        string[] parts = function.Split('(');

                        string functionName = functionMatches[0].ToString().Replace("function", "").Replace("(", "").Replace(".", "").Replace("=", "").Trim();

                        if (parts.Length > 1)
                        {
                            int descriptionIndex = Scintilla.CurrentPos;
                            string newText = "";
                            newText = "*\r\n";
                           
                            //define the type first
                            if (e.ClickedItem == commentClass)
                                newText += tabStr + "* @type public class " + functionName + "\r\n";
                            else if (e.ClickedItem == commentMemberFunction)
                                newText += tabStr + "* @type public mfunc " + functionName + "\r\n";
                            else if (e.ClickedItem == commentStaticFunction)
                                newText += tabStr + "* @type public sfunc " + functionName + "\r\n";

                            newText += tabStr + "* @description \r\n";
                            descriptionIndex += newText.Length - 2;

                            //If a class
                            if (e.ClickedItem == commentClass)
                            {
                                newText += tabStr + "* @created " + DateTime.Now.Date.ToString("MM/dd/yyyy HH:mm") + "\r\n";
                                newText += tabStr + "* @html \r\n";
                                newText += tabStr + "* @author \r\n";
                            }
                           
                            parts = parts[1].Split(')');
                            parts = parts[0].Split(',');

                            foreach (string paramName in parts)
                                if (paramName.Trim() != "")
                                    newText += tabStr + "* @param <object> " + paramName.Trim() + " \r\n";


                            if (e.ClickedItem == commentClass)
                                newText += tabStr + "* @extends <object>\r\n";
                            else
                                newText += tabStr + "* @extends <" + (className != "" ? className : "object" ) + ">\r\n";

                            if (e.ClickedItem != commentClass)
                                newText += tabStr + "* @returns <object> \r\n";

                           
                            newText += tabStr + "*/";
                            Scintilla.InsertText(Scintilla.CurrentPos, newText);
                            mDoc.CurrentPos = descriptionIndex;
                            mPerformKeyUpChecks = false;
                        }
                    }
                }
                else
                {
                    List<Match> variableMatches = GetVariableMatched(line.Text);

                    //Is the line a function?
                    if (variableMatches.Count > 0)
                    {
                        string variable = variableMatches[0].ToString();
                        var variableName = variable.Replace(".", "").Replace("=", "").Trim();

                        int descriptionIndex = Scintilla.CurrentPos;
                        string newText = "";
                        newText = "*\r\n";
                       

                        //If a class
                        if (e.ClickedItem == commentMemberVariable)
                            newText += tabStr + "* @type public mvar " + variableName + "\r\n";
                        if (e.ClickedItem == commentStaticVariable)
                            newText += tabStr + "* @type public svar " + variableName + "\r\n";
                        if (e.ClickedItem == commentEnum)
                            newText += tabStr + "* @type public enum " + variableName + "\r\n";

                        newText += tabStr + "* @description \r\n";
                        descriptionIndex += newText.Length - 2;

                        newText += tabStr + "* @extends <" + (className != "" ? className : "object") + ">\r\n";
                        newText += tabStr + "*/";

                        Scintilla.InsertText(Scintilla.CurrentPos, newText);
                        mDoc.CurrentPos = descriptionIndex;
                        mPerformKeyUpChecks = false;
                    }
                }
            }
        }

        /// <summary>
        /// Show or hide the comment options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCommentContextOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Get the next line of text
            Line line = Scintilla.Lines.FromPosition( Scintilla.CurrentPos );
            line = line.Next;

            ////Now see if the next line is a function. If it is, then get its parameters
            //if (line != null)
            //{
            //    MatchCollection functionMatches = GetFunctionMatched(line.Text, true, true);
            //    MatchCollection variableMatches = GetVariableMatched(line.Text);

            //    //Is the line a function?
            //    if (functionMatches.Count > 0)
            //    {
            //        commentClass.Visible = true;
            //        commentMemberFunction.Visible = true;
            //        commentStaticFunction.Visible = true;
            //    }
            //    else
            //    {
            //        commentClass.Visible = false;
            //        commentMemberFunction.Visible = false;
            //        commentStaticFunction.Visible = false;
            //    }

            //    //Is the line a variable?
            //    if (variableMatches.Count > 0)
            //    {
            //        commentEnum.Visible = true;
            //        commentMemberVariable.Visible = true;
            //        commentStaticVariable.Visible = true;
            //    }
            //    else
            //    {
            //        commentEnum.Visible = false;
            //        commentMemberVariable.Visible = false;
            //        commentStaticVariable.Visible = false;
            //    }
            //}

        }
    }
}
