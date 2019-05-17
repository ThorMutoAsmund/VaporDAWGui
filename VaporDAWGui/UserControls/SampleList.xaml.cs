﻿using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for SampleList.xaml
    /// </summary>
    public partial class SampleList : UserControl
    {
        public SampleInfo SelectedSample { get; set; }

        private FileSystemWatcher watcher;

        public SampleList()
        {
            InitializeComponent();

            Env.Project.Loaded.ValueChanged += (object sender, bool e) =>
            {
                if (!e)
                {
                    DataContext = null;
                }
                else
                {
                    ConfigureWatcher();
                    RescanSamples();
                }
            };
        }

        private void ConfigureWatcher()
        {
            var context = SynchronizationContext.Current;
            this.watcher = new FileSystemWatcher()
            {
                Path = System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.SamplesFolder),
                NotifyFilter = NotifyFilters.FileName,
                Filter = "*.wav",
            };
            this.watcher.Changed += (source, e) => context.Post(val => RescanSamples(), source);
            this.watcher.Created += (source, e) => context.Post(val => RescanSamples(), source);
            this.watcher.Deleted += (source, e) => context.Post(val => RescanSamples(), source);
            this.watcher.Renamed += (source, e) => context.Post(val => RescanSamples(), source);

            this.watcher.EnableRaisingEvents = true;
        }

        private void RescanSamples()
        {
            try
            {
                this.DataContext = Directory.EnumerateFiles(System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Conf.SamplesFolder), "*.wav").Select(x => new SampleInfo()
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
