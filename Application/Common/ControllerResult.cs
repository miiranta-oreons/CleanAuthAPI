using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class ControllerResult<T>
    {
        public bool Success { get; init; }

        public T? Result { get; init; }

        public string? Message { get; init; }
    }

    public static class ControllerResultBuilder
    {
        public static ControllerResult<T> Resolve<T>(T result)
        {
            return new ControllerResult<T>
            {
                Result = result,
                Success = true
            };
        }

        public static ControllerResult<T> Reject<T>(string message)
        {
            return new ControllerResult<T>
            {
                Message = message,
                Success = false
            };
        }
    }
}
