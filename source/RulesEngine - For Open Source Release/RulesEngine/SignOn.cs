//Dashboard and Rules Engine
//Copyright [2020] [Central Regional Hospital, State of North Carolina]
// 
//Licensed under the Apache License, Version 2.0 (the "License")
//you may not use this file except in ;compliance with the License.
//You may obtain a copy of the License at
// 
//http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is ;distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Windows.Forms;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;


namespace RulesEngine
{
    public partial class SignOn : Form
    {
        private string name;        
  
        public SignOn()
        {
            InitializeComponent();
        }

        private void SetDefault(Button myDefaultBtn)
        {
            this.AcceptButton = myDefaultBtn;
        }

        private void SignOn_Load(object sender, EventArgs e)
        {
           
            labeluname.Text = "Access Code";
            labelpword.Text = "Verify Code";
            txtuname.PasswordChar = '*';
            txtuname.Text = "";
            txtuname.TabStop = true;
            txtuname.Enabled = true;
            txtuname.Focus();
        }

        private void btnConnect_Click(object sender, System.EventArgs e)
        {
            LstDivisions.Items.Clear();
            if (String.IsNullOrEmpty(txtpword.Text))
            {
                MessageBox.Show("Invalid Username/Password");
                return;
            }

            if (String.IsNullOrEmpty(txtuname.Text))
            {
                MessageBox.Show("Invalid Username/Password");
                return;
            }

            string pw = txtpword.Text;
            string uname = txtuname.Text;            

            
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.GetLength(0) < 2)
            {
                MessageBox.Show("Need Command Line Aruguments for Server and Port!");
                this.Close();
            }
            int port=0;
            string server="None";
            string thisarg = "";
            for (int iii = 0; iii < arguments.GetLength(0); iii++)
            {
                thisarg = arguments[iii].ToUpper();
                if (thisarg.StartsWith("S="))
                {
                    if (thisarg.Length > 8)
                    {
                        server = thisarg.Substring(2);
                    }
                }
                if (thisarg.StartsWith("P="))
                {
                    if (thisarg.Length > 2)
                    {
                        port = Convert.ToInt32(thisarg.Substring(2));
                    }
                }
            }
            if (port == 0)
            {
                MessageBox.Show("Invalid Port");
                this.Close();
            }
            if (server == "None")
            {
                MessageBox.Show("Invalid Server");
                this.Close();
            }
            GlobalVars.dc = new DataConnection("AD_CONNECT", server, port, true);
            string AV_ARG = "";
            string[] results;
            
            AV_ARG = RpcFormatter.FormatArgs(true, uname);
            string res = GlobalVars.dc.CallRPC("XUS SIGNON SETUP", AV_ARG);
            results = Common.Split(res);

            AV_ARG = RpcFormatter.FormatArgs(true, Hash.Encrypt(uname + ';' + pw));
            string resac = GlobalVars.dc.CallRPC("XUS AV CODE", AV_ARG);
            results = Common.Split(resac);
            if (results[0].ToString() == "0")
            {
                MessageBox.Show("Invalid Access/Verify Code");
                this.Close();
            }

            try
            {
                LstDivisions.Items.Clear();

                string context = "C9C RULES ENGINE"; //could use OR CPRS GUI CHART if you want, but register all rpcs to the option!
                string res2 = GlobalVars.dc.CallRPC("XWB CREATE CONTEXT",RpcFormatter.FormatArgs(true, Hash.Encrypt(context)));
                results = Common.Split(res2);

                string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                string res3 = GlobalVars.dc.CallRPC("XUS GET USER INFO", NO_ARG);
                results = Common.Split(res3);
                int DUZ = 0;
                GlobalVars.SelectedDivision = "None";
                Int32.TryParse(results[0], out DUZ);
                if (DUZ > 0)
                {   
                    AV_ARG = RpcFormatter.FormatArgs(true, DUZ.ToString());
                    string res4 = GlobalVars.dc.CallRPC("C9C GET DIVISIONS", AV_ARG);
                    results = Common.Split(res4);
                    if (results.GetLength(0) > 0)
                    {
                        int NumDivs = 0;
                        Int32.TryParse(results[0], out NumDivs);
                        if (NumDivs > 1)
                        {
                            txtuname.Visible = false;
                            txtpword.Visible = false;
                            btnConnect.Visible = false;
                            labeluname.Visible = false;
                            labelpword.Visible = false;
                            LstDivisions.Visible = true;
                            btnSelect.Visible = true;
                            SetDefault(btnSelect);
                            LstDivisions.Focus();
                            LstDivisions.Items.Clear();
                            char[] charSeparators = new char[] { '^' };
                            string[] splitresults;
                            int defaultDiv = 0;

                            for (int i = 1; i < results.GetLength(0); i++)
                            {                                
                                LstDivisions.Items.Add(results[i].Split('^')[1]);
                                splitresults = results[i].Split(charSeparators, 4, StringSplitOptions.None);
                                if (splitresults.Length > 3)
                                {
                                    if (String.IsNullOrEmpty(splitresults[3]))
                                    {
                                    }
                                    else if (splitresults[3] == "1")
                                    {
                                        defaultDiv = i - 1;
                                    }
                                }
                            }
                            LstDivisions.SelectedIndex = defaultDiv;

                        }
                        else if (NumDivs == 1)
                        {
                            GlobalVars.SelectedDivision = results[1].Split('^')[1].ToString();
                            AV_ARG = RpcFormatter.FormatArgs(true, GlobalVars.SelectedDivision);
                            string res5 = GlobalVars.dc.CallRPC("XUS DIVISION SET", AV_ARG);
                            results = Common.Split(res5);
 
                            if (results[0] == "1")
                            {
                                //Logged in and Division Set
                                frmEditRules formMain = new frmEditRules();
                                this.Hide();
                                formMain.ShowDialog();
                                GlobalVars.dc.Close();
                                this.Close();
                            }
                            else
                            {
                                GlobalVars.dc.Close();
                            }

                        }
                        else if (NumDivs == 0)
                        {
                            MessageBox.Show("No Divisions");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Divisions!");
                    }

                }
                else
                {
                    MessageBox.Show("Invalid DUZ");
                }
 
   
             }
            catch
            {
                MessageBox.Show("No Divisions or you don't have the secondary menu option C9C RULES ENGINE!");
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
  
            GlobalVars.SelectedDivision = LstDivisions.SelectedItem.ToString();
            if (GlobalVars.SelectedDivision == "None")
            {
                MessageBox.Show("No Division Selected");
                GlobalVars.dc.Close();
            }
            else
            {
                string AV_ARG = RpcFormatter.FormatArgs(true, GlobalVars.SelectedDivision);
                string res4 = GlobalVars.dc.CallRPC("XUS DIVISION SET", AV_ARG);
                string[] results = Common.Split(res4);
     
                if (results[0] == "1")
                {
                    //Logged in and Division Set
                    frmEditRules formER = new frmEditRules();
                    this.Hide();
                    formER.ShowDialog();
                    GlobalVars.dc.Close();
                    this.Close();
                }
                else
                {
                    GlobalVars.dc.Close();
                }
            }
        }
        
    }
}


