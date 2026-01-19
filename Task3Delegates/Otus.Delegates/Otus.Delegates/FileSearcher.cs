using System;
using System.IO;

namespace Otus.Delegates
{
    public class FileSearcher
    {
        public event EventHandler<FileArgs> FileFound;

        public void Search(string path)
        {
            if (!Directory.Exists(path)) return;

            foreach (var file in Directory.EnumerateFiles(path))
            {
                var args = new FileArgs(Path.GetFileName(file));

                FileFound?.Invoke(this, args);

                if (args.Cancel) break;
            }
        }
    }
}