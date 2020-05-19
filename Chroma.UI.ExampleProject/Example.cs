using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.UI;
using Chroma.UI.Controls;

namespace Chroma.UI.ExampleProject
{
    class Example : Game
    {

        private List<ChromaControl> UiControls;

        private Texture testTexture;
        private TrueTypeFont buttonFont;
        private TrueTypeFont labelFont;

        public Example()
        {
            UiControls = new List<ChromaControl>
            {
                new Image(Vector2.Zero, new Vector2(300),
                    "https://media.discordapp.net/attachments/142480770265513984/712359741053206625/MW2.png"),
                new Panel(Vector2.Zero, new Vector2(150, 150), testTexture)
                {
                    AnchorPoint = new Vector2(Window.Properties.Width / 2, Window.Properties.Height / 2),
                    Origin = new Vector2(150f / 2f, 150f / 2f),
                    Scale = new Vector2(2),
                    BorderColor = Color.Red,
                    BorderThickness = 4
                },
                new Button(new Vector2(200, 200), buttonFont)
                {
                    Color = Color.LightGray,
                    Text = "fuck"
                },
                new Label(new Vector2(300, 300), 
                    "fuck me lmao",
                    labelFont,
                    22)
                {
                    Color = Color.Red,
                }
            };
            //((Button) UiControls[2]).ButtonPressed += ButtonPressed;
        }

        private void ButtonPressed(object sender, EventArgs e)
        {
            Console.WriteLine("yay! you pressed the button!");
        }

        protected override void LoadContent()
        {
            testTexture = Content.Load<Texture>("small.jpg");
            buttonFont = Content.Load<TrueTypeFont>("ARIAL.TTF", 17);
            labelFont = Content.Load<TrueTypeFont>("ARIAL.TTF", 17);
        }

        protected override void Draw(RenderContext context)
        {
            UiControls.ForEach(control => control.Draw(context));
        }

        protected override void Update(float delta)
        {
            UiControls.ForEach(control => control.Update(delta));
        }
    }
}
