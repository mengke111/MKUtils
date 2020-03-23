using System.Configuration;

namespace MK
{
    public class ConfigEx
    {
        public static void SetConfig(string profilePath, string v)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[v].Value = profilePath;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            
        }

        public static string GetConfig(string profilePath)
        {
            return  ConfigurationManager.AppSettings[profilePath];
        }
    }
}