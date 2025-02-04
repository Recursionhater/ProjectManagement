using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.App.Services;
using ProjectManagement.App.ViewModels;
using Refit;

namespace ProjectManagement.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;
        Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
        Application.Current.Dispatcher.UnhandledExceptionFilter += OnFilterDispatcherException;
        TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

        var services = new ServiceCollection();

        services.AddRefitClient<IApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://localhost:5078"));

        services
            .AddTransient<ProjectsViewModel>()
            .AddTransient<EmployeesViewModel>()
            .AddTransient<MainWindowViewModel>();

        Ioc.Default.ConfigureServices(services.BuildServiceProvider());
    }

    private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        ShowError(e.Exception.ToString(), "OnUnobservedTaskException");
    }

    private void OnFilterDispatcherException(object sender, DispatcherUnhandledExceptionFilterEventArgs e)
    {

        ShowError(e.Exception.ToString(), "OnFilterDispatcherException");
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        ShowError(e.Exception.ToString(), "OnDispatcherUnhandledException");
    }

    private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowError(((Exception)e.ExceptionObject).ToString(), "OnAppDomainUnhandledException");
    }

    private void ShowError(string text, string title)
    {
        MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }
}