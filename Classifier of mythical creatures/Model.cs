using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Classifier_of_mythical_creatures
{
    internal class Model
    {

        private SQLiteConnection myConnection;

        public void ConnectToDB()
        {
            myConnection = new SQLiteConnection("Data Source=database.sqlite3");
            if (!File.Exists("./database.sqlite3"))
            {
                SQLiteConnection.CreateFile("database.sqlite3");
            }
            

        }

        private SQLiteDataReader ExecuteSql(string command)
        {
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = command;
            return Com.ExecuteReader();
        }

        public List<(string, string)> ReadTypes()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql("select id, name from data_type");
            
            while(SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }
        public List<(string, string)> ReadPrimaryAttributes()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql("select id, name from primary_attribute");
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public List<(string, string, string)> ReadDependentAttributes()
        {
            myConnection.Open();

            List<(string, string, string)> lt = new List<(string, string, string)>();

            SQLiteDataReader SRead = ExecuteSql("select da.id, da.name, pa.name from dependent_attribute as da inner join primary_attribute as pa on pa.id = da.id_primary_attribute");
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString(), SRead[2].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public string selectPrimeAtt(int id)
        {
            myConnection.Open();
            SQLiteDataReader SRead = ExecuteSql("select name from primary_attribute where id = " + id);
            string lt = "";
            while (SRead.Read())
            {
                lt = SRead[0].ToString();
            }

            myConnection.Close();
            return lt;
        }

        public (string,string,string,string, string) selectDependentAtt(int id)
        {
            myConnection.Open();

            SQLiteDataReader SRead = ExecuteSql("select da.id, da.name, pa.name, dt.name, da.data_value from dependent_attribute as da inner join primary_attribute as pa on pa.id = da.id_primary_attribute, data_type as dt on da.id_data_type = dt.id where da.id = " + id);
            (string, string, string, string, string) lt = ("", "", "", "", "");
            while (SRead.Read())
            {
                lt = (SRead[0].ToString(), SRead[1].ToString(), SRead[2].ToString(), SRead[3].ToString(), SRead[4].ToString());
            }

            myConnection.Close();
            return lt;
        }

        public void AddPrimaryAttributes(string name)
        {
            myConnection.Open();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "insert into primary_attribute(name) values ( "+ '"' + name + '"' + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void AddDependentAttributes(string name, string id_prime, string id_type, string values)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "insert into dependent_attribute(id_primary_attribute, name, id_data_type, data_value) values ( " + id_prime + ',' + '"' + name + '"' + ',' + id_type + ',' + '"' + values + '"' + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelDependentAttributes(int id)
        {
            myConnection.Open();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Delete from dependent_attribute where id = " + id;
            Com.ExecuteNonQuery();
            myConnection.Close();
            Console.WriteLine(Com.CommandText);
        }

        public void DelPrimeAttributes(int id)
        {
            myConnection.Open();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Delete from primary_attribute where id = " + id;
            Com.ExecuteNonQuery();
            myConnection.Close();
            Console.WriteLine(Com.CommandText);
        }

        public void EditDependentAttributes(string id, string name, string id_prime, string id_type, string values)
        {
            myConnection.Open();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Update dependent_attribute set id_primary_attribute =" + id_prime + ", name = " + '"' + name + '"' + ",id_data_type = " + id_type + ", data_value =" + '"' + values + '"' + " where id = " + id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void EditPrimaryAttributes(string id, string name)
        {
            myConnection.Open();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Update primary_attribute set name = " + '"' + name + '"' + " where id = " + id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public List<(string, string)> ReadClasses()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select id, name from class";
            SQLiteDataReader SRead = Com.ExecuteReader();
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public List<(string, string)> ReadPrimaryAttributeForClass(int id)
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                @"select pvfc.id_patt, pa.name 
                from patt_value_for_class as pvfc 
                inner join primary_attribute as pa on pvfc.id_patt = pa.id where pvfc.id_class = " + id
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public List<(string, string)> ReadPrimaryAttributeNotInClass(int id)
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                @"select id, name from primary_attribute as pa left join (select id_patt from patt_value_for_class where id_class = " + id + 
                ") as pvfc on pvfc.id_patt = pa.id where pvfc.id_patt is null"
            ) ;
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public string NameClassFromId(int id)
        {
            myConnection.Open();

            SQLiteDataReader SRead = ExecuteSql(
                @"SELECT name FROM class where id = " + id
            ) ;
            string s = "";
            while (SRead.Read())
            {
                s = SRead[0].ToString();
            }
            myConnection.Close();
            return s;
        }

        public void AddPrimaryAttributeToClass(int class_id, string attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "INSERT INTO patt_value_for_class(id_class, id_patt) values(" + class_id + ", " + attribute_id + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelPrimaryAttributeToClass(int class_id, string attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM patt_value_for_class WHERE id_class = " + class_id + " AND id_patt = " + attribute_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void AddClass(string name)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "INSERT INTO class(name) values(" + '"' + name + '"' + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void EditClass(int id, string name)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Update class set name = " + '"' + name + '"' + " where id = " + id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }


        public List<(string, string, string)> ReadDependentAttributeForClass(string id_patt, int id_class)
        {
            myConnection.Open();

            List<(string, string, string)> lt = new List<(string, string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                @"select id, name, value from datt_value_for_class as dvfc 
                inner join dependent_attribute as da on dvfc.id_datt = da.id 
                where da.id_primary_attribute = " + id_patt + " and dvfc.id_class = "+ id_class +";"
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString(), SRead[2].ToString()));
            }
            myConnection.Close();
            return lt;
        }


        public List<(string, string)> ReadDependentAttributeNotInClass(string id_patt, int id_class)
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                @"select id, name from dependent_attribute as da 
                left join (select id_datt from datt_value_for_class where id_class = " + id_class +@") as dvfc on dvfc.id_datt = da.id 
                where dvfc.id_datt is null and da.id_primary_attribute = "+ id_patt+";"
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }


        public void AddDependentAttributeToClass(int class_id, string attribute_id, string value)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "INSERT INTO datt_value_for_class(id_class, id_datt, value) values(" + class_id + ", " + attribute_id + "," + '"' + value + '"' + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelDependentAttributeToClass(int class_id, string attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM datt_value_for_class WHERE id_class = " + class_id + " AND id_datt = " + attribute_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelDependentAttributeToClassByPrimary(int class_id, string attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM datt_value_for_class " +
                "WHERE id_datt=(SELECT id_datt from datt_value_for_class as dvfc " +
                "inner join dependent_attribute as da on dvfc.id_datt=da.id where da.id_primary_attribute="+ attribute_id +" and dvfc.id_class="+ class_id+")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void EditDependentAttributeToClass(int class_id, string attribute_id, string value)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "Update datt_value_for_class set value = " + '"' + value + '"' + " where id_class = " + class_id + " and id_datt = " + attribute_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelClass(int class_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM class WHERE id = " + class_id;
            Com.ExecuteNonQuery();
            Com.CommandText = "DELETE FROM patt_value_for_class WHERE id_class = " + class_id;
            Com.ExecuteNonQuery();
            Com.CommandText = "DELETE FROM datt_value_for_class WHERE id_class = " + class_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public (string, string) GetTypeAttribute(string attribute_id)
        {
            myConnection.Open();

            (string, string) lt = ("", "");

            SQLiteDataReader SRead = ExecuteSql(
                @"select id_data_type, data_value from dependent_attribute where id = " + attribute_id
            ) ;
            while (SRead.Read())
            {
                lt = (SRead[0].ToString(), SRead[1].ToString());
            }
            myConnection.Close();
            return lt;
        }

        public void DelDependentAttributeToClassByAttId(int attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM datt_value_for_class " +
                "WHERE id_datt = " + attribute_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelPrimeAttributeToClassByAttId(int attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM patt_value_for_class " +
                "WHERE id_patt = " + attribute_id;
            Com.ExecuteNonQuery();
            myConnection.Close();
        }

        public void DelDependentByPrimeAttributeToClassByAttId(int attribute_id)
        {
            myConnection.Open();
            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "DELETE FROM datt_value_for_class " +
                "WHERE id_datt=(SELECT id_datt from datt_value_for_class as dvfc " +
                "inner join dependent_attribute as da on dvfc.id_datt=da.id where da.id_primary_attribute=" + attribute_id + ")";
            Com.ExecuteNonQuery();
            myConnection.Close();
        }


        public List<(string, string)> ReadPrimaryAttributeNotInClassByArrow(List<(string, string)> attIn)
        {
            myConnection.Open();
            string command;
            List<(string, string)> lt = new List<(string, string)>();
            if (attIn.Count > 0)
            {
                command = "select id, name from primary_attribute where id not in (";
                foreach (var att in attIn)
                {
                    command += att.Item1;
                    command += ",";
                }
                command = command.Remove(command.Length - 1);
                command += ")";
            }
            else
            {
                command = "select id, name from primary_attribute";
            }
            SQLiteDataReader SRead = ExecuteSql(
                command
            ) ;
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public List<(string, string)> ReadDependentAttributeNotInClassByArrow(List<(string, string, string,string)> attIn,string patt)
        {
            myConnection.Open();
            string command;
            List<(string, string)> lt = new List<(string, string)>();
            if (attIn.Count > 0)
            {
                command = "select id, name from dependent_attribute where id not in (";
                foreach (var att in attIn)
                {
                    command += att.Item1;
                    command += ",";
                }
                command = command.Remove(command.Length - 1);
                command += ") and id_primary_attribute=" + patt;
            }
            else
            {
                command = "select id, name from dependent_attribute where id_primary_attribute=" + patt;
            }
            SQLiteDataReader SRead = ExecuteSql(
                command
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }

        public List<(string, string, string)> ReadDependentAttributesByPrimaryId(string id)
        {
            myConnection.Open();

            List<(string, string, string)> lt = new List<(string, string, string)>();

            SQLiteDataReader SRead = ExecuteSql("select id, name, id_primary_attribute from dependent_attribute where id_primary_attribute = " + id);
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString(), SRead[2].ToString()));
            }
            myConnection.Close();
            return lt;
        }


        public List<(string, string)> GetPAttClasses()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                @"select id_class, GROUP_CONCAT(id_patt) as gc from patt_value_for_class group by id_class"
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }
        public List<(string, string)> GetDAttClasses()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteDataReader SRead = ExecuteSql(
                "select id_class, GROUP_CONCAT(id_datt) as gc from datt_value_for_class group by id_class;"
            );
            while (SRead.Read())
            {
                lt.Add((SRead[0].ToString(), SRead[1].ToString()));
            }
            myConnection.Close();
            return lt;
        }


        public string GetClassName(string id)
        {
            myConnection.Open();

            string s = "";

            SQLiteDataReader SRead = ExecuteSql(
                "select name from class where id = " + id
            ) ;
            while (SRead.Read())
            {
                s = SRead[0].ToString();
            }
            myConnection.Close();
            return s;
        }

    }
}
