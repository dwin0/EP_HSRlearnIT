using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public class Progress
    {
        #region Private Members
        private Dictionary<object, object> AppProgress { get; set; }

        #endregion

        #region Constructors
        public Progress()
        {
            AppProgress = new Dictionary<object, object>();
        }

        #endregion

        #region Public Methods
        public void SaveProgress(Dictionary<object, object> progress)
        {
            AppProgress = progress;
        }

        public void SaveProgress(object key, object value)
        {
            if(AppProgress.ContainsKey(key))
            {
                AppProgress[key] = value;
            } else
            {
                AppProgress.Add(key, value);
            }
        }

        public Dictionary<object, object> GetProgress()
        {
            return AppProgress;
        }

        public object GetProgress(object key)
        {
            return AppProgress[key];
        }

        public object TryGetProgress(object key)
        {
            object retVal;
            AppProgress.TryGetValue(key, out retVal);
            return retVal;
        }

        #endregion
    }
}
