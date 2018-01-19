using Newtonsoft.Json;

namespace Cogniac
{
    /// <summary>
    /// Cogniac.SubjectMediaAssociations class
    /// </summary>
    public partial class SubjectMediaAssociations
    {
        /// <summary>
        /// Paging property, for next page
        /// </summary>
        [JsonProperty("paging")]
        public Paging Paging { get; set; }

        /// <summary>
        /// Data on the current page
        /// </summary>
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    /// <summary>
    /// Cogniac.Datum class
    /// </summary>
    public partial class Datum
    {
        /// <summary>
        /// Other media property
        /// </summary>
        [JsonProperty("other_media")]
        public object[] OtherMedia { get; set; }

        /// <summary>
        /// Media object associated
        /// </summary>
        [JsonProperty("media")]
        public Media Media { get; set; }

        /// <summary>
        /// Focus data
        /// </summary>
        [JsonProperty("focus")]
        public object Focus { get; set; }

        /// <summary>
        /// Media objects list
        /// </summary>
        [JsonProperty("media_list")]
        public Media[] MediaList { get; set; }

        /// <summary>
        /// Subject object
        /// </summary>
        [JsonProperty("subject")]
        public Subject Subject { get; set; }
    }

    /// <summary>
    /// Cogniac.Subject class
    /// Extra properties for subject media associations
    /// </summary>
    public partial class Subject
    {
        /// <summary>
        /// Media ID property
        /// </summary>
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        /// <summary>
        /// Probability value
        /// </summary>
        [JsonProperty("probability")]
        public double? Probability { get; set; }

        /// <summary>
        /// Focus data
        /// </summary>
        [JsonProperty("focus")]
        public object Focus { get; set; }

        /// <summary>
        /// Application data type
        /// </summary>
        [JsonProperty("app_data_type")]
        public string AppDataType { get; set; }

        /// <summary>
        /// Updated at time
        /// </summary>
        [JsonProperty("updated_at")]
        public double? UpdatedAt { get; set; }

        /// <summary>
        /// Consensus data
        /// </summary>
        [JsonProperty("consensus")]
        public string Consensus { get; set; }

        /// <summary>
        /// Application data object
        /// </summary>
        [JsonProperty("app_data")]
        public AppDatum[] AppData { get; set; }
    }

    /// <summary>
    /// Cogniac.AppDatum object
    /// </summary>
    public partial class AppDatum
    {
        /// <summary>
        /// Box object
        /// </summary>
        [JsonProperty("box")]
        public Box Box { get; set; }

        /// <summary>
        /// Probability value
        /// </summary>
        [JsonProperty("probability")]
        public double? Probability { get; set; }
    }

    /// <summary>
    /// Cogniac.Box object
    /// </summary>
    public partial class Box
    {
        /// <summary>
        /// Y1 value
        /// </summary>
        [JsonProperty("y1")]
        public long? Y1 { get; set; }

        /// <summary>
        /// Y0 value
        /// </summary>
        [JsonProperty("y0")]
        public long? Y0 { get; set; }

        /// <summary>
        /// X0 value
        /// </summary>
        [JsonProperty("x0")]
        public long? X0 { get; set; }

        /// <summary>
        /// X1 value
        /// </summary>
        [JsonProperty("x1")]
        public long? X1 { get; set; }
    }

    /// <summary>
    /// Cogniac.Paging object
    /// </summary>
    public partial class Paging
    {
        /// <summary>
        /// Next page property
        /// </summary>
        [JsonProperty("next")]
        public string Next { get; set; }
    }

    public partial class SubjectMediaAssociations
    {
        /// <summary>
        /// Build the Cogniac.SubjectMediaAssociations object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.SubjectMediaAssociations object</returns>
        public static SubjectMediaAssociations FromJson(string json) => JsonConvert.DeserializeObject<SubjectMediaAssociations>(json, Converter.Settings);
    }

    public partial class Datum
    {
        /// <summary>
        /// Build the Cogniac.Datum object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Datum object</returns>
        public static Datum FromJson(string json) => JsonConvert.DeserializeObject<Datum>(json, Converter.Settings);
    }

    public partial class AppDatum
    {
        /// <summary>
        /// Build the Cogniac.AppDatum object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.AppDatum object</returns>
        public static AppDatum FromJson(string json) => JsonConvert.DeserializeObject<AppDatum>(json, Converter.Settings);
    }

    public partial class Box
    {
        /// <summary>
        /// Build the Cogniac.Box object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Box object</returns>
        public static Box FromJson(string json) => JsonConvert.DeserializeObject<Box>(json, Converter.Settings);
    }

    public partial class Paging
    {
        /// <summary>
        /// Build the Cogniac.Paging object from a valid JSON string
        /// </summary>
        /// <param name="json">A valid JSON string</param>
        /// <returns>Cogniac.Paging object</returns>
        public static Paging FromJson(string json) => JsonConvert.DeserializeObject<Paging>(json, Converter.Settings);
    }
}
