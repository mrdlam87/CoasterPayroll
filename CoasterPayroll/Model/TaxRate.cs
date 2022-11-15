using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    /// <summary>
    /// Class representing tax rate data
    /// </summary>
    public class TaxRate
    {
        public int LowerThreshold { get; set; }
        public int UpperThreshold { get; set; }
        public double TaxRateA { get; set; }
        public double TaxRateB { get; set; }
    }
}
