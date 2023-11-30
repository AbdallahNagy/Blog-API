using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.BL.Exception_Handling;

public class BusinessException : Exception
{
    public int StatusCode { get; }
    public BusinessException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
