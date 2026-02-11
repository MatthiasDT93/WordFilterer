using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Storage;

public class Storage : IStorage
{
    public string firstfile;

    public IFileStore _filestore;

    public Storage(IFileStore filestore)
    {
        _filestore = filestore;
    }

    public string[] ReadFile()
    {
        var firstFile = _filestore
                        .EnumerateFiles()
                        .OrderBy(f => f)
                        .FirstOrDefault();
        if (firstFile is null)
            throw new FileNotFoundException("No file found in the Input folder.");
        
        return _filestore.ReadAllLines(firstFile);
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
        _filestore.WriteAllLines("output.txt", SaveWords(combinations));
    }

    public bool CombinationExists(List<Word> words, string combination)
    {
        return words.Any(w => w.Content==combination);
    }

    public List<Word> FindCombinations(List<Word> words, int targetLength)
    {
        if (targetLength <= 0)
            throw new ArgumentException("Please only enter positive numbers.");

        if (!words.Any(w => w.Length >= targetLength))
            throw new ArgumentException("There are no words of this target length in the input data.");

        HashSet<string> matchWords = words.Where(w => w.Length == targetLength).Select(w => w.Content).ToHashSet();
        List<Word> searchWords = words.Where(w => w.Length <= targetLength).ToList();
        List<string> stringResult = new List<string>();
        foreach(var word in searchWords)
        {
            foreach(var combo in searchWords)
            {
                if (combo.Id != word.Id && matchWords.Contains(combo.Content + word.Content))
                {
                    stringResult.Add($"{word.Content} + {combo.Content} = {word.Content + combo.Content}");
                }
            }
        }
        var result = stringResult.Distinct().Select(x => Word.StringToWord(x)).ToList();

        return result.OrderBy(w => w.Content.Split(" = ")[1]).ToList();
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

