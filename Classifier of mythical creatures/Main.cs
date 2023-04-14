using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Classifier_of_mythical_creatures
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class_predict frm = new Class_predict();
            frm.Show();
            Hide();
        }
    }
}
