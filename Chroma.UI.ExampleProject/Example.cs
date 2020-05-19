using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.UI;
using Chroma.UI.Controls;

namespace Chroma.UI.ExampleProject
{
    class Example : Game
    {

        private List<ChromaControl> UiControls;

        private Texture testTexture;

        public Example()
        {
            UiControls = new List<ChromaControl>();
            UiControls.Add(new Panel(Vector2.Zero, new Vector2(150, 200), testTexture)
            {
                Scale = new Vector2(2)
            });
        }

        protected override void LoadContent()
        {
            testTexture = Content.Load<Texture>("small.jpg");
        }

        protected override void Draw(RenderContext context)
        {
            UiControls.ForEach(control => control.Draw(context));
        }

        protected override void Update(float delta)
        {
            Panel testPanel = (Panel)UiControls[0];
            testPanel.Position = new Vector2(testPanel.Position.X + (15 * delta), testPanel.Position.Y);
        }
    }
}
