using HitachiSPACEProgramme.HitachiSPACEProgramme.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme
{
    internal class FileManager
    {

        public static (string folderPath, string[] csvFiles) GetFolderAndCSVFilesFromUser(string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Console.WriteLine(LanguageHandler.GetString("FolderNotExist"));
                    return (null, null);
                }

                string[] csvFiles = Directory.GetFiles(folderPath, "*.csv");
                if (csvFiles.Length == 0)
                {
                    Console.WriteLine(LanguageHandler.GetString("NoCSVFiles"));
                    return (null, null);
                }

                return (folderPath, csvFiles);
            }
            catch (Exception ex)
            {
                Console.WriteLine(LanguageHandler.GetString("UnexpectedErrorGetFolder"),ex.Message);
                return (null, null);
            }
        }


        public static string WriteLaunchAnalysisReport(List<Spaceport> spaceports )
        {
            string reportFilePath = "LaunchAnalysisReport.csv";

            try
            {
                using (StreamWriter writer = new StreamWriter(reportFilePath))
                {
                    // Write headers
                    writer.WriteLine("Spaceport,Date");

                    // Write spaceports data
                    foreach (Spaceport sp in spaceports)
                    {
                        writer.WriteLine($"{sp.FileName},{sp.Day}");

                    }
                }
                Console.WriteLine(LanguageHandler.GetString("ReportCreatedSuccessfully"), reportFilePath);
                return reportFilePath;

            }
            catch (Exception ex)
            {
                Console.WriteLine(LanguageHandler.GetString("ErrorWritingReport"),ex.Message);
                return null;
            }
        }
        
    }
}
