using System.Timers;
using System.Linq;

namespace MyGame
{
    public partial class Form1 : Form
    {
        System.Media.SoundPlayer music = new System.Media.SoundPlayer();

        public enum Color
        {
            Red,
            Blue
        }

        Player player;
        Floor floor1;
        Floor floor2;
        Floor floor3;
        Floor floor4;

        int deltaWidth = 0;
        float instaJump = 12f;
        float reloadDoubleJump = 0;

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
            music.SoundLocation = "D:\\gamecourse\\dubstep.wav";
            music.Play();
            
            music.PlayLooping();
            var startY = 700;
            timer1.Stop();
            player = new Player(200, startY-100);
            floor1 = new Floor(200, startY);
            floor2 = new Floor(400, startY);
            floor3 = new Floor(600, startY);
            floor4 = new Floor(800, startY);

            floor1.SetBlueColor();
            floor1.brokable = false;

            floors = new LinkedList<Floor>();

            floors.AddLast(floor1);
            floors.AddLast(floor2);
            floors.AddLast(floor3);
            floors.AddLast(floor4);         

            gravity = 0;
            this.Text = "Colorful Jumper";
            timer1 = new System.Windows.Forms.Timer();

            timer1.Interval = 1;
            timer1.Tick += new EventHandler(Update);
            timer1.Tick += new EventHandler(Update);

            timer1.Start();              
        }

        private void Update(object sender, EventArgs e)
        {
            label1.Text = "Score: " + player.Score.ToString() + $" speed{player.Speed}" ;

            player.Animation();
            //if (player.onPlatform)
            //    runSound.Play();
            //else
            //    runSound.Stop();

            if (Collide(player, floor1))
            {
                player.isAlive = false;
                timer1.Stop();
            }

            if (player.onPlatform)
            {
                gravity = 0;
                player.isJump = false;
            }
                
            else
            {
                if (player.GravityValue != 0.1f)
                    player.GravityValue += 0.005f;

                gravity += player.GravityValue;
                if (gravity > 4)
                    gravity = 4;
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
                item.brokableZone = (int)(1/player.Speed) * 20;
                //if ((int)(Math.Abs(player.Y + player.Size - item.Y)) <= deltaHeight & item.X - deltaWidth <= player.X & player.X <= item.X + item.SizeX+ deltaWidth)
                if (
                    (int)(player.Y + player.Size) <= (int)(item.Y + item.SizeY)
                    & (int)(player.Y + player.Size) >= (int)item.Y
                    & item.X - deltaWidth <= player.X + player.Size - player.Size/2
                    & player.X <= item.X + item.SizeX+ deltaWidth)
                {
                    player.Y = item.Y - player.Size;
                    player.onPlatform = true;
                    player.isJump = false;
                    gravity = 0;
                    reloadDoubleJump = 0;
                    player.GravityValue = 0;

                    if (player.color != item.color && !item.brokable)
                        player.isAlive = false;

                    if (item.brokable)
                    {
                        item.brokableTimer++;
                        if (item.brokableTimer >= item.brokableZone)
                        {
                            RegenerateFloor(item);
                        }
                    }
                    break;
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
                music.Stop();
            }
            Invalidate();
        }

        private void JumpMove()
        {
            player.Y -= instaJump;
            gravity = -1.2f;
            player.GravityValue = -0.15f;
            player.onPlatform = false;
            player.isJump = true;
            player.animStyle = 0;
        }

        private void JumpLogic()
        {
            if (reloadDoubleJump < 2)
            {
                JumpMove();
                //if (reloadDoubleJump == 1)
                //{
                //    foreach (var item in floors)
                //    {
                //        item.X -= 20;
                //    }
                //}
                reloadDoubleJump++;            
            }     
        }

        private bool Collide(Player player, Floor floor)
        {
            PointF delta = new PointF();
            delta.X = player.X - floor.X;
            delta.Y = player.Y - floor.Y;
            if (delta.Y > 250)
                return true;
            return false;
        }

        private void CreateNewFloors(Floor elem)
        {         
            Random r = new Random();
            var x = r.Next(35, 55);
            var y = r.Next(-50, 25);

            var maxX = floors.Select(x => x.X).Max();
            var maxY = floors.Select(x => x).Where(x => x.X == maxX).Select(x => x.Y).Max();

            var newX = (int)(maxX + x * player.Speed * 1.5 + elem.SizeX);
            var newY = maxY + y;

            if (elem.X + elem.SizeX < player.X)
            {
                elem.X = newX;
                if (newY >= 600)
                    elem.Y = newY - y;
                else if (newY < 200)
                    elem.Y = maxY + Math.Abs(y);
                else
                    elem.Y = maxY + y;

                var randomLenght = r.Next(0, 10 * (int)player.Speed);
                elem.SizeX += randomLenght;
                if (elem.SizeX > 300)
                    elem.SizeX = 300;
                elem.TryChangeColor();
                floorCounter++;
                player.Score += 1;
            }
        }

        private void RegenerateFloor(Floor elem)
        {
            Random r = new Random();
            var x = r.Next(35, 55);
            var y = r.Next(-50, 25);

            var maxX = floors.Select(x => x.X).Max();
            var maxY = floors.Select(x => x).Where(x => x.X == maxX).Select(x => x.Y).Max();

            var newX = (int)(maxX + x * player.Speed * 1.5 + elem.SizeX);
            var newY = maxY + y;

            elem.X = newX;
            if (newY >= 600)
                elem.Y = newY - y;
            else if (newY < 200)
                elem.Y = maxY + Math.Abs(y);
            else
                elem.Y = maxY + y;


            var randomLenght = r.Next(0, 10 * (int)player.Speed);
            elem.SizeX += randomLenght;
            elem.TryChangeColor();
            floorCounter++;
            player.Score += 1;
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
                JumpLogic();              
            }

            if ((keyData & Keys.Enter) == Keys.Enter)
            {
                Init();
            }

            if ((keyData & Keys.Control) == Keys.Control)
            {
                player.GravityValue = 10;
                gravity = 10;
            }

            if ((keyData & Keys.D) == Keys.D)
            {
                player.Speed += 0.25f;
            }

            return base.ProcessCmdKey(ref message, keyData);
        }
    }
}