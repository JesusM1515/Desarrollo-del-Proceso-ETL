using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Base
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public OperationResult(bool isSuccess, string message, T? data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static OperationResult<T> Success(string message, T? data)
        {
            return new OperationResult<T>(true, message, data);
        }

        public static OperationResult<T> Failure (string message)
        {
            return new OperationResult<T>(false, message, default);
        }
    }
}
