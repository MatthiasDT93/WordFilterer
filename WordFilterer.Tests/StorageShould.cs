using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using WordFilterer.Core.Domain;
using WordFilterer.Core.Storage;

namespace WordFilterer.Tests;

public class StorageShould
{
    private readonly Mock<IFileStore> filestore;
    private readonly Storage storage;
    private readonly string[] stringArrayData;
    private readonly List<Word> wordData;
    private readonly string outputPath;
    private readonly string inputPath;

    public StorageShould()
    {
        filestore = new Mock<IFileStore>();
        storage = new Storage(filestore.Object);
        stringArrayData = [
            "te",
            "st",
            "foo",
            "bar",
            "foobar",
            "fo",
            "obar"
        ];
        wordData = new List<Word>() {
            Word.StringToWord("te"),
            Word.StringToWord("st"),
            Word.StringToWord("foo"),
            Word.StringToWord("bar"),
            Word.StringToWord("foobar"),
            Word.StringToWord("fo"),
            Word.StringToWord("obar"),
        };

        inputPath = "C:\\Users\\matth\\source\\repos\\WordFilterer\\Data\\Input\\input.txt";
        outputPath = "C:\\Users\\matth\\source\\repos\\WordFilterer\\Data\\Output\\output.txt";
    }

    [Fact]
    public void Throw_If_No_Input_Available()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(Enumerable.Empty<string>);

        var exception = Assert.Throws<FileNotFoundException>(() => storage.ReadFile());
        Assert.Equal("No file found in the Input folder.", exception.Message);
    }

    [Fact]
    public void Correctly_Import_Data()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(new[] {""});
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(stringArrayData);

        var list = storage.ReadFile();

        Assert.Equivalent(list, stringArrayData);
    }

    [Fact]
    public void Correctly_Convert_ReadData_To_Words()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(new[] { "" });
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(stringArrayData);

        var list = storage.LoadWords();

        Assert.Equal(7, list.Count);
        Assert.Equal("te", list[0].Content);
        Assert.IsType<Word>(list[0]);
    }

    [Fact]
    public void Correctly_Converts_Words_To_Array()
    {
        var result = storage.SaveWords(wordData);

        Assert.Equal(result, stringArrayData);
    }

    [Fact]
    public void Correctly_Verify_If_Combination_Exist()
    {
        var stringHashData = new HashSet<string> { "foobar" };
        var word1 = Word.StringToWord("fo");
        var word2 = Word.StringToWord("obar");
        var word3 = Word.StringToWord("te");
        var word4 = Word.StringToWord("st");

        var result1 = storage.CombinationExists(stringHashData, word1, word2); 
        var result2 = storage.CombinationExists(stringHashData, word3, word4);

        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Throws_If_CombinationLength_Is_Not_Present_In_Data()
    {
        var exception1 = Assert.Throws<ArgumentException>(() => storage.FindCombinations(wordData, 42));
        var exception2 = Assert.Throws<ArgumentException>(() => storage.FindCombinations(wordData, -1));

        Assert.Equal("There are no words of this target length in the input data.", exception1.Message);
        Assert.Equal("Please only enter positive numbers.", exception2.Message);
    }

    [Fact]
    public void Correctly_Find_Combinations_In_Data()
    {
        var result = storage.FindCombinations(wordData, 6);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, w => w.Content == "fo + obar = foobar");
        Assert.Contains(result, w => w.Content == "foo + bar = foobar");
    }

    // STILL NEED TO FIX FILE INPUT PATHS
    [Fact(Skip = "wip")]
    public void Call_FileStore_To_Write_Output()
    {
        var combinations = storage.FindCombinations(wordData, 6);
        var output = storage.SaveWords(combinations);
        storage.WriteCombinationsToFile(combinations);

        filestore.Verify(fs => fs.WriteAllLines(outputPath, output));
    }
}

