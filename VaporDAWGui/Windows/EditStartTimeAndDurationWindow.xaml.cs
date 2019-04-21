using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for EditStartTimeAndDurationWindow.xaml
    /// </summary>
    public partial class EditStartTimeAndDurationWindow : StandardDialogWindow
    {
        private double _startTime;
        public double StartTime
        {
            get => double.TryParse(this.startTimeTextBox.Text, out double result) ? result : this._startTime;
            set
            {
                this._startTime = value;
                this.startTimeTextBox.Text = value.ToString(Env.Conf.TimeDurationFormat);
            }
        }

        private double _duration;
        public double Duration
        {
            get => double.TryParse(this.durationTextBox.Text, out double result) ? result : this._duration;
            set
            {
                this._duration = value;
                this.durationTextBox.Text = value.ToString(Env.Conf.TimeDurationFormat);
            }
        }

        public EditStartTimeAndDurationWindow()
        {
            InitializeComponent();
        }
    }
}
