using System.Numerics;
using Chroma.Graphics;

namespace Chroma.UI.Controls
{
    public class Image : Panel
    {

        // ReSharper disable once UnusedMember.Local
        private new ChromaControlCollection Children => null;

        public Image(Vector2 position, Vector2 dimensions, Texture texture) : base(position, dimensions, texture)
        {
        }

        public Image(Vector2 position, Vector2 dimensions, string textureUrl)
            : base(position, dimensions, new Texture(0, 0))
        {
            Texture = ChromaExtensions.DownloadTexture(textureUrl);
        }

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();

            foreach(var child in Children)
                child.Dispose();

            if(Texture != null)
                Texture.Dispose();
        }
    }
}