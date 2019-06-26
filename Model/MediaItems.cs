namespace webapidemo.Model
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class MediaItems
    {
        [JsonProperty("newMediaItemResults")]
        public List<MediaItemResponse> NewMediaItemResults { get; set; }
    }

    public class MediaItemResponse
    {
        [JsonProperty("uploadToken")]
        public string UploadToken { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("mediaItem", NullValueHandling = NullValueHandling.Ignore)]
        public MediaItemResonse MediaItem { get; set; }
    }

    public class MediaItemResonse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productUrl")]
        public Uri ProductUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("mediaMetadata")]
        public MediaMetadata MediaMetadata { get; set; }
    }

    public class MediaMetadata
    {
        [JsonProperty("width")]
        public string Width { get; set; }

        [JsonProperty("height")]
        public string Height { get; set; }

        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }

        [JsonProperty("photo")]
        public Photo Photo { get; set; }
    }

    public class Photo
    {
    }

    public class Status
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }
    }
}
