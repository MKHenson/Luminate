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
    /// <summary>
    /// This control shows function arguments
    /// </summary>
    public partial class ArgumentsControl : UserControl
    {
        private Graphics mGraphics;

        /// <summary>
        /// Simple constuctor
        /// </summary>
        public ArgumentsControl()
        {
            InitializeComponent();

            mGraphics = CreateGraphics();
        }

        public void FillData( Token token )
        {
            Name = token.visibility + " ";

            //Fill in the name
            if ( token.type == "class")
                Name += "class ";
            else if (token.type == "mfunc" || token.type == "sfunc")
                Name += "function ";
            else if (token.type == "enum")
                Name += "enum ";
            else
                Name += "variable ";

            Name += token.name;
            Name = (token.extends != null ? "<" + token.extends.Trim() + "> " : "") + Name;

            //The description
            Description = token.desc;

            //Params
            string paramsStr = "";
            foreach (KeyValuePair<string, Token> pair in token.parameters)
                paramsStr += "<" + pair.Value.type.Trim() + "> " + pair.Value.name + " - " + pair.Value.desc + "\n";

            if (paramsStr == "")
                labelParams.Visible = false;
            else
            {
                Params = paramsStr;
                labelParams.Visible = true;
                
            }

            labelP.Visible = labelParams.Visible;
            labelParams.Location = new Point(labelDesc.Location.X, labelDesc.Location.Y + labelDesc.Size.Height + 10);
            labelP.Location = new Point(labelP.Location.X, labelParams.Location.Y);

            //Returns
            if (token.returnType != "" && token.returnType != "null" && token.returnType != null)
            {
                labelReturns.Visible = true;
                Returns = "<" + token.returnType.Trim() + "> " + token.returnDesc;

                if ( labelParams.Visible )
                    labelReturns.Location = new Point(labelParams.Location.X, labelParams.Location.Y + labelParams.Size.Height + 10);
                else
                    labelReturns.Location = new Point(labelDesc.Location.X, labelDesc.Location.Y + labelDesc.Size.Height + 10);

                labelR.Location = new Point(labelR.Location.X, labelReturns.Location.Y);
            }
            else
                labelReturns.Visible = false;

            labelR.Visible = labelReturns.Visible;

            if ( labelReturns.Visible )
                Size = new Size(Size.Width, labelReturns.Location.Y + labelReturns.Size.Height + 20);
            else if (labelParams.Visible)
                Size = new Size(Size.Width, labelParams.Location.Y + labelParams.Size.Height + 20);
            else
                Size = new Size(Size.Width, labelDesc.Location.Y + labelDesc.Size.Height + 20 );
            
        }

        /// <summary>
        /// Gets or sets the font for this control
        /// </summary>
        public new string Name
        {
            get { return labelName.Text; }
            set 
            { 
                labelName.Text = value;                
                //float padding = 20;
                //System.Drawing.Size s = TextRenderer.MeasureText(value, labelName.Font);
                //this.Size = new System.Drawing.Size(s.Width + (int)padding, s.Height + (int)padding);
                //labelName.Location = new System.Drawing.Point((int)padding / 2, (int)padding / 2);
                //labelName.Size = s;
            }
        }

        /// <summary>
        /// Gets or sets the font for this control
        /// </summary>
        public string Description
        {
            get { return labelDesc.Text; }
            set
            {
                labelDesc.Text = value;
                //float padding = 20;
                //System.Drawing.Size s = TextRenderer.MeasureText(value, labelDesc.Font, new Size(Size.Width, labelDesc.Height));
                //System.Drawing.Size size = new System.Drawing.Size(s.Width, s.Height);


                SizeF size = mGraphics.MeasureString(value, labelDesc.Font, labelDesc.Width);
                labelDesc.Size = new System.Drawing.Size(labelDesc.Width, (int)size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the font for this control
        /// </summary>
        public string Params
        {
            get { return labelParams.Text; }
            set
            {
                labelParams.Text = value;

                SizeF size = mGraphics.MeasureString(value, labelParams.Font, labelParams.Width);
                labelParams.Size = new System.Drawing.Size(labelParams.Width, (int)size.Height);
                
                labelParams.Visible = true;
                labelP.Visible = true;
            }
        }

        /// <summary>
        /// Gets or sets the font for this control
        /// </summary>
        public string Returns
        {
            get { return labelReturns.Text; }
            set
            {
                labelReturns.Text = value;
                //float padding = 20;
                //System.Drawing.Size s = TextRenderer.MeasureText(value, labelName.Font);
                //this.Size = new System.Drawing.Size(s.Width + (int)padding, s.Height + (int)padding);
                //labelName.Location = new System.Drawing.Point((int)padding / 2, (int)padding / 2);
                //labelName.Size = s;
            }
        }
    }
}
