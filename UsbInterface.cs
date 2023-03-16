﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Timer = System.Windows.Forms.Timer;
//using System.Windows.Automation;
using System.IO;
using System.Security.Policy;
using Draka_Antivirus.DAO;
using System.Windows.Automation;
using System.Reflection.Emit;
using System.Windows.Forms.VisualStyles;

namespace ScanAutomatique
{
    public partial class UsbInterface : Form
    {
        private System.Threading.ManualResetEvent _busy = new System.Threading.ManualResetEvent(false);
        public static string targetPath = AppDomain.CurrentDomain.BaseDirectory;
       // public static string name_db = "ScanDataBase.db";
        public static string path = "";
        public static string sourceFile = targetPath + "MD5Base.txt";
        string db1 = "rr";
        Color[] colors = { Color.Aqua, Color.Green, Color.Blue, Color.Black, Color.DeepSkyBlue, Color.Red };
        public static string name_db3 = "objectionable_websites.db";
        public static string sourceFile3 = targetPath + name_db3;
        Database db = new Database();

        string BD_Virale = targetPath + "viraldatabase.txt";

        // nous sommes dans la classe Scan qui sera auto appelé dans le constructeur        

        private const int WM_DEVICECHANGE = 0x219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVTYP_VOLUME = 0x00000002;
        //private bool isVisibleCore = false;
        public UsbInterface()
        {
            InitializeComponent();
            MessageBox.Show("Click sur okey pour déclencher le scan automatique ");
            InitTimer();
            ScanTotal scanT = new ScanTotal();
            try
            {
                if(sourceFile3 != null)
                {
                    MessageBox.Show("la bd existe deja ");
                }
                else
                {
                    sourceFile3 = db.createDatabase(name_db3);
                    Boolean er = db.CreateTable(sourceFile3, "websites") == true;
                    if(er == true)
                    {
                        MessageBox.Show("base de créer avec succès ");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex);
            }
            //SetVisibleCore(isVisibleCore);
            /*this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width,
                                   Screen.PrimaryScreen.WorkingArea.Height - this.Height);*/
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width ,
                                   Screen.PrimaryScreen.WorkingArea.Height );
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:
                            //listBox1.Items.Add("New Device Arrived");
                            //MessageBox.Show("Clé branché, En attente du lancement du Scan Auto");
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)Marshal.PtrToStructure(m.LParam, typeof(DevBroadcastVolume));
                                /*listBox1.Items.Add("Mask is " + vol.Mask);
                                listBox1.Items.Add("Letter is " + GetLetter(vol.Mask));*/
                                path = GetLetter(vol.Mask).ToString() + @":\";
                                //MessageBox.Show("Chemin : " + path);
                                //File.Copy(sourceFile, path);
                                CheckRemove(path);
                            }
                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            //listBox1.Items.Add("Device Removed");
                            MessageBox.Show("Clé débranché");
                            //CheckRemove(path);
                            break;
                    }
                    break;
            }
        }

        public void CheckRemove(string str)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    double free, total;
                    free = d.TotalFreeSpace / Math.Pow(1024, 3);
                    total = d.TotalSize / Math.Pow(1024, 3);
                    //MessageBox.Show("ll : "+d);
                    string usb = d.Name;
                    
                    //MessageBox.Show("Total Free : " + free + " Total size : " + total);
                    if (usb.Equals(str))
                    {
                        Form1 DetectionUSB = new Form1(str);
                        DetectionUSB.StartPosition = FormStartPosition.Manual;
                        DetectionUSB.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - DetectionUSB.Width,
                                               Screen.PrimaryScreen.WorkingArea.Height - DetectionUSB.Height);
                        //DirectoryInfo lecteurUSB = new DirectoryInfo(d.RootDirectory.ToString());
                        //MessageBox.Show("Je suis la clé usb branché : "+ lecteurUSB.FullName);
                        //int Taille = (int)(total - free);
                        //MessageBox.Show("gggg : " + Taille);

                        // ici, on appel la méthode scan complet sur la clé connecté                        
                        DetectionUSB.Show();
                    }
                }


            }
            /*string tt = @"\Program Files\" + str;
            MessageBox.Show(tt);*/
        }

        // les sous méthodes

        public char GetLetter(int mask)
        {
            int ch = 0;
            for (; ch < 26; ch++)
            {
                if ((mask & 0x1) == 0x1)
                    break;
                mask >>= 1;
            }
            ch += 0x41;
            return (char)ch;
        }

        private void UsbInterface_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void UsbInterface_Load(object sender, EventArgs e)
        {
           /* this.Visible = false;
            this.WindowState = FormWindowState.Normal;*/
           this.Hide(); 
        }

        //*********************************************************************************************************
        //Objectional Website
        //*********************************************************************************************************

        // 1 - chercher le navigateur courant encours d'utilisation c'est a dire tand qu'il est au premier plan

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public string ChechNavCurren()
        {
            string CurrenNav = "";
            // ici nous cherchons le procecus au premier plan 
            //MessageBox.Show(" je suis la methode ChechNavCurren ");
            IntPtr hWnd = GetForegroundWindow();
            uint procId = 0;

            GetWindowThreadProcessId(hWnd, out procId);
            var proc = Process.GetProcessById((int)procId);
            var name = proc.ProcessName;
           
            // ce processus est t'il l'un des navigateurs cité plus bas ?

            if (name.Equals("msedge"))
            {
                // MessageBox.Show(" navigateur choisi ");
                CurrenNav = proc.ProcessName;
            }
            else if (name.Equals("chrome"))
            {
                CurrenNav = proc.ProcessName;
            }
            else if (name.Equals("opera"))
            {
                CurrenNav = proc.ProcessName;
            }
            else if (name.Equals("firefox"))
            {
                CurrenNav = "firefox";
            }else if(name.Equals("internet explorer"))
            {
                CurrenNav = proc.ProcessName;
            }
            CurrenNav = name;
            //MessageBox.Show("Je suis : " + CurrenNav);
            return CurrenNav;
        }

        // 2 - chercher l'url courant sur le navigateur
        public string CheckUrl(string nomNav)
        {
            //MessageBox.Show(" je suis la méthode checkUrl ");
            string nav = nomNav;
            string url = LoadNav(nomNav);

            return url;
        }

        //ControleNavigateur ctrlNav = new ControleNavigateur();
        public static string LoadNav(string nav)
        {
            string url = "";
            //MessageBox.Show(" je suis la méthode LoadNav ");
            Process[] procsNav = Process.GetProcessesByName(nav);

            if(nav.Equals("internet explorer"))
            {
                foreach (Process proc in procsNav)
                {
                    url = ControleNavigateur.GetInternetExplorerUrl(proc);

                    if (url == null)
                    {
                        continue;
                    }
                    else
                    {
                        return url;
                    }
                }
            }else if (nav.Equals("msedge"))
            {
                foreach (Process proc in procsNav)
                {
                    url = ControleNavigateur.GetFirefoxUrl(proc);

                    if (url == null)
                    {
                        continue;
                    }
                    else
                    {
                        //MessageBox.Show("URL : " + url);
                        return url;
                    }
                }
            }else if(nav.Equals("firefox"))
            {
                foreach (Process proc in procsNav)
                {
                    url = ControleNavigateur.GetFirefoxUrl(proc);

                    if (url == null)
                    {
                        continue;
                    }
                    else
                    {
                        //MessageBox.Show("URL : " + url);
                        return url;
                    }
                }
            }else if (nav.Equals("chrome"))
            {
                foreach (Process proc in procsNav)
                {
                    url = ControleNavigateur.GetFirefoxUrl(proc);

                    if (url == null)
                    {
                        continue;
                    }
                    else
                    {
                        //MessageBox.Show("URL : " + url);
                        return url;
                    }
                }
            }
            else if (nav.Equals("opera"))
            {
                /*foreach (Process proc in procsNav)
                {
                    url = ControleNavigateur.GetFirefoxUrl(proc);

                    if (url == null)
                    {
                        continue;
                    }
                    else
                    {
                        MessageBox.Show("URL : " + url);
                        return url;
                    }
                }*/
            }
            return (url);
        }

        // comparer l'url courant avec la base de donnée virale, puis ajouter à la base de données objectional website puis tu affiche un message de sécurité

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            string url = CheckUrl(ChechNavCurren());

            Boolean verif = db.CreateTable(sourceFile3, "websites");
            if (verif == true)
            {
                MessageBox.Show("La table website a bien été créer ");
            }
            else
            {
                MessageBox.Show("la table website existe deja ");

                // 1 - on traite l'url obtenue

                string url1 = "";
                if (url != null)
                {
                    try
                    {
                        Uri uri = new Uri(url);
                        url1 = uri.Host;
                        string url3 = "http://" + url1;
                        DateTime Date = DateTime.Now;
                        Date.ToString("dd/MM/yyyy HH:mm:ss");

                        string statut = "website visited";
                        MessageBox.Show("url : " + url3);

                        // 2 - on vérifi si l'url traité est bien dans la base de données viral
                        try
                        {
                            var md5signatures = File.ReadAllLines(BD_Virale);
                            //MessageBox.Show("Etape1 validé ");
                            if (md5signatures.Contains(url3.Trim()))
                            {
                                //MessageBox.Show("Etape2 validé ");
                                // 3 - si c'est le cas on enrégistre le fichier dans la base de données website
                                try
                                {
                                    string sql = "insert into websites (url, statut, date) values(";
                                    //sql = sql + "'" + index + "', ";                        
                                    sql = sql + "'" + url3 + "', ";
                                    sql = sql + "'" + statut + "', ";
                                    sql = sql + "'" + Date + "')";

                                    try
                                    {
                                        if (db.insertData(sourceFile, sql) == true)
                                        {
                                            // si appres plusieurs tentative on veux blocqué le site, on fait ça ici.
                                            MessageBox.Show("site enrégistré dans la table website ");
                                        }
                                        else
                                        {
                                            MessageBox.Show("site pas enregistré");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show(ex.Message);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("il ne s'agit pas d'un navigateur au premier plan");
                    }

                }
                else
                {
                    // rien à faire
                }
            }
                    
            //MessageBox.Show("C'est ça ? : "+ url3);
        }

        // la minuterie du scan automatique

        public void InitTimer()
        {
            Timer time = new Timer();
            time.Tick += new EventHandler(timer1_Tick);
            time.Interval = 9000; // in miliseconds 1080000 pour 30min, 7200000 pour 2h
            time.Start();
        }
    }
}
