using System;

namespace Syringe
{
    public class InjectionException : Exception
    {
        public InjectionException()
            : base()
        {

        }

        public InjectionException(string message)
            : base(message)
        {

        }

        public InjectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
