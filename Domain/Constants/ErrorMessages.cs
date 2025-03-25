using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class ErrorMessages
    {
        public const string UserNotFound = "User not found";
        public const string UserAlreadyExists = "User already exists";
        public const string InvalidCredentials = "Invalid credentials";
        public const string InvalidTokenOrUserId = "Invalid token or user id";

    }
}
