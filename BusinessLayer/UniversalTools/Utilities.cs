
namespace EP_HSRlearnIT.BusinessLayer.UniversalTools
{
    public class Utilities
    {
        public FileSaver fileSaver;
        public Progress progress;
        public ExceptionLogger logger;

        public Utilities()
        {
            fileSaver = new FileSaver();
            progress = new Progress();
            logger = new ExceptionLogger();
        }

    }
}
