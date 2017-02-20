using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Signal_viewer.Util;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;


namespace Signal_viewer
{
    public partial class Form1 : Form
    {
        private System.Drawing.Pen penRaw = new System.Drawing.Pen(Color.Gray, 1F);
        private System.Drawing.Pen penFir = new System.Drawing.Pen(Color.Orange, 1.5F);
        static  DataTable table;
        private List<int> sizePara = new List<int>();
        
        public Form1()
        {
            InitializeComponent();
            SaveFormSize();
            table = new DataTable("Mic");
            ListBox1_ShowFilesInFolder(Properties.Settings.Default.defaultFolderPath);  
        }

        private void SaveFormSize()
        {
            sizePara.Add(this.Width);       //0
            sizePara.Add(this.Height);      //1
            sizePara.Add(panel1.Width);     //2
            sizePara.Add(panel1.Height);    //3
            sizePara.Add(listBox1.Width);   //4
            sizePara.Add(listBox1.Height);  //5
            sizePara.Add(dataGridView1.Width);//6
            sizePara.Add(dataGridView1.Height);//7
            sizePara.Add(chart1.Width);//8
            sizePara.Add(chart1.Height);//9
        }


        private void AutoFormSize(object sender, EventArgs e)
        {
            float scaleW,scaleH;
            try
            {
                scaleW = (float)this.Width / sizePara[0];
                scaleH = (float)this.Height / sizePara[1];
            }
            catch { return; }
            
            //panel1           
            panel1.Width = (int)(scaleW * sizePara[2]);
            panel1.Height = (int)(scaleH * sizePara[3]);
            
            //listbox           
            listBox1.Width = (int)(scaleW * sizePara[4]);
            listBox1.Height = panel1.Height;   
         
            //dataGridView1
            dataGridView1.Width = panel1.Width - 3 - listBox1.Width;
            //dataGridView1.Height = panel1.Height;  
            

            //chart1
            chart1.Width = (int)(scaleW * sizePara[8]);
            chart1.Height = (int)(scaleH * sizePara[9]);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = FileIO.SelectFolder(Properties.Settings.Default.defaultFolderPath);
            if (fbd != null)
            {
                Properties.Settings.Default.defaultFolderPath = fbd.SelectedPath;
                Properties.Settings.Default.Save(); //保存最後開啟的FOLDER設定值

                ListBox1_ShowFilesInFolder(fbd.SelectedPath);                
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = FileIO.SelectFileToLoad("CSV Files|*.csv|All files|*.*");
            if (openFileDialog != null)
            {
                string filename = openFileDialog.FileName;
                table = csvTool.CsvToDataTable(filename, "temp", ",");
                dataGridView1.DataSource = table;

            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
         
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {           
            if(table != null)
            {
                //chart1.Series["RawData"].XValueMember = "RAWDATA";
                chart1.Series["RawData"].YValueMembers = "RAWDATA";
                //chart1.Series["FirData"].XValueMember = "FILTERDATA";
                chart1.Series["FirData"].YValueMembers = "FILTERDATA";
                chart1.DataSource = table;
                chart1.DataBind();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void ListBox1_ShowFilesInFolder(string path)
        {
            listBox1.Items.Clear();
            if (Directory.Exists(path))
            {
                string[] filenames = Directory.GetFiles(path);
                foreach (string n in filenames)
                {
                    string s = n.ToLower();
                    if (s.Contains(".csv"))
                        listBox1.Items.Add(n);
                }
            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string file = listBox1.SelectedItem.ToString();
                LoadFileToTable(file);
            }
            catch { }            
        }

        private void LoadFileToTable(string filename)
        {
            table = csvTool.CsvToDataTable(filename, "temp", ",");
            dataGridView1.DataSource = table;
            DrawChart();
        }

        private void DrawChart()
        {
            if (table != null)
            {
                //chart1.Series["RawData"].XValueMember = "RAWDATA";
                chart1.Series["RawData"].YValueMembers = "RAWDATA";
                //chart1.Series["FirData"].XValueMember = "FILTERDATA";
                chart1.Series["FirData"].YValueMembers = "FILTERDATA";
                chart1.DataSource = table;
                chart1.DataBind();
            }
        }
    }
}
