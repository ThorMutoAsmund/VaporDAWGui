using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for SampleList.xaml
    /// </summary>
    public partial class ScriptList : UserControl
    {
        public ScriptInfo SelectedScript { get; set; }

        public ScriptList()
        {
            InitializeComponent();

            Env.Watchers.ScriptsList.ValueChanged += scriptsList =>
            {
                this.DataContext = scriptsList;
            };
        }

        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var item = (sender as Grid).DataContext as ScriptInfo;
                Env.MainWindow.EditScript(item);
            }
        }
    }
}
