namespace webapidemo.Model
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GetPhoto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("productUrl")]
        public string ProductUrl { get; set; }

        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }

        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("mediaMetadata")]
        public MediaMetadata MediaMetadata { get; set; }

        [JsonProperty("contributorInfo")]
        public ContributorInfo ContributorInfo { get; set; }
    }

    public partial class ContributorInfo
    {
        [JsonProperty("profilePictureBaseUrl")]
        public string ProfilePictureBaseUrl { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public partial class MediaMetadata
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

    public partial class Photo
    {
        [JsonProperty("cameraMake")]
        public string CameraMake { get; set; }

        [JsonProperty("cameraModel")]
        public string CameraModel { get; set; }

        [JsonProperty("focalLength")]
        public string FocalLength { get; set; }

        [JsonProperty("apertureFNumber")]
        public string ApertureFNumber { get; set; }

        [JsonProperty("isoEquivalent")]
        public string IsoEquivalent { get; set; }

        [JsonProperty("exposureTime")]
        public string ExposureTime { get; set; }
    }
}
