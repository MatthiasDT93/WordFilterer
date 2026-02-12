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

    public bool CombinationExists(HashSet<string> stringWords, Word word1, Word word2)
    {
        return stringWords.Contains(word1.Content + word2.Content);
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
                if (combo.Id != word.Id && CombinationExists(matchWords, word, combo))
                {
                    stringResult.Add($"{word.Content} + {combo.Content} = {word.Content + combo.Content}");
                }
            }
        }

        var result = stringResult.Distinct().Select(x => Word.StringToWord(x)).ToList();

        return result.OrderBy(w => w.Content.Split(" = ")[1]).ToList();
    }


    // Recursion try-out

    public List<Word> FindAnyCombinations(List<Word> words, int targetLength)
    {
        if (targetLength <= 0)
            throw new ArgumentException("Please only enter positive numbers.");

        if (!words.Any(w => w.Length >= targetLength))
            throw new ArgumentException("There are no words of this target length in the input data.");

        HashSet<Word> matchWords = words.Where(w => w.Length == targetLength).ToHashSet();
        var combinations = new List<string>();

        foreach(var word in matchWords)
        {
            var parts = words.Where(w => word.Content.Contains(w.Content)).ToHashSet();
            var result = new List<string>();
            var workList = new List<string>();
            FindCombinationsForWord(word.Content, parts, workList, result);
            combinations.AddRange(result);
        }

        return combinations.Distinct().Select(r => Word.StringToWord(r)).OrderBy(w => w.Content.Split(" = ")[1]).ToList();
    }

    public void FindCombinationsForWord(string remaining, HashSet<Word> parts, List<string> workList, List<string> result)
    {
        if ((remaining == ""))
        {
            if(string.Join(" + ", workList) != string.Join("", workList))
            {
                result.Add(string.Join(" + ", workList) + " = " + string.Join("", workList));
            }
            return;
        }
        foreach (var part in parts)
        {
            if (remaining.StartsWith(part.Content))
            {
                workList.Add(part.Content);
                FindCombinationsForWord(remaining.Substring(part.Length), parts, workList, result);
                workList.RemoveAt(workList.Count - 1);
            }
        }
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


    //public List<Word> combtest(List<Word> words, Word targetWord)
    //{
    //    var stringResult = new List<string>();


    //    foreach(var word in words)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        if (targetWord.Content.StartsWith(sb.Append(word.Content).ToString())) 
    //        { 
    //            sb.Append(word.Content);
    //        }
    //        if (sb.ToString() == targetWord.Content)
    //        { 
    //            stringResult.
    //        }
    //    }

    //    return stringResult.Select(s => Word.StringToWord(s)).ToList();
    //}


    //public List<Word> recursiontest(List<Word> words, Word targetWord, List<string> builderList, string builderWord = "")
    //{
    //    foreach(var word in words)
    //    {
    //        if(targetWord.Content == builderWord)
    //        {

    //        }
    //        if (targetWord.Content.StartsWith(builderWord + word.Content))
    //        {
    //            builderWord = builderWord + word.Content;
    //            builderList.Add(word.Content);
    //            recursiontest(words, targetWord, builderList, builderWord);
    //        }
    //    }
    //}
}

