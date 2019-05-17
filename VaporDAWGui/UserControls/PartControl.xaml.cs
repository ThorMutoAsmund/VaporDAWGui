using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace VaporDAWGui
{

    /// <summary>
    /// Interaction logic for MyUserControl.xaml 
    /// </summary> 

    public partial class PartControl : UserControl
    {
        private enum PartInteractionType { None, Drag, ResizeRight }

        private TranslateTransform transform;
        private PartInteractionType currentInteraction = PartInteractionType.None;
        private bool dragSnapLeft;
        private PartControl[] snapCandidates;
        private bool wasInteractedWith;
        //private double draggedX;
        //private double resizedWidth;
        private int draggedRow;
        private Point clickPosition;
        private double clickTransformX;
        private double clickWidth;
        private PartInteractionType whenClicked;

        public string Id { get; set; }
        //public double StartTime { get; private set; }
        //public double Duration { get; private set; }
        //public double EndTime => this.StartTime + this.Duration;

        public double StartPos => this.transform.X;
        public double EndPos => this.transform.X + this.Width;
        public double StartTime => this.StartPos * Env.Composer.SecondsPerPixel + Env.Composer.TimeOffset;
        public double Duration => this.Width * Env.Composer.SecondsPerPixel;

        public string Title
        {
            get => this.titleTextBlock.Text;
            set => this.titleTextBlock.Text = value;
        }

        public PartControl()
        {
            InitializeComponent();

            this.transform = new TranslateTransform();
            this.RenderTransform = this.transform;

            this.MouseLeftButtonDown += (object sender, MouseButtonEventArgs e) =>
            {
                this.snapCandidates = null;
                this.wasInteractedWith = false;
                this.currentInteraction = this.whenClicked;

                switch (this.whenClicked)
                {
                    case PartInteractionType.Drag:
                        {
                            Env.Composer.BringToFront(this);

                            this.clickPosition = e.GetPosition(this.Parent as UIElement);
                            this.clickTransformX = this.transform.X;

                            break;
                        }
                    case PartInteractionType.ResizeRight:
                        {
                            this.clickPosition = e.GetPosition(this.Parent as UIElement);
                            this.clickWidth = this.Width;
                            this.clickTransformX = this.transform.X;
                            break;
                        }
                }

                this.snapCandidates = GetSnapCandidates();
                this.CaptureMouse();
            };

            this.MouseLeftButtonUp += (object sender, MouseButtonEventArgs e) =>
            {
                this.ReleaseMouseCapture();

                if (this.wasInteractedWith)
                {
                    switch (this.currentInteraction)
                    {
                        case PartInteractionType.Drag:
                            {
                                Env.Project.UpdatePartStartTimeAndDuration(this.Id, this.StartTime, this.Duration);
                                Env.Project.UpdatePartTrack(this.Id, this.draggedRow);
                                break;
                            }
                        case PartInteractionType.ResizeRight:
                            {
                                Env.Project.UpdatePartStartTimeAndDuration(this.Id, this.StartTime, this.Duration);
                                break;

                            }
                    }

                }

                this.currentInteraction = PartInteractionType.None;
            };

            this.MouseLeave += (sender, e) =>
            {
                this.currentInteraction = PartInteractionType.None;
            };

            this.MouseMove += (sender, e) =>
            {
                switch (this.currentInteraction)
                {
                    case PartInteractionType.Drag:
                        {
                            this.wasInteractedWith = true;
                            Point currentPosition = e.GetPosition(this.Parent as UIElement);

                            var draggedX = this.clickTransformX + currentPosition.X - clickPosition.X;
                            var newDraggedRow = MathX.Clamp((int)(currentPosition.Y / Env.Composer.RowHeight), 0, Env.Composer.NumberOfRows - 1);

                            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                            {
                                var snapCandidate = this.snapCandidates.Select(p => (snapEnd: true, part: p, distance: Math.Abs(p.EndPos - draggedX))).
                                Concat(this.snapCandidates.Select(p => (snapEnd: false, part: p, distance: Math.Abs(p.StartPos - draggedX)))).
                                    Where(c => c.distance < Env.Conf.SnapMargin).OrderBy(c => c.distance).FirstOrDefault();
                                if (snapCandidate.part != null)
                                {
                                    draggedX = snapCandidate.snapEnd ? snapCandidate.part.EndPos : snapCandidate.part.StartPos;
                                }
                            }
                            else if (Keyboard.IsKeyDown(Key.RightCtrl))
                            {
                                var snapCandidate = this.snapCandidates.Select(p => (snapEnd: false, part: p, distance: Math.Abs(p.StartPos - (draggedX + this.Width)))).
                                Concat(this.snapCandidates.Select(p => (snapEnd: true, part: p, distance: Math.Abs(p.EndPos - (draggedX + this.Width))))).
                                    Where(c => c.distance < Env.Conf.SnapMargin).OrderBy(c => c.distance).FirstOrDefault();
                                if (snapCandidate.part != null)
                                {
                                    draggedX = (snapCandidate.snapEnd ? snapCandidate.part.EndPos : snapCandidate.part.StartPos) - this.Width;
                                }
                            }

                            this.draggedRow = newDraggedRow;
                            this.transform.X = draggedX;
                            Grid.SetRow(this, this.draggedRow);
                            break;
                        }
                    case PartInteractionType.ResizeRight:
                        {
                            this.wasInteractedWith = true;
                            Point currentPosition = e.GetPosition(this.Parent as UIElement);
                            var resizedWidth = this.clickWidth + currentPosition.X - clickPosition.X;

                            if (Keyboard.IsKeyDown(Key.LeftCtrl))
                            {
                                var snapCandidate = this.snapCandidates.Select(p => (snapEnd: true, part: p, distance: Math.Abs(p.EndPos - (this.StartPos + resizedWidth)))).
                                Concat(this.snapCandidates.Select(p => (snapEnd: false, part: p, distance: Math.Abs(p.StartPos - (this.StartPos + resizedWidth))))).
                                    Where(c => c.distance < Env.Conf.SnapMargin).OrderBy(c => c.distance).FirstOrDefault();
                                if (snapCandidate.part != null)
                                {
                                    resizedWidth = (snapCandidate.snapEnd ? snapCandidate.part.EndPos : snapCandidate.part.StartPos) - this.StartPos;
                                }
                            }

                            this.Width = Math.Max(resizedWidth, Env.Conf.MinPartWidth);

                            break;
                        }
                    case PartInteractionType.None:
                        {
                            Point currentPosition = e.GetPosition(this);

                            if (currentPosition.X > this.Width - 3)
                            {
                                this.Cursor = Cursors.SizeWE;
                                this.whenClicked = PartInteractionType.ResizeRight;
                            }
                            else
                            {
                                this.Cursor = Cursors.Hand;
                                this.whenClicked = PartInteractionType.Drag;
                                this.dragSnapLeft = currentPosition.X < this.Width / 2;
                            }
                            break;
                        }
                }
            };

            this.editStartAndDurationMenuItem.Click += (sender, e) => EditStartAndDuration();
            this.renameMenuItem.Click += (sender, e) => Rename();
        }

        private void EditStartAndDuration()
        {
            var window = new EditStartTimeAndDurationWindow()
            {
                Owner = Env.MainWindow,
                StartTime = this.StartTime,
                Duration = this.Duration
            };
            if (window.ShowDialog() ?? false)
            {
                Env.Project.UpdatePartStartTimeAndDuration(this.Id, window.StartTime, window.Duration);

                SetStartTimeAndDuration(window.StartTime, window.Duration);
            }
        }

        private void Rename()
        {
            var window = new EditStringWindow()
            {
                Owner = Env.MainWindow,
                Value = this.Title
            };
            if (window.ShowDialog() ?? false)
            {
                Env.Project.UpdatePartTitle(this.Id, window.Value);

                this.Title = window.Value;
            }
        }

        private PartControl[] GetSnapCandidates()
        {
            //return Env.Composer.GetPartsInRow(newDraggedRow).ToArray();
            return Env.Composer.GetAllParts().Where(p => p != this).ToArray();
        }

        public void SetStartTimeAndDuration(double startTime, double duration)
        {
            // Calc new position and width
            var x = (startTime - Env.Composer.TimeOffset) / Env.Composer.SecondsPerPixel;
            var w = duration / Env.Composer.SecondsPerPixel;

            this.transform.X = x;
            this.Width = w;
        }
    }
}
