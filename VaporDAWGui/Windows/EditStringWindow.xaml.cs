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
    /// Interaction logic for EditStartAndDurationWindow.xaml
    /// </summary>
    public partial class EditStringWindow : Window
    {
        private string _value;
        public string Value
        {
            get => this.stringTextBox.Text;
            set
            {
                this._value = value;
                this.stringTextBox.Text = value;
            }
        }

        public string Label
        {
            get => this.stringLabel.Content.ToString();
            set => this.stringLabel.Content = value;
        }

        public EditStringWindow()
        {
            InitializeComponent();

            this.okButton.Click += (_,__) => this.DialogResult = true;
            this.cancelButton.Click += (_, __) => this.DialogResult = false;
        }
    }
}
