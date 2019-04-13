﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaporDAWGui
{
    public static class MathX
    {
        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
        public static double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
