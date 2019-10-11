using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for Composer.xaml
    /// </summary>
    public partial class Composer : UserControl
    {
        private int nextPartNumber = 1;
        private int lastTrackClicked;
        private double timeWhereLastClicked;

        public double TimeOffset { get; private set; }
        public double SecondsPerPixel { get; private set; }
        public int NumberOfRows { get; private set; }
        public double RowHeight { get; private set; }
        private double defaultDuration;

        public Composer()
        {
            InitializeComponent();

            Env.Composer = this;

            this.TimeOffset = 0F;
            this.SecondsPerPixel = 0.01F;
            this.defaultDuration = 10F;
            this.NumberOfRows = 10;
            this.RowHeight = 50F;

            this.MouseDown += (object sender, MouseButtonEventArgs e) =>
            {
                var position = e.GetPosition(this);
                this.timeWhereLastClicked = this.TimeOffset + this.SecondsPerPixel * position.X;
                this.lastTrackClicked = (int)(position.Y / this.RowHeight);
            };

            this.addPartMenuItem.Click += (sender, e) =>
                AddPart(Project.CreateUUID(), $"Part {nextPartNumber++}", this.lastTrackClicked, this.timeWhereLastClicked, this.defaultDuration);

            Env.Project.Loaded.ValueChanged += loaded =>
                UpdateAll(loaded ? Env.Project.Song : null);
        }

        private void UpdateAll(Song song)
        {
            if (song == null)
            {
                this.grid.Children.Clear();
            }
            else
            {
                foreach (var track in song.Tracks)
                {
                    this.grid.RowDefinitions.Add(new RowDefinition() {
                        Height = new GridLength(this.RowHeight),
                        Tag = track.Id
                    });
                }

                foreach (var part in song.Parts)
                {
                    var track = this.grid.RowDefinitions.FirstOrDefault(rd => rd.Tag as string == part.Track);

                    if (track == null)
                    {
                        MessageBox.Show($"Track with id 'part.Track' not found", "Update error", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        continue;
                    }

                    AddPart(part.Id, part.Title, this.grid.RowDefinitions.IndexOf(track), part.StartTime, part.Duration);
                }
            }
        }

        private void AddPart(string id, string title, int row, double startTime, double duration)
        {
            var partControl = new PartControl()
            {
                Id = id,
                Title = title
            };
            Grid.SetRow(partControl, row);
            this.grid.Children.Add(partControl);
            partControl.SetStartTimeAndDuration(startTime, duration);
        }

        public IEnumerable<PartControl> GetPartsInRow(int row)
        {
            return this.grid.Children.OfType<PartControl>().Where(element => Grid.GetRow(element) == row);
        }

        public IEnumerable<PartControl> GetAllParts()
        {
            return this.grid.Children.OfType<PartControl>();
        }

        public void BringToFront(PartControl part)
        {
            var maxZ = -10000;
            foreach (var child in this.grid.Children)
            {
                var z = Grid.GetZIndex(child as UIElement);
                if (z > maxZ)
                {
                    maxZ = z;
                }

            }
            Grid.SetZIndex(part, maxZ + 1);
        }

    }
}
