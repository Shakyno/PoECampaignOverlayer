using System;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace Overlayer
{
    internal class Settings
    {
        public string? CampaignTextFilePath { get; set; }
        public string? HotkeyBack { get; set; }
        public string? HotkeyNext { get; set; }
        public string? UserName { get; set; }
        public int? CurrentStep { get; set; }

        /// <summary>
        /// Reads settings file and sets the properties.
        /// </summary>
        public static Settings ReadSettings()
        {
            string fileName = "CampaignOverlayerSettings.json";
            string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PoECampaignOverlayer", fileName);

            string filePath = File.Exists(localPath) ? localPath : documentsPath;

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Settings file not found.");
            }

            string jsonContent = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(jsonContent))
            {
                MessageBox.Show("Settings file is empty.");
                return null;
            }

            return JsonConvert.DeserializeObject<Settings>(jsonContent);
        }

        /// <summary>
        /// Saves the settings to the settings file.
        /// </summary>
        public void SaveSettings()
        {
            string fileName = "CampaignOverlayerSettings.json";
            string localPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "PoECampaignOverlayer", fileName);
            string filePath = File.Exists(localPath) ? localPath : documentsPath;
            string jsonContent = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, jsonContent);
        }
    }
}
