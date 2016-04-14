using System;
using System.Globalization;
using System.Windows.Data;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    class FormFilledMulticonverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                {
                    return false;
                }
            }

            int ivLength = System.Convert.ToInt32(values[0]);
            int plaintextLength = System.Convert.ToInt32(values[1]);
            int keyLength = System.Convert.ToInt32(values[2]);

            return (plaintextLength > 0 && keyLength >= 8 && (ivLength == 0 || ivLength == 12));
        }

        //is not needed, but must be overriden
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
