using CoasterPayroll.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    /// <summary>
    /// A class representing a calculator for claiming without tax free threshold
    /// </summary>
    public class PayCalculatorNoThreshold : PayCalculator
    {
        public PayCalculatorNoThreshold()
        {
            List<TaxRate> taxRates = CsvTools.ImportTaxRates(@"C:\Users\dannn\OneDrive\Programming\C#\Coaster Payroll\CoasterPayroll\CoasterPayroll\taxrate-nothreshold.csv");
            TaxThresholds = taxRates.Select(taxRate => taxRate.UpperThreshold).ToArray();
            TaxRatesA = taxRates.Select(taxRate => taxRate.TaxRateA).ToArray();
            TaxRatesB = taxRates.Select(taxRate => taxRate.TaxRateB).ToArray();
        }
    }
}
