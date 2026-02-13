using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Combinations;
using WordFilterer.Core.Storage;
using WordFilterer.Core.UI;

namespace WordFilterer.CLI.UI;

public class UserInput : IUserInput
{
    private readonly IStorage _storage;
    private readonly IConsole _console;
    private readonly ICombinationFinder _combinationFinder;

    public UserInput(IStorage storage, IConsole console, ICombinationFinder combinationFinder)
    {
        _storage = storage;
        _console = console;
        _combinationFinder = combinationFinder;
    }
    public void CalculateCombinations(int targetLength = 6, bool binaryCombinations = true)
    {
        var words = _storage.LoadDataIntoWords();
        _console.WriteLine($"Total number of words found in the input file: {words.Count}");
        _console.WriteLine($"Total number of words satisfying length <= {targetLength}: {words.Where(w => w.Length <= targetLength).ToList().Count}");
        _console.WriteLine("Finding combinations...");
        var combinations = binaryCombinations ? _combinationFinder.FindCombinations(words, targetLength) : _combinationFinder.FindAnyCombinations(words, targetLength);
        _console.WriteLine($"Number of combinations found: {combinations.Count}");
        _storage.WriteCombinationsToFile(combinations);
    }
}
