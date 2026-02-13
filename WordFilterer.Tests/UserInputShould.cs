using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.CLI.UI;
using WordFilterer.CLI.Consoles;
using WordFilterer.Core.Domain;
using WordFilterer.Core.UI;
using WordFilterer.Core.Storage;
using WordFilterer.Core.Combinations;


namespace WordFilterer.Tests;

public class UserInputShould
{
    private readonly Mock<IConsole> console;
    private readonly Mock<IStorage> storage;
    private readonly Mock<ICombinationFinder> combinationFinder;
    private readonly UserInput userInput;
    private readonly List<Word> wordData;
    private readonly List<Word> combinations1;
    private readonly List<Word> combinations2;

    public UserInputShould()
    {
        console = new Mock<IConsole>();
        storage = new Mock<IStorage>();
        combinationFinder = new Mock<ICombinationFinder>();
        userInput = new UserInput(storage.Object, console.Object, combinationFinder.Object);
        wordData = new List<Word>() {
            Word.StringToWord("te"),
            Word.StringToWord("st"),
            Word.StringToWord("foo"),
            Word.StringToWord("bar"),
            Word.StringToWord("foobar"),
            Word.StringToWord("fo"),
            Word.StringToWord("obar")
        };
        combinations1 = new List<Word>() {
            Word.StringToWord("foo + bar = foobar"),
            Word.StringToWord("fo + obar = foobar")
        };
        combinations2 = new List<Word>();
    }

    [Fact]
    public void Use_Default_If_No_Argument()
    {
        storage.Setup(s => s.LoadDataIntoWords()).Returns(wordData);
        combinationFinder.Setup(cf => cf.FindCombinations(It.IsAny<List<Word>>(), It.IsAny<int>())).Returns(combinations1);
        userInput.CalculateCombinations();

        storage.Verify(s => s.LoadDataIntoWords(), Times.Once());
        console.Verify(c => c.WriteLine("Total number of words found in the input file: 7"));
        console.Verify(c => c.WriteLine("Total number of words satisfying length <= 6: 7"));
        console.Verify(c => c.WriteLine("Finding combinations..."));
        combinationFinder.Verify(cf => cf.FindCombinations(wordData, 6), Times.Once());
        console.Verify(c => c.WriteLine("Number of combinations found: 2"));
        storage.Verify(s => s.WriteCombinationsToFile(combinations1));
    }

    [Fact]
    public void Use_Input_Correctly()
    {
        storage.Setup(s => s.LoadDataIntoWords()).Returns(wordData);
        combinationFinder.Setup(cf => cf.FindCombinations(It.IsAny<List<Word>>(), It.IsAny<int>())).Returns(combinations2);
        userInput.CalculateCombinations(4);

        storage.Verify(s => s.LoadDataIntoWords(), Times.Once());
        console.Verify(c => c.WriteLine("Total number of words found in the input file: 7"));
        console.Verify(c => c.WriteLine("Total number of words satisfying length <= 4: 6"));
        console.Verify(c => c.WriteLine("Finding combinations..."));
        combinationFinder.Verify(cf => cf.FindCombinations(wordData, 4), Times.Once());
        console.Verify(c => c.WriteLine("Number of combinations found: 0"));
        storage.Verify(s => s.WriteCombinationsToFile(combinations2));
    }

    [Fact]
    public void Calls_Correct_Storage_Method_When_Any_Combinations_Requested()
    {
        storage.Setup(s => s.LoadDataIntoWords()).Returns(wordData);
        combinationFinder.Setup(cf => cf.FindAnyCombinations(It.IsAny<List<Word>>(), It.IsAny<int>())).Returns(combinations1);
        userInput.CalculateCombinations(binaryCombinations: false);

        storage.Verify(s => s.LoadDataIntoWords(), Times.Once());
        console.Verify(c => c.WriteLine("Total number of words found in the input file: 7"));
        console.Verify(c => c.WriteLine("Total number of words satisfying length <= 6: 7"));
        console.Verify(c => c.WriteLine("Finding combinations..."));
        combinationFinder.Verify(cf => cf.FindAnyCombinations(wordData, 6), Times.Once());
        console.Verify(c => c.WriteLine("Number of combinations found: 2"));
        storage.Verify(s => s.WriteCombinationsToFile(combinations1));
    }
}
