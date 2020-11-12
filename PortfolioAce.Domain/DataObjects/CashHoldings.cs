﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.Domain.DataObjects
{
    // I will add more info to these classes these are temporary for now.
    public class CashHoldings
    {
        private List<CashAccountBalance> _accountBalances;
        public CashHoldings()
        {
            _accountBalances = new List<CashAccountBalance>();
        }
        
        public void Add(CashAccountBalance cashAccount)
        {
            _accountBalances.Add(cashAccount);
        }

        public List<CashAccountBalance> GetCashBalances()
        {
            return _accountBalances;
        }
    }

    public class CashAccountBalance
    {
        public string Name { get; set; } // currency name? different accounts? i.e USD EXPENSE a/c etc
        public decimal Balance { get; set; }
        public CashAccountBalance(string name, decimal balance)
        {
            this.Name = name;
            this.Balance = balance;
        }
    }
}
