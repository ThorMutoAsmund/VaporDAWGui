using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace VaporDAWGui
{
    /// <summary>
    /// Interaction logic for ScriptTabItem.xaml
    /// </summary>
    public partial class ScriptTabItem : TabItem
    {
        public ScriptInfo Script { get; private set; }

        public ScriptTabItem(ScriptInfo script)
        {
            InitializeComponent();

            this.Script = script;
            this.Header = script.Name;
        }
    }
}
