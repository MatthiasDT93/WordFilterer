using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Storage;

namespace WordFilterer.CLI.FileStore;

public class FileStore : IFileStore
{
    private readonly string _inputDir;
    private readonly string _outputDir;

    public FileStore(string inputDir, string outputDir)
    {
        _inputDir = inputDir;
        _outputDir = outputDir;

        Directory.CreateDirectory(_inputDir);
        Directory.CreateDirectory(_outputDir);
    }

    public IEnumerable<string> EnumerateFiles()
    {
        return Directory.EnumerateFiles(_inputDir);
    }

    public string[] ReadAllLines(string path)
    {
        return File.ReadAllLines(path);
    }

    public void WriteAllLines(string fileName, string[] lines)
    {
        var fullPath = Path.Combine(_outputDir, fileName);
        File.WriteAllLines(fullPath, lines);
    }
}