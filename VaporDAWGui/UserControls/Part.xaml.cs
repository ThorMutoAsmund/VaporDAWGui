using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace VaporDAWGui
{

    /// <summary>
    /// Interaction logic for MyUserControl.xaml 
    /// </summary> 

    public partial class Part : UserControl
    {
        private bool isDragging;
        private bool wasDragged;
        private double draggedX;
        private int draggedRow;
        private Point clickPosition;

        public string Id { get; set; }
        public double StartTime { get; private set; }
        public double Duration { get; private set; }

        public string Title
        {
            get => this.titleTextBlock.Text;
            set => this.titleTextBlock.Text = value;
        }

        public Part()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                this.isDragging = true;
                this.wasDragged = false;
                var draggableControl = sender as UserControl;
                this.clickPosition = e.GetPosition(this);
                draggableControl.CaptureMouse();
            };

            this.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
            {
                this.isDragging = false;
                var draggable = sender as UserControl;
                draggable.ReleaseMouseCapture();

                if (this.wasDragged)
                {
                    Env.Project.SetPartStartTime(this.Id, this.draggedX * Env.Composer.SecondsPerPixel + Env.Composer.TimeOffset);
                }
            };

            this.MouseMove += (object sender, MouseEventArgs e) =>
            {
                var draggableControl = sender as UserControl;

                if (this.isDragging && draggableControl != null)
                {
                    this.wasDragged = true;
                    Point currentPosition = e.GetPosition(this.Parent as UIElement);

                    var transform = draggableControl.RenderTransform as TranslateTransform;
                    if (transform == null)
                    {
                        transform = new TranslateTransform();
                        draggableControl.RenderTransform = transform;
                    }

                    this.draggedX = currentPosition.X - clickPosition.X;
                    transform.X = this.draggedX;
                    this.draggedRow = MathX.Clamp((int)((currentPosition.Y - clickPosition.Y) / 50), 0, Env.Composer.NumberOfRows-1);
                    Grid.SetRow(draggableControl, this.draggedRow);
                }
            };
        }

        public void SetStartTimeAndDuration(double startTime, double duration)
        {
            this.StartTime = startTime;
            this.Duration = duration;

            // Calc new position and width
            var x = (this.StartTime - Env.Composer.TimeOffset) / Env.Composer.SecondsPerPixel;
            var w = this.Duration / Env.Composer.SecondsPerPixel;

            //this.RenderTransform.X = x;

        }
    }
}
