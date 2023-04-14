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
    public partial class AddClass : Form
    {
        Model model;
        public AddClass()
        {
            model = new Model();
            model.ConnectToDB();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                model.AddClass(textBox1.Text);
                Close();
            }
        }
    }
}
