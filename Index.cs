using Draka_Antivirus.Pages_Principales.Scan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Draka_Antivirus.DAO;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Diagnostics;
using Guna.UI2.WinForms;
using Draka_Antivirus.Pages_Principales;
using System.Net.Http;
using Microsoft.Win32;
using System.Drawing;
using System.Threading;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Activities.Statements;
using Shell32;
using WUApiLib;
//using WMPLib;
using System.Data.SQLite;
using Draka_Antivirus.Pages_Principales.Pages_Security;
using System.Net.Security;
using System.Xml;
using System.Data;
using Org.BouncyCastle.Asn1.Crmf;
using System.Windows.Documents;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Security.Policy;
using System.Security.RightsManagement;
using Azure;
using System.Web;
using System.Windows.Shapes;
using Path = System.IO.Path;
using static System.Windows.Forms.LinkLabel;
using Draka_Antivirus.Pages_Principales.Pages_Stability;
using System.Net.Mail;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using Org.BouncyCastle.Crypto.Macs;
using System.Security.Cryptography;
using System.Windows.Media;
using Color = System.Drawing.Color;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Windows.Controls.Primitives;
//using Windows.ApplicationModel.VoiceCommands;

namespace Draka_Antivirus
{
    public partial class Index : Form
    {
        private object obj;
        int virus;
        int files;
        int index = 0;
        int ich = 0;
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
        string PathDataVirale = targetPath + "viraldatabase.txt";
        public static string name_db = "ScanDataBase.db";
        public static string sourceFile = targetPath + name_db;
        public static string OvrirRapport = targetPath + "OpenRepport.txt";
        public static string VirusBD = targetPath + "MD5Base.txt";
        Database db1 = new Database();
        ToolTip t1 = new ToolTip();
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };
        String obt = "Pause";
        string bbt = "Pause";
        string[] SelectAll = { "SelQ", "SelH", "SelM" };
        string[] typeScan;
        string choix = "";
        string chemin11 = "";
        string FichierHost = @"C:\Windows\System32\drivers\etc\hosts";

        //string FichierHost7 = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts"));                                
        string passeword = targetPath + "PWD.txt";
        string[] identifiants = { " ", " " };
        string Auto = "";
        string[] Partiel = {" ", " " };
        string path = targetPath + "Error_Log.txt";
        Boolean verif = true;
        Boolean verif1 = false;
        Boolean verif2 = false;
        string QAutoManuel = "Manuel";
        string pathAutoScan = targetPath + @"\Draka_Shiel\DrakaScanAuto.exe";
        //string pathAutoScan = targetPath + @"\shiel\sartre\ScanAutomatique.exe";
        string VerifAutoScan = targetPath + @"\Draka_Shiel\ScanAuto.txt";
        string ProgramScan = targetPath + @"\Draka_Shiel\Debug\ProgramScan.txt";
        string ProgramPartiel = targetPath + @"\Draka_Shiel\Debug\PragramPartiel.txt";
        string ProgrammerScan = targetPath + @"\Draka_Shiel\Debug\DrakaSchedScan.exe";
        string pathVideo = targetPath + @"\Videos\LigneRouge.MP4";
        //**************************************************************************
        //**************************************************************************
        //string path = targetPath + @"\Code\ConsoleApp2.exe";
        string pathVideo1 = targetPath + @"\Videos\LigneRouge.MP4";
        string pathVideo2 = targetPath + @"\Videos\ScreenJaune.MP4";
        string pathVideo3 = targetPath + @"\Videos\ScreenP.MP4";
        int virisScan = 0;  
        string programScan = "";  
        //**************************************************************************
        //**************************************************************************
        int l = 0;
        int ll = 0;
        public Index()
        {
            InitializeComponent();
            IntitialisationScan();

            //controlkey();

            obj = new ScanComplete(false);
            choix = "";
            guna2CustomCheckBox4.Checked = verif;
            guna2CustomCheckBox7.Checked = true;

            this.l = 0;
            this.ll = 0;

            ChargementScan();

            // création ou vérification de l'existance de la base de données
            try
            {
                sourceFile = db1.createDatabase(name_db);
                //db1.CreateTable(sourceFile, "Chiarita");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex);
            }

            // démarrer l'application scan programmé
            Process P2 = Process.Start(ProgrammerScan);

            if(verif == true)
            {
                Process P1 = Process.Start(pathAutoScan);
            }
            //MessageBox.Show("Etape 2");

            // Afficheage Home
            //virisScan = LectureScanAuto();
            //MessageBox.Show("Valeur virusScan : " + virisScan);
            start_Click();
            //MessageBox.Show("Démarrage Terminé");
        }

        private void HomeBtn_Click(object sender, EventArgs e)
        {
            start_Click();
            pagesView.SelectTab("home");
        }
        // démarrage application
        public void ChargementScan()
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals("DrakaScanAuto"))
                {
                    process.Kill();
                    //MessageBox.Show("Scan automatique arrêté ");
                }else if (process.ProcessName.Equals("DrakaSchedScan"))
                {
                    process.Kill(); 
                    //MessageBox.Show("Scan programmé arrêté ");
                }
                //MessageBox.Show($"{process.ProcessName}");
                /**/
            }
            // démarage de l'application
        }
        // démarer le MediaPlayer
        private void start_Click()
        {
            refreshData12();

            //Methode de choix d'ecran --> image écran guna2PictureBox1

            DateTime localTime = DateTime.Now;

            // -1- rechercher l'heure du dernier site dansgeureux enregistré dans l'objectionalwebsite

            int taille1 = guna2DataGridView19.Rows.Count;
            int temps = 0;
            virisScan = LectureScanAuto();

            if (taille1 > 0 || virisScan > 0)
            {
                string date = "";
                for (int i = 1; i <= taille1; i++)
                {
                    if (i == taille1)
                    {
                        // MessageBox.Show("je suis i = " + i);
                        date = (string)guna2DataGridView19[3, i - 1].Value;
                        //date = guna2DataGridView19.CurrentRow.Cells[3].Value.ToString();
                        // MessageBox.Show("Mon contenue est = " + date);
                    }
                    else
                    {
                        continue;
                    }
                }

                string date1 = localTime.ToString();

                // je fais la différence de date
                int[] heure = { 0, 0, 0, 0, 0, 0 };

                heure = ConversionDate(date, date1);
                int I = heure[0] * 24;
                temps = I;
            }
            else
            {
                // Aucun virus détecté l'écra passe au vert
                MediaPlayer.URL = (pathVideo3);
            }
            // -2- 3)	Dans le panneau de sécurité se trouve un site tagué dans le panneau des sites Web répréhensibles détecté dans les dernières 48 heures. Conditions :
            /* •	Si un virus est détecté, il devient rouge
             •	Si aucun virus détecté mais que la base de données n'est pas à jour, elle devient jaune OU ET un ou plusieurs sites ont été tagués par le panel Web OBJECTIONNEL pendant les 48 heures, il devient jaune. 
             •	Si aucun des cas précédents ne se produit, nous sommes verts.*/

            //temps = 38;

            if (virisScan > 0)  // virus détecté écran passe en rouge. soit 2jours
            {
                // virus détecté écran passe en rouge.
                MediaPlayer.URL = (pathVideo1);
                MediaPlayer.settings.setMode("loop", true);
                label1.Text = "your pc is very virused".ToUpper();
                label1.Location = new System.Drawing.Point(270, 350);
                label1.ForeColor = System.Drawing.Color.Red;
            }
            else if (temps <= 48 && virisScan <= 0 )
            {
                // Aucun virus détecté sous les derniers soixante douze heure, ecran passe au jaune
                MediaPlayer.URL = (pathVideo2);
                //MessageBox.Show("Valeur dans le fichier : " + LectureScanAuto());
                MediaPlayer.settings.setMode("loop", true);  // bouclé la lecture de windows Media
                label1.Text = "your pc is likely to be infected".ToUpper();
                label1.Location = new System.Drawing.Point(210, 350);
                label1.ForeColor = System.Drawing.Color.Orange;
            }
            else
            {
                // Aucun virus détecté l'écra passe au vert
                MediaPlayer.URL = (pathVideo3);
                MediaPlayer.settings.setMode("loop", true);
                label1.Text = "YOUR PC IS PROTECTED";
            }
        }
        
        public int LectureScanAuto()
        {
            string val = "";
            int k = 0;
            int k1 = 0;
            int t = TailleTable("Quarantaine");
            //MessageBox.Show("Taille = " + t);
            if(t <= 0)
            {
                try
                {
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(VerifAutoScan);
                    //Write a line of text
                    sw.WriteLine(0);
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                k = int.Parse("0");
            }
            else
            {
                //MessageBox.Show("Oui ");
                try
                {
                    //Créez une instance de StreamReader pour lire à partir d'un fichier
                    using (StreamReader sr = new StreamReader(VerifAutoScan))
                    {
                        string line;
                        // Lire les lignes du fichier jusqu'à la fin.
                        while ((line = sr.ReadLine()) != null)
                        {
                            val = line.Trim();
                        }
                    }
                    k = int.Parse(val);
                }
                catch (Exception e)
                {
                    //MessageBox.Show("Le fichier n'a pas pu être lu.");
                    MessageBox.Show(e.Message);
                }
            }
            if(k == 0)
            {
                k1 = t;
            }
            else
            {
                k1 = k;
            }
            //MessageBox.Show("K = " + k1);
            return k1;
        }

        // comment faire le calcul de date.
        public int[] ConversionDate(string str, string date)
        {
            var heure = "";
            //string date1 = date.ToString("dd MMMM yyyy  hh:mm:ss");
            var dt = str.Split(new string[] { "/", " ", ":" }, StringSplitOptions.None);
            var st = date.Split(new string[] { "/", " ", ":" }, StringSplitOptions.None);
            var tt = 9;

            int[] jour = {  int.Parse(dt[0]), int.Parse(dt[1]), int.Parse(dt[2]), int.Parse(dt[3]), int.Parse(dt[4]), int.Parse(dt[5]) };
            int[] jour1 = { int.Parse(st[0]), int.Parse(st[1]), int.Parse(st[2]), int.Parse(st[3]), int.Parse(st[4]), int.Parse(st[5]) };
            int[] jourFinal = { 0, 0, 0, 0, 0, 0 };
            
            if (jour1[0] < jour[0])
            {
                jourFinal[0] = ((jour1[0] + 30) - (jour[0]));
                //MessageBox.Show("jour si : " + jourFinal[0]);
            }
            else
            {
                jourFinal[0] = jour1[0] - jour[0];
                //MessageBox.Show("jour : " + jour1[0]);
            }
            // le mois
            if (jour1[1] < jour[1])
            {
                jourFinal[1] = (jour1[1] + 12) - jour[1];
                //MessageBox.Show("Mois si : " + jourFinal[1]);
            }else if(jour1[1] == jour[1])
            {
                jourFinal[1] = jour1[1];
            }
            else
            {
                jourFinal[1] = jour1[1] - jour[1];
                //MessageBox.Show("Mois : " + jourFinal[1]);
            }
            // heure
            if (jour1[3] < jour[3])
            {
                jourFinal[3] = (jour1[3] + 24) - jour[1];
            }
            else
            {
                jourFinal[3] = jour1[3] - jour[3];
            }

            // minute
            if (jour1[4] < jour[4])
            {
                jourFinal[4] = (jour1[4] + 60) - jour[1];
            }
            else
            {
                jourFinal[4] = jour1[4] - jour[4];
            }

            // seconde
            if (jour1[5] < jour[5])
            {
                jourFinal[5] = (jour1[5] + 60) - jour[1];
            }
            else
            {
                jourFinal[5] = jour1[5] - jour[5];
            }
            // Année
            jourFinal[2] = jour1[2];

            // recap

            if (jourFinal[5] >= 60)
            {
                jourFinal[5] = jourFinal[5] - 60;
                jourFinal[4] = jourFinal[4] + 1;
            }
            if (jourFinal[4] >= 60)
            {
                jourFinal[4] = jourFinal[4] - 60;
                jourFinal[3] = jourFinal[3] + 1;
            }
            return jourFinal;
        }
        // chargement scan programme

        public void IntitialisationScan()
        {
            guna2DateTimePicker1.Visible = false;
            guna2DateTimePicker4.Visible = false;
            guna2DateTimePicker2.Visible = false;
            guna2DateTimePicker3.Visible = false;
        }
        // guna2comboBox2_Click
        private void guna2comboBox1_Click(object sender, EventArgs e)
        {
            string selected = guna2comboBox1.GetItemText(this.guna2comboBox1.SelectedItem);
            
            if (selected == " ")
            {
                guna2DateTimePicker1.Visible = false;
                guna2DateTimePicker4.Visible = false;                
            }
            else if(selected == "Daily")
            {
                programScan = "Daily";
                guna2DateTimePicker1.Visible = false;
                guna2DateTimePicker4.Visible = true;
                guna2DateTimePicker4.Location = new Point(125, 58);
            }
            else if( selected == "Weekly")
            {
                programScan = "Weekly";
                guna2DateTimePicker1.Visible = true;
                guna2DateTimePicker4.Visible = true;
                guna2DateTimePicker4.Location = new Point(247, 58);
            }
        }
        private void guna2comboBox2_Click(object sender, EventArgs e)
        {
            string selected = guna2comboBox2.GetItemText(this.guna2comboBox2.SelectedItem);

            if (selected == " ")
            {
                guna2DateTimePicker2.Visible = false;
                guna2DateTimePicker3.Visible = false;
            }
            else if (selected == "Daily")
            {
                programScan = "Daily";
                guna2DateTimePicker2.Visible = false;
                guna2DateTimePicker3.Visible = true;
                guna2DateTimePicker3.Location = new Point(230, 69);
            }
            else if (selected == "Weekly")
            {
                programScan = "Weekly";
                guna2DateTimePicker2.Visible = true;
                guna2DateTimePicker3.Visible = true;
                guna2DateTimePicker3.Location = new Point(357, 69);
            }
        }
        private void ScanBtn_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("scan");
            //MessageBox.Show("Ici la page scan ");
        }

        private void PerformanceBtn_Click(object sender, EventArgs e)
        {
            // MessageBox.Show("Ici la page performance ");
            pagesView.SelectTab("performance");
            perfornancesview();
        }

        private void StabilityBtn_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Ici la page stability ");
            pagesView.SelectTab("stabilite");
            ListProgram();
        }

        private void MaintainBtn_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("maintenance");
            maintenanceViewCollection.SelectTab("fichier");
            tempoFiles();
        }

        private void HistoryBtn_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("historique");
            Historicsloading7();
        }

        private void QuarantBtn_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Ici la page Quarantine ");
            pagesView.SelectTab("quarant");
            QuarantaineHistoricsloading();
        }

        private void SecurityBtn_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("securite");
            loadFirewall();
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("activate");
        }

        private void Guna2ImageButton10_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Ici la page parametres ");
            pagesView.SelectTab("setting");
            // ici nous sommes dans le les programmes        

        }

        public void guna2CustomCheckBox4_Click(object sender, EventArgs e)
        {
            // Méthode de choix du scan automatique depuis les parametres de l'interface de l'av
            if (guna2CustomCheckBox4.Checked == true)
            {
                verif = true;
                Process P1 = Process.Start(pathAutoScan);
            }
            else
            {
                verif = false;

                foreach (var process in Process.GetProcesses())
                {
                    if (process.ProcessName.Equals("DrakaScanAuto"))
                    {
                        process.Kill();
                        MessageBox.Show("Scan automatique arrêté ");
                    }
                    //MessageBox.Show($"{process.ProcessName}");
                }

            }
        }
        private void Guna2ImageButton11_Click(object sender, EventArgs e)
        {
            pagesView.SelectTab("aide");
        }

        private void Guna2Button2_Click(object sender, EventArgs e)
        {
            // ici on traite le scan Total            
            scanViewCollection.SelectTab("complet");
        }

        private void Guna2Button3_Click(object sender, EventArgs e)
        {
            scanViewCollection.SelectTab("personnalise");
            // ici on traite le scan Personalisé
        }

        private void Guna2Button17_Click(object sender, EventArgs e)
        {
            performanceViewCollection.SelectTab("system");
            perfornancesview();
        }

        private void Guna2Button16_Click(object sender, EventArgs e)
        {
            guna2DataGridView3.Rows.Clear();
            performanceViewCollection.SelectTab("disque");
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            resizing();
            int count = 0;
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    double free, total;
                    free = d.TotalFreeSpace / Math.Pow(1024, 3);
                    total = d.TotalSize / Math.Pow(1024, 3);
                    guna2DataGridView3.Rows.Add(d.Name, d.DriveType.ToString(), d.VolumeLabel, d.DriveFormat, Math.Round(free, 2) + " GB", Math.Round(total, 2) + " GB");
                }

                count++;
            }
        }

        private void guna2CustomcheckBox4_Click(object sender, System.EventArgs e)
        {
            // The CheckBox control's Text property is changed each time the
            // control is clicked, indicating a checked or unchecked state.  
            if (guna2CustomCheckBox4.Checked)
            {
                //MessageBox.Show("je suis le premier choix");
            }
            else
            {
                //MessageBox.Show("je suis le deuxieme choix");
            }
        }
        private void Guna2Button19_Click(object sender, EventArgs e)
        {
            guna2DataGridView4.Rows.Clear();
            performanceViewCollection.SelectTab("memoire");
            //Part Information Memory
            resizing1();
            ManagementObjectSearcher physicalMemoryObject = new ManagementObjectSearcher("select * from Win32_PhysicalMemory ");
            int cpt = 0;
            foreach (ManagementObject obj in physicalMemoryObject.Get())
            {
                UInt64 capacity = 0;
                capacity += (UInt64)obj["Capacity"];
                guna2DataGridView4.Rows.Add(obj["BankLabel"].ToString(), obj["Speed"].ToString() + " MHz",
                    Math.Round(capacity / Math.Pow(1024, 3), 2) + " GB", obj["Name"].ToString(), obj["Manufacturer"].ToString());
                cpt++;
            }

            // End Part Information Memory 
        }

        private void Guna2Button18_Click(object sender, EventArgs e)
        {
            performanceViewCollection.SelectTab("reseau");
            Network_Load();
        }

        private void Guna2Button13_Click(object sender, EventArgs e)
        {
            /*stabiliteViewCollection.SelectTab("programs");*/
            ListProgram();
        }

        private void Guna2Button12_Click(object sender, EventArgs e)
        {
            stabiliteViewCollection.SelectTab("restaure");
            getRestorePoint();
        }

        private void Guna2Button11_Click(object sender, EventArgs e)
        {
            stabiliteViewCollection.SelectTab("crashing");
            LogDisplay();
        }

        private void Guna2Button10_Click(object sender, EventArgs e)
        {
            stabiliteViewCollection.SelectTab("alerts");
            EventLogAlert_Load();
        }

        private void Guna2Button25_Click(object sender, EventArgs e)
        {
            maintenanceViewCollection.SelectTab("corbeille");
            recyclebin();
        }

        private void Guna2Button24_Click(object sender, EventArgs e)
        {
            maintenanceViewCollection.SelectTab("fichier");
        }

        private void Guna2Button21_Click(object sender, EventArgs e)
        {
            maintenanceViewCollection.SelectTab("demarrage");
            startupPrograms();
        }

        private void Guna2Button15_Click(object sender, EventArgs e)
        {
            maintenanceViewCollection.SelectTab("update");
            displayWindowsUpdates();
        }

        private void Guna2Button44_Click(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("firewall");
        }

        private void Guna2Button43_Click(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("antivirus");
            antivirusDisplay();
        }

        private void Guna2Button37_Click(object sender, EventArgs e)
        {
            MessageBox.Show("search for connected wifi networks ");
            securityViewCollection.SelectTab("wifi");
            get_Wifi_passwords();
        }

        private void Guna2Button36_Click(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("password");
            getPassword();
        }

        private void Guna2Button45_Click(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("control");
            refreshData1();
        }

        private void Guna2Button46_Click(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("website");
        }

        private void Guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.pagesView.SelectTab("scan");
        }

        private void Guna2PictureBox1_Click(object sender, EventArgs e)
        {
            // ici on manipule l'image pictureBox1
        }

        private void Guna2PictureBox55_Click(object sender, EventArgs e)
        {
            // ici on manipule l'image Guna2PictureBox55
        }

        private void Guna2Button48_Click(object sender, EventArgs e)
        {
            Activation activation = new Activation();
            string key = guna2TextBox8.Text;
            //MessageBox.Show("je suis le bouton activation produit");
            /*activation.ActivateProduct(key);*/
        }

        private void guna2Button52_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != null && guna2TextBox4.Text != null && guna2TextBox5.Text != null && guna2TextBox6.Text != null && guna2TextBox7.Text != null)
            {
                if (guna2TextBox3.Text.ToLower() == guna2TextBox4.Text.ToLower())
                {
                    var datas = new Dictionary<string, string>
                    {
                        {"Email", guna2TextBox3.Text.ToLower() },
                        {"Name", guna2TextBox5.Text.ToLower() },
                        {"Phone", guna2TextBox6.Text.ToLower() },
                        {"Message", guna2TextBox7.Text.ToLower() }
                    };

                    var dats = new FormUrlEncodedContent(datas);
                    var url = "https://keygen.drakashield.com/clientdatas";
                    var client = new HttpClient();
                    var response = client.PostAsync(url, dats);
                    string result = response.ToString();
                    Console.WriteLine("http response => " + result);
                    MessageBox.Show("Thanks for your informations \n Admin will respond in 48h", " Client Complaint", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Emails not the same please try again", " Client Complaint", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Some information was not completed", " Client Complaint", MessageBoxButtons.OK);
            }

        }

        private string FreeMessage()
        {
            Activation activation = new Activation();
            string key = guna2TextBox8.Text;
            activation.ActivateProduct(key);

            return key;
        }

        private void Guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Guna2ProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }

        //******************************************************************************************************************************
        // Scan perso worker 
        //******************************************************************************************************************************
        int j;
        int count;
        int i;
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);

        private string BytesToHex(byte[] bytes)
        {
            // write each byte as two char hex output.
            return String.Concat(Array.ConvertAll(bytes, x => x.ToString("X2")));
        }

        private void backgroundWorkerPerso_DoWork_1(object sender, DoWorkEventArgs e)
        {
            //List<String> search = Directory.GetFiles(@custom_FolderPicker.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories).ToList();  //, System.IO.SearchOption.AllDirectories
            List<String> search = new List<String>();
            search.AddRange(fichiers(custom_FolderPicker.SelectedPath));
            custom_progressBar.Maximum = search.Count;
            Scancp perso = new Scancp();
            perso.filesCount = search.Count;

            int max = search.Count;
            int Tempmax = max;

            foreach (string item in search)
            {
                try
                {
                    files += 1;
                    string chemin = item;
                    Console.WriteLine(chemin);

                    if (backgroundWorkerPerso.CancellationPending)
                    {
                        search.Clear();
                        custom_dureeAnalyse.Text = "00h:00mm:00s";
                        e.Cancel = true;
                        pictureBox1.Visible = false;
                        break;

                    }
                    else if (obt == "Continue")
                    {
                        do
                        {
                            Thread.Sleep(500);
                        } while (obt == "Continue");
                    }
                    else
                    {
                        // read all bytes of file so we can send them to the MD5 hash algo
                        Byte[] allBytes = File.ReadAllBytes(item);
                        System.Security.Cryptography.HashAlgorithm md5Algo = null;
                        md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();
                        // compute the Hash (MD5) on the bytes we got from the file
                        byte[] hash = md5Algo.ComputeHash(allBytes);
                        Console.WriteLine(BytesToHex(hash));

                        perso.file = files;
                        var md5signatures = File.ReadAllLines("MD5Base.txt");
                        if (md5signatures.Contains(BytesToHex(hash)))
                        {
                            perso.statut = "Infected";
                            virus += 1;
                            perso.virus = virus;
                            string detection = BytesToHex(hash);
                            MoveItem(chemin, perso.statut, detection);
                        }
                        else
                        {
                            perso.statut = "Clean";
                        }

                        perso.item = item;
                        backgroundWorkerPerso.ReportProgress(0, perso);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    /*MessageBox.Show("Error : " + ex.ToString());*/
                }
            }
            //
        }

        private void backgroundWorkerPerso_ProgressChanged_1(object sender, ProgressChangedEventArgs e)
        {
            int scanend, intervale, time, days, hours, minutes, seconds;
            Scancp scan = (Scancp)e.UserState;
            custom_ProgressIndicator.Visible = false;
            custom_progressBar.Visible = true;
            complet_viewList.Visible = true;
            custom_progressBar.Maximum = scan.filesCount;
            custom_progressBar.Increment(1);
            custom_progressBar.Update();
            custom_detection.Text = scan.virus.ToString() + " virus detected";
            custom_totalObject.Text = "Files Scanned : " + scan.file.ToString();
            complet_viewList.Rows.Add(scan.item);
            label3.Text = scan.item;

            scanend = scan.filesCount;
            intervale = 1;
            time = (scanend * intervale) / 4;
            days = time / 86400;
            hours = time / 3600;
            minutes = time / 60 % 60;
            seconds = time % 60;
            custom_dureeAnalyse.Text = hours + "h :" + minutes + "mm :" + seconds + "s";
        }

        // Ajout des virus détectés dans le fichier virus
        public void Charger(int k)
        {
            //MessageBox.Show("kk : " + k);
            if (k == 0)
            {
                int heing = TailleTable("Quarantaine");
                //MessageBox.Show("rr : " + heing);
                if(heing <= 0)
                {
                    try
                    {
                        //Pass the filepath and filename to the StreamWriter Constructor
                        StreamWriter sw = new StreamWriter(VerifAutoScan);
                        //Write a line of text
                        sw.WriteLine(0);
                        sw.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                }
            }
            else
            {
                try
                {
                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(VerifAutoScan);
                    //Write a line of text
                    sw.WriteLine(k);
                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
        }
        private void backgroundWorkerPerso_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan stopped");
                PartialScan("Scan stopped", custom_dureeAnalyse.Text, custom_totalObject.Text, "Partial Scan", custom_detection.Text);
                Charger(virus);
                custom_folderPickerText.Text = "Chemin";
                custom_progressBar.Value = 0;
                custom_runScan.Visible = true;
                custom_cancelScan.Visible = false;
                custom_pauseScan.Visible = false;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Scan stopped");
                complet_viewList.ClearSelection();
                label3.Text = "";
                // dureeAnalyse = custom_dureeAnalyse.Text;
                //string totalObject = custom_totalObject.Text;
                Charger(virus);
                PartialScan("Scan stopped", custom_dureeAnalyse.Text, custom_totalObject.Text, "Partial Scan", custom_detection.Text);
                custom_folderPickerText.Text = "Chemin";
                custom_progressBar.Value = 0;
                custom_runScan.Visible = true;
                custom_cancelScan.Visible = false;
                custom_pauseScan.Visible = false;
            }
            else
            {
                MessageBox.Show("Scan Complete");
                custom_FolderPicker.SelectedPath.Trim();
                // ici nous allons prendre les informations de l'historique de scan                
                Charger(virus);
                PartialScan("scan done", custom_dureeAnalyse.Text, custom_totalObject.Text, "Partial Scan", custom_detection.Text);
                custom_folderPickerText.Text = "Chemin";
                custom_progressBar.Value = 0;
                custom_runScan.Visible = true;
                custom_cancelScan.Visible = false;
                custom_pauseScan.Visible = false;
            }
            complet_viewList.Rows.Clear();
            custom_runScan.Enabled = true;
            Program.scanRun = false;
            Program.isSp = false;
        }

        private void PartialScan(string Etat, string dureeAnalyse, string totalObj, string typescan, string totalVirus)
        {
            try
            {
                if (db1.CreateTable(sourceFile, "HistoryScan") == true)
                {
                    //MessageBox.Show("Table historique de scan bien créer");
                }
                else
                {
                    // MessageBox.Show("Table historique de scan Existe deja");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            string etat = Etat;
            DateTime date = DateTime.Now;
            string date1 = date.ToString("dddd, dd MMMM yyyy  hh:mm:ss");
            /*DateTime date = DateTime.Now;
            string date1 = date.ToString("yyyy:MM:dd  hh:mm:ss");*/
            string duree = dureeAnalyse;
            string totalAnalyser = totalObj;
            string virus = totalVirus;
            string scantype = typescan;// "Partial Scan";
            //string status = "Fait";
            this.ich = this.ich + 1;
            string sql = "insert into HistoryScan  (date,duree,TotalVirus,totalAna,typescan,Etat) values(";
            //sql = sql + "'" + ich + "', ";
            sql = sql + "'" + date1 + "', ";
            sql = sql + "'" + duree + "', ";
            sql = sql + "'" + virus + "', ";
            sql = sql + "'" + totalAnalyser + "', ";
            sql = sql + "'" + scantype + "', ";
            sql = sql + "'" + etat + "')";

            try
            {
                Boolean error = db1.insertData(sourceFile, sql);

                if (error == true)
                {
                    Console.WriteLine("Good Scan");
                    AutoClosingMessageBox.Show("Scan saved successfully");
                }
                else
                {
                    Console.WriteLine("Bad no complete scan");
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }

                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }
        }

        private void MoveItem(string directory, string etat, string hash)
        {
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";
            string subdir = @"Draka Antivirus\Draka Antivirus\Draka Antivirus\bin\Debug\Quarantaine\";

            try
            {
                Boolean create = db1.CreateTable(sourceFile, "Quarantaine");
                if (create == true)
                {
                    // MessageBox.Show("Table quantaine créer ");
                }
                else
                {
                    // AutoClosingMessageBox.Show("la table quarantaine Existe deja ");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex);
            }

            try
            {
                string file = Path.GetFileName(directory);
                root = root + file;

                if (FileSystem.FileExists(directory))
                {
                    if (Directory.Exists(subdir))
                    {
                        Directory.CreateDirectory(subdir);
                        string chemin = directory;
                        string fileName = file;
                        string NewDirectory = root;
                        DateTime date = DateTime.Now;
                        string date1 = date.ToString("dddd, dd MMMM yyyy  hh:mm:ss");
                        string taille = "";
                        string Etat = etat;
                        string editeur = "";
                        string action = "Mettre en quarantaine";
                        string detection = file + " -> " + hash;
                        index++;

                        string sql = "insert into Quarantaine (chemin,nomfichier,nouveldirection,date,taille,etat,editeur,action,detection) values(";
                        //sql = sql + "'" + index + "', ";
                        sql = sql + "'" + chemin + "', ";
                        sql = sql + "'" + fileName + "', ";
                        sql = sql + "'" + NewDirectory + "', ";
                        sql = sql + "'" + date1 + "', ";
                        sql = sql + "'" + taille + "', ";
                        sql = sql + "'" + Etat + "', ";
                        sql = sql + "'" + editeur + "', ";
                        sql = sql + "'" + action + "', ";
                        sql = sql + "'" + detection + "')";

                        try
                        {
                            if (QAutoManuel.Equals("Auto"))
                            {
                                AutoClosingMessageBox.Show("Virus detectees", " Draka Quarantaine ", 3000);
                                File.Delete(directory);
                            }
                            else
                            {
                                Boolean error = db1.insertData(sourceFile, sql);
                                if (error == true)
                                {
                                    AutoClosingMessageBox.Show("Viruses detected and saved in quarantine", " Draka Quarantaine ", 3000);
                                }
                                else
                                {
                                    Console.WriteLine("Virus detectees", " Draka Quarantaine ", 3000);
                                }

                                FileSystem.MoveFile(directory, root, true);

                            } //QAutoManuel
                        }
                        catch (Exception ex)
                        {
                            AutoClosingMessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        if (QAutoManuel.Equals("Auto"))
                        {
                            AutoClosingMessageBox.Show("Virus detectees", " Draka Quarantaine ", 3000);
                            File.Delete(directory);
                        }
                        else
                        {
                            Directory.CreateDirectory(subdir);
                            string chemin = directory;
                            string fileName = file;
                            string NewDirectory = root;
                            DateTime date = DateTime.Now;
                            string date1 = date.ToString("dddd, dd MMMM yyyy  hh:mm:ss");
                            string taille = "";
                            string Etat = etat;
                            string editeur = "";
                            string action = "Mettre en quarantaine";
                            string detection = file + " -> " + hash;
                            index++;

                            string sql = "insert into Quarantaine (chemin,nomfichier,nouveldirection,date,taille,etat,editeur,action,detection) values(";
                            //sql = sql + "'" + index + "', ";
                            sql = sql + "'" + chemin + "', ";
                            sql = sql + "'" + fileName + "', ";
                            sql = sql + "'" + NewDirectory + "', ";
                            sql = sql + "'" + date1 + "', ";
                            sql = sql + "'" + taille + "', ";
                            sql = sql + "'" + Etat + "', ";
                            sql = sql + "'" + editeur + "', ";
                            sql = sql + "'" + action + "', ";
                            sql = sql + "'" + detection + "')";
                            FileSystem.MoveFile(directory, root, true);
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Le fichier n'existe pas dans le repertoire : " + directory);
                    AutoClosingMessageBox.Show("The file does not exist in the directory: " + directory, " Draka Quarantaine ", 3000);
                }
            }
            catch (Exception ex)
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }

                else if (File.Exists(path))
                {
                    TextWriter tw = new StreamWriter(path, true);
                    tw.WriteLine(DateTime.Now.ToString() + " " + "Error_Message:" + ex);
                    tw.Close();
                }
            }
        }

        public void startbtnclick(string directory)
        {
            custom_folderPickerText.Text = @"" + directory;
            custom_FolderPicker.SelectedPath = @"" + directory;

            if (!backgroundWorkerPerso.IsBusy)
            {
                custom_pauseScan.Enabled = true;
                custom_cancelScan.Visible = true;
                custom_runScan.Visible = false;
                custom_progressBar.Value = 0;
                custom_progressBar.Update();
                custom_dureeAnalyse.Text = "00h:00mm:00s";
                custom_totalObject.Text = "0";
                custom_detection.Text = "0";

                backgroundWorkerPerso.RunWorkerAsync();

                if (label8.Text == "Initiailisations...")
                {
                    custom_progressBar.Visible = false;
                    complet_viewList.Visible = false;
                    custom_progressBar.Visible = true;
                }

                Program.scanRun = true;
                Program.isSp = true;
            }
        }

        private void Custom_folderPickerBtn_Click(object sender, EventArgs e)
        {
            custom_FolderPicker.ShowDialog();
            custom_folderPickerText.Text = custom_FolderPicker.SelectedPath;
            virus = 0;
            files = 0;
            //*****************************************
            custom_progressBar.Value = 0;
            complet_viewList.ClearSelection();
            label3.Text = "";
            custom_dureeAnalyse.Text = "00h:00mm:00s";
            complet_viewList.Visible = false;
            custom_progressBar.Visible = true;
            custom_cancelScan.Visible = false;
            custom_runScan.Visible = true;
            custom_pauseScan.Visible = false;
        }
        private void Custom_runScan_Click(object sender, EventArgs e)
        {

            Console.WriteLine("Welcome to the custom scan");
            Console.WriteLine(custom_folderPickerText.Text);

            if (custom_folderPickerText.Text != "" && custom_folderPickerText.Text != "Chemin")
            {
                if (!backgroundWorkerPerso.IsBusy)
                {
                    custom_ProgressIndicator.Visible = true;
                    custom_progressBar.Value = 0;
                    custom_progressBar.Update();
                    pictureBox1.Visible = true;

                    custom_cancelScan.Visible = true;
                    custom_pauseScan.Visible = true;
                    custom_runScan.Visible = false;
                    backgroundWorkerPerso.RunWorkerAsync();
                    AutoClosingMessageBox.Show("Loading files in progress");
                }
            }
            else
            {
                MessageBox.Show("Please select a good directory", " Custon Scan", MessageBoxButtons.OK);
            }

        }
        private void custom_cancelScan_Click(object sender, EventArgs e)
        {

            if (backgroundWorkerPerso.IsBusy)
            {
                if (MessageBox.Show("Do yo want to stop scan .... ", " Custon Scan ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    custom_pauseScan.Visible = true; custom_cancelScan.Visible = true; custom_runScan.Visible = true;
                    pictureBox1.Visible = false;
                    backgroundWorkerPerso.CancelAsync();
                    complet_viewList.Rows.Clear();
                }
            }
        }

        private void custom_pauseScan_Click(object sender, EventArgs e)
        {
            if (backgroundWorkerPerso.IsBusy)
            {
                if (obt == "Continue")
                {
                    obt = "Pause";
                    custom_pauseScan.Text = obt;
                    custom_pauseScan.Visible = true;
                    custom_cancelScan.Visible = true;
                    custom_runScan.Visible = false;
                }
                else
                {
                    obt = "Continue";
                    custom_pauseScan.Text = obt;
                    custom_pauseScan.Visible = true;
                    custom_cancelScan.Visible = false;
                    custom_runScan.Visible = false;
                }
            }

        }
        //******************************************************************************************************************************
        // End scan Perso
        //******************************************************************************************************************************


        //******************************************************************************************************************************
        // Full Scan Start
        //******************************************************************************************************************************    
        public List<String> fichiers(string dir)
        {
            List<String> f = new List<String>();
            List<string> dirs = Directory.GetDirectories(dir.ToString()).ToList();

            if (dirs.Count > 0)
            {
                foreach (string item in dirs)
                {
                    try
                    {
                        f.AddRange(fichiers(item));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
            }

            try
            {
                f.AddRange(Directory.GetFiles(dir.ToString(), "*.*", System.IO.SearchOption.TopDirectoryOnly).ToList());
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            return f;
        }

        private void backgroundWorkerComplet_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<String> search = new List<String>();
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                    {
                        /*search.AddRange(fichiers(d.RootDirectory.ToString() + "\\"));*/
                        search.AddRange(fichiers(@"C:\Users\"));
                    }
                }

                Scancp complet = new Scancp();
                /*complet.isInit = true;*/
                complet.filesCount = search.Count;

                /*backgroundScanPersonaliser.ReportProgress(0, complet);*/
                /*complet.isInit = false;*/

                foreach (String item in search)
                {
                    try
                    {
                        files += 1;
                        string chemin = item;
                        if (backgroundWorkerComplet.CancellationPending)
                        {
                            e.Cancel = true;
                            search.Clear();
                            //custom_dureeAnalyse.Text = "00h:00mm:00s";
                            pictureBox1.Visible = false;
                            break;
                        }
                        else if (bbt == "Continue")
                        {
                            do
                            {
                                Thread.Sleep(500);
                            } while (bbt == "Continue");
                        }
                        else
                        {
                            // read all bytes of file so we can send them to the MD5 hash algo
                            Byte[] allBytes = File.ReadAllBytes(item);
                            System.Security.Cryptography.HashAlgorithm md5Algo = null;
                            md5Algo = new System.Security.Cryptography.MD5CryptoServiceProvider();
                            // compute the Hash (MD5) on the bytes we got from the file
                            byte[] hash = md5Algo.ComputeHash(allBytes);
                            /*Console.WriteLine(BytesToHex(hash));*/

                            complet.file = files;
                            var md5signatures = File.ReadAllLines("MD5Base.txt");
                            if (md5signatures.Contains(BytesToHex(hash)))
                            {
                                //Console.WriteLine("Infected");
                                complet.statut = "Infected";
                                virus += 1;
                                complet.virus = virus;
                                string detection = BytesToHex(hash);
                                MoveItem(chemin, complet.statut, detection);
                            }
                            else
                            {
                                complet.statut = "Clean";
                            }

                            complet.item = item;
                            backgroundWorkerComplet.ReportProgress(0, complet);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                /*MessageBox.Show("Error : " + ex.ToString());*/
            }
        }

        private void backgroundWorkerComplet_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int scanend, intervale, time, days, hours, minutes, seconds;
            Scancp scan = (Scancp)e.UserState;

            complet_ProgressIndicator.Visible = false;
            complet_progressbar.Visible = true;
            complet_listView.Visible = true;
            complet_progressbar.Maximum = scan.filesCount;
            complet_progressbar.Increment(1);
            complet_progressbar.Update();
            label5.Text = scan.virus.ToString() + " virus detected";
            label6.Text = "Files Scanned : " + scan.file.ToString() + " ";
            CompletFileRoute.Text = scan.item;
            complet_listView.Rows.Add(scan.item);
            complet_repertoireText.Text = @"C:\Users\";

            scanend = scan.file;
            intervale = 1;
            time = (scanend * intervale) / 4;
            days = time / 86400;
            hours = time / 3600;
            minutes = time / 60 % 60;
            seconds = time % 60;
            label8.Text = hours + "h:" + minutes + "mm:" + seconds + "s";
        }

        private void backgroundWorkerComplet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Scan stopped");
                Charger(virus);
                PartialScan("Scan stopped", label8.Text, label6.Text, "Global Scan", label5.Text);

                complet_progressbar.Value = 0;
                complet_runScanBtn.Visible = true;
                complet_cancelScanBtn.Visible = false;
                complet_pauseScanBtn.Visible = false;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Scan stopped");
                Charger(virus);
                PartialScan("Scan stopped", label8.Text, label6.Text, "Global Scan", label5.Text);

                complet_progressbar.Value = 0;
                complet_runScanBtn.Visible = true;
                complet_cancelScanBtn.Visible = false;
                complet_pauseScanBtn.Visible = false;
            }
            else
            {
                MessageBox.Show("Scan Complete");
                Charger(virus);
                PartialScan("Scan done", label8.Text, label6.Text, "Global Scan", label5.Text);

                complet_progressbar.Value = 0;
                complet_runScanBtn.Visible = true;
                complet_cancelScanBtn.Visible = false;
                complet_pauseScanBtn.Visible = false;
            }
            complet_listView.Rows.Clear();
            //complet_viewList.ClearSelection();
            Program.scanRun = false;
            Program.isSp = false;
        }
        private void Guna2Button5_Click(object sender, EventArgs e)
        {
            //  complet_pauseScanBtn
            if (backgroundWorkerComplet.IsBusy)
            {
                if (bbt == "Continue")
                {
                    bbt = "Pause";
                    complet_pauseScanBtn.Text = bbt;
                    complet_pauseScanBtn.Visible = true;
                    complet_cancelScanBtn.Visible = true;
                    complet_runScanBtn.Visible = false;

                }
                else
                {
                    bbt = "Continue";
                    complet_pauseScanBtn.Text = bbt;
                    complet_pauseScanBtn.Visible = true;
                    complet_cancelScanBtn.Visible = false;
                    complet_runScanBtn.Visible = false;
                }
            }
        }

        private void Guna2Button4_Click(object sender, EventArgs e)
        {
            // run button
            complet_repertoireText.Text = @"C:\Users\";
            //CompletfolderBrowserDialog.SelectedPath = @"C:\Users\";

            if (!backgroundWorkerComplet.IsBusy)
            {
                complet_pauseScanBtn.Visible = true;
                complet_cancelScanBtn.Visible = true;
                complet_runScanBtn.Visible = false;
                complet_progressbar.Value = 0;
                complet_progressbar.Update();
                complet_ProgressIndicator.Visible = true;
                pictureBox1.Visible = true;
                label8.Text = "00h:00mm:00s";
                label6.Text = "0";
                label5.Text = "0";
                backgroundWorkerComplet.RunWorkerAsync();

                if (CompletFileRoute.Text == "Initiailisations...")
                {
                    complet_progressbar.Visible = false;
                    complet_listView.Visible = false;
                    complet_progressbar.Visible = true;
                }

                Program.scanRun = true;
                Program.isSp = true;
                AutoClosingMessageBox.Show("Loading files in progress");
            }
        }
        private void Guna2Button6_Click(object sender, EventArgs e)   // complet_cancelScanBtn
        {
            if (backgroundWorkerComplet.IsBusy)
            {
                if (MessageBox.Show("Do yo want to stop scan .... ", " Custon Scan ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    complet_pauseScanBtn.Visible = true;
                    complet_cancelScanBtn.Visible = true;
                    complet_runScanBtn.Visible = true;
                    backgroundWorkerComplet.CancelAsync();
                    complet_listView.Rows.Clear();
                    pictureBox1.Visible = false;
                }
            }
        }

        //******************************************************************************************************************************
        // End Full Scan Start
        //******************************************************************************************************************************


        //******************************************************************************************************************************
        // Performances
        //******************************************************************************************************************************
        private void perfornancesview()
        {
            // Part Infos System 
            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            string pathName = (string)registryKey.GetValue("productName");
            labelOperatingSystem.Text = pathName;


            label38.Text = Environment.MachineName;

            label37.Text = Environment.UserName;


            ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");

            foreach (ManagementObject obj in myProcessorObject.Get())
            {
                label34.Text = obj["Name"].ToString();
                label25.Text = obj["NumberOfCores"].ToString();
                label23.Text = obj["MaxClockSpeed"].ToString() + "MHz";

            }

            ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

            foreach (ManagementObject obj in myVideoObject.Get())
            {
                label22.Text = obj["Name"].ToString();

            }

            ManagementObjectSearcher mos =
            new ManagementObjectSearcher(@"root\CIMV2", @"SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject mo in mos.Get())
            {
                label36.Text = mo["Workgroup"].ToString();
            }


            ManagementObjectSearcher operatinSystem = new ManagementObjectSearcher("select * from Win32_OperatingSystem ");
            double res;
            foreach (ManagementObject obj in operatinSystem.Get())
            {
                label24.Text = obj["OSArchitecture"].ToString();

                res = Convert.ToDouble(obj["TotalVisibleMemorySize"]);
                double fres = Math.Round((res / (1024 * 1024)), 2);
                label35.Text = fres.ToString() + "Gb";
            }


            var cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            for (int i = 0; i < 5; i++)
            {
                label21.Text = cpuUsage.NextValue() + "%";
            }

            var search = new ManagementObjectSearcher("SELECT * FROM Win32_baseboard");
            var mobos = search.Get();

            foreach (var m in mobos)
            {
                var serial = m["SerialNumber"]; // ProcessorID if you use Win32_CPU
                label18.Text = serial.ToString();
            }
            // End Part infos System
        }  // Performances view and button
        public void resizing()
        {
            float totalColumnWidth = 0;

            // Get the sum of all column tags
            for (int i = 0; i < guna2DataGridView3.Columns.Count; i++)
                totalColumnWidth += Convert.ToInt32(guna2DataGridView3.Columns[i].Tag);

            // Calculate the percentage of space each column should 
            // occupy in reference to the other columns and then set the 
            // width of the column to that percentage of the visible space.
            for (int i = 0; i < guna2DataGridView3.Columns.Count; i++)
            {
                float colPercentage = (Convert.ToInt32(guna2DataGridView3.Columns[i].Tag) / totalColumnWidth);
                guna2DataGridView3.Columns[i].Width = (int)(colPercentage * guna2DataGridView3.ClientRectangle.Width);
            }
        }  // Performances elements
        public void resizing1()
        {
            float totalColumnWidth = 0;

            // Get the sum of all column tags
            for (int i = 0; i < guna2DataGridView4.Columns.Count; i++)
                totalColumnWidth += Convert.ToInt32(guna2DataGridView4.Columns[i].Tag);

            // Calculate the percentage of space each column should 
            // occupy in reference to the other columns and then set the 
            // width of the column to that percentage of the visible space.
            for (int i = 0; i < guna2DataGridView4.Columns.Count; i++)
            {
                float colPercentage = (Convert.ToInt32(guna2DataGridView4.Columns[i].Tag) / totalColumnWidth);
                guna2DataGridView4.Columns[i].Width = (int)(colPercentage * guna2DataGridView4.ClientRectangle.Width);
            }
        } // disk elements

        // Network Methode
        private void Network_Load()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            Console.WriteLine(adapters.Length);

            foreach (NetworkInterface adapter in adapters)
            {
                if (adapter.GetIPStatistics().BytesSent > 0)
                {
                    IPAddress ipv4 = adapter.GetIPProperties().UnicastAddresses[1].Address;
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    DateTime today = DateTime.Now;
                    label74.Text = adapter.Name;
                    label58.Text = adapter.Description;
                    label57.Text = adapter.Id;
                    label56.Text = adapter.GetIPProperties().GetIPv4Properties().IsDhcpEnabled.ToString();
                    label55.Text = adapter.GetIPProperties().UnicastAddresses[1].Address.ToString();
                    label54.Text = adapter.GetIPProperties().UnicastAddresses[1].IPv4Mask.ToString();
                    label53.Text = today.ToString("yyyy:MM:dd hh:mm:ss");
                    label52.Text = today.ToString("yyyy:MM:dd hh:mm:ss");
                    label51.Text = getwayaddress();
                    label50.Text = DHCPAddress();
                    label28.Text = GetDnsAdress();
                    label29.Text = "";
                    label30.Text = ipv4.IsIPv6LinkLocal.ToString();
                    label31.Text = getipv6address();
                    label32.Text = "";
                    label33.Text = "";
                }
                else
                {
                    continue;
                }

            }

            foreach (NetworkInterface ni in adapters)
            {
                if (ni.GetIPStatistics().BytesSent > 0)
                {
                    label26.Text = "Bytes Send: " + (ni.GetIPv4Statistics().BytesSent / 1024) + " Mbps" + "      Bytes Receive: " + (ni.GetIPv4Statistics().BytesReceived / 1024) + " Mbps";
                }

            }
        }
        public string DHCPAddress()
        {
            string address1 = "";
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {

                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                IPAddressCollection addresses = adapterProperties.DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress address in addresses)
                    {
                        address1 = address.ToString();
                    }
                }
            }

            return address1;
        }
        public string getwayaddress()
        {
            string address2 = "";
            foreach (NetworkInterface f in NetworkInterface.GetAllNetworkInterfaces())
                if (f.OperationalStatus == OperationalStatus.Up)
                    foreach (GatewayIPAddressInformation d in f.GetIPProperties().GatewayAddresses)

                        if (d.Address.ToString().Trim().Length > 2)//ignore ::
                            address2 = d.Address.ToString();
            return address2;
        }
        private string GetDnsAdress()
        {

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();
                    IPAddressCollection dnsAddresses = ipProperties.DnsAddresses;

                    foreach (IPAddress dnsAdress in dnsAddresses)
                    {
                        return dnsAdress.ToString();
                    }
                }
            }

            throw new InvalidOperationException("Unable to find DNS Address");
        }
        public string getipv6address()
        {
            string address3 = "";
            string strHostName = System.Net.Dns.GetHostName(); ;
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            Console.WriteLine(addr[addr.Length - 1].ToString());
            if (addr[0].AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
            {
                address3 = addr[0].ToString(); //ipv6
            }

            return address3;
        }
        public string NetWorkSpeed()
        {
            string speed = "";
            PerformanceCounterCategory pcg = new PerformanceCounterCategory("Network Interface");
            string instance = pcg.GetInstanceNames()[0];
            PerformanceCounter pcsent = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
            PerformanceCounter pcreceived = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);

            speed = "Bytes Sent: " + pcsent.NextValue() / 1024 + "   Bytes Received: " + pcreceived.NextValue() / 1024;

            return speed;
        }

        //******************************************************************************************************************************
        // End Performances
        //******************************************************************************************************************************


        //******************************************************************************************************************************
        // Start stability
        //******************************************************************************************************************************
        public const string SYSTEM_RESTORE = "SystemRestore";
        public const string CREATE_SYSTEM_RESTORE_POINT = "CreateRestorePoint";
        public const string SYSTEM_RESTORE_POINT_DESCRIPTION = "Description";
        public const string SYSTEM_RESTORE_POINT_TYPE = "RestorePointType";
        public const string SYSTEM_RESTORE_EVENTTYPE = "EventType";

        // List of programs
        private void ListProgram()
        {
            /*singleTable.Rows = View.Details;*/


            string displayName;
            string editName;
            string version;
            RegistryKey key;

            // search in: CurrentUser
            key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            /*new DataGridViewColumn(new String[] { "Name", "Publisher", "Version" })*/
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                editName = subkey.GetValue("Publisher") as string;
                version = subkey.GetValue("DisplayVersion") as string;
                //Console.WriteLine(displayName);
                singleTable.Rows.Add(new string[] { displayName, editName, version });
                /*new ListViewItem(new string[] { displayName, editName, version })*/
            }

            // search in: LocalMachine_32
            key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
            foreach (String keyName in key.GetSubKeyNames())
            {
                RegistryKey subkey = key.OpenSubKey(keyName);
                displayName = subkey.GetValue("DisplayName") as string;
                editName = subkey.GetValue("Publisher") as string;
                version = subkey.GetValue("DisplayVersion") as string;
                //Console.WriteLine(displayName);
                singleTable.Rows.Add(new string[] { displayName, editName, version });
                /*new ListViewItem(new string[] { displayName, editName, version })*/
            }
        }

        // Restoration point
        public void getRestorePoint()
        {
            try
            {
                guna2DataGridView5.Visible = false;
                guna2WinProgressIndicator1.Visible = true;
                label79.Visible = true;
                label78.Visible = false;
                bool iscreated1 = false;
                if (iscreated1 != CreateRestorePoint())
                {
                    AutoClosingMessageBox.Show("Point de restauration creer le : " + DateTime.Now.ToString(), "Point de Restauration", 10000);
                    getRestorePoint();
                    guna2DataGridView5.Visible = true;
                    guna2WinProgressIndicator1.Visible = false;
                    label78.Visible = false;
                    label79.Visible = false;
                }
                else
                {
                    guna2DataGridView5.Visible = false;
                    guna2WinProgressIndicator1.Visible = true;
                    label79.Visible = true;
                    label78.Visible = false;
                    if (CreateRestorePoint() == true)
                    {
                        AutoClosingMessageBox.Show("Point de restauration creer le : " + DateTime.Now.ToString(), "Point de Restauration", 10000);
                        getRestorePoint();
                        guna2DataGridView5.Visible = true;
                        guna2WinProgressIndicator1.Visible = false;
                        label78.Visible = false;
                        label79.Visible = false;
                    }
                    else
                    {
                        guna2DataGridView5.Visible = false;
                        guna2WinProgressIndicator1.Visible = false;
                        label78.Visible = false;
                        label79.Visible = true;
                        label79.Text = "Impossible de creer un point de restoration";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ManagementClass c = new ManagementClass("root/default:SystemRestore");
            Console.WriteLine(c.ToString());
            try
            {
                foreach (ManagementObject o in c.GetInstances())
                {
                    guna2DataGridView5.Rows.Add(o["Description"].ToString(), o["RestorePointType"].ToString(), o["EventType"].ToString(), o["SequenceNumber"].ToString(), getDate(o["CreationTime"].ToString()));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
        public String getDate(String d)
        {
            String date = d.Remove(4);
            d = d.Remove(0, 4);
            date += "/" + d.Remove(2);
            d = d.Remove(0, 2);
            date += "/" + d.Remove(2);
            d = d.Remove(0, 2);
            date += " " + d.Remove(2);
            d = d.Remove(0, 2);
            date += ":" + d.Remove(2);
            d = d.Remove(0, 2);
            date += ":" + d.Remove(2);
            return date;
        }
        //Create restore point
        public static bool CreateRestorePoint()
        {
            bool isCreated = true;
            try
            {
                ManagementObject classInstance =
                   new ManagementObject(@"root\DEFAULT",
                   "SystemRestore.ReplaceKeyPropery='ReplaceKeyPropertyValue'",
                   null);

                // Obtain in-parameters for the method
                ManagementBaseObject inParams =
                    classInstance.GetMethodParameters("CreateRestorePoint");

                // Add the input parameters.
                inParams["Description"] = "My test restore point";
                inParams["EventType"] = 100;
                inParams["RestorePointType"] = 0;

                // Execute the method and obtain the return values.
                ManagementBaseObject outParams =
                    classInstance.InvokeMethod("CreateRestorePoint", inParams, null);

                // List outParams
                Console.WriteLine("Out parameters:");
                Console.WriteLine("Return Value restore point => " + outParams["ReturnValue"]);

                isCreated = (outParams == null) ? !isCreated : isCreated;
            }
            catch (ManagementException me)
            {
                //handle the error.
                Console.WriteLine("Return Value restore point2 => " + me.Message);
                isCreated = !isCreated;

            }
            return isCreated;
        }
        // Delete restore Point
        [DllImport("Srclient.dll")]
        public static extern int SRRemoveRestorePoint(int index);
        public static int DeleteRestorePoint(int SequenceNumber)
        {
            int intReturn = SRRemoveRestorePoint(SequenceNumber);
            return intReturn;
        }

        //Frequently crash program
        public void LogDisplay()
        {
            guna2DataGridView6.Rows.Clear();
            EventLog[] eventLogs = EventLog.GetEventLogs();
            try
            {
                foreach (EventLog e in eventLogs)
                {
                    Int64 sizeKB = 0;
                    string logfile = "";
                    string currentsize = "";
                    string creationdatetime = "";
                    Console.WriteLine();
                    Console.WriteLine("{0}:", e.LogDisplayName.ToString());
                    Console.WriteLine("  Log name = \t\t {0}", e.Log.ToString());

                    Console.WriteLine("  Number of event log entries = {0}", e.Entries.Count.ToString());

                    // Determine if there is an event log file for this event log.
                    RegistryKey regEventLog = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Services\\EventLog\\" + e.Log.ToString());
                    if (regEventLog != null)
                    {
                        Object temp = regEventLog.GetValue("File");
                        if (temp != null)
                        {
                            Console.WriteLine("  Log file path = \t {0}", temp.ToString());
                            logfile = temp.ToString();
                            FileInfo file = new FileInfo(temp.ToString());
                            creationdatetime = file.CreationTime.ToString();

                            // Get the current size of the event log file.
                            if (file.Exists)
                            {
                                sizeKB = file.Length / 1024;
                                if ((file.Length % 1024) != 0)
                                {
                                    sizeKB++;
                                }
                                Console.WriteLine("  Current size = \t {0} kilobytes", sizeKB.ToString());
                                currentsize = sizeKB.ToString();
                            }
                        }
                        else
                        {
                            Console.WriteLine("  Log file path = \t <not set>");
                        }
                    }

                    // Display the maximum size and overflow settings.

                    sizeKB = e.MaximumKilobytes;
                    Console.WriteLine("  Maximum size = \t {0} kilobytes", sizeKB.ToString());
                    Console.WriteLine("  Overflow setting = \t {0}", e.OverflowAction.ToString());

                    /*switch (e.OverflowAction)
                    {
                        case OverflowAction.OverwriteOlder:
                            Console.WriteLine("\t Entries are retained a minimum of {0} days.",
                                e.MinimumRetentionDays);
                            break;
                        case OverflowAction.DoNotOverwrite:
                            Console.WriteLine("\t Older entries are not overwritten.");
                            break;
                        case OverflowAction.OverwriteAsNeeded:
                            Console.WriteLine("\t If number of entries equals max size limit, a new event log entry overwrites the oldest entry.");
                            break;
                        default:
                            break;
                    }*/

                    guna2DataGridView6.Rows.Add(
                    e.Log.ToString(), e.Entries.Count.ToString(),
                    logfile,
                    sizeKB.ToString() + " KB",
                    e.OverflowAction.ToString(),
                    creationdatetime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Crasher log => " + ex.Message);
            }
        }

        // Event Log
        /*public void EventLogAlert()
        {
            GetSystemLogs();
            GetSecurityLogs();
            GetApplicationLogs();
        }

        public void GetSystemLogs()
        {
            if (!EventLog.Exists("System", Environment.MachineName))
            {
                labelSystemCount.Text = "0";
                return;
            }
            else
            {
                EventLog myLog = new EventLog("System", Environment.MachineName);
                labelSystemCount.Text = myLog.Entries.Count.ToString();
            }
        }
        public void GetSecurityLogs()
        {
            try
            {
                if (!EventLog.Exists("Security", Environment.MachineName))
                {
                    labelSecurityCount.Text = "0";
                    return;
                }
                else
                {
                    EventLog myLog = new EventLog("Security", Environment.MachineName);
                    labelSecurityCount.Text = myLog.Entries.Count.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void GetApplicationLogs()
        {
            if (!EventLog.Exists("Application", Environment.MachineName))
            {
                labelAppCount.Text = "0";
                return;
            }
            else
            {
                EventLog myLog = new EventLog("Application", Environment.MachineName);
                labelAppCount.Text = myLog.Entries.Count.ToString();
            }
        }*/

        private void EventLogAlert_Load()
        {
            /*guna2DataGridView8.FullRowSelect = true;*/
            guna2DataGridView8.Rows.Clear();
            EventLog[] remoteEventLogs;

            remoteEventLogs = EventLog.GetEventLogs();

            Console.WriteLine("Number of logs on computer: " + remoteEventLogs.Length);
            try
            {
                foreach (EventLog log in remoteEventLogs)
                {
                    Console.WriteLine("Log: " + log.Log);
                    guna2DataGridView8.Rows.Add(log.Log.ToString(), log.OverflowAction.ToString(), log.Entries.Count.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Crasher log => " + ex.Message);
            }
        }

        //******************************************************************************************************************************
        // End stability
        //******************************************************************************************************************************

        //******************************************************************************************************************************
        // Start maintain 
        //******************************************************************************************************************************
        // reclycle bin display
        private void recyclebin()
        {
            // Recycle bin content Begin 
            Shell shell = new Shell();
            Folder folder = shell.NameSpace(0x000a);

            resizing3();

            double taille = 0;
            try
            {
                foreach (FolderItem2 item in folder.Items())
                {
                    Console.WriteLine("FileName:{0}     Size:{1} bytes ", item.Name, CalculateSize(item));
                    guna2DataGridView9.Rows.Add(item.Name.ToString(), CalculateSize(item).ToString() + " Mo", item.Path, item.ModifyDate.ToString());

                    taille = taille + CalculateSize(item);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Recycle Bin Error => " + e.Message);
            }
            /*label1.Text = " Taille du contenu de la Corbeille : " + taille + " Mo";*/


            Marshal.FinalReleaseComObject(shell);
            Console.ReadLine();

            // End Recycle bin content
        }
        // temporaly files
        private void tempoFiles()
        {
            guna2DataGridView10.Rows.Clear();
            // Temporary Files Begin
            DirectoryInfo Dir = new DirectoryInfo(System.IO.Path.GetTempPath());
            string tempFile = Path.GetTempPath();

            FileInfo[] Files = Dir.GetFiles();
            int i = 0;
            int taill = 0;
            resizing4();
            try
            {
                foreach (FileInfo file in Files)
                {
                    //Console.WriteLine((int)file.Length);
                    /* taill = taill + (int)file.Length;*/
                    taill = (int)file.Length;
                    guna2DataGridView10.Rows.Add(file.Name, taill + " Bytes", i.ToString());
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Tempo Files Error => " + e.Message);
            }
            // End Temporary File 
        }
        //delete temporaly files 
        private void deleteTempoFiles()
        {
            DirectoryInfo Dir1 = new DirectoryInfo(System.IO.Path.GetTempPath());
            string tempFile1 = Path.GetTempPath();

            FileInfo[] Files1 = Dir1.GetFiles();
            int i1 = 0;
            resizing4();
            try
            {
                foreach (FileInfo file1 in Files1)
                {
                    File.Delete(file1.ToString());
                    i1++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Tempo Files Deteleted Error message => " + e.Message);
            }
            guna2DataGridView10.Rows.Clear();
        }
        private void guna2Button22_Click(object sender, EventArgs e)
        {
            deleteTempoFiles();
        }
        // Startup Programs
        private void startupPrograms()
        {
            // Startup Software Begin
            ManagementClass mangnmt = new ManagementClass("Win32_StartupCommand");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            resizing5();
            foreach (ManagementObject strt in mcol)
            {
                guna2DataGridView11.Rows.Add(strt["Name"].ToString(), strt["Location"].ToString(), strt["Command"].ToString(), strt["User"].ToString());
                //Console.WriteLine("User: " + strt["User"].ToString());
            }

            // End Startup Software 
        }
        // Display windows Update
        private void displayWindowsUpdates()
        {
            guna2WinProgressIndicator2.Visible = false;
            /*label84.Visible = true;
            label90.Visible = true;*/
            label91.Visible = true;
            label92.Visible = true;

            // Windows Update Begin

            var AUC = new AutomaticUpdatesClass();
            try
            {
                bool isWUEnabled = AUC.ServiceEnabled;

                if (isWUEnabled)
                {
                    //Console.WriteLine("Windows Update is Enabled");
                    label90.Text = "Enabled";
                }
                else
                {
                    //Console.WriteLine("Windows Update is Disabled");
                    label90.Text = "Disabled";
                }

                DateTime? lastInstallationSuccessDateUtc = null;
                if (AUC.Results.LastInstallationSuccessDate is DateTime)
                {
                    lastInstallationSuccessDateUtc = new DateTime(((DateTime)AUC.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);
                    label92.Text = lastInstallationSuccessDateUtc.ToString();
                }

                DateTime? lastSearchSuccessDateUtc = null;
                if (AUC.Results.LastSearchSuccessDate is DateTime)
                {
                    lastSearchSuccessDateUtc = new DateTime(((DateTime)AUC.Results.LastSearchSuccessDate).Ticks, DateTimeKind.Utc);
                    label91.Text = lastSearchSuccessDateUtc.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // End Windows update 
        }
        // get windows Update
        private void GetSystemUpdate()
        {
            guna2DataGridView12.Rows.Clear();
            label89.Visible = false;
            label84.Visible = false;
            label90.Visible = false;
            label91.Visible = false;
            label92.Visible = false;

            /*
             var auc = new AutomaticUpdatesClass();

             DateTime? lastInstallationSuccessDateUtc = null;
             if (auc.Results.LastInstallationSuccessDate is DateTime)
                lastInstallationSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);

             DateTime? lastSearchSuccessDateUtc = null;
             if (auc.Results.LastSearchSuccessDate is DateTime)
             lastSearchSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastSearchSuccessDate).Ticks, DateTimeKind.Utc);
             */


            // Now, we need to search the Microsoft updates
            //MessageBox.Show("Etape 1");
            try
            {
                UpdateSessionClass uSession = new UpdateSessionClass();
                IUpdateSearcher uSearcher = uSession.CreateUpdateSearcher();
                ISearchResult uResult = uSearcher.Search("IsInstalled=0 and Type = 'Software'");
                if (uResult.Updates.Count < 1)
                {
                    var auc = new AutomaticUpdatesClass();

                    DateTime? lastInstallationSuccessDateUtc = null;
                    if (auc.Results.LastInstallationSuccessDate is DateTime)
                        lastInstallationSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);
                        label92.Text = lastInstallationSuccessDateUtc.ToString();
                    DateTime? lastSearchSuccessDateUtc = null;
                    if (auc.Results.LastSearchSuccessDate is DateTime)
                        lastSearchSuccessDateUtc = new DateTime(((DateTime)auc.Results.LastSearchSuccessDate).Ticks, DateTimeKind.Utc);
                    label91.Text = lastSearchSuccessDateUtc.ToString();
                    AutoClosingMessageBox.Show("No new updates available");
                }
                else
                {
                    foreach (IUpdate update in uResult.Updates)
                    {
                        Console.WriteLine("Update Message => " + update.Title);
                        MessageBox.Show("Update Message => " + update.Title);
                    }
                    //MessageBox.Show("Etape 2");
                    //Now we have to create an UpdateDownloader class object to download the updates
                    try
                    {
                        UpdateDownloader downloader = uSession.CreateUpdateDownloader();
                        downloader.Updates = uResult.Updates;
                        downloader.Download();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("No Update available ");
                    }

                    //Now, as we have completed the download of the required updates, we have to install the
                    //downloaded updates by checking its IsDownloaded property is true.
                    int i = 0;
                    int size = 0;
                    UpdateCollection updatesToInstall = new UpdateCollection();
                    foreach (IUpdate update in uResult.Updates)
                    {
                        MessageBox.Show("No : " + update);
                        try
                        {
                            if (update.IsDownloaded)
                                updatesToInstall.Add(update);
                            i++;
                            size = size + (int)update.MaxDownloadSize;
                            if (guna2DataGridView12.Rows.Count == 0)
                            {
                                guna2DataGridView12.Visible = false;
                                guna2WinProgressIndicator2.Visible = false;
                            }
                            else
                            {
                                guna2DataGridView12.Rows.Add(update.Title, update.MaxDownloadSize.ToString() + " KB");
                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Exception 10 : " + ex.Message);
                            Console.WriteLine($"{ex.Message}");
                        }

                    }
                    //MessageBox.Show("Etape 3");
                    label90.Visible = true;
                    label91.Visible = true;
                    label92.Visible = true;

                    label89.Text = i.ToString();
                    label84.Text = size.ToString() + " KB";
                    //Now assign the update collection of to be installed updates to the installer class
                    IUpdateInstaller installer = uSession.CreateUpdateInstaller();
                    installer.Updates = updatesToInstall;
                    //MessageBox.Show("Etape 4 : "+ installer);
                    //Now, call the install method of the IUpdateInstaller object and result of which will be stored in
                    //IInstallationResult object installationRes.
                    IInstallationResult installationRes = installer.Install();

                    //Now as all updates will be installed sequentally by the WUAPI
                    int k = 0, l = 0;
                    for (int i1 = 0; i1 < updatesToInstall.Count; i1++)
                    {
                        if (installationRes.GetUpdateResult(i1).HResult == 0)
                        {
                            Console.WriteLine("Installed : " + updatesToInstall[i1].Title);
                            k++;
                        }
                        else
                        {
                            Console.WriteLine("Failed : " + updatesToInstall[i1].Title);
                            l++;
                        }
                    }

                    AutoClosingMessageBox.Show("Draka Antivirus a Intaller : " + k + " Mise a jour et : " + l + " Ne sont pas installer", "Update", 10000);
                    label90.Visible = true;
                    label91.Visible = true;
                    label92.Visible = true;

                    var AUC = new AutomaticUpdatesClass();
                    bool isWUEnabled = AUC.ServiceEnabled;
                    if (isWUEnabled)
                    {
                        //Console.WriteLine("Windows Update is Enabled");
                        label90.Text = "Enabled";
                    }
                    else
                    {
                        //Console.WriteLine("Windows Update is Disabled");
                        label90.Text = "Disabled";
                    }

                    DateTime? lastInstallationSuccessDateUtc = null;
                    if (AUC.Results.LastInstallationSuccessDate is DateTime)
                    {
                        lastInstallationSuccessDateUtc = new DateTime(((DateTime)AUC.Results.LastInstallationSuccessDate).Ticks, DateTimeKind.Utc);
                        label92.Text = lastInstallationSuccessDateUtc.ToString();
                    }

                    DateTime? lastSearchSuccessDateUtc = null;
                    if (AUC.Results.LastSearchSuccessDate is DateTime)
                    {
                        lastSearchSuccessDateUtc = new DateTime(((DateTime)AUC.Results.LastSearchSuccessDate).Ticks, DateTimeKind.Utc);
                        label91.Text = lastSearchSuccessDateUtc.ToString();
                    }
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                AutoClosingMessageBox.Show("Error while installing Updates");
                //Console.WriteLine(installationRes.);
            }
        }
        private void guna2Button23_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Update en cours ");
            GetSystemUpdate();
        }
        // update Btn
        private void updateBtn_Click(object sender, EventArgs e)
        {
            // Attribution des droits de securiter et paramettrage de la variable sur le site officiel de Draka 
            // C'est ici qu'il va verifier si le fichier xml du site fait mention d'une nouvelle version disponible

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            string downloadurl = "";
            Version newversion = null;
            string xmlUrl = "https://www.drakashield.com/fr/";
            XmlTextReader reader = null;

            string messages = "";

            try
            {   // lecture du fichier xml
                //MessageBox.Show("uuuuuu");
                reader = new XmlTextReader(xmlUrl);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "draka-Antivirus"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            elementName = reader.Name;
                        }
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newversion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        downloadurl = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            // recuperation de la version systeme
            Version applicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            /* comparaison des version
             * si la version systeme est inferieur a la verison en ligne on la telecharge si non on mentionne que celle disponible est a jour
             */
            try
            {
                if (applicationVersion.CompareTo(newversion) < 0)
                {
                    DialogResult result;
                    Console.WriteLine("version" + newversion.Major + "." + newversion.Minor + "." + newversion.Build + " de Draka est maintenant disponible. Voulez-vous le telecharger ? Y/N  ");
                    result = MessageBox.Show("version" + newversion.Major + "." + newversion.Minor + "." + newversion.Build + " de Draka est maintenant disponible. Voulez-vous le telecharger ? Y/N ", "Draka Mise a jour", MessageBoxButtons.YesNo);
                    messages = "Une nouvelle version est disponible!";
                    /*string userInput = Console.ReadLine();*/

                    if (result == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(downloadurl);
                    }
                    else
                    {
                        MessageBox.Show("Pour avoir les nouvelle fonctionnaliter et une meilleur protection \n Vous devez mettre a jour l'appilcation ", "Draka Mise a jour", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    Console.WriteLine("L'Application est a jour ");
                    MessageBox.Show("The app is up to date ", "Draka Mise a jour", MessageBoxButtons.OK);
                    messages = "Version a jour !";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message);
            }
        }
        // Others Methods
        public void resizing4()
        {
            float totalColumnWidth = 0;

            // Get the sum of all column tags
            for (int i = 0; i < guna2DataGridView10.Columns.Count; i++)
                totalColumnWidth += Convert.ToInt32(guna2DataGridView10.Columns[i].Tag);

            // Calculate the percentage of space each column should 
            // occupy in reference to the other columns and then set the 
            // width of the column to that percentage of the visible space.
            for (int i = 0; i < guna2DataGridView10.Columns.Count; i++)
            {
                float colPercentage = (Convert.ToInt32(guna2DataGridView10.Columns[i].Tag) / totalColumnWidth);
                guna2DataGridView10.Columns[i].Width = (int)(colPercentage * guna2DataGridView10.ClientRectangle.Width);
            }
        }
        public void resizing5()
        {
            float totalColumnWidth = 0;

            // Get the sum of all column tags
            for (int i = 0; i < guna2DataGridView11.Columns.Count; i++)
                totalColumnWidth += Convert.ToInt32(guna2DataGridView11.Columns[i].Tag);

            // Calculate the percentage of space each column should 
            // occupy in reference to the other columns and then set the 
            // width of the column to that percentage of the visible space.
            for (int i = 0; i < guna2DataGridView11.Columns.Count; i++)
            {
                float colPercentage = (Convert.ToInt32(guna2DataGridView11.Columns[i].Tag) / totalColumnWidth);
                guna2DataGridView11.Columns[i].Width = (int)(colPercentage * guna2DataGridView11.ClientRectangle.Width);
            }
        }
        public static double CalculateSize(FolderItem obj)
        {

            if (!obj.IsFolder)
                return (double)Math.Round(obj.Size / Math.Pow(1024, 2), 2);

            Folder recycleBin = (Folder)obj.GetFolder;

            double size = 0;


            foreach (FolderItem2 item in recycleBin.Items())
                size += CalculateSize(item);

            return size;

        }
        enum RecycleFlags : int
        {
            SHRB_NOCONFIRMATION = 0x00000001, // Don't ask for confirmation
            SHRB_NOPROGRESSUI = 0x00000001, // Don't show progress
            SHRB_NOSOUND = 0x00000004 // Don't make sound when the action is executed
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlags dwFlags);
        public void resizing3()
        {
            float totalColumnWidth = 0;

            // Get the sum of all column tags
            for (int i = 0; i < guna2DataGridView9.Columns.Count; i++)
                totalColumnWidth += Convert.ToInt32(guna2DataGridView9.Columns[i].Tag);

            // Calculate the percentage of space each column should 
            // occupy in reference to the other columns and then set the 
            // width of the column to that percentage of the visible space.
            for (int i = 0; i < guna2DataGridView9.Columns.Count; i++)
            {
                float colPercentage = (Convert.ToInt32(guna2DataGridView9.Columns[i].Tag) / totalColumnWidth);
                guna2DataGridView9.Columns[i].Width = (int)(colPercentage * guna2DataGridView9.ClientRectangle.Width);
            }
        }
        //******************************************************************************************************************************
        // End maintain
        //******************************************************************************************************************************

        //******************************************************************************************************************************
        // Start Histroy
        //******************************************************************************************************************************

        private void Historicsloading7()
        {
            choix = "";
            Int32 selectedCellCount1 = 0;
            try
            {
                if (File.Exists(sourceFile))
                {
                    Boolean verif = db1.CreateTable(sourceFile, "HistoryScan");
                    if (verif == true)
                    {
                        //MessageBox.Show("La tale HistoryScan creer ");
                        pictureBox1.Visible = true;
                        label93.Visible = true;
                        guna2DataGridView13.Visible = false;
                        selectedCellCount1 = 0;
                    }
                    else
                    {
                        //MessageBox.Show("La tale HistoryScan Existe deja ");
                        selectedCellCount1 = guna2DataGridView13.Rows.Count;
                        //MessageBox.Show("Nous sommes dans la tale quarantaine : "+selectedCellCount1);

                        string sql2 = "";
                        int i1 = 1;
                        // acces à la as la valeur max de la table History
                        //string sql = "SELECT MAX(Id) FROM HistoryScan; ";

                        int LastRowID = NewTailleTableau("HistoryScan");

                        pictureBox1.Visible = true;
                        label93.Visible = true;
                        guna2DataGridView13.Visible = false;

                        guna2DataGridView13.Rows.Clear();

                        try
                        {
                            if (LastRowID > 0)
                            {
                                pictureBox1.Visible = false;
                                label93.Visible = false;
                                guna2DataGridView13.Rows.Clear();

                                while (i1 <= LastRowID)
                                {
                                    sql2 = "select * from HistoryScan where Id= " + i1 + ";";
                                    Object[] dburl = db1.searchData(sourceFile, sql2);

                                    if (dburl != null)
                                    {
                                        guna2DataGridView13.Rows.Add(dburl[1].ToString(), dburl[3].ToString(), dburl[5].ToString());
                                    }

                                    i1++;
                                    guna2DataGridView13.Visible = true;
                                }
                            }
                            else
                            {
                                pictureBox1.Visible = true;
                                label93.Visible = true;
                                guna2DataGridView13.Visible = false;
                            }

                        }
                        catch (Exception ex)
                        {
                            if (!File.Exists(path))
                            {
                                File.Create(path);
                                TextWriter tw = new StreamWriter(path, true);
                                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql2 + " " + "Error_Message:" + ex);
                                tw.Close();
                            }

                            else if (File.Exists(path))
                            {
                                TextWriter tw = new StreamWriter(path, true);
                                tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql2 + " " + "Error_Message:" + ex);
                                tw.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AutoClosingMessageBox.Show("Oups");
                MessageBox.Show("An error has occurred, restart the application");
                guna2DataGridView13.Visible = false;
            }

        }

        //Open report
        private void guna2Button29_Click(object sender, EventArgs e)  // ici c'est open report
        {
            Int32 selectedCellCount = guna2DataGridView13.GetCellCount(DataGridViewElementStates.Selected);

            //MessageBox.Show("Je suis open repport " + selectedCellCount);
            int i1 = 0;
            if (selectedCellCount > 0)
            {
                if (guna2DataGridView13.CurrentRow != null)
                {
                    int indice = guna2DataGridView13.CurrentCell.RowIndex;
                    i1 = indice + 1;
                    string date = (string)guna2DataGridView13[0, indice].Value;
                    //string sql8 = "select date, duree, TotalVirus, totalAna, TypeScan, Etat from HistoryScan where Id= " + date + ";";
                    string sql8 = "select * from HistoryScan where Id= " + i1 + ";";

                    try
                    {
                        Object[] dburl = db1.searchData(sourceFile, sql8);
                        if (dburl != null)
                        {
                            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
                            string pathName = (string)registryKey.GetValue("productName");
                            string strr = "";
                            string strrr = "";
                            ManagementObjectSearcher mos = new ManagementObjectSearcher(@"root\CIMV2", @"SELECT * FROM Win32_ComputerSystem");
                            foreach (ManagementObject mo in mos.Get())
                            {
                                strr = mo["Workgroup"].ToString();
                            }

                            ManagementObjectSearcher myProcessorObject = new ManagementObjectSearcher("select * from Win32_Processor");

                            foreach (ManagementObject obj in myProcessorObject.Get())
                            {
                                strrr = obj["Name"].ToString();
                            }

                            string[] str = new string[]
                            {
                                "Product Name       : Draka Shiel Antivirus",
                                "Analysis status    : " + dburl[6],
                                "Date of analysis   : " + dburl[1],
                                "Type of analysis   : " + dburl[5],
                                "Analysis Duration  : " + dburl[2],
                                "Scanned objects    : " + dburl[4],
                                "Viruses detected   : " + dburl[3],
                                "Object excluded    : 00",
                                "Auto send          : No",
                                "Operating system   : " + pathName,
                                "Processor          : " + strrr,
                                "Domain information : " + strr,
                                "CUID               : "
                            };

                            using (StreamWriter sw = new StreamWriter(OvrirRapport))
                            {
                                foreach (string s in str)
                                {
                                    sw.WriteLine(s);
                                }
                            }

                        }
                        else
                        {
                            AutoClosingMessageBox.Show("Please check that the item chosen is correct", "Details Historique", 10000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    //MessageBox.Show("Ligne séléctionnée : " +indice.ToString() +" Date : "+date);
                }

            }

            try
            {
                Process proc = new Process();
                proc.StartInfo = new ProcessStartInfo(OvrirRapport);
                proc.Start();
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        // supprimer la ligne historique
        private void guna2Button30_Click(object sender, EventArgs e)
        {
            if (choix.Equals("SelH"))
            {
                // ici on supprime tous le contenue de la table
                string sql7 = "DROP TABLE HistoryScan";

                DialogResult result;
                result = MessageBox.Show("Do you really want to delete everything ?", "HistoryScan", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (db1.deleteData(sourceFile, sql7) == true)
                    {
                        AutoClosingMessageBox.Show("Successful deletion ");
                    }
                    else
                    {
                        MessageBox.Show("Impossible to delete all this content ");
                    }
                    Historicsloading7();
                }

            }
            else if (choix.Equals(""))
            {
                int i1 = 0;

                Int32 selectedCellCount1 = guna2DataGridView13.Rows.Count;
                int taille = NewTailleTableau("HistoryScan");
                DialogResult result;
                result = MessageBox.Show("Do you want to restore this file?", "Quarantaine", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (taille > 0)
                    {
                        guna2DataGridView13.Visible = true;

                        if (guna2DataGridView13.CurrentRow != null)
                        {
                            int indice = guna2DataGridView13.CurrentCell.RowIndex;
                            i1 = indice + 1;
                            string date = (string)guna2DataGridView13[0, indice].Value;

                            string sql1 = "DELETE FROM HistoryScan where Id = '" + i1 + "';";

                            if (db1.deleteData(sourceFile, sql1) == true)
                            {
                                //MessageBox.Show("Suppression réussite = "+i1);
                                guna2DataGridView13.Rows.RemoveAt(indice);
                            }
                            else
                            {
                                MessageBox.Show("Delete Failed ");
                            }

                            // mise à jour de la table HistoryScan.
                            Historicsloading7();
                        }
                        else
                        {
                            MessageBox.Show("select a line to delete ");
                        }
                    }
                    else
                    {
                        guna2DataGridView13.Visible = false;
                        pictureBox1.Visible = true;
                        label93.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("Deletion abandoned ");
                }
                    
            }

        }

        // Select all sur la table historique
        private void guna2Button31_Click(object sender, EventArgs e)
        {
            this.l = this.l + 1;
            int reste = this.l % 2;
            choix = "";
            if (reste == 0)
            {
                foreach (DataGridViewRow item in guna2DataGridView13.Rows)
                {
                    item.Selected = false;
                    item.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            else
            {
                foreach (DataGridViewRow item in guna2DataGridView13.Rows)
                {
                    item.Selected = true;
                    item.DefaultCellStyle.ForeColor = Color.Aqua;
                }
                choix = SelectAll[1];
                // MessageBox.Show("je suis Selecte all dans l'historique : " + choix);
            }


        }
        //******************************************************************************************************************************
        // End Histroy
        //******************************************************************************************************************************

        //******************************************************************************************************************************
        // Start Quarantine
        //******************************************************************************************************************************
        private void QuarantaineHistoricsloading()
        {
            //int indice = guna2DataGridView14.CurrentCell.RowIndex;

            if (db1.CreateTable(sourceFile, "Quarantaine") == true)
            {
                AutoClosingMessageBox.Show("quarantine table reset.");
                pictureBox2.Visible = true;
                label94.Visible = true;
                guna2DataGridView14.Visible = false;
            }
            else
            {
                pictureBox2.Visible = true;
                label94.Visible = true;
                guna2DataGridView14.Visible = false;

                string sql2 = "";
                int i1 = 1;

                try
                {
                    int LastRowID = NewTailleTableau("Quarantaine");

                    if (LastRowID > 0)
                    {
                        pictureBox2.Visible = false;
                        label94.Visible = false;
                        guna2DataGridView14.Rows.Clear();

                        while (i1 <= LastRowID)
                        {
                            sql2 = "select chemin, detection, date from Quarantaine where Id='" + i1 + "';";
                            Object[] dburl = db1.searchData(sourceFile, sql2);
                            if (dburl != null)
                            {
                                guna2DataGridView14.Rows.Add(dburl[0].ToString(), dburl[2].ToString(), dburl[1].ToString());
                            }

                            i1++;
                            guna2DataGridView14.Visible = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // Restore Element
        private void guna2Button34_Click(object sender, EventArgs e)
        {
            if (choix.Equals(SelectAll[0]))
            {
                int taille = NewTailleTableau("Quarantaine");

                for (int i = 1; i <= taille; i++)
                {
                    string sql = "SELECT nouveldirection FROM Quarantaine where Id = '" + i + "';";
                    int i1 = (i - 1);
                    try
                    {
                        Object[] dburl = db1.searchData(sourceFile, sql);
                        string chemin = (string)guna2DataGridView14[0, i1].Value;
                        if (File.Exists(dburl[0].ToString()))
                        {
                            FileSystem.MoveFile(dburl[0].ToString(), chemin, true);
                            //File.Move(dburl[0].ToString(), chemin);
                            AutoClosingMessageBox.Show("Successful restoration ");
                            // guna2DataGridView14.Rows.RemoveAt(indice);

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur == " + ex.Message);
                    }
                }
                // on drop la table Quarantaine
                string sql7 = "DROP TABLE Quarantaine";
                if (db1.deleteData(sourceFile, sql7) == true)
                {
                    AutoClosingMessageBox.Show("Quarantine Table Fully restored");
                    pictureBox2.Visible = true;
                    label94.Visible = true;
                    guna2DataGridView14.Visible = false;
                }
            }
            else
            {
                Int32 selectedCellCount1 = guna2DataGridView14.Rows.Count;
                //MessageBox.Show("Gooood = " + selectedCellCount1);
                int i1 = 0;
                if (guna2DataGridView14.CurrentRow != null)
                {
                    int indice = guna2DataGridView14.CurrentRow.Index;
                    string chemin = (string)guna2DataGridView14[0, indice].Value;
                    i1 = indice + 1;

                    string sql = "SELECT nouveldirection FROM Quarantaine where Id = '" + i1 + "';";
                    string sql1 = "DELETE FROM Quarantaine where Id = '" + i1 + "';";

                    Object[] dburl = db1.searchData(sourceFile, sql);

                    DialogResult result;
                    result = MessageBox.Show("Do you want to restore this file?", "Quarantaine", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        if (db1.deleteData(sourceFile, sql1) == true)
                        {
                            try
                            {
                                if (dburl != null)
                                {
                                    if (File.Exists(dburl[0].ToString()))
                                    {
                                        FileSystem.MoveFile(dburl[0].ToString(), chemin, true);
                                        //File.Move(dburl[0].ToString(), chemin);
                                        // mise à jour de la table Quarantaine.
                                        int x = i1 - 1;
                                        string sql8 = "UPDATE Quarantaine SET Id = (Id-1) where Id > '" + x + "';";

                                        if (db1.deleteData(sourceFile, sql8) == true)
                                        {
                                            AutoClosingMessageBox.Show("Quarantine table successfully updated");
                                            QuarantaineHistoricsloading();
                                        }
                                    }
                                    else
                                    {
                                        AutoClosingMessageBox.Show("File does not exist in quarantine ");
                                        guna2DataGridView14.Rows.RemoveAt(indice);
                                    }
                                }
                                else
                                {
                                    string sql7 = "DROP TABLE Quarantaine";
                                    if (db1.deleteData(sourceFile, sql7) == true)
                                    {
                                        //MessageBox.Show("Table Quarantaine creer");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Exception  = " + ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No line selected ");
                        }
                    }
                    else
                    {
                        MessageBox.Show("You have chosen not to validate the quarantine of the file");
                    }

                }
                else
                {
                    MessageBox.Show("No line selected ");
                }
            }
        }
        // delete element quarantaine
        private void guna2Button33_Click(object sender, EventArgs e)
        {
            string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

            if (choix.Equals(SelectAll[0]))
            {
                //string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

                string sql7 = "DROP TABLE Quarantaine";

                DialogResult result;
                result = MessageBox.Show("Do you all want to delete these files?", "Quarantaine", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (db1.deleteData(sourceFile, sql7) == true)
                    {
                        AutoClosingMessageBox.Show("Delete success");
                        string[] files = Directory.GetFiles(root);
                        foreach (string file in files)
                        {
                            File.Delete(file);
                        }
                        pictureBox2.Visible = true;
                        label94.Visible = true;
                        guna2DataGridView14.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please choose a line to delete");
                    QuarantaineHistoricsloading();
                }
            }
            else if (choix.Equals(""))
            {
                Int32 selectedCellCount1 = guna2DataGridView14.Rows.Count;

                if (selectedCellCount1 > 0)
                {
                    int taille = NewTailleTableau("Quarantaine");

                    MessageBox.Show("Taille : " + taille);
                    int i1 = 0;

                    if (guna2DataGridView14.CurrentRow != null)
                    {
                        int indice = guna2DataGridView14.CurrentRow.Index;
                        string chemin = (string)guna2DataGridView14[0, indice].Value;
                        i1 = indice + 1;

                        string sql = "SELECT nouveldirection FROM Quarantaine where Id = '" + i1 + "';";
                        string sql1 = "DELETE FROM Quarantaine where Id = '" + i1 + "';";

                        DialogResult result;
                        result = MessageBox.Show("Do you want to delete this file?", "Quarantaine", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                Object[] dburl = db1.searchData(sourceFile, sql);
                                if (db1.deleteData(sourceFile, sql1) == true)
                                {
                                    if (dburl != null)
                                    {
                                        if (File.Exists(dburl[0].ToString()))
                                        {
                                            File.Delete(dburl[0].ToString());
                                            AutoClosingMessageBox.Show("Delete success ");
                                            guna2DataGridView14.Rows.RemoveAt(indice);

                                            // mise à jour de la table Quarantaine.
                                            int x = i1 - 1;
                                            string sql8 = "UPDATE Quarantaine SET Id = (Id-1) where Id > '" + x + "';";
                                            if (db1.deleteData(sourceFile, sql8) == true)
                                            {
                                                AutoClosingMessageBox.Show("Quarantine table successfully updated");
                                                QuarantaineHistoricsloading();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Update failed on quarantine table");
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                        }
                        else
                        {
                            MessageBox.Show("You have chosen not to continue deleting");
                            QuarantaineHistoricsloading();
                        }

                    }
                    else
                    {
                        MessageBox.Show("No line selected");
                    }
                }
                else
                {
                    pictureBox2.Visible = true;
                    label94.Visible = true;
                    guna2DataGridView14.Visible = false;
                }
            }
        }

        // Report As safe 
        private void guna2Button35_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Jes suis le bouton report as safe");

            if (db1.CreateTable(sourceFile, "Chiarita") == true)
            {
                //MessageBox.Show("la table chiarita a été créer ");
                pictureBox2.Visible = true;
                label94.Visible = true;
                guna2DataGridView14.Visible = false;
            }
            else
            {
                if (choix.Equals(SelectAll[0]))
                {
                    //MessageBox.Show("Je suis Choix : " + choix);

                    // report as safe all
                    int taille = NewTailleTableau("Quarantaine");
                    //MessageBox.Show("ici on doit tous report as safe = " + taille);

                    DateTime Date = DateTime.Now;
                    Date.ToString("dd/MM/yyyy HH:mm:ss");

                    Int32 select11 = guna2DataGridView14.Rows.Count;

                    List<string> SignatureV = new List<string>();
                    List<string> CheminV = new List<string>();
                    List<string> Autre = new List<string>();
                    List<string> AutreBD = new List<string>();
                    List<string> DetectionV = new List<string>();

                    int i1 = 0;
                    var md5signatures = File.ReadAllLines(VirusBD);

                    DialogResult result;
                    result = MessageBox.Show("Do you want to Restore this file ?", "Quarantaine", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // récupérons les signatures du dataGrid
                        foreach (string item in md5signatures)
                        {
                            Autre.Add(item.Trim());
                        }
                        //MessageBox.Show("Etape1 terminé");

                        for (int i = 0; i < taille; i++)
                        {
                            /*if (i > 4)
                            {
                                break;
                            }*/
                            string detection = (string)guna2DataGridView14[2, i].Value;
                            // MessageBox.Show("oooooooooo : " + detection.ToString());
                            var dt = detection.Split(new string[] { "->" }, StringSplitOptions.None);
                            string chemin = (string)guna2DataGridView14[0, i].Value;

                            SignatureV.Add(dt[1].Trim());
                            //MessageBox.Show("Virus : " + dt[1]);
                            Autre.Add(dt[1].Trim());
                            CheminV.Add(chemin);
                            DetectionV.Add(detection);

                        }
                        //MessageBox.Show("Etape2 terminé");
                        foreach (string signature in SignatureV)
                        {
                            if (Autre.Contains(signature))
                            {
                                //MessageBox.Show("En fin je réussi ");
                                Autre.Remove(signature);
                                //continue;
                            }

                        }
                        File.WriteAllLines(VirusBD, Autre);

                        string root = @"C:\Program Files(x86)\Default Company Name\Setup1\Quarantaine\";

                        string sql7 = "DROP TABLE Quarantaine";

                        if (db1.deleteData(sourceFile, sql7) == true)
                        {
                            int i = 0;
                            foreach (string item in CheminV)
                            {
                                string sql10 = "INSERT INTO Chiarita (chemin, date, nomfichier)values(";
                                sql10 = sql10 + "'" + item + "', ";
                                sql10 = sql10 + "'" + Date + "', ";
                                sql10 = sql10 + "'" + DetectionV[i] + "')";

                                i++;

                                try
                                {
                                    Boolean error = db1.insertData(sourceFile, sql10);

                                    if (error == true)
                                    {
                                        Console.WriteLine("Good Scan");
                                        AutoClosingMessageBox.Show("Repport as safe are succesfully ");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Bad no complete scan");
                                        //MessageBox.Show("Repport as safe are bad ");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AutoClosingMessageBox.Show("EXCEPTION : " + ex.Message);
                                }
                            }

                            AutoClosingMessageBox.Show("Report As Safe success");
                            string[] files = Directory.GetFiles(root);
                            foreach (string file in files)
                            {
                                File.Delete(file);
                            }
                        }
                        pictureBox2.Visible = true;
                        label94.Visible = true;
                        guna2DataGridView14.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("You don't want to delete all files from Quarantine.");
                    }

                }
                else
                {
                    int i1 = 0;
                    // MessageBox.Show("Je suis le contraire de choix ");

                    if (guna2DataGridView14.CurrentRow != null)
                    {
                        int indice = guna2DataGridView14.CurrentRow.Index;
                        string detection = (string)guna2DataGridView14[2, indice].Value;
                        string chemin = (string)guna2DataGridView14[0, indice].Value;
                        i1 = indice + 1;

                        var chaine1 = detection.Split(new string[] { "->" }, StringSplitOptions.None);
                        List<string> AutreBD = new List<string>();

                        var md5signatures = File.ReadAllLines(VirusBD);
                        string signature = chaine1[1].Trim();

                        //recherche de la signature virale

                        foreach (string s in md5signatures)
                        {
                            AutreBD.Add(s);
                        }
                        //MessageBox.Show("Etape1 réussite ");

                        if (AutreBD.Contains(signature))
                        {
                            AutreBD.Remove(signature);
                            //MessageBox.Show("voilà c'est bon ");
                        }

                        File.WriteAllLines(VirusBD, AutreBD);
                        // Process P3 = Process.Start(VirusBD);

                        // 1- je restore le fichier nouvelle direction

                        string sql5 = "SELECT nouveldirection FROM Quarantaine where Id = '" + i1 + "';";

                        DialogResult result;
                        result = MessageBox.Show("Do you want to Restore this file ?", "Quarantaine", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                Object[] dburl5 = db1.searchData(sourceFile, sql5);

                                DateTime Date = DateTime.Now;
                                Date.ToString("dd/MM/yyyy HH:mm:ss");

                                if (dburl5 != null)
                                {
                                    if (File.Exists(dburl5[0].ToString()))
                                    {
                                        string sql10 = "INSERT INTO Chiarita (chemin, date, nomfichier)values(";
                                        sql10 = sql10 + "'" + chemin + "', ";
                                        sql10 = sql10 + "'" + Date + "', ";
                                        sql10 = sql10 + "'" + detection + "')";
                                        // );
                                        try
                                        {
                                            Boolean error = db1.insertData(sourceFile, sql10);
                                            if (error == true)
                                            {
                                                Console.WriteLine("Good Scan");
                                                AutoClosingMessageBox.Show("Repport as safe are succesfully ");
                                                FileSystem.MoveFile(dburl5[0].ToString(), chemin, true);
                                                //File.Move(dburl5[0].ToString(), chemin);
                                                AutoClosingMessageBox.Show("Files As Safe ");

                                                guna2DataGridView14.Rows.RemoveAt(indice);
                                                // supprimer le fichier de la table quarantaine
                                                string sql1 = "DELETE FROM Quarantaine where Id = '" + i1 + "';";

                                                if (db1.deleteData(sourceFile, sql1) == true)
                                                {

                                                }
                                                // 3- je met la table quarantaine à jour
                                                QuarantaineHistoricsloading();

                                            }
                                            //Boolean error = db1.insertData(sourceFile, sql10);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    string sql7 = "DROP TABLE Quarantaine";
                                    if (db1.deleteData(sourceFile, sql7) == true)
                                    {
                                        //MessageBox.Show("Table Quarantaine creer");
                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            MessageBox.Show("You do not assert trust in this file");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Line selected ");
                    }
                }
            }

        }

        // Management Safe

        private void guna2Button53_Click(object sender, EventArgs e)
        {
            // ici nous somme dans le management Safe
            //MessageBox.Show("Le bouton Management safe a vu le jour");
            pagesView.SelectTab("ManageSafe");
            LoadManageSafe();
        }
        private void guna2Button54_Click(object sender, EventArgs e)
        {
            // ici nous somme dans le management Safe
            MessageBox.Show("Le bouton Management safe a vu le jour");
            //guna2DataGridView14.Visible = true;
            //pagesView.SelectTab("ManageSafe");
            MessageBox.Show("Le bouton Management safe a vu le jour");
        }
        // cette méthode retourne la taille du taleau
        private int TailleTable(string nomtable)
        {
            string sql4 = "SELECT MAX(Id) FROM '" + nomtable + "';";
            int dex = 0;
            try
            {
                Object[] dburl1 = db1.searchData(sourceFile, sql4);
                if (dburl1 != null)
                {
                    dex = Int32.Parse(dburl1[0].ToString());
                    //MessageBox.Show("La taille du tableau est  = " + dex);
                }
                else
                {
                    //MessageBox.Show("Echec Mise à jour d'Index ");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception : " + ex.Message);
            }
            return dex;
        }
        // Button supprimer du Management Safe
        private void guna2Button55_Click(object sender, EventArgs e)
        {
            if (choix.Equals(SelectAll[2]))
            {
                // ici on va faire une action globale
                int taille = NewTailleTableau("Chiarita");
                List<string> AutreBD = new List<string>();
                DialogResult result;
                result = MessageBox.Show("Do you want to delete this file ?", "ManageSafe", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    for (int i = 1; i <= taille; i++)
                    {
                        string sql = "SELECT nomfichier FROM Chiarita where Id = '" + i + "';";
                        string sql1 = "DELETE FROM Chiarita where Id = '" + i + "';";

                        try
                        {
                            Object[] dburl = db1.searchData(sourceFile, sql);

                            var tab = dburl[0].ToString().Split(new string[] { "->" }, StringSplitOptions.None);
                            string signature = tab[1].Trim();
                            int indice = (i - 1);
                            string chemin = (string)guna2DataGridView30[0, indice].Value;

                            AutreBD.Add(signature);
                            File.Delete(chemin);
                            AutoClosingMessageBox.Show("Delete success ");
                            //guna2DataGridView30.Rows.RemoveAt(indice);
                        }
                        catch (Exception ex)
                        {
                            AutoClosingMessageBox.Show("Exception : " + ex.Message);
                            File.AppendAllLines(VirusBD, AutreBD);
                        }
                    }

                    File.AppendAllLines(VirusBD, AutreBD);
                    choix = "";
                    //***************************************
                    string sql70 = "DROP TABLE Chiarita";
                    if (db1.deleteData(sourceFile, sql70) == true)
                    {
                        //MessageBox.Show("Table Chiarita creer");
                        pictureBox55.Visible = true;
                        label150.Visible = true;
                        guna2DataGridView30.Visible = false;
                    }
                }
                else
                {
                    MessageBox.Show("You have chosen to keep all these files as safe.");
                }

            }
            else
            {
                // ici on va faire une actigne par ligne
                int taille = NewTailleTableau("Chiarita");
                int i = 0;
                int i1 = 0;

                if (taille > 0)
                {
                    if (guna2DataGridView30.CurrentRow != null)
                    {
                        int indice = guna2DataGridView30.CurrentRow.Index;
                        i1 = indice + 1;
                        string chemin = (string)guna2DataGridView30[0, indice].Value;
                        i1 = indice + 1;

                        string sql = "SELECT nomfichier FROM Chiarita where Id = '" + i1 + "';";
                        string sql1 = "DELETE FROM Chiarita where Id = '" + i1 + "';";

                        try
                        {
                            Object[] dburl = db1.searchData(sourceFile, sql);

                            DialogResult result;
                            result = MessageBox.Show("Do you want to delete this file ?", "ManageSafe", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                if (db1.deleteData(sourceFile, sql1) == true)
                                {
                                    var tab = dburl[0].ToString().Split(new string[] { "->" }, StringSplitOptions.None);
                                    string signature = tab[1].Trim();
                                    MessageBox.Show("suppression réussite : " + signature);
                                    if (dburl != null)
                                    {
                                        if (File.Exists(chemin))
                                        {
                                            //MessageBox.Show("fichier existe");
                                            List<string> AutreBD = new List<string>();
                                            var md5signatures = File.ReadAllLines(VirusBD);

                                            foreach (string s in md5signatures)
                                            {
                                                AutreBD.Add(s);
                                            }
                                            AutreBD.Add(signature);
                                            //MessageBox.Show("ici tu as = " + signature);

                                            File.WriteAllLines(VirusBD, AutreBD);

                                            //suppression du fichier de l'ordinateur.

                                            File.Delete(chemin);
                                            AutoClosingMessageBox.Show("Delete success ");
                                            //guna2DataGridView30.Rows.RemoveAt(indice);

                                            // mise à jour de la table Quarantaine.
                                            LoadManageSafe();
                                        }
                                        else
                                        {
                                            MessageBox.Show("The file no longer exists on this computer");
                                            LoadManageSafe();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("You still consider this file safe");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    choix = "";
                    //***************************************
                    string sql70 = "DROP TABLE Chiarita";
                    if (db1.deleteData(sourceFile, sql70) == true)
                    {
                        //MessageBox.Show("Table Chiarita creer");
                        pictureBox55.Visible = true;
                        label150.Visible = true;
                        guna2DataGridView30.Visible = false;
                    }
                }
            }
        }

        // ici je vais faire l'update d'une table
        public int UpdateTable(string nomtable)
        {
            int LastRowID = TailleTable(nomtable);


            for (int i = 1; i <= TailleTable(nomtable); i++)
            {
                string sql = "SELECT *FROM '" + nomtable + "' WHERE Id = '" + i + "';";
                string sql1 = "DROP TABLE '" + nomtable + "';";
                string sql2 = "DELETE FROM Quarantaine where Id = '" + i + "';";

                Object[] dburl1 = db1.searchData(sourceFile, sql);
                if (dburl1 != null)
                {
                    //MessageBox.Show("Valeur Existante");
                    LastRowID = LastRowID + 0;
                }
                else
                {
                    //MessageBox.Show("La valeur n'existe pas");
                    if (db1.deleteData(sourceFile, sql2) == true)
                    {
                        //MessageBox.Show("voilà c'est bon la valeur est là");
                        int x = i - 1;
                        string sql8 = "UPDATE '" + nomtable + "' SET Id = (Id-1) where Id > '" + x + "';";
                        if (db1.deleteData(sourceFile, sql8) == true)
                        {
                            // MessageBox.Show("Table mise à jour");                            
                        }
                    }
                    else
                    {
                        //MessageBox.Show("delete failling");
                        LastRowID = TailleTable(nomtable);
                    }
                }
                string sql9 = "SELECT MAX(Id) FROM '" + nomtable + "';";

                Object[] dburl9 = db1.searchData(sourceFile, sql9);

                string str = dburl9[0].ToString();
                int LastRowID1 = Int32.Parse(str);
                LastRowID = LastRowID1;
            }

            //MessageBox.Show("nouvelle taille tableau : " + LastRowID);
            return (LastRowID);
        }
        // nouvelle taille de tableau mise à jour        
        public int NewTailleTableau(string nomtable)
        {
            int taille = TailleTable(nomtable);
            int newTaille = UpdateTable(nomtable);
            return newTaille;
        }
        // select all pour le Management safe
        private void guna2Button56_Click(object sender, EventArgs e)
        {
            this.ll = this.ll + 1;
            int reste = this.ll % 2;
            choix = "";
            if (reste == 0)
            {
                foreach (DataGridViewRow item in guna2DataGridView30.Rows)
                {
                    item.Selected = false;
                    item.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            else
            {
                foreach (DataGridViewRow item in guna2DataGridView30.Rows)
                {
                    item.Selected = true;
                    item.DefaultCellStyle.ForeColor = Color.Aqua;
                }
                choix = SelectAll[2];
                //MessageBox.Show("je suis Selecte all dans le Manage Safe : " + choix);
            }
        }

        // charger la tale management safe

        private void LoadManageSafe()
        {
            // vérifier que la tale quarantaine existe

            if (File.Exists(sourceFile))
            {
                Boolean verif = db1.CreateTable(sourceFile, "Chiarita");
                if (verif == true)
                {
                    pictureBox55.Visible = true;
                    label150.Visible = true;
                    guna2DataGridView30.Visible = false;
                    //selectedCellCount1 = 0;
                }
                else
                {
                    string sql2 = "";
                    int i1 = 1;
                    string signature = "";
                    //selectedCellCount1 = guna2DataGridView13.Rows.Count;
                    //MessageBox.Show("taille : ");
                    pictureBox55.Visible = true;
                    label150.Visible = true;
                    guna2DataGridView30.Visible = false;

                    try
                    {
                        int LastRowID = NewTailleTableau("Chiarita");

                        //MessageBox.Show("taille : " + LastRowID);
                        LastRowID = UpdateTable("Chiarita");
                        if (LastRowID > 0)
                        {
                            guna2DataGridView30.Rows.Clear();

                            while (i1 <= LastRowID)
                            {
                                sql2 = "select chemin, date, nomfichier from Chiarita where Id='" + i1 + "';";

                                try
                                {
                                    Object[] dburl = db1.searchData(sourceFile, sql2);
                                    /*List<Object[]> datas = db1.selectDatasAuto(sourceFile1, sql2);
                                    if (datas != null)
                                    {
                                        for (var i = 0; i < datas.Count; i++)
                                        {
                                            guna2DataGridView30.Rows.Add(datas[i]);
                                        }
                                    }
                                    else
                                    {
                                        AutoClosingMessageBox.Show("Fatal Error, reboot the application");

                                    }*/
                                    //MessageBox.Show("tttttttttttttttttttt");
                                    if (dburl != null)
                                    {
                                        var nomfichier = dburl[2].ToString().Split(new string[] { "->" }, StringSplitOptions.None);
                                        signature = nomfichier[1];

                                        guna2DataGridView30.Rows.Add(dburl[0].ToString(), dburl[1].ToString(), nomfichier[0]);

                                    }
                                    else
                                    {
                                        AutoClosingMessageBox.Show("Fatal Error, reboot the application");
                                    }

                                    i1++;
                                    guna2DataGridView30.Visible = true;
                                    pictureBox55.Visible = false;
                                    label150.Visible = false;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                //MessageBox.Show("ooooooooooooooooo : " + i1);

                            }
                        }
                        else
                        {
                            AutoClosingMessageBox.Show("Manage Safe table is empty");
                            pictureBox55.Visible = true;
                            label150.Visible = true;
                            guna2DataGridView30.Visible = false;
                        }

                    }
                    catch (Exception ex)
                    {
                        if (!File.Exists(path))
                        {
                            File.Create(path);
                            TextWriter tw = new StreamWriter(path, true);
                            tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql2 + " " + "Error_Message:" + ex);
                            tw.Close();
                        }

                        else if (File.Exists(path))
                        {
                            TextWriter tw = new StreamWriter(path, true);
                            tw.WriteLine(DateTime.Now.ToString() + " " + "Request:" + " " + sql2 + " " + "Error_Message:" + ex);
                            tw.Close();
                        }
                    }
                }
            }
            else
            {
                //AutoClosingMessageBox.Show("Manage Safe table is empty");
                pictureBox55.Visible = true;
                label150.Visible = true;
                guna2DataGridView30.Visible = false;
            }

        }
        // Select All pour la table quarantaine
        private void guna2Button32_Click(object sender, EventArgs e)
        {
            this.ll = this.ll + 1;
            int reste = this.ll % 2;
            choix = "";
            if (reste == 0)
            {
                foreach (DataGridViewRow item in guna2DataGridView14.Rows)
                {
                    item.Selected = false;
                    item.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            else
            {
                foreach (DataGridViewRow item in guna2DataGridView14.Rows)
                {
                    item.Selected = true;
                    item.DefaultCellStyle.ForeColor = Color.Aqua;
                }
                choix = SelectAll[0];
                //MessageBox.Show("je suis Selecte all dans la quarantaine : " + choix);
            }
        }
        //******************************************************************************************************************************
        // End Quarantine
        //******************************************************************************************************************************

        //******************************************************************************************************************************
        // Start Security
        //******************************************************************************************************************************
        private void loadFirewall()
        {
            try
            {
                ManagementObjectSearcher wmiData = new ManagementObjectSearcher(@"root\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
                ManagementObjectCollection data = wmiData.Get();
                ManagementObject firewall = data.OfType<ManagementObject>().First();
                labelFirewallName.Text = firewall["displayName"].ToString();
                String state = firewall["productState"].ToString();
                switch (state)
                {
                    case "397568": //Windows defender
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "True";
                        break;
                    case "397584"://Windows defender
                        labelFirewallSignature.Text = "Out to date";
                        labelFirewallStatut.Text = "True";
                        break;
                    case "393472"://Windows defender
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "False";
                        break;
                    case "397312"://Microsoft security essentials
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "True";
                        break;
                    case "393216"://Microsoft security essentials
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "False";
                        break;
                    case "266256"://AVG Internet Security 2012 firewall product
                        labelFirewallSignature.Text = "Firewall";
                        labelFirewallStatut.Text = "True";
                        break;
                    case "262160"://AVG Internet Security 2012 firewall product
                        labelFirewallSignature.Text = "Firewall";
                        labelFirewallStatut.Text = "False";
                        break;
                    case "262144"://AVG Internet Security 2012 antivirus product
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "False";
                        break;
                    case "266240"://AVG Internet Security 2012 antivirus product
                        labelFirewallSignature.Text = "Up to date";
                        labelFirewallStatut.Text = "True";
                        break;
                    default://We don't have information about product
                        labelFirewallSignature.Text = "--";
                        labelFirewallStatut.Text = "False";
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show(e.Message);
            }
        }

        // display antivirus
        private void antivirusDisplay()
        {
            string wmipathstr = @"\\" + Environment.MachineName + @"\root\SecurityCenter2";
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmipathstr, "SELECT * FROM AntivirusProduct");
                ManagementObjectCollection instances = searcher.Get();
                guna2DataGridView16.Rows.Clear();
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    String state = queryObj["productState"].ToString();
                    switch (state)
                    {
                        case "397568"://Windows defender
                            state = "Enabled and up to date";
                            break;
                        case "397584"://Windows defender
                            state = "Enabled and out to date";
                            break;
                        case "393472"://Windows defender
                            state = "Disabled and up to date";
                            break;
                        case "397312"://Microsoft security essentials
                            state = "Enabled and up to date";
                            break;
                        case "393216"://Microsoft security essentials
                            state = "Disabled and up to date";
                            break;
                        case "266256"://AVG Internet Security 2012 firewall product
                            state = "Firewall enabled";
                            break;
                        case "262160"://AVG Internet Security 2012 firewall product
                            state = "Firewall disabled";
                            break;
                        case "262144"://AVG Internet Security 2012 antivirus product
                            state = "Disable and up to date";
                            break;
                        case "266240"://AVG Internet Security 2012 antivirus product
                            state = "Enabled and up to date";
                            break;
                        default://We don't have information about product
                            state = queryObj["productState"].ToString();
                            break;
                    }
                    guna2DataGridView16.Rows.Add(queryObj["displayName"], queryObj["instanceGuid"], queryObj["pathToSignedProductExe"], /*queryObj["productState"]*/
            state);
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("InstanceName: {0}", queryObj.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show(e.Message);
            }
        }

        // wifi password
        private string GetWifiNetworks()
        {
            //execute the netsh command using process class
            Process processWifi = new Process();
            processWifi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processWifi.StartInfo.FileName = "netsh";
            processWifi.StartInfo.Arguments = "wlan show profile";

            processWifi.StartInfo.UseShellExecute = false;
            processWifi.StartInfo.RedirectStandardError = true;
            processWifi.StartInfo.RedirectStandardInput = true;
            processWifi.StartInfo.RedirectStandardOutput = true;
            processWifi.StartInfo.CreateNoWindow = true;
            processWifi.Start();

            string output = processWifi.StandardOutput.ReadToEnd();

            processWifi.WaitForExit();
            return output;
        }
        private string ReadPassword(string Wifi_Name)
        {

            string argument = "wlan show profile name=\"" + Wifi_Name + "\" key=clear";
            Process processWifi = new Process();
            processWifi.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processWifi.StartInfo.FileName = "netsh";
            processWifi.StartInfo.Arguments = argument;


            processWifi.StartInfo.UseShellExecute = false;
            processWifi.StartInfo.RedirectStandardError = true;
            processWifi.StartInfo.RedirectStandardInput = true;
            processWifi.StartInfo.RedirectStandardOutput = true;
            processWifi.StartInfo.CreateNoWindow = true;
            processWifi.Start();

            string output = processWifi.StandardOutput.ReadToEnd();
            processWifi.WaitForExit();
            return output;
        }
        private string GetWifiPassword(string Wifi_Name)
        {
            string get_password = ReadPassword(Wifi_Name.Trim());
            using (StringReader reader = new StringReader(get_password))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Contenu de la") || line.Contains("Key Content"))
                    {
                        string current_password = line.Substring(line.IndexOf(":") + 1).Trim();
                        return current_password;
                    }
                }
            }
            return "Open Network - NO PASSWORD";
        }
        private string GetAutentication(string Wifi_Name)
        {
            string get_Auth = ReadPassword(Wifi_Name.Trim());
            using (StringReader reader = new StringReader(get_Auth))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Authentification") || line.Contains("Authentication"))
                    {
                        string current_Auth = line.Substring(line.IndexOf(":") + 1).Trim();
                        return current_Auth;
                    }
                }
            }
            return "None";
        }
        // main Method 
        int wifiCount = 0;
        int Wifi_count_names = 0;
        private void get_Wifi_passwords()
        {
            string WifiNetworks = GetWifiNetworks();
            using (StringReader reader = new StringReader(WifiNetworks))
            {

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    wifiCount++;
                    string wifi = line.Substring(line.IndexOf(":") + 1);
                    if ((wifi.Trim() != "") && (wifi != line))
                    {
                        labelInfo.Visible = false;
                        Wifi_count_names++;
                        string Wifi_name = wifi;
                        string Wifi_password = GetWifiPassword(Wifi_name);
                        string Wifi_authentication = GetAutentication(Wifi_name);
                        guna2DataGridView17.Rows.Add("", Wifi_name, Wifi_authentication == "Ouvrir" ? "Open" : Wifi_authentication, Wifi_password);

                    }
                }
            }


        }
        // Scan wifi password
        private void guna2Button38_Click(object sender, EventArgs e)
        {
            guna2Button38.Enabled = false;
            guna2DataGridView17.Rows.Clear();
            get_Wifi_passwords();
            if (guna2DataGridView17.Rows.Count <= 0)
            {
                labelInfo.Visible = true;
                labelInfo.Text = "There is no wifi password stored on this computer !";
            }
            guna2Button38.Enabled = true;
        }
        // get password stored
        public void getPassword()
        {
            List<IPassReader> readers = new List<IPassReader>();
            readers.Add(new FirefoxPassReader());
            readers.Add(new ChromePassReader());
            //readers.Add(new IE10PassReader());

            foreach (var reader in readers)
            {
                MessageBox.Show("Val = " + reader);
                try
                {
                    ShowDetails(reader.ReadPasswords(), reader.BrowserName);

                    /*foreach (var d in reader.ReadPasswords())
                    {
                        guna2DataGridView18.Rows.Add(d.Url.ToString(), d.Username.ToString(), d.Password.ToString(), reader.BrowserName.ToString());

                    }*/

                    label95.Text = "" + guna2DataGridView18.Rows.Count;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading " + reader.BrowserName + " passwords: " + ex.Message);
                }
            }
                        #if DEBUG
                        Console.ReadLine();
                        #endif
        }
        public void ShowDetails(IEnumerable<CredentialModel> data, String browser)
        {
            foreach (var d in data)
            {
                guna2DataGridView18.Rows.Add(d.Url, d.Username.Remove(3, 5), d.Password.Remove(3, 5), browser);
                Console.WriteLine("Url: {d.Url}, UN: {d.Username}, Pwd:{d.Password}, Browser: {browser}");
            }
        }
        // Scan pwrd btn click
        private void guna2Button42_Click(object sender, EventArgs e)
        {
            getPassword();
        }
        // parental control 
        public Boolean isURL1(string url)
        {
            //création d'une uri a valeur "null"
            Uri CreatedUri;

            //on tente de créer l'url en vérifiant qu'elle est conforme a une url http ou https
            Boolean IsValid = Uri.TryCreate(url, UriKind.Absolute, out CreatedUri) && (CreatedUri.Scheme == Uri.UriSchemeHttp || CreatedUri.Scheme == Uri.UriSchemeHttps);

            if ((url != "") && (IsValid))
            {
                return true;// on valide l'url entrée 
            }
            else
            {
                return false;//on invalide l'url entrée
            }
        }

        public static string targetPath1 = AppDomain.CurrentDomain.BaseDirectory;
        //public static string name_db1 = "parentalControl.db";
        public static string name_db1 = "ScanDataBase.db";
        public static string sourceFile1 = targetPath1 + name_db1;
        public void refreshData1()
        {
            if(db1.CreateTable(sourceFile1, "parentControl") == true)
            {
                MessageBox.Show("Table Control create. ");
            }
            else
            {
                List<Object[]> datas = db1.selectDatasAuto(sourceFile1, "select * from parentControl");
                guna2DataGridView15.Rows.Clear();

                if (datas != null)
                {
                    for (var i = 0; i < datas.Count; i++)
                    {
                        guna2DataGridView15.Rows.Add(datas[i]);
                    }
                }
            }
        }
        //Enable Control
        public void enable_controle1(Boolean f)
        {
            guna2TextBox1.Enabled = f;
            guna2Button39.Enabled = f;
        }
        // Add website
        private void guna2Button39_Click(object sender, EventArgs e)
        {
            // je recupere le nom de domaine de l'url recuperer dans le browser

            string url = guna2TextBox1.Text;
            string status = "";
            string url1 = "";
            Uri uri;
            uri = new Uri(url);
            url1 = uri.Host;

            // j'effectue une recherche du nom de domaine et le statut dans la base de donnee

            string url3 = "http://" + url1;
            status = "Bad";
            string sitetblock = "127.0.0.1    " + url1;

            DateTime Date = DateTime.Now;
            Date.ToString("dd/MM/yyyy HH:mm:ss");

            try
            {
                Boolean tr = db1.CreateTable(sourceFile1, "parentControl");
                if (tr == true)
                {
                    //MessageBox.Show("Table créer ");
                }
                else
                {
                    // MessageBox.Show("Table Existe deja ");
                    if (isURL1(url) == true)
                    {
                        if (db1.searchData(sourceFile1, "select * from parentControl where url='" + url + "';") == null)
                        {
                            string sql = "insert into parentControl (url, status, date) values(";
                            sql = sql + "'" + url1 + "', ";
                            sql = sql + "'" + status + "', ";
                            sql = sql + "'" + Date + "')";

                            Boolean error = db1.insertData(sourceFile1, sql);

                            if (error == true)
                            {
                                // ajouter le site au fichier Host : site bloqué
                                try
                                {
                                    using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers/etc/hosts")))
                                    {
                                        w.WriteLine(sitetblock);
                                        w.Close();
                                    }
                                    AutoClosingMessageBox.Show("Site Blocked By Draka Shiel ");

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Fatal error, please start the application as Administrator");

                                }
                                MessageBox.Show("Website Blocked");
                            }
                            else
                            {
                                MessageBox.Show("Unable to save file to DB");
                            }
                            enable_controle1(false);
                            refreshData1();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception " + ex.Message);
            }

        }

        // new Btn click 
        private void guna2Button40_Click(object sender, EventArgs e)
        {
            enable_controle1(true);
            guna2TextBox1.Text = "";
        }
        // delete website
        private void guna2Button41_Click(object sender, EventArgs e)
        {
            enable_controle1(false);

            if (guna2DataGridView15.SelectedRows.Count > 0)
            {
                string num = guna2DataGridView15.CurrentRow.Cells[0].Value.ToString();
                string url = guna2DataGridView15.CurrentRow.Cells[1].Value.ToString();
                string status = guna2DataGridView15.CurrentRow.Cells[2].Value.ToString();

                string sitedebloque = "127.0.0.1    " + url;
                //MessageBox.Show(sitedebloque);
                
                DialogResult result;
                result = MessageBox.Show("Are you an administrator ?", "Quarantaine", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string sql = "delete from parentControl where url='" + url + "'";
                    Boolean error = db1.deleteData(sourceFile1, sql);

                    if (error == true)
                    {
                        try
                        {
                            //MessageBox.Show("URL : " + url);
                            deblocageSite(sitedebloque);
                        }
                        catch (Exception ex)
                        {
                            //deblocageSite(url);
                            MessageBox.Show("Fatal error, please start the application as Administrator");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Database does not exist", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You can only perform this Action as an administrator.");
                    refreshData1();
                }                    
            }
            else
            {
                MessageBox.Show("No line selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // déblocage d'un site internet
        public void deblocageSite(String str)
        {
            // récupération du contenu du fichier Host.
            // récupération des bonne valeur pour l'authentification Administrateur.
            // cette valeur sera prise plus tard dans la table Admin, de notre base de données
            // enrégistrer à l'activation du produit avec l'email et le mot de passe creer

            string Email = "";
            string PWD = "";
            ID_Admin frm14 = new ID_Admin();
            //MessageBox.Show("Je suis la suite du botton delete ");

            

            if (db1.CreateTable(sourceFile, "Admin") == true)
            {
                //MessageBox.Show("the Admin table is successfully created ");

                string email1   = "client_Test@drakashield.com";
                string password = "azerTYuiop_1254";
                DateTime Date = DateTime.Now;
                Date.ToString("dd/MM/yyyy HH:mm:ss");

                string sql10 = "INSERT INTO Admin (Email, password, dateActivation)values(";
                sql10 = sql10 + "'" + email1 + "', ";
                sql10 = sql10 + "'" + password + "', ";
                sql10 = sql10 + "'" + Date + "')";

                if(db1.insertData(sourceFile, sql10) == true)
                {
                    MessageBox.Show("Registered Administrator.");
                }
            }
            else
            {
                //MessageBox.Show("the Admin table is no created");
                // ici on va récuperer de la table admin creer à l'activation du produit et qui représente les 
                // les identifiants administrateurs

                string sql = "SELECT * FROM Admin ;";
                try
                {
                    Object[] dburl = db1.searchData(sourceFile, sql);

                    Email = dburl[1].ToString();
                    PWD = dburl[2].ToString();
                    
                    /*MessageBox.Show("Email : " + Email);
                    MessageBox.Show("passeword : " + PWD);*/

                    frm14.ShowDialog();

                    List<String> list = new List<String>();
                    var lecture = File.ReadAllLines(passeword);

                    foreach (string item in lecture)
                    {
                        list.Add(item.Trim());
                    }
                    /*MessageBox.Show("Email : " + list[0].Trim());
                    MessageBox.Show("passeword : " + list[1].Trim());*/

                    if (Email.Equals(list[0].Trim()) && PWD.Equals(list[1].Trim()))
                    {
                        AutoClosingMessageBox.Show("Action Allowed.");

                        // récupération du contenu du fichier Host.
                        
                        string[] ContenueHost = File.ReadAllLines(FichierHost);
                        List<string> Hosts = new List<string>();

                        StreamWriter st = new StreamWriter(FichierHost, false); // j'éfface le contenu du fichier Hosts
                        st.Close();

                        //MessageBox.Show("Contenue du fichier Hosts éffacé");

                        foreach (string elt in ContenueHost)
                        {
                            if (elt.Equals(str))
                            {
                                //MessageBox.Show("uyuyuy  :  " + elt);
                                continue;
                            }
                            else
                            {
                                Hosts.Add(elt.Trim());
                                //MessageBox.Show("yayatoure :  " + elt);
                            }
                        }
                        File.AppendAllLines(FichierHost, Hosts);
                        refreshData1();
                        AutoClosingMessageBox.Show(" Website now available ");
                    }
                    else
                    {
                        MessageBox.Show("Please log in as Administrator to perform this action");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Restart the application, then try again");
                }
            }
        }
         
        // récupération des valeurs saisies dans le formulaire contact

        public void Contact_Valeurs()
        {
            
            string Piece_jointe = "";
            
            string check_casa = "";
            //guna2Button52 est le bouton Envoyer sur le formulaire
            // guna2Button51 le buton selectionnner un fichier
            // label label117 Chemin
            //guna2CustomCheckBox12 c'est checkbonn
            //label128 Message    guna2TextBox7
            //label126 c'est pour le tel
            //label120 numéro de tel guna2TextBox6
            //guna2TextBox5 nom et prenom
            //guna2TextBox3 email  
            
        }

        // boutton envoyer du formulaire de contact

        private void BoutonEnvoyer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Je suis le boutton Envoyer les information ");
            CreateTestMessage2("smtp.gmail.com");
        }
        private void guna2Button51_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if(fd.ShowDialog() == DialogResult.OK)
            {
                label117.Text= fd.FileName; 
            }
        }
        public void CreateTestMessage2(string server)
        {
            string Email = guna2TextBox3.Text;
            string password = guna2TextBox4.Text;
            string Nom_P = guna2TextBox5.Text;
            string Tel = guna2TextBox6.Text;
            string message = guna2TextBox7.Text;
            string str = "drakashieldsolutions@gmail.com";//"ragotjeune55@gmail.com";


            SmtpClient smtp = new SmtpClient()
            {
                Host = server,//"smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential
                {
                    UserName = Email,
                    Password = password//"rgfmvxbczdxhbyso"
                },
            };
            MailAddress mailAddress = new MailAddress(Email, "Visiteur");
            MailAddress ToAddress = new MailAddress(str, "DrakaShield");
            MailMessage mail = new MailMessage()
            {
                From = mailAddress,
                Subject = "Need informations",
                Body = message
            };
            
            if (Email.Equals(""))
            {
                try
                {
                    mail.To.Add(ToAddress);
                    smtp.Send(mail);
                    MessageBox.Show("Email well sent");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Mail has been sent successfully!", "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        //***********************************************************************************
        //                      Objectionnals part objectional website
        //***********************************************************************************
        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            securityViewCollection.SelectTab("website");
            refreshData12();
        }
        public static string chemin = targetPath + @"\Draka_Shiel\";
        public static string name_db3 = "objectionable_websites.db";
        public static string sourceFile3 = chemin + name_db3;
        Database db = new Database();

        public System.Drawing.Color[] Colors { get => colors; set => colors = value; }
        public System.Drawing.Color[] Colors1 { get => colors; set => colors = value; }
        public System.Drawing.Color[] Colors2 { get => colors; set => colors = value; }

        public void refreshData12()
        {
            //int taille = NewTailleTableau("websites");
            List<Object[]> datas = db.selectDatasAuto(sourceFile3, "select * from websites");

            string[] tab = {"", "", "", "" };

            guna2DataGridView19.Rows.Clear();
            int i1 = 0;
            if (datas != null)
            {
                for (var i = 0; i < datas.Count; i++)
                {
                    i1 = i + 1;
                    tab[0] = i1.ToString();//datas[i][1].ToString();
                    tab[1]  = datas[i][2].ToString();
                    tab[2]  = datas[i][3].ToString();
                    tab[3]  = datas[i][4].ToString();

                    guna2DataGridView19.Rows.Add(tab);                    
                }                
            }
        }
        public void enable_controle(Boolean f)
        {

        }
        /*public void enable_controle1(Boolean f)
        {
            guna2TextBox1.Enabled = f;
            guna2Button39.Enabled = f;
        }*/

        private void guna2Button47_Click(object sender, EventArgs e)
        {
            enable_controle(false);

            if (guna2DataGridView19.SelectedRows.Count > 0)
            {
                string num = guna2DataGridView19.CurrentRow.Cells[0].Value.ToString();
                string url = guna2DataGridView19.CurrentRow.Cells[1].Value.ToString();
                string status = guna2DataGridView19.CurrentRow.Cells[2].Value.ToString();

                string sql = "delete from websites where url='" + url + "'";
                Boolean error = db.deleteData(sourceFile3, sql);

                if (error == true)
                {
                    refreshData12();
                }
                else
                {
                    MessageBox.Show("Database does not exist", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No line selected", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //******************************************************************************************************************************
        // End Objectional Website
        //******************************************************************************************************************************

        //***********************************************************************
        // Quarantaine Automatique et manuel
        //***********************************************************************
        private void guna2CustomCheckBox6_Click(object sender, EventArgs e)
        {
            // checkbox  --> label114 : Automatique
            MessageBox.Show("je suis checkBox6 et je suis Anglais");
        }

        private void guna2CustomCheckBox7_Click(object sender, EventArgs e)
        {
            // checkbox  --> label112 : manuel
            //MessageBox.Show("je suis manuel dans quarantaine");
            if (guna2CustomCheckBox7.Checked == true)
            {
                QAutoManuel = "Manuel";
                //MessageBox.Show("Delete Manuel Activé");
                guna2CustomCheckBox9.Checked = false;
            }
            else
            {
                QAutoManuel = "Auto";
                //MessageBox.Show("Delete Manuel désactivé");
                guna2CustomCheckBox9.Checked = true;
            }
        }

        private void guna2CustomCheckBox8_Click(object sender, EventArgs e)
        {
            // checkbox  --> label116 : Activer
            MessageBox.Show("je suis la langue fançais");
        }

        private void guna2CustomCheckBox9_Click(object sender, EventArgs e)
        {
            // checkbox  --> label116 : Activer
            //MessageBox.Show("je suis Automatique dans quarantaine");
            //guna2CustomCheckBox9.Checked = QAutoManuel;

            if (guna2CustomCheckBox9.Checked == true)
            {
                QAutoManuel = "Auto";
                //MessageBox.Show("Delete Activé");
                guna2CustomCheckBox7.Checked = false;
            }
            else
            {
                QAutoManuel = "Manuel";
                //MessageBox.Show("Delete désactivé");
                guna2CustomCheckBox7.Checked = true;
            }
        }

        private void guna2CustomCheckBox10_Click(object sender, EventArgs e)
        {
            // checkbox  --> label115 : desactiver
            MessageBox.Show("je suis desactiver dans controle parental");
        }

        private void guna2CustomCheckBox11_Click(object sender, EventArgs e)
        {
            // checkbox  --> label116 : Activer
            MessageBox.Show("je suis Activer dans controle parental");
        }

        private void guna2CustomCheckBox12_Click(object sender, EventArgs e)
        {
            // checkbox  --> label116 : Activer
            MessageBox.Show("je suis checkBox12");
        }
        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            if(guna2CustomCheckBox1.Checked == true)
            {
                verif1 = true;
                if(programScan == "Daily")
                {
                    programScan = " ";
                    var str = guna2DateTimePicker4.Value.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                    var heurr = str[0].ToString().Split(new string[] { " " }, StringSplitOptions.None);
                    string heure = heurr[1] + ":" + str[1] + ":" + str[2];
                    MessageBox.Show("Valeur Daily");
                }
                else if(programScan == "Weekly")
                {
                    programScan = " ";
                    var str = guna2DateTimePicker1.Value.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                    MessageBox.Show("V = " + str[0]);
                    var heurr = str[0].ToString().Split(new string[] { " " }, StringSplitOptions.None);
                    string heure = heurr[1] + ":" + str[1] + ":" + str[2];

                    MessageBox.Show("Valeur Weekly");
                }
                else
                {
                    MessageBox.Show("Please Choose a Scan Period.");
                }
            }
            else
            {
                verif1 = false;
                programScan = " ";
            }
        }
        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            if (guna2CustomCheckBox2.Checked == true)
            {
                verif2 = true;
                if (programScan == "Daily")
                {
                    programScan = " ";
                    MessageBox.Show("Valeur Daily");
                }
                else if( programScan == "Weekly")
                {
                    programScan = " ";
                    MessageBox.Show("Valeur Weekly");
                }
                else
                {
                    MessageBox.Show("Please Choose a Scan Period.");
                }
                /*verif2 = true;
                var str = guna2DateTimePicker2.Value.ToString().Split(new string[] { ":"}, StringSplitOptions.None);
                var heurr = guna2DateTimePicker3.Value.ToString().Split(new string[] { " " }, StringSplitOptions.None);
                
                string heure = heurr[1];
                var heur11 = heure.ToString().Split(new string[] { ":" }, StringSplitOptions.None);
                string heuref = heur11[0] + ":" + heur11[1];
                string kk = str[0];
                var val = kk.Split(new string[] { " " }, StringSplitOptions.None);
                Partiel[0] = val[0] + " " + heuref;
                Partiel[1] = label104.Text;*/
                //MessageBox.Show(Partiel[0]);
            }
            else
            {
                verif2 = false;
                programScan = " ";
                // suppression du scan dans la base de données
                Partiel[0] = " ";
                Partiel[1] = " ";
                //MessageBox.Show("Déactiver");
            }
        }

        private void guna2DateTimePicker1_Click(object sender, EventArgs e)
        {
            /*
             * 
                string VerifAutoScan
                string ProgramScan 
                string ProgramPartiel 
            *
            */

            // checkbox  --> label116 : Activer
            //MessageBox.Show("On commence date1");                      
            //guna2DateTimePicker1.Value = new System.DateTime(2021, 12, 31, 13, 13, 50, 949);
        }

        private void guna2DateTimePicker2_Click(object sender, EventArgs e)
        {
            // checkbox  --> label116 : Activer
            //Partial[0] = "Scan_Partiel";
            //MessageBox.Show("On commence date2");
            //guna2CustomCheckBox2
        }
        private void guna2Button49_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            label104.Text = folderBrowserDialog1.SelectedPath;
            chemin11 = label104.Text;

        }

        //******************************************************************************************************************************
        // End Security
        //******************************************************************************************************************************

        private void button1_Click_1(object sender, EventArgs e)
        {
            /* label2.Text = "0";*/
            label8.Text = "initiailisations... ";
            label6.Text = "00h:00mm:00s";
            custom_FolderPicker.ShowDialog();
            custom_folderPickerText.Text = custom_FolderPicker.SelectedPath;

            virus = 0;
            files = 0;
            label4.Text = virus.ToString();
            custom_progressBar.Value = 0;
        }


        private void custom_viewList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void title3_Click(object sender, EventArgs e)
        {
            controlkey();
        }

        public void controlkey()
        {
            try
            {
                string sqlkey = "select key,dateregist,expiration,daynotkey,passedday from keyregister where id = " + LastInseretId();
                Object[] dburl = db1.searchData(sourceFile, sqlkey);

                if (dburl != null)
                {
                    Console.WriteLine("Object 1 => " + dburl[1].ToString());
                    Console.WriteLine("Object 2 => " + dburl[2].ToString());

                    DateTime expirationTime = Convert.ToDateTime(dburl[2].ToString());
                    DateTime nowDate = DateTime.Now;

                    TimeSpan t = (expirationTime - nowDate);
                    int validityDate = (int)t.TotalDays;

                    Console.WriteLine("Validation day => " + validityDate);


                    if (validityDate > 0)
                    {
                        title3.Refresh();
                        title3.ForeColor = System.Drawing.Color.Green;
                        title3.Text = "Reactivation dans " + validityDate + " jrs ";
                    }
                    else if (validityDate < 0)
                    {
                        int v = ((-1) * validityDate);

                        MessageBox.Show("Activate your antivirus now because it’s already \n " + v + "days you are no longer protected !!!", "Draka recording", MessageBoxButtons.OK);
                        title3.Text = "Activate your antivirus !!!";
                    }
                    else
                    {
                        MessageBox.Show("Activate your antivirus now because it makes dejaProduct Keys Draka Antivirus is espirer or abscente \n Enter a key and reboot Draka Antivirus", "Draka recording", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    //MessageBox.Show("Désolé pour l'échec");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                pagesView.SelectTab("activate");
                /*MessageBox.Show("Please Contact a Draka administrator with this message \n " + e.Message, "Draka Registration", MessageBoxButtons.OK);*/
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK);

            }

        }

        private int LastInseretId()
        {
            string sqlkey = "select max(id) from keyregister";
            int lastId = db1.LasInsertId(sourceFile, sqlkey);
            /*Console.WriteLine("LAst insert Id is => " + lastId);*/
            return lastId;
        }
        // activation du produit
        private void guna2Button48_Click_1(object sender, EventArgs e)
        {
            Activation activation = new Activation();
            string key = guna2TextBox8.Text;
            activation.ActivateProduct(key);
        }

        private void homeBtn_MouseHover_1(object sender, EventArgs e)
        {
            t1.Show("Home", homeBtn);
        }

        private void scanBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Scan", scanBtn);
        }

        private void securityBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Security", securityBtn);
        }

        private void QuarantBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Quarantine", QuarantBtn);
        }

        private void historyBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("History", historyBtn);
        }

        private void updateBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Update", updateBtn);
        }

        private void maintainBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Maintenance", maintainBtn);
        }

        private void stabilityBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Stability", stabilityBtn);
        }

        private void performanceBtn_MouseHover(object sender, EventArgs e)
        {
            t1.Show("Perfornace", performanceBtn);
        }

        private void guna2Button50_Click(object sender, EventArgs e)
        {
            // ici nous allons validé la programmation de scan
            //- 1 - on prend tout d'abor l'information sur le type de scan

            string selected = guna2comboBox1.GetItemText(this.guna2comboBox1.SelectedItem);
            string selected1 = guna2comboBox2.GetItemText(this.guna2comboBox2.SelectedItem);

            if (verif1 == true && verif2 == false)
            {
                
            }


        }

        private void guna2Button52_Click_1(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text != null && guna2TextBox4.Text != null && guna2TextBox5.Text != null && guna2TextBox6.Text != null && guna2TextBox7.Text != null)
            {
                if (guna2TextBox3.Text.ToLower() == guna2TextBox4.Text.ToLower())
                {
                    var datas = new Dictionary<string, string>
                    {
                        {"Email", guna2TextBox3.Text.ToLower() },
                        {"Name", guna2TextBox5.Text.ToLower() },
                        {"Phone", guna2TextBox6.Text.ToLower() },
                        {"Message", guna2TextBox7.Text.ToLower() }
                    };

                    var dats = new FormUrlEncodedContent(datas);
                    var url = "https://keygen.drakashield.com/clientdatas";
                    var client = new HttpClient();
                    var response = client.PostAsync(url, dats);
                    string result = response.ToString();
                    Console.WriteLine("http response => " + result);
                    MessageBox.Show("Thanks for your informations \n Admin will respond in 48h", " Client Complaint", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Emails not the same please try again", " Client Complaint", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Some information was not completed", " Client Complaint", MessageBoxButtons.OK);
            }
        }

        private void CompletfolderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {

        }

        private void label71_Click(object sender, EventArgs e)
        {

        }

        private void label90_Click(object sender, EventArgs e)
        {

        }
        private void guna2DataGridView13_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2DataGridView20_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
