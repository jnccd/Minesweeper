using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MineSweeperField MineSweeper;
        int ElementSize = 20;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            MineSweeper = new MineSweeperField(new Rectangle(0, 0, ElementSize * 35, ElementSize * 35), ElementSize, 175);
            Values.WindowSize = new Vector2(MineSweeper.Field.Width, MineSweeper.Field.Height + MineSweeper.Field.Y);
            graphics.PreferredBackBufferWidth = (int)Values.WindowSize.X;
            graphics.PreferredBackBufferHeight = (int)Values.WindowSize.Y;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Assets.Load(Content, GraphicsDevice);
            MineSweeper.PlayStartUpSound();
        }

        protected override void Update(GameTime gameTime)
        {
            Control.Update();
            MineSweeper.Update();
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            MineSweeper.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
