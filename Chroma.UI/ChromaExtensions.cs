using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using Chroma.Graphics;
using Color = Chroma.Graphics.Color;

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

        public static Color ToChromaColor(this System.Drawing.Color c)
        {
            return new Color(c.A, c.B, c.G, c.R);
        }

        public static Texture DownloadTexture(string url)
        {
            using WebClient client = new WebClient();
            using Stream stream = client.OpenRead(url);
            using Bitmap downloadedBmp = new Bitmap(stream);
            Texture downloadedTexture = new Texture((ushort)downloadedBmp.Width, (ushort)downloadedBmp.Height);
            for (int y = 0; y < downloadedBmp.Height; y++)
            {
                for (int x = 0; x < downloadedBmp.Width; x++)
                {
                    downloadedTexture.SetPixel(x, y, downloadedBmp.GetPixel(x, y).ToChromaColor());
                }
            }
            downloadedTexture.Flush();
            return downloadedTexture;
        }
    }
}
