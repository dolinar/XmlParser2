using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetronikParser.FileReader;
using MetronikParser.Logger;

namespace MetronikParser.FileReader
{
    public abstract class FileObject<T>
    { 
        public string StringPath { get; set; }

        public abstract T ReadFile();

        public ILogger Log { get; set; }
    }
}
