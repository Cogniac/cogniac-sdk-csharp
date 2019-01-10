/*
    Copyright 2018 Cogniac Corporation.

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
    /// Cogniac.MediaSubjects object
    /// </summary>
    public partial class MediaDetections
    {
        /// <summary>
        /// Data array containing media subjects
        /// </summary>
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class MediaDetections
    {
        /// <summary>
        /// Build the Cogniac.MediaDetections object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.MediaDetections object</returns>
        public static MediaDetections FromJson(string json) => JsonConvert.DeserializeObject<MediaDetections>(json, Converter.Settings);
    }
}