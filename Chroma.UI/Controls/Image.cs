using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Chroma.Graphics;
using Chroma.UI;

namespace Chroma.UI.Controls
{
    public class Image : Panel
    {
        public Image(Vector2 position, Vector2 dimensions, Texture texture) : base(position, dimensions, texture)
        {
        }

        public Image(Vector2 position, Vector2 dimensions, string textureUrl) 
            : base(position, dimensions, new Texture(0,0))
        {
            Texture = ChromaExtensions.DownloadTexture(textureUrl);
        }
    }
}
