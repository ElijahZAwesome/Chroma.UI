using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
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

        public static bool ShiftPressed(KeyEventArgs e)
        {
            if ((e.Modifiers & KeyModifiers.LeftShift) != 0 || (e.Modifiers & KeyModifiers.RightShift) != 0)
            {
                return true;
            }

            return false;
        }

        public static string ToChar(this KeyCode key, bool shift)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
            {
                keyboardState[16] = 0xff;
            }
            ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int ToUnicode(
            uint virtualKeyCode,
            uint scanCode,
            byte[] keyboardState,
            StringBuilder receivingBuffer,
            int bufferSize,
            uint flags
        );
    }
}
