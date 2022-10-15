using CoasterPayroll.Model;
using CsvHelperMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CoasterPayroll.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private List<Employee> _employeeRecords;
        private ObservableCollection<PaySlip> _paySlips;
        private Employee _selectedEmployee;
        private string _query;
        private string _hourlyRateInput;
        private string _weekHoursInput;
        #endregion

        #region Properties
        public List<Employee> EmployeeRecords
        {
            get => _employeeRecords;
            set
            {
                _employeeRecords = value;
                OnPropertyChanged(nameof(EmployeeRecords));
                OnPropertyChanged(nameof(FilteredRecords));
            }
        }

        public ObservableCollection<Employee> FilteredRecords
        {
            get
            {
                var filteredRecords = EmployeeRecords.Where(record => (record.Person.FirstName.ToLower() + record.Person.LastName.ToLower()).Contains(Query.ToLower())).ToList();

                return new ObservableCollection<Employee>(filteredRecords);
            }
        }

        public ObservableCollection<PaySlip> PaySlips
        {
            get => _paySlips;
            set
            {
                _paySlips = value;
                OnPropertyChanged(nameof(PaySlips));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
                OnPropertyChanged(nameof(FilteredRecords));
            }
        }

        public string HourlyRateInput 
        { 
            get => _hourlyRateInput;
            set 
            { 
                _hourlyRateInput = value;
                OnPropertyChanged(nameof(HourlyRateInput));
            }
        }
        public string WeekHoursInput 
        { 
            get => _weekHoursInput; 
            set
            {
                _weekHoursInput = value;
                OnPropertyChanged(nameof(WeekHoursInput));
            }
        }

        #endregion

        #region ICommands
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CalculateCommand { get; }

        #endregion

        //Constructor
        public MainWindowViewModel()
        {
            EmployeeRecords = CsvImporter.ImportRecords(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
            EmployeeRecords = new List<Employee>();
            LoadCommand = new ViewModelCommand(LoadRecords);
            CalculateCommand = new ViewModelCommand(AddPaySlip);
            Query = "";
            PaySlips = new ObservableCollection<PaySlip>();
        }

        #region Methods
        private void LoadRecords(object obj)
        {
            EmployeeRecords = CsvImporter.ImportRecords(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
        }

        private void AddPaySlip(object obj)
        {
            if (SelectedEmployee is null)
                return;

            PaySlips.Add(new()
            {
                Employee = SelectedEmployee,
                WeekHours = int.Parse(WeekHoursInput),
                PayGrossCalculated = int.Parse(WeekHoursInput) * double.Parse(HourlyRateInput),
            });

            HourlyRateInput = WeekHoursInput = "";
        }
        #endregion
    }
}
