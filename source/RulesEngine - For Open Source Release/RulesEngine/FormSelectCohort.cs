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
    public partial class FormSelectCohort : Form
    {
        public FormSelectCohort()
        {
            InitializeComponent();
        }

        private void FormSelectCohort_Load(object sender, EventArgs e)
        {
            //Populate the Reminders Combobox
            comboBoxSelectCohort.DisplayMember = "itemtext";
            comboBoxSelectCohort.ValueMember = "itemien";
            comboBoxSelectCohort.Items.Clear();
            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res3 = GlobalVars.dc.CallRPC("C9C GET DASHBOARD REMINDERS", NO_ARG);
            string[] results = Common.Split(res3);
            if (results.Length > 0)
            {

                char[] charSeparators = new char[] { '^' };
                string[] splitresults;                
                for (int i = 0; i < results.GetLength(0); i++)
                {
                    splitresults = results[i].Split(charSeparators, 2, StringSplitOptions.None);
                    if (splitresults.Length > 1)
                    {
                        var thisitem = new GlobalVars.LType { itemtext = splitresults[0], itemien = splitresults[1] };
                        comboBoxSelectCohort.Items.Add(thisitem);                        
                    }
                }
                comboBoxSelectCohort.SelectedIndex = -1;
                if ((GlobalVars.chosenCohort != null) && (GlobalVars.chosenCohort.itemien != null) && 
                    (GlobalVars.chosenCohort.itemtext != null))
                {
                    comboBoxSelectCohort.SelectedValue = GlobalVars.chosenCohort.itemien;
                    comboBoxSelectCohort.Text = GlobalVars.chosenCohort.itemtext;
                    if ((GlobalVars.PTOnly == 1))
                    {
                        checkBoxPTOnly.Checked = true;
                    }
                    else
                    {
                        checkBoxPTOnly.Checked = false;
                    }
                }
                else
                {
                    comboBoxSelectCohort.SelectedIndex = -1;
                    checkBoxPTOnly.Checked = false;
                }

            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {                        
            if (GlobalVars.chosenTitleIEN != "")
            {
                int k = comboBoxSelectCohort.SelectedIndex;
                
                string AV_ARG = "";
                string thisvptr = GlobalVars.chosenTitleIEN + ";TIU(8925.1,";
                string rcIEN = "";
                
                if (k > -1)
                {
                    rcIEN = ((GlobalVars.LType)comboBoxSelectCohort.SelectedItem).itemien.ToString();
                }
                string PTOnly = "0";
                if (checkBoxPTOnly.Checked)
                {
                    PTOnly = "1";
                }
                AV_ARG = RpcFormatter.FormatArgs(true, thisvptr, rcIEN, PTOnly);
                

                string resSet = GlobalVars.dc.CallRPC("C9C SET COHORT FOR TITLE", AV_ARG);
                if (resSet == "1")
                {
                    if (k > -1)
                    {
                        GlobalVars.chosenCohort.itemien = ((GlobalVars.LType)comboBoxSelectCohort.SelectedItem).itemien;
                        GlobalVars.chosenCohort.itemtext = ((GlobalVars.LType)comboBoxSelectCohort.SelectedItem).itemtext;
                        if (PTOnly == "1")
                        {
                            GlobalVars.PTOnly = 1;
                        }
                        else
                        {
                            GlobalVars.PTOnly = 0;
                        }

                    }
                    else
                    {
                        GlobalVars.chosenCohort.itemien = "";
                        GlobalVars.chosenCohort.itemtext = "";
                        GlobalVars.PTOnly = 0;
                    }
                    
                } 
                else
                {
                    MessageBox.Show("There was an error saving the reminder cohort!");
                }
            }           

            this.DialogResult = DialogResult.OK;
        }
    }
}
