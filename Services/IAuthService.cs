using System.Threading.Tasks;
using webapidemo.DTO;

namespace webapidemo.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
    }
}