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
        public static List<Employee> ImportRecords(string fileName)
        {
            List<Employee> data = new();

            using (var reader = new StreamReader(fileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvMapMap>();

                    int employeeID;
                    string firstName;
                    string lastName;
                    int taxNumber;

                    //Start Reading Csv File
                    //csv.Read();
                    //Skip Header
                    //csv.ReadHeader();

                    while (csv.Read())
                    {
                        employeeID = csv.GetField<int>(0);
                        firstName = csv.GetField<string>(1);
                        lastName = csv.GetField<string>(2);
                        taxNumber = csv.GetField<int>(3);
                        data.Add(new Employee { 
                            EmployeeID = employeeID,
                            Person = new Person { FirstName = firstName, LastName = lastName },
                            TaxNumber = taxNumber, 
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
    public sealed class CsvMapMap : ClassMap<Employee>
    {
        public CsvMapMap()
        {
            Map(m => m.EmployeeID).Index(0);
            Map(m => m.Person.FirstName).Index(1);
            Map(m => m.Person.LastName).Index(2);
            Map(m => m.TaxNumber).Index(3);

        }
    }
}
