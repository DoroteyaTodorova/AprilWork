using HitachiSPACEProgramme.HitachiSPACEProgramme.Languages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme
{
    internal class WeatherForecast
    {

        public  Dictionary<int, double> CheckWeatherConditions(string filePath, Criteria criteria)
        {
            try
            {
                Dictionary<int, double> scores = new Dictionary<int, double>();
                string[] values = null;
                string[] lines = File.ReadAllLines(filePath);
                values = lines[0].Split(',');

                int criteriaNum = 7;
                double[,] weatherData = new double[criteriaNum, values.Length];

                if (lines.Length != criteriaNum)
                {
                    Console.WriteLine(LanguageHandler.GetString("InvalidNumberOfLines"));
                    return null;
                }

                for (int i = 1; i < criteriaNum; i++)
                {
                    values = lines[i].Split(',');
                    for (int j = 1; j < values.Length; j++)
                    {
                        if (i == 1)
                        {
                            weatherData[i,j] =  ProcessTemperatureData(values, j, criteria);
                        }
                        else if (i >= 2 && i <= 4)
                        {
                            // Wind, Humidity, Precipitation
                            weatherData[i, j] = ProcessParameterData(values, i, j, criteria);
                        }
                        else if (i == 5)
                        {
                            weatherData[i, j] = ProcessLightningData(values, j, criteria);
                        }
                        else if (i == 6)
                        {
                            weatherData[i, j] = ProcessCloudsData(values, j, criteria);
                        }
                    }


                }

                double[] score = new double[values.Length];
                bool flag = false;

                if(values != null)
                {
                    for (int i = 1; i < values.Length; i++)
                    {
                        for (int j = 1; j < criteriaNum; j++)
                        {
                            if (weatherData[j, i] == -1)
                            {
                                flag = true;
                                continue;
                            }
                            score[i] += weatherData[j, i];
                        }
                        if (flag)
                        {
                            score[i] = -1;
                        }
                        flag = false;
                    }
                    for (int i = 1; i < values.Length; i++)
                    {
                        if (score[i] != -1)
                        {
                            scores.Add(i, score[i]);
                        }
                    }
                    if (scores.Count > 0)
                    {
                        var minScore = scores.Values.Min();
                        var minScoreDay = scores.First(kvp => kvp.Value == minScore).Key;
                        // Save the minimum score and its corresponding day
                        scores.Clear();
                        scores[minScoreDay] = minScore;
                    }
                }


                return scores;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(LanguageHandler.GetString("UnexpectedErrorMessage"),ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(LanguageHandler.GetString("UnknownSpaceport"),ex.Message);
                return null;
            }
        }



        public static Spaceport FindBestCombination(List<Spaceport> spaceports)
        {

            double lowestScore = double.MaxValue;
            Spaceport spaceportWithLowestScore = null;

            foreach (Spaceport spaceport in spaceports)
            {
                double distanceFromEquator = GetDistanceFromEquator(spaceport);
                spaceport.Score *= distanceFromEquator;
            }
            
            foreach (Spaceport spaceport in spaceports)
            {
                if (spaceport.Score < lowestScore)
                {
                    lowestScore = spaceport.Score;
                    spaceportWithLowestScore = spaceport;
                }
            }

            return spaceportWithLowestScore;
        }


        private static double GetDistanceFromEquator(Spaceport spaceport)
        {
            //Calculates the position in relation of the Equator
            if (spaceport.FileName.Contains("Kourou"))
            {
                spaceport.FileName = "Kourou";
                return 1.0 / 5.1559;
            }

            else if (spaceport.FileName.Contains("Tanegashima"))
            {
                spaceport.FileName = "Tanegashima";
                return 1.0 / 30.6046;

            }
            else if (spaceport.FileName.Contains("Cape"))
            {
                spaceport.FileName = "Cape Canaveral";
                return 1.0 / 28.3922;

            }
            else if (spaceport.FileName.Contains("Mahia"))
            {
                spaceport.FileName = "Mahia";
                return 1.0 / Math.Abs(-39.1412);

            }
            else if (spaceport.FileName.Contains("Kodiak"))
            {
                spaceport.FileName = "Kodiak";
                return 1.0 / 57.79;
            }
            else
            {
                Console.WriteLine($"Unknown spaceport: {spaceport.FileName}. Cannot calculate distance.");
                return 0.0;
            }
        }
        //The best day is calculated by scoring the criterias and their values form the files

        //Assign 0.2 weight to the temperature's deviation from the avarage temperature based on the criterias
        //The weight is approximation based on the importance of the criteria
        private double ProcessTemperatureData(string[] values, int j, Criteria criteria)
        {
            double score;
            int value;
            if (!int.TryParse(values[j], out value))
            {
                throw new FormatException(string.Format(LanguageHandler.GetString("InvalidDataFormatForTemperature"), j));
            }

            if (value < criteria.MinTemperature || value > criteria.MaxTemperature)
            {
                score = -1;
            }
            else
            {
                score = 0.2 * (double)Math.Abs(value - (criteria.MaxTemperature - criteria.MinTemperature)/2); //Deviatioin from the avarage temperature
            }
            return score;
        }

        //Assign weight 0.4 for Wind's speed, 0.1 for Humidy's %, and takes the value of the Precipitation

        private double ProcessParameterData(string[] values, int i, int j, Criteria criteria)
        {
            double score;
            int value;
            if (!int.TryParse(values[j], out value))
            {
                throw new FormatException(string.Format(LanguageHandler.GetString("InvalidDataFormatForDayParameter"), i, j));
            }

            if ((i == 2 && value > criteria.MaxWind) || (i == 3 && value > criteria.MaxHumidity) || (i == 4 && value > criteria.MaxPrecipitation))
            {
                score = -1;
            }
            else
            {
                switch (i)
                {
                    case 2:
                        score = 0.4 * (double)value;
                        break;
                    case 3:
                        score = 0.1 * (double)value;
                        break;
                    case 4:
                        score = value;
                        break;
                    default:
                        score = -1;
                        break;
                }
            }
            return score;
        }

        //Depending if Lightnings are accepted adds score
        private double ProcessLightningData(string[] values, int j, Criteria criteria)
        {
            double score;
            if (criteria.AcceptLightnings)
            {
                if (values[j].ToLower() == "yes")
                {
                    score = 0.5;
                }
                else
                {
                    score = 0;

                }
            }
            else
            {
                if (values[j].ToLower() == "yes")
                {
                    score = -1;
                }
                else if (values[j].ToLower() == "no")
                {
                    score = 0;
                }
                else
                {
                    throw new FormatException(string.Format(LanguageHandler.GetString("InvalidDataFormatForLightning"), j));
                }
            }
            return score;
        }

        // Adds priority to the different types of clouds
        private double ProcessCloudsData(string[] values, int j, Criteria criteria)
        {
            double score;
            int value;

            switch (values[j].ToLower())
            {
                case "cirrus":
                    value = 0;
                    break;
                case "stratus":
                    value = 1;
                    break;
                case "cumulus":
                    value = 2;
                    break;
                case "nimbus":
                    value = 3;
                    break;
                default:
                    throw new FormatException(string.Format(LanguageHandler.GetString("InvalidCloudFormat"), values[j], j));
            }
            //Assign weight 0.3 to the cloud criteria
            if (value != -1 && criteria.AcceptableCloudTypes.Contains(values[j].Trim().ToLower()))
            {
                score = 0.3 * (double)value;
            }
            else
            {
                score = -1;
            }
            return score;
        }


    }
}
