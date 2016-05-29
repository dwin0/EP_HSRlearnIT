using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using static System.Convert;

namespace EP_HSRlearnIT.PresentationLayer.Exercises
{
    public class FormFilledDecryption : IMultiValueConverter
    {
        #region Private Members
        //since the hex-values are checked, the values need to be multiplied with 2
        private const int MinSizeCiphertext = 0;
        private const int MinSizeAad = 0;
        private const int MinSizeOptionalIv = 0;
        private const int MaxSizeIv = 12 * 2;
        private const int SizeTag = 16*2;
        private const int MinSizeKey = 8 * 2;
        #endregion

        #region Public Methods
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(t => t == null))
            {
                return false;
            }

            int ciphertextLength = ToInt32(values[0]);
            int ivLength = ToInt32(values[1]);
            int tagLength = ToInt32(values[2]);
            int keyLength = ToInt32(values[3]);
            int aadLength = ToInt32(values[4]);

            return (ciphertextLength > MinSizeCiphertext || aadLength > MinSizeAad) && 
                   keyLength >= MinSizeKey && 
                   (ivLength == MinSizeOptionalIv || ivLength == MaxSizeIv) && 
                   tagLength == SizeTag;
        }

        //is not needed, but must be overriden
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
