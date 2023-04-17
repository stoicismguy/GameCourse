using System.Timers;
using System.Linq;

namespace MyGame
{
    public partial class Form1 : Form
    {
        public enum Color
        {
            Red,
            Blue
        }

        Player player;
        Floor floor1;
        Floor floor2;
        Floor floor3;

        int deltaHeight = 6;
        int deltaWidth = 20;
        float instaJump = 10f;

        int floorCounter = 1;

        LinkedList<Floor> floors;

        float gravity;

        public Form1()
        {
            InitializeComponent();
            Init();
            Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Init()
        {
            player = new Player(200, 200);
            floor1 = new Floor(200, 400);
            floor2 = new Floor(400, 400);
            floor3 = new Floor(600, 400);

            floor1.SetBlueColor();
            player.SetBlueColor();

            floors = new LinkedList<Floor>();

            floors.AddLast(floor1);
            floors.AddLast(floor2);
            floors.AddLast(floor3);

            gravity = 0;

            timer1 = new System.Windows.Forms.Timer();

            timer1.Interval = 1;
            timer1.Tick += new EventHandler(Update);
            timer1.Tick += new EventHandler(Update);
            timer1.Start();         
        }

        private void Update(object sender, EventArgs e)
        {
            //this.Text = gravity.ToString() + " " + player.GravityValue.ToString() + " " + player.Speed.ToString();
            this.Text = "Colorful Jumper";
            label1.Text = "Score: " + player.Score.ToString();
            if (Collide(player, floor1))
            {
                player.isAlive = false;
                timer1.Stop();
            }

            if (player.onPlatform)
            {
                gravity = 0;
            }
            else
            {
                if (player.GravityValue != 0.1f)
                    player.GravityValue += 0.005f;

                gravity += player.GravityValue;
                player.Y += gravity;
                player.Y = (float)Math.Round(player.Y, 2);
            }
            

            if (floorCounter % 10 == 0)
            {
                player.Speed += 0.25f;
                floorCounter = 1;
            }                

            foreach (var item in floors)
            {
                if (Math.Round(Math.Abs(player.Y + player.Size - item.Y)) <= deltaHeight & item.X- deltaWidth <= player.X & player.X <= item.X + item.SizeX+ deltaWidth)
                {
                    player.onPlatform = true;
                    player.isJump = false;
                    gravity = 0;
                    player.GravityValue = 0;
                    if (player.color != item.color)
                        player.isAlive = false;
                }
                else
                {
                    player.onPlatform = false;
                    player.isJump = true;
                }
            }

            if (player.isAlive)
                MoveFloors();
            else
            {
                label1.Text = "Score: " + player.Score.ToString() + "   GAME OVER!";
                timer1.Stop();
            }
            Invalidate();   
        }

        private void Jump()
        {
            player.Y -= instaJump;
            gravity = -1.2f;
            player.GravityValue = -0.15f;
            player.onPlatform = false;
            player.isJump = true;
        }

        private bool Collide(Player player, Floor floor)
        {
            PointF delta = new PointF();
            delta.X = player.X - floor.X;
            delta.Y = player.Y - floor.Y;

            if (delta.Y > 250)
            {
                return true;
            }
            return false;
        }

        private void CreateNewFloors(Floor elem)
        {         
            Random r = new Random();
            var x = r.Next(35, 55);
            var y = r.Next(-50, 25);

            var maxX = floors.Select(x => x.X).Max();
            var maxY = floors.Select(x => x).Where(x => x.X == maxX).Select(x => x.Y).Max();

            var newX = maxX + x*player.Speed + elem.SizeX;
            var newY = maxY + y;

            if (elem.X + elem.SizeX < player.X)
            {
                elem.X = newX;
                if (newY >= 720)
                    elem.Y = newY - y;
                else if (newY < 200)
                    elem.Y = maxY + Math.Abs(y);
                else
                    elem.Y = maxY + y;


                elem.TryChangeColor();
                floorCounter++;
                player.Score += 10;
            }
        }

        private void MoveFloors()
        {
            foreach (var item in floors)
            {
                item.X -= player.Speed;
                CreateNewFloors(item);
            }           
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.DrawImage(player.playerImg, player.X, player.Y, player.Size, player.Size);

            foreach (var item in floors)
            {
                graphics.DrawImage(item.floorImg, item.X, item.Y, item.SizeX, item.SizeY);
            }
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if ((keyData & Keys.Q) == Keys.Q)
                player.SetRedColor();

            if ((keyData & Keys.E) == Keys.E)
                player.SetBlueColor();

            if ((keyData & Keys.Space) == Keys.Space)
            {
                Jump();              
            }

            if ((keyData & Keys.Enter) == Keys.Enter)
            {
                Init();
            }

            return base.ProcessCmdKey(ref message, keyData);
        }
    }
}