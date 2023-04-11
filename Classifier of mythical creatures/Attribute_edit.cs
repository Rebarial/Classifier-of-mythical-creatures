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
    public partial class Attribute_edit : Form
    {
        int id = -1;
        int atType = -1;
        Model model;
        List<(string, string)> types;
        List<(string, string)> primeAtt;

        //type_att: -1 = новый, 0 = первичный, 1 = зависимый.
        //id_att - id атрибута
        public Attribute_edit(int type_att = -1,int id_att = -1)
        {
            InitializeComponent();
            model = new Model();
            model.ConnectToDB();
            id = id_att;
            atType = type_att;
            primeAtt = model.ReadPrimaryAttributes();
            foreach (var a in primeAtt)
            {
                comboBox1.Items.Add(a.Item2);
            }
            types = model.ReadTypes();
            foreach (var a in types)
            {
                comboBox2.Items.Add(a.Item2);
            }
            if (type_att == -1) this.Text = "Добавление паризнака"; else this.Text = "Изменение паризнака";

                
            if (type_att == -1)
            {
                
            }
            if (type_att == 0)
            {
                textBox1.Text = model.selectPrimeAtt(id);
            }
            if (type_att == 1)
            {
                (string, string, string, string, string) depAtt = model.selectDependentAtt(id);
                textBox1.Text = depAtt.Item2;
                comboBox1.SelectedIndex = IdItemFromText(comboBox1, depAtt.Item3);
                comboBox2.SelectedIndex = IdItemFromText(comboBox2, depAtt.Item4);
                textBox2.Text = depAtt.Item5;
            }


        }

        private int IdItemFromText(ComboBox comb, string s)
        {
            int id = 0;
            foreach (var a in comb.Items)
            {

                if (a.ToString() == s) return id;
                id++;
            }
            return -1;
        }

        private string IdFromText(List<(string, string)> list, string s)
        {
            foreach (var a in list)
            {
                if (a.Item2 == s)
                {
                    return a.Item1;
                }
            }
            return "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string dep = IdFromText(primeAtt, comboBox1.Text);
            string type = IdFromText(types,comboBox2.Text);
            string values = textBox2.Text;
            if (name != "" && name != null) { 
                if (dep != "" && dep != null && name != comboBox1.Text)
                {
                    if (type != "" && values != "")
                    {
                        if (atType == -1) model.AddDependentAttributes(name, dep, type, values);
                        if (atType == 1) model.EditDependentAttributes(id.ToString(), name, dep, type, values);
                        if (atType == 0 && name != comboBox1.Text)
                        {
                            model.DelPrimeAttributes(id);
                            model.AddDependentAttributes(name, dep, type, values);
                        }
                    }
                }
                else
                {
                    if (atType == -1) model.AddPrimaryAttributes(name);
                    if (atType == 0) model.EditPrimaryAttributes(id.ToString(), name);
                    if (atType == 1)
                    {
                        model.DelDependentAttributes(id);
                        model.AddPrimaryAttributes(name);
                    }
                }
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (atType == 0) model.DelPrimeAttributes(id);
            if (atType == 1) model.DelDependentAttributes(id);
            Close();
        }
    }
}
