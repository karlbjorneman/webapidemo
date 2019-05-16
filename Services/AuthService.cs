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
        private const string _photoAlbumName = "Notes_5d0508a2-4222-40dd-8631-583706b9b627";

        private IMapper _mapper;
        private IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository, IMapper mapper)
        {   
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> Authenticate(UserDto user, string accessToken)
        {
            var foundUser = await _userRepository.GetOrCreate(user);

            if (string.IsNullOrEmpty(foundUser.GooglePhotoAlbumId))
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://photoslibrary.googleapis.com/v1/albums"))
                    {
                        request.Content = new StringContent(JsonConvert.SerializeObject(
                            new {
                                    album = new { title = _photoAlbumName }
                                }));
                        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        request.Headers.Add("Authorization", $"Bearer {accessToken}");
                        var response = await httpClient.SendAsync(request);
                        var album = await response.Content.ReadAsAsync<GooglePhotoAlbumResponse>();

                        foundUser.GooglePhotoAlbumId = album.id;
                        _userRepository.Update(foundUser);
                    }
                }
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