using System;

namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public class Utilities
    {
        #region Public Members
        
        public Progress Progress;
        
        public string FolderPath;
        public string FileName;

        #endregion

        #region Public Methods
        public Utilities()
        {
            
            Progress = new Progress();
            
        }

        #endregion
    }
}
