using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using Chroma.ContentManagement;

namespace Chroma.UI
{
    public class UiContentLoader
    {
        public static UiContentLoader Instance;
        private readonly IContentProvider Content;

        public string DefaultFontPath;
        public string CheckmarkTexturePath;

        public UiContentLoader(IContentProvider content)
        {
            Instance = this;
            Content = content;
        }

        public void LoadUiContent()
        {
            DefaultFontPath = Content.ContentRoot + "/Arial.ttf";
            File.WriteAllBytes(DefaultFontPath, ReadResourceAsBytes("Chroma.UI.Assets.Arial.ttf"));
            CheckmarkTexturePath = Content.ContentRoot + "/checkmark.png";
            ReadBitmapResource("Chroma.UI.Assets.checkmark.png").Save(CheckmarkTexturePath, ImageFormat.Png);
        }

        public Stream ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(nameof(Chroma.UI)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            return assembly.GetManifestResourceStream(resourcePath);
        }

        public Bitmap ReadBitmapResource(string name)
        {
            using(Stream bitmapStream = ReadResource(name))
            {
                return new Bitmap(bitmapStream);
            }
        }

        public byte[] ReadResourceAsBytes(string name)
        {
            using(Stream byteStream = ReadResource(name))
            {
                return ReadToEnd(byteStream);
            }
        }

        public static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;

                }
            }
        }
    }
}