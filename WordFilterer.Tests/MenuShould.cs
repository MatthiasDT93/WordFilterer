using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WordFilterer.Core.UI;
using WordFilterer.CLI.UI;
using WordFilterer.Core.Storage;

namespace WordFilterer.Tests;

public class MenuShould
{
    private readonly Mock<IUserInput> userinput;
    private readonly Mock<IConsole> console;
    private readonly Menu menu;

    public MenuShould()
    {
        userinput = new Mock<IUserInput>();
        console = new Mock<IConsole>();
        menu = new Menu(userinput.Object, console.Object);
    }

    [Fact]
    public void Initialise_And_Quit_Correctly()
    {
        console.Setup(c => c.ReadLine()).Returns("q");

        menu.InputCombinationLength();

        console.Verify(c => c.WriteLine("Please place your input file in the Input folder, and enter a positive number to define combination length (default: 6)"));
        console.Verify(c => c.WriteLine("Press 'q' to quit, press ENTER to use the default length (6)"));
        console.Verify(c => c.WriteLine("Shutting down..."));
    }

    [Fact]
    public void Use_Default_If_Nothing_Entered()
    {
        console.Setup(c => c.ReadLine()).Returns("");

        menu.InputCombinationLength();

        console.Verify(c => c.WriteLine("You selected the default option: 6"));
    }

    [Fact]
    public void Use_Default_If_6_Entered()
    {
        console.Setup(c => c.ReadLine()).Returns("");

        menu.InputCombinationLength();

        console.Verify(c => c.WriteLine("You selected the default option: 6"));
    }

    [Fact]
    public void Show_Your_Input_Correctly()
    {
        console.Setup(c => c.ReadLine()).Returns("4");

        menu.InputCombinationLength();

        console.Verify(c => c.WriteLine("You selected: 4"));
    }

    [Fact]
    public void Use_Your_Input_Correctly()
    {
        console.Setup(c => c.ReadLine()).Returns("4");

        menu.InputCombinationLength();

        userinput.Verify(u => u.EnterTargetLength(4), Times.Once());
    }

    [Fact]
    public void Handle_Letter_Input()
    {
        console.Setup(c => c.ReadLine()).Returns("a");

        menu.InputCombinationLength();

        console.Verify(c => c.WriteLine("Invalid choice."));
    }
}
