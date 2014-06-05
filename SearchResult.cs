using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Luminate
{
    /// <summary>
    /// This class acts as a small holder for a search 
    /// </summary>
    public class SearchResult
    {
        public ScintillaNET.Range range;
        public Document document;
        public string file;

        /// <summary>
        /// Simple constuctor
        /// </summary>
        /// <param name="range"></param>
        /// <param name="file"></param>
        public SearchResult( ScintillaNET.Range range, string file, Document doc )
        {
            this.document = doc;
            this.range = range;
            this.file = file;
        }

        /// <summary>
        /// Clean up
        /// </summary>
        public void Dispose()
        {
            this.document = null;
            this.range = null;
            this.file = null;
        }
    }
}
