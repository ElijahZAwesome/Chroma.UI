using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;

namespace Chroma.UI.Controls
{
    public class Button : ChromaControl
    {

        public Color Color;
        public Color PressedColor;
        public Color TextColor;
        public string Text;
        public TrueTypeFont TextFont = null;

        public bool HoldingButton = false;

        public Button(Vector2 position) : base(position, new Vector2(80, 25))
        {
            Color = Color.Gray;
            PressedColor = Color.Divide(2);
            TextColor = Color.Black;
            Text = "Button";
            TextFont = new TrueTypeFont(UiContentLoader.Instance.DefaultFontPath, 17);
            BorderColor = Color.Black;
            BorderThickness = 1;
        }

        public override void Draw(RenderContext context)
        {
            context.Rectangle(ShapeMode.Fill, 
                CalculatedPosition, 
                CalculatedSize.X, 
                CalculatedSize.Y, 
                HoldingButton ? PressedColor : Color);
            base.Draw(context);
            Vector2 textSize = TextFont.Measure(Text);
            Vector2 textPosition = CalculatedPosition + new Vector2(
                (CalculatedSize.X / 2) - (textSize.X / 2),
                (CalculatedSize.Y / 2) - (textSize.Y / 2));
            context.DrawString(TextFont, Text, textPosition, 
                (c, i, arg3, arg4) => 
                    new GlyphTransformData(arg3) { Color = TextColor});
        }

        public override void Update(float delta)
        {
            bool mouseOverlapping = 
                ChromaExtensions.MouseOverlapping(Mouse.GetPosition(), CalculatedPosition, CalculatedSize);
            if (GetMouseUp(MouseButton.Left) && mouseOverlapping)
            {
                OnButtonPressed(EventArgs.Empty);
            }
            if (GetMouseDown(MouseButton.Left) && mouseOverlapping)
            {
                OnButtonDown(EventArgs.Empty);
            }

            HoldingButton = Mouse.IsButtonDown(MouseButton.Left) && mouseOverlapping;
            base.Update(delta);
        }

        protected virtual void OnButtonPressed(EventArgs e)
        {
            ButtonPressed?.Invoke(this, e);
        }

        protected virtual void OnButtonDown(EventArgs e)
        {
            ButtonDown?.Invoke(this, e);
        }

        public event EventHandler ButtonPressed;
        public event EventHandler ButtonDown;
    }
}
