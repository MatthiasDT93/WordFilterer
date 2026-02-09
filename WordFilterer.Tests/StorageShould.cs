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
    private readonly Word word;
    private readonly string[] fakeInputData;
    private readonly List<Word> readData;

    public StorageShould()
    {
        filestore = new Mock<IFileStore>();
        storage = new Storage(filestore.Object);
        word = Word.StringToWord("foobar");
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
            Word.StringToWord("oobar"),
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
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(fakeInputData);

        var list = storage.ReadFile();

        Assert.Equivalent(list, fakeInputData);
    }

    [Fact]
    public void Correctly_Convert_ReadData_To_Words()
    {
        filestore.Setup(fs => fs.ReadAllLines(It.IsAny<string>())).Returns(fakeInputData);

        var list = storage.LoadWords();


    }
}

