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
    private readonly string[] fakeInputData;
    private readonly List<Word> readData;

    public StorageShould()
    {
        filestore = new Mock<IFileStore>();
        storage = new Storage(filestore.Object);
        fakeInputData = [
            "te",
            "st",
            "foo",
            "bar",
            "foobar",
            "fo",
            "obar"
        ];
        readData = new List<Word>() {
            Word.StringToWord("te"),
            Word.StringToWord("st"),
            Word.StringToWord("foo"),
            Word.StringToWord("bar"),
            Word.StringToWord("foobar"),
            Word.StringToWord("fo"),
            Word.StringToWord("obar"),
        };
    }

    [Fact]
    public void Throw_If_No_Input_Available()
    {
        filestore.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(false);

        var exception = Assert.Throws<FileNotFoundException>(() => storage.ReadFile());
        Assert.Equal("No file found in the Input folder.", exception.Message);
    }

    [Fact]
    public void Correctly_Import_Data()
    {
        filestore.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(fakeInputData);

        var list = storage.ReadFile();

        Assert.Equivalent(list, fakeInputData);
    }

    [Fact]
    public void Correctly_Convert_ReadData_To_Words()
    {
        filestore.Setup(fs => fs.FileExists(It.IsAny<string>())).Returns(true);
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(fakeInputData);

        var list = storage.LoadWords();

        Assert.Equal(7, list.Count);
        Assert.Equal("te", list[0].Content);
        Assert.IsType<Word>(list[0]);
    }

    [Fact]
    public void Correctly_Converts_Words_To_Array()
    {
        var result = storage.SaveWords(readData);

        Assert.Equal(result, fakeInputData);
    }

    [Fact]
    public void Correctly_Verify_If_Combination_Exist()
    {
        var result1 = storage.CombinationExists(readData, "foobar");
        var result2 = storage.CombinationExists(readData, "test");

        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public void Throws_If_CombinationLength_Is_Not_Present_In_Data()
    {
        var exception1 = Assert.Throws<ArgumentException>(() => storage.FindCombinations(readData, 42));
        var exception2 = Assert.Throws<ArgumentException>(() => storage.FindCombinations(readData, -1));

        Assert.Equal("There are no words of this target length in the input data.", exception1.Message);
        Assert.Equal("Please only enter positive numbers.", exception2.Message);
    }

    [Fact]
    public void Correctly_Find_Combinations_In_Data()
    {
        var result = storage.FindCombinations(readData, 6);

        Assert.Equal(2, result.Count);
        Assert.Equal("foo + bar = foobar", result[0].Content);
        Assert.Equal("fo + obar = foobar", result[1].Content);
    }
}

