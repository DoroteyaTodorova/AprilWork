using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HitachiSPACEProgramme.HitachiSPACEProgramme.Languages
{
    internal class LanguageHandler
    {
        private static ResourceManager _resourceManager;

        static LanguageHandler()
        {
            _resourceManager = new ResourceManager("HitachiSPACEProgramme.HitachiSPACEProgramme.Languages.en.language", Assembly.GetExecutingAssembly());
        }

        public static string? GetString(string name)
        {
            return _resourceManager.GetString(name);
        }

        public static void ChangeLanguage(string language)
        {
            _resourceManager = new ResourceManager($"HitachiSPACEProgramme.HitachiSPACEProgramme.Languages.{language}.language", Assembly.GetExecutingAssembly());

        }

    }
}
