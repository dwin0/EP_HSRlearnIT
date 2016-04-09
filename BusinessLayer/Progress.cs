using System.Collections.Generic;

namespace BusinessLayer
{
    public class Progress
    {
        private Dictionary<object, object> AppProgress { get; set; }

        public Progress()
        {
            AppProgress = new Dictionary<object, object>();
        }

        public void saveProgress(Dictionary<object, object> progress)
        {
            AppProgress = progress;
        }

        public void saveProgress(object key, object value)
        {
            if(AppProgress.ContainsKey(key) == true)
            {
                AppProgress[key] = value;
            } else
            {
                AppProgress.Add(key, value);
            }
        }

        public Dictionary<object, object> getProgress()
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
