using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int GetNOD(int val1, int val2)
        {
            if (val2 == 0)
                return val1;
            else
                return GetNOD(val2, val1 % val2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = GetNOD(Decimal.ToInt32(edA.Value), Decimal.ToInt32(edK.Value)).ToString();
        }
    }
}
