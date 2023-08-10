using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Exceptions
{
    public class NotFailLoginException : Exception
    {
        public NotFailLoginException() : base("Hatalı giriş yapıldı")
        {
        }

        public NotFailLoginException(string? message) : base(message)
        {
        }

        public NotFailLoginException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
