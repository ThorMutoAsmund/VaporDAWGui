using System.IO;
using System.Linq;
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
                    RescanSamples();
                }
            };

            Env.Project.ProjectPath.ValueChanged += (object sender, string e) =>
            {
                if (Env.Project.Loaded.Value)
                {
                    RescanSamples();
                }
            };
        }

        private void RescanSamples()
        {
            try
            {
                this.DataContext = Directory.EnumerateFiles(System.IO.Path.Combine(Env.Project.ProjectPath.Value, Env.Config.SamplesFolder), "*.wav").Select(x => new SampleInfo()
                {
                    Name = System.IO.Path.GetFileName(x),
                    Path = x
                });
                this.Content = null;
            }
            catch
            {
                //info of this folder was not able to get
                this.DataContext = null;
                this.Content = "Couldn't scan project path";
                return;
            }
        }

    }
}
