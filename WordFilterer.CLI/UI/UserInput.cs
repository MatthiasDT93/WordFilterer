using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Storage;
using WordFilterer.Core.UI;

namespace WordFilterer.CLI.UI;

public class UserInput : IUserInput
{
    private readonly IStorage _storage;
    private readonly IConsole _console;

    public UserInput(IStorage storage, IConsole console)
    {
        _storage = storage;
        _console = console;
    }
    public void CalculateCombinations(int targetLength = 6, bool binaryCombinations = true)
    {
        var words = _storage.LoadWords();
        _console.WriteLine($"Total number of words found in the input file: {words.Count}");
        _console.WriteLine($"Total number of words satisfying length <= {targetLength}: {words.Where(w => w.Length <= targetLength).ToList().Count}");
        _console.WriteLine("Finding combinations...");
        var combinations = binaryCombinations ? _storage.FindCombinations(words, targetLength) : _storage.FindAnyCombinations(words, targetLength);
        _console.WriteLine($"Number of combinations found: {combinations.Count}");
        _storage.WriteCombinationsToFile(combinations);
    }
}
