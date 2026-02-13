namespace WordFilterer.Core.Domain;

// this could be a record if refactoring of combination finding makes the ID check obsolete
public class Word
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Content { get; private set; } = string.Empty;
    public int Length => Content.Length;
    public static Word StringToWord(string content) => new Word() { Content = content ?? String.Empty };
    public static string WordToString(Word word) => word.Content;
}
