using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Chroma.Graphics;

namespace Chroma.UI
{
    public class ChromaControl
    {

        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Scale;
        public Vector2 Origin;
        public Vector2 AnchorPoint;

        public Vector2 CalculatedPosition => (AnchorPoint + Position);
        public Vector2 CalculatedSize => (Size * Scale);

        public ChromaControl()
        {
            Position = Vector2.Zero;
            Size = Vector2.One;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            AnchorPoint = Vector2.Zero;
        }

        public ChromaControl(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            AnchorPoint = Vector2.Zero;
        }

        public virtual void Draw(RenderContext context)
        {
        }

        public virtual void Update(float delta)
        {
        }

    }
}
