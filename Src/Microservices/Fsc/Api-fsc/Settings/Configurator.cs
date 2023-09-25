﻿using Api_fsc_Entities.DTO;
using Api_fsc_Entities.Models;
using check_up_money.Cypher;
using FreeSpaceChecker.Settings.CheckObject;
using FreeSpaceChecker.Settings.Email;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
        public (string admLogin, string loginSalt, string admPass, string passSalt) LoadAdminSettings()
        {
            Configuration config = LoadConfig();
            var myConfig = config.GetSection("settings") as SettingsConfiguration;

            return (myConfig.AdminLogin, myConfig.LoginSalt, myConfig.AdminPass, myConfig.PassSalt);
        }
        public void SaveAdminSettings((string admLogin, string loginSalt, string admPass, string passSalt) req)
        {
            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            myConfig.AdminLogin = req.admLogin;
            myConfig.LoginSalt = req.loginSalt;
            myConfig.AdminPass = req.admPass;
            myConfig.PassSalt = req.passSalt;

            myConfig.CurrentConfiguration.Save();
        }
        public List<DeviceSettingDTO> LoadCompSettings()
        {
            List<DeviceSettingDTO> comps = new();

            SettingsConfiguration myConfig = (SettingsConfiguration)ConfigurationManager.GetSection("settings");

            foreach(CheckObjectElement compSetting in myConfig.CheckObjects)
            {
                DeviceSettingDTO comp = new();
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
        public (bool sendEmail, string smtpServer, string mailFrom) LoadSmtpSettings()
        {
            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            bool sendEmail = false;
            bool.TryParse(myConfig.Emails.SendEmail, out sendEmail);

            return (sendEmail, myConfig.Emails.SmtpServer, myConfig.Emails.MailFrom);
        }
        public List<Mail> LoadMailSettings()
        {
            List<Mail> emails = new List<Mail>();

            Configuration config = LoadConfig();
            SettingsConfiguration myConfig = config.GetSection("settings") as SettingsConfiguration;

            foreach (EmailElement mailSetting in myConfig.Emails)
            {
                Mail mail = new Mail();
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