﻿namespace Gu.Wpf.Adorners.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class IsNotFocusedAndEmptyToTrueConverter : IMultiValueConverter
    {
        public static readonly IsNotFocusedAndEmptyToTrueConverter Default = new IsNotFocusedAndEmptyToTrueConverter();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Equals(values[0], false) && Equals(values[1], true);
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
