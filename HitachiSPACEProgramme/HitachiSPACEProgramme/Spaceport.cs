using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme
{
    internal class Spaceport
    {
        public string FileName { get; set; }
        public double Score { get; set; }
        public int Day { get; }

        public Spaceport(string fileName, double score, int day)
        {
            FileName = fileName;
            Score = score;
            Day = day;

        }
    }
}
