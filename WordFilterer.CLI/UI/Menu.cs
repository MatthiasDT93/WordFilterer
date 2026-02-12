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
        var validComboChoiceOptions = new HashSet<string> { "Y", "N", "q" };
        bool quit = false;
        while (!quit)
        {
            _console.WriteLine("Please place your input file in the Input folder.");
            _console.WriteLine("Find combinations of two words? (Y) or any number of words (N).");
            _console.WriteLine("Enter a positive number to define the length of the words of which combinations need to be generated (default: 6).");
            _console.WriteLine("Press 'q' to quit, press ENTER to use the default option.");
            _console.WriteLine("");

            _console.WriteLine("Two words? (Y/N)");
            var comboChoiceInput = _console.ReadLine().ToUpper();
            var comboChoice = true;

            _console.WriteLine("Combination length:");
            var input = _console.ReadLine() ?? string.Empty;

            if (input == "q" || comboChoiceInput == "q")
            {
                quit = true;
                _console.WriteLine("Shutting down...");
                break;
            }

            if (string.IsNullOrWhiteSpace(comboChoiceInput) || comboChoiceInput == "Y")
            {
                _console.WriteLine("You selected the default option: Y");
                comboChoice = true;
            }
            else if (comboChoiceInput == "N")
            {
                _console.WriteLine("You selected the option: N");
                comboChoice = false;
            }
            else if (!validComboChoiceOptions.Contains(comboChoiceInput))
            {
                _console.WriteLine("Invalid choice.");
                break;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                _console.WriteLine("You selected the default option: 6");
                _userInput.CalculateCombinations(binaryCombinations: comboChoice);
                break;
            }

            if (!int.TryParse(input, out _)) 
            {
                _console.WriteLine("Invalid choice.");
                break;
            }

            if(int.Parse(input) == 6)
            {
                _console.WriteLine("You selected the default option: 6");
                _userInput.CalculateCombinations(binaryCombinations: comboChoice);
                break;
            }

            var choice = int.Parse(input);
            _console.WriteLine($"You selected: {input}");
            _userInput.CalculateCombinations(choice, comboChoice);
            break;
        }
    }
}

