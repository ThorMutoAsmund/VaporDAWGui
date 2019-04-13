using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Project
    {
        public Subscribable<bool> Loaded = new Subscribable<bool>(false);
        public Subscribable<string> ProjectPath = new Subscribable<string>();
        public Subscribable<bool> ChangesMade = new Subscribable<bool>(false);

        public void Close()
        {
            this.Loaded.Value = false;
            this.ChangesMade.Value = false;
        }

        public void Open(string path)
        {
            this.Loaded.Value = false;
            this.ProjectPath.Value = path;
            this.Loaded.Value = true;
            this.ChangesMade.Value = false;

            Env.Config.AddRecentFile(path);
        }

        public void Save()
        {
            this.ChangesMade.Value = false;
        }

        public string GenerateId()
        {
            return new Guid().ToString();
        }

        public void SetPartStartTime(string id, double startTime)
        {
            this.ChangesMade.Value = true;
        }

        public void SetPartDuration(string id, double duration)
        {
            this.ChangesMade.Value = true;
        }

        public void SetPartTrack(string id, int track)
        {
            this.ChangesMade.Value = true;
        }
    }
}
