using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TimeZoneNames;

namespace WindowsTimezoneLocales
{
    class Program
    {
        static void Main(string[] args)
        {
            var langs = new string[] {
                "en-US",
                "en-GB",
                "en-AU",
                "fr-FR",
                "fr-CA",
                "de-DE",
                "it-IT",
                "es-ES",
                "es-419",
                "ja-JP",
                "pt-BR",
                "zh-CN",
                "zh-TW",
                "zh-HK",
            };

            var timezones = File.ReadLines("timezones.csv");

            var folder = "translated";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            else
            {
                Directory.Delete(folder, true);
            }

            System.Threading.Thread.Sleep(1000);

            foreach (var lang in langs)
            {
                var sb = new StringBuilder();
                sb.AppendLine("{");

                var lines = new List<string>();
                var converLang = getConvertName(lang);

                foreach (var timezone in timezones)
                {
                    var translated = TZNames.GetNamesForTimeZone(timezone, converLang);

                    var selected = !string.IsNullOrWhiteSpace(translated.Generic) ? translated.Generic : translated.Standard;

                    Console.WriteLine("{0}: {1}", timezone, selected);
                    lines.Add($"    \"{timezone}\": \"{selected}\"");
                }
                sb.Append(string.Join(",", lines));
                sb.AppendLine("}");
                File.WriteAllText($"{folder}/{lang}-cldr.json", sb.ToString());
            }

        }

        private static string getConvertName(string lang)
        {
            switch (lang)
            {
                case "zh-CN":
                    return "zh-Hans";
                case "zh-TW":
                case "zh-HK":
                    return "zh-Hant";
                default:
                    return lang;
            }
        }
    }
}
