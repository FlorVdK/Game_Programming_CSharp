using JoPhysics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows;

namespace GDI_framework
{
    public partial class Form1 : Form
    {
        // Timing

        Timer timer;
        int[] Score = new int[2] { 0, 0 };

        // Globale variabelen voor GDI+
        Graphics screen;
        Bitmap backBuffer;
        float SchaalX;
        float SchaalY;
        int centerStraal = 150;

        //teams aanmaken
        int numberRobots = 1;
        List<Robot> team1;
        List<Robot> team2;
        Bal bal;

        // variabelen voor model

        Int32 time;                  // in msec

        double R;                    // van de cirkel, in m
        double theta;                // in radialen;
        double hoeksnelheid;         // in radialen/sec
        const double straal = 30.0d; //van de bol, in m
        const double balstraal = 15.0d; //van de bol, in m
        double rico;



        public Form1()
        {
            InitializeComponent();		//aanmaken inhoud form
            InitRenderer();				//aanmaken backbuffer 
			InitGame();					//Beginwaarden van de simulatie instellen
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
            DoGame();
            CheckCollision();
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
            stopButton.Enabled = false;
            startButton.Enabled = true;		//aanmaken inhoud form
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
            foreach (var robot in team1)
            {
                Vector v = new Vector(robot.y + robot.straal / 2 - bal.y + bal.straal / 2, robot.x + robot.straal / 2 - bal.x + bal.straal / 2);
                robot.direction = v;
            }
            foreach (var robot in team2)
            {
                Vector v = new Vector(bal.y + bal.straal / 2 - robot.y + robot.straal / 2, bal.x + bal.straal / 2 - robot.x + robot.straal / 2);
                robot.direction = v;
            }
        }


        /// <summary>
        /// Elke iteratie de hoek van de bal bijwerken
        /// </summary>
        private void DoGame()
        {
            switch (BalPosition())
            {
                case 0: break;
                case 1:Goal(1);
                    break;
                case 2:Goal(2);
                    break;
                case 3:
                    break;
            }
            SetDirection();
            time += timer.Interval;
            foreach( var robot in team1)
            {
                theta = ((double) robot.y - (double)bal.y) / (double)(robot.x - (double)bal.x);
                if (robot.speed < robot.maxspeed)
                {
                    robot.speed = 2;
                }
                if (robot.x >= 0)
                {
                    if (robot.y >= 0)
                    {
                        robot.x -= (int)(robot.speed * robot.direction.X);
                        robot.y -= (int)(robot.speed * robot.direction.Y);
                    }
                    else
                    {
                        robot.x -= (int)(robot.speed * robot.direction.X);
                        robot.y += (int)(robot.speed * robot.direction.Y);
                    }
                }else
                {
                    if (robot.y >= 0)
                    {
                        robot.x += (int)(robot.speed * robot.direction.X);
                        robot.y -= (int)(robot.speed * robot.direction.Y);
                    }
                    else
                    {
                        robot.x -= (int)(robot.speed * robot.direction.X);
                        robot.y -= (int)(robot.speed * robot.direction.Y);
                    }
                }
            }
            foreach (var robot in team2)
            {
                if (robot.speed < robot.maxspeed)
                {
                    robot.speed += robot.acceleration;
                }
                robot.x += (int)robot.speed;
            }
            bal.x += (int)bal.speed;
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
                        Console.WriteLine("col");
                        robot1.speed = -robot2.speed;
                        robot2.speed = -robot1.speed;
                    }
                }
            }
            foreach (var robot2 in team2)
            {
                if (Math.Sqrt(Math.Pow((bal.x - robot2.x), 2) + Math.Pow((bal.y - robot2.y), 2)) < robot2.straal || Math.Sqrt(Math.Pow((bal.x - robot2.x), 2) + Math.Pow((bal.y - robot2.y), 2)) < bal.straal)
                {
                    bal.speed = robot2.speed;
                }
            }
            foreach (var robot2 in team1)
            {
                if (Math.Sqrt(Math.Pow((bal.x +bal.straal/2 - (robot2.x + robot2.straal / 2)), 2) + Math.Pow((bal.y + bal.straal / 2 - (robot2.y + robot2.straal / 2)), 2)) < robot2.straal || Math.Sqrt(Math.Pow((bal.x + bal.straal / 2 - (robot2.x + robot2.straal / 2)), 2) + Math.Pow((bal.y + bal.straal / 2 - (robot2.y + robot2.straal / 2)), 2)) < bal.straal)
                {
                    bal.speed = robot2.speed;
                }
            }
        }

        #endregion game loop

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
            thetaBox.Text = String.Format("{0:F}", theta);
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
                screen.DrawLine(new Pen(Color.Black), new System.Drawing.Point((int)(0), (int)(0)), new System.Drawing.Point((int)(rb.direction.X *10), (int)(rb.direction.Y*10)));

            }
            foreach (var rb in team2)
            {
                Rectangle box = new Rectangle(new System.Drawing.Point((int)(rb.x), (int)(rb.y)), new System.Drawing.Size((int)(rb.straal), (int)(rb.straal)));
                screen.FillEllipse(new SolidBrush(Color.SkyBlue), box);
                screen.DrawLine(new Pen(Color.Black), new System.Drawing.Point(rb.x, (int)(rb.y + straal / 2)), new System.Drawing.Point((int)(rb.x + rb.straal / 2), (int)(rb.y + straal / 2)));
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
