using System;

namespace Otus.Delegates
{
    public class FileArgs : EventArgs
    {
        public string FileName { get; }
        public bool Cancel { get; set; }

        public FileArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}