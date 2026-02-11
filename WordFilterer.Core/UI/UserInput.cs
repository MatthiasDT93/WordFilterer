using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Storage;

namespace WordFilterer.Core.UI;

public class UserInput : IUserInput
{
    private readonly IStorage _storage;

    public UserInput(IStorage storage)
    {
        _storage = storage;
    }
    public void EnterTargetLength(int targetLength = 6)
    {
        var words = _storage.LoadWords();
        Console.WriteLine($"Total number of words found in the input file: {words.Count}");
        Console.WriteLine($"Total number of words satisfying length <= {targetLength}: {words.Where(w => w.Length <= targetLength).ToList().Count}");
        Console.WriteLine("Finding combinations...");
        var combinations = _storage.FindCombinations(words, targetLength);
        Console.WriteLine($"Number of combinations found: {combinations.Count}");
        _storage.WriteCombinationsToFile(combinations);
    }
}
