using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.App.Services;
using ProjectManagement.Shared.Dto.Employees;

namespace ProjectManagement.App.ViewModels;

public class EmployeesViewModel : ObservableObject
{
    private IReadOnlyCollection<EmployeeDto> _employees = [];
    private readonly IApiClient _apiClient;
    private EmployeeDto? _selectedEmployee;

    public EmployeesViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
        UpdateCommand = new AsyncRelayCommand(UpdateAsync);
    }

    public IReadOnlyCollection<EmployeeDto> Employees
    {
        get => _employees;
        set => SetProperty(ref _employees, value);
    }

    public EmployeeDto? SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            SetProperty(ref _selectedEmployee, value);
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public AsyncRelayCommand DeleteCommand { get; }

    public ICommand UpdateCommand { get; }

    public async Task InitializeAsync(CancellationToken token) =>
        Employees = await _apiClient.GetAllEmployeesAsync(token);

    private bool CanDelete() => SelectedEmployee is not null;

    private async Task DeleteAsync(CancellationToken token)
    {
        if (SelectedEmployee is not null)
        {
            await _apiClient.DeleteEmployeeAsync(SelectedEmployee.Id, token);
            Employees = Employees.Where(e => e.Id != SelectedEmployee.Id).ToList();
        }
    }

    private async Task UpdateAsync(CancellationToken token)
    {
        if (SelectedEmployee is null)
        {
            return;
        }

        var dto = new UpdateEmployeeDto
        {
            Id = SelectedEmployee.Id,
            FirstName = SelectedEmployee.FirstName,
            LastName = SelectedEmployee.LastName,
            Email = SelectedEmployee.Email,
            MiddleName = SelectedEmployee.MiddleName,
        };

        await _apiClient.UpdateEmployeeAsync(dto, token);
    }
}