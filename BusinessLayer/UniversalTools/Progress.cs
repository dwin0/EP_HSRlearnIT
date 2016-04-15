using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public static class Progress
    {
        #region Private Members
        private static Dictionary<object, object> _appProgress = new Dictionary<object, object>();

        #endregion

        #region Public Methods
        public static void SaveProgress(object key, object value)
        {
            if(_appProgress.ContainsKey(key))
            {
                _appProgress[key] = value;
            } else
            {
                _appProgress.Add(key, value);
            }
        }

        public static Dictionary<object, object> GetProgress()
        {
            return _appProgress;
        }

        public static object GetProgress(object key)
        {
            object retVal;
            _appProgress.TryGetValue(key, out retVal);
            return retVal;
        }

        public static void CleanProgress()
        {
            _appProgress.Clear();
        }

        #endregion

        }
}
