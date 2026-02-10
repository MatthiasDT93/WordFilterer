using System;
using System.Collections.Generic;
using System.Text;

namespace WordFilterer.Core.UI;

public interface IConsole
{
    void WriteLine(string text);
    void Write(string text);
    string ReadLine();
}
