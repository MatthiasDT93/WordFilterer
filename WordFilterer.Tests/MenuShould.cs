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

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("Please place your input file in the Input folder."));
        console.Verify(c => c.WriteLine("Find combinations of two words? (Y) or any number of words (N)."));
        console.Verify(c => c.WriteLine("Enter a positive number to define the length of the words of which combinations need to be generated (default: 6)."));
        console.Verify(c => c.WriteLine("Press 'q' to quit, press ENTER to use the default option."));
        console.Verify(c => c.WriteLine("Shutting down..."));
    }

    [Fact]
    public void Use_Default_If_Nothing_Entered()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("You selected the default option: Y"));
        console.Verify(c => c.WriteLine("You selected the default option: 6"));
    }

    [Fact]
    public void Shuts_Down_If_Default_Then_q_Entered()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("q");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("Shutting down..."));
    }

    [Fact]
    public void Shuts_Down_If_q_Then_Number_Entered()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("q");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("4");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("Shutting down..."));
    }

    [Fact]
    public void Use_Default_If_Y_Entered()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("Y");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("You selected the default option: Y"));
    }

    [Fact]
    public void Use_Default_If_6_Entered()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("6");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("You selected the default option: 6"));
    }

    [Fact]
    public void Show_Combination_Input_Correctly()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("N");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("You selected the option: N"));
    }

    [Fact]
    public void Show_Length_Input_Correctly()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("4");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("You selected: 4"));
    }

    [Fact]
    public void Use_Input_For_Length_Correctly()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("4");

        menu.GenerateCombinations();

        userinput.Verify(u => u.CalculateCombinations(4), Times.Once());
    }

    [Fact]
    public void Handle_Letter_Input_For_Length()
    {
        var sequence = new MockSequence();
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("");
        console.InSequence(sequence).Setup(c => c.ReadLine()).Returns("a");

        menu.GenerateCombinations();

        console.Verify(c => c.WriteLine("Invalid choice."));
    }
}
