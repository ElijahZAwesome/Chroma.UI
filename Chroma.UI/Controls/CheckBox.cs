using System;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;

namespace Chroma.UI.Controls
{
    public class CheckBox : ChromaControl
    {
        public bool Checked;
        public string Text;
        public TrueTypeFont Font;
        public bool HoldingBox = false;

        private readonly Texture CheckmarkTexture;

        private const int BoxSize = 16;
        private const int BoxOffset = 4;

        public CheckBox(Vector2 position) : base(position, Vector2.One)
        {
            Checked = false;
            Text = "CheckBox";
            Font = new TrueTypeFont(UiContentLoader.Instance.DefaultFontPath, 13);
            CheckmarkTexture = new Texture(UiContentLoader.Instance.CheckmarkTexturePath);
        }

        public override void Draw(RenderContext context)
        {
            context.Rectangle(ShapeMode.Fill,
                CalculatedPosition + new Vector2(BoxOffset),
                BoxSize,
                BoxSize,
                HoldingBox ? Color.White.Divide(2) : Color.White);

            var oldThickness = context.LineThickness;
            context.LineThickness = 1;
            context.Rectangle(ShapeMode.Stroke,
                CalculatedPosition + new Vector2(BoxOffset),
                BoxSize,
                BoxSize,
                Color.Black);
            context.LineThickness = oldThickness;

            // Render Checkmark
            if (Checked)
                context.DrawTexture(CheckmarkTexture,
                    CalculatedPosition + new Vector2(BoxOffset),
                    Vector2.One, Vector2.Zero, 0);

            context.DrawString(Font,
                Text,
                CalculatedPosition +
                new Vector2(BoxOffset + BoxSize + BoxOffset,
                    BoxOffset), (c, i, arg3, arg4) =>
                    new GlyphTransformData(arg3) {Color = Color.Black});
        }

        public override void Update(float delta)
        {
            Size = new Vector2(BoxOffset + BoxSize + BoxOffset + Font.Measure(Text).X, BoxSize);

            var mouseOverlapping =
                ChromaExtensions.MouseOverlapping(Mouse.GetPosition(),
                    CalculatedPosition + new Vector2(BoxOffset), new Vector2(BoxSize));
            if (GetMouseUp(MouseButton.Left) && mouseOverlapping)
            {
                Checked = !Checked;
                OnCheckChanged(EventArgs.Empty);
            }

            HoldingBox = Mouse.IsButtonDown(MouseButton.Left) && mouseOverlapping;
            base.Update(delta);
        }

        protected virtual void OnCheckChanged(EventArgs e)
        {
            CheckChanged?.Invoke(this, e);
        }

        public event EventHandler CheckChanged;
    }
}