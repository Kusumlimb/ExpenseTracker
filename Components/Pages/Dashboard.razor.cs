using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using milestone2.Models;

namespace milestone2.Components.Pages
{
    public partial class Dashboard : ComponentBase
    {
        private AppData Data = new AppData();
        private Transaction newTransaction = new Transaction();
        private DateTime? filterStartDate = null;
        private DateTime? filterEndDate = null;

        protected override void OnInitialized()
        {
            // Initialize data using the UserService
            Data = UserService.LoadData();
        }

        // Recent Transactions
        private IEnumerable<Transaction> RecentTransactions()
        {
            return Data.Transactions.OrderByDescending(t => t.Date).Take(5);
        }

        // Recent Debts
        private IEnumerable<Debts> RecentDebts()
        {
            return Data.Debts.OrderByDescending(d => d.Date).Take(5);
        }

        // Total Debts
        private decimal TotalDebts()
        {
            return Data.Debts.Sum(d => d.Amount);
        }

        // Total Credits
        private decimal TotalCredits()
        {
            return Data.Transactions.Sum(t => t.Credit);
        }

        // Total Debits
        private decimal TotalDebits()
        {
            return Data.Transactions.Sum(t => t.Debit);
        }

        // Cleared Debts
        private decimal ClearedDebts()
        {
            return Data.Debts.Sum(d => d.PaidAmount);
        }

        // Remaining Debts
        private decimal RemainingDebts()
        {
            return Data.Debts.Sum(d => d.Amount - d.PaidAmount);
        }

        // Filtered Debts by Date Range
        private IEnumerable<Debts> FilteredDebts =>
            Data.Debts.Where(d =>
                (!filterStartDate.HasValue || d.Date >= filterStartDate.Value) &&
                (!filterEndDate.HasValue || d.Date <= filterEndDate.Value));

        // Total Transaction Count
        private int TotalTransactionCount()
        {
            return Data.Transactions.Count();
        }

        // Net Transaction Amount
        private decimal NetTransactionAmount()
        {
            var totalCredits = TotalCredits(); // inflows
            var totalDebits = TotalDebits();   // outflows
            var totalDebts = TotalDebts();     // debts
            return totalCredits + totalDebts - totalDebits;
        }

        // Top Five Highest Transactions
        public List<Transaction> GetTopFiveHighestTransactions()
        {
            return Data.Transactions
                       .OrderByDescending(t => t.Amount)
                       .Take(5)
                       .ToList();
        }

        // Top Five Lowest Transactions
        public List<Transaction> GetTopFiveLowestTransactions()
        {
            return Data.Transactions
                       .OrderBy(t => t.Amount)
                       .Take(5)
                       .ToList();
        }
    }
}
