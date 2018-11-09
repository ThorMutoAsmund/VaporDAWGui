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

        public void Close()
        {
            this.Loaded.Value = false;
        }

        public void Open(string path)
        {
            this.Loaded.Value = false;

            this.ProjectPath.Value = path;
            this.Loaded.Value = true;
        }
    }
}
