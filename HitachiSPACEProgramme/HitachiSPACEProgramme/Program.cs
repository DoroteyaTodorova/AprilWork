using HitachiSPACEProgramme.HitachiSPACEProgramme;
using HitachiSPACEProgramme.HitachiSPACEProgramme.Languages;


Dictionary<string, Criteria> fileCriteria = new Dictionary<string, Criteria>();
Dictionary<int, double> scores = new Dictionary<int, double>();
List<Spaceport> spaceports = new List<Spaceport>();
Criteria criteria = new Criteria();

string language = "en";
string folderPath = "";
string[] csvFiles = null;
bool isChoosingCriteria = true;
int choice;


do
{
    Console.Write("Please select the language you want to continue with (EN or DE)" +
        "\nBitte geben Sie die Sprache ein, mit der Sie fortfahren möchten (EN oder DE)\nEnter: ");
    language = Console.ReadLine();
} while (!(language.ToLower() == "en" || language.ToLower() == "de"));
if (language.Equals("de"))
{
    LanguageHandler.ChangeLanguage("de");
}
Console.WriteLine(LanguageHandler.GetString("Introduction") + " \n");

//Options to change criterias or continue
do
{
    Console.WriteLine(LanguageHandler.GetString("Criteria_ModifyPrompt"));
    Console.WriteLine("1. " + LanguageHandler.GetString("Criteria_YesOption"));
    Console.WriteLine("2. " + LanguageHandler.GetString("Criteria_NoOption"));
    Console.WriteLine("3. " + LanguageHandler.GetString("Criteria_ContinueOption"));
    Console.WriteLine("4. " + LanguageHandler.GetString("Criteria_ModifyFileOption"));

    do
    {
        Console.Write(LanguageHandler.GetString("EnterChoice"));
    } while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4);
    
    switch (choice)
    {
        case 1:
            criteria.ShowCriteria();
            criteria.ChangeCriteria();
            break;
        case 2:
            criteria.ShowCriteria();
            break;
        case 3:
        case 4:
            isChoosingCriteria = false;
            break;
        default:
            Console.WriteLine(LanguageHandler.GetString("InvalidChoice"));
            break;
    }
    Console.WriteLine("");

} while (isChoosingCriteria);




do
{
    try
    {

        do
        {
            try
            {
                Console.WriteLine(LanguageHandler.GetString("FolderPath_Prompt"));
                folderPath = Console.ReadLine();
                if (!Directory.Exists(folderPath))
                {
                    throw new DirectoryNotFoundException(LanguageHandler.GetString("FolderPath_Error"));
                }

                (folderPath, csvFiles) = FileManager.GetFolderAndCSVFilesFromUser(folderPath);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                continue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                continue;
            }

        } while (csvFiles == null);


        foreach (string file in csvFiles)
        {
            WeatherForecast weatherForecast = new WeatherForecast();

            //Option to modify criterias for specific file
            if(choice == 4)
            {
                string answer = null;
                do
                {
                    Console.WriteLine(string.Format(LanguageHandler.GetString("CriteriaFile_Prompt"), file));
                    answer = Console.ReadLine();
                    if (answer.ToLower() == LanguageHandler.GetString("Yes"))
                    {
                        criteria.ShowCriteria();
                        Console.WriteLine(LanguageHandler.GetString("CriteriaFile_EnterCriteria"), file);
                        Criteria specificCriteria = new Criteria
                        {
                            MinTemperature = criteria.MinTemperature,
                            MaxTemperature = criteria.MaxTemperature,
                            MaxWind = criteria.MaxWind,
                            MaxPrecipitation = criteria.MaxPrecipitation,
                            MaxHumidity = criteria.MaxHumidity,
                            AcceptLightnings = criteria.AcceptLightnings,
                            AcceptableCloudTypes = criteria.AcceptableCloudTypes.ToArray() // Clone the array to avoid reference sharing
                        };
                        specificCriteria.ChangeCriteria();
                        fileCriteria[file] = specificCriteria;
                        Console.WriteLine(LanguageHandler.GetString("CriteriaFile_Checking"), file);
                        //scores = WeatherForecast.CheckWeatherConditions(file, specificCriteria);
                        scores = weatherForecast.CheckWeatherConditions(file, specificCriteria);
                    }
                    else
                    {
                        scores = weatherForecast.CheckWeatherConditions(file, criteria);
                    }

                } while (answer.ToLower() == LanguageHandler.GetString("Yes") && answer.ToLower() == LanguageHandler.GetString("No"));

            }
            else
            {
                scores = weatherForecast.CheckWeatherConditions(file, criteria);

            }

            if (scores == null)
            {
                Console.WriteLine(LanguageHandler.GetString("WeatherCheck_Error"));
                break;
            }
            foreach (var item in scores)
            {
                Spaceport spaceport = new Spaceport(file, item.Value, item.Key);
                spaceports.Add(spaceport);
            }
        }

        
        if (spaceports.Count != 0)
        {
            Spaceport   bestSpaceport = WeatherForecast.FindBestCombination(spaceports);
            string analysisReportPath = FileManager.WriteLaunchAnalysisReport(spaceports);
            Console.WriteLine(LanguageHandler.GetString("BestSpaceport_Message"),bestSpaceport.FileName, bestSpaceport.Day);

            EmailSender.SendEmail(bestSpaceport, analysisReportPath);
        }
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" {LanguageHandler.GetString("FolderPath_UnexpectedError")} {ex.Message}");
    }

} while (csvFiles == null || spaceports.Count == 0);



