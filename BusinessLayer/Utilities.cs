using EP_HSRlearnIT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
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
