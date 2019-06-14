using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using webapidemo.DTO;
using webapidemo.Model;
using webapidemo.Repositories;

namespace webapidemo.Services
{
    public class PhotoService : IPhotoService
    {
        private const string _photoAlbumName = "Notes_5d0508a2-4222-40dd-8631-583706b9b627";

        private IUserRepository _userRepository;
        private IMapper _mapper;

        public PhotoService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddAlbum(string accessToken, UserDto foundUser)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://photoslibrary.googleapis.com/v1/albums"))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(
                        new
                        {
                            album = new { title = _photoAlbumName }
                        }));
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    var album = await response.Content.ReadAsAsync<GooglePhotoAlbumResponse>();

                    foundUser.GooglePhotoAlbumId = album.id;
                    _userRepository.Update(foundUser);
                }
            }
        }

        public async Task<NewPhoto> AddPhoto(string accessToken, string userId, IFormFile imageFile)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string uploadToken = null;

                // Upload image data, gets uploadToken back
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://photoslibrary.googleapis.com/v1/uploads"))
                { 
                    using (StreamContent content = new StreamContent(imageFile.OpenReadStream()))
                    {
                        request.Content = content;
                        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                        request.Headers.Add("Authorization", $"Bearer {accessToken}");
                        request.Headers.Add("X-Goog-Upload-File-Name", imageFile.FileName);
                        request.Headers.Add("X-Goog-Upload-Protocol", "raw");

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        uploadToken = await response.Content.ReadAsStringAsync();
                    }
                }

                if (uploadToken == null)
                {
                    // TODO: Something went wrong.
                    return;
                }

                UserDto user = await _userRepository.Get(userId);

                MediaItems mediaItems = new MediaItems();
                mediaItems.AlbumId = user.GooglePhotoAlbumId;
                MediaItem mediaItem = new MediaItem();
                mediaItem.SimpleMediaItem = new SimpleMediaItem {UploadToken = uploadToken};
                mediaItems.NewMediaItems.Add(mediaItem);

                // Create media item with upload token as ref.
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://photoslibrary.googleapis.com/v1/mediaItems:batchCreate"))
                { 

                        request.Content = new StringContent(JsonConvert.SerializeObject(mediaItems));
                        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        request.Headers.Add("Authorization", $"Bearer {accessToken}");

                        HttpResponseMessage response = await httpClient.SendAsync(request);
                        string result = await response.Content.ReadAsStringAsync();

                        var newMediaItems = JsonConvert.DeserializeObject<NewMediaItems>(result);

                        var newMediaItem = newMediaItems.NewMediaItemResults.FirstOrDefault();
                        
                        var photoData = _mapper.Map<NewPhoto>(newMediaItem.MediaItem);

                        return photoData;
                        
                }
            }
        }
    }
}