namespace WordFilterer.Core.Domain;

public record Word
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Content { get; private set; } = string.Empty;
    public int Length => Content.Length;

    //public Word(string content)
    //{
    //    Id = Guid.NewGuid();
    //    Content = content;
    //}


    public static Word StringToWord(string content) => new Word() { Content = content ?? String.Empty };
    public static string WordToString(Word word) => word.Content;
}
