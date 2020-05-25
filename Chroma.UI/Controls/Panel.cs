using System.Collections.Specialized;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input.EventArgs;

namespace Chroma.UI.Controls
{
    public class Panel : ChromaControl
    {
        public Texture Texture = null;
        public Color? Color = null;

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

        public Panel(Vector2 position, Vector2 dimensions, Color color) : base(position, dimensions)
        {
            Color = color;

            Children = new ChromaControlCollection();
        }

        public Panel(Vector2 position, Vector2 dimensions, Texture texture) : base(position, dimensions)
        {
            Texture = texture;

            Children = new ChromaControlCollection();
        }

        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Children[e.NewStartingIndex].Parent = this;
        }

        public override void Draw(RenderContext context)
        {
            if (Texture != null)
                context.DrawTexture(Texture,
                    CalculatedPosition,
                    CalculatedSize / new Vector2(Texture.Width, Texture.Height),
                    Vector2.Zero,
                    0);
            else if (Color.HasValue)
                context.Rectangle(ShapeMode.Fill,
                    CalculatedPosition,
                    CalculatedSize.X,
                    CalculatedSize.Y,
                    Color.Value);

            base.Draw(context);
        }
    }
}