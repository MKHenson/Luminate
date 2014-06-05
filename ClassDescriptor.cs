using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Luminate
{
    public partial class ClassDescriptor : UserControl
    {
        public ClassDescriptor()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Get or set the image list of the list
        /// </summary>
        public ImageList ImageList 
        {
            get { return listView.SmallImageList; }
            set { listView.SmallImageList = value; }
        }

        /// <summary>
        /// Fills the component with token data
        /// </summary>
        /// <param name="token"></param>
        public void FillData(Token token)
        {
            listView.Items.Clear();

            //enums
            foreach (KeyValuePair<string, Token> sub in token.enums)
            {
                ListViewItem toAdd = new ListViewItem(sub.Value.name.Trim(), 4);
                toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, sub.Value.desc));
                if ( sub.Value.returnDesc != null && sub.Value.returnDesc.Trim() != "" )
                    toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, "<" + sub.Value.returnType + "> " + sub.Value.returnDesc));
                listView.Items.Add(toAdd);
            }

            //member variables
            foreach (KeyValuePair<string, Token> sub in token.mVariables)
            {
                ListViewItem toAdd = new ListViewItem(sub.Value.name.Trim(), 3);
                toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, sub.Value.desc));
                if (sub.Value.returnDesc != null && sub.Value.returnDesc.Trim() != "")
                    toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, "<" + sub.Value.returnType + "> " + sub.Value.returnDesc));
                listView.Items.Add(toAdd);
            }

            //static member variables
            foreach (KeyValuePair<string, Token> sub in token.sVariables)
            {
                ListViewItem toAdd = new ListViewItem( sub.Value.name.Trim(), 3);
                toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, sub.Value.desc));
                if (sub.Value.returnDesc != null && sub.Value.returnDesc.Trim() != "")
                    toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, "<" + sub.Value.returnType + "> " + sub.Value.returnDesc));
                listView.Items.Add(toAdd);
            }

            //member functions
            foreach (KeyValuePair<string, Token> sub in token.mFunctions)
            {
                ListViewItem toAdd = new ListViewItem( sub.Value.name.Trim(), 0);
                toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, sub.Value.desc));
                if (sub.Value.returnDesc != null && sub.Value.returnDesc.Trim() != "")
                    toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, "<" + sub.Value.returnType + "> " + sub.Value.returnDesc));
                listView.Items.Add(toAdd);
            }

            //static member functions
            foreach (KeyValuePair<string, Token> sub in token.sFunctions)
            {
                ListViewItem toAdd = new ListViewItem( sub.Value.name.Trim(), 0);
                toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, sub.Value.desc));
                if (sub.Value.returnDesc != null && sub.Value.returnDesc.Trim() != "")
                    toAdd.SubItems.Add(new ListViewItem.ListViewSubItem(toAdd, "<" + sub.Value.returnType + "> " + sub.Value.returnDesc));
                listView.Items.Add(toAdd);
            }
        }
    }
}
