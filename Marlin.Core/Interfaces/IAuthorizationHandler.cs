using Marlin.Core.Interfaces.Entities;

namespace Marlin.Core.Interfaces
{
    public interface IAuthorizationHandler<T, T1> where T : IIdentity<T1>, new()
    {
        T Login(object[] args);
        string GenerateCredential(T identity);
    }
}
