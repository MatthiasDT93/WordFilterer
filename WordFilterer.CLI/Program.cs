using System.Security.Authentication.ExtendedProtection;
using Microsoft.Extensions.DependencyInjection;
using WordFilterer.Core;
using WordFilterer.Core.Storage;
//using WordFilterer.CLI;


BasicProgram.Run();

class BasicProgram
{
    public static void Run()
    {
        var services = new ServiceCollection();

        services.AddTransient<IFileStore, FileStore>();
        services.AddTransient<IStorage, Storage>();



        var provider = services.BuildServiceProvider();
    }
}