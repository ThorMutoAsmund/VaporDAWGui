using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public static class Env
    {
        public static Config Config { get; private set; } = new Config();
        public static Project Project { get; private set; } = new Project();
        public static Composer Composer { get; set; } 
    }
}
