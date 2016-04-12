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
                

            int plaintextLength = System.Convert.ToInt32(values[0]);
            int keyLength = System.Convert.ToInt32(values[1]);

            //if some text was entered AND the password is bigger than 8, return true, else return false.
            return (plaintextLength > 0 && keyLength >= 8);
        }

        //is not needed, but must be overriden
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
