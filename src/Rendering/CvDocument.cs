using CvPdfGenerator.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CvPdfGenerator.Rendering
{
    /// <summary>
    /// Represents the CV document structure, implementing the QuestPDF IDocument interface.
    /// Uses a CvData object for content.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CvDocument"/> class.
    /// </remarks>
    /// <param name="model">The data model containing the CV information.</param>
    public class CvDocument(CvData model) : IDocument
    {
        /// <summary>
        /// Gets the data model containing all information for the CV.
        /// </summary>
        public CvData Model { get; } = model ?? throw new ArgumentNullException(nameof(model));

        private static readonly string AccentColor = "#D3002E";
        private static readonly string HeadingColor = Colors.Grey.Darken3;
        private static readonly string TextColor = Colors.Black;
        private static readonly string MutedColor = Colors.Grey.Darken1;

        /// <summary>
        /// Gets the metadata for the PDF document.
        /// </summary>
        /// <returns>The document metadata.</returns>
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        /// <summary>
        /// Gets the settings for the PDF document.
        /// </summary>
        /// <returns>The document settings.</returns>
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        /// <summary>
        /// Composes the entire document structure page by page.
        /// </summary>
        /// <param name="container">The document container provided by QuestPDF.</param>
        [Obsolete]
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginHorizontal(1.5f, Unit.Centimetre);
                page.MarginVertical(1.5f, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x =>
                    x.FontSize(10).FontFamily(Fonts.Calibri).Color(TextColor)
                );

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().Element(ComposeFooter);
            });
        }

        /// <summary>
        /// Composes the header section of the CV page.
        /// </summary>
        /// <param name="container">The container for the header.</param>
        private void ComposeHeader(IContainer container)
        {
            container
                .PaddingBottom(10)
                .Column(column =>
                {
                    column.Spacing(5);
                    column
                        .Item()
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .Text("Curriculum Vitae")
                                .FontSize(24)
                                .Bold()
                                .FontColor(AccentColor);

                            row.AutoItem()
                                .AlignRight()
                                .Column(contactCol =>
                                {
                                    contactCol
                                        .Item()
                                        .Text(Model.PersonalData.Street)
                                        .FontSize(8)
                                        .FontColor(MutedColor);
                                    contactCol
                                        .Item()
                                        .Text(Model.PersonalData.City)
                                        .FontSize(8)
                                        .FontColor(MutedColor);
                                    contactCol
                                        .Item()
                                        .Text(Model.PersonalData.Email)
                                        .FontSize(8)
                                        .FontColor(MutedColor);
                                    contactCol
                                        .Item()
                                        .Text(Model.PersonalData.Phone)
                                        .FontSize(8)
                                        .FontColor(MutedColor);
                                });
                        });

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
                });
        }

        /// <summary>
        /// Composes the main content area, setting up the two-column layout.
        /// </summary>
        /// <param name="container">The container for the main content.</param>
        private void ComposeContent(IContainer container)
        {
            container.Row(row =>
            {
                row.ConstantItem(3).Background(AccentColor);
                row.ConstantItem(15);

                row.RelativeItem(3).PaddingRight(10).Column(ComposeLeftColumn);

                row.RelativeItem(7).PaddingLeft(10).Column(ComposeRightColumn);
            });
        }

        /// <summary>
        /// Composes the content for the left column (profile pic, summary, skills, etc.).
        /// </summary>
        /// <param name="column">The column descriptor for the left column.</param>
        private void ComposeLeftColumn(ColumnDescriptor column)
        {
            column.Spacing(20);

            ComposeProfilePicture(column.Item());

            ComposeSection(
                column,
                "Profile",
                c => c.Item().Text(Model.ProfileSummary).LineHeight(1.2f)
            );

            ComposeSection(
                column,
                "Core Competencies",
                c =>
                {
                    c.Spacing(5);
                    foreach (var competency in Model.CoreCompetencies)
                    {
                        c.Item()
                            .Row(row =>
                            {
                                row.ConstantItem(10).Text("• ").FontColor(AccentColor);
                                row.RelativeItem().Text(competency).LineHeight(1.2f);
                            });
                    }
                }
            );

            ComposeSection(
                column,
                "Personal Data",
                c =>
                {
                    c.Spacing(4);
                    ComposePersonalDataItem(c, "Street", Model.PersonalData.Street);
                    ComposePersonalDataItem(c, "City", Model.PersonalData.City);
                    ComposePersonalDataItem(c, "Email", Model.PersonalData.Email);
                    ComposePersonalDataItem(c, "Phone", Model.PersonalData.Phone);
                    ComposePersonalDataItem(c, "Date/PoB", Model.PersonalData.DateOfBirth);
                    ComposePersonalDataItem(c, "Nationality", Model.PersonalData.Nationality);
                }
            );

            if (Model.Attachments != null && Model.Attachments.Count != 0)
            {
                ComposeSection(
                    column,
                    "Attachments",
                    c =>
                    {
                        c.Spacing(4);
                        foreach (var attachment in Model.Attachments)
                        {
                            c.Item().Text(attachment);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Composes the profile picture element, including placeholder and error handling.
        /// </summary>
        /// <param name="container">The container to place the picture in.</param>
        private void ComposeProfilePicture(IContainer container)
        {
            if (
                !string.IsNullOrEmpty(Model.ProfilePicturePath)
                && File.Exists(Model.ProfilePicturePath)
            )
            {
                try
                {
                    container.Width(100).AlignLeft().Image(Model.ProfilePicturePath).FitArea();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error loading profile picture: {Model.ProfilePicturePath} - {ex.Message}"
                    );
                    container
                        .Element(Placeholder)
                        .Text("[Picture load error]")
                        .FontColor(Colors.Red.Medium);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Model.ProfilePicturePath))
                {
                    Console.WriteLine(
                        $"Warning: Profile picture not found at '{Model.ProfilePicturePath}'. Displaying placeholder."
                    );
                }
                container.Element(Placeholder).Text("Photo");
            }

            static IContainer Placeholder(IContainer c) =>
                c.Width(100)
                    .Height(100)
                    .Background(Colors.Grey.Lighten2)
                    .AlignCenter()
                    .AlignMiddle();
        }

        /// <summary>
        /// Helper to compose a key-value pair item for the Personal Data section.
        /// Uses English labels.
        /// </summary>
        /// <param name="column">The column descriptor to add the item to.</param>
        /// <param name="label">The English label for the data item.</param>
        /// <param name="value">The value of the data item.</param>
        private static void ComposePersonalDataItem(
            ColumnDescriptor column,
            string label,
            string value
        )
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            column
                .Item()
                .Row(row =>
                {
                    row.RelativeItem(2).Text(label).FontSize(9).FontColor(MutedColor);

                    row.RelativeItem(3).Text(value).FontSize(9);
                });
        }

        /// <summary>
        /// Composes the content for the right column (CV title, experience, education, skills).
        /// </summary>
        /// <param name="column">The column descriptor for the right column.</param>
        private void ComposeRightColumn(ColumnDescriptor column)
        {
            column.Spacing(25);

            column.Item().Text(Model.FullName).FontSize(20).SemiBold().FontColor(HeadingColor);

            if (Model.WorkExperience != null && Model.WorkExperience.Count != 0)
            {
                ComposeSection(
                    column,
                    "Work Experience",
                    c =>
                    {
                        c.Spacing(15);
                        foreach (var experience in Model.WorkExperience)
                        {
                            ComposeWorkExperienceEntry(c.Item(), experience);
                        }
                    }
                );
            }

            if (Model.DevelopmentAndEducation != null && Model.DevelopmentAndEducation.Count != 0)
            {
                ComposeSection(
                    column,
                    "Development and Education",
                    c =>
                    {
                        c.Spacing(15);
                        foreach (var devItem in Model.DevelopmentAndEducation)
                        {
                            ComposeDevelopmentEntry(c.Item(), devItem);
                        }
                    }
                );
            }

            if (
                (Model.LanguageSkills == null || Model.LanguageSkills.Count == 0)
                && (Model.HardSkills == null || Model.HardSkills.Count == 0)
                && (Model.SoftSkills == null || Model.SoftSkills.Count == 0)
                && string.IsNullOrWhiteSpace(Model.DriversLicense)
            )
            {
                return;
            }
            ComposeSection(column, "Skills and Knowledge", ComposeSkillsSectionContent);
        }

        /// <summary>
        /// Composes a single work experience entry.
        /// </summary>
        /// <param name="container">The container for this entry.</param>
        /// <param name="item">The work experience data item.</param>
        private static void ComposeWorkExperienceEntry(
            IContainer container,
            CvWorkExperienceItem item
        )
        {
            container.Column(column =>
            {
                column.Spacing(5);

                column
                    .Item()
                    .Row(row =>
                    {
                        row.ConstantItem(100)
                            .Text(item.DateRange)
                            .FontSize(9)
                            .FontColor(MutedColor);

                        row.RelativeItem()
                            .Column(titleCol =>
                            {
                                titleCol.Item().Text(item.JobTitle).Bold();
                                titleCol
                                    .Item()
                                    .Text(item.Company)
                                    .FontSize(9)
                                    .FontColor(MutedColor);
                            });
                    });

                if (item.Responsibilities != null && item.Responsibilities.Count != 0)
                {
                    column
                        .Item()
                        .PaddingLeft(10)
                        .Column(respCol =>
                        {
                            respCol.Spacing(3);
                            foreach (var resp in item.Responsibilities)
                            {
                                respCol
                                    .Item()
                                    .Row(row =>
                                    {
                                        row.ConstantItem(10).Text("• ").FontColor(AccentColor);
                                        row.RelativeItem().Text(resp).LineHeight(1.2f);
                                    });
                            }
                        });
                }
            });
        }

        /// <summary>
        /// Composes a single development/education entry.
        /// </summary>
        /// <param name="container">The container for this entry.</param>
        /// <param name="item">The development/education data item.</param>
        private static void ComposeDevelopmentEntry(IContainer container, CvDevelopmentItem item)
        {
            container.Column(column =>
            {
                column.Spacing(5);

                column
                    .Item()
                    .Row(row =>
                    {
                        row.ConstantItem(100)
                            .Text(item.DateRange ?? string.Empty)
                            .FontSize(9)
                            .FontColor(MutedColor);

                        row.RelativeItem()
                            .Column(titleCol =>
                            {
                                titleCol.Item().Text(item.TitleOrDescription).Bold();
                                if (!string.IsNullOrWhiteSpace(item.Details))
                                {
                                    titleCol
                                        .Item()
                                        .PaddingTop(2)
                                        .Text(item.Details)
                                        .LineHeight(1.2f);
                                }
                            });
                    });
            });
        }

        /// <summary>
        /// Composes the content for the "Skills and Knowledge" section, including sub-sections.
        /// </summary>
        /// <param name="column">The column descriptor for the Skills section content.</param>
        private void ComposeSkillsSectionContent(ColumnDescriptor column)
        {
            column.Spacing(15);

            if (Model.LanguageSkills != null && Model.LanguageSkills.Count != 0)
            {
                ComposeSkillCategory(
                    column.Item(),
                    "Languages",
                    c =>
                    {
                        c.Spacing(4);
                        foreach (var lang in Model.LanguageSkills)
                        {
                            c.Item()
                                .Row(row =>
                                {
                                    row.RelativeItem(1).Text(lang.Language).SemiBold();
                                    row.RelativeItem(2).Text(lang.Level);
                                });
                        }
                    }
                );
            }

            if (Model.HardSkills != null && Model.HardSkills.Count != 0)
            {
                ComposeSkillCategory(
                    column.Item(),
                    "Hard Skills",
                    c =>
                    {
                        c.Item().Text(string.Join(", ", Model.HardSkills)).LineHeight(1.3f);
                    }
                );
            }

            if (Model.SoftSkills != null && Model.SoftSkills.Count != 0)
            {
                ComposeSkillCategory(
                    column.Item(),
                    "Soft Skills",
                    c =>
                    {
                        c.Item().Text(string.Join(", ", Model.SoftSkills)).LineHeight(1.3f);
                    }
                );
            }

            if (!string.IsNullOrWhiteSpace(Model.DriversLicense))
            {
                ComposeSkillCategory(
                    column.Item(),
                    "Driver's License",
                    c =>
                    {
                        c.Item().Text(Model.DriversLicense);
                    }
                );
            }
        }

        /// <summary>
        /// Helper method to compose a category within the Skills section (e.g., Languages, Hard Skills).
        /// </summary>
        /// <param name="container">The container for the skill category.</param>
        /// <param name="title">The title of the skill category.</param>
        /// <param name="contentComposer">An action to compose the content of the category.</param>
        private static void ComposeSkillCategory(
            IContainer container,
            string title,
            Action<ColumnDescriptor> contentComposer
        )
        {
            container.Row(row =>
            {
                row.Spacing(10);

                row.ConstantItem(100).Text(title).SemiBold().FontSize(9).FontColor(MutedColor);

                var contentContainer = row.RelativeItem();

                contentContainer.Column(innerCol =>
                {
                    contentComposer(innerCol);
                });
            });
        }

        /// <summary>
        /// Helper method to compose a standard section with a title and content.
        /// </summary>
        /// <param name="column">The column descriptor to add the section to.</param>
        /// <param name="title">The English title of the section.</param>
        /// <param name="contentComposer">An action to compose the content of the section.</param>
        private static void ComposeSection(
            ColumnDescriptor column,
            string title,
            Action<ColumnDescriptor> contentComposer
        )
        {
            column
                .Item()
                .Column(sectionCol =>
                {
                    sectionCol.Spacing(8);

                    sectionCol.Item().Text(title).FontSize(14).SemiBold().FontColor(AccentColor);

                    var contentContainer = sectionCol.Item();

                    contentContainer.Column(innerContentCol =>
                    {
                        contentComposer(innerContentCol);
                    });
                });
        }

        /// <summary>
        /// Composes the footer section of the CV page (page numbers).
        /// </summary>
        /// <param name="container">The container for the footer.</param>
        private void ComposeFooter(IContainer container)
        {
            container
                .AlignCenter()
                .DefaultTextStyle(style => style.FontSize(8).FontColor(MutedColor))
                .Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                    x.Span(" of ");
                    x.TotalPages();
                });
        }
    }
}
