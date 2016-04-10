using System.Collections.Generic;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public class Progress
    {
        private Dictionary<object, object> AppProgress { get; set; }

        public Progress()
        {
            AppProgress = new Dictionary<object, object>();
        }

        public void SaveProgress(Dictionary<object, object> progress)
        {
            AppProgress = progress;
        }

        public void SaveProgress(object key, object value)
        {
            if(AppProgress.ContainsKey(key) == true)
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
            object retVal = null;
            AppProgress.TryGetValue(key, out retVal);
            return retVal;
        }
    }
}
