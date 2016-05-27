using System.Collections.Generic;
using System.Linq;

namespace EP_HSRlearnIT.BusinessLayer.Persistence
{
    /// <summary>
    /// Class for saving the progress
    /// </summary>
    public static class Progress
    {
        #region Private Members
        private static readonly Dictionary<object, object> AppProgress = new Dictionary<object, object>();

        #endregion


        #region Public Methods

        /// <summary>
        /// Method to save a progress
        /// </summary>
        /// <param name="key">A unique object key</param>
        /// <param name="value">The progress object to be saved</param>
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
        /// Method to get all saved progresses
        /// </summary>
        /// <returns>Dictionary which contains all saved progresses</returns>
        public static Dictionary<object, object> GetProgress()
        {
            return AppProgress.ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        /// <summary>
        /// Method to get a specific progress
        /// </summary>
        /// <param name="key">Unique key to search a progress</param>
        /// <returns>object = a progress was found. null = no progress was found</returns>
        public static object GetProgress(object key)
        {
            object returnValue;
            AppProgress.TryGetValue(key, out returnValue);
            return returnValue;
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
