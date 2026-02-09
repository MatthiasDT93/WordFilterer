using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.Storage;

public class FileStore : IFileStore
{
    public bool FileExists(string file)
    {
        return !String.IsNullOrEmpty(file);
    }

    public string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    public void WriteAllLines(string path, string[] lines)
    {
        File.WriteAllLines(path, lines);
    }
}

