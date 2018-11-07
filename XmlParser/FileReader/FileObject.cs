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
