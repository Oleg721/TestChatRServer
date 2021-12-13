using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authentification
{
    public interface IAuthentificationResult<T>
    {
        public T Payload { get; }
        public string Error { get; }
        public bool Succeeded { get; }
    }

    public interface IAuthentificationResult : IAuthentificationResult<bool>
    {    }
}
