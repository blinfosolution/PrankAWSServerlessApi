using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSServerlessApi.Exceptions
{
    public class DbProcException : Exception
    {
        public DbProcException(string message) : base(message)
        {
        }
    }
}
