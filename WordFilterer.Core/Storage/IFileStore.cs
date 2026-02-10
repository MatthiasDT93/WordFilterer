using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.Storage;

public interface IFileStore
{
    public IEnumerable<string> EnumerateFiles();
    public string[] ReadAllLines(string path);
    public void WriteAllLines(string path, string[] lines);
}
