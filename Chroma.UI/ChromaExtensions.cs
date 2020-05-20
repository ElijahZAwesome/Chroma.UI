using System.Drawing;
using System.IO;
using System.Net;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Chroma.UI
{
    public static class ChromaExtensions
    {
        public static Color Divide(this Color c, int n)
        {
            return new Color(
                (byte) (c.R / n),
                (byte) (c.G / n),
                (byte) (c.B / n));
        }

        public static Color Divide(this Color c, float n)
        {
            return new Color(
                (byte) (c.R / n),
                (byte) (c.G / n),
                (byte) (c.B / n));
        }

        public static Color ToChromaColor(this System.Drawing.Color c)
        {
            // I have no fucking clue why system colors are ABGR
            // Just go with it
            return new Color(c.A, c.B, c.G, c.R);
        }

        public static bool MouseOverlapping(Vector2 mousePos, Vector2 pos, Vector2 size)
        {
            if (mousePos.X > pos.X + size.X) return false;
            if (mousePos.X < pos.X) return false;
            if (mousePos.Y > pos.Y + size.Y) return false;
            if (mousePos.Y < pos.Y) return false;
            return true;
        }

        public static Texture DownloadTexture(string url)
        {
            using var client = new WebClient();
            using Stream stream = client.OpenRead(url);
            using var downloadedBmp = new Bitmap(stream);
            var downloadedTexture = new Texture((ushort) downloadedBmp.Width, (ushort) downloadedBmp.Height);
            for (var y = 0; y < downloadedBmp.Height; y++)
            for (var x = 0; x < downloadedBmp.Width; x++)
                downloadedTexture.SetPixel(x, y, downloadedBmp.GetPixel(x, y).ToChromaColor());
            downloadedTexture.Flush();
            return downloadedTexture;
        }

        public static bool ShiftPressed(KeyEventArgs e)
        {
            if ((e.Modifiers & KeyModifiers.LeftShift) != 0 || (e.Modifiers & KeyModifiers.RightShift) != 0)
                return true;

            return false;
        }
    }
}