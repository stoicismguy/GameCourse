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

        public int brokableTimer;
        public bool brokable;
        public int brokableZone;

        public Floor(int x, int y)
        {
            TryChangeColor();
            this.X = x;
            this.Y = y;
            SizeX = 180;
            SizeY = 30;
            brokableZone = 15;
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
            this.brokableTimer = 0;
            if (r.Next(0, 2) == 1)
                SetRedColor();
            else
                SetBlueColor();

            if (r.Next(10, 16) == 15)
                brokable = true;
            else
                brokable = false;

            if (brokable)
            {
                this.floorImg = new Bitmap("D:\\floor_brokable.png");
            }
        }      
    }
}
