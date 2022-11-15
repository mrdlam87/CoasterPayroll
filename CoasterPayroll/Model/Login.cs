using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoasterPayroll.Model
{
    public class Login
    {
        /// <summary>
        /// Class representing login details
        /// </summary>
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
    }
}
