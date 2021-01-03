using PortfolioAce.Domain.DataObjects;
using PortfolioAce.EFCore.Services.DimensionServices;
using PortfolioAce.EFCore.Services.PriceServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioAce.ViewModels.Modals
{
    public class PositionDetailWindowViewModel: ViewModelWindowBase
    {

        // I need the actual position, take the static references)
        private IStaticReferences _staticReferences;
        private IPriceService _priceService;
        
        public PositionDetailWindowViewModel(IPriceService priceService, IStaticReferences staticReferences,
           SecurityPositionValuation valuedPosition)
        {
            _staticReferences = staticReferences;
            _priceService = priceService;
            _valuedPosition = valuedPosition;
            _PositionOpenLots = _valuedPosition.Position.GetOpenLots();
        }

        private SecurityPositionValuation _valuedPosition;
        public SecurityPositionValuation TargetPosition
        {
            get
            {
                return _valuedPosition;
            }
        }

        private List<OpenLots> _PositionOpenLots;
        public List<OpenLots> PositionOpenLots
        {
            get
            {
                return _PositionOpenLots;
            }
        }
    }
}
