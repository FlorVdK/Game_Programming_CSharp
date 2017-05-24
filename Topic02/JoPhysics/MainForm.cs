using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JoPhysics;
using GDI_framework;

namespace JoPhysics
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form = new Form1(0);
            form.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form form = new Form1(1);
            form.Show();

        }
    }
}
