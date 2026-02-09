using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.Storage;

public interface IFileStore
{
    public bool FileExists(string path);
    public string[] ReadAllLines(string path);
    public void WriteAllLines(string path, string[] lines);
}
