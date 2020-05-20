using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.UI.Controls
{
    /// <summary>
    /// TODO: Overflow is fucked, fix pls
    /// Need to actually properly register input
    /// Need to add placeholders
    /// Probably make the caret offset less hard-coded
    /// </summary>
    public class InputField : ChromaControl
    {

        public string Text;
        public string Placeholder;
        public bool Focused = false;

        private Caret caret;
        private Vector2 caretStartPosition;
        private TrueTypeFont font;

        public InputField(Vector2 position, TrueTypeFont textFont) : base(position, new Vector2(80, 25))
        {
            Text = "";
            caret = new Caret(position + new Vector2(4), (int)(Size.Y * 0.75f));
            caretStartPosition = caret.Position;
            BorderColor = Color.Black;
            BorderThickness = 1;
            font = textFont;
            font.Size = 20;
        }

        public override void Draw(RenderContext context)
        {
            context.Rectangle(ShapeMode.Fill,
                CalculatedPosition,
                CalculatedSize.X,
                CalculatedSize.Y,
                Color.White);
            context.DrawString(font, Text, CalculatedPosition + new Vector2(3, -1),
                (c, i, arg3, arg4) =>
                    new GlyphTransformData(arg3) { Color = Color.Black });
            if (Focused)
            {
                caret.Draw(context);
            }
            base.Draw(context);
        }

        public override void Update(float delta)
        {
            bool mouseOverlapping =
                ChromaExtensions.MouseOverlapping(Mouse.GetPosition(), CalculatedPosition, CalculatedSize);
            if (GetMouseUp(MouseButton.Left) && mouseOverlapping)
            {
                Focused = true;
            }
            else if (GetMouseUp(MouseButton.Left) && !mouseOverlapping)
            {
                Focused = false;
            }

            caret.Update(delta);

            caret.Position = new Vector2(
                caretStartPosition.X + font.Measure(Text.Substring(0, caret.TextPosition)).X,
                caretStartPosition.Y);
            base.Update(delta);
        }

        public override void KeyPressed(KeyEventArgs e)
        {
            if (!Focused) return;
            caret.ResetTimer();

            if (e.KeyCode == KeyCode.Backspace)
            {
                if (Text.Length <= 0) return;
                Text = Text.Remove(Text.Length - 1, 1);
                caret.TextPosition--;
                return;
            }

            string charToAdd = e.KeyCode.ToChar(ChromaExtensions.ShiftPressed(e));
            Text += charToAdd;
            caret.TextPosition++;
        }
    }
}
