namespace RecruitmentTask.Helpers
{
    public interface IWebConfigReader
    {
        string GetWebConfigSetting(string settingKey);
    }

    public class WebConfigReader : IWebConfigReader
    {
        public string GetWebConfigSetting(string settingKey)
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings[settingKey];
        }
    }
}