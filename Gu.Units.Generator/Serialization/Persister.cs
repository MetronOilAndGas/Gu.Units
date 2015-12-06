namespace Gu.Units.Generator
{
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;

    public static class Persister
    {
        public static string FullFileName
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
                var file = File.ReadAllText(FullFileName);
                return json != file;
            }
        }

        public static Settings GetSettings()
        {
            return settings ?? (settings = JsonConvert.DeserializeObject<Settings>(Properties.Resources.Units));
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText(FullFileName, json);
        }

        private static JsonSerializerSettings CreateSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = ExcludeCalculatedResolver.Default
            };
        }
    }
}
