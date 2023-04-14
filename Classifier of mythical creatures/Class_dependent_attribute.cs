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
    public partial class Class_dependent_attribute : Form
    {
        
        int id;
        string pattId;
        private WorkingMode mode;
        Model model;
        List<(string, string)> dependentAttNotInClass;
        ComboBox comboBoxNew;
        public Class_dependent_attribute(string idPAtt, int idClass = -1)
        {
            pattId = idPAtt;
            id = idClass;
            List<ComboBox> comboBoxList = new List<ComboBox>();
            model = new Model();
            model.ConnectToDB();
            InitializeComponent();
            label6.Text = model.NameClassFromId(idClass);
            InitializeData(id);

        }

        private void InitializeData(int id)
        {
            panel1.Controls.Clear();
            List<(string, string, string)> primaryAtt = model.ReadDependentAttributeForClass(pattId,id);
            Label labelNew;
            ComboBox textBoxNew1;
            Button buttonNew2;
            Button buttonNew1;
            int position = 0;
            foreach (var a in primaryAtt)
            {
                labelNew = new Label();
                textBoxNew1 = new ComboBox();
                textBoxNew1.Name = a.Item1;
                buttonNew2 = new Button();
                buttonNew1 = new Button();
                textBoxNew1.Location = new Point(100, position);
                buttonNew2.Location = new Point(190, position);
                buttonNew1.Location = new Point(220, position);
                labelNew.Location = new Point(12, position);
                position += 30;
                labelNew.Text = a.Item2;
                textBoxNew1.Text = a.Item3;
                textBoxNew1.Name = a.Item1 + "textbox";
                (string, string) s = model.GetTypeAttribute(a.Item1);
                if (s.Item1 == "2")
                {
                    Console.WriteLine(s.Item1);
                    List<string> tokens = s.Item2.Split(',').ToList();
                    textBoxNew1.DropDownStyle = ComboBoxStyle.DropDownList;
                    foreach (string token in tokens)
                    {
                        textBoxNew1.Items.Add(token);
                    }
                    if (a.Item3 == "")
                    {
                        textBoxNew1.SelectedIndex = 0;
                    }
                    else
                    {
                        textBoxNew1.SelectedIndex = IdItemFromText(textBoxNew1, a.Item3);
                    }

                }
                if (s.Item1 == "1")
                {
                    textBoxNew1.DropDownStyle = ComboBoxStyle.DropDownList;
                    textBoxNew1.Items.Add("Да");
                    textBoxNew1.Items.Add("Нет");
                    if (a.Item3 != "")
                    {
                        textBoxNew1.SelectedIndex = IdItemFromText(textBoxNew1, a.Item3);
                    }
                    else
                    {
                        textBoxNew1.SelectedIndex = 0;
                    }

                }
                //textBoxNew1.Text = a.Item3;
                buttonNew2.Text = "Изменить";
                buttonNew1.Text = "Удалить";
                buttonNew2.Click += EditDependentAttToClass;
                buttonNew1.Click += DelDependentAttToClass;
                buttonNew1.Name = a.Item1;
                labelNew.Width = 70;
                textBoxNew1.Width = 80;
                buttonNew2.Width = 30;
                buttonNew1.Width = 30;
                buttonNew2.Name = a.Item1;
                //this.Controls.Add(comboBoxNew);
                panel1.Controls.Add(labelNew);
                panel1.Controls.Add(textBoxNew1);
                panel1.Controls.Add(buttonNew2);
                panel1.Controls.Add(buttonNew1);
                //comboBoxList.Add(comboBoxNew);
            }

            dependentAttNotInClass = model.ReadDependentAttributeNotInClass(pattId,id);
            if (dependentAttNotInClass.Count > 0)
            {
                comboBoxNew = new ComboBox();
                comboBoxNew.Location = new Point(12, position);
                foreach (var a in dependentAttNotInClass)
                {
                    comboBoxNew.Items.Add(a.Item2);

                }
                comboBoxNew.Width = 70;
                comboBoxNew.DropDownStyle = ComboBoxStyle.DropDownList;
                buttonNew2 = new Button();
                buttonNew2.Location = new Point(190, position);
                buttonNew2.Width = 60;
                buttonNew2.Click += AddDependentAttToClass;
                buttonNew2.Text = "Добавить";
                panel1.Controls.Add(comboBoxNew);
                panel1.Controls.Add(buttonNew2);
            }
        }


        private void DelDependentAttToClass(object sender, EventArgs e)
        {
            Button a = sender as Button;
            model.DelDependentAttributeToClass(id, a.Name);
            InitializeData(id);
        }

        private void EditDependentAttToClass(object sender, EventArgs e)
        {
            Button a = sender as Button;
            ComboBox textBox = this.Controls.Find(a.Name + "textbox",true).FirstOrDefault() as ComboBox;
            (string,string) s = model.GetTypeAttribute(a.Name);
            if (s.Item2 != "")
            {
                if (s.Item1 == "1")
                {
                    if (a.Text != "")
                    {
                        model.EditDependentAttributeToClass(id, a.Name, textBox.Text);
                    }

                }
                if (s.Item1 == "2")
                {
                    if (a.Text != "")
                    {
                        model.EditDependentAttributeToClass(id, a.Name, textBox.Text);
                    }

                }
                if (s.Item1 == "3")
                {
                    try
                    {
                        Console.WriteLine(s.Item1);
                        string v1 = s.Item2.Substring(0, s.Item2.IndexOf(':'));
                        string v2 = s.Item2.Substring(s.Item2.IndexOf(':') + 1);
                        Console.WriteLine(Int32.Parse(v1) + " " + Int32.Parse(v2));
                        if (Int32.Parse(v1) <= Int32.Parse(textBox.Text) && Int32.Parse(textBox.Text) <= Int32.Parse(v2))
                        {
                            model.EditDependentAttributeToClass(id, a.Name, textBox.Text);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                InitializeData(id);
            }
        }
        private void AddDependentAttToClass(object sender, EventArgs e)
        {
            if (comboBoxNew.Text != "")
            {
                string attId = IdFromText(dependentAttNotInClass, comboBoxNew.Text);
                (string, string) a = model.GetTypeAttribute(attId);
                switch (a.Item1)
                {
                    case "1":
                        model.AddDependentAttributeToClass(id, attId, "Да");
                        break;
                    case "2":
                        model.AddDependentAttributeToClass(id, attId, a.Item2.Split(',').ToList()[0]);
                        break;
                    case "3":
                        model.AddDependentAttributeToClass(id, attId, a.Item2.Substring(0, a.Item2.IndexOf(':')));
                        break;
                    default:
                        break;

                      
                }
                InitializeData(id);
            }
        }

        public int IdItemFromText(ComboBox comb, string s)
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
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
