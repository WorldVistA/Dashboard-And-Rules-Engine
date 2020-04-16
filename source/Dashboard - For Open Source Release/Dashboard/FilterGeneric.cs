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
using System.Windows.Forms;
using System.Collections;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;

namespace Dashboard
{
    public partial class FilterGeneric : Form
    {
        private List<String> thisModuleAllFilterables;
        private List<String> thisModuleShow;
        private ArrayList LFilterablesShow = new ArrayList();
        private ArrayList LFilterablesDoNotShow = new ArrayList();
        public Boolean FiltersJustSaved = false;

        public FilterGeneric(List<String> AllFilterables, List<String> ShowThese)
        {
            InitializeComponent();
            thisModuleAllFilterables = new List<String>();
            thisModuleShow = new List<String>();
            thisModuleAllFilterables = AllFilterables;
            thisModuleShow = ShowThese;
        }

        public class LType
        {
            public string filtertext { get; set; }
            public string filterien { get; set; }
        }

        public string ModuleThatClickedIEN = "";
        private void FilterOnService_Shown(object sender, EventArgs e)
        {
            
            listBoxDoNotShow.DisplayMember = "filtertext";
            listBoxDoNotShow.ValueMember = "filterien";
            listBoxShow.DisplayMember = "filtertext";
            listBoxShow.ValueMember = "filterien";


            thisModuleShow.Clear(); //may have changed


            //RPC has to return array of IEN^Name
            string[] args4 = { };
            string[] args1 = { ModuleThatClickedIEN };  //this is ModuleIEN;Instance
            string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
            string res9 = GlobalVars.RunRPC("C9C DASHBOARD GET USER FILTERS", THIS_ARG);
            string[] results = Common.Split(res9);
            if (results.Length > 0)
            {
                for (int m = 0; m < results.Length; m++)
                {
                    thisModuleShow.Add(results[m].Trim());
                }
            }

            int numshowitems = thisModuleShow.Count();
            if (numshowitems == 0)
            {
                numshowitems = thisModuleAllFilterables.Count();
                for (int i = 0; i < numshowitems; i++)
                {
                    
                    string[] filterSplit = thisModuleAllFilterables[i].Split('^');
                    if (filterSplit.Length == 2)
                    {
                        
                        //add [0] to listbox item without showing it here...
                        var thisitem = new LType { filtertext = filterSplit[1], filterien = filterSplit[0] };
                        LFilterablesShow.Add(thisitem);
                        thisModuleShow.Add(thisModuleAllFilterables[i]);
                    }
                }
                listBoxShow.DataSource = LFilterablesShow;
            }
            else //user filter has items
            {
                int numAllitems = thisModuleAllFilterables.Count();
                for (int i = 0; i < numAllitems; i++)
                {
                    if (thisModuleShow.Contains(thisModuleAllFilterables[i]))
                    {
                        string[] filterSplit = thisModuleAllFilterables[i].Split('^');
                        if (filterSplit.Length == 2)
                        {
                            
                            //add [0] to listbox item without showing it here...
                            var thisitem = new LType { filtertext = filterSplit[1], filterien = filterSplit[0] };
                            LFilterablesShow.Add(thisitem);
                        }
                    }
                    else
                    {
                        string[] filterSplit = thisModuleAllFilterables[i].Split('^');
                        if (filterSplit.Length == 2)
                        {
                            
                            //add [0] to listbox item without showing it here...
                            var thisitem = new LType { filtertext = filterSplit[1], filterien = filterSplit[0] };
                            LFilterablesDoNotShow.Add(thisitem);
                        }
                    }
                }
                listBoxShow.DataSource = LFilterablesShow;
                listBoxDoNotShow.DataSource = LFilterablesDoNotShow;
            }
           
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxShow.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxShow.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            //build temporary holding areas for items
            ArrayList LNewFilterablesShow = new ArrayList();
            ArrayList LNewFilterablesDoNotShow = new ArrayList();
            
            for (int s = 0; s < listBoxDoNotShow.Items.Count; s++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxDoNotShow.Items)[s]).filtertext,
                    filterien = ((LType)(listBoxDoNotShow.Items)[s]).filterien
                };
                LNewFilterablesDoNotShow.Add(oneitem);
            }

            string selectedItemValue = listBoxShow.SelectedValue.ToString();

            for (int f = 0; f < listBoxShow.Items.Count; f++)
            {
                var Litem = new LType();
                Litem.filterien = ((LType)listBoxShow.Items[f]).filterien;
                Litem.filtertext = ((LType)listBoxShow.Items[f]).filtertext;
                if (Litem.filterien == selectedItemValue)
                {
                    var itemToMove = new LType { filtertext = Litem.filtertext, filterien = Litem.filterien };
                    LNewFilterablesDoNotShow.Add(itemToMove);


                }
                else
                {
                    var itemToMove = new LType { filtertext = Litem.filtertext, filterien = Litem.filterien };
                    LNewFilterablesShow.Add(itemToMove);
                }
            }

            listBoxShow.DataSource = null;
            listBoxShow.Items.Clear();
            listBoxShow.DisplayMember = "filtertext";
            listBoxShow.ValueMember = "filterien";
            listBoxShow.DataSource = LNewFilterablesShow;

            listBoxDoNotShow.DataSource = null;
            listBoxDoNotShow.Items.Clear();
            listBoxDoNotShow.DisplayMember = "filtertext";
            listBoxDoNotShow.ValueMember = "filterien";
            listBoxDoNotShow.DataSource = LNewFilterablesDoNotShow;
        } 

        private void buttonRight_Click(object sender, EventArgs e)
        {
            int selectedIndex = listBoxDoNotShow.SelectedIndex;
            if (selectedIndex < 0)
            {
                return;
            }
            string itemText = listBoxDoNotShow.SelectedItem.ToString();
            if (itemText == "")
            {
                return;
            }
            //build temporary holding areas for items
            ArrayList LNewFilterablesShow = new ArrayList();
            ArrayList LNewFilterablesDoNotShow = new ArrayList();

            for (int s = 0; s < listBoxShow.Items.Count; s++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxShow.Items)[s]).filtertext,
                    filterien = ((LType)(listBoxShow.Items)[s]).filterien
                };
                LNewFilterablesShow.Add(oneitem);
            }

            string selectedItemValue = listBoxDoNotShow.SelectedValue.ToString();

            for (int f = 0; f < listBoxDoNotShow.Items.Count; f++)
            {
                var Litem = new LType();
                Litem.filterien = ((LType)listBoxDoNotShow.Items[f]).filterien;
                Litem.filtertext = ((LType)listBoxDoNotShow.Items[f]).filtertext;
                if (Litem.filterien == selectedItemValue)
                {
                    var itemToMove = new LType { filtertext = Litem.filtertext, filterien = Litem.filterien };
                    LNewFilterablesShow.Add(itemToMove);


                }
                else
                {
                    var itemToMove = new LType { filtertext = Litem.filtertext, filterien = Litem.filterien };
                    LNewFilterablesDoNotShow.Add(itemToMove);
                }
            }

            listBoxShow.DataSource = null;
            listBoxShow.Items.Clear();
            listBoxShow.DisplayMember = "filtertext";
            listBoxShow.ValueMember = "filterien";
            listBoxShow.DataSource = LNewFilterablesShow;

            listBoxDoNotShow.DataSource = null;
            listBoxDoNotShow.Items.Clear();
            listBoxDoNotShow.DisplayMember = "filtertext";
            listBoxDoNotShow.ValueMember = "filterien";
            listBoxDoNotShow.DataSource = LNewFilterablesDoNotShow;
        }

        private void buttonAllRight_Click(object sender, EventArgs e)
        {
            //build temporary holding areas for items
            ArrayList LNewFilterablesShow = new ArrayList();
            ArrayList LNewFilterablesDoNotShow = new ArrayList();

            for (int s = 0; s < listBoxShow.Items.Count; s++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxShow.Items)[s]).filtertext,
                    filterien = ((LType)(listBoxShow.Items)[s]).filterien
                };
                LNewFilterablesShow.Add(oneitem);
            }

            
            for (int f = 0; f < listBoxDoNotShow.Items.Count; f++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxDoNotShow.Items)[f]).filtertext,
                    filterien = ((LType)(listBoxDoNotShow.Items)[f]).filterien
                };
                LNewFilterablesShow.Add(oneitem);
            }

            listBoxShow.DataSource = null;
            listBoxShow.Items.Clear();
            listBoxShow.DisplayMember = "filtertext";
            listBoxShow.ValueMember = "filterien";
            listBoxShow.DataSource = LNewFilterablesShow;

            listBoxDoNotShow.DataSource = null;
            listBoxDoNotShow.Items.Clear();
            listBoxDoNotShow.DisplayMember = "filtertext";
            listBoxDoNotShow.ValueMember = "filterien";
            listBoxDoNotShow.DataSource = LNewFilterablesDoNotShow;
        }

        private void buttonAllLeft_Click(object sender, EventArgs e)
        {
            //build temporary holding areas for items
            ArrayList LNewFilterablesShow = new ArrayList();
            ArrayList LNewFilterablesDoNotShow = new ArrayList();

            for (int s = 0; s < listBoxShow.Items.Count; s++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxShow.Items)[s]).filtertext,
                    filterien = ((LType)(listBoxShow.Items)[s]).filterien
                };
                LNewFilterablesDoNotShow.Add(oneitem);
            }


            for (int f = 0; f < listBoxDoNotShow.Items.Count; f++)
            {
                var oneitem = new LType
                {
                    filtertext = ((LType)(listBoxDoNotShow.Items)[f]).filtertext,
                    filterien = ((LType)(listBoxDoNotShow.Items)[f]).filterien
                };
                LNewFilterablesDoNotShow.Add(oneitem);
            }

            listBoxShow.DataSource = null;
            listBoxShow.Items.Clear();
            listBoxShow.DisplayMember = "filtertext";
            listBoxShow.ValueMember = "filterien";
            listBoxShow.DataSource = LNewFilterablesShow;

            listBoxDoNotShow.DataSource = null;
            listBoxDoNotShow.Items.Clear();
            listBoxDoNotShow.DisplayMember = "filtertext";
            listBoxDoNotShow.ValueMember = "filterien";
            listBoxDoNotShow.DataSource = LNewFilterablesDoNotShow;
        }

        private void FilterOnService_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (listBoxDoNotShow.Items.Count > 0)
            {
                if (listBoxShow.Items.Count < 1)
                {
                    MessageBox.Show("Not using these changes - nothing selected to show!");
                }              
            }            
        }

        public void buttonSave_Click(object sender, EventArgs e, string thisMIEN)
        {
            if (listBoxShow.Items.Count < 1)
            {
                MessageBox.Show("You must have at least one item chosen to show!");
            }
            else
            {
                //Save to VistA               

                string[] SendListToRPC = new string[listBoxShow.Items.Count]; //Send arguments as List Type
                for(int q=0; q<listBoxShow.Items.Count; q++)
                {
                    LType anItem = new LType
                    {
                     filtertext = ((LType)(listBoxShow.Items)[q]).filtertext,
                     filterien = ((LType)(listBoxShow.Items)[q]).filterien
                    };
                    SendListToRPC[q] = anItem.filterien + "^" + anItem.filtertext;
                }

                string THIS_ARG = RpcFormatter.FormatArgs(true, thisMIEN, SendListToRPC);  //thisMIEN is ModuleIEN;Instance
                string res9 = GlobalVars.RunRPC("C9C DASHBOARD PUT USER FILTERS", THIS_ARG);                
                
                if(res9 == "1")
                {                    
                    FiltersJustSaved = true;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Error Saving Filters!");
                }
                
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
