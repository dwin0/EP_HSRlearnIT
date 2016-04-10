
namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public class Utilities
    {
        #region Public Members
        public FileSaver FileSaver;
        public Progress Progress;
        public ExceptionLogger ExLogger;

        #endregion

        #region public Methods
        public Utilities()
        {
            FileSaver = new FileSaver();
            Progress = new Progress();
            ExLogger = new ExceptionLogger();
        }

        #endregion
    }
}
