using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Config : IStartUp
    {
       
        public bool OpenDemoProjectOnLoad => true;
        public string ApplicationName => "Vapor DAW";
        public string AppPath => System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public string SamplesFolder => "Samples";
        public string ScriptsFolder => "Scripts";
        public string ProjectFileName => "Project";
        public List<string> RecentFiles { get; } = new List<string>();

        public int SnapMargin => 12;
        public int MinPartWidth => 10;
        public string TimeDurationFormat => "g5";

        public void StartUp()
        {
        }

        public void AddRecentFile(string path)
        {
            if (!this.RecentFiles.Contains(path))
            {
                this.RecentFiles.Add(path);
            }
        }
    }
}
