using BookManager.Data;
using BookManager.Services;
using BookManager.ViewModels;
using BookManager.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Windows;

namespace BookManager;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        var defaultConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<BookCollectionView>();
                services.AddTransient<IBooksRepository, BooksRepository>();
                services.AddTransient<IBookItemViewFactory, BookItemViewFactory>();
                services.AddDbContext<BookManagerContext>(options => options.UseSqlServer(defaultConnectionString));
            }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<BookCollectionView>();
        startupForm.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();

        base.OnExit(e);
    }
}
