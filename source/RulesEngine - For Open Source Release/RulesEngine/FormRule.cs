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
    public partial class FormRule : Form
    {
        public FormRule()
        {
            InitializeComponent();
        }

        private void FormRule_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < GlobalVars.arrayRules.GetLength(0); i++)
            {
                comboBoxRules.Items.Add(GlobalVars.arrayRules[i, 1]);
            }
            if (comboBoxRules.Items.Count > 0)
            {
                comboBoxRules.SelectedIndex = -1;
            }
            //Populate the Orderable Items
            int numitems = GlobalVars.arrayOrderables.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                comboBoxOrderableItem.Items.Add(GlobalVars.arrayOrderables[i,1]);                
            }

        }

        private void comboBoxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelCalendarDate.Visible = false;
            labelDateofYear.Visible = false;
            labelDayofMonth.Visible = false;
            labelDayofWeek.Visible = false;
            labelDays.Visible = false;
            labelHours.Visible = false;
            dateTimePickerCalendarDate.Visible = false;
            dateTimePickerCalendarDate.Text = "";
            comboBoxDayofWeek.Visible = false;
            comboBoxDayofWeek.SelectedIndex = -1;
            comboBoxDayofMonth.Visible = false;
            comboBoxDayofMonth.SelectedIndex = -1;
            comboBoxOrderableItem.Visible = false;
            comboBoxOrderableItem.SelectedIndex = -1;
            labelOrderableItem.Visible = false;
            textBoxHours.Visible = false;
            textBoxHours.Text = "";
            textBoxDays.Visible = false;
            textBoxDays.Text = "";
            textBoxDateofYear.Visible = false;
            textBoxDateofYear.Text = "";
            labelOrderableItem.Visible = false;
            comboBoxOrderableItem.Visible = false;

            if (comboBoxRules.Text.Contains("X DAYS"))
            {
                labelDays.Visible = true;
                textBoxDays.Visible = true;
                labelDays.Left = 40;
                labelDays.Top = 90;
                textBoxDays.Left = 43;
                textBoxDays.Top = 107;
            }
            if (comboBoxRules.Text.Contains("X HOURS"))
            {
                labelHours.Visible = true;
                textBoxHours.Visible = true;
                labelHours.Left = 40;
                labelHours.Top = 90;
                textBoxHours.Left = 43;
                textBoxHours.Top = 107;
            }
            if (comboBoxRules.Text.Contains("CALENDAR DATE"))
            {
                labelCalendarDate.Visible = true;
                dateTimePickerCalendarDate.Visible = true;
                labelCalendarDate.Left = 40;
                labelCalendarDate.Top = 90;
                dateTimePickerCalendarDate.Left = 43;
                dateTimePickerCalendarDate.Top = 107;
                dateTimePickerCalendarDate.Format = DateTimePickerFormat.Custom;
                dateTimePickerCalendarDate.CustomFormat = " ";
            }
            if (comboBoxRules.Text.Contains("DATE OF YEAR"))
            {
                labelDateofYear.Visible = true;
                textBoxDateofYear.Visible = true;
                labelDateofYear.Left = 40;
                labelDateofYear.Top = 90;
                textBoxDateofYear.Left = 43;
                textBoxDateofYear.Top = 107;
            }
            if (comboBoxRules.Text.Contains("DATE OF MONTH"))
            {
                labelDayofMonth.Visible = true;
                comboBoxDayofMonth.Visible = true;
                labelDayofMonth.Left = 40;
                labelDayofMonth.Top = 90;
                comboBoxDayofMonth.Left = 43;
                comboBoxDayofMonth.Top = 107;
            }
            if (comboBoxRules.Text.Contains("DAY OF WEEK"))
            {
                labelDayofWeek.Visible = true;
                comboBoxDayofWeek.Visible = true;
                labelDayofWeek.Left = 40;
                labelDayofWeek.Top = 90;
                comboBoxDayofWeek.Left = 43;
                comboBoxDayofWeek.Top = 107;
            }
            if (comboBoxRules.Text.Contains("EVENT DATE"))
            {
                labelOrderableItem.Visible = true;
                comboBoxOrderableItem.Visible = true;
            }
            else
            {
                labelOrderableItem.Visible = false;
                comboBoxOrderableItem.Visible = false;
            }
            if (comboBoxRules.Text.Contains("PASS-THROUGH REMINDER"))
            {
                //need to populate the combobox here
                comboBoxSelectReminder.DisplayMember = "itemtext";
                comboBoxSelectReminder.ValueMember = "itemien";
                comboBoxSelectReminder.Items.Clear();
                string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                string res3 = GlobalVars.dc.CallRPC("C9C GET PASS-THROUGH REMINDERS", NO_ARG);
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
                            comboBoxSelectReminder.Items.Add(thisitem);
                        }
                    }
                    comboBoxSelectReminder.SelectedIndex = -1;
                }
                labelSelectReminder.Visible = true;
                comboBoxSelectReminder.Visible = true;
            }
            else
            {
                labelSelectReminder.Visible = false;
                comboBoxSelectReminder.Visible = false;
            }

        }

        private void dateTimePickerCalendarDate_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerCalendarDate.Format = DateTimePickerFormat.Short;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxRules.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a Rule!");
                return;
            }

            frmEditRules owningForm = (frmEditRules)this.Owner;           

            int numrules = owningForm.listBoxWardRulesSelectedItems.Count;            
            
            string res5 = "0";
            string ptrRuleIEN = GlobalVars.arrayRules[comboBoxRules.SelectedIndex, 0];
            string argXDAYSfromAdmit = "";
            string argXHOURSfromAdmit = "";
            string argXDAYSfromEvent = "";
            string argXHOURSfromEvent = "";
            string argCALENDARDATE = "";
            string argDATEOFYEAR = "";
            string argDATEOFMONTH = "";
            string argDAYOFWEEK = "";
            string argOrderableItem = "";
            string argPassThroughIEN = "";

            if (ptrRuleIEN == "")
            {
                MessageBox.Show("Invalid Rule Pointer");
                return;
            }

            if ((comboBoxRules.Text.Contains("X DAYS")) && ((comboBoxRules.Text.Contains("ADMIT DATE"))
                || (comboBoxRules.Text.Contains("ADMISSION DAY"))))
            {
                int j;
                if (Int32.TryParse(textBoxDays.Text, out j))
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, j, "", "", "", "", 0, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argXDAYSfromAdmit = textBoxDays.Text;
                }                
            }
            else if ((comboBoxRules.Text.Contains("X DAYS")) && (comboBoxRules.Text.Contains("EVENT DATE")))
            {
                if (comboBoxOrderableItem.Text == "")
                {
                    MessageBox.Show("You must select an orderable item as an Event for this rule!");
                    return;
                }
                int j;
                if (Int32.TryParse(textBoxDays.Text, out j))
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, "", "", "", "", 0, j, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argXDAYSfromEvent = textBoxDays.Text;
                }
            }
            else if ((comboBoxRules.Text.Contains("X HOURS")) && ((comboBoxRules.Text.Contains("ADMIT DATE"))
                || (comboBoxRules.Text.Contains("ADMISSION DAY"))))
            {
                int j;
                if (Int32.TryParse(textBoxHours.Text, out j))
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text,0 , "", "", "", "", j, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argXHOURSfromAdmit = textBoxHours.Text;
                }                
            }
            else if ((comboBoxRules.Text.Contains("X HOURS")) && (comboBoxRules.Text.Contains("EVENT DATE")))
            {
                if (comboBoxOrderableItem.Text == "")
                {
                    MessageBox.Show("You must select an orderable item as an Event for this rule!");
                    return;
                }
                int j;
                if (Int32.TryParse(textBoxHours.Text, out j))
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, "", "", "", "", 0, 0, j, "");
                    textBoxRuleText.Text = convertedrule;
                    argXHOURSfromEvent = textBoxHours.Text;
                }
            }
            else if (comboBoxRules.Text.Contains("CALENDAR DATE"))
            {
                string caldate = dateTimePickerCalendarDate.Text;
                if (caldate != "")
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, caldate, "", "", "", 0, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argCALENDARDATE = caldate;
                }                
            }
            else if (comboBoxRules.Text.Contains("DATE OF YEAR"))
            {
                string dateofyear = textBoxDateofYear.Text;
                if (dateofyear != "")
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, "", dateofyear, "", "", 0, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argDATEOFYEAR = dateofyear;
                }                
            }
            else if (comboBoxRules.Text.Contains("DATE OF MONTH"))
            {
                string dateofmonth = comboBoxDayofMonth.Text;
                if (dateofmonth != "")
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, "", "", dateofmonth, "", 0, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argDATEOFMONTH = dateofmonth;
                }                
            }
            else if (comboBoxRules.Text.Contains("DAY OF WEEK"))
            {
                string dayofweek = comboBoxDayofWeek.Text;
                if (dayofweek != "")
                {
                    string convertedrule = frmEditRules.convertRuleText(comboBoxRules.Text, 0, "", "", "", dayofweek, 0, 0, 0, "");
                    textBoxRuleText.Text = convertedrule;
                    argDAYOFWEEK = dayofweek;
                }                
            }
            else if (comboBoxRules.Text.Contains("PASS-THROUGH REMINDER"))
            {
                if (comboBoxSelectReminder.Text == "")
                {
                    MessageBox.Show("You must select a pass-through reminder for this rule!");
                    return;
                }
                if (comboBoxSelectReminder.SelectedIndex > -1)
                {                    
                    string reminderien = ((GlobalVars.LType)comboBoxSelectReminder.SelectedItem).itemien.ToString();
                    textBoxRuleText.Text = "PASS-THROUGH REMINDER IEN=" + reminderien;
                    argPassThroughIEN = reminderien;
                }
            }
            else
            {
                textBoxRuleText.Text = comboBoxRules.Text;
            }
            if (comboBoxOrderableItem.Text != "")
            {
                int oindex = comboBoxOrderableItem.SelectedIndex;
                if ((oindex > -1) && (oindex < GlobalVars.arrayOrderables.GetLength(0)))
                {
                    argOrderableItem = GlobalVars.arrayOrderables[oindex,0]; 
                    textBoxRuleText.Text = textBoxRuleText.Text + " WITH ACTIVE ORDERABLE ITEM " +
                                           comboBoxOrderableItem.Text;
                }
            }
            
            String Cohort = "";
            String FormulaIENstring = "";
            if (this.Text=="Add New Ward Rule")
            {
                Cohort = "WARD";
                FormulaIENstring = GlobalVars.wardcurrentFormulaIENstring;
            }
            else if (this.Text == "Add New Unit Rule")
            {
                Cohort = "UNIT";
                FormulaIENstring = GlobalVars.unitcurrentFormulaIENstring;
            }
            else if (this.Text == "Add New Hospital Rule")
            {
                Cohort = "HOSPITAL";
                FormulaIENstring = GlobalVars.hospitalcurrentFormulaIENstring;
            }


            if ((FormulaIENstring != "") && (ptrRuleIEN != ""))
            {
                string[] args = { Cohort,ptrRuleIEN, FormulaIENstring, argXDAYSfromAdmit,
                                  argXHOURSfromAdmit, argCALENDARDATE, argDATEOFYEAR, argDATEOFMONTH,
                                  argDAYOFWEEK, argXDAYSfromEvent, argXHOURSfromEvent, argOrderableItem, argPassThroughIEN};
                res5 = GlobalVars.dc.CallRPC("C9C ADD RULE TO FORMULA", RpcFormatter.FormatArgs(true, args));
            }
            if (res5 == "1")
            {
                MessageBox.Show("Successfully Added Rule!");
                this.DialogResult = DialogResult.OK;
                //then the rules will be refreshed on the main form
            }
            else
            {
                MessageBox.Show("Error Updating Rules For This Formula!");
            }
        }

        private void FormRule_Load(object sender, EventArgs e)
        {

        }
    }
}
