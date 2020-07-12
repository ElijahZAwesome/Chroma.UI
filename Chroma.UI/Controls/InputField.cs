using System;
using System.Numerics;
using System.Text.RegularExpressions;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;

namespace Chroma.UI.Controls
{
    /// <summary>
    /// TODO: Need to set text to the right if AllowOverflow is false
    /// Probably make the caret offset less hard-coded
    /// </summary>
    public class InputField : ChromaControl
    {
        public string Text;
        public string Placeholder;
        public bool AllowOverflow;
        public bool Focused;
        public TrueTypeFont Font;
        /// <summary>
        /// Regex filter to only allow characters that match
        /// </summary>
        public string Filter;
        /// <summary>
        /// How many characters can be typed
        /// </summary>
        public int SizeLimit;

        private readonly Caret caret;
        private readonly Vector2 caretStartPosition;
        private Vector2 _renderedSize;

        public InputField(Vector2 position) : base(position, new Vector2(120, 25))
        {
            Text = "";
            Placeholder = "Type here...";
            caret = new Caret(position + new Vector2(4), (int)(Size.Y * 0.75f));
            caretStartPosition = caret.Position;
            BorderColor = Color.Black;
            BorderThickness = 1;
            Font = new TrueTypeFont(UiContentLoader.Instance.DefaultFontPath, 17);

            AllowOverflow = false;
            UpdateOverflow();
        }

        public override void Draw(RenderContext context, GraphicsManager gfx)
        {
            context.Rectangle(ShapeMode.Fill,
                CalculatedPosition,
                _renderedSize.X,
                _renderedSize.Y,
                Color.White);
            if (string.IsNullOrEmpty(Text))
            {
                var textSize = Font.Measure(Placeholder);
                var textPosition = CalculatedPosition + new Vector2(
                    0,
                    CalculatedSize.Y / 2 - textSize.Height / 2);
                context.DrawString(Font, Placeholder, textPosition + new Vector2(3, -1),
                    (c, i, arg3, arg4) =>
                        new GlyphTransformData(arg3) {Color = Color.LightSlateGray});
            }
            else
            {
                var textSize = Font.Measure(Text);
                var textPosition = CalculatedPosition + new Vector2(
                    0,
                    CalculatedSize.Y / 2 - textSize.Height / 2);
                context.DrawString(Font, Text, textPosition + new Vector2(3, -1),
                    (c, i, arg3, arg4) =>
                        new GlyphTransformData(arg3) {Color = Color.Black});
            }

            if (Focused) caret.Draw(context, gfx);

            if (BorderColor.HasValue)
            {
                var oldThickness = gfx.LineThickness;
                gfx.LineThickness = BorderThickness;
                context.Rectangle(ShapeMode.Stroke,
                    CalculatedPosition,
                    _renderedSize.X,
                    _renderedSize.Y,
                    BorderColor.Value);
                gfx.LineThickness = oldThickness;
            }
        }

        public override void Update(float delta)
        {
            var mouseOverlapping =
                ChromaExtensions.MouseOverlapping(Mouse.GetPosition(), CalculatedPosition, CalculatedSize);
            if (GetMouseUp(MouseButton.Left) && mouseOverlapping)
                Focus();
            else if (GetMouseUp(MouseButton.Left) && !mouseOverlapping) 
                DeFocus();

            caret.Update(delta);

            caret.Position = new Vector2(
                caretStartPosition.X + Font.Measure(Text.Substring(0, caret.TextPosition)).Width,
                caretStartPosition.Y);
            base.Update(delta);
        }

        public override void KeyPressed(KeyEventArgs e)
        {
            if (!Focused) return;

            if (e.KeyCode == KeyCode.Backspace)
            {
                caret.ResetTimer();
                if (Text.Length <= 0) return;
                OnTextChanged(EventArgs.Empty);
                Text = Text.Remove(Text.Length - 1, 1);
                caret.TextPosition--;
                UpdateOverflow();
                return;
            }
        }

        public override void TextInput(TextInputEventArgs e)
        {
            if (!Focused) return;
            caret.ResetTimer();

            if (Filter != null)
            {
                if (!Regex.IsMatch(e.Text, Filter))
                {
                    return;
                }
            }

            if (SizeLimit != 0)
            {
                if (Text.Length >= SizeLimit)
                    return;
            }

            OnTextChanged(EventArgs.Empty);
            var charToAdd = e.Text;
            Text += charToAdd;
            caret.TextPosition++;
            UpdateOverflow();
        }

        public void Focus()
        {
            Focused = true;

            OnFocusChanged(EventArgs.Empty);
        }

        public void DeFocus()
        {
            Focused = false;

            OnFocusChanged(EventArgs.Empty);
        }

        private void UpdateOverflow()
        {
            if (AllowOverflow)
            {
                var textPosition = CalculatedPosition + new Vector2(3, -1);
                var textSize = Font.Measure(Text);

                if (textPosition.X + textSize.Width + 13 > CalculatedPosition.X + CalculatedSize.X)
                    _renderedSize = new Vector2(textSize.Width + 13, CalculatedSize.Y);
                else
                    _renderedSize = CalculatedSize;
            }
            else
            {
                _renderedSize = CalculatedSize;
            }
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        protected virtual void OnFocusChanged(EventArgs e)
        {
            FocusChanged?.Invoke(this, e);
        }

        public event EventHandler TextChanged;
        public event EventHandler FocusChanged;
    }
}