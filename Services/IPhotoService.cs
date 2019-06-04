using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using webapidemo.DTO;

namespace webapidemo.Services
{
    public interface IPhotoService
    {
         Task AddAlbum(string accessToken, UserDto foundUser);
         Task AddPhoto(string accessToken, string userId, IFormFile imageFile);
    }
}