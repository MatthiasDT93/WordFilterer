using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.Combinations;
using WordFilterer.Core.Domain;
using WordFilterer.Core.Storage;

namespace WordFilterer.Tests;

public class CombinationFinderShould
{
    private readonly CombinationFinder combinationFinder;
    private readonly List<Word> wordData;

    public CombinationFinderShould()
    {
        combinationFinder = new CombinationFinder();
        wordData = new List<Word>() {
            Word.StringToWord("te"),
            Word.StringToWord("st"),
            Word.StringToWord("foo"),
            Word.StringToWord("bar"),
            Word.StringToWord("foobar"),
            Word.StringToWord("fo"),
            Word.StringToWord("o"),
            Word.StringToWord("obar")
        };
    }

    [Fact]
    public void Correctly_Verify_If_Combination_Exist()
    {
        var stringHashData = new HashSet<string> { "foobar" };
        var word1 = Word.StringToWord("fo");
        var word2 = Word.StringToWord("obar");
        var word3 = Word.StringToWord("te");
        var word4 = Word.StringToWord("st");

        var result1 = combinationFinder.CombinationExists(stringHashData, word1, word2);
        var result2 = combinationFinder.CombinationExists(stringHashData, word3, word4);

        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Throws_If_CombinationLength_Is_Not_Present_In_Data()
    {
        var exception1 = Assert.Throws<ArgumentException>(() => combinationFinder.FindCombinations(wordData, 42));
        var exception2 = Assert.Throws<ArgumentException>(() => combinationFinder.FindCombinations(wordData, -1));

        Assert.Equal("There are no words of this target length in the input data.", exception1.Message);
        Assert.Equal("Please only enter positive numbers.", exception2.Message);
    }

    [Fact]
    public void Correctly_Find_Combinations_In_Data()
    {
        var result = combinationFinder.FindCombinations(wordData, 6);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, w => w.Content == "fo + obar = foobar");
        Assert.Contains(result, w => w.Content == "foo + bar = foobar");
    }

    [Fact]
    public void Correctly_Find_Combinations_Of_Any_Number_Of_Words()
    {
        var result = combinationFinder.FindAnyCombinations(wordData, 6);

        Assert.Equal(3, result.Count);
        Assert.Contains(result, w => w.Content == "fo + obar = foobar");
        Assert.Contains(result, w => w.Content == "foo + bar = foobar");
        Assert.Contains(result, w => w.Content == "fo + o + bar = foobar");
    }
}
