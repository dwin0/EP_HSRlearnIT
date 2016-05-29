using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayer.Extensions
{
    /// <summary>
    /// Extension Class for strings
    /// </summary>
    public static class StringExtension
    {
        #region Public Methods
        /// <summary>
        /// Extension Method to replace multiple strings within a text
        /// </summary>
        /// <param name="text">Text in which replacements are made</param>
        /// <param name="replacements">Dictionary which contains a string-string - Pair.
        /// First string: Pattern searched in text - Second string: Text to replace the pattern</param>
        /// <returns>Text in which all found patterns are replaced with a given string</returns>
        public static string MultipleReplace(this string text, Dictionary<string, string> replacements)
        {
            string returnValue = text;
            foreach (string textToReplace in replacements.Keys)
            {
                returnValue = returnValue.Replace(textToReplace, replacements[textToReplace]);
            }
            return returnValue;
        }

        #endregion
    }
}
