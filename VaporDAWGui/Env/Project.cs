using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VaporDAWGui
{
    public class Project
    {
        public Subscribable<bool> Loaded = new Subscribable<bool>(false);
        public Subscribable<string> ProjectPath = new Subscribable<string>();
        public Subscribable<bool> ChangesMade = new Subscribable<bool>(false);
        public Subscribable<Song> Song = new Subscribable<Song>(false);

        public void Close()
        {
            this.Loaded.Value = false;
            this.ChangesMade.Value = false;
        }

        public void Open(string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show(path, "Path not found", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            this.Loaded.Value = false;
            this.ProjectPath.Value = path;
            this.Loaded.Value = true;
            this.ChangesMade.Value = false;

            // Ensure script directory
            var scriptDirectory = Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.ScriptsFolder);
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }

            // Ensure sample directory
            var sampleDirectory = System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.SamplesFolder);
            if (!Directory.Exists(sampleDirectory))
            {
                Directory.CreateDirectory(sampleDirectory);
            }

            Env.Conf.AddRecentFile(path);
        }

        public void Save()
        {
            this.ChangesMade.Value = false;
        }

        public string GenerateId()
        {
            return new Guid().ToString();
        }

        public void UpdatePartStartTimeAndDuration(string id, double startTime, double duration)
        {
            this.ChangesMade.Value = true;
        }

        public void UpdatePartTitle(string id, string title)
        {
            this.ChangesMade.Value = true;
        }

        public void UpdatePartTrack(string id, int track)
        {
            this.ChangesMade.Value = true;
        }
    }
}
