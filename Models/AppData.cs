using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace milestone2.Models
{
    public class AppData
    {
        public List<User> Users { get; set; } = new();

        public List<Debts> Debts { get; set; } = new();
        public List<Transaction> Transactions { get; set; } = new();
    }
}
