namespace WordFilterer.Core.Domain;

public record Word
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Length => Content.Length;

    public Word(string content)
    {
        Id = Guid.NewGuid();
        Content = content;
    }


    public static Word StringToWord(string content) => new Word(content);
    public static string WordToString(Word word) => word.Content;
}
