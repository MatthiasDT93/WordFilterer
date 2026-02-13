using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Domain;

namespace WordFilterer.Core.Storage;

public interface IStorage
{
    public string[] ReadDataFromFile();
    public List<Word> LoadDataIntoWords();
    public string[] SaveWordsIntoData(List<Word> words);
    public void WriteCombinationsToFile(List<Word> words);
}

