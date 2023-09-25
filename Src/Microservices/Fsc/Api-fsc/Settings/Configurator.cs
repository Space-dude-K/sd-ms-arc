using Api_fsc_Entities.Models;
using FreeSpaceChecker.Settings.CheckObject;
using FreeSpaceChecker.Settings.Email;
using System.Configuration;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace FreeSpaceChecker.Settings
{
    class Configurator
    {
        enum SectionTypes
        {
            Comp,
            Mail
        }
        public Configurator()
        {
        }
        #region User Helpers Comps, Emails
        private Configuration LoadConfig()
        {
            #if DEBUG
            string applicationName =
                Environment.GetCommandLineArgs()[0];
            #else
                string applicationName =
                    Environment.GetCommandLineArgs()[0];
                    //Environment.GetCommandLineArgs()[0]+ ".exe";
            #endif

            string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);

            Console.WriteLine("Opening conf -> " + exePath);

            // Get the configuration file. The file name has
            // this format appname.exe.config.
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(exePath);

            return config;
        }
        public AdminSetting LoadAdminSettings()
        {
            Configuration config = LoadConfig();
            var myConfig = config.GetSection("settings") as SettingsConfiguration;

            return new AdminSetting()
            { 
                AdminLogin = myConfig.AdminLogin, 
                AdminLoginSalt = myConfig.LoginSalt, 
                AdminPassword = myConfig.AdminPass, 
                AdminPasswordSalt = myConfig.PassSalt 
            };
        }
        public void SaveAdminSettings(AdminSetting req)
        {
            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            myConfig.AdminLogin = req.AdminLogin;
            myConfig.LoginSalt = req.AdminLoginSalt;
            myConfig.AdminPass = req.AdminPassword;
            myConfig.PassSalt = req.AdminPasswordSalt;

            myConfig.CurrentConfiguration.Save();
        }
        public List<DeviceSetting> LoadCompSettings()
        {
            List<DeviceSetting> comps = new();

            SettingsConfiguration myConfig = (SettingsConfiguration)ConfigurationManager.GetSection("settings");

            foreach(CheckObjectElement compSetting in myConfig.CheckObjects)
            {
                DeviceSetting comp = new();
                comp.Ip = compSetting.ObjectIp;

                if (compSetting.ObjectDisks != string.Empty)
                {
                    comp.Disks = compSetting.ObjectDisks;
                }
                else
                {
                    throw new Exception("Null setting exception.");
                }
                    
                comps.Add(comp);
            }

            return comps;
        }
        public SmtpSetting LoadSmtpSettings()
        {
            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            bool sendEmail = false;
            bool.TryParse(myConfig.Emails.SendEmail, out sendEmail);

            return new SmtpSetting() 
            { 
                SendEmail = sendEmail, 
                StmpServerAddress = myConfig.Emails.SmtpServer, 
                MailFrom = myConfig.Emails.MailFrom 
            };
        }
        public List<Mail> LoadMailSettings()
        {
            List<Mail> emails = new();

            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            foreach (EmailElement mailSetting in myConfig.Emails)
            {
                Mail mail = new();
                mail.Email = mailSetting.Email;
                mail.Subject = mailSetting.Subject;

                emails.Add(mail);
            }

            return emails;
        }
        public string GetLoggerPath()
        {
            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            string cfgPath;

            if (!string.IsNullOrEmpty(myConfig.CheckObjects.Loggerpath) && Directory.Exists(myConfig.CheckObjects.Loggerpath))
                cfgPath = myConfig.CheckObjects.Loggerpath;
            else
            {
                cfgPath = Environment.CurrentDirectory;
            }

            return cfgPath;
        }
        #endregion
    }
}