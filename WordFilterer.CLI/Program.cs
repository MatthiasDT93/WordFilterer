using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using WordFilterer.Core.Storage;
using WordFilterer.Core.UI;
using WordFilterer.CLI.FileStore;
using WordFilterer.CLI.UI;
using WordFilterer.CLI.Consoles;


BasicProgram.Run();

class BasicProgram
{
    public static void Run()
    {
        var projectDataDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data"));

        var inputDir = Path.Combine(projectDataDir, "Input");
        var outputDir = Path.Combine(projectDataDir, "Output");

        var services = new ServiceCollection();

        services.AddTransient<IFileStore>(fs => new FileStore(inputDir, outputDir));
        services.AddTransient<IStorage, Storage>();
        services.AddTransient<IConsole, SystemConsole>();
        services.AddTransient<IUserInput, UserInput>();
        services.AddTransient<IMenu, Menu>();

        var provider = services.BuildServiceProvider();
        var menu = provider.GetService<IMenu>();
        menu!.GenerateCombinations();
    }
}