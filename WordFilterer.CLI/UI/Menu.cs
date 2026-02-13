using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.UI;

namespace WordFilterer.CLI.UI;

public class Menu : IMenu
{
    private readonly IUserInput _userInput;
    private readonly IConsole _console;

    public Menu(IUserInput userInput, IConsole console)
    {
        _userInput = userInput;
        _console = console;
    }

    public void GenerateCombinations()
    {
        _console.WriteLine("Please place your input file in the Input folder.");
        _console.WriteLine("Find combinations of two words? (Y) or any number of words (N).");
        _console.WriteLine("Enter a positive number to define the length of the words of which combinations need to be generated (default: 6).");
        _console.WriteLine("Press 'Q' to quit, press ENTER to use the default option.");
        _console.WriteLine("");

        _console.WriteLine("Two words? (Y/N)");
        var comboChoiceInput = _console.ReadLine();
        var comboChoice = true;

        _console.WriteLine("Combination length:");
        var input = _console.ReadLine() ?? string.Empty;

        if (string.Equals(comboChoiceInput, "q", StringComparison.OrdinalIgnoreCase) || 
            string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
        {
            _console.WriteLine("Shutting down...");
            return;
        }

        if (string.IsNullOrWhiteSpace(comboChoiceInput) || string.Equals(comboChoiceInput, "Y", StringComparison.OrdinalIgnoreCase))
        {
            _console.WriteLine("You selected the default option: Y");
            comboChoice = true;
        }
        else if (string.Equals(comboChoiceInput, "N", StringComparison.OrdinalIgnoreCase))
        {
            _console.WriteLine("You selected the option: N");
            comboChoice = false;
        }
        else
        {
            _console.WriteLine("Invalid choice.");
            return;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            _console.WriteLine("You selected the default option: 6");
            _userInput.CalculateCombinations(binaryCombinations: comboChoice);
            return;
        }

        if (!int.TryParse(input, out var parsed)) 
        {
            _console.WriteLine("Invalid choice.");
            return;
        }

        if(parsed == 6)
        {
            _console.WriteLine("You selected the default option: 6");
            _userInput.CalculateCombinations(binaryCombinations: comboChoice);
            return;
        }

        _console.WriteLine($"You selected: {input}");
        _userInput.CalculateCombinations(parsed, comboChoice);
    }
}

