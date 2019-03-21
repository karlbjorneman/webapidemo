using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using webapidemo.DTO;

namespace webapidemo.Services
{
    public class AuthService : IAuthService
    {
        private IMapper _mapper;
        private IMongoCollection<UserDto> _usersCollection;

        public AuthService(IMongoDatabase database, IMapper mapper)
        {   
            _mapper = mapper;
            _usersCollection = database.GetCollection<UserDto>("User");
        }

        public async Task<UserDto> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            return await this.FindUserOrAdd(payload);
        }

        private async Task<UserDto> FindUserOrAdd(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload)
        {
            var existingUser = (await _usersCollection.FindAsync(user => user.Email == payload.Email)).FirstOrDefault();

            if (existingUser == null)
            {
                var newUser = _mapper.Map<UserDto>(payload);

                await _usersCollection.InsertOneAsync(newUser);

                return newUser;
            }

            return existingUser;
        }
    }
}