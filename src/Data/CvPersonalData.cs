namespace CvPdfGenerator.Data
{
    /// <summary>
    /// Represents personal data for the CV.
    /// Contains detailed personal information about the individual.
    /// </summary>
    public class CvPersonalData
    {
        /// <summary>
        /// Gets or sets the street name and house number of the individual's address.
        /// Example: "Generic Street 123"
        /// </summary>
        public string Street { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the city or town name, potentially including the postal code.
        /// Example: "12345 Anytown"
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the personal email address.
        /// This might differ from the primary contact email in CvContactInfo.
        /// Example: "personal.email@example.com"
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the personal phone number.
        /// This might differ from the primary contact phone in CvContactInfo.
        /// Example: "+1 555-987-6543"
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and optionally the place of birth.
        /// Format can vary, e.g., "YYYY-MM-DD in City", "Month DD, YYYY".
        /// Example: "1990-01-15 in Berlin"
        /// </summary>
        public string DateOfBirth { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the nationality of the individual.
        /// Example: "German"
        /// </summary>
        public string Nationality { get; set; } = string.Empty;
    }
}
