using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoPhysics
{
    public class Bal
    {
        public int x { get; set; }
        public int y { get; set; }
        public double straal { get; set; }
        public double speed { get; set; }
        public double maxspeed { get; set; }
        public double acceleration { get; set; }

        public Bal()
        {

        }

        public Bal(int x, int y, double straal)
        {
            this.x = x;
            this.y = y;
            this.straal = straal;
            speed = 0;
            maxspeed = 4;
            acceleration = -0.1;
        }
    }
}
