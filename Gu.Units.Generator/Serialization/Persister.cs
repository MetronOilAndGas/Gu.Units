namespace Gu.Units.Generator
{
    using System;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public static class Persister
    {
        public static string SettingsFileName
        {
            get
            {
                var directoryInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
                var directory = directoryInfo.Parent.Parent.Parent.FullName; // Perhaps not the most elegant code ever written
                return System.IO.Path.Combine(directory, "Units.txt");
            }
        }

        private static Settings settings;

        public static bool IsDirty
        {
            get
            {
                if (settings == null)
                {
                    return true;
                }

                var json = JsonConvert.SerializeObject(settings);
                var file = File.ReadAllText(SettingsFileName);
                return json != file;
            }
        }

        public static Settings GetSettingsFromFile()
        {
            if (settings == null)
            {
                var json = File.ReadAllText(SettingsFileName);
                settings = JsonConvert.DeserializeObject<Settings>(json, CreateSettings());
            }
            return settings;
        }

        public static void Save(string fileName)
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(fileName, json);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = ExcludeCalculatedResolver.Default,
                MissingMemberHandling = MissingMemberHandling.Error
            };
        }
    }
}
