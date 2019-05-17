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

            Env.Watchers.SamplesList.ValueChanged += samplesList =>
            {
                this.DataContext = samplesList;
            };
        }
    }
}
