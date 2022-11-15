using CoasterPayroll.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    public class PayCalculator
    {
        /// <summary>
        /// Class representing a pay calculator
        /// </summary>
        public PayCalculator()
        {
            List<TaxRate> taxRates = CsvTools.ImportTaxRates(@"C:\\Users\\dannn\\OneDrive\\Programming\\C#\\Coaster Payroll\\CoasterPayroll\\CoasterPayroll\\taxrate-withthreshold.csv");
            TaxThresholds = taxRates.Select(taxRate => taxRate.UpperThreshold).ToArray();
            TaxRatesA = taxRates.Select(taxRate => taxRate.TaxRateA).ToArray();
            TaxRatesB = taxRates.Select(taxRate => taxRate.TaxRateB).ToArray();
        }

        public int[] TaxThresholds { get; set; }
        public double[] TaxRatesA { get; set; }
        public double[] TaxRatesB { get; set; }

        /// <summary>
        /// Calculates the net pay for the week
        /// </summary>
        /// <param name="weekHours">Hours worked for the week</param>
        /// <param name="hourlyRate">Hourly rate for current payslip</param>
        /// <returns>A double containing the net pay for the week</returns>
        public double CalculatePay(int weekHours, double hourlyRate)
        {
            return weekHours * hourlyRate - CalculateTax(weekHours,hourlyRate);
        }

        /// <summary>
        /// Calculates the total tax to be deducted for the week
        /// </summary>
        /// <param name="weekHours">Hours worked for the week</param>
        /// <param name="hourlyRate">Hourly rate for current payslip</param>
        /// <returns>A double containing the total tax to be deducted for the week</returns>
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

        /// <summary>
        /// Calculates the total superannuation to pay for the week
        /// </summary>
        /// <param name="weekHours">Hours worked for the week</param>
        /// <param name="hourlyRate">Hourly rate for current payslip</param>
        /// <param name="superRate">Current superannuation rate</param>
        /// <returns>A double containing the total superaanuation to pay for the week</returns>
        public double CalculateSuper(int weekHours, double hourlyRate, double superRate)
        {
            return weekHours * hourlyRate * superRate / 100;
        }
    }
}
