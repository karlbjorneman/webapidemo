using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Driver;
using webapidemo.DTO;
using webapidemo.Repositories;
using Newtonsoft.Json;

namespace webapidemo.Services
{
    public class AuthService : IAuthService
    {
        private IMapper _mapper;
        private IUserRepository _userRepository;
        private IPhotoService _photoService;

        public AuthService(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {   
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<UserDto> Authenticate(UserDto user, string accessToken)
        {
            var foundUser = await _userRepository.GetOrCreate(user);

            if (string.IsNullOrEmpty(foundUser.GooglePhotoAlbumId))
            {
                await _photoService.AddAlbum(accessToken, foundUser);
            }

            return foundUser;
        }

    }

    public class GooglePhotoAlbumResponse
    {
        public string productUrl {get; set;}
        public string id {get; set;}
        public string title {get; set;}
        public string isWriteable {get; set;}
    }
}