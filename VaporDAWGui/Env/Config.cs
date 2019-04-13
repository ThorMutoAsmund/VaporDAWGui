using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Config
    {
        private List<string> recentFiles = new List<string>();

        public bool OpenDemoProjectOnLoad => true;
        public string ApplicationName => "Vapor DAW";
        public string AppPath => System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public string SamplesFolder => "samples";
        public IEnumerable<string> RecentFiles => this.recentFiles;

        public void AddRecentFile(string path)
        {
            if (!this.recentFiles.Contains(path))
            {
                this.recentFiles.Add(path);
            }
        }
    }
}
