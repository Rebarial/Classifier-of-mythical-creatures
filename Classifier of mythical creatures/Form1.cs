using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Classifier_of_mythical_creatures
{
    public partial class Form1 : Form
    {

        List<(string, string)> primeAtt;
        List<(string, string, string)> dependentAtt;
        Label label1;
        List<Label> labelAttList;
        Model model;
        public Form1()
        {
            
            InitializeComponent();
      
            model = new Model();
            model.ConnectToDB();
            
            labelAttList = new List<Label>();


            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size((tabControl1.Width / tabControl1.TabPages.Count) - 2, tabControl1.ItemSize.Height);
            updateAtt();
            

        }

        private void updateAtt()
        {
            foreach (var a in labelAttList)
                this.tabPage2.Controls.Remove(a);
            int position = 60;
            primeAtt = model.ReadPrimaryAttributes();
            dependentAtt = model.ReadDependentAttributes();
            foreach (var a in primeAtt)
            {
                label1 = new Label();
                label1.Location = new Point(6, position);
                position += 30;
                label1.Name = a.Item1;
                label1.Width = 230;
                label1.Text = a.Item2 + " (Первичный)";
                label1.Visible = true;
                label1.Click += P_Att_Click;
                this.tabPage2.Controls.Add(label1);
                labelAttList.Add(label1);

            }
            foreach (var a in dependentAtt)
            {
                label1 = new Label();
                label1.Location = new Point(6, position);
                position += 30;
                label1.Name = a.Item1;
                label1.Width = 230;
                label1.Text = a.Item2 + " (Зависимый от " + a.Item3 + ")";
                label1.Visible = true;
                label1.Click += D_Att_Click;
                labelAttList.Add(label1);
                this.tabPage2.Controls.Add(label1);
            }

        }

        private void P_Att_Click(object sender, EventArgs e)
        {
            Label a = sender as Label;
            Attribute_edit AEForm = new Attribute_edit(0, int.Parse(a.Name));
            AEForm.ShowDialog();
            updateAtt();
        }

        private void D_Att_Click(object sender, EventArgs e)
        {
            Label a = sender as Label;
            Attribute_edit AEForm = new Attribute_edit(1, int.Parse(a.Name));
            AEForm.ShowDialog();
            updateAtt();
        }


        private void AddLabel(string text)
        { 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
                    }

        private void buttonAddClass_Click(object sender, EventArgs e)
        {
            Class_edit CEForm = new Class_edit();
            CEForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Attribute_edit AEForm = new Attribute_edit();
            AEForm.ShowDialog();
            updateAtt();
        }
    }
}
