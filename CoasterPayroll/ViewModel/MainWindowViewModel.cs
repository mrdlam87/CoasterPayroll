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
        private Employee _selectedEmployee;
        private string _query;
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

        #endregion

        #region ICommands
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion

        //Constructor
        public MainWindowViewModel()
        {
            //List<Employee> data = CsvImporter.ImportRecords(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
            //EmployeeRecords = new ObservableCollection<Employee>(data);
            EmployeeRecords = new List<Employee>();
            LoadCommand = new ViewModelCommand(LoadRecords);
            Query = "";
        }

        #region Methods
        private void LoadRecords(object obj)
        {
            EmployeeRecords = CsvImporter.ImportRecords(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
        }
        #endregion
    }
}
