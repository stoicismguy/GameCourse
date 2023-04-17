using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    internal class Player
    {
        public enum Color
        {
            Red,
            Blue
        }

        public float X;
        public float Y;
        public int Size;

        public float Speed;

        public bool onPlatform;

        public Color color;

        public int Score;
        public bool isAlive;

        public float GravityValue;

        public Image playerImg;

        public Player(int x, int y)
        {
            color = Color.Blue;
            playerImg = new Bitmap("D:\\player_red.png");
            this.X = x;
            this.Y = y;
            Speed = 1.5f;
            onPlatform = false;
            this.Size = 64;
            isAlive = true;
            GravityValue = 0.1f;
        }

        public void SetRedColor()
        {
            color = Color.Red;
            playerImg = new Bitmap("D:\\player_red.png");
        }

        public void SetBlueColor()
        {
            color = Color.Blue;
            playerImg = new Bitmap("D:\\player_blue.png");
        }
    }
}
