using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for SampleList.xaml
    /// </summary>
    public partial class ScriptList : UserControl
    {
        public ScriptInfo SelectedScript { get; set; }

        private FileSystemWatcher watcher;

        public ScriptList()
        {
            InitializeComponent();

            Env.Project.Loaded.ValueChanged += (sender, e) =>
            {
                if (!e)
                {
                    DataContext = null;
                }
                else
                {
                    ConfigureWatcher();
                    RescanSripts();
                }
            };
        }

        private void ConfigureWatcher()
        {
            var context = SynchronizationContext.Current;
            this.watcher = new FileSystemWatcher()
            {
                Path = System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.ScriptsFolder),
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.js",
            };
            this.watcher.Changed += (source, e) => context.Post(val => RescanSripts(), source);
            this.watcher.Created += (source, e) => context.Post(val => RescanSripts(), source);
            this.watcher.Deleted += (source, e) => context.Post(val => RescanSripts(), source);
            this.watcher.Renamed += (source, e) => context.Post(val => RescanSripts(), source);

            this.watcher.EnableRaisingEvents = true;
        }

        private void RescanSripts()
        {
            try
            {
                this.DataContext = Directory.EnumerateFiles(System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.ScriptsFolder), "*.js").Select(x => new ScriptInfo()
                {
                    Name = System.IO.Path.GetFileName(x),
                    Path = x
                });
            }
            catch
            {
                this.DataContext = null;
                return;
            }
        }

    }
}
