using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class ResultViewModel<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public bool Authorized { get; set; }
        public object Errors { get; set; }

        public ResultViewModel<T> CreateError(string message)
        {
            return new ResultViewModel<T>() {  Message = message, Success = false };
        }
       
        public ResultViewModel()
        {
            Success = true;
            Message = string.Empty;
            Authorized = true;
        }

        public ResultViewModel(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Authorized = true;
        }

        public ResultViewModel(T data, string message = "", bool success = true, bool authorized = true)
        {
            Data = data;
            Success = success;
            Message = message;
            Authorized = authorized;
        }
    }
}
