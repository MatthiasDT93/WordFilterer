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
        var combinations = _storage.FindCombinations(words, targetLength);
        _storage.WriteCombinationsToFile(combinations);
    }
}
