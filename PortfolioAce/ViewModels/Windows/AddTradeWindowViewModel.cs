using PortfolioAce.Commands;
using PortfolioAce.EFCore.Repository;
using PortfolioAce.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PortfolioAce.ViewModels.Windows
{
    public class AddTradeWindowViewModel: ViewModelWindowBase
    {
        private int _fundId;

        public AddTradeWindowViewModel(ITradeRepository tradeRepo, int fundId)
        {
            AddTradeCommand = new AddTradeCommand(this, tradeRepo);
            _fundId = fundId;
            _tradeDate = DateTime.Today;
            _settleDate = DateTime.Today;
        }

        public int FundId
        {
            get
            {
                return _fundId;
            }
            private set
            {

            }
        }

        private string _tradeType;
        public string TradeType
        {
            get
            {
                return _tradeType;
            }
            set
            {
                _tradeType = value;
                OnPropertyChanged(nameof(TradeType));
            }
        }

        private string _symbol;
        public string Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
                // I can check the Database for the value,
                // and if it exists prefill the currency field
                // if not raise exception
                OnPropertyChanged(nameof(Symbol));
            }
        }

        private decimal _quantity;
        public decimal Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
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
                _price= value;
                OnPropertyChanged(nameof(Price));
                OnPropertyChanged(nameof(TradeAmount)); 
            }
        }

        private decimal _tradeAmount;
        public decimal TradeAmount
        {
            
            get
            {
                if (TradeType != "Corporate Action")
                {
                    int multiplier = -1;
                    _tradeAmount = Math.Round((Quantity * Price * multiplier) - Commission, 2);
                }

                return _tradeAmount;
            }
            set
            {
                if(TradeType == "Corporate Action")
                {
                    _tradeAmount = value;
                }
                OnPropertyChanged(nameof(TradeAmount));
            }
        }

        private string _tradeCurrency;
        public string TradeCurrency
        {
            get
            {
                return _tradeCurrency;
            }
            set
            {
                _tradeCurrency = value;
                OnPropertyChanged(nameof(TradeCurrency));
            }
        }

        private decimal _commission;
        public decimal Commission
        {
            get
            {
                return _commission;
            }
            set
            {
                _commission = value;
                OnPropertyChanged(nameof(Commission));
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



        public ICommand AddTradeCommand { get; set; }
    }
}
