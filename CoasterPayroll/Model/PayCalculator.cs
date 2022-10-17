using CsvHelperMaui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    public class PayCalculator
    {
        public PayCalculator()
        {
            List<TaxRate> taxRates = CsvImporter.ImportTaxRates(@"C:\\Users\\dannn\\OneDrive\\Programming\\C#\\Coaster Payroll\\CoasterPayroll\\CoasterPayroll\\taxrate-withthreshold.csv");
            TaxThresholds = taxRates.Select(taxRate => taxRate.UpperThreshold).ToArray();
            TaxRatesA = taxRates.Select(taxRate => taxRate.TaxRateA).ToArray();
            TaxRatesB = taxRates.Select(taxRate => taxRate.TaxRateB).ToArray();
        }

        public int[] TaxThresholds { get; set; }
        public double[] TaxRatesA { get; set; }
        public double[] TaxRatesB { get; set; }
        public double CalculatePay(int weekHours, double hourlyRate)
        {
            return weekHours * hourlyRate - CalculateTax(weekHours,hourlyRate);
        }
        public double CalculateTax(int weekHours, double hourlyRate)
        {
            double grossPay = weekHours * hourlyRate;
            int taxRateIndex = 0;

            while (taxRateIndex < TaxThresholds.Length && grossPay > TaxThresholds[taxRateIndex])
            {
                taxRateIndex++;
            }

            return TaxRatesA[taxRateIndex] * grossPay - TaxRatesB[taxRateIndex];
        }
        public double CalculateSuper(int weekHours, double hourlyRate, double superRate)
        {
            return weekHours * hourlyRate * superRate / 100;
        }
    }
}
