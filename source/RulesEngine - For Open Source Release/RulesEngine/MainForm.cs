using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;
using System.IO;
using System.Drawing.Printing;

namespace VistA_Import_PDF_Tool
{
    
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res3 = GlobalVars.dc.CallRPC("XUS INTRO MSG", NO_ARG);
            string[] results = Common.Split(res3);
            if (results.Length > 0)
            {
                this.Text = results[0].Trim();
            }

      
            string RPC_ARG = RpcFormatter.FormatArgs(true, "PROVIDER");
            string reskey = GlobalVars.RunRPC("ORWU HASKEY", RPC_ARG);
            results = Common.Split(reskey);
            GlobalVars.hasSEEkey = 0;
            Int32.TryParse(results[0], out GlobalVars.hasSEEkey);
            if (GlobalVars.hasSEEkey <= 0)
            {
                RPC_ARG = RpcFormatter.FormatArgs(true, "C9C 42CFR OVERRIDE");
                reskey = GlobalVars.RunRPC("ORWU HASKEY", RPC_ARG);
                results = Common.Split(reskey);
                GlobalVars.hasSEEkey = 0;
                Int32.TryParse(results[0], out GlobalVars.hasSEEkey);
            }
            textAuthor.Text = GlobalVars.strAuthor;
            textAuthorIEN.Text = GlobalVars.strDUZ;
            res3 = GlobalVars.dc.CallRPC("C9C GET TITLES", NO_ARG);
            results = Common.Split(res3);
            if (results.Length > 0)
            {
   
                char[] charSeparators = new char[] { '^' };
                string[] splitresults;
                GlobalVars.arrayTitles = new string[results.GetLength(0), 2];
                for (int i = 0; i < results.GetLength(0); i++)
                {
                    splitresults = results[i].Split(charSeparators, 2, StringSplitOptions.None);
                    if (splitresults.Length > 1)
                    {
                        comboSelect.Items.Add(results[i].Split('^')[1]);
                        GlobalVars.arrayTitles[i, 0] = results[i].Split('^')[0];
                        GlobalVars.arrayTitles[i, 1] = results[i].Split('^')[1];
                        
                    }
                }
                comboSelect.SelectedIndex = 0;

            }
        }

 

 
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

  
 
        private void buttonSelectPDF_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            webBrowser1.Navigate(openFileDialog1.FileName);
        }

        private void comboSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            textTitleIEN.Text = GlobalVars.arrayTitles[comboSelect.SelectedIndex, 0];
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            comboPatients.Items.Clear();
            comboPatients.Text = "";
            textIEN.Text = "";
            textDOB.Text = "";
            textName.Text = "";
            textMRN.Text = "";
            textWard.Text = "";
            textRoomBed.Text = "";

            if (txtSearch.Text.Length > 1)
            {
  
                string[] args3 = { txtSearch.Text };
                string res3 = GlobalVars.RunRPC("ORWPT ENHANCED PATLOOKUP", RpcFormatter.FormatArgs(true, args3));
                string[] results = Common.Split(res3);
                char[] charSeparators = new char[] { '^' };
                string[] splitresults;


                if (results.GetLength(0) > 0)
                {

                    GlobalVars.arrayPatients = new string[results.GetLength(0), 4];

                    for (int i = 0; i < results.GetLength(0); i++)
                    {
                        splitresults = results[i].Split(charSeparators, 5, StringSplitOptions.None);
                        if (splitresults.Length > 4)
                        {
                            comboPatients.Items.Add(results[i].Split('^')[1]);
                            GlobalVars.arrayPatients[i, 0] = results[i].Split('^')[0];
                            GlobalVars.arrayPatients[i, 1] = results[i].Split('^')[1];
                            GlobalVars.arrayPatients[i, 2] = (results[i].Split('^')[2]).Split('\t')[0];
                            GlobalVars.arrayPatients[i, 3] = results[i].Split('^')[3];

                        }
                    }
                    comboPatients.SelectedIndex = 0;
                    comboPatients.Focus();
                }

            }
            else
            {
                MessageBox.Show("Enter at least two characters!");
            }
        }

        private void comboPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            textIEN.Text = "";
            textMRN.Text = "";
            textName.Text = "";
            textDOB.Text = "";
            textWard.Text = "";
            textRoomBed.Text = "";
            textIEN.Text = GlobalVars.arrayPatients[comboPatients.SelectedIndex, 0];
            textName.Text = GlobalVars.arrayPatients[comboPatients.SelectedIndex, 1];
            textDOB.Text = GlobalVars.arrayPatients[comboPatients.SelectedIndex, 2];
            textMRN.Text = GlobalVars.arrayPatients[comboPatients.SelectedIndex, 3];
            string[] args5 = { textIEN.Text };
            //string res5 = GlobalVars.dc.CallRPC("C9C WARD-ROOMBED", RpcFormatter.FormatArgs(true, args5));
            string res5 = GlobalVars.RunRPC("C9C WARD-ROOMBED", RpcFormatter.FormatArgs(true, args5));
            string[] results = Common.Split(res5);
 
            if (results.GetLength(0) > 0)
            {
                textWard.Text = results[0];
            }
            if (results.GetLength(0) > 1)
            {
                textRoomBed.Text = results[1];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textTitleIEN.Text == "")
            {
                MessageBox.Show("You must select a valid Note Title!");
                return;
            }
            if (textIEN.Text == "")
            {
                MessageBox.Show("You must select a patient!");
                return;
            }
            if (openFileDialog1.FileName == "")
            {
                MessageBox.Show("You must select a PDF file!");
                return;
            }
            string subject = "PDF IMPORTED DOCUMENT";
            string TextLine1 = "This note references an imported PDF document.";
            string TextLine2 = "Please see attached PDF!";
            string[] args1 = { textIEN.Text, textTitleIEN.Text, subject, TextLine1, TextLine2 };
                            
            
            string res1 = GlobalVars.RunRPC("C9C TIU CREATE RECORD", RpcFormatter.FormatArgs(true, args1));
            string[] results = Common.Split(res1);
            if (results.Length > 0)
            {
                //MessageBox.Show(results[0]);
                if (Convert.ToInt32(results[0]) > 0)
                {
                    string strNoteIEN = results[0];
                 
                    res1 = GlobalVars.RunRPC("C9C MAG4 ADD IMAGE", RpcFormatter.FormatArgs(true, textIEN.Text));
                    results=Common.Split(res1);
                    //MessageBox.Show(results[0]);
                    if (results.Length > 0)
                    {
                        string ienfname = results[0];
                        string imageIEN = ienfname.Split('^')[0];
                        string imagePath = ienfname.Split('^')[1];
                        string imageFName = ienfname.Split('^')[2];
                        //Now let's copy the PDF file to the repository

                        // To copy a file to another location and  
                        // overwrite the destination file if it already exists.
                        System.IO.File.Copy(openFileDialog1.FileName, imagePath + imageFName, true);
                        if (System.IO.File.Exists(imagePath + imageFName))
                        {
                            //Now add the proper indexes
                            string[] args7 = { imageIEN, strNoteIEN };
                            res1 = GlobalVars.RunRPC("MAG3 TIU IMAGE", RpcFormatter.FormatArgs(true, args7));
                            results = Common.Split(res1);
                            if (results.Length > 0)
                            {
                                if (results[0].Split('^')[0] == "0")
                                {
                                    MessageBox.Show("Error Creating Indexes!");
                                }
                                else
                                {
                                    MessageBox.Show("PDF File Successfully Attached to Note!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error Creating Indexes!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Error Copying PDF File to Repository!");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Note Creation Unsuccessful!");
                    return;
                }
                
            }
            
            
            openFileDialog1.FileName = "";
       
            webBrowser1.Hide();
            webBrowser1.Navigate("about:blank");
 
            do
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            } while (webBrowser1.ReadyState != WebBrowserReadyState.Complete);
            webBrowser1.Show();

        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSearch.PerformClick();
            }

        }

        private void comboPatients_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboSelect.Focus();
            }
        }

        private void comboSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSelectPDF.PerformClick();
            }
        }

  
 }
}
