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

namespace CsvHelperMaui.Services
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
                        data.Add(new Employee { 
                            EmployeeID = csv.GetField<int>(0),
                            Person = new Person { 
                                FirstName = csv.GetField<string>(1), 
                                LastName = csv.GetField<string>(2) 
                            },
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

        public static void SaveRecords(List<Employee> records, string path)
        {
            using (var writer = new StreamWriter(path))
            {
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(records);
            }

        }
    }

    public sealed class EmployeeRecord
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TaxNumber { get; set; }
        public string IsWithThreshold { get; set; }
    }

    public sealed class CsvEmployeeMap : ClassMap<EmployeeRecord>
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
