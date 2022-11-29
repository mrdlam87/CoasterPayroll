using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CoasterPayroll.Model;
using System.IO;

namespace CoasterPayroll.Services
{
    /// <summary>
    /// Class for static Csv tools methods
    /// </summary>
    public class CsvTools
    {
        /// <summary>
        /// Imports CSV employee data
        /// </summary>
        /// <param name="fileName">String containing the file path</param>
        /// <returns>A List of employee records</returns>
        public static List<Employee> ImportEmployees(string fileName)
        {
            List<Employee> data = new();

            using (var reader = new StreamReader(fileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvEmployeeMap>();

                    while (csv.Read())
                    {
                        //Add manager instance if csv field is Manager
                        if (csv.GetField<string>(5) == "Manager")
                        {
                            data.Add(new Manager
                            {
                                EmployeeID = csv.GetField<int>(0),
                                FirstName = csv.GetField<string>(1),
                                LastName = csv.GetField<string>(2),
                                TaxNumber = csv.GetField<int>(3),
                                IsWithThreshold = csv.GetField<string>(4) == "Y" ? true : false,
                                HasPermissions = true,
                            });
                        }
                        else
                        {
                            data.Add(new Employee
                            {
                                EmployeeID = csv.GetField<int>(0),
                                FirstName = csv.GetField<string>(1),
                                LastName = csv.GetField<string>(2),
                                TaxNumber = csv.GetField<int>(3),
                                IsWithThreshold = csv.GetField<string>(4) == "Y" ? true : false,
                            });
                        }
                    }
                }

            }
            return data;
        }

        /// <summary>
        /// Imports CSV tax rate data
        /// </summary>
        /// <param name="fileName">The file path name</param>
        /// <returns>A List of tax rate data</returns>
        public static List<TaxRate> ImportTaxRates(string fileName)
        {
            List<TaxRate> data = new();

            using (var reader = new StreamReader(fileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvTaxRateMap>();

                    while (csv.Read())
                    {
                        data.Add(new TaxRate
                        {
                            LowerThreshold = csv.GetField<int>(0),
                            UpperThreshold = csv.GetField<int>(1),
                            TaxRateA = csv.GetField<double>(2),
                            TaxRateB = csv.GetField<double>(3),
                        });
                    }
                }

            }

            return data;
        }

        /// <summary>
        /// Exports selected payslip to CSV file
        /// </summary>
        /// <param name="paySlip">Selected payslip object</param>
        /// <param name="path">The file path name to export to</param>
        public static void SavePaySlip(PaySlip paySlip, string path)
        {
            PaySlipMap record = new()
            {
                EmployeeID = paySlip.Employee.EmployeeID,
                WeekHours = paySlip.WeekHours,
                HourlyRate = paySlip.HourlyRate,
                IsWithThreshold = paySlip.Employee.IsWithThreshold ? "Y" : "N",
                GrossPay = paySlip.PayGrossCalculated,
                Tax = paySlip.TaxCalculated,
                NetPay = paySlip.PayNetCalculated,
                Superannuation = paySlip.SuperCalculated,
            };

            using (var writer = new StreamWriter(path))
            {
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteHeader<PaySlipMap>();
                csv.NextRecord();
                csv.WriteRecord(record);
            }
        }

        /// <summary>
        /// Exports all the available payslips to CSV file
        /// </summary>
        /// <param name="paySlips">List of available payslips</param>
        /// <param name="path">The file path name to export to</param>
        public static void SavePaySlips(List<PaySlip> paySlips, string path)
        {
            List<PaySlipMap> records = paySlips.Select(paySlip => new PaySlipMap
            {
                EmployeeID = paySlip.Employee.EmployeeID,
                WeekHours = paySlip.WeekHours,
                HourlyRate = paySlip.HourlyRate,
                IsWithThreshold = paySlip.Employee.IsWithThreshold ? "Y" : "N",
                GrossPay = paySlip.PayGrossCalculated,
                Tax = paySlip.TaxCalculated,
                NetPay = paySlip.PayNetCalculated,
                Superannuation = paySlip.SuperCalculated,
            }).ToList();

            using (var writer = new StreamWriter(path))
            {
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(records);
            }
        }
    }

    /// <summary>
    /// A class representing employee map for Csv helper
    /// </summary>
    public sealed class EmployeeMap
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeID { get; set; }
        public int TaxNumber { get; set; }
        public bool IsWithThreshold { get; set; }
        public bool HasPermissions { get; set; }
    }

    /// <summary>
    /// A class representing paylsip map for Csv helper
    /// </summary>
    public sealed class PaySlipMap
    {
        public int EmployeeID { get; set; }
        public int WeekHours { get; set; }
        public double HourlyRate { get; set; }
        public string IsWithThreshold { get; set; }
        public double GrossPay { get; set; }
        public double Tax { get; set; }
        public double NetPay { get; set; }
        public double Superannuation { get; set; }
    }


    public sealed class CsvEmployeeMap : ClassMap<EmployeeMap>
    {
        public CsvEmployeeMap()
        {
            Map(m => m.EmployeeID).Index(0);
            Map(m => m.FirstName).Index(1);
            Map(m => m.LastName).Index(2);
            Map(m => m.TaxNumber).Index(3);
            Map(m => m.IsWithThreshold).Index(4);
            Map(m => m.HasPermissions).Index(5);
        }
    }

    public sealed class CsvTaxRateMap : ClassMap<TaxRate>
    {
        public CsvTaxRateMap()
        {
            Map(m => m.LowerThreshold).Index(0);
            Map(m => m.UpperThreshold).Index(1);
            Map(m => m.TaxRateA).Index(2);
            Map(m => m.TaxRateB).Index(3);
        }
    }
}
