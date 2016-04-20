using System;
using System.Globalization;
using System.Windows.Data;
using static System.Convert;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public class FormFilledEncryption : IMultiValueConverter
    {
        #region Private Members
        //since the hex-values are checked, the values need to be multiplied with 2
        private const int MinSizePlaintext = 0;
        private const int MinSizeKey = 8*2;
        private const int MinSizeOptionalIv = 0;
        private const int MaxSizeIv = 12*2;

        #endregion


        #region Public Methods
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var t in values)
            {
                if (t == null)
                {
                    return false;
                }
            }

            int ivLength = ToInt32(values[0]);
            int plaintextLength = ToInt32(values[1]);
            int keyLength = ToInt32(values[2]);
            
            return plaintextLength > MinSizePlaintext && keyLength >= MinSizeKey && (ivLength == MinSizeOptionalIv || ivLength == MaxSizeIv);
        }
        
        //is not needed, but must be overriden
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
