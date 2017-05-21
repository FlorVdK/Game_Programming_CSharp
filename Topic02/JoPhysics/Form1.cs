using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;  // nodig voor GDI+, 2D graphics
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GDI_framework
{
    public partial class Form1 : Form
    {
        // Timing

        Timer timer;

        // Globale variabelen voor GDI+
        Graphics screen;
        Bitmap backBuffer;
        float SchaalX;
        float SchaalY;
        int centerStraal = 150;

        //teams aanmaken
        int numberRobots = 3;
        List<Robot> team1;
        List<Robot> team2;

        // variabelen voor model

        Int32 time;                  // in msec

        double R;                    // van de cirkel, in m
        double theta;                // in radialen;
        double hoeksnelheid;         // in radialen/sec
        const double straal = 40.0d; //van de bol, in m



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

		#endregion interactie


		#region game loop

		/// <summary>
		/// Beginwaarden van de simulatie instellen
		/// </summary>
        private void InitGame()
        {
            R = 300.0d;
            hoeksnelheid = Math.PI * 2 / 4.0d;
            theta = 0.0d;
            time = 0;
            team1 = new List<Robot>();
            team2 = new List<Robot>();
            Random rnd = new Random();
            for (int i = 0; i < numberRobots; i++)
            {
                team1.Add(new Robot(rnd.Next(0 + (int) straal, display.Width / 2 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height/2 - (int)straal), straal));
                team2.Add(new Robot(rnd.Next(-display.Width / 2 + (int)straal, 0 - (int)straal), rnd.Next(-display.Height / 2 + (int)straal, display.Height/2 - (int)straal), straal));
            }
        }


		/// <summary>
		/// Elke iteratie de hoek van de bal bijwerken
		/// </summary>
        private void DoGame()
        {
            time += timer.Interval;
            theta = hoeksnelheid * time / 1000.0d;
            foreach( var robot in team1)
            {
                robot.x--;
            }
            foreach (var robot in team2)
            {
                robot.x++;
            }
        }

        private void CheckCollision()
        {
            foreach(var robot1 in team1)
            {
                foreach(var robot2 in team2)
                {
                    if( Math.Sqrt( Math.Pow((robot1.x - robot2.x),2) + Math.Pow((robot1.y - robot2.y), 2)) < robot2.straal || Math.Sqrt(Math.Pow((robot1.x - robot2.x), 2) + Math.Pow((robot1.y - robot2.y), 2)) < robot1.straal)
                    {
                        robot1.x = robot1.x + 10;
                        robot2.x = robot2.x - 10;
                    }
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
            
            // bol
            double X = R * Math.Cos(theta);
            double Y = R * Math.Sin(theta);
            Rectangle boundingBox = new Rectangle(new Point((int)(X - straal), (int)(Y - straal)), new Size((int)(3 * straal), (int)(2 * straal)));
            screen.DrawEllipse(new Pen(Color.GreenYellow), boundingBox);

            DrawBots();


            // toon backbuffer op display
            output.DrawImage(backBuffer, new Rectangle(0, 0, display.Width, display.Height), new Rectangle(0, 0, display.Width, display.Height), GraphicsUnit.Pixel);

            // display textboxes
            tijdBox.Text = String.Format("{0:F}", time / 1000.0d);
            thetaBox.Text = String.Format("{0:F}", theta);


        }

        private void DrawBots()
        {
            List<Rectangle> boxes = new List<Rectangle>();
            foreach (var rb in team1)
            {
                Rectangle box = new Rectangle(new Point((int)(rb.x), (int)(rb.y)), new Size((int)(rb.straal), (int)(rb.straal)));
                boxes.Add(box);
            }
            foreach (var rb in team2)
            {
                Rectangle box = new Rectangle(new Point((int)(rb.x), (int)(rb.y)), new Size((int)(rb.straal), (int)(rb.straal)));
                boxes.Add(box);
            }
            foreach (var box in boxes)
            {
                screen.DrawEllipse(new Pen(Color.GreenYellow), box);
            }
        }

        private void DrawField()
        {
            Console.Write(display.Width + " " + display.Height);
            screen.Clear(Color.Green);
            screen.DrawLine(new Pen(Color.White), new Point(-450, 300), new Point(450, 300));
            screen.DrawLine(new Pen(Color.White), new Point(-450, -300), new Point(450, -300));
            screen.DrawLine(new Pen(Color.White), new Point(-450, -300), new Point(-450, 300));
            screen.DrawLine(new Pen(Color.White), new Point(450, -300), new Point(450, 300));
            screen.DrawLine(new Pen(Color.White), new Point(0, -300), new Point(0, 300));
            screen.DrawLine(new Pen(Color.White), new Point(475, -100), new Point(450, -100));
            screen.DrawLine(new Pen(Color.White), new Point(475, -100), new Point(475, 100));
            screen.DrawLine(new Pen(Color.White), new Point(475, 100), new Point(450, 100));
            screen.DrawLine(new Pen(Color.White), new Point(-475, -100), new Point(-450, -100));
            screen.DrawLine(new Pen(Color.White), new Point(-475, -100), new Point(-475, 100));
            screen.DrawLine(new Pen(Color.White), new Point(-475, 100), new Point(-450, 100));
            screen.DrawEllipse(new Pen(Color.White), new Rectangle(new Point((int)(-centerStraal / 2), (int)(-centerStraal / 2)), new Size((int)(centerStraal), (int)(centerStraal))));
        }


        #endregion renderer

    }

    public class Robot
    {
        public int x { get; set; }
        public int y { get; set; }
        public double straal { get; set; }
        public bool balBezit { get; set; }

        public Robot()
        {

        }

        public Robot(int x, int y, double straal)
        {
            this.x = x;
            this.y = y;
            this.straal = straal;
            balBezit = false;
        }
    }
}
