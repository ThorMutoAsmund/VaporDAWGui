using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Part
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public string Track { get; set; }

        public double StartTime { get; set; }

        public double Duration { get; set; }
    }
}
