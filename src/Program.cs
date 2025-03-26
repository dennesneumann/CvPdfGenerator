using System.Text.Json;
using CvPdfGenerator.Data;
using CvPdfGenerator.Rendering;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace CvPdfGenerator
{
    /// <summary>
    /// Main application class for generating a CV PDF from JSON data.
    /// </summary>
    public static class Program
    {
        private const string CvDataFileName = "cv_data.json";

        /// <summary>
        /// The main entry point for the application.
        /// Loads CV data from a JSON file and generates a PDF document.
        /// </summary>
        public static void Main()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var cvData = LoadCvDataFromJson(CvDataFileName, GetOptions());

            if (cvData == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    $"Error: Failed to load CV data from '{CvDataFileName}'. Exiting."
                );
                Console.ResetColor();
                PauseBeforeExit();
                return;
            }

            var outputPdfPath = "Curriculum_Vitae.pdf";
            Console.WriteLine(
                $"Generating CV PDF to '{outputPdfPath}' using data from '{CvDataFileName}'..."
            );

            try
            {
                var document = new CvDocument(cvData);
#if DEBUG
                Console.WriteLine($"Showing PDF preview in QuestPDF Companion (port 12500).");
                document.ShowInCompanion(12500);
#else
                document.GeneratePdf(outputPdfPath);
                Console.WriteLine($"PDF generated successfully at: {outputPdfPath}");
#endif

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("CV PDF generation process completed successfully.");
                Console.ResetColor();
            }
            catch (FileNotFoundException fileEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Rendering Error: Image file not found. {fileEx.Message}");
                Console.WriteLine(
                    $"Please ensure the image path in '{CvDataFileName}' is correct and the file exists."
                );
                Console.ResetColor();
                Console.WriteLine($"Trace: {fileEx.StackTrace}");
            }
            catch (Exception ex)
            {
                LogExceptionDetails("An unexpected error occurred during PDF generation", ex);
            }
            finally
            {
                PauseBeforeExit();
            }
        }

        private static JsonSerializerOptions GetOptions() =>
            new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Loads and deserializes CV data from the specified JSON file path.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>A <see cref="CvData"/> object if successful, otherwise null.</returns>
        private static CvData? LoadCvDataFromJson(string filePath, JsonSerializerOptions options)
        {
            if (!File.Exists(filePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Data file not found at '{Path.GetFullPath(filePath)}'");
                Console.WriteLine(
                    $"Please ensure '{filePath}' exists in the application directory."
                );
                Console.ResetColor();
                return null;
            }

            try
            {
                var jsonString = File.ReadAllText(filePath);
                var cvData = JsonSerializer.Deserialize<CvData>(jsonString, options);

                if (cvData == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(
                        $"Error: Failed to deserialize JSON data from '{filePath}'. The file might be empty or invalid."
                    );
                    Console.ResetColor();
                    return null;
                }

                Console.WriteLine($"Successfully loaded CV data from '{filePath}'.");
                return cvData;
            }
            catch (JsonException jsonEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: Invalid JSON format in '{filePath}'. {jsonEx.Message}");
                Console.ResetColor();
                return null;
            }
            catch (IOException ioEx)
            {
                LogExceptionDetails($"Error reading data file '{filePath}'", ioEx);
                return null;
            }
            catch (Exception ex)
            {
                LogExceptionDetails(
                    $"An unexpected error occurred while loading data from '{filePath}'",
                    ex
                );
                return null;
            }
        }

        /// <summary>
        /// Logs detailed exception information to the console.
        /// </summary>
        /// <param name="message">A prefix message describing the context of the error.</param>
        /// <param name="exception">The exception that occurred.</param>
        private static void LogExceptionDetails(string message, Exception exception)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{message}: {exception.Message}");
            Console.ResetColor();
            Console.WriteLine("---------------- Exception Details ----------------");
            Console.WriteLine($"Type: {exception.GetType().FullName}");
            Console.WriteLine($"Trace: {exception.StackTrace}");
            var inner = exception.InnerException;
            var level = 1;
            while (inner != null)
            {
                Console.WriteLine($"\n--------- Inner Exception ({level}) ---------");
                Console.WriteLine($"Type: {inner.GetType().FullName}");
                Console.WriteLine($"Message: {inner.Message}");
                Console.WriteLine($"Trace: {inner.StackTrace}");
                inner = inner.InnerException;
                level++;
            }
            Console.WriteLine("-----------------------------------------------");
        }

        /// <summary>
        /// Pauses console execution until a key is pressed.
        /// </summary>
        private static void PauseBeforeExit()
        {
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
