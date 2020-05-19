using System;
using System.Collections.Generic;
using System.Text;
using Chroma.Graphics;

namespace Chroma.UI
{
    public static class ChromaExtensions
    {
        public static Color Divide(this Color c, int n)
        {
            return new Color(
                (byte)(c.R / n),
                (byte)(c.G / n),
                (byte)(c.B / n));
        }

        public static Color Divide(this Color c, float n)
        {
            return new Color(
                (byte)(c.R / n),
                (byte)(c.G / n),
                (byte)(c.B / n));
        }
    }
}
