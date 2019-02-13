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

using Newtonsoft.Json;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.MediaDetections class
    /// </summary>
    public partial class MediaDetections
    {
        /// <summary>
        /// List of other meida objects
        /// </summary>
        [JsonProperty("other_media")]
        public object[] OtherMedia { get; set; }

        /// <summary>
        /// List of detection objects
        /// </summary>
        [JsonProperty("detections")]
        public Detection[] Detections { get; set; }

        /// <summary>
        /// Media list objects array
        /// </summary>
        [JsonProperty("media_list")]
        public Media[] MediaList { get; set; }
    }

    /// <summary>
    /// Cogniac.Detection class
    /// </summary>
    public partial class Detection
    {
        /// <summary>
        /// Model ID
        /// </summary>
        [JsonProperty("model_id")]
        public object ModelId { get; set; }

        /// <summary>
        /// Media ID
        /// </summary>
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        /// <summary>
        /// Probability value
        /// </summary>
        [JsonProperty("probability")]
        public double? Probability { get; set; }

        /// <summary>
        /// Created at time-stamp
        /// </summary>
        [JsonProperty("created_at")]
        public double? CreatedAt { get; set; }

        /// <summary>
        /// Focus object
        /// </summary>
        [JsonProperty("focus")]
        public object Focus { get; set; }

        /// <summary>
        /// Application data type string
        /// </summary>
        [JsonProperty("app_data_type")]
        public string AppDataType { get; set; }

        /// <summary>
        /// Inference focus object
        /// </summary>
        [JsonProperty("inference_focus")]
        public object InferenceFocus { get; set; }

        /// <summary>
        /// Subject UID string
        /// </summary>
        [JsonProperty("subject_uid")]
        public string SubjectUid { get; set; }

        /// <summary>
        /// Application data object
        /// </summary>
        [JsonProperty("app_data")]
        public dynamic AppData { get; set; }

        /// <summary>
        /// Uncal prob object
        /// </summary>
        [JsonProperty("uncal_prob")]
        public object UncalProb { get; set; }

        /// <summary>
        /// Prev prob object
        /// </summary>
        [JsonProperty("prev_prob")]
        public object PrevProb { get; set; }

        /// <summary>
        /// Detectoin ID
        /// </summary>
        [JsonProperty("detection_id")]
        public string DetectionId { get; set; }

        /// <summary>
        /// User ID
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Applicatoin ID
        /// </summary>
        [JsonProperty("app_id")]
        public object AppId { get; set; }

        /// <summary>
        /// Activation object
        /// </summary>
        [JsonProperty("activation")]
        public object Activation { get; set; }
    }

    /// <summary>
    /// Cogniac.MediaList class
    /// </summary>
    public partial class Media
    {
        /// <summary>
        /// Video contaxt object
        /// </summary>
        [JsonProperty("video_context")]
        public object VideoContext { get; set; }

        /// <summary>
        /// Frame preview map
        /// </summary>
        [JsonProperty("frame_preview_map")]
        public dynamic FramePreviewMap { get; set; }
    }

    public partial class MediaDetections
    {
        /// <summary>
        /// Build the Cogniac.MediaDetections object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.MediaDetections object</returns>
        public static MediaDetections FromJson(string json) 
            => JsonConvert.DeserializeObject<MediaDetections>(json, Cogniac.Converter.Settings);
    }
}