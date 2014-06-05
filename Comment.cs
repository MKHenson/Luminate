using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luminate
{
    /// <summary>
    /// This simple class is used to as a structure to hold information about the JS structure.
    /// </summary>
    public class Token
    {
        public string name = "";
        public string desc = "";
        public string type = "";
        public string extends = null;
        public Dictionary<string, Token> parameters = new Dictionary<string,Token>();
        public string visibility = "";
        public string author = "";
        public string created = "";
        public string returnType = null;
        public string returnDesc = "";
        public Dictionary<string, Token> mVariables = new Dictionary<string, Token>();
        public Dictionary<string, Token> sVariables = new Dictionary<string, Token>();
        public Dictionary<string, Token> mFunctions = new  Dictionary<string, Token>();
        public Dictionary<string, Token> sFunctions = new Dictionary<string, Token>();
        public Dictionary<string, Token> enums = new Dictionary<string, Token>();
    }
}
