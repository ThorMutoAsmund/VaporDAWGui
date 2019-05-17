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
                this.lastTrackClicked = (int)(position.Y / 50);
            };

            this.addPartMenuItem.Click += (sender, e) => AddPart();
        }

        private void AddPart()
        { 
            {
                var part = new Part()
                {
                    Id = Env.Project.GenerateId(),
                    Title = $"Part {nextPartNumber++}"
                };
                Grid.SetRow(part, this.lastTrackClicked);
                this.grid.Children.Add(part);

                part.SetStartTimeAndDuration(this.timeWhereLastClicked, this.defaultDuration);
            };
        }

        public IEnumerable<Part> GetPartsInRow(int row)
        {
            return this.grid.Children.OfType<Part>().Where(element => Grid.GetRow(element) == row);
        }

        public IEnumerable<Part> GetAllParts()
        {
            return this.grid.Children.OfType<Part>();
        }

        public void BringToFront(Part part)
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
