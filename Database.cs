using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Draka_Antivirus.DAO
{
    class Database
    {
        public static string name_db3 = "objectionable_websites.db";
        // chemin jusqu'au repertoire courant
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        string path = targetPath + "Error_Log.txt";

        public void ErrorMessage(string sql, string e)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + e);
                tw.Close();
            }

            else if (File.Exists(path))
            {
                TextWriter tw = new StreamWriter(path, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + e);
                tw.Close();
            }
        }


        /* 
         * Création d'une base de données SQLite 
         */
        public string createDatabase(string name_db)
        {
            string sourceFile = targetPath + name_db;

            if (File.Exists(sourceFile) != true)
            {
                try
                {
                    File.Create(sourceFile);
                    AutoClosingMessageBox.Show("BD creer ");
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage("Create Database ", ex.ToString());
                    return null;
                }
            }

            return sourceFile;
        }
        /* 
         * Sélectionner des données dans une base de données SQLite
         */
        public List<Object[]> selectDatas(string sourceFile, string sql)
        {
            List<Object[]> liste = null;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);

                    con.Open();
                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        liste = new List<Object[]>();
                        while (datas.Read())
                        {
                            Object[] element = new Object[datas.FieldCount];
                            for (var i = 0; i < datas.FieldCount; i++)
                            {
                                element[i] = datas.GetValue(i);
                            }

                            liste.Add(element);
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);*//*
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return liste;
        }
        /* 
         * Supprimer une base de données SQLite 
         */
        public Boolean deleteDatabase(string name_db)
        {
            string sourceFile = targetPath + name_db;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    File.Delete(sourceFile);
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage("Delete Database ", ex.ToString());
                    return false;
                }
            }

            return true;
        }
        //   creation d'une table dans une base de données
        //
        // creation d'une table de la base de donnée
        public Boolean CreateTable(string sourceFile, string nameTable)
        {
            if (!File.Exists(sourceFile))
            {
                sourceFile = createDatabase(name_db3);
                //MessageBox.Show("Base de données bien creeeeeeeeeeeeeeeeer");
            }
            else
            {
                String sql = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement , chemin text, nomfichier text, nouveldirection text, date text, taille text, etat text, editeur text, action text, detection text);";
                string myCommand = "CREATE TABLE " + nameTable + "(url varchar(128) not null primary key, status varchar(128), date varchar(128))";

                if (nameTable == "Quarantaine")
                {
                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(sql, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        //AutoClosingMessageBox.Show("La table quarantaine existe deja");
                        //MessageBox.Show("Echec Table Quarantaine ");
                        return false;
                    }
                }
                else if (nameTable == "parentControl")
                {
                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //AutoClosingMessageBox.Show("la table parentControl a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "FullScan")
                {
                    string myCommand1 = "CREATE TABLE " + nameTable + "(Id INTEGER AUTOINCREMENT not null primary key ,date text , taille text, statut varchar(128))";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand1, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "HistoryScan")
                {
                    // ici nous crayons la table historique de scan
                    string myCommand11 = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement, date text , duree text, TotalVirus text, totalAna text, TypeScan varchar(128), Etat text)";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand11, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //AutoClosingMessageBox.Show("la table Historique de scan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "Chiarita")
                {
                    string myCommand1 = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement , chemin text, date text, nomfichier text)";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand1, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "Admin")
                {
                    string myCommand1 = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement , Email text, password text, dateActivation text)";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand1, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }
                else if (nameTable == "websites")
                {
                    string myCommand1 = "CREATE TABLE " + nameTable + "(Id Integer primary key Autoincrement , url text, statut text, date text)";

                    try
                    {
                        var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                        var cmd = new SQLiteCommand(myCommand1, con);
                        con.Open();
                        //MessageBox.Show("Table creer etape1");
                        cmd.ExecuteNonQuery();
                        //MessageBox.Show("Table creer etape2");
                        con.Close();
                        //  AutoClosingMessageBox.Show("la table FullScan a été créer avec succes");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Internal Error : " + ex.Message);
                        //Console.WriteLine("Internal Error : " + ex.Message);
                        ErrorMessage(sql, ex.ToString());
                        return false;
                    }
                }

            }
            //MessageBox.Show("Table creer avec succes");
            return true;
        }

        /* 
         * Sélectionner des données dans une base de données SQLite
         * Et ajouter une donnée pour incrémenter automatiquement
         * le numéro ou l'identifiant de la table
         */
        public List<Object[]> selectDatasAuto(string sourceFile, string sql)
        {
            List<Object[]> liste = null;

            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);

                    con.Open();
                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        int j = 1;
                        liste = new List<Object[]>();
                        while (datas.Read())
                        {
                            Object[] element = new Object[datas.FieldCount + 1];

                            for (var i = 0; i < (datas.FieldCount + 1); i++)
                            {
                                if (i == 0)
                                {
                                    element[i] = j;
                                }
                                else
                                {
                                    element[i] = datas.GetValue(i - 1);
                                }
                            }

                            liste.Add(element);
                            j = j + 1;
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);*/
                    /*Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return liste;
        }
        /* 
         * Sélectionner une donnée dans une base de données SQLite
         */
        public Object[] searchData(string sourceFile, string sql)
        {
            Object[] element = null;
            //MessageBox.Show("la recherche encours");
            if (File.Exists(sourceFile) == true)
            {
                //AutoClosingMessageBox.Show("oui la base de données existe");
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();

                    SQLiteDataReader datas = cmd.ExecuteReader();
                    if (datas.HasRows == true)
                    {
                        while (datas.Read())
                        {
                            element = new Object[datas.FieldCount];
                            for (var i = 0; i < datas.FieldCount; i++)
                            {
                                element[i] = datas.GetValue(i);
                            }
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage(sql, ex.ToString());
                }
            }

            return element;
        }

        /* 
         * Supprimer des données dans une base de données SQLite
         */
        public Boolean deleteData(string sourceFile, string sql)
        {
            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    //MessageBox.Show("suppression encours");
                    con.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    /* MessageBox.Show("Internal Error : " + ex.Message);
                     Console.WriteLine("Internal Error : " + ex.Message);*/
                    ErrorMessage(sql, ex.ToString());
                    return false;
                }
            }

            return false;
        }

        /* 
         * Recuperer le dernier element inserer dans une base de donne SQLite
         */
        public int LasInsertId(string sourceFile, string sql)
        {
            int insertid = 1;
            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();
                    insertid = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                    return insertid;
                }
                catch (Exception ex)
                {
                    /*MessageBox.Show("Internal Error : " + ex.Message);*/
                    Console.WriteLine("Internal Error 1 : " + ex.Message);
                    /*ErrorMessage(sql, ex.ToString());*/
                    return insertid;
                }
            }

            return insertid;
        }


        /* 
         * Inserer des données dans une base de données SQLite
         */
        public Boolean insertData(string sourceFile, string sql)
        {
            if (File.Exists(sourceFile) == true)
            {
                try
                {
                    var con = new SQLiteConnection("Data Source=" + sourceFile + ";");
                    var cmd = new SQLiteCommand(sql, con);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    //AutoClosingMessageBox.Show("Insertion réussi");
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Internal Error : " + ex.Message);
                    Console.WriteLine("Internal Error : " + ex.Message);
                    ErrorMessage(sql, ex.ToString());
                    return false;
                }
            }
            //MessageBox.Show("Insertion réussi");
            return true;
        }


    }
}
