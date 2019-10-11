using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VaporDAWGui
{
    public class SongSerializer
    {
        public static int CurrentVersion = 1;

        public Song Song { get; set; }
        public SongSerializer()
        {
        }

        public void ToFile(string projectFilePath)
        {
            this.Song.Ver = CurrentVersion;

            string json;

            try
            {
                json = JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error serializing project file. {ex.Message}", "Serializer error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            try
            {
                File.WriteAllText(projectFilePath, json);
            }
            catch (Exception)
            {
                MessageBox.Show("Error writing project file", "Serializer error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
        }

        public static SongSerializer FromFile(string projectFilePath)
        {
            string json;
            try
            {
                json = File.ReadAllText(projectFilePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error reading project file", "Serializer error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return null;
            }

            try
            {
                return JsonConvert.DeserializeObject<SongSerializer>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deserializing project file. {ex.Message}", "Serializer error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return null;
            }

        }
    }
}
