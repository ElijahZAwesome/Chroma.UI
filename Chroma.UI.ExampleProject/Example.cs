using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Chroma.UI;
using Chroma.UI.Controls;

namespace Chroma.UI.ExampleProject
{
    class Example : Game
    {

        private readonly List<ChromaControl> UiControls;

        private Texture testTexture;
        private TrueTypeFont labelFont;

        public Example()
        {
            UiControls = new List<ChromaControl>
            {
                new Image(Vector2.Zero, new Vector2(300),
                    "https://cdn.discordapp.com/attachments/142480770265513984/712327512209752214/60a.jpg"),
                new Panel(Vector2.Zero, new Vector2(150, 150), testTexture)
                {
                    AnchorPoint = new Vector2(Window.Properties.Width / 2, Window.Properties.Height / 2),
                    Origin = new Vector2(150f / 2f, 150f / 2f),
                    Scale = new Vector2(2),
                    BorderColor = Color.Red,
                    BorderThickness = 4
                },
                new Button(new Vector2(200, 200))
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
                },
                new CheckBox(Vector2.Zero)
                {
                    AnchorPoint = new Vector2(Window.Properties.Width / 2, Window.Properties.Height / 2),
                    Checked = true
                },
                new InputField(Vector2.One)
                {
                    AllowOverflow = true
                }
            };
            ((Button) UiControls[2]).ButtonPressed += ButtonPressed;
        }

        private void ButtonPressed(object sender, EventArgs e)
        {
            Console.WriteLine("yay! you pressed the button!");
        }

        protected override void LoadContent()
        {
            new UiContentLoader(Content).LoadUiContent();

            testTexture = Content.Load<Texture>("small.jpg");
            labelFont = Content.Load<TrueTypeFont>("ARIAL.TTF");
        }

        protected override void Draw(RenderContext context)
        {
            UiControls.ForEach(control => control.Draw(context));
        }

        protected override void Update(float delta)
        {
            UiControls.ForEach(control => control.Update(delta));
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            UiControls.ForEach(control => control.KeyPressed(e));
        }

        protected override void TextInput(TextInputEventArgs e)
        {
            UiControls.ForEach(control => control.TextInput(e));
        }
    }
}
