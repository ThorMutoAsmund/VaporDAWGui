using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VaporDAWGui
{
    public class Project : IStartUp
    {
        private int DefaultNumberOfTracks = 5;

        public Subscribable<bool> Loaded = new Subscribable<bool>(false);
        public Subscribable<bool> ChangesMade = new Subscribable<bool>(false);

        public string ProjectPath { get; private set; }
        public Song Song { get; private set; }
        

        public void StartUp()
        {
        }

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
            this.ChangesMade.Value = false;
            this.ProjectPath = path;

            // Ensure script directory
            var scriptDirectory = Path.Combine(Env.Project.ProjectPath, Env.Conf.ScriptsFolder);
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }

            // Ensure sample directory
            var sampleDirectory = System.IO.Path.Combine(Env.Project.ProjectPath, Env.Conf.SamplesFolder);
            if (!Directory.Exists(sampleDirectory))
            {
                Directory.CreateDirectory(sampleDirectory);
            }

            // Create or load project
            var projectFilePath = Path.Combine(Env.Project.ProjectPath, $"{Env.Conf.ProjectFileName}.json");
            if (File.Exists(projectFilePath))
            {
                OpenProject(projectFilePath);
            }
            else
            {
                CreateProject(projectFilePath);
            }

            Env.Conf.AddRecentFile(path);

            this.Loaded.Value = true;
        }

        public void Save()
        {
            this.ChangesMade.Value = false;
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
        private void OpenProject(string projectFilePath)
        {
            var songSerializer = SongSerializer.FromFile(projectFilePath);
            this.Song = songSerializer.Song;
        }

        private void CreateProject(string projectFilePath)
        {
            this.Song = new Song()
            {
                Title = "Untitled Song",
                CreationDate = DateTime.UtcNow,
                ChangeDate = DateTime.UtcNow,
                Tracks = new Track[DefaultNumberOfTracks]
            };

            for (int t=0; t< DefaultNumberOfTracks; ++t)
            {
                this.Song.Tracks[t] = new Track()
                {
                    Id = Base64.UUID()
                };
            }

            new SongSerializer()
            {
                Song = this.Song,
            }.ToFile(projectFilePath);
        }
    }
}
