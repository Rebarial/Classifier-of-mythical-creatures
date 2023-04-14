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
    public partial class Class_predict : Form
    {
        
        Model model;
        List<(string, string)> primeAttNotInClass;
        List<(string, string)> patt_in_class;
        List<(string, string, string, string)> dependentAttInClass;
        ComboBox comboBoxNew;
        public Class_predict()
        {
            dependentAttInClass = new List<(string, string, string, string)>();
            patt_in_class = new List<(string, string)> ();
            List<ComboBox> comboBoxList = new List<ComboBox>();
            model = new Model();
            model.ConnectToDB();
            InitializeComponent();
            InitializeData();
            
        }

        private void InitializeData()
        {
            panel1.Controls.Clear();
            Label labelNew;
            Button buttonNew1;
            Button buttonNew2;
            int position = 0;
            foreach (var a in patt_in_class)
            {
                labelNew = new Label();
                buttonNew1 = new Button();
                buttonNew2 = new Button();
                buttonNew1.Location = new Point(100, position);
                buttonNew2.Location = new Point(190, position);
                labelNew.Location = new Point(12, position);
                position += 30;
                labelNew.Text = a.Item2;
                buttonNew1.Text = "Зависимые";
                buttonNew1.Name = a.Item1;
                buttonNew1.Click += EditDepAttribute;
                buttonNew2.Text = "Удалить";
                buttonNew2.Click += DelPrimaryAttToClass;
                labelNew.Width = 70;
                buttonNew1.Width = 80;
                buttonNew2.Width = 60;
                buttonNew2.Name = a.Item1;
                //this.Controls.Add(comboBoxNew);
                panel1.Controls.Add(labelNew);
                panel1.Controls.Add(buttonNew1);
                panel1.Controls.Add(buttonNew2);
                //comboBoxList.Add(comboBoxNew);
            }
            primeAttNotInClass = model.ReadPrimaryAttributeNotInClassByArrow(patt_in_class);
            if (primeAttNotInClass.Count > 0)
            {
                comboBoxNew = new ComboBox();
                comboBoxNew.Location = new Point(12, position);
                foreach (var a in primeAttNotInClass)
                {
                    comboBoxNew.Items.Add(a.Item2);
                }
                comboBoxNew.Width = 70;
                comboBoxNew.DropDownStyle = ComboBoxStyle.DropDownList;
                buttonNew1 = new Button();
                buttonNew1.Location = new Point(190, position);
                buttonNew1.Width = 60;
                buttonNew1.Click += AddPrimaryAttToClass;
                buttonNew1.Text = "Добавить";
                panel1.Controls.Add(comboBoxNew);
                panel1.Controls.Add(buttonNew1);
            }
        }

        private void EditDepAttribute(object sender, EventArgs e)
        {
            Button a = sender as Button;
            Class_predict_dependent_attribute CDA = new Class_predict_dependent_attribute(a.Name, dependentAttInClass);
            CDA.Show();
        }

        private void DelPrimaryAttToClass(object sender, EventArgs e)
        {
            Button a = sender as Button;
            DelFromListPatt(a.Name);
            InitializeData();
        }

        private void DelFromListPatt(string id)
        {
            (string, string) del = ("", "");
            foreach(var a in patt_in_class)
            {
                
                if (a.Item1 == id)
                {
                    del = a;
                }
                
            }
            patt_in_class.Remove(del);
        }
        private void AddPrimaryAttToClass(object sender, EventArgs e)
        {
            if (comboBoxNew.Text != "")
            {
                patt_in_class.Add((IdFromText(primeAttNotInClass, comboBoxNew.Text), comboBoxNew.Text));
                InitializeData();
            }
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

        private void Class_predict_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<(string, string)>  da = model.GetDAttClasses();
            List<(string, string)> pa = model.GetPAttClasses();
            string class_name = "Неопределен";
            bool check = true;
            foreach (var p in pa)
            {
                check = true;
                if (p.Item2.Split(',').ToList().Count != dependentAttInClass.Count)
                    check = false;
                foreach (var pr in patt_in_class)
                {
                    if (!p.Item2.Split(',').ToList().Contains(pr.Item1))
                        check = false;
                }
                /*if (check)
                {
                    foreach (var d in da)
                    {
                        
                        if (d.Item2.Split(',').ToList().Count != dependentAttInClass.Count)
                            check = false;
                        foreach (var dep in dependentAttInClass)
                        {
                            if (!d.Item2.Split(',').ToList().Contains(dep.Item1))
                                check = false;
                        }
                    }
                }*/
                if (check)
                    class_name = model.GetClassName(p.Item1);
            }
            //p.Item2.Split(',').ToList().Count
            //class_name = model.GetClassName(p.Item1);
            textBox1.Text = class_name;
        }
    }
}
