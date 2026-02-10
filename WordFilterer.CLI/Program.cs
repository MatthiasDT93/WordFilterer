using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using WordFilterer.Core;
using WordFilterer.Core.Storage;
using WordFilterer.CLI.FileStore;
using WordFilterer.Core.UI;
using WordFilterer.CLI.Consoles;


BasicProgram.Run();

class BasicProgram
{
    public static void Run()
    {
        var baseDir = AppContext.BaseDirectory;

        var inputDir = Path.Combine(baseDir, "Data", "Input");
        var outputDir = Path.Combine(baseDir, "Data", "Output");

        var services = new ServiceCollection();

        services.AddTransient<IFileStore>(fs => new FileStore(inputDir, outputDir));
        services.AddTransient<IStorage, Storage>();
        services.AddTransient<IConsole, SystemConsole>();
        services.AddTransient<IUserInput, UserInput>();
        services.AddTransient<IMenu, Menu>();

        var provider = services.BuildServiceProvider();
    }
}