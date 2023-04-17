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
        public float X;
        public float Y;
        public int Size;

        public float Speed;


        public bool onPlatform;
        public bool isJump;

        public MyGame.Form1.Color color;

        public int Score;
        public bool isAlive;

        public float GravityValue;

        public Image playerImg;

        public Player(int x, int y)
        {
            this.X = x;
            this.Y = y;
            Speed = 1.0f;
            onPlatform = false;
            this.Size = 64;
            isAlive = true;
            GravityValue = 0.1f;
            Score = 0;
        }

        public void SetRedColor()
        {
            this.color = MyGame.Form1.Color.Red;
            playerImg = new Bitmap("D:\\player_red.png");
        }

        public void SetBlueColor()
        {
            this.color = MyGame.Form1.Color.Blue;
            playerImg = new Bitmap("D:\\player_blue.png");
        }
    }
}
