using Contracts.Authentification;
using System;

namespace BLL.Utils
{


    public class AuthentificationResult<T> : IAuthentificationResult<T>
    {
        
        public bool Succeeded { get; protected set; }
        public string Error { get; protected set; }

        public T Payload { get; protected set; }

        protected AuthentificationResult(bool success, T payload)
        {
            Succeeded = success;
            Payload = payload;
        }
        protected AuthentificationResult(string err)
        {
            Succeeded = false;
            Error = err;
        }

        public static AuthentificationResult<T> Success(T payload)
        {
            return new AuthentificationResult<T>(true, payload);
        }
        public static AuthentificationResult<T> Failed(string error = "Error")
        {
            return new AuthentificationResult<T>(error);
        }

    }

}

