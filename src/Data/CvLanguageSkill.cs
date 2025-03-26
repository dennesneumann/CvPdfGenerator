namespace CvPdfGenerator.Data
{
    /// <summary>
    /// Represents a language skill entry in the CV.
    /// </summary>
    public class CvLanguageSkill
    {
        /// <summary>
        /// Gets or sets the name of the language (e.g., "German", "English").
        /// </summary>
        public required string Language { get; set; }

        /// <summary>
        /// Gets or sets the proficiency level (e.g., "Native Spoken", "fluent in spoken and written").
        /// </summary>
        public required string Level { get; set; }
    }
}
