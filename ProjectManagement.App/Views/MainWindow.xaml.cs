using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using ProjectManagement.App.ViewModels;

namespace ProjectManagement.App.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>();
    }
}