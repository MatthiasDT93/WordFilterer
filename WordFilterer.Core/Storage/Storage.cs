using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Storage;

public class Storage : IStorage
{
    private readonly IFileStore _filestore;

    public Storage(IFileStore filestore)
    {
        _filestore = filestore;
    }

    public string[] ReadDataFromFile()
    {
        var firstFile = _filestore
                        .EnumerateFiles()
                        .OrderBy(f => f)
                        .FirstOrDefault();
        if (firstFile is null)
            throw new FileNotFoundException("No file found in the Input folder.");
        
        return _filestore.ReadAllLines(firstFile);
    }

    public List<Word> LoadDataIntoWords()
    {
        var lines = ReadDataFromFile();
        return lines.Select(l => Word.StringToWord(l)).ToList();
    }

    public string[] SaveWordsIntoData(List<Word> words)
    {
        return words.Select(w => Word.WordToString(w)).ToArray();
    }

    public void WriteCombinationsToFile(List<Word> combinations)
    {
        _filestore.WriteAllLines("output.txt", SaveWordsIntoData(combinations));
    }
}

