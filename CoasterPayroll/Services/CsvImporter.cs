using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoasterPayroll.Model;
using System.IO;

namespace CoasterPayroll.Services
{
    public class CsvImporter
    {
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
            return data;
        }

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

    public sealed class EmployeeMap
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TaxNumber { get; set; }
        public string IsWithThreshold { get; set; }
    }

    public sealed class TaxRateMap
    {
        public int LowerThreshold { get; set; }
        public int UpperThreshold { get; set; }
        public double TaxRateA { get; set; }
        public double TaxRateB { get; set; }
    }

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
            Map(m => m.ID).Index(0);
            Map(m => m.FirstName).Index(1);
            Map(m => m.LastName).Index(2);
            Map(m => m.TaxNumber).Index(3);
            Map(m => m.IsWithThreshold).Index(4);
        }
    }

    public sealed class CsvTaxRateMap : ClassMap<TaxRateMap>
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
