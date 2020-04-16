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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;

namespace RulesEngine
{
    
    public partial class frmEditRules : Form
    {
        
        public frmEditRules()
        {
            InitializeComponent();
        }

        public int listBoxWardFormulasSelectedIndex
        {
            get { return this.listBoxWardFormulas.SelectedIndex; }
        }

        public int listBoxWardRulesSelectedSelectedIndex
        {
            get { return this.listBoxWardRulesSelected.SelectedIndex; }
        }

        public ListBox.ObjectCollection listBoxWardRulesSelectedItems
        {
            get { return this.listBoxWardRulesSelected.Items; }
        }

        private void frmEditRules_Load(object sender, EventArgs e)
        {
            GlobalVars.chosenCohort = new GlobalVars.LType();
            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res3 = GlobalVars.dc.CallRPC("XUS INTRO MSG", NO_ARG);
            string[] results = Common.Split(res3);
            if (results.Length > 0)
            {
                this.Text = results[0].Trim();
            }

            //Get List of Orderable Items available to this program
            NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res14 = GlobalVars.RunRPC("C9C RULES ENGINE ORD ITEMS", NO_ARG);
            results = Common.Split(res14);
            if (results.GetLength(0) > 0)
            {
                int thisLen = results.GetLength(0);
                GlobalVars.arrayOrderables = new string[thisLen, 2];
                for (int i = 0; i < results.GetLength(0); i++)
                {
                    GlobalVars.arrayOrderables[i, 0] = results[i].Split('~')[0]; //IEN
                    GlobalVars.arrayOrderables[i, 1] = results[i].Split('~')[1]; //name
                }
            }
            else
            {
                GlobalVars.arrayOrderables = new string[0, 2];
            }

            //Get List of wards 
            NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            
            res3 = GlobalVars.RunRPC("C9C WARDS AND UNITS", NO_ARG);
            results = Common.Split(res3);

            if (results.GetLength(0) > 0)
            {
                int arrayLen = results.GetLength(0);
                GlobalVars.arrayWards = new string[arrayLen, 4];
                GlobalVars.listUniqueUnits = new List<string>();

                for (int i = 0; i < results.GetLength(0); i++)
                {
                    string wien = results[i].Split('^')[0];
                    string wname = results[i].Split('^')[1]; 
                    string winclude = "0";
                    string wunit = results[i].Split('^')[2]; //Unit
                    if ((wname != "")
                        && (wname.IndexOf("CERT NON-CERT") < 0)
                        && (wname.IndexOf("ON MEDICAL LEAVE") < 0)
                        && (wien != ""))
                    {
                        GlobalVars.arrayWards[i, 0] = wien;
                        GlobalVars.arrayWards[i, 1] = wname;
                        GlobalVars.arrayWards[i, 2] = winclude;
                        GlobalVars.arrayWards[i, 3] = wunit;
                        if (GlobalVars.listUniqueUnits.Contains(wunit))
                        {
                            //do nothing
                        }
                        else
                        {
                            GlobalVars.listUniqueUnits.Add(wunit);
                        }


                    }
                    else
                    {
                        GlobalVars.arrayWards[i, 0] = "";
                        GlobalVars.arrayWards[i, 1] = "";
                        GlobalVars.arrayWards[i, 2] = "";
                        GlobalVars.arrayWards[i, 3] = "";
                    }
                }
            }
            else
            {
                GlobalVars.arrayWards = new string[1, 3];
                GlobalVars.arrayWards[0, 0] = "";
                GlobalVars.arrayWards[0, 1] = "";
                GlobalVars.arrayWards[0, 2] = "";
                GlobalVars.arrayWards[0, 3] = "";
            }



            //Populate the Wards Lists
            int numitems = GlobalVars.arrayWards.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                {
                    listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
                else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                {
                    listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
            }
            //Populate the Units Lists
            numitems = GlobalVars.listUniqueUnits.Count;
            GlobalVars.arrayUnits = new string[numitems, 2];
            for (int i = 0; i < numitems; i++)
            {
                GlobalVars.arrayUnits[i, 0] = GlobalVars.listUniqueUnits[i];
                GlobalVars.arrayUnits[i, 1] = "0";
                listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
            }
            //Populate the Hosiptal Name
            textBoxHospitalName.Text = GlobalVars.SelectedDivision;

            //Populate the TIU Titles Combobox
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
                        comboSelectTitle.Items.Add(results[i].Split('^')[1]);
                        GlobalVars.arrayTitles[i, 0] = results[i].Split('^')[0];
                        GlobalVars.arrayTitles[i, 1] = results[i].Split('^')[1];

                    }
                }
                comboSelectTitle.SelectedIndex = 0;
                

            }

            //Populate rules array
            NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res9 = GlobalVars.dc.CallRPC("C9C GET RULE NAMES", NO_ARG);
            results = Common.Split(res9);
            if (results.Length > 0)
            {
                int howmanyrules = results.GetLength(0);
                GlobalVars.arrayRules = new string[howmanyrules, 2];
                for (int i = 0; i < howmanyrules; i++)
                {

                    string rien = results[i].Split('^')[0]; //RuleIEN
                    string ruletext = results[i].Split('^')[1]; //RuleText
                    if ((rien != "") && (ruletext != ""))
                    { 
                        GlobalVars.arrayRules[i, 0] = rien;
                        GlobalVars.arrayRules[i, 1] = ruletext;                     
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            GlobalVars.RunRPC("XOBV TEST PING", NO_ARG);
            timer1.Start();
        }

        private void buttonAllRight_Click(object sender, EventArgs e)
        {
            int numitems = GlobalVars.arrayWards.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                GlobalVars.arrayWards[i, 2] = "1";
            }
            listBoxIncludeWards.Items.Clear();
            listBoxDoNotIncludeWards.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                {
                    listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
                else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                {
                    listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
            }
        }

        private void buttonAllLeft_Click(object sender, EventArgs e)
        {
            int numitems = GlobalVars.arrayWards.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                GlobalVars.arrayWards[i, 2] = "0";
            }
            listBoxIncludeWards.Items.Clear();
            listBoxDoNotIncludeWards.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                {
                    listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
                else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                {
                    listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
            }
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxDoNotIncludeWards.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxDoNotIncludeWards.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            int numitems = GlobalVars.arrayWards.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                if (GlobalVars.arrayWards[i, 1] == itemText)
                {
                    GlobalVars.arrayWards[i, 2] = "1";
                }
            }
            listBoxIncludeWards.Items.Clear();
            listBoxDoNotIncludeWards.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                {
                    listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
                else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                {
                    listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
            }
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxIncludeWards.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxIncludeWards.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            int numitems = GlobalVars.arrayWards.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                if (GlobalVars.arrayWards[i, 1] == itemText)
                {
                    GlobalVars.arrayWards[i, 2] = "0";
                }
            }
            listBoxIncludeWards.Items.Clear();
            listBoxDoNotIncludeWards.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                {
                    listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
                else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                {
                    listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                }
            }

        }

        

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonUnitsAllRight_Click(object sender, EventArgs e)
        {
            int numitems = GlobalVars.arrayUnits.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                GlobalVars.arrayUnits[i, 1] = "1";
            }
            listBoxIncludeUnits.Items.Clear();
            listBoxDoNotIncludeUnits.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "1"))
                {
                    listBoxIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
                else if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "0"))
                {
                    listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
            }
        }

        private void buttonUnitsAllLeft_Click(object sender, EventArgs e)
        {
            int numitems = GlobalVars.arrayUnits.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                GlobalVars.arrayUnits[i, 1] = "0";
            }
            listBoxIncludeUnits.Items.Clear();
            listBoxDoNotIncludeUnits.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "1"))
                {
                    listBoxIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
                else if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "0"))
                {
                    listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
            }
        }

        private void buttonUnitRight_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxDoNotIncludeUnits.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxDoNotIncludeUnits.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            int numitems = GlobalVars.arrayUnits.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                if (GlobalVars.arrayUnits[i, 0] == itemText)
                {
                    GlobalVars.arrayUnits[i, 1] = "1";
                }
            }
            listBoxIncludeUnits.Items.Clear();
            listBoxDoNotIncludeUnits.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "1"))
                {
                    listBoxIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
                else if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "0"))
                {
                    listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
            }
        }

        private void buttonUnitLeft_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxIncludeUnits.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxIncludeUnits.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            int numitems = GlobalVars.arrayUnits.GetLength(0);
            for (int i = 0; i < numitems; i++)
            {
                if (GlobalVars.arrayUnits[i, 0] == itemText)
                {
                    GlobalVars.arrayUnits[i, 1] = "0";
                }
            }
            listBoxIncludeUnits.Items.Clear();
            listBoxDoNotIncludeUnits.Items.Clear();
            for (int i = 0; i < numitems; i++)
            {
                if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "1"))
                {
                    listBoxIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
                else if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "0"))
                {
                    listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
            }
        }

        public static String convertRuleText(String RawRuleText, int AdmitOffsetInDays, String strCalendarDate,
                                       String strDayofYear, String strDayOfMonth, String strDayOfWeek,
                                       int AdmitOffsetInHours, int EventOffsetInDays, 
                                       int EventOffsetInHours, String ActiveOrderableItem)
        {
            
            String strNewRuleText = RawRuleText;
            
            if ((RawRuleText.Contains("X DAYS")) && ((RawRuleText.Contains("ADMIT DATE"))
                || (RawRuleText.Contains("ADMISSION DAY"))))
            {
                strNewRuleText = strNewRuleText.Replace("X", AdmitOffsetInDays.ToString());
            }
            
            else if ((RawRuleText.Contains("X HOURS")) && ((RawRuleText.Contains("ADMIT DATE"))
                || (RawRuleText.Contains("ADMISSION DAY"))))
            {
                strNewRuleText = strNewRuleText.Replace("X", AdmitOffsetInHours.ToString());
            }
            else if ((RawRuleText.Contains("X DAYS")) && (RawRuleText.Contains("EVENT DATE")))
            {
                strNewRuleText = strNewRuleText.Replace("X", EventOffsetInDays.ToString());
            }
            else if ((RawRuleText.Contains("X HOURS")) && (RawRuleText.Contains("EVENT DATE")))
            {
                strNewRuleText = strNewRuleText.Replace("X", EventOffsetInHours.ToString());
            }
            else if (RawRuleText == "DATE OF MONTH")
            {
                strNewRuleText = strNewRuleText + " ON DAY: " + strDayOfMonth;
            }
            else if (RawRuleText == "DATE OF YEAR")
            {
                if (strDayofYear.Length == 4)
                {
                    strNewRuleText = strNewRuleText + " ON DATE: " + strDayofYear.Substring(0, 2) + "/" + strDayofYear.Substring(2, 2);
                }
            }
            else if (RawRuleText == "DAY OF WEEK")
            {
                strNewRuleText = strNewRuleText + " ON " + strDayOfWeek;
            }
            else if (RawRuleText == "ONE TIME - CALENDAR DATE")
            {
                strNewRuleText = strNewRuleText + " ON " + strCalendarDate;
            }
            if (ActiveOrderableItem != "")
            {
                String AOItemText = ActiveOrderableItem.Split('~')[1];
                strNewRuleText = strNewRuleText + " WITH ACTIVE ORDER FOR " + AOItemText;
            }
            return strNewRuleText;
        }

        private void comboSelectTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBoxWardFormulas.Items.Clear();
            listBoxDoNotIncludeWards.Items.Clear();
            listBoxIncludeWards.Items.Clear();
            listBoxWardRulesSelected.DataSource = null;
            listBoxWardRulesSelected.Items.Clear();
            listBoxUnitFormulas.Items.Clear();
            listBoxDoNotIncludeUnits.Items.Clear();
            listBoxIncludeUnits.Items.Clear();
            listBoxUnitRulesSelected.DataSource = null;
            listBoxUnitRulesSelected.Items.Clear();
            listBoxHospitalFormulas.Items.Clear();
            listBoxHospitalRulesSelected.DataSource = null;
            listBoxHospitalRulesSelected.Items.Clear();
            textBoxHospitalName.Text = "";
            //Create New Rules Objects

            labelWardNoteTitleSelected.Text = "Note Title Selected: " + comboSelectTitle.Text;
            labelUnitNoteTitleSelected.Text = "Note Title Selected: " + comboSelectTitle.Text;
            labelHospitalNoteTitleSelected.Text = "Note Title Selected: " + comboSelectTitle.Text;

            textBoxReminderCohort.Text = "";
            GlobalVars.chosenCohort.itemtext = "";
            GlobalVars.chosenCohort.itemien = "";
            GlobalVars.chosenTitleIEN = "";

            if (comboSelectTitle.Text != "")
            {
                string thisItemIen = GlobalVars.arrayTitles[comboSelectTitle.SelectedIndex, 0];
                if (thisItemIen != "")
                {
                    GlobalVars.chosenTitleIEN = thisItemIen;
                    string IEN_ARG = RpcFormatter.FormatArgs(true, thisItemIen + ";TIU(8925.1,");
                    string resItem = GlobalVars.dc.CallRPC("C9C GET COHORT FOR TITLE", IEN_ARG);
                    string[] TitleResult = resItem.Split('^');
                    if (TitleResult.Count() > 1)
                    {
                        if ((TitleResult[0] != "") && (TitleResult[1] != ""))
                        {
                            GlobalVars.chosenCohort.itemien = TitleResult[0];
                            GlobalVars.chosenCohort.itemtext = TitleResult[1];
                            textBoxReminderCohort.Text = TitleResult[1];
                        }
                    }
                    if (TitleResult.Count() > 2)
                    {
                        if (TitleResult[2] == "1")
                        {
                            textBoxReminderCohort.Text = textBoxReminderCohort.Text + " - Pass-Throughs Processed";
                            GlobalVars.PTOnly = 1;
                        }
                        else
                        {
                            GlobalVars.PTOnly = 0;
                        }
                    }
                }

                GlobalVars.TheseFormulas.Clear();
                string thisvptr = GlobalVars.arrayTitles[comboSelectTitle.SelectedIndex, 0] +
                                 ";TIU(8925.1,";
                string AV_ARG = RpcFormatter.FormatArgs(true, thisvptr);

                if (comboSelectTitle.Text != "")
                {
                    string resItem = GlobalVars.dc.CallRPC("C9C GET COHORT FOR TITLE", AV_ARG);
                    string[] TitleResult = resItem.Split('^');
                    if (TitleResult.Count() > 1)
                    {
                        if ((TitleResult[0] != "") && (TitleResult[1] != ""))
                        {
                            GlobalVars.chosenCohort.itemien = TitleResult[0];
                            GlobalVars.chosenCohort.itemtext = TitleResult[1];
                            textBoxReminderCohort.Text = TitleResult[1];
                        }
                    }
                    if (TitleResult.Count() > 2)
                    {
                        if (TitleResult[2] == "1")
                        {
                            textBoxReminderCohort.Text = textBoxReminderCohort.Text + " - Pass-Throughs Processed";
                            GlobalVars.PTOnly = 1;
                        }
                        else
                        {
                            GlobalVars.PTOnly = 0;
                        }
                    }
                }

                string res4 = GlobalVars.dc.CallRPC("C9C GET FORMULAS", AV_ARG);
                string[] results = Common.Split(res4);
                if (results.GetLength(0) > 0)
                {
                    List<String> FormulaSet = new List<String>();
                    List<String> ScopeList = new List<String>();
                    
                    String ThisFormulaName = "";
                    String ThisCategory = "";
                    for (int i = 0; i < results.GetLength(0); i++)
                    {
                        String[] SplitRule = results[i].Split('^');
                        if (SplitRule.GetLength(0) > 0)
                        {

                            if (SplitRule[1] != ThisFormulaName)
                            {

                                
                                if (ThisFormulaName != "")
                                {
                                    if (ThisCategory != "")
                                    {
                                        GlobalVars.TheseFormulas.Add(new ClassFormula("TIU", ThisFormulaName, ThisCategory, ScopeList, FormulaSet));
                                    }
                                }
                                ThisCategory = SplitRule[0];
                                FormulaSet.Clear();
                                ScopeList.Clear();
                                ThisFormulaName = SplitRule[1];
                                StringBuilder sb = new StringBuilder();
                                for (int r = 2; r < 12; r++)
                                {
                                    sb.Append(SplitRule[r] + '^');
                                }
                                FormulaSet.Add(sb.ToString());
                                sb.Clear();
                                for (int s = 12; s < (SplitRule.GetLength(0) - 1); s++)
                                {
                                    ScopeList.Add(SplitRule[s]);
                                }

                            }
                            else
                            {
                                StringBuilder sb = new StringBuilder();
                                for (int r = 2; r < 12; r++)
                                {
                                    sb.Append(SplitRule[r] + '^');
                                }
                                FormulaSet.Add(sb.ToString());
                                sb.Clear();
                            }
                        }
                    }
                    if ((ThisFormulaName != "") && (FormulaSet.Count > 0) && (ThisCategory != "")) // && (ScopeList.Count > 0))
                    {

                        GlobalVars.TheseFormulas.Add(new ClassFormula("TIU", ThisFormulaName, ThisCategory, ScopeList, FormulaSet));

                    }
                }
                int numwards = GlobalVars.arrayWards.GetLength(0);
                int numunits = GlobalVars.arrayUnits.GetLength(0);
                for (int tr = 0; tr < GlobalVars.TheseFormulas.Count; tr++)
                {
                    if (GlobalVars.TheseFormulas[tr].FormulaScope == "WARD")
                    {
                        //MessageBox.Show(GlobalVars.TheseRules[tr].RuleName);
                        listBoxDoNotIncludeWards.Items.Clear();
                        listBoxIncludeWards.Items.Clear();
                        listBoxWardRulesSelected.DataSource = null;
                        listBoxWardRulesSelected.Items.Clear();
                        listBoxWardFormulas.Items.Add(GlobalVars.TheseFormulas[tr].FormulaName);
                        listBoxWardFormulas.SelectedItem = GlobalVars.TheseFormulas[tr].FormulaName;
                        GlobalVars.wardcurrentFormulaIENstring = GlobalVars.TheseFormulas[tr].IENString;                        
                    }
                    else if (GlobalVars.TheseFormulas[tr].FormulaScope == "UNIT")
                    {
                        listBoxDoNotIncludeUnits.Items.Clear();
                        listBoxIncludeUnits.Items.Clear();
                        listBoxUnitRulesSelected.DataSource = null;
                        listBoxUnitRulesSelected.Items.Clear();
                        //MessageBox.Show(GlobalVars.TheseRules[tr].RuleName);
                        listBoxUnitFormulas.Items.Add(GlobalVars.TheseFormulas[tr].FormulaName);
                        listBoxUnitFormulas.SelectedItem = GlobalVars.TheseFormulas[tr].FormulaName;
                        GlobalVars.unitcurrentFormulaIENstring = GlobalVars.TheseFormulas[tr].IENString;                
                    }
                    else if (GlobalVars.TheseFormulas[tr].FormulaScope == "HOSPITAL")
                    {
                        listBoxHospitalRulesSelected.DataSource = null;
                        listBoxHospitalRulesSelected.Items.Clear();
                        textBoxHospitalName.Text = "";
                        //MessageBox.Show(GlobalVars.TheseRules[tr].RuleName);

                        if (GlobalVars.TheseFormulas[tr].ScopeList.Contains(GlobalVars.SelectedDivision))
                        {
                            listBoxHospitalFormulas.Items.Add(GlobalVars.TheseFormulas[tr].FormulaName);
                            listBoxHospitalFormulas.SelectedItem = GlobalVars.TheseFormulas[tr].FormulaName;
                            textBoxHospitalName.Text = GlobalVars.SelectedDivision;
                            GlobalVars.hospitalcurrentFormulaIENstring = GlobalVars.TheseFormulas[tr].IENString;
                        }
                        if (GlobalVars.TheseFormulas[tr].ScopeList.Count < 1)
                        {
                            listBoxHospitalFormulas.Items.Add(GlobalVars.TheseFormulas[tr].FormulaName);
                            listBoxHospitalFormulas.SelectedItem = GlobalVars.TheseFormulas[tr].FormulaName;
                            textBoxHospitalName.Text = GlobalVars.SelectedDivision;
                            GlobalVars.hospitalcurrentFormulaIENstring = GlobalVars.TheseFormulas[tr].IENString;
                        }
                       
                    }
                }
            }


        }

        private void listBoxWardFormulas_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelWardMoves.Enabled = false;
            String CurrentWardFormulaName = listBoxWardFormulas.Text;
            int CurrentIndex = -1;
            for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
            {
                if (GlobalVars.TheseFormulas[rn].FormulaName == CurrentWardFormulaName)
                {
                    CurrentIndex = rn;
                }
            }

            if ((CurrentWardFormulaName != "") && (CurrentIndex > -1))
            {
                listBoxDoNotIncludeWards.Items.Clear();
                listBoxIncludeWards.Items.Clear();
                listBoxWardRulesSelected.DataSource = null;
                listBoxWardRulesSelected.Items.Clear();
                GlobalVars.wardcurrentFormulaIENstring = "";
                if (GlobalVars.TheseFormulas[CurrentIndex].FormulaName == CurrentWardFormulaName)
                {
                    GlobalVars.wardcurrentFormulaIENstring = GlobalVars.TheseFormulas[CurrentIndex].IENString;
                    ArrayList LWardRules = new ArrayList();
                    //MessageBox.Show(CurrentWardRuleName);
                    int numwards = GlobalVars.arrayWards.GetLength(0);
                    for (int w = 0; w < numwards; w++)
                    {
                        if (GlobalVars.TheseFormulas[CurrentIndex].ScopeList.Contains(GlobalVars.arrayWards[w, 1]))
                        {
                            GlobalVars.arrayWards[w, 2] = "1";
                        }
                        else
                        {
                            GlobalVars.arrayWards[w, 2] = "0";
                        }
                    }
                    for (int i = 0; i < numwards; i++)
                    {
                        if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "1"))
                        {
                            listBoxIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                        }
                        else if ((GlobalVars.arrayWards[i, 1] != "") && (GlobalVars.arrayWards[i, 2] == "0"))
                        {
                            listBoxDoNotIncludeWards.Items.Add(GlobalVars.arrayWards[i, 1]);
                        }
                    }
                    
                    for (int rt = 0; rt < GlobalVars.TheseFormulas[CurrentIndex].Rules.Count(); rt++)
                    {

                        String ConvertedRule = convertRuleText(GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleText, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInDays,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrCalendarDate, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayofYear,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfMonth, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfWeek,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInHours,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInDays,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInHours,
                                                               GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisActiveOrderableItem);

                        String CurrentRuleIEN = GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleIEN;
                        if ((ConvertedRule != "") && (CurrentRuleIEN != ""))
                        {
                            listBoxWardRulesSelected.DisplayMember = "ruledescription";
                            listBoxWardRulesSelected.ValueMember = "ruleien";
                            var thisitem = new LType { ruledescription = ConvertedRule, ruleien = CurrentRuleIEN };
                            
                            LWardRules.Add(thisitem);
                        }                        
                        
                    }
                    listBoxWardRulesSelected.DataSource = LWardRules;
                }
            }
        }

        private void listBoxUnitFormulas_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelUnitMoves.Enabled = false;
            String CurrentUnitFormulaName = listBoxUnitFormulas.Text;
            int CurrentIndex = -1;
            for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
            {
                if (GlobalVars.TheseFormulas[rn].FormulaName == CurrentUnitFormulaName)
                {
                    CurrentIndex = rn;
                }
            }

            if ((CurrentUnitFormulaName != "") && (CurrentIndex > -1))
            {
                listBoxDoNotIncludeUnits.Items.Clear();
                listBoxIncludeUnits.Items.Clear();
                listBoxUnitRulesSelected.DataSource = null;
                listBoxUnitRulesSelected.Items.Clear();
                GlobalVars.unitcurrentFormulaIENstring = "";
            }
            GlobalVars.unitcurrentFormulaIENstring = GlobalVars.TheseFormulas[CurrentIndex].IENString;
            int numunits = GlobalVars.arrayUnits.GetLength(0);
            for (int w = 0; w < numunits; w++)
            {
                if (GlobalVars.TheseFormulas[CurrentIndex].ScopeList.Contains(GlobalVars.arrayUnits[w, 0]))
                {
                    GlobalVars.arrayUnits[w, 1] = "1";
                }
                else
                {
                    GlobalVars.arrayUnits[w, 1] = "0";
                }
            }
            for (int i = 0; i < numunits; i++)
            {
                if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "1"))
                {
                    listBoxIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
                else if ((GlobalVars.arrayUnits[i, 0] != "") && (GlobalVars.arrayUnits[i, 1] == "0"))
                {
                    listBoxDoNotIncludeUnits.Items.Add(GlobalVars.arrayUnits[i, 0]);
                }
            }
            ArrayList LUnitRules = new ArrayList();
            for (int rt = 0; rt < GlobalVars.TheseFormulas[CurrentIndex].Rules.Count(); rt++)
            {
                String ConvertedRule = convertRuleText(GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleText, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInDays,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrCalendarDate, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayofYear,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfMonth, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfWeek,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInHours,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInDays,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInHours,
                                                       GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisActiveOrderableItem);

                String CurrentRuleIEN = GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleIEN;
                if ((ConvertedRule != "") && (CurrentRuleIEN != ""))
                {
                    listBoxUnitRulesSelected.DisplayMember = "ruledescription";
                    listBoxUnitRulesSelected.ValueMember = "ruleien";
                    var thisitem = new LType { ruledescription = ConvertedRule, ruleien = CurrentRuleIEN };
                    
                    LUnitRules.Add(thisitem);
                }
                
            }
            listBoxUnitRulesSelected.DataSource = LUnitRules;
        }

        private void listBoxHospitalFormulas_SelectedIndexChanged(object sender, EventArgs e)
        {
            String CurrentHospitalFormulaName = listBoxHospitalFormulas.Text;
            int CurrentIndex = -1;
            for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
            {
                if (GlobalVars.TheseFormulas[rn].FormulaName == CurrentHospitalFormulaName)
                {
                    CurrentIndex = rn;
                }
            }

            if ((CurrentHospitalFormulaName != "") && (CurrentIndex > -1))
            {
                listBoxHospitalRulesSelected.DataSource = null;
                listBoxHospitalRulesSelected.Items.Clear();
            }
            
            ArrayList LHospitalRules = new ArrayList();
            for (int rt = 0; rt < GlobalVars.TheseFormulas[CurrentIndex].Rules.Count(); rt++)
            {
                String ConvertedRule = convertRuleText(GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleText, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInDays,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrCalendarDate, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayofYear,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfMonth, GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisstrDayOfWeek,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisAdmitOffsetInHours,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInDays,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisEventOffsetInHours,
                                                   GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisActiveOrderableItem);
                String CurrentRuleIEN = GlobalVars.TheseFormulas[CurrentIndex].Rules[rt].ThisRuleIEN;
                if ((ConvertedRule != "") && (CurrentRuleIEN != ""))
                {
                    listBoxHospitalRulesSelected.DisplayMember = "ruledescription";
                    listBoxHospitalRulesSelected.ValueMember = "ruleien";
                    var thisitem = new LType { ruledescription = ConvertedRule, ruleien = CurrentRuleIEN };
                    
                    LHospitalRules.Add(thisitem);
                }
                
            }
            listBoxHospitalRulesSelected.DataSource = LHospitalRules;
            
        }

        public class LType
        {
            public string ruledescription { get; set; }
            public string ruleien { get; set; }
        }

        private void buttonDeleteWardFormula_Click(object sender, EventArgs e)
        {
            String HighlightedRule = listBoxWardFormulas.Text;
            if (HighlightedRule != "")
            {
                int CurrentIndex = -1;
                for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
                {
                    if (GlobalVars.TheseFormulas[rn].FormulaName == HighlightedRule)
                    {
                        CurrentIndex = rn;
                    }
                }
                if (CurrentIndex > -1)
                {                    
                    string[] IENSplit = GlobalVars.wardcurrentFormulaIENstring.Split(';');
                    if (IENSplit.GetLength(0) > 2)
                    {
                        string[] args = { IENSplit[0], IENSplit[2], "WARD" };
                        string res6 = GlobalVars.dc.CallRPC("C9C DELETE FORMULA", RpcFormatter.FormatArgs(true, args));

                        if (res6 == "0")
                        {
                            MessageBox.Show("Error Deleting Formula!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error Deleting Formula!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error Deleting Formula!");
                    return;
                }
                
            }
            else
            {
                MessageBox.Show("You must first select a formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonDeleteUnitFormula_Click(object sender, EventArgs e)
        {
            String HighlightedRule = listBoxUnitFormulas.Text;
            if (HighlightedRule != "")
            {
                int CurrentIndex = -1;
                for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
                {
                    if (GlobalVars.TheseFormulas[rn].FormulaName == HighlightedRule)
                    {
                        CurrentIndex = rn;
                    }
                }
                if (CurrentIndex > -1)
                {                    
                    string[] IENSplit = GlobalVars.unitcurrentFormulaIENstring.Split(';');
                    if (IENSplit.GetLength(0) > 2)
                    {
                        string[] args = { IENSplit[0], IENSplit[2], "UNIT" };
                        string res6 = GlobalVars.dc.CallRPC("C9C DELETE FORMULA", RpcFormatter.FormatArgs(true, args));

                        if (res6 == "0")
                        {
                            MessageBox.Show("Error Deleting Formula!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error Deleting Formula!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error Deleting Formula!");
                    return;
                }

            }
            else
            {
                MessageBox.Show("You must first select a formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonDeleteHospFormula_Click(object sender, EventArgs e)
        {
            String HighlightedRule = listBoxHospitalFormulas.Text;
            if (HighlightedRule != "")
            {
                int CurrentIndex = -1;
                for (int rn = 0; rn < GlobalVars.TheseFormulas.Count; rn++)
                {
                    if (GlobalVars.TheseFormulas[rn].FormulaName == HighlightedRule)
                    {
                        CurrentIndex = rn;
                    }
                }
                if (CurrentIndex > -1)
                {                    
                    string[] IENSplit = GlobalVars.hospitalcurrentFormulaIENstring.Split(';');
                    if (IENSplit.GetLength(0) > 2)
                    {
                        string[] args = { IENSplit[0], IENSplit[2], "HOSPITAL" };
                        string res6 = GlobalVars.dc.CallRPC("C9C DELETE FORMULA", RpcFormatter.FormatArgs(true, args));

                        if (res6 == "0")
                        {
                            MessageBox.Show("Error Deleting Formula!");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error Deleting Formula!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error Deleting Formula!");
                    return;
                }

            }
            else
            {
                MessageBox.Show("You must first select a formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonChangeWards_Click(object sender, EventArgs e)
        {
            if (listBoxWardFormulas.SelectedIndex > -1)
            {
                panelWardMoves.Enabled = true;
            }

        }

        private void buttonSaveWardChanges_Click(object sender, EventArgs e)
        {
            if (listBoxIncludeWards.Items.Count < 1)
            {
                MessageBox.Show("You must have at least one item chosen to show!");
            }
            else
            {
                //Save to VistA
                string lItems = "";
                for (int g = 0; g < GlobalVars.arrayWards.GetLength(0); g++)
                {
                    if ((GlobalVars.arrayWards[g, 2] == "1") && (GlobalVars.arrayWards[g, 1] != ""))
                    {
                        if (lItems == "")
                        {
                            lItems = GlobalVars.arrayWards[g, 0];
                        }
                        else
                        {
                            lItems = lItems + "^" + GlobalVars.arrayWards[g, 0];
                        }
                    }
                }                
                string res5 = "0";
                if ((GlobalVars.wardcurrentFormulaIENstring != "") && (lItems != ""))
                {
                    string[] args = { lItems, GlobalVars.wardcurrentFormulaIENstring };
                    res5 = GlobalVars.dc.CallRPC("C9C SAVE RULES ENGINE WARDS", RpcFormatter.FormatArgs(true, args));
                }
                if (res5 == "1")
                {
                    MessageBox.Show("Successfully Updated Wards For This Formula!");
                }
                else
                {
                    MessageBox.Show("Error Updating Wards For This Formula!");
                }

            }
            
            panelWardMoves.Enabled = false;
        }

        private void buttonAddWardFormulaName_Click(object sender, EventArgs e)
        {            
            String NewFormulaName = "";
            GlobalVars.ShowInputDialog(ref NewFormulaName, "Formula Name");
            
            for (int i=0; i<listBoxWardFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxWardFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Ward Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxUnitFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxUnitFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Unit Formula With This Name Already Exists This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxHospitalFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxHospitalFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Hospital Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            int currentTitleIndex = comboSelectTitle.SelectedIndex;
            if (currentTitleIndex < 0)
            {
                MessageBox.Show("A Document Title Must Be Selected!");
                return;
            }
            string RIEN = GlobalVars.arrayTitles[currentTitleIndex, 0];
            string[] args = { RIEN, "WARD", NewFormulaName };
            string res8 = GlobalVars.dc.CallRPC("C9C SAVE NEW FORMULA", RpcFormatter.FormatArgs(true, args));
        
                if (res8 == "0")
            {
                MessageBox.Show("Error Saving Formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonAddUnitFormula_Click(object sender, EventArgs e)
        {
            String NewFormulaName = "";
            GlobalVars.ShowInputDialog(ref NewFormulaName, "Formula Name");
            
            for (int i = 0; i < listBoxWardFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxWardFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Ward Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxUnitFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxUnitFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Unit Formula With This Name Already Exists This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxHospitalFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxHospitalFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Hospital Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            int currentTitleIndex = comboSelectTitle.SelectedIndex;
            if (currentTitleIndex < 0)
            {
                MessageBox.Show("A Document Title Must Be Selected!");
                return;
            }
            string RIEN = GlobalVars.arrayTitles[currentTitleIndex, 0];
            string[] args = { RIEN, "UNIT", NewFormulaName };
            string res8 = GlobalVars.dc.CallRPC("C9C SAVE NEW FORMULA", RpcFormatter.FormatArgs(true, args));

            if (res8 == "0")
            {
                MessageBox.Show("Error Saving Formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonAddHospFormula_Click(object sender, EventArgs e)
        {
            String NewFormulaName = "";
            GlobalVars.ShowInputDialog(ref NewFormulaName, "Formula Name");
            
            for (int i = 0; i < listBoxWardFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxWardFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Ward Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxUnitFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxUnitFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Unit Formula With This Name Already Exists This Note Title!");
                    return;
                }
            }
            for (int i = 0; i < listBoxHospitalFormulas.Items.Count; i++)
            {
                if (NewFormulaName == listBoxHospitalFormulas.Items[i].ToString())
                {
                    MessageBox.Show("A Hospital Formula With This Name Already Exists For This Note Title!");
                    return;
                }
            }
            int currentTitleIndex = comboSelectTitle.SelectedIndex;
            if (currentTitleIndex < 0)
            {
                MessageBox.Show("A Document Title Must Be Selected!");
                return;
            }
            string RIEN = GlobalVars.arrayTitles[currentTitleIndex, 0];
            string[] args = { RIEN, "HOSPITAL", NewFormulaName };
            string res8 = GlobalVars.dc.CallRPC("C9C SAVE NEW FORMULA", RpcFormatter.FormatArgs(true, args));

            if (res8 == "0")
            {
                MessageBox.Show("Error Saving Formula!");
                return;
            }
            comboSelectTitle_SelectedIndexChanged(sender, e);
        }

        private void buttonAddWardRuleSelected_Click(object sender, EventArgs e)
        {
            
            FormRule fr = new FormRule();
            fr.Owner = this;
            fr.Text = "Add New Ward Rule";
            var retresult = fr.ShowDialog();
            int indexSelected = listBoxWardFormulas.SelectedIndex;
            if (retresult == DialogResult.OK)
            {
                comboSelectTitle_SelectedIndexChanged(sender, e);
            }
            listBoxWardFormulas.SelectedIndex = indexSelected;
            
        }

        private void buttonSaveUnitChanges_Click(object sender, EventArgs e)
        {
            if (listBoxIncludeUnits.Items.Count < 1)
            {
                MessageBox.Show("You must have at least one item chosen to show!");
            }
            else
            {
                //Save to VistA
                string lItems = "";
                for (int g = 0; g < GlobalVars.arrayUnits.GetLength(0); g++)
                {
                    if ((GlobalVars.arrayUnits[g, 1] == "1") && (GlobalVars.arrayUnits[g, 0] != ""))
                    {
                        if (lItems == "")
                        {
                            lItems = GlobalVars.arrayUnits[g, 0];
                        }
                        else
                        {
                            lItems = lItems + "^" + GlobalVars.arrayUnits[g, 0];
                        }
                    }
                }
                
                string res5 = "0";
                if ((GlobalVars.unitcurrentFormulaIENstring != "") && (lItems != ""))
                {
                    string[] args = { lItems, GlobalVars.unitcurrentFormulaIENstring };
                    res5 = GlobalVars.dc.CallRPC("C9C SAVE RULES ENGINE UNITS", RpcFormatter.FormatArgs(true, args));
                }
                if (res5 == "1")
                {
                    MessageBox.Show("Successfully Updated Units For This Formula!");
                }
                else
                {
                    MessageBox.Show("Error Updating Units For This Formula!");
                }

            }
            
            panelUnitMoves.Enabled = false;
        }

        private void buttonChangeUnits_Click(object sender, EventArgs e)
        {
            if (listBoxUnitFormulas.SelectedIndex > -1)
            { 
              panelUnitMoves.Enabled = true;
            }
        }

        private void buttonAddUnitRuleSelected_Click(object sender, EventArgs e)
        {
            
            FormRule fr = new FormRule();
            fr.Owner = this;
            fr.Text = "Add New Unit Rule";
            var retresult = fr.ShowDialog();
            int indexSelected = listBoxUnitFormulas.SelectedIndex;
            if (retresult == DialogResult.OK)
            {
                comboSelectTitle_SelectedIndexChanged(sender, e);
            }
            listBoxUnitFormulas.SelectedIndex = indexSelected;
        }

        private void buttonAddHospitalRuleSelected_Click(object sender, EventArgs e)
        {
            FormRule fr = new FormRule();
            fr.Owner = this;
            fr.Text = "Add New Hospital Rule";
            var retresult = fr.ShowDialog();
            int indexSelected = listBoxHospitalFormulas.SelectedIndex;
            if (retresult == DialogResult.OK)
            {
                comboSelectTitle_SelectedIndexChanged(sender, e);
            }
            listBoxHospitalFormulas.SelectedIndex = indexSelected;
        }

        private void listBoxWardRulesSelected_SelectedIndexChanged(object sender, EventArgs e)
        {            
            string[] splitFormula = GlobalVars.wardcurrentFormulaIENstring.Split(';');
            if (splitFormula.GetLength(0) == 5)
            {
                GlobalVars.wardcurrentFormulaIENstring = splitFormula[0] + ";" + splitFormula[1] + ";" +
                                                         splitFormula[2] + ";" + splitFormula[3] + ";" +
                                                         listBoxWardRulesSelected.SelectedValue;
                
            }

        }

        private void listBoxUnitRulesSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] splitFormula = GlobalVars.unitcurrentFormulaIENstring.Split(';');
            if (splitFormula.GetLength(0) == 5)
            {
                GlobalVars.unitcurrentFormulaIENstring = splitFormula[0] + ";" + splitFormula[1] + ";" +
                                                         splitFormula[2] + ";" + splitFormula[3] + ";" +
                                                         listBoxUnitRulesSelected.SelectedValue;

            }
        }

        private void listBoxHospitalRulesSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] splitFormula = GlobalVars.hospitalcurrentFormulaIENstring.Split(';');
            if (splitFormula.GetLength(0) == 5)
            {
                GlobalVars.hospitalcurrentFormulaIENstring = splitFormula[0] + ";" + splitFormula[1] + ";" +
                                                         splitFormula[2] + ";" + splitFormula[3] + ";" +
                                                         listBoxHospitalRulesSelected.SelectedValue;

            }

        }

        private void buttonDeleteWardRuleSelected_Click(object sender, EventArgs e)
        {
            
            if (listBoxWardRulesSelected.SelectedIndex < 0)
            {
                MessageBox.Show("You must first select a rule from the above listbox to delete!");
                return;
            }
            string res5 = "0";
            if (GlobalVars.wardcurrentFormulaIENstring != "")
            {
                string[] args = { GlobalVars.wardcurrentFormulaIENstring };
                res5 = GlobalVars.dc.CallRPC("C9C DELETE SINGLE RULE", RpcFormatter.FormatArgs(true, args));
            }
            if (res5 == "0")
            {
                MessageBox.Show("Error Deleting This Rule!");
            }
            int indexSelected = listBoxWardFormulas.SelectedIndex;
            comboSelectTitle_SelectedIndexChanged(sender, e);
            listBoxWardFormulas.SelectedIndex = indexSelected;
            
        }

        private void buttonDeleteUnitRuleSelected_Click(object sender, EventArgs e)
        {
            if (listBoxUnitRulesSelected.SelectedIndex < 0)
            {
                MessageBox.Show("You must first select a rule from the above listbox to delete!");
                return;
            }
            string res5 = "0";
            if (GlobalVars.unitcurrentFormulaIENstring != "")
            {
                string[] args = { GlobalVars.unitcurrentFormulaIENstring };
                res5 = GlobalVars.dc.CallRPC("C9C DELETE SINGLE RULE", RpcFormatter.FormatArgs(true, args));
            }
            if (res5 == "0")
            {
                MessageBox.Show("Error Deleting This Rule!");
            }
            int indexSelected = listBoxUnitFormulas.SelectedIndex;
            comboSelectTitle_SelectedIndexChanged(sender, e);
            listBoxUnitFormulas.SelectedIndex = indexSelected;
           
        }

        private void buttonDeleteHospitalRuleSelected_Click(object sender, EventArgs e)
        {
            if (listBoxHospitalRulesSelected.SelectedIndex < 0)
            {
                MessageBox.Show("You must first select a rule from the above listbox to delete!");
                return;
            }
            string res5 = "0";
            if (GlobalVars.hospitalcurrentFormulaIENstring != "")
            {
                string[] args = { GlobalVars.hospitalcurrentFormulaIENstring };
                res5 = GlobalVars.dc.CallRPC("C9C DELETE SINGLE RULE", RpcFormatter.FormatArgs(true, args));
            }
            if (res5 == "0")
            {
                MessageBox.Show("Error Deleting This Rule!");
            }
            int indexSelected = listBoxHospitalFormulas.SelectedIndex;
            comboSelectTitle_SelectedIndexChanged(sender, e);
            listBoxHospitalFormulas.SelectedIndex = indexSelected;            
        }



        private void buttonSelectCohort_Click(object sender, EventArgs e)
        {
            FormSelectCohort frC = new FormSelectCohort();
            frC.Owner = this;
            frC.Text = "Select Cohort";
            var retresult = frC.ShowDialog();
            int TitleSelected = comboSelectTitle.SelectedIndex;

            if ((GlobalVars.chosenCohort != null) && (GlobalVars.chosenCohort.itemtext != ""))
            {
                textBoxReminderCohort.Text = GlobalVars.chosenCohort.itemtext;
                if (GlobalVars.PTOnly == 1)
                {
                    textBoxReminderCohort.Text = textBoxReminderCohort.Text + " - Pass-Throughs Processed";
                }            
            }
            else
            {
                textBoxReminderCohort.Text = "";
            }

        }
    }
}
