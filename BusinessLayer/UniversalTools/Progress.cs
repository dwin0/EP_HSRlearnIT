using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    /// <summary>
    /// Class for saving the progress
    /// </summary>
    public static class Progress
    {
        #region Private Members
        private static readonly Dictionary<object, object> AppProgress = new Dictionary<object, object>();

        #endregion

        /// <summary>
        /// Method to save a progress
        /// </summary>
        /// <param name="key">A unique object key</param>
        /// <param name="value">The progress object to be stored</param>
        #region Public Methods
        public static void SaveProgress(object key, object value)
        {
            if(AppProgress.ContainsKey(key))
            {
                AppProgress[key] = value;
            } else
            {
                AppProgress.Add(key, value);
            }
        }

        /// <summary>
        /// Method to get all the saved progresses
        /// </summary>
        /// <returns>Dictionary containing all progresses</returns>
        public static Dictionary<object, object> GetProgress()
        {
            return AppProgress;
        }

        /// <summary>
        /// Method to get a specific progress
        /// </summary>
        /// <param name="key">Unique key to search a progress</param>
        /// <returns>object = a progress was found. null = no progress was found</returns>
        public static object GetProgress(object key)
        {
            object retVal;
            AppProgress.TryGetValue(key, out retVal);
            return retVal;
        }

        /// <summary>
        /// Method to remove all saved progresses
        /// </summary>
        public static void CleanProgress()
        {
            AppProgress.Clear();
        }

        #endregion

        }
}
