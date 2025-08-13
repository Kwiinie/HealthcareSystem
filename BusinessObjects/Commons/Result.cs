using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Commons
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public Result(T data)
        {
            Success = true;
            Data = data;
            ErrorMessage = null;
        }

        public Result(string errorMessage)
        {
            Success = false;
            Data = default(T); 
            ErrorMessage = errorMessage;
        }

        public static Result<T> SuccessResult(T data) => new Result<T>(data);
        public static Result<T> ErrorResult(string errorMessage) => new Result<T>(errorMessage);
    }

}
