using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Luminate
{
    /// <summary>
    /// This represents a tree node that is closed.
    /// </summary>
    public class TreeNodeClosed : TreeNode
    {
        public TreeNodeClosed(string text) : base(text)
        {
            ImageIndex = 8;
            SelectedImageIndex = 8;
        }
    }
}
