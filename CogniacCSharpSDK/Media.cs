using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cogniac
{
    public partial class Media
    {
        [JsonProperty("set_assignment")]
        public string SetAssignment { get; set; }

        [JsonProperty("num_frames")]
        public long? NumFrames { get; set; }

        [JsonProperty("frame")]
        public long? Frame { get; set; }

        [JsonProperty("video")]
        public bool? Video { get; set; }

        [JsonProperty("frame_durations")]
        public object[] FrameDurations { get; set; }

        [JsonProperty("duration")]
        public long? Duration { get; set; }

        [JsonProperty("size")]
        public long? Size { get; set; }

        [JsonProperty("network_camera_id")]
        public string NetworkCameraId { get; set; }

        [JsonProperty("resize_urls")]
        public Dictionary<string, string> ResizeUrls { get; set; }

        [JsonProperty("original_url")]
        public string OriginalUrl { get; set; }

        [JsonProperty("image_width")]
        public long? ImageWidth { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("original_landing_url")]
        public string OriginalLandingUrl { get; set; }

        [JsonProperty("fps")]
        public double? Fps { get; set; }

        [JsonProperty("uploaded_by_user")]
        public string UploadedByUser { get; set; }

        [JsonProperty("media_timestamp")]
        public long? MediaTimestamp { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("external_media_id")]
        public string ExternalMediaId { get; set; }

        [JsonProperty("time_base")]
        public object TimeBase { get; set; }

        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        [JsonProperty("author_profile_url")]
        public string AuthorProfileUrl { get; set; }

        [JsonProperty("media_src")]
        public string MediaSrc { get; set; }

        [JsonProperty("parent_media_id")]
        public string ParentMediaId { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("parent_media_ids")]
        public string[] ParentMediaIds { get; set; }

        [JsonProperty("meta_tags")]
        public string[] MetaTags { get; set; }

        [JsonProperty("license")]
        public string License { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        [JsonProperty("image_height")]
        public long? ImageHeight { get; set; }

        [JsonProperty("media_format")]
        public string MediaFormat { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public partial class CaptureId
    {
        [JsonProperty("capture_id")]
        public string Id { get; set; }
    }

    public partial class CaptureId
    {
        public static CaptureId FromJson(string json) => JsonConvert.DeserializeObject<CaptureId>(json, Converter.Settings);
    }

    public partial class Media
    {
        public static Media FromJson(string json) => JsonConvert.DeserializeObject<Media>(json, Converter.Settings);
    }
}
