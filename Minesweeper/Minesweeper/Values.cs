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
    public static class Values
    {
        public static Random RDM = new Random();
        public static Vector2 WindowSize = new Vector2(1360, 768);
        public static float MasterVolume = 1f;
    }
}
