using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Chroma.ContentManagement;
using Chroma.Graphics.TextRendering;

namespace Chroma.UI
{
    public class UiContentLoader
    {
        public static UiContentLoader Instance;
        private IContentProvider Content;

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
            File.WriteAllBytes(DefaultFontPath, Properties.Resources.ARIAL);
            CheckmarkTexturePath = Content.ContentRoot + "/checkmark.png";
            Properties.Resources.checkmark.Save(CheckmarkTexturePath, ImageFormat.Png);
        }
    }
}
