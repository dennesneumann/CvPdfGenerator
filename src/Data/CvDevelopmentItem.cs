namespace CvPdfGenerator.Data
{
    /// <summary>
    /// Represents an item in the Further Education and Personal Development section of the CV.
    /// Can describe certifications, courses, personal projects, or periods of development.
    /// </summary>
    public class CvDevelopmentItem
    {
        /// <summary>
        /// Gets or sets the date range for the development activity.
        /// Format can vary, e.g., "MM.YYYY - MM.YYYY", "YYYY", "MM.YYYY - ongoing".
        /// This property is optional and can be null if a specific date range is not applicable.
        /// Example: "06.2020 - 12.2020" or "2021 - ongoing"
        /// </summary>
        public string? DateRange { get; set; }

        /// <summary>
        /// Gets or sets the required title or main description of the development activity.
        /// Should concisely state what the activity was (e.g., certification name, course title, project type).
        /// Example: "Certification as DevOps Engineer (Microsoft)" or "Continuous Further Education"
        /// </summary>
        public required string TitleOrDescription { get; set; }

        /// <summary>
        /// Gets or sets optional further details, elaboration, or context about the activity.
        /// Can be used for longer descriptions or specific achievements during the activity.
        /// This property is optional and can be null.
        /// </summary>
        public string? Details { get; set; }
    }
}
