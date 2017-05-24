using JoPhysics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace GDI_framework
{
    public partial class Form1 : Form
    {
        // Timing
        #region Variables

        Timer timer;
        int[] Score = new int[2] { 0, 0 };
        bool team1Bal;
        bool team2Bal;
        List<System.Drawing.Point[]> rrtList;

        // Globale variabelen voor GDI+
        Graphics screen;
        Bitmap backBuffer;
        float SchaalX;
        float SchaalY;
        int centerStraal = 150;

        //teams aanmaken
        int numberRobots = 2;
        List<Robot> team1;
        List<Robot> team2;
        Bal bal;
        int id;

        // variabelen voor model

        Int32 time;                  // in msec
        const double straal = 30.0d; //van de bol, in m
        const double balstraal = 15.0d; //van de bol, in m

        #endregion Variables

        public Form1(int id)
        {
            this.id = id;
            InitializeComponent();		//aanmaken inhoud form
            InitRenderer();				//aanmaken backbuffer 
            if (id == 0)
            {
                InitGame();
            }    //Beginwaarden van de simulatie instellen
            else
            {
                initRRT();
            }
            InitTimer();
            startButton.Enabled = true;
        }


		#region Timing

		/// <summary>
		/// Initialisatie van de timer
		/// </summary>
		private void InitTimer()
        {
            timer = new Timer();
            timer.Interval = 10; // msec, f = 100 Hz;
            timer.Tick += new EventHandler(timer_Tick);
        }
		#endregion Timing

		/// <summary>
		/// Uitvoeren van een update. 
		/// Deze methode zal elke iteratie opgeroepen worden door de timer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (id==0)
            {
                DoGame();
            }
            else if (id==1)
            {
                DoRRT();
            }
            display.Invalidate(); // force redraw (& paint event);
        }


		#region interactie

		private void startButton_Click(object sender, EventArgs e)
        {
            stopButton.Enabled = true;
            startButton.Enabled = false;
            timer.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopButton.Enabled = false;
            startButton.Enabled = true;
            timer.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            restart();
        }

        private void restart()
        {
            InitRenderer();             //aanmaken backbuffer 
            InitGame();					//Beginwaarden van de simulatie instellen
            InitTimer();
            timer.Enabled = false;
        }

        #endregion interactie

        #region game loop

        /// <summary>
        /// Beginwaarden van de simulatie instellen
        /// </summary>
        private void InitGame()
        {
            time = 0;
            team1 = new List<Robot>();
            team2 = new List<Robot>();
            team1Bal = false;
            team2Bal = false;
            Random rnd = new Random();
            bal = new Bal(-(int)balstraal / 2, -(int)balstraal / 2, (int)balstraal);
            for (int i = 0; i < numberRobots; i++)
            {
                team1.Add(new Robot(rnd.Next(0 + (int)straal, display.Width / 2 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height / 2 - (int)straal), straal));
                team2.Add(new Robot(rnd.Next(-display.Width / 2 + (int)straal, 0 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height / 2 - (int)straal), straal));
            }
            SetDirection();
        }

        private void SetDirection()
        {
            if (team1Bal)
            {
                foreach (var robot in team1)
                {
                    Vector v = new Vector(robot.x + robot.straal / 2 - (-460), robot.y + robot.straal / 2);
                    robot.direction = v;
                }
            }
            else
            {
                foreach (var robot in team1)
                {
                    Vector v = new Vector(robot.x + robot.straal / 2 - (bal.x + bal.straal / 2), robot.y + robot.straal / 2 - (bal.y + bal.straal / 2));
                    robot.direction = v;
                }
            }
            if (team2Bal)
            {
                foreach (var robot in team2)
                {
                    Vector v = new Vector(robot.x + robot.straal / 2 - 460, robot.y + robot.straal / 2);
                    robot.direction = v;
                }
            }
            else
            {
                foreach (var robot in team2)
                {
                    Vector v = new Vector(robot.x + robot.straal / 2 - (bal.x + bal.straal / 2), robot.y + robot.straal / 2 - (bal.y + bal.straal / 2));
                    robot.direction = v;
                }
            }
        }


        /// <summary>
        /// Elke iteratie de hoek van de bal bijwerken
        /// </summary>
        private void DoGame()
        {
            CheckCollision();
            switch (BalPosition())
            {
                case 0: break;
                case 1:
                    Goal(1);
                    break;
                case 2:
                    Goal(2);
                    break;
                case 3:
                    restart();
                    break;
            }

            SetDirection();
            time += timer.Interval;
            ChangeSpeed();
            SetBall();
        }

        private void SetBall()
        {
            foreach (var robot in team1)
            {
                if (robot.hasBall)
                {
                    bal.x = (int)(robot.x + robot.straal / 4);
                    bal.y = (int)(robot.y + robot.straal / 4);
                }
            }
            foreach (var robot in team2)
            {
                if (robot.hasBall)
                {
                    bal.x = (int)(robot.x + robot.straal / 4);
                    bal.y = (int)(robot.y + robot.straal / 4);
                }
            }
        }

        private void ChangeSpeed()
        {
            foreach (var robot in team1)
            {
                if (robot.speed < robot.maxspeed)
                {
                    robot.speed += robot.acceleration;
                }
                Move(robot);
            }
            foreach (var robot in team2)
            {
                if (robot.speed < robot.maxspeed)
                {
                    robot.speed += robot.acceleration;
                }
                Move(robot);
            }
        }

        private static void Move(Robot robot)
        {
            robot.x -= (int)(robot.speed * robot.direction.X / robot.direction.Length);
            robot.y -= (int)(robot.speed * robot.direction.Y / robot.direction.Length);
        }

        public void Goal(int id)
        {
            switch (id)
            {
                case 1: Score[1]++;
                    restart();
                    break;
                case 2:Score[0]++;
                    restart();
                    break;
            }
        }

        private int BalPosition()
        {
            int b = 0;
            if(bal.x >= 450 && bal.y <= 100 && bal.y >= -100)
            {
                b = 1;
            }
            if (bal.x <= -450 && bal.y <= 100 && bal.y >= -100)
            {
                b = 2;
            }
            if ( bal.y >= 300 || bal.y <= -300)
            {
                b = 3;
            }
            return b;
        }

        private void CheckCollision()
        {
            foreach(var robot1 in team1)
            {
                foreach(var robot2 in team2)
                {
                    if( Math.Sqrt( Math.Pow((robot1.x - robot2.x),2) + Math.Pow((robot1.y - robot2.y), 2)) < robot2.straal || Math.Sqrt(Math.Pow((robot1.x - robot2.x), 2) + Math.Pow((robot1.y - robot2.y), 2)) < robot1.straal)
                    {
                        DoCollision(robot1, robot2);
                    }
                }
            }
            foreach (var robot2 in team2)
            {
                if (Math.Sqrt(Math.Pow((bal.x - robot2.x), 2) + Math.Pow((bal.y - robot2.y), 2)) < robot2.straal || Math.Sqrt(Math.Pow((bal.x - robot2.x), 2) + Math.Pow((bal.y - robot2.y), 2)) < bal.straal)
                {
                    robot2.hasBall = true;
                    team2Bal = true;
                }
            }
            foreach (var robot2 in team1)
            {
                if (Math.Sqrt(Math.Pow((bal.x +bal.straal/2 - (robot2.x + robot2.straal / 2)), 2) + Math.Pow((bal.y + bal.straal / 2 - (robot2.y + robot2.straal / 2)), 2)) < robot2.straal || Math.Sqrt(Math.Pow((bal.x + bal.straal / 2 - (robot2.x + robot2.straal / 2)), 2) + Math.Pow((bal.y + bal.straal / 2 - (robot2.y + robot2.straal / 2)), 2)) < bal.straal)
                {
                    robot2.hasBall = true;
                    team1Bal = true;
                }
            }
        }

        private void DoCollision(Robot robot1, Robot robot2)
        {
            Console.WriteLine("col");
            double temp = robot1.speed;
            robot1.speed = -robot2.speed;
            robot2.speed = -temp;
            if (robot1.hasBall || robot2.hasBall)
            {
                Random rd = new Random();
                if (rd.Next(5) > 3)
                {
                    if (robot1.hasBall)
                    {
                        robot1.hasBall = false;
                        robot2.hasBall = true;
                        team1Bal = false;
                        team2Bal = true;
                    }
                    else
                    {
                        robot1.hasBall = true;
                        robot2.hasBall = false;
                        team1Bal = true;
                        team2Bal = false;
                    }
                }
            }
        }

        #endregion game loop

        #region RRT

        private void initRRT()
        {
            time = 0;
            team1 = new List<Robot>();
            team2 = new List<Robot>();
            Random rnd = new Random();
            bal = new Bal(-(int)balstraal / 2, -(int)balstraal / 2, (int)balstraal);
            for (int i = 0; i < 5; i++)
            {
                 team2.Add(new Robot(rnd.Next(-display.Width / 2 + (int)straal, 0 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height / 2 - (int)straal), straal));
            }
            team1.Add(new Robot(rnd.Next(0 + (int)straal, display.Width / 2 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height / 2 - (int)straal), straal));
            rrtList = new List<System.Drawing.Point[]>();
        }

        private void DoRRT()
        {
            
        }


        #endregion RRT   

        #region renderer

        /// <summary>
        /// Aanmaken backbuffer en transformaties
        /// </summary>
        private void InitRenderer()
        {
            backBuffer = new Bitmap(display.Width, display.Height);
            screen = Graphics.FromImage(backBuffer);
            // transformatie voor display met oorsprong in midden, breedte en hoogte van 1000m, rechtshandig assenstelsel
            SchaalX = display.Width / 1000.0f;
            SchaalY = display.Height / 640.0f;
            screen.ResetTransform();
            screen.ScaleTransform(SchaalX, -SchaalY); //schaling
            screen.TranslateTransform(display.Width / (SchaalX * 2f), -display.Height / (SchaalY * 2f)); // oorsprong in centrum
            // trigger Render voor elke refresh van display;
            display.Paint += new PaintEventHandler(PaintDisplay);
        }


        private void PaintDisplay(object sender, PaintEventArgs e)
        {
            // On_Paint event handler voor display
            Render(e.Graphics);
        }

		/// <summary>
		/// Tekent de aangepaste weergave op de backbuffer en plaatst daarna de backbuffer op het scherm. 
		/// </summary>
		/// <param name="output"></param>
        private void Render(Graphics output)
        {
            //assen
            DrawField();
            DrawBotsAndBal();

            // toon backbuffer op display
            output.DrawImage(backBuffer, new Rectangle(0, 0, display.Width, display.Height), new Rectangle(0, 0, display.Width, display.Height), GraphicsUnit.Pixel);

            // display textboxes
            tijdBox.Text = String.Format("{0:F}", time / 1000.0d);
            label3.Text = "team blauw :" + Score[1];
            label4.Text = "team rood :" + Score[0];

        }

        private void DrawBotsAndBal()
        {
            List<Rectangle> boxes = new List<Rectangle>();
            foreach (var rb in team1)
            {
                Rectangle box = new Rectangle(new System.Drawing.Point((int)(rb.x), (int)(rb.y)), new System.Drawing.Size((int)(rb.straal), (int)(rb.straal)));
                screen.FillEllipse(new SolidBrush(Color.Red), box);
            }
            foreach (var rb in team2)
            {
                Rectangle box = new Rectangle(new System.Drawing.Point((int)(rb.x), (int)(rb.y)), new System.Drawing.Size((int)(rb.straal), (int)(rb.straal)));
                screen.FillEllipse(new SolidBrush(Color.SkyBlue), box);
            }            
            screen.FillEllipse(new SolidBrush(Color.Black), new Rectangle(new System.Drawing.Point((int)(bal.x), (int)(bal.y)), new System.Drawing.Size((int)(bal.straal), (int)(bal.straal))));
        }

        private void DrawField()
        {
            screen.Clear(Color.Green);
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-450, 300), new System.Drawing.Point(450, 300));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-450, -300), new System.Drawing.Point(450, -300));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-450, -300), new System.Drawing.Point(-450, 300));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(450, -300), new System.Drawing.Point(450, 300));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(0, -300), new System.Drawing.Point(0, 300));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(475, -100), new System.Drawing.Point(450, -100));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(475, -100), new System.Drawing.Point(475, 100));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(475, 100), new System.Drawing.Point(450, 100));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-475, -100), new System.Drawing.Point(-450, -100));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-475, -100), new System.Drawing.Point(-475, 100));
            screen.DrawLine(new Pen(Color.White), new System.Drawing.Point(-475, 100), new System.Drawing.Point(-450, 100));
            screen.DrawEllipse(new Pen(Color.White), new Rectangle(new System.Drawing.Point((int)(-centerStraal / 2), (int)(-centerStraal / 2)), new System.Drawing.Size((int)(centerStraal), (int)(centerStraal))));
        }


        #endregion renderer

    }     

    
}
