using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Combinations;

public interface ICombinationFinder
{
    public bool CombinationExists(HashSet<string> stringWords, Word word1, Word word2);

    public List<Word> FindCombinations(List<Word> words, int targetLength);

    public List<Word> FindAnyCombinations(List<Word> words, int targetLength);

    public void FindCombinationsForWord(string remaining, HashSet<Word> parts, List<string> workList, List<string> result);
}
