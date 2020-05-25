using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;

namespace Chroma.UI.Controls
{
    public class GroupBox : ChromaControl
    {

        public string Text;
        public TrueTypeFont Font;

        private Vector2 TextOffset => new Vector2(12, -(Font.Size * 0.75f));
        private ChromaControlCollection _children;

        public ChromaControlCollection Children
        {
            get => _children;
            set
            {
                _children = value;
                _children.CollectionChanged += ChildrenChanged;

                foreach (var control in _children)
                {
                    control.Parent = this;
                }
            }
        }

        public GroupBox(Vector2 position, Vector2 size) : base(position, size)
        {
            BorderColor = Color.Black;
            BorderThickness = 1;
            Text = "";
            Font = new TrueTypeFont(UiContentLoader.Instance.DefaultFontPath, 16);
        }

        public override void Draw(RenderContext context)
        {
            DrawBorder(context);

            ForeachInChildren(control => control.Draw(context));

            context.DrawString(Font,
                Text,
                CalculatedPosition + TextOffset, 
                (c, i, arg3, arg4) => 
                    new GlyphTransformData(arg3) { Color = Color.Black });
        }

        private void DrawBorder(RenderContext context)
        {
            if (BorderColor.HasValue)
            {
                var oldThickness = context.LineThickness;
                context.LineThickness = BorderThickness;
                context.Line(CalculatedPosition, CalculatedPosition + new Vector2(0, CalculatedSize.Y),
                    BorderColor.Value);
                context.Line(CalculatedPosition + new Vector2(CalculatedSize.X, CalculatedSize.Y),
                    CalculatedPosition + new Vector2(0, CalculatedSize.Y), BorderColor.Value);
                context.Line(CalculatedPosition + new Vector2(CalculatedSize.X, CalculatedSize.Y),
                    CalculatedPosition + new Vector2(CalculatedSize.X, 0), BorderColor.Value);
                context.Line(CalculatedPosition + new Vector2(TextOffset.X, 0), CalculatedPosition, BorderColor.Value);
                context.Line(CalculatedPosition + new Vector2(CalculatedSize.X, 0),
                    CalculatedPosition + new Vector2(TextOffset.X + Font.Measure(Text).X, 0), BorderColor.Value);
                context.LineThickness = oldThickness;
            }
        }

        public override void Update(float delta)
        {
            ForeachInChildren(control => control.Update(delta));

            int processedControls = 0;
            int currentRow = 0;
            List<List<ChromaControl>> controlMatrix = new List<List<ChromaControl>>();
            while(processedControls < Children.Count)
            {
                controlMatrix.Add(new List<ChromaControl>());

                float currentRowWidth = 0;
                foreach (var control in Children)
                {
                    if (Children.IndexOf(control) < processedControls) continue;
                    if (currentRowWidth + control.CalculatedSize.X < CalculatedSize.X)
                    {
                        currentRowWidth += control.CalculatedSize.X;
                        controlMatrix[currentRow].Add(control);
                        processedControls++;
                    }
                    else
                        break;
                }

                currentRow++;
            }

            controlMatrix.ForEach(row =>
            {
                float totalRowWidth = 0;
                row.ForEach(obj => totalRowWidth += obj.CalculatedSize.X);

                row.FirstOrDefault().Position = 
                    new Vector2((CalculatedSize.X - totalRowWidth) / 2, 
                        (CalculatedSize.Y / controlMatrix.Count) * controlMatrix.IndexOf(row));

                row.ForEach(obj =>
                {
                    if (obj == row.First()) return;
                    var previousControl = row[row.IndexOf(obj) - 1];
                    obj.Position = new Vector2(previousControl.Position.X + previousControl.CalculatedSize.X, 
                        (CalculatedSize.Y / controlMatrix.Count) * controlMatrix.IndexOf(row));
                });
            });
        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Children[e.NewStartingIndex].Parent = this;
        }
    }
}
