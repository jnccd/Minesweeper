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
using System.Threading.Tasks;

namespace Minesweeper
{
    public class MineSweeperField
    {
        public MineSweeperElement[,] Grid;
        public Rectangle Field;
        public int MineCount;
        public int ElementSize;
        public bool IsBigFontActive;
        string BigFontContent;
        string SmallFontContent;
        Color BigFontColor;
        public int Score;
        public int Timer;
        const int UpperBarHeight = 25;
        public const int ScorePerReveal = 25;

        public MineSweeperField(Rectangle Field, int ElementSize, int MineCount)
        {
            this.ElementSize = ElementSize;
            Grid = new MineSweeperElement[Field.Width / ElementSize, Field.Height / ElementSize];
            this.Field = new Rectangle(Field.X, Field.Y + UpperBarHeight, Grid.GetLength(0) * ElementSize, Grid.GetLength(1) * ElementSize);
            this.MineCount = MineCount;

            Restart(GetMouseHoveredElement());
        }

        public int GetElementGridIndexX(MineSweeperElement E)
        {
            return (E.x - Field.X) / ElementSize;
        }
        public int GetElementGridIndexY(MineSweeperElement E)
        {
            return (E.y - Field.Y) / ElementSize;
        }
        public void PlayStartUpSound()
        {
            if (Assets.StartUp != null)
                Assets.StartUp.Play(0.4f, 0, 0);
        }
        MineSweeperElement GetMouseHoveredElement()
        {
            try
            {
                return Grid[((int)Control.GetMouseVector().X - Field.X) / ElementSize, ((int)Control.GetMouseVector().Y - Field.Y) / ElementSize];
            }
            catch { return null; }
        }
        void SetBigFont(string Message, string SmallMessage, Color C)
        {
            IsBigFontActive = true;
            BigFontContent = Message;
            SmallFontContent = SmallMessage;
            BigFontColor = C;
        }
        void Reveal(MineSweeperElement E)
        {
            if (E.TouchingBombsCount == 0 && !E.IsRevealed)
            {
                List<MineSweeperElement> NeedToBeRevealed = new List<MineSweeperElement>();

                try { if (!E.IsRevealed) { NeedToBeRevealed.Add(E); } } catch { }

                for (int i = 0; i < NeedToBeRevealed.Count; i++)
                {
                    NeedToBeRevealed[i].IsRevealed = true;
                    List<MineSweeperElement> N = NeedToBeRevealed[i].GetNeigbours();
                    for (int i2 = 0; i2 < N.Count; i2++)
                    {
                        if (!N[i2].IsRevealed && N[i2].TouchingBombsCount == 0 && !NeedToBeRevealed.Contains(N[i2]))
                            NeedToBeRevealed.Add(N[i2]);
                    }
                }

                if (NeedToBeRevealed.Count > 1)
                {
                    Assets.Reveal.Play(0.4f * Values.MasterVolume, 0, 0);
                }

                for (int i = 0; i < NeedToBeRevealed.Count; i++)
                {
                    List<MineSweeperElement> N = NeedToBeRevealed[i].GetNeigbours();
                    for (int i2 = 0; i2 < N.Count; i2++)
                    {
                        if (!N[i2].HasBomb)
                            N[i2].IsRevealed = true;
                    }
                }
            }
            else
            {
                if (!E.IsRevealed)
                {
                    E.IsRevealed = true;
                    Assets.Reveal.Play(0.2f * Values.MasterVolume, 0, 0);
                }
                else
                {
                    Assets.Error.Play(0.2f * Values.MasterVolume, 0, 0);
                }
            }
        }
        bool HasPlayerWon()
        {
            bool Won = true;
            for (int ix = 0; ix < Grid.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Grid.GetLength(1); iy++)
                {
                    if (!Grid[ix, iy].IsRevealed && !Grid[ix, iy].HasBomb)
                        Won = false;
                }
            }
            return Won;
        }
        void Restart(MineSweeperElement FirstClick)
        {
            lock (Grid)
            {
                // Reset Grid
                for (int ix = 0; ix < Grid.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Grid.GetLength(1); iy++)
                    {
                        Grid[ix, iy] = new MineSweeperElement(ix * ElementSize + Field.X, iy * ElementSize + Field.Y, ElementSize, this);
                    }
                }

                // Add Bombs
                for (int i = 0; i < MineCount; i++)
                {
                    int x = Values.RDM.Next(Grid.GetLength(0));
                    int y = Values.RDM.Next(Grid.GetLength(1));

                    if (Grid[x, y].HasBomb && Grid[x, y] != FirstClick)
                        i--;
                    else
                        Grid[x, y].HasBomb = true;
                }

                // Update BombCounters
                for (int ix = 0; ix < Grid.GetLength(0); ix++)
                {
                    for (int iy = 0; iy < Grid.GetLength(1); iy++)
                    {
                        Grid[ix, iy].TouchingBombsCount = Grid[ix, iy].GetNeigbourBombCount();
                    }
                }
                Timer = 0;
            }
            Score = 0;
        }
        void Cheat()
        {
            for (int ix = 0; ix < Grid.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Grid.GetLength(1); iy++)
                {
                    if (!Grid[ix, iy].HasBomb)
                        Grid[ix, iy].IsRevealed = true;
                }
            }
        }
        
        public void Update()
        {
            if (IsBigFontActive && Control.WasLMBJustPressed())
            { Restart(GetMouseHoveredElement()); IsBigFontActive = false; }

            if (Control.WasLMBJustPressed() && !IsBigFontActive && GetMouseHoveredElement() != null)
            {
                Reveal(GetMouseHoveredElement());
                if (GetMouseHoveredElement().HasBomb)
                    SetBigFont("You lost!", "", Color.Red);
            }

            if (Control.WasRMBJustPressed() && !IsBigFontActive && GetMouseHoveredElement() != null)
                GetMouseHoveredElement().IsMarked = !GetMouseHoveredElement().IsMarked;

            if (Control.WasKeyJustPressed(Keys.W))
                Cheat();

            if (HasPlayerWon() && !IsBigFontActive)
                SetBigFont("You won!", "Time: " + Math.Round(Timer / 60f, 5).ToString(), Color.Green);

            if (!IsBigFontActive)
                Timer++;
        }
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Assets.White, new Rectangle(Field.X, Field.Y - UpperBarHeight, Field.Width, UpperBarHeight), Color.Gray);
            SB.Draw(Assets.Block, new Rectangle(Field.X, Field.Y - UpperBarHeight, Field.Width, UpperBarHeight), Color.White);
            SB.DrawString(Assets.NumberFont, "Score: " + Score.ToString(), new Vector2(Values.WindowSize.X - 2.5f - Assets.NumberFont.MeasureString("Score: " + Score.ToString()).X, 2.5f), Color.Blue);
            SB.DrawString(Assets.NumberFont, "Timer: " + Math.Round(Timer / 60f, 2).ToString(), new Vector2(6.5f, 2.5f), Color.Blue);
            for (int ix = 0; ix < Grid.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Grid.GetLength(1); iy++)
                {
                    Grid[ix, iy].Draw(SB);
                }
            }
            for (int ix = 0; ix < Grid.GetLength(0); ix++)
            {
                for (int iy = 0; iy < Grid.GetLength(1); iy++)
                {
                    Grid[ix, iy].DrawPointAnim(SB);
                }
            }
            if (IsBigFontActive)
            {
                SB.Draw(Assets.White, Field, Color.Black * 0.7f);
                SB.DrawString(Assets.BigFont, BigFontContent, Values.WindowSize / 2 - Assets.BigFont.MeasureString(BigFontContent) / 2, BigFontColor);
                SB.DrawString(Assets.NumberFont, SmallFontContent, Values.WindowSize / 2 - Assets.NumberFont.MeasureString(SmallFontContent) / 2 + new Vector2(0, Assets.BigFont.MeasureString(BigFontContent).Y / 2 + 5), BigFontColor);
            }
        }
    }
}
