using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Minesweeper
{
    public static class Assets
    {
        public static SpriteFont NumberFont;
        public static SpriteFont BigFont;
        public static Texture2D White;
        public static Texture2D Block;
        public static Texture2D Bomb;
        public static SoundEffect Reveal;
        public static SoundEffect StartUp;
        public static SoundEffect Error;

        public static void Load(ContentManager Content, GraphicsDevice GD)
        {
            Bomb = Content.Load<Texture2D>("Bomb");
            Block = Content.Load<Texture2D>("Block");
            NumberFont = Content.Load<SpriteFont>("Numbers");
            BigFont = Content.Load<SpriteFont>("BigFont");
            Reveal = Content.Load<SoundEffect>("SingleReveal");
            StartUp = Content.Load<SoundEffect>("StartUp");
            Error = Content.Load<SoundEffect>("Error");
            White = new Texture2D(GD, 1, 1);
            Color[] Col = new Color[1];
            Col[0] = Color.White;
            White.SetData<Color>(Col);
        }
    }
}
