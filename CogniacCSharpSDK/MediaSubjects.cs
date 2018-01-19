using Newtonsoft.Json;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.MediaSubjects object
    /// </summary>
    public partial class MediaSubjects
    {
        /// <summary>
        /// Data array containing media subjects
        /// </summary>
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public partial class MediaSubjects
    {
        /// <summary>
        /// Build the Cogniac.MediaSubjects object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.MediaSubjects object</returns>
        public static MediaSubjects FromJson(string json) => JsonConvert.DeserializeObject<MediaSubjects>(json, Converter.Settings);
    }
}
