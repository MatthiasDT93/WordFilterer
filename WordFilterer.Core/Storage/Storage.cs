using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Storage;

public class Storage : IStorage
{
    public const string inputDirectoryPath = @".\Data\Input\";
    public const string outputDirectoryPath = @".\Data\Output\";

    public string firstfile = Directory
                                .EnumerateFiles(inputDirectoryPath)
                                .OrderBy(f => f)
                                .FirstOrDefault() ?? String.Empty;

    public IFileStore _filestore;

    public Storage(IFileStore filestore)
    {
        _filestore = filestore;
    }

    public string[] ReadFile()
    {
        if (!_filestore.FileExists(firstfile))
            throw new FileNotFoundException("No file found in the Input folder.");

        return _filestore.ReadAllLines(firstfile);
    }

    public List<Word> LoadWords()
    {
        var lines = ReadFile();
        return lines.Select(l => Word.StringToWord(l)).ToList();
    }

    public string[] SaveWords(List<Word> words)
    {
        return words.Select(w => Word.WordToString(w)).ToArray();
    }

    public void WriteCombinationsToFile(List<Word> combinations)
    {
        _filestore.WriteAllLines(outputDirectoryPath+"output.txt", SaveWords(combinations));
    }

    public bool CombinationExists(List<Word> words, string combination)
    {
        return words.Any(w => w.Content==combination);
    }

    public List<Word> FindCombinations(List<Word> words, int targetLength)
    {
        if (!words.Any(w => w.Length >= targetLength))
            throw new ArgumentException("There are no words of this target length in the input data.");

        List<Word> result = new List<Word>();

        foreach (var word in words)
        {
            var temp = new List<Word>();
            foreach (var combo in words)
            {
                if(combo.Id != word.Id && combo.Length + word.Length == targetLength && CombinationExists(words, word.Content + combo.Content))
                {
                    temp.Add(Word.StringToWord($"{word.Content} + {combo.Content} = {word.Content+combo.Content}"));
                }
            }
            result.AddRange(temp);
        }

        return result;
    }


    //public string MakeCombination(List<Word> words, Word word)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    string output = "";
    //    int count = 0;
    //    foreach (var part in words)
    //    {
    //        if (word.Content.StartsWith(sb.ToString() + part.Content, StringComparison.OrdinalIgnoreCase))
    //        {
    //            sb.Append(part.Content);
    //            count += part.Length;
    //        }
    //    }

    //}

    //public List<Word> FindCombinations2(List<Word> words, int targetLength)
    //{
    //    var result = new List<Word>();
    //    var targetWords = words.Where(w => w.Length == targetLength).Distinct().ToList();

    //    foreach(var word in targetWords)
    //    {
    //        var parts = words.Where(w => word.Content.Contains(w.Content)).ToList();
    //        foreach(var piece in parts)
    //        {
    //            var temp = parts.Where(p => p.Id != piece.Id && piece.Content+p.Content==word.Content).ToList();
    //        }
    //    }
    //}
}

