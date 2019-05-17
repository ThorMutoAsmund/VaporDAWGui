using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public class Song
    {
        public string Title { get; set; }
        public int Ver { get; set; }
        public string Author { get; set; }
        public string Licence { get; set; }
        public string Comments { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ChangeDate { get; set; }

        public Track[] Tracks { get; set; }
        public Script[] Scripts { get; set; }
        public Sample[] Samples { get; set; }
        public Part[] Parts { get; set; }

    }
}
