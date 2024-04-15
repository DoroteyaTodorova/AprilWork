using HitachiSPACEProgramme.HitachiSPACEProgramme.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme
{
    internal class Criteria
    {
        public int MinTemperature { get; set; } = 1; // Default value is 1 degree Celsius
        public int MaxTemperature { get; set; } = 32; // Default value is 32 degree Celsius
        public int MaxWind { get; set; } = 11; // Default value is 11 m/s
        public int MaxPrecipitation { get; set; } = 0; // Default value is 0 %
        public int MaxHumidity { get; set; } = 55; // Default value is 55 %
        public bool AcceptLightnings { get; set; } = false; // Default value is false
        public string[] AcceptableCloudTypes { get; set; } = { "stratus", "cirrus" }; // Default values for acceptable cloud types

        public void ChangeCriteria()
        {


            int choice = 0;
            do
            {
                Console.WriteLine("\n" + LanguageHandler.GetString("ChangeCriteriaPrompt"));
                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ChangeMinTemperature();
                        break;
                    case 2:
                        ChangeMaxTemperature();
                        break;
                    case 3:
                        ChangeMaxWind();
                        break;
                    case 4:
                        ChangeMaxPrecipitation();
                        break;
                    case 5:
                        ChangeMaxHumidity();
                        break;
                    case 6:
                        ChangeAcceptLightnings();
                        break;
                    case 7:
                        ChangeAcceptableCloudTypes();
                        break;
                    default:
                        Console.WriteLine(LanguageHandler.GetString("NoChangesMade"));
                        break;
                }

            } while (choice != 0);
        }
        private bool IsAcceptableCloudType(string cloudType)
        {
            string[] acceptableCloudTypes = { "cumulus", "stratus", "nimbus", "cirrus" };
           return acceptableCloudTypes.Contains(cloudType.ToLower());
        }
        public void ShowCriteria()
        {
            Console.WriteLine($"\n {LanguageHandler.GetString("CurrentCriteria")}");
            Console.WriteLine($"1. {LanguageHandler.GetString("MinTemperature")} {MinTemperature}°C");
            Console.WriteLine($"2. {LanguageHandler.GetString("MaxTemperature")} {MaxTemperature}°C");
            Console.WriteLine($"3. {LanguageHandler.GetString("MaxWind")} {MaxWind} m/s");
            Console.WriteLine($"4. {LanguageHandler.GetString("MaxPrecipitation")}  {MaxPrecipitation} %");
            Console.WriteLine($"5. {LanguageHandler.GetString("MaxHumidity")}  {MaxHumidity}%");
            Console.WriteLine($"6. {LanguageHandler.GetString("AcceptLightnings")} {(AcceptLightnings ? "Yes" : "No")}");
            Console.WriteLine($"7. {LanguageHandler.GetString("AcceptableClouds")} {string.Join(", ", AcceptableCloudTypes)}");
        }


        private void ChangeMinTemperature()
        {

            Console.WriteLine(LanguageHandler.GetString("MinTemperaturePrompt"));
            try
            {
                MinTemperature = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(LanguageHandler.GetString("MinTemperatureSuccess"));
            }
            catch (FormatException)
            {
                Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
            }
        }

        private void ChangeMaxTemperature()
        {
            Console.WriteLine(LanguageHandler.GetString("MaxTemperaturePrompt"));
            try
            {
                MaxTemperature = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(LanguageHandler.GetString("MaxTemperatureSuccess"));
            }
            catch (FormatException)
            {
                Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
            }
        }

        private void ChangeMaxWind()
        {
            Console.WriteLine(LanguageHandler.GetString("MaxWindPrompt"));
            try
            {
                MaxWind = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(LanguageHandler.GetString("MaxWindSuccess"));
            }
            catch (FormatException)
            {
                Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
            }
        }

        private void ChangeMaxPrecipitation()
        {
            Console.WriteLine(LanguageHandler.GetString("MaxPrecipitationPrompt"));
            try
            {
                MaxPrecipitation = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(LanguageHandler.GetString("MaxPrecipitationSuccess"));
            }
            catch (FormatException)
            {
                Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
            }
        }

        private void ChangeMaxHumidity()
        {
            Console.WriteLine(LanguageHandler.GetString("MaxHumidityPrompt"));
            try
            {
                MaxHumidity = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(LanguageHandler.GetString("MaxHumiditySucces"));
            }
            catch (FormatException)
            {
                Console.WriteLine(LanguageHandler.GetString("InvalidInput"));
            }
        }

        private void ChangeAcceptLightnings()
        {
            Console.WriteLine(LanguageHandler.GetString("AcceptLightningsPrompt"));
            string lightningsInput = Console.ReadLine().ToLower();

            if (lightningsInput == LanguageHandler.GetString("Yes"))
            {
                AcceptLightnings = true;
                Console.WriteLine(LanguageHandler.GetString("AcceptLightningsSetYes"));
            }
            else if (lightningsInput == LanguageHandler.GetString("No"))
            {
                AcceptLightnings = false;
                Console.WriteLine(LanguageHandler.GetString("AcceptLightningsSetNo"));
            }
            else
            {
                Console.WriteLine(LanguageHandler.GetString("AcceptLightningsPrompt"));
            }
        }

        private void ChangeAcceptableCloudTypes()
        {
            Console.WriteLine(LanguageHandler.GetString("CloudTypesPrompt"));
            string cloudTypesInput = Console.ReadLine();
            string[] cloudTypes = cloudTypesInput.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string cloudType in cloudTypes)
            {
                
                if (!IsAcceptableCloudType(cloudType.Trim().ToLower()))
                {
                    Console.WriteLine(LanguageHandler.GetString("InvalidCloudType"),cloudType);
                    return; 
                }
            }
            Console.WriteLine(LanguageHandler.GetString("AcceptableCloudTypeSucces"));
            AcceptableCloudTypes = cloudTypes;
        }
    }
}
