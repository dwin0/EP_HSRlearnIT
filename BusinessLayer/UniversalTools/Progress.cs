using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class Progress
    {
        #region Private Members
        private static Dictionary<object, object> AppProgress = new Dictionary<object, object>();

        #endregion

        #region Public Methods
        public static void SaveProgress(Dictionary<object, object> progress)
        {
            AppProgress = progress;
        }

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

        public static Dictionary<object, object> GetProgress()
        {
            return AppProgress;
        }

        public static object GetProgress(object key)
        {
            object retVal;
            AppProgress.TryGetValue(key, out retVal);
            return retVal;
        }

        #endregion
    }
}
