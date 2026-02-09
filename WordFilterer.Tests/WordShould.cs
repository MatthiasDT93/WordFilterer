using WordFilterer.Core.Domain;
namespace WordFilterer.Tests;

public class WordShould
{
    [Fact]
    public void Correctly_Initialise()
    {
        var word = new Word("Test");

        Assert.IsType<Guid>(word.Id);
        Assert.NotEqual(Guid.Empty, word.Id);
        Assert.Equal("Test", word.Content);
        Assert.Equal(4, word.Length);
    }

    [Fact]
    public void Be_Correctly_Converted_Into_String()
    {
        var word = new Word("Test");

        var stringWord = Word.WordToString(word);
        Assert.Equal("Test", stringWord);
    }

    [Fact]
    public void Be_Correctly_Converted_From_String()
    {
        var stringWord = "Test";

        var word = Word.StringToWord(stringWord);

        Assert.IsType<Guid>(word.Id);
        Assert.NotEqual(Guid.Empty, word.Id);
        Assert.Equal("Test", word.Content);
        Assert.Equal(4, word.Length);
    }
}
