using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.UI
{
    public class ChromaControl
    {

        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Scale;
        public Vector2 Origin;
        public Vector2 AnchorPoint;

        public Color? BorderColor = null;
        public int BorderThickness;

        public Vector2 CalculatedPosition => (AnchorPoint + Position) - CalculatedOrigin;
        public Vector2 CalculatedSize => (Size * Scale);
        public Vector2 CalculatedOrigin => (Origin * Scale);

        private bool LMBPressedLastFrame = false;
        private bool RMBPressedLastFrame = false;
        private bool MMBPressedLastFrame = false;

        public ChromaControl()
        {
            Position = Vector2.Zero;
            Size = Vector2.One;
            Scale = Vector2.One;
            Origin = Vector2.Zero;
            AnchorPoint = Vector2.Zero;
            BorderThickness = 1;
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
            if (BorderColor.HasValue)
            {
                float oldThickness = context.LineThickness;
                context.LineThickness = BorderThickness;
                context.Rectangle(ShapeMode.Stroke,
                    CalculatedPosition,
                    CalculatedSize.X,
                    CalculatedSize.Y,
                    BorderColor.Value);
                context.LineThickness = oldThickness;
            }
        }

        public virtual void Update(float delta)
        {
            LMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Left);
            RMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Right);
            MMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Middle);
        }

        public virtual void KeyPressed(KeyEventArgs e)
        {
        }

        public bool GetMouseUp(MouseButton button)
        {
            bool pressedLastFrame = button switch
            {
                MouseButton.Left => LMBPressedLastFrame,
                MouseButton.Middle => MMBPressedLastFrame,
                MouseButton.Right => RMBPressedLastFrame,
                _ => false
            };

            if (pressedLastFrame && Mouse.IsButtonUp(button))
            {
                return true;
            }

            return false;
        }

        public bool GetMouseDown(MouseButton button)
        {
            bool pressedLastFrame = button switch
            {
                MouseButton.Left => LMBPressedLastFrame,
                MouseButton.Middle => MMBPressedLastFrame,
                MouseButton.Right => RMBPressedLastFrame,
                _ => false
            };

            if (!pressedLastFrame && Mouse.IsButtonDown(button))
            {
                return true;
            }

            return false;
        }
    }
}
