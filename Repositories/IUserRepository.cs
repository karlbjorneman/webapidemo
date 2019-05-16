using System.Threading.Tasks;
using webapidemo.DTO;
using webapidemo.Model;

namespace webapidemo.Repositories
{
    public interface IUserRepository
    {
        Task<UserDto> GetOrCreate(UserDto user);

        void Update(UserDto user);
    }
}