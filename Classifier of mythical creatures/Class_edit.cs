﻿using System;
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
    public partial class Class_edit : Form
    {
        
        int id;
        private WorkingMode mode;
        Model model;
        List<(string, string)> primeAttNotInClass;
        ComboBox comboBoxNew;
        public Class_edit(WorkingMode mode_s, int idClass = -1)
        {
            mode = mode_s;
            id = idClass;
            List<ComboBox> comboBoxList = new List<ComboBox>();
            model = new Model();
            model.ConnectToDB();
            InitializeComponent(); 
            textBox1.Text = model.NameClassFromId(id);
            InitializeData(id);
        }

        private void InitializeData(int id)
        {
            panel1.Controls.Clear();
            List<(string, string)> primaryAtt = model.ReadPrimaryAttributeForClass(id);
            Label labelNew;
            Button buttonNew1;
            Button buttonNew2;
            int position = 0;
            foreach (var a in primaryAtt)
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
            primeAttNotInClass = model.ReadPrimaryAttributeNotInClass(id);
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
            Class_dependent_attribute CDA = new Class_dependent_attribute(a.Name, id);
            CDA.Show();

        }

        private void DelPrimaryAttToClass(object sender, EventArgs e)
        {
            Button a = sender as Button;
            model.DelPrimaryAttributeToClass(id, a.Name);
            model.DelDependentAttributeToClassByPrimary(id, a.Name);
            InitializeData(id);
        }
        private void AddPrimaryAttToClass(object sender, EventArgs e)
        {
            if (comboBoxNew.Text != "")
            {
                model.AddPrimaryAttributeToClass(id, IdFromText(primeAttNotInClass, comboBoxNew.Text));
                InitializeData(id);
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
            switch (mode)
            {
                case WorkingMode.Add:
                    break;
                case WorkingMode.Edit:
                    model.EditClass(id, textBox1.Text);
                    Close();
                    break;
                case WorkingMode.Del:
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.DelClass(id);
            Close();
        }
    }
}
