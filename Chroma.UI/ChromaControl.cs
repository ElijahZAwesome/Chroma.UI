using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.UI.Controls;

namespace Chroma.UI
{
    public class ChromaControl
    {
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 Scale;
        public Vector2 Origin;
        public Vector2 AnchorPoint;

        public ChromaControl Parent = null;

        public Color? BorderColor = null;
        public int BorderThickness;

        public Vector2 CalculatedPosition
        {
            get
            {
                if (Parent != null)
                    return Parent.CalculatedPosition + AnchorPoint + Position - CalculatedOrigin;
                else
                    return AnchorPoint + Position - CalculatedOrigin;
            }
        }

        public Vector2 CalculatedSize => Size * Scale;
        public Vector2 CalculatedOrigin => Origin * Scale;

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
                var oldThickness = context.LineThickness;
                context.LineThickness = BorderThickness;
                context.Rectangle(ShapeMode.Stroke,
                    CalculatedPosition,
                    CalculatedSize.X,
                    CalculatedSize.Y,
                    BorderColor.Value);
                context.LineThickness = oldThickness;
            }

            ForeachInChildren(control => control.Draw(context));
        }

        public virtual void Update(float delta)
        {
            LMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Left);
            RMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Right);
            MMBPressedLastFrame = Mouse.IsButtonDown(MouseButton.Middle);

            ForeachInChildren(control => control.Update(delta));
        }

        public virtual void KeyPressed(KeyEventArgs e)
        {
            ForeachInChildren(control => control.KeyPressed(e));
        }

        public virtual void TextInput(TextInputEventArgs e)
        {
            ForeachInChildren(control => control.TextInput(e));
        }

        public bool GetMouseUp(MouseButton button)
        {
            var pressedLastFrame = button switch
            {
                MouseButton.Left => LMBPressedLastFrame,
                MouseButton.Middle => MMBPressedLastFrame,
                MouseButton.Right => RMBPressedLastFrame,
                _ => false
            };

            if (pressedLastFrame && Mouse.IsButtonUp(button)) return true;

            return false;
        }

        public bool GetMouseDown(MouseButton button)
        {
            var pressedLastFrame = button switch
            {
                MouseButton.Left => LMBPressedLastFrame,
                MouseButton.Middle => MMBPressedLastFrame,
                MouseButton.Right => RMBPressedLastFrame,
                _ => false
            };

            if (!pressedLastFrame && Mouse.IsButtonDown(button)) return true;

            return false;
        }

        private bool TypeHasChildren(Type type = null)
        {
            if (type == null) type = this.GetType();

            if (type == typeof(Panel) || type == typeof(GroupBox))
            {
                return true;
            }

            return false;
        }

        internal void ForeachInChildren(Action<ChromaControl> action)
        {
            if (TypeHasChildren())
            {
                if (this.GetType() == typeof(Panel))
                {
                    foreach (var control in ((Panel)this).Children)
                    {
                        action(control);
                    }
                }
                if (this.GetType() == typeof(GroupBox))
                {
                    foreach (var control in ((GroupBox)this).Children)
                    {
                        action(control);
                    }
                }
            }
        }
    }
}