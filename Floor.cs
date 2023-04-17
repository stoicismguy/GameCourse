using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyGame
{
    internal class Floor
    {
        public float X;
        public float Y;
        public int SizeX;
        public int SizeY;
        public MyGame.Form1.Color color;
        public Image floorImg;

        public Floor(int x, int y)
        {
            TryChangeColor();
            this.X = x;
            this.Y = y;
            SizeX = 180;
            SizeY = 30;
        }

        public void SetRedColor()
        {
            color = MyGame.Form1.Color.Red;
            floorImg = new Bitmap("D:\\floor_red.png");
        }

        public void SetBlueColor()
        {
            color = MyGame.Form1.Color.Blue;
            floorImg = new Bitmap("D:\\floor_blue.png");
        }

        public void TryChangeColor()
        {
            Random r = new Random();
            if (r.Next(0, 2) == 1)
                SetRedColor();
            else
                SetBlueColor();
        }
    }
}
