using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.UI;

namespace WordFilterer.CLI.Consoles;

public class SystemConsole : IConsole
{
    public void WriteLine(string text) => Console.WriteLine(text);
    public void Write(string text) => Console.Write(text);
    public string ReadLine() => Console.ReadLine() ?? string.Empty;
}
