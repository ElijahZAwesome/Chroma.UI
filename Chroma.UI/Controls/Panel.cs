using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Chroma.Graphics;

namespace Chroma.UI.Controls
{
    public class Panel : ChromaControl
    {

        public Texture Texture = null;
        public Color? Color = null;

        public Panel(Vector2 position, Vector2 dimensions, Color color) : base(position, dimensions)
        {
            Color = color;
        }

        public Panel(Vector2 position, Vector2 dimensions, Texture texture) : base(position, dimensions)
        {
            Texture = texture;
        }

        public override void Draw(RenderContext context)
        {
            if (Texture != null)
            {
                context.DrawTexture(Texture, 
                    CalculatedPosition,
                    (CalculatedSize / new Vector2(Texture.Width, Texture.Height)), 
                    Origin, 
                    0);
            }
            else if(Color.HasValue)
            {
                context.Rectangle(ShapeMode.Fill, 
                    CalculatedPosition, 
                    CalculatedSize.X, 
                    CalculatedSize.Y, 
                    Color.Value);
            }
        }
    }
}
