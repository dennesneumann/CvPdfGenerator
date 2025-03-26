namespace CvPdfGenerator.Data
{
    /// <summary>
    /// Represents a single work experience entry in the CV.
    /// Details a specific job or role held by the individual.
    /// </summary>
    public class CvWorkExperienceItem
    {
        /// <summary>
        /// Gets or sets the duration of the employment or activity.
        /// Format can vary, e.g., "MM.YYYY - MM.YYYY", "MM.YYYY - Current".
        /// Example: "07.2019 - 05.2020"
        /// </summary>
        public string DateRange { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the official job title or role held during this period.
        /// Example: "Software Developer / DevOps Engineer"
        /// </summary>
        public string JobTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the employer or company, potentially including the location.
        /// Example: "Tech Company, City"
        /// </summary>
        public string Company { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a list of key responsibilities, tasks, or achievements associated with this role.
        /// Each string represents a separate point or description line.
        /// </summary>
        public List<string>? Responsibilities { get; set; } = [];
    }
}
