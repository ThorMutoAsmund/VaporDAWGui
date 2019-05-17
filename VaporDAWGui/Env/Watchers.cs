using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Watchers : IStartUp
    {
        public Subscribable<IEnumerable<SampleInfo>> SamplesList = new Subscribable<IEnumerable<SampleInfo>>(new List<SampleInfo>());
        public Subscribable<IEnumerable<ScriptInfo>> ScriptsList = new Subscribable<IEnumerable<ScriptInfo>>(new List<ScriptInfo>());

        private FileSystemWatcher samplesWatcher;
        private FileSystemWatcher scriptsWatcher;

        public void StartUp()
        {
            Env.Project.Loaded.ValueChanged += loaded =>
            {
                if (loaded)
                {
                    ConfigureWatchers();
                    RescanSamples();
                    RescanSripts();
                }
                else
                {
                    StopWatchers();
                }
            };
        }

        private void ConfigureWatchers()
        {
            // Samples
            var context = SynchronizationContext.Current;
            this.samplesWatcher = new FileSystemWatcher()
            {
                Path = Path.Combine(Env.Project.ProjectPath, Env.Conf.SamplesFolder),
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.wav",
            };
            this.samplesWatcher.Changed += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Created += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Deleted += (source, e) => context.Post(val => RescanSamples(), source);
            this.samplesWatcher.Renamed += (source, e) => context.Post(val => RescanSamples(), source);

            this.samplesWatcher.EnableRaisingEvents = true;

            // Scripts
            this.scriptsWatcher = new FileSystemWatcher()
            {
                Path = Path.Combine(Env.Project.ProjectPath, Env.Conf.ScriptsFolder),
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.js",
            };
            this.scriptsWatcher.Changed += (source, e) => context.Post(val => RescanSripts(), source);
            this.scriptsWatcher.Created += (source, e) => context.Post(val => RescanSripts(), source);
            this.scriptsWatcher.Deleted += (source, e) => context.Post(val => RescanSripts(), source);
            this.scriptsWatcher.Renamed += (source, e) => context.Post(val => RescanSripts(), source);

            this.scriptsWatcher.EnableRaisingEvents = true;

        }

        private void StopWatchers()
        {
            this.samplesWatcher.EnableRaisingEvents = false;
            this.samplesWatcher = null;
            this.scriptsWatcher.EnableRaisingEvents = false;
            this.scriptsWatcher = null;

            this.SamplesList.Clear();
            this.ScriptsList.Clear();
        }

        private void RescanSamples()
        {
            try
            {
                this.SamplesList.Value = Directory.EnumerateFiles(Path.Combine(Env.Project.ProjectPath, Env.Conf.SamplesFolder), "*.wav").Select(x => new SampleInfo()
                {
                    Name = System.IO.Path.GetFileName(x),
                    Path = x
                });
            }
            catch (Exception ex)
            {
                Env.LogException(ex);
                this.SamplesList.Clear();
            }
        }

        private void RescanSripts()
        {
            try
            {
                this.ScriptsList.Value = Directory.EnumerateFiles(Path.Combine(Env.Project.ProjectPath, Env.Conf.ScriptsFolder), "*.js").Select(x => new ScriptInfo()
                {
                    Name = System.IO.Path.GetFileName(x),
                    Path = x
                });
            }
            catch (Exception ex)
            {
                Env.LogException(ex);
                this.ScriptsList.Clear();
            }
        }

    }
}
