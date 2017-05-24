using GDI_framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace JoPhysics
{
    public class Robot : IRobot
    {
        public int x { get; set; }
        public int y { get; set; }
        public double straal { get; set; }
        public double speed { get; set; }
        public double maxspeed { get; set; }
        public double acceleration { get; set; }
        public Vector direction { get; set; }
        public bool hasBall { get; set; }

        public Robot()
        {

        }

        public Robot(int x, int y, double straal)
        {
            this.x = x;
            this.y = y;
            this.straal = straal;
            speed = 0;
            maxspeed = 4;
            acceleration = 0.1;
        }

        public Robot(int x, int y, double straal, Vector direction)
        {
            this.x = x;
            this.y = y;
            this.straal = straal;
            this.direction = direction;
            speed = 0;
            maxspeed = 4;
            acceleration = 0.1;
        }
    }
}
