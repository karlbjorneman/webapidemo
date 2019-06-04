using System.Threading.Tasks;
using MongoDB.Driver;
using webapidemo.DTO;
using webapidemo.Model;

namespace webapidemo.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IMongoCollection<UserDto> _usersCollection;

        public UserRepository(IMongoDatabase database)
        {
            _usersCollection = database.GetCollection<UserDto>("User");
        }

        public async Task<UserDto> Get(string userId)
        {
            UserDto user = (await _usersCollection.FindAsync(u => u.Id == userId)).FirstOrDefault();
            return user;
        }

        public async Task<UserDto> GetOrCreate(UserDto user)
        {
            var existingUser = (await _usersCollection.FindAsync(u => u.Email == user.Email)).FirstOrDefault();

            if (existingUser == null)
            {
                await _usersCollection.InsertOneAsync(user);
                return user;
            }

            return existingUser;
        }

        public async void Update(UserDto user)
        {
            var update = Builders<UserDto>.Update.Set(x => x.GooglePhotoAlbumId, user.GooglePhotoAlbumId);
            await _usersCollection.UpdateOneAsync(u => u.Id == user.Id, update);
        }
    }
}