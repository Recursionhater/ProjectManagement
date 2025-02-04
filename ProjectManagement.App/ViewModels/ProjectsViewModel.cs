using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManagement.App.Services;
using ProjectManagement.Shared.Dto.Employees;
using ProjectManagement.Shared.Dto.Projects;

namespace ProjectManagement.App.ViewModels;

public class ProjectsViewModel : ObservableObject
{
    private readonly IApiClient _apiClient;
    private bool _isWizardMode;
    private int _currentWizardStep;
    private string _projectName = "";
    private DateTime? _startDate;
    private DateTime? _endDate;
    private int _priority;
    private string _customerCompanyName = "";
    private string _contractorCompanyName = "";
    private EmployeeDto? _selectedOwner;
    private IReadOnlyCollection<ProjectDto> _projects = [];
    private string? _filterPropertyName;
    private string? _filterValue;
    private ProjectDto? _selectedProject;

    public ProjectsViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
        AddCommand = new RelayCommand(AddProject);
        CloseWizardCommand = new RelayCommand(CloseWizard);
        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        SearchCommand = new AsyncRelayCommand(SearchAsync, CanSearch);
        DeleteCommand = new AsyncRelayCommand(DeleteAsync, CanDelete);
        UpdateCommand = new AsyncRelayCommand<DataGridRowEditEndingEventArgs>(UpdateAsync);
    }

    public IReadOnlyDictionary<string, string> PropertyNames { get; } = new Dictionary<string, string>
    {
        [nameof(ProjectDto.Name)] = "Name",
        [nameof(ProjectDto.StartDate)] = "StartDate",
        [nameof(ProjectDto.EndDate)] = "EndDate",
        [nameof(ProjectDto.Priority)] = "Priority",
        [nameof(ProjectDto.CustomerCompanyName)] = "Customer company name",
        [nameof(ProjectDto.ContractorCompanyName)] = "Contractor company name",
        [nameof(ProjectDto.OwnerName)] = "Owner name",
    };

    public bool IsWizardMode
    {
        get => _isWizardMode;
        set => SetProperty(ref _isWizardMode, value);
    }

    public int CurrentWizardStep
    {
        get => _currentWizardStep;
        set => SetProperty(ref _currentWizardStep, value);
    }

    public string ProjectName
    {
        get => _projectName;
        set
        {
            SetProperty(ref _projectName, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            SetProperty(ref _startDate, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            SetProperty(ref _endDate, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public int Priority
    {
        get => _priority;
        set
        {
            SetProperty(ref _priority, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string CustomerCompanyName
    {
        get => _customerCompanyName;
        set
        {
            SetProperty(ref _customerCompanyName, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public string ContractorCompanyName
    {
        get => _contractorCompanyName;
        set
        {
            SetProperty(ref _contractorCompanyName, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public EmployeeDto? SelectedOwner
    {
        get => _selectedOwner;
        set
        {
            SetProperty(ref _selectedOwner, value);
            SaveCommand.NotifyCanExecuteChanged();
        }
    }

    public IReadOnlyCollection<ProjectDto> Projects
    {
        get => _projects;
        set => SetProperty(ref _projects, value);
    }

    public string? FilterPropertyName
    {
        get => _filterPropertyName;
        set
        {
            SetProperty(ref _filterPropertyName, value);
            SearchCommand.NotifyCanExecuteChanged();
        }
    }

    public string? FilterValue
    {
        get => _filterValue;
        set
        {
            SetProperty(ref _filterValue, value);
            SearchCommand.NotifyCanExecuteChanged();
        }
    }

    public ProjectDto? SelectedProject
    {
        get => _selectedProject;
        set
        {
            SetProperty(ref _selectedProject, value);
            DeleteCommand.NotifyCanExecuteChanged();
        }
    }

    public ICommand AddCommand { get; }

    public ICommand CloseWizardCommand { get; }

    public AsyncRelayCommand SaveCommand { get; }

    public AsyncRelayCommand SearchCommand { get; }

    public AsyncRelayCommand DeleteCommand { get; }

    public ICommand UpdateCommand { get; }

    public async Task InitializeAsync(CancellationToken token) =>
        Projects = await _apiClient.GetAllProjectsAsync(new GetAllProjectsDto(), token);

    private void AddProject() => IsWizardMode = true;

    private void CloseWizard()
    {
        IsWizardMode = false;
        CurrentWizardStep = 0;
        ProjectName = "";
        StartDate = null;
        EndDate = null;
        CustomerCompanyName = "";
        ContractorCompanyName = "";
        SelectedOwner = null;
    }

    private async Task SaveAsync(CancellationToken token)
    {
        if (SelectedOwner is null || StartDate is null || EndDate is null)
        {
            return;
        }

        var dto = new CreateProjectDto
        {
            Name = ProjectName,
            CustomerCompanyName = CustomerCompanyName,
            ContractorCompanyName = ContractorCompanyName,
            OwnerId = SelectedOwner.Id,
            StartDate = DateOnly.FromDateTime(StartDate.Value),
            EndDate = DateOnly.FromDateTime(EndDate.Value),
            Priority = Priority,
        };

        await _apiClient.CreateProjectAsync(dto, token);
        
        CloseWizard();

        await InitializeAsync(token);
    }

    private bool CanSave()
    {
        return !string.IsNullOrEmpty(ProjectName) &&
               StartDate is not null &&
               EndDate is not null &&
               Priority >= 0 &&
               SelectedOwner is not null &&
               !string.IsNullOrEmpty(CustomerCompanyName) &&
               !string.IsNullOrEmpty(ContractorCompanyName);
    }

    private bool CanSearch() => !string.IsNullOrEmpty(FilterPropertyName);

    private async Task SearchAsync(CancellationToken token)
    {
        var dto = new GetAllProjectsDto
        {
            FilterPropertyName = FilterPropertyName,
            FilterValue = FilterValue,
        };
        Projects = await _apiClient.GetAllProjectsAsync(dto, token);
    }

    private bool CanDelete() => SelectedProject is not null;

    private async Task DeleteAsync(CancellationToken token)
    {
        if (SelectedProject is not null)
        {
            await _apiClient.DeleteProjectAsync(SelectedProject.Id, token);
            Projects = Projects.Where(p => p.Id != SelectedProject.Id).ToList();
        }
    }

    private async Task UpdateAsync(DataGridRowEditEndingEventArgs? args, CancellationToken token)
    {
        if (args is null)
        {
            return;
        }
        
        var project = (ProjectDto)args.Row.Item ;
        
        var dto = new UpdateProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            CustomerCompanyName = project.CustomerCompanyName,
            ContractorCompanyName = project.ContractorCompanyName,
            OwnerId = project.OwnerId,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Priority = project.Priority
        };

        await _apiClient.UpdateProjectAsync(dto, token);
    }
}