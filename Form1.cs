using Draka_Antivirus.DAO;
using Draka_Antivirus.Pages_Principales;
using Draka_Antivirus.Pages_Principales.Pages_Parametres;
using Draka_Antivirus.Pages_Principales.Scan;
using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Draka_Antivirus
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer timer;
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string name_db = "ScanDataBase.db";
        public static string sourceFile = targetPath + name_db;
        Database db1 = new Database();

        //Pages SYTEME INFO
        ScanPersonalise scanperso = new ScanPersonalise();
        ScanComplete scancomplet = new ScanComplete();
        ParentalControl controlParent = new ParentalControl();
        Home home = new Home();
        Scan scan = new Scan();

        // PATH FILE INFOS
        string path = @"D:\job\AGMA Organization technology inc\Draka new verison\Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Error_Log.txt";
        /*string path = @"C:\Program Files (x86)\Default Company Name\Setup1\Error_Log.txt";*/

        // for test
        /*string path = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Error_Log.txt";*/


        bool drag = false;
        Point start_point = new Point(0, 0);

        public void SetterTitle(string a)
        {
            label3.Text = a;
        }

        public Form1()
        {
            InitializeComponent();
            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            label9.Text = label9.Text + applicationVersion.Major + "." + applicationVersion.Minor + "." + applicationVersion.Build;

            string sqlkey = "select key,dateregist,expiration,daynotkey,passedday from keyregister";
            Object[] dburl = db1.searchData(sourceFile, sqlkey);
            DateTime datet = DateTime.Now;

            if (dburl != null)
            {
                int day = int.Parse(dburl[3].ToString());
                int passedday = int.Parse(dburl[4].ToString());
                if (dburl[2].ToString() != datet.ToString() && passedday < 30)
                {
                    using (SQLiteConnection connection = new SQLiteConnection())
                    {
                        day = passedday + 1;
                        connection.ConnectionString = "Data Source=" + sourceFile;
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandText =
                                "update keyregister set " +
                                "passedday = :datekey";
                            command.Parameters.Add("datekey", DbType.String).Value = day.ToString();
                            if (command.ExecuteNonQuery() > 0)
                            {
                                label1.Text = "il vous reste " + day + " pour activer Draka Antivirus";
                            }
                        }
                    }

                    homeload();
                    openChildrenForm(new Home());
                }
                else if (dburl[2].ToString() == "" && day < 15)
                {
                    using (SQLiteConnection connection = new SQLiteConnection())
                    {
                        day++;
                        connection.ConnectionString = "Data Source=" + sourceFile;
                        connection.Open();
                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            command.CommandText =
                                "update keyregister set " +
                                "daynotkey = :datekey";
                            command.Parameters.Add("datekey", DbType.String).Value = day.ToString();
                            if (command.ExecuteNonQuery() > 0)
                            {
                                label1.Text = "il vous reste " + day + " pour activer Draka Antivirus";
                            }
                        }
                    }
                    partialhome();
                    openChildrenForm(new Home());
                }
                else
                {
                    MessageBox.Show("Cles de produit Draka Antivirus est espirer ou abscente \n Entrer une cle et redemarrer Draka Antivirus", "Enregistrement Draka", MessageBoxButtons.OK);
                    openChildrenForm(new Activation());
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = false;
                }
            }
            else
            {
                using (SQLiteConnection connection = new SQLiteConnection())
                {
                    connection.ConnectionString = "Data Source=" + sourceFile;
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        int dayp = 1;
                        command.CommandText =
                            "update keyregister set " +
                            "daynotkey = :datekey";
                        command.Parameters.Add("datekey", DbType.String).Value = dayp.ToString();
                        if (command.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Veuillez enregistrer votre produit Draka Antivirus \n Entrer une cle et redemarrer Draka Antivirus", "Enregistrement Draka", MessageBoxButtons.OK);
                        }
                    }
                }
                partialhome();
                openChildrenForm(new Home());
                label1.Text = "Veuillez enregistrer votre produit Draka Antivirus !!!!!!!!!!!!!!!!!!!!";
            }
        }

        public Home start()
        {
            if (Program.home == null)
            {
                Program.home = new Home();
            }
            Home home = Program.home;
            return home;
        }
        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true; //drag is your variable flag.
            start_point = new Point(e.X, e.Y);
        }
        private void panelHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }
        private void panelHeader_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true; //drag is your variable flag.
            start_point = new Point(e.X, e.Y);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y);
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }



        private Form activeForm = null;
        public void openChildrenForm(Form childForm)
        {
            //if (activeForm != null)
            //activeForm.Close();
            if ((activeForm is Home) && Program.scanRun)
            {
                activeForm.Hide();
            }
            else
            {
                if (activeForm != null)
                    activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelBody.Controls.Add(childForm);
            panelBody.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            openChildrenForm(new Home());
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Parametres para = new Parametres();
            openChildrenForm(new Parametres());
            /*para.Show();*/
        }

        // Changement de la langue du systeme en fonction des parametre client
        public void ChangeLanguage(string lang)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

        }

        private void OnTimedEvent1(Object source, System.Timers.ElapsedEventArgs e)
        {
            controlParent.parentalControl("active");
        }

        private void panelBody_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panelNotification_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            pictureBox8.Visible = false;
            pictureBox4.Visible = false;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            openChildrenForm(new Contacts());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildrenForm(new Activation());
        }

        public void homeload()
        {
            try
            {
                // Scan au demarrage du systeme
                string sql = "Select * from Settings;";
                Object[] dburl1 = db1.searchData(sourceFile, sql);

                for (int i = 0; i < dburl1.Length; i++)
                {
                    Console.WriteLine(dburl1[i].ToString());
                }

                if (dburl1 != null)
                {
                    // la langue
                    if (dburl1[8].ToString() == "Fr")
                    {
                        ChangeLanguage("fr-FR");
                    }
                    else if (dburl1[8].ToString() == "En")
                    {
                        ChangeLanguage("en-US");
                    }
                    else if (dburl1[8].ToString() == "Defauts")
                    {
                        ChangeLanguage("en-US");
                    }

                    if (dburl1[5].ToString() == "Scan Complet Demarrage" && DateTime.Now.ToString("yyyy-MM-dd") == dburl1[3].ToString())
                    {
                        scancomplet.Visible = true;
                        //scancomplet.CompletScan();
                        openChildrenForm(new NotificationFrom());
                    }
                    else if (dburl1[6].ToString() == "Scan Partiel Demarrage")
                    {
                        if (DateTime.Now.ToString("yyyy-MM-dd") == dburl1[4].ToString())
                        {
                            scanperso.Visible = true;
                            openChildrenForm(new NotificationFrom());
                            //scanperso.startbtnclick(dburl1[2].ToString());
                        }
                    }

                    // Control Parental
                    if (dburl1[10].ToString() == "Activer")
                    {
                        // Parenatl control  run
                        timer = new System.Timers.Timer();
                        timer.Interval = 20000;
                        timer.Elapsed += OnTimedEvent1;
                        timer.AutoReset = true;
                        timer.Enabled = true;
                    }
                    else if (dburl1[10].ToString() == "Defauts")
                    {
                        // control parenatl nom activer
                    }
                    else
                    {
                        // control parenatl nom activer
                    }

                    // mise a jour 

                    if (dburl1[11].ToString() == "Maintenant")
                    {
                        home.DrakaUpdate();
                    }
                    else if (dburl1[11].ToString() == "Mensuel")
                    {
                        if (DateTime.Today.Day.ToString() == "30") home.DrakaUpdate();
                    }
                    else if (dburl1[11].ToString() == "Automatique")
                    {
                        if (DateTime.Today.Day.ToString() == "30") home.DrakaUpdate();
                        else if (DateTime.Today.Day.ToString() == "10") home.DrakaUpdate();
                        else if (DateTime.Today.Day.ToString() == "20") home.DrakaUpdate();
                    }
                    else
                    {
                        if (DateTime.Today.Day.ToString() == "30") home.DrakaUpdate();
                    }
                }

                label1.Text = home.DrakaUpdate();
                if (label1.Text != "")
                {
                    pictureBox4.Visible = true;
                    pictureBox8.Visible = true;
                }
                else
                {
                    pictureBox4.Visible = false;
                    pictureBox8.Visible = false;
                }
                if (scancomplet.Visible == true && scancomplet.backgroundScanComplet.IsBusy == false)
                {
                    scancomplet.Close();
                    openChildrenForm(new Home());
                }
                else if (scanperso.Visible == true && scanperso.backgroundScanPersonaliser.IsBusy == false)
                {
                    scancomplet.Close();
                    openChildrenForm(new Home());
                }
                else
                {
                    openChildrenForm(new Home());
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message: " + ex);
                    tw.Close();
                }

                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message: " + ex);
                    tw.Close();
                }
            }
        }

        public void partialhome()
        {
            label1.Text = home.DrakaUpdate();
            if (label1.Text != "")
            {
                pictureBox4.Visible = true;
                pictureBox8.Visible = true;
            }
            else
            {
                pictureBox4.Visible = false;
                pictureBox8.Visible = false;
            }
            if (scancomplet.Visible == true && scancomplet.backgroundScanComplet.IsBusy == false)
            {
                scancomplet.Close();
                openChildrenForm(new Home());
            }
            else if (scanperso.Visible == true && scanperso.backgroundScanPersonaliser.IsBusy == false)
            {
                scancomplet.Close();
                openChildrenForm(new Home());
            }
            else
            {
                openChildrenForm(new Home());
            }
        }
    }
}
