using CsvHelperMaui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    public class PayCalculatorWithThreshold : PayCalculator
    {
        public PayCalculatorWithThreshold()
        {
            List<TaxRate> taxRates = CsvImporter.ImportTaxRates(@"C:\\Users\\dannn\\OneDrive\\Programming\\C#\\Coaster Payroll\\CoasterPayroll\\CoasterPayroll\\taxrate-withthreshold.csv");
            TaxThresholds = taxRates.Select(taxRate => taxRate.UpperThreshold).ToArray();
            TaxRatesA = taxRates.Select(taxRate => taxRate.TaxRateA).ToArray();
            TaxRatesB = taxRates.Select(taxRate => taxRate.TaxRateB).ToArray();
        }
    }
}
