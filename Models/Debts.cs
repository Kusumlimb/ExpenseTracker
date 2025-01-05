using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace milestone2.Models
{
    public class Debts
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Link debt to a user

        public decimal Amount { get; set; } // Total debt amount
        public decimal PaidAmount { get; set; } // Amount paid towards debt
        public DateTime Date { get; set; }
        public string Description { get; set; }

    }
}
