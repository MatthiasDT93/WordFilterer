using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using WordFilterer.Core.Domain;
using WordFilterer.Core.Storage;
using WordFilterer.Core.Combinations;

namespace WordFilterer.Tests;

public class StorageShould
{
    private readonly Mock<IFileStore> filestore;
    private readonly Mock<ICombinationFinder> combinationFinder;
    private readonly Storage storage;
    private readonly string[] stringArrayData;
    private readonly List<Word> wordData;

    public StorageShould()
    {
        filestore = new Mock<IFileStore>();
        combinationFinder = new Mock<ICombinationFinder>();
        storage = new Storage(filestore.Object);
        stringArrayData = [
            "te",
            "st",
            "foo",
            "bar",
            "foobar",
            "fo",
            "o",
            "obar"
        ];
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
    public void Throw_If_No_Input_Available()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(Enumerable.Empty<string>);

        var exception = Assert.Throws<FileNotFoundException>(() => storage.ReadDataFromFile());
        Assert.Equal("No file found in the Input folder.", exception.Message);
    }

    [Fact]
    public void Correctly_Import_Data()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(new[] {""});
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(stringArrayData);

        var list = storage.ReadDataFromFile();

        Assert.Equivalent(list, stringArrayData);
    }

    [Fact]
    public void Correctly_Convert_ReadData_To_Words()
    {
        filestore.Setup(fs => fs.EnumerateFiles()).Returns(new[] { "" });
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(stringArrayData);

        var list = storage.LoadDataIntoWords();

        Assert.Equal(8, list.Count);
        Assert.Equal("te", list[0].Content);
        Assert.IsType<Word>(list[0]);
    }

    [Fact]
    public void Correctly_Converts_Words_To_Array()
    {
        var result = storage.SaveWordsIntoData(wordData);

        Assert.Equal(result, stringArrayData);
    }

    [Fact]
    public void Call_FileStore_To_Write_Output()
    {
        var combinations = new List<Word>();
        storage.WriteCombinationsToFile(combinations);

        filestore.Verify(fs => fs.WriteAllLines("output.txt", It.IsAny<string[]>()));
    }
}

