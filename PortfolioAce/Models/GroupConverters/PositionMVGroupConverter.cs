using PortfolioAce.Domain.DataObjects;
using PortfolioAce.Domain.DataObjects.PositionData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace PortfolioAce.Models.GroupConverters
{
    public class PositionMVGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GroupItem groupItem = value as GroupItem;
            CollectionViewGroup collectionViewGroup = groupItem.Content as CollectionViewGroup;
            decimal sum = 0;

            foreach (var sec in collectionViewGroup.Items)
            {
                ValuedSecurityPosition position = sec as ValuedSecurityPosition;
                sum += position.MarketValueBase;
            }
            string sumString = sum.ToString("N2");
            return $"Total Market Value: {sumString}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
