using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    /// <summary>
    /// Class representing a manager
    /// </summary>
    public class Manager : Employee
    {
        public bool Permissions { get; set; }
    }
}
