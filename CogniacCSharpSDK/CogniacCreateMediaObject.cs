using System;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CogniacCSharpSDK
{
    public partial class CogniacCreateMediaObject
    {
        [JsonProperty("set_assignment")]
        public string SetAssignment { get; set; }

        [JsonProperty("num_frames")]
        public object NumFrames { get; set; }

        [JsonProperty("frame")]
        public object Frame { get; set; }

        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("frame_durations")]
        public object[] FrameDurations { get; set; }

        [JsonProperty("duration")]
        public object Duration { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("network_camera_id")]
        public object NetworkCameraId { get; set; }

        [JsonProperty("resize_urls")]
        public Dictionary<string, string> ResizeUrls { get; set; }

        [JsonProperty("original_url")]
        public object OriginalUrl { get; set; }

        [JsonProperty("image_width")]
        public long ImageWidth { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("original_landing_url")]
        public object OriginalLandingUrl { get; set; }

        [JsonProperty("fps")]
        public object Fps { get; set; }

        [JsonProperty("uploaded_by_user")]
        public string UploadedByUser { get; set; }

        [JsonProperty("media_timestamp")]
        public long MediaTimestamp { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("external_media_id")]
        public object ExternalMediaId { get; set; }

        [JsonProperty("time_base")]
        public object TimeBase { get; set; }

        [JsonProperty("source_url")]
        public object SourceUrl { get; set; }

        [JsonProperty("author_profile_url")]
        public object AuthorProfileUrl { get; set; }

        [JsonProperty("media_src")]
        public string MediaSrc { get; set; }

        [JsonProperty("parent_media_id")]
        public object ParentMediaId { get; set; }

        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("parent_media_ids")]
        public object[] ParentMediaIds { get; set; }

        [JsonProperty("meta_tags")]
        public string[] MetaTags { get; set; }

        [JsonProperty("license")]
        public object License { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("created_at")]
        public double CreatedAt { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("preview_url")]
        public object PreviewUrl { get; set; }

        [JsonProperty("image_height")]
        public long ImageHeight { get; set; }

        [JsonProperty("media_format")]
        public string MediaFormat { get; set; }

        [JsonProperty("title")]
        public object Title { get; set; }
    }

    public partial class CogniacCreateMediaObject
    {
        public static CogniacCreateMediaObject FromJson(string json) => JsonConvert.DeserializeObject<CogniacCreateMediaObject>(json, Converter.Settings);
    }
}
