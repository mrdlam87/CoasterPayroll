using CoasterPayroll.Model;
using CsvHelperMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CoasterPayroll.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        private List<Employee> _employeeRecords;
        private ObservableCollection<PaySlip> _paySlips;
        private Employee _selectedEmployee;
        private PaySlip _selectedPaySlip;
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

        public ObservableCollection<PaySlip> DisplayedPaySlips
        {
            get
            {
                if (SelectedEmployee is null)
                    return new ObservableCollection<PaySlip>();

                var filteredPayslips = PaySlips.Where(paySlip => paySlip.Employee.EmployeeID == SelectedEmployee.EmployeeID).ToList();

                return new ObservableCollection<PaySlip>(filteredPayslips);
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                OnPropertyChanged(nameof(DisplayedPaySlips));
            }
        }

        public PaySlip SelectedPaySlip
        {
            get => _selectedPaySlip;
            set
            {
                _selectedPaySlip = value;
                OnPropertyChanged(nameof(SelectedPaySlip));
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

        public double SuperRate { get => 10.5; }

        public PayCalculatorNoThreshold PayCalculatorNoThreshold { get; set; }
        public PayCalculatorWithThreshold PayCalculatorWithThreshold { get; set; }

        #endregion

        #region ICommands
        public ICommand LoadCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand SavePayslipCommand { get; }
        public ICommand SaveAllPayslipsCommand { get; }
        #endregion

        //Constructor
        public MainWindowViewModel()
        {
            //EmployeeRecords = CsvImporter.ImportEmployees(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
            EmployeeRecords = new List<Employee>();
            PaySlips = new ObservableCollection<PaySlip>();
            PayCalculatorNoThreshold = new();
            PayCalculatorWithThreshold = new();
            LoadCommand = new ViewModelCommand(LoadRecords);
            CalculateCommand = new ViewModelCommand(AddPaySlip);
            SavePayslipCommand = new ViewModelCommand(SavePaySlip);
            SaveAllPayslipsCommand = new ViewModelCommand(SaveAllPaySlips);
            Query = "";
        }

        #region Methods
        private void LoadRecords(object obj)
        {
            EmployeeRecords = CsvImporter.ImportEmployees(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
            
            if (EmployeeRecords.Count > 0)
            {
                MessageBox.Show("All employee records have been loaded.");
            }
        }

        private void AddPaySlip(object obj)
        {
            if (SelectedEmployee is null)
                return;

            PayCalculator PayCalcualtor = SelectedEmployee.IsWithThreshold ? PayCalculatorWithThreshold : PayCalculatorNoThreshold;
            int weekHours = int.Parse(WeekHoursInput);
            double hourlyRate = double.Parse(HourlyRateInput);

            PaySlips.Add(new()
            {
                Employee = SelectedEmployee,
                SubmittedDate = DateTime.Now,
                WeekHours = weekHours,
                HourlyRate = hourlyRate,
                PayGrossCalculated = weekHours * hourlyRate,
                TaxCalculated = PayCalcualtor.CalculateTax(weekHours, hourlyRate),
                PayNetCalculated = PayCalcualtor.CalculatePay(weekHours, hourlyRate),
                SuperCalculated = PayCalcualtor.CalculateSuper(weekHours, hourlyRate, SuperRate),
            });

            HourlyRateInput = WeekHoursInput = "";
            OnPropertyChanged(nameof(DisplayedPaySlips));
        }



        public void SavePaySlip(object obj)
        {
            if (SelectedPaySlip is null)
                return;

            string employeeDetailHeader = $"{SelectedEmployee.EmployeeID}-{SelectedEmployee.Person.FirstName}_{SelectedEmployee.Person.LastName}";
            var fileName = $@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\Pay-{employeeDetailHeader}-{DateTime.Now.ToFileTime()}.csv";

            CsvImporter.SavePaySlip(SelectedPaySlip, fileName);
        }

        public void SaveAllPaySlips(object obj)
        {
            if (DisplayedPaySlips.Count == 0)
                return;

            string employeeDetailHeader = $"{SelectedEmployee.EmployeeID}-{SelectedEmployee.Person.FirstName}_{SelectedEmployee.Person.LastName}";
            var fileName = $@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\AllPays-{employeeDetailHeader}-{DateTime.Now.ToFileTime()}.csv";

            CsvImporter.SavePaySlips(DisplayedPaySlips.ToList(), fileName);
        }
        #endregion
    }
}
