using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.Services
{
    public abstract class OperationResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SuccessOperaationResult<T> : OperationResult<T>
    {
        public SuccessOperaationResult(T data)
        {
            Data = data;
            Success = true;
        }
    }

    public class FailedOperaationResult<T> : OperationResult<T>
    {
        public FailedOperaationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Success = false;
        }
    }


}
