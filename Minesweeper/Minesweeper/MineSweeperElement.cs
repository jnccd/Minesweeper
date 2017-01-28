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
    public class MineSweeperElement
    {
        public bool HasBomb;
        private bool isRevealed;
        public bool IsRevealed
        {
            get
            {
                return isRevealed;
            }
            set
            {
                if (isRevealed == false && value == true && !HasBomb) { PointGotAnim = 0; MotherField.Score += MineSweeperField.ScorePerReveal; }
                isRevealed = value;
            }
        }
        public bool IsMarked;
        public int TouchingBombsCount;
        public int x;
        public int y;
        public int Size;
        int PointGotAnim;
        MineSweeperField MotherField;

        public MineSweeperElement(int x, int y, int Size, MineSweeperField MotherField) { HasBomb = false; IsRevealed = false; TouchingBombsCount = 0;
            this.x = x; this.y = y; this.Size = Size; PointGotAnim = 50000; this.MotherField = MotherField; }

        public List<MineSweeperElement> GetNeigbours()
        {
            List<MineSweeperElement> a = new List<MineSweeperElement>();
            for (int ix = MotherField.GetElementGridIndexX(this) - 1; ix < MotherField.GetElementGridIndexX(this) + 2; ix++)
            {
                for (int iy = MotherField.GetElementGridIndexY(this) - 1; iy < MotherField.GetElementGridIndexY(this) + 2; iy++)
                {
                    if (ix == MotherField.GetElementGridIndexX(this) && iy == MotherField.GetElementGridIndexY(this)) { iy++; }
                    try { a.Add(MotherField.Grid[ix, iy]); } catch { }
                }
            }
            return a;
        }
        public int GetNeigbourBombCount()
        {
            int Count = 0;
            for (int ix = MotherField.GetElementGridIndexX(this) - 1; ix < MotherField.GetElementGridIndexX(this) + 2; ix++)
            {
                for (int iy = MotherField.GetElementGridIndexY(this) - 1; iy < MotherField.GetElementGridIndexY(this) + 2; iy++)
                {
                    try { if (MotherField.Grid[ix, iy].HasBomb) { Count++; } } catch { }
                }
            }
            return Count;
        }

        public void Draw(SpriteBatch SB)
        {
            if (IsRevealed)
            {
                SB.Draw(Assets.White, new Rectangle(x, y, Size, Size), Color.LightGray);

                switch (TouchingBombsCount)
                {
                    case 0: break;
                    case 1:
                        SB.DrawString(Assets.NumberFont, "1", new Vector2(x + Size / 2 - Assets.NumberFont.MeasureString("1").X / 2,
                            y + Size / 2 - Assets.NumberFont.MeasureString("1").Y / 2), Color.Blue);
                        break;

                    case 2:
                        SB.DrawString(Assets.NumberFont, "2", new Vector2(x + Size / 2 - Assets.NumberFont.MeasureString("2").X / 2,
                            y + Size / 2 - Assets.NumberFont.MeasureString("2").Y / 2), Color.Green);
                        break;

                    case 3:
                        SB.DrawString(Assets.NumberFont, "3", new Vector2(x + Size / 2 - Assets.NumberFont.MeasureString("3").X / 2,
                            y + Size / 2 - Assets.NumberFont.MeasureString("3").Y / 2), Color.Red);
                        break;

                    default:
                        SB.DrawString(Assets.NumberFont, TouchingBombsCount.ToString(), new Vector2(x + Size / 2 - Assets.NumberFont.MeasureString(TouchingBombsCount.ToString()).X / 2,
                            y + Size / 2 - Assets.NumberFont.MeasureString(TouchingBombsCount.ToString()).Y / 2), Color.Violet);
                        break;
                }

                SB.Draw(Assets.Block, new Rectangle(x, y, Size, Size), new Rectangle(0, 0, Assets.Block.Width, Assets.Block.Height), Color.White, 0, Vector2.Zero, 
                    SpriteEffects.FlipVertically, 1);

                if (HasBomb)
                    SB.Draw(Assets.Bomb, new Rectangle(x, y, Size, Size), Color.White);
            }
            else
            {
                SB.Draw(Assets.White, new Rectangle(x, y, Size, Size), Color.FromNonPremultiplied(175, 175, 175, 255));
                SB.Draw(Assets.Block, new Rectangle(x, y, Size, Size), Color.White);

                if (IsMarked)
                    SB.Draw(Assets.White, new Rectangle(x, y, Size, Size), Color.Red * 0.4f);

                if (MotherField.IsBigFontActive && HasBomb)
                    SB.Draw(Assets.Bomb, new Rectangle(x, y, Size, Size), Color.White);
            }
        }
        public void DrawPointAnim(SpriteBatch SB)
        {
            if (PointGotAnim < 100)
            {
                SB.DrawString(Assets.NumberFont, "+25", new Vector2(x + Size / 2 - Assets.NumberFont.MeasureString("+25").X / 2,
                            y + Size / 2 - Assets.NumberFont.MeasureString("+25").Y / 2 - PointGotAnim / 2), Color.Red * ((100 - PointGotAnim) / 100f));
            }
            PointGotAnim += 3;
        }
    }
}
