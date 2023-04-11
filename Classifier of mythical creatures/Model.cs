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

        public List<(string, string)> ReadTypes()
        {
            myConnection.Open();

            List<(string, string)> lt = new List<(string, string)>();

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select id, name from data_type";
            SQLiteDataReader SRead = Com.ExecuteReader();
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

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select id, name from primary_attribute";
            SQLiteDataReader SRead = Com.ExecuteReader();
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

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select da.id, da.name, pa.name from dependent_attribute as da inner join primary_attribute as pa on pa.id = da.id_primary_attribute";
            SQLiteDataReader SRead = Com.ExecuteReader();
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

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select name from primary_attribute where id = " + id;
            SQLiteDataReader SRead = Com.ExecuteReader();
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

            SQLiteCommand Com = myConnection.CreateCommand();
            Com.CommandText = "select da.id, da.name, pa.name, dt.name, da.data_value from dependent_attribute as da inner join primary_attribute as pa on pa.id = da.id_primary_attribute, data_type as dt on da.id_data_type = dt.id where da.id = " + id;
            SQLiteDataReader SRead = Com.ExecuteReader();
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

    }
}
