namespace webapidemo.Model
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class MediaItems
    {
        public MediaItems()
        {
            NewMediaItems = new List<MediaItem>();
        }

        [JsonProperty("albumId")]
        public string AlbumId { get; set; }

        [JsonProperty("newMediaItems")]
        public List<MediaItem> NewMediaItems { get; set; }
    }

    public class MediaItem
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("simpleMediaItem")]
        public SimpleMediaItem SimpleMediaItem { get; set; }
    }

    public class SimpleMediaItem
    {
        [JsonProperty("uploadToken")]
        public string UploadToken { get; set; }
    }
}