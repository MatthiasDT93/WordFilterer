using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Storage;

public interface IStorage
{
    public string[] ReadFile();
    public List<Word> LoadWords();
    public string[] SaveWords(List<Word> words);
    public void WriteCombinationsToFile(List<Word> words);

    public bool CombinationExists(List<Word> words, string combination);
    public List<Word> FindCombinations(List<Word> words, int targetLength);
}

