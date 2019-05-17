using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace VaporDAWGui
{
    public class StandardDialogWindow : Window
    {
        protected Button okButton;
        protected Button cancelButton;

        public StandardDialogWindow()
        {
            this.SizeChanged += (sender, e) =>
            {
                // calculates incorrect when window is maximized
                //this.MaxHeight = this.Height;
                //this.MinHeight = this.Height;
            };
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.okButton = (Button)Template.FindName("okButton", this);
            this.cancelButton = (Button)Template.FindName("cancelButton", this);

            this.okButton.Click += (_, __) => this.DialogResult = true;
            this.cancelButton.Click += (_, __) => this.DialogResult = false;

            this.Background = SystemColors.ControlBrush;
        }
    }
}
