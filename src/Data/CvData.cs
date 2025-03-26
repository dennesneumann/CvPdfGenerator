namespace CvPdfGenerator.Data
{
    /// <summary>
    /// Represents the structured data required to generate the CV PDF.
    /// Holds all sections of the curriculum vitae.
    /// </summary>
    public class CvData
    {
        /// <summary>
        /// Gets or sets the full name of the individual.
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// Gets or sets the primary job title or role.
        /// </summary>
        public required string JobTitle { get; set; }

        /// <summary>
        /// Gets or sets the detailed personal data (address, DOB, etc.).
        /// </summary>
        public required CvPersonalData PersonalData { get; set; }

        /// <summary>
        /// Gets or sets the file path to the profile picture.
        /// Path can be relative or absolute. Ensure the file is accessible at runtime.
        /// </summary>
        public required string ProfilePicturePath { get; set; }

        /// <summary>
        /// Gets or sets the professional summary or profile text.
        /// </summary>
        public required string ProfileSummary { get; set; }

        /// <summary>
        /// Gets or sets a list of core professional competencies or skills.
        /// </summary>
        public required List<string> CoreCompetencies { get; set; }

        /// <summary>
        /// Gets or sets a list of attachments mentioned (e.g., "Resume", "Certificates").
        /// </summary>
        public required List<string> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the list of work experience entries.
        /// </summary>
        public required List<CvWorkExperienceItem> WorkExperience { get; set; }

        /// <summary>
        /// Gets or sets the list of further education and personal development entries.
        /// </summary>
        public required List<CvDevelopmentItem> DevelopmentAndEducation { get; set; }

        /// <summary>
        /// Gets or sets the list of language skills.
        /// </summary>
        public required List<CvLanguageSkill> LanguageSkills { get; set; }

        /// <summary>
        /// Gets or sets the list of technical / hard skills.
        /// </summary>
        public required List<string> HardSkills { get; set; }

        /// <summary>
        /// Gets or sets the list of soft skills / personal attributes.
        /// </summary>
        public required List<string> SoftSkills { get; set; }

        /// <summary>
        /// Gets or sets the driver's license category (e.g., "Class B", "Class A/B").
        /// </summary>
        public required string DriversLicense { get; set; }
    }
}
