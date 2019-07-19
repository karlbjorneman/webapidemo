using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using webapidemo.DTO;
using webapidemo.Model;

namespace webapidemo.Services
{
    public interface IPhotoService
    {
         Task AddAlbum(string accessToken, UserDto foundUser);
         Task<NewPhoto> AddPhoto(string accessToken, string userId, IFormFile imageFile);

         Task<GetPhoto> GetPhoto(string accessToken, string mediaItemId);
    }
}