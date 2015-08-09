using System;

namespace Genetics
{
    public class SpliceException : Exception
    {
        public SpliceException()
            : base()
        {

        }

        public SpliceException(string message)
            : base(message)
        {

        }

        public SpliceException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
