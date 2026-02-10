using System;
using System.Collections.Generic;
using System.Text;
//using WordFilterer.CLI.Console;

namespace WordFilterer.Core.UI;

public class Menu : IMenu
{
    private readonly IUserInput _userInput;
    private readonly IConsole _console;

    public Menu(IUserInput userInput, IConsole console)
    {
        _userInput = userInput;
        _console = console;
    }

    public void InputCombinationLength()
    {
        bool quit = false;
        while (!quit)
        {
            _console.WriteLine("Please place your input file in the Input folder, and enter a positive number to define combination length (default: 6)");
            _console.WriteLine("Press 'q' to quit, press ENTER to use the default length (6)");

            var input = "";
            while (string.IsNullOrEmpty(input))
            {
                input = _console.ReadLine();
            }

            if (input == "q")
            {
                quit = true;
                _console.WriteLine("Shutting down...");
                break;
            }

            if (int.TryParse(input, out _))
            {
                var choice = int.Parse(input);
                _console.WriteLine($"You selected: {input}");
                _userInput.EnterTargetLength(choice);
            } else
            {
                _console.WriteLine("Invalid choice.");
            }
        }
    }
}

