using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    public class PaySlip
    {
        public Employee Employee { get; set; }
        public double HourlyRate { get; set; }
        public int WeekNumber { get; set; }
        public int WeekHours { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDate { get; set; }
        public double PayGrossCalculated { get; set; }
        public double TaxCalculated { get; set; }
        public double SuperCalculated { get; set; }
        public double PayNetCalculated { get; set; }

    }
}
