using PortfolioAce.Commands;
using PortfolioAce.Domain.Models;
using PortfolioAce.EFCore.Services;
using PortfolioAce.ViewModels.Modals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Modals
{

    public class InvestorActionViewModel : ViewModelWindowBase
    {
        private ITransferAgencyService investorService;
        private Fund _fund;

        public InvestorActionViewModel(ITransferAgencyService investorService, Fund fund)
        {
            this.investorService = investorService;
            this._fund = fund;
            _tradeDate = DateTime.Today;
            _settleDate = DateTime.Today;
            //currency should be the funds base currency
            AddInvestorActionCommand = new AddInvestorActionCommand(this, investorService);
        }

        public int FundId
        {
            get
            {
                return _fund.FundId;
            }
            private set
            {

            }
        }

        private string _TAType;
        public string TAType
        {
            get
            {
                return _TAType;
            }
            set
            {
                _TAType = value;
                OnPropertyChanged(nameof(TAType));
            }
        }

        private string _InvestorName;
        public string InvestorName
        {
            get
            {
                return _InvestorName;
            }
            set
            {
                _InvestorName = value;
                // I can check the Database for the value,
                // and if it exists prefill the currency field
                // if not raise exception
                OnPropertyChanged(nameof(InvestorName));
            }
        }

        private decimal _units;
        public decimal Units
        {
            get
            {
                // these should be absolute.
                return _units;
            }
            set
            {
                _units = value;
                OnPropertyChanged(nameof(Units)); 
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        private decimal _price;
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        private decimal _tradeAmount;
        public decimal TradeAmount
        {

            get
            {
                // units should be absolute. multiplier should be based on sub/ red type.
                _tradeAmount = Math.Round((Units * Price) - Fee, 2);
                

                return _tradeAmount;
            }
            set
            {
                _tradeAmount = value;
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        
        public string Currency
        {
            get
            {
                return _fund.BaseCurrency;
            }
            private set
            {

            }
        }

        private decimal _fee;
        public decimal Fee
        {
            get
            {
                return _fee;
            }
            set
            {
                _fee = value;
                OnPropertyChanged(nameof(Fee));
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        private DateTime _tradeDate;
        public DateTime TradeDate
        {
            get
            {
                return _tradeDate;
            }
            set
            {
                _tradeDate = value;
                OnPropertyChanged(nameof(TradeDate));
            }
        }

        private DateTime _settleDate;
        public DateTime SettleDate
        {
            get
            {
                return _settleDate;
            }
            set
            {
                _settleDate = value;
                OnPropertyChanged(nameof(SettleDate));
            }
        }

        public ICommand AddInvestorActionCommand { get; set; }
    }
}
