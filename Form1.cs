using System.Timers;

namespace MyGame
{
    public partial class Form1 : Form
    {
        Player player;
        Floor floor1;
        Floor floor2;
        Floor floor3;

        int floorCounter = 1;

        LinkedList<Floor> floors = new LinkedList<Floor>();

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
            floor1 = new Floor(300, 400);
            floor2 = new Floor(500, 400);
            floor3 = new Floor(700, 400);

            floors.AddLast(floor1);
            floors.AddLast(floor2);
            floors.AddLast(floor3);

            timer1.Interval = 1;
            timer1.Tick += new EventHandler(Update);
            timer1.Start();         
        }

        private void Update(object sender, EventArgs e)
        {
            if (Collide(player, floor1))
            {
                player.isAlive = false;
                timer1.Stop();
            }

            if (player.GravityValue != 0.1f)
                player.GravityValue += 0.005f;

            gravity += player.GravityValue;
            player.Y += gravity;

            if (floorCounter % 10 == 0)
            {
                player.Speed += 0.5f;
                floorCounter = 1;
            }
                

            foreach (var item in floors)
            {
                if (player.Y == item.Y && item.X <= player.X && player.X <= item.X + item.SizeX)
                {
                    player.onPlatform = true;
                }
                else
                {
                    player.onPlatform = false;
                }
            }

            if (player.isAlive)
                MoveFloors();


            Invalidate();   
        }

        private bool Collide(Player player, Floor floor)
        {
            PointF delta = new PointF();
            delta.X = player.X - floor.X;
            delta.Y = player.Y - floor.Y;

            if (delta.Y > 0)
            {
                return true;
            }
            return false;
        }

        private void CreateNewFloors(Floor elem)
        {         
            Random r = new Random();
            var x = r.Next(-10, 10);
            var y = r.Next(-10, 10);

            if (elem.X + elem.SizeX < player.X)
            {
                elem.X = 700+x;
                elem.Y = 400+y;
                elem.TryChangeColor();
                floorCounter++;
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
                gravity = 0;
                player.GravityValue = -0.15f;

            }
            return base.ProcessCmdKey(ref message, keyData);
        }
    }
}