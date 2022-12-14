using CoasterPayroll.Model;
using CoasterPayroll.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CoasterPayroll.ViewModel
{
    /// <summary>
    /// Class representing the main view model
    /// </summary>
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
        private string _showAll;
        private string _showDetail;
        private string _showEmployees;
        private string _showEmployee;
        #endregion

        #region Properties
        /// <summary>
        /// List for imported employee records
        /// </summary>
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

        /// <summary>
        /// Observable collection for filtered employee records
        /// </summary>
        public ObservableCollection<Employee> FilteredRecords
        {
            get
            {
                //Filter records based on string in search query
                var filteredRecords = EmployeeRecords.Where(record => (record.FirstName.ToLower() + record.LastName.ToLower()).Contains(Query.ToLower())).ToList();

                return new ObservableCollection<Employee>(filteredRecords);
            }
        }

        /// <summary>
        /// Observable collection for all available payslips
        /// </summary>
        public ObservableCollection<PaySlip> PaySlips
        {
            get => _paySlips;
            set
            {
                _paySlips = value;
                OnPropertyChanged(nameof(PaySlips));
            }
        }

        /// <summary>
        /// Observable collection for all available payslips of selected employee
        /// </summary>
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

        /// <summary>
        /// The employee currently selected from the data grid
        /// </summary>
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                //Update displayed payslips when selected employee changes
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
                OnPropertyChanged(nameof(DisplayedPaySlips));

                //Set selected payslip to employee's latest
                if (DisplayedPaySlips.Count > 0)
                {
                    SelectedPaySlip = DisplayedPaySlips.Last();
                }
            }
        }

        /// <summary>
        /// The payslip currently selected from the data grid
        /// </summary>
        public PaySlip SelectedPaySlip
        {
            get => _selectedPaySlip;
            set
            {
                _selectedPaySlip = value;
                OnPropertyChanged(nameof(SelectedPaySlip));
            }
        }

        /// <summary>
        /// String in the search query
        /// </summary>
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

        /// <summary>
        /// Input in hourly rate text field
        /// </summary>
        public string HourlyRateInput
        {
            get => _hourlyRateInput;
            set
            {
                _hourlyRateInput = value;
                OnPropertyChanged(nameof(HourlyRateInput));
            }
        }

        /// <summary>
        /// Input in week hours text field
        /// </summary>
        public string WeekHoursInput
        {
            get => _weekHoursInput;
            set
            {
                _weekHoursInput = value;
                OnPropertyChanged(nameof(WeekHoursInput));
            }
        }

        /// <summary>
        /// Status/flag for visibity of show all components
        /// </summary>
        public string ShowAllVisibility
        {
            get => _showAll;
            set
            {
                _showAll = value;
                OnPropertyChanged(nameof(ShowAllVisibility));
            }
        }

        /// <summary>
        /// Status/flag for visibity of detail components
        /// </summary>
        public string ShowDetailVisibility
        {
            get => _showDetail;
            set
            {
                _showDetail = value;
                OnPropertyChanged(nameof(ShowDetailVisibility));
            }
        }

        /// <summary>
        /// Status/flag for visibity of show all employees components
        /// </summary>
        public string ShowAllEmployeesVisibilty
        {
            get => _showEmployees;
            set
            {
                _showEmployees = value;
                OnPropertyChanged(nameof(ShowAllEmployeesVisibilty));
            }
        }

        /// <summary>
        /// Status/flag for visibity of show employee detail components
        /// </summary>
        public string ShowEmployeeVisibility
        {
            get => _showEmployee;
            set
            {
                _showEmployee = value;
                OnPropertyChanged(nameof(ShowEmployeeVisibility));
            }
        }

        /// <summary>
        /// Current Australian superannuation rate
        /// </summary>
        public static double SuperRate { get => 10.5; }

        /// <summary>
        /// Pay calculator object instance for calculating without tax free claimed
        /// </summary>
        public PayCalculatorNoThreshold PayCalculatorNoThreshold { get; set; }

        /// <summary>
        /// Pay calculator object instance for calculating with tax free claimed
        /// </summary>
        public PayCalculatorWithThreshold PayCalculatorWithThreshold { get; set; }

        #endregion

        #region ICommands
        public ICommand LoadCommand { get; }
        public ICommand CalculateCommand { get; }
        public ICommand SavePayslipCommand { get; }
        public ICommand SaveAllPayslipsCommand { get; }
        public ICommand ShowAllCommand { get; }
        public ICommand ShowDetailCommand { get; }
        public ICommand ShowEmployeeCommand { get; }
        public ICommand ShowAllEmployeesCommand { get; }
        #endregion

        /// <summary>
        /// Constructor for MainWindowViewModel
        /// </summary>
        /// 
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
            ShowAllCommand = new ViewModelCommand(ShowAll);
            ShowDetailCommand = new ViewModelCommand(ShowDetail);
            ShowEmployeeCommand = new ViewModelCommand(ShowEmployee);
            ShowAllEmployeesCommand = new ViewModelCommand(ShowAllEmployees);
            Query = "";
            ShowAllVisibility = "Hidden";
            ShowDetailVisibility = "Visible";
            ShowEmployeeVisibility = "Hidden";
            ShowAllEmployeesVisibilty = "Visible";
        }

        #region ICommand Methods
        /// <summary>
        /// Loads all employee records from CSV file
        /// </summary>
        /// <param name="obj"></param>
        private void LoadRecords(object obj)
        {
            EmployeeRecords = CsvTools.ImportEmployees(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\employee.csv");
        }

        /// <summary>
        /// Calculates and adds payslip based on week hours and hourly rate in text fields
        /// </summary>
        /// <param name="obj"></param>
        private void AddPaySlip(object obj)
        {
            bool weekHoursValid = int.TryParse(WeekHoursInput, out int weekHours);
            bool hourlyRateValid = int.TryParse(HourlyRateInput, out int hourlyRate);

            //Validate inputs
            if (SelectedEmployee is null)
            {
                MessageBox.Show("No employee selected.");
                return;
            } else if (!weekHoursValid || !hourlyRateValid)
            {
                MessageBox.Show("Invalid input type.");
                return;
            }

            //Assign calculator type based on tax free claimed
            PayCalculator PayCalcualtor = SelectedEmployee.IsWithThreshold ? PayCalculatorWithThreshold : PayCalculatorNoThreshold;

            SelectedPaySlip = new()
            {
                Employee = SelectedEmployee,
                SubmittedDate = DateTime.Now,
                WeekHours = weekHours,
                HourlyRate = hourlyRate,
                PayGrossCalculated = weekHours * hourlyRate,
                TaxCalculated = PayCalcualtor.CalculateTax(weekHours, hourlyRate),
                PayNetCalculated = PayCalcualtor.CalculatePay(weekHours, hourlyRate),
                SuperCalculated = PayCalcualtor.CalculateSuper(weekHours, hourlyRate, SuperRate),
            };

            PaySlips.Add(SelectedPaySlip);

            //Reset input fields
            HourlyRateInput = WeekHoursInput = "";
            OnPropertyChanged(nameof(DisplayedPaySlips));
        }

        /// <summary>
        /// Exports the currently selected payslip to CSV file
        /// </summary>
        /// <param name="obj"></param>
        public void SavePaySlip(object obj)
        {
            if (SelectedPaySlip is null)
                return;

            string employeeDetailHeader = $"{SelectedEmployee.EmployeeID}-{SelectedEmployee.FirstName}_{SelectedEmployee.LastName}";
            var fileName = $@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\Pay-{employeeDetailHeader}-{DateTime.Now.ToFileTime()}.csv";

            CsvTools.SavePaySlip(SelectedPaySlip, fileName);
            MessageBox.Show($"File saved at {fileName}");
        }

        /// <summary>
        /// Exports all available payslips for current selected employee to CSV file
        /// </summary>
        /// <param name="obj"></param>
        public void SaveAllPaySlips(object obj)
        {
            if (DisplayedPaySlips.Count == 0)
                return;

            string employeeDetailHeader = $"{SelectedEmployee.EmployeeID}-{SelectedEmployee.FirstName}_{SelectedEmployee.LastName}";
            var fileName = $@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\AllPays-{employeeDetailHeader}-{DateTime.Now.ToFileTime()}.csv";

            CsvTools.SavePaySlips(DisplayedPaySlips.ToList(), fileName);
            MessageBox.Show($"File saved at {fileName}");
        }

        /// <summary>
        /// Sets the show all visibility flag to visible
        /// </summary>
        /// <param name="obj"></param>
        public void ShowAll(object obj)
        {
            ShowAllVisibility = "Visible";
            ShowDetailVisibility = "Hidden";
        }

        /// <summary>
        /// Sets the detail flag to visible
        /// </summary>
        /// <param name="obj"></param>
        public void ShowDetail(object obj)
        {
            ShowAllVisibility = "Hidden";
            ShowDetailVisibility = "Visible";
        }

        /// <summary>
        /// Sets the show employee detail flag to visible
        /// </summary>
        /// <param name="obj"></param>
        public void ShowEmployee(object obj)
        {
            ShowEmployeeVisibility = "Visible";
            ShowAllEmployeesVisibilty = "Hidden";
        }

        /// <summary>
        /// Sets the show all employees flag to visible
        /// </summary>
        /// <param name="obj"></param>
        public void ShowAllEmployees(object obj)
        {
            ShowEmployeeVisibility = "Hidden";
            ShowAllEmployeesVisibilty = "Visible";
        }


        #endregion
    }
}
