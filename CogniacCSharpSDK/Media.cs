/*
    Copyright 2019 Cogniac Corporation.

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.Media class
    /// </summary>
    public partial class Media
    {
        /// <summary>
        /// Set assignment
        /// </summary>
        [JsonProperty("set_assignment")]
        public string SetAssignment { get; set; }

        /// <summary>
        /// Number of frames
        /// </summary>
        [JsonProperty("num_frames")]
        public long? NumFrames { get; set; }

        /// <summary>
        /// Frame number
        /// </summary>
        [JsonProperty("frame")]
        public long? Frame { get; set; }

        /// <summary>
        /// Video flag
        /// </summary>
        [JsonProperty("video")]
        public bool? Video { get; set; }

        /// <summary>
        /// Frame duration
        /// </summary>
        [JsonProperty("frame_durations")]
        public object[] FrameDurations { get; set; }

        /// <summary>
        /// Media duration
        /// </summary>
        [JsonProperty("duration")]
        public long? Duration { get; set; }

        /// <summary>
        /// Media size
        /// </summary>
        [JsonProperty("size")]
        public long? Size { get; set; }

        /// <summary>
        /// Network camera ID
        /// </summary>
        [JsonProperty("network_camera_id")]
        public string NetworkCameraId { get; set; }

        /// <summary>
        /// Resize URL's list
        /// </summary>
        [JsonProperty("resize_urls")]
        public Dictionary<string, string> ResizeUrls { get; set; }

        /// <summary>
        /// Original URL
        /// </summary>
        [JsonProperty("original_url")]
        public string OriginalUrl { get; set; }

        /// <summary>
        /// Image width
        /// </summary>
        [JsonProperty("image_width")]
        public long? ImageWidth { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        [JsonProperty("filename")]
        public string Filename { get; set; }

        /// <summary>
        /// Original landing URL
        /// </summary>
        [JsonProperty("original_landing_url")]
        public string OriginalLandingUrl { get; set; }

        /// <summary>
        /// Frames per second value
        /// </summary>
        [JsonProperty("fps")]
        public double? Fps { get; set; }

        /// <summary>
        /// The user uploading the midea
        /// </summary>
        [JsonProperty("uploaded_by_user")]
        public string UploadedByUser { get; set; }

        /// <summary>
        /// Media time stamp
        /// </summary>
        [JsonProperty("media_timestamp")]
        public double? MediaTimestamp { get; set; }

        /// <summary>
        /// Media URL
        /// </summary>
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        /// <summary>
        /// Status of upload
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Media ID
        /// </summary>
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        /// <summary>
        /// External media ID
        /// </summary>
        [JsonProperty("external_media_id")]
        public string ExternalMediaId { get; set; }

        /// <summary>
        /// Time base
        /// </summary>
        [JsonProperty("time_base")]
        public object TimeBase { get; set; }

        /// <summary>
        /// Source URL
        /// </summary>
        [JsonProperty("source_url")]
        public string SourceUrl { get; set; }

        /// <summary>
        /// Author profile URL
        /// </summary>
        [JsonProperty("author_profile_url")]
        public string AuthorProfileUrl { get; set; }

        /// <summary>
        /// Media source
        /// </summary>
        [JsonProperty("media_src")]
        public string MediaSrc { get; set; }

        /// <summary>
        /// Parent media ID
        /// </summary>
        [JsonProperty("parent_media_id")]
        public string ParentMediaId { get; set; }

        /// <summary>
        /// MD5 hash
        /// </summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        /// <summary>
        /// Parent media ID's
        /// </summary>
        [JsonProperty("parent_media_ids")]
        public string[] ParentMediaIds { get; set; }

        /// <summary>
        /// Meta tags
        /// </summary>
        [JsonProperty("meta_tags")]
        public string[] MetaTags { get; set; }

        /// <summary>
        /// License string
        /// </summary>
        [JsonProperty("license")]
        public string License { get; set; }

        /// <summary>
        /// Tenant ID
        /// </summary>
        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        /// <summary>
        /// Create at time
        /// </summary>
        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        /// <summary>
        /// Name of author
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// Preview URL
        /// </summary>
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }

        /// <summary>
        /// Image height
        /// </summary>
        [JsonProperty("image_height")]
        public long? ImageHeight { get; set; }

        /// <summary>
        /// Media format
        /// </summary>
        [JsonProperty("media_format")]
        public string MediaFormat { get; set; }

        /// <summary>
        /// Title of media
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    /// <summary>
    /// Cogniac.CaptureId class
    /// </summary>
    public partial class CaptureId
    {
        /// <summary>
        /// Capture ID value
        /// </summary>
        [JsonProperty("capture_id")]
        public string Id { get; set; }
    }

    public partial class CaptureId
    {
        /// <summary>
        /// Build the Cogniac.CaptureId object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.CaptureId object</returns>
        public static CaptureId FromJson(string json) 
            => JsonConvert.DeserializeObject<CaptureId>(json, Converter.Settings);
    }

    public partial class Media
    {
        /// <summary>
        /// Build the Cogniac.Media object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Media object</returns>
        public static Media FromJson(string json) 
            => JsonConvert.DeserializeObject<Media>(json, Converter.Settings);
    }
}
