using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.Shared.Dto.Employees;

namespace ProjectManagement.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private IReadOnlyCollection<EmployeeDto> _employees = [];

    public MainWindowViewModel(ProjectsViewModel projectsViewModel, EmployeesViewModel employeesViewModel)
    {
        ProjectsViewModel = projectsViewModel;
        EmployeesViewModel = employeesViewModel;
        InitializeCommand = new AsyncRelayCommand(InitializeAsync);
    }

    public IReadOnlyCollection<EmployeeDto> Employees
    {
        get => _employees;
        set => SetProperty(ref _employees, value);
    }

    public ICommand InitializeCommand { get; }

    public ProjectsViewModel ProjectsViewModel { get; }

    public EmployeesViewModel EmployeesViewModel { get; }

    private async Task InitializeAsync(CancellationToken token)
    {
        await Task.WhenAll(
            ProjectsViewModel.InitializeAsync(token),
            EmployeesViewModel.InitializeAsync(token));
    }
}