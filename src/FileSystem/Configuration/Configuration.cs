namespace FileSystem.Configuration
{
    public class Configuration
    {
        public string PathToRootDir { get; set; }
        public string RootDirName { get; set; }
    }

    public class ConfigurationProvider
    {
        public string PathToConfigFile { get; set; }

        public ConfigurationProvider(string pathToConfigFile)
        {
            PathToConfigFile = pathToConfigFile;
        }

        public Configuration LoadConfig()
        {
            string json = System.IO.File.ReadAllText(PathToConfigFile);   
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Configuration>(json);
        }
    }
}
