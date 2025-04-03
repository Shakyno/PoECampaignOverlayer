using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Overlayer
{
    internal class OverlayData
    {
        public Settings Settings { get; set; }
        public List<string> CampaignText { get; set; }
        public int StepCountWithoutActs
        { 
            get
            {
                List<string> prefixes = new List<string> { "act 1", "act 2", "act 3", "act 4", "act 5", "act 6" };

                return CampaignText.Count(item =>
                    !prefixes.Any(prefix => item.ToLower().StartsWith(prefix)));
            }
        }
         

        public OverlayData(Settings settings)
        {
            Settings = settings;
            CampaignText = new List<string>();

            if (Settings.CampaignTextFilePath != null)
            {
                ReadCampaignTextFile(Settings.CampaignTextFilePath);
            }
        }

        /// <summary>
        /// Reads the campaign text file
        /// </summary>
        /// <param name="path"></param>
        public void ReadCampaignTextFile(string path)
        {
            CampaignText.Clear();

            // Read the file and display it line by line.
            StreamReader file = new StreamReader(path);

            if (file == null)
            {
               MessageBox.Show("Campaign textfile not found or has the wrong format!");
            }

            string line;
            while ((line = file.ReadLine()) != null)
            {
                CampaignText.Add(line);
            }
            file.Close();
        }

    }
}
