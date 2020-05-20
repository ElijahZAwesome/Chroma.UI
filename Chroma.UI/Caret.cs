using System.Numerics;
using Chroma.Graphics;

namespace Chroma.UI
{
    internal class Caret
    {
        public Vector2 Position;
        public int Height;
        public int TextPosition = 0;

        private bool inverted = false;
        private float blinkTimer = 0;
        private const float blinkTime = 0.530f;

        public Caret(Vector2 position, int height)
        {
            Position = position;
            Height = height;
        }

        public void Draw(RenderContext context)
        {
            var oldThickness = context.LineThickness;
            context.LineThickness = 1;
            if (!inverted)
                context.Line(Position, new Vector2(Position.X, Position.Y + Height), Color.Black);
            context.LineThickness = oldThickness;
        }

        public void Update(float delta)
        {
            blinkTimer += delta;

            if (blinkTimer >= blinkTime)
            {
                blinkTimer = 0;
                inverted = !inverted;
            }
        }

        public void ResetTimer()
        {
            blinkTimer = 0;
            inverted = false;
        }
    }
}