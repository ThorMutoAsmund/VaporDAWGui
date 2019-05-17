using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            File.WriteAllText(projectFilePath, json);
        }

        public static SongSerializer FromFile(string projectFilePath)
        {
            var json = File.ReadAllText(projectFilePath);
            return JsonConvert.DeserializeObject<SongSerializer>(json);
        }
    }
}
