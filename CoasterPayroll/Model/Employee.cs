﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CoasterPayroll.Model
{
    public class Employee : Person
    {
        public int EmployeeID { get; set; }
        public int TaxNumber { get; set; }
        public bool IsWithThreshold { get; set; }
    }
}
