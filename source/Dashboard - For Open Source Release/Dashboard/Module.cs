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
using System.Drawing;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;

namespace Dashboard
{
    class dataGridColumn
    {
        public string parentModule;
        public Boolean IsColumnDisplayed;
        public int columnIndex;
        public string headerText;
        public string columnType;
        public string columnHint;
        public string columnIEN;
        public string RPCParameterColumns;
        
        //this column value from this row will be sent as data to the RPC
        public string columnClickRPC = ""; //what happens when you click on content in this column
        public Color highlightColor;
        public List<String> highlightData;
        public DataGridView parentDataGridView;

        public void dataGridColumn_CellContentClick(object sender, DataGridViewCellEventArgs e, List<dataGridColumn> listofColumns)
        {            
            int thisColumnIndex = e.ColumnIndex;
            int thisRowIndex = e.RowIndex;
            if (thisRowIndex < 0)
            {
                return;
            }
            DataGridViewCell thisCell = (sender as DataGridView)[thisColumnIndex, thisRowIndex];
            string RPCToRun = "";
            if (listofColumns.Count >= thisColumnIndex)
            {
                if (listofColumns[thisColumnIndex].columnType == "WEBSITE")
                {
                    if ((thisCell.Value != null) && (thisCell.Value.ToString() != ""))
                    {
                        string webaddress = thisCell.Value.ToString();
                        FormWebsite ws = new FormWebsite();
                        ws.Navigate(webaddress);
                        ws.ShowDialog();
                    }
                    return;
                }
            }
            if(listofColumns.Count >= thisColumnIndex)
            {
                RPCToRun = listofColumns[thisColumnIndex].columnClickRPC;
            }

            if (RPCToRun != "")
            {                
                string RPCParms = listofColumns[thisColumnIndex].RPCParameterColumns;
                string THIS_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                if(RPCParms == "")
                {
                    THIS_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                }
                else
                {
                    string[] argsString;
                    string[] ParmSplit = listofColumns[thisColumnIndex].RPCParameterColumns.Split(';');
                    
                    if (ParmSplit.Count() > 0)
                    {
                        List<string> argsColumns = new List<string>();
                        
                        for (int p = 0; p < ParmSplit.Count(); p++)
                        {
                            int j;

                            if (Int32.TryParse(ParmSplit[p], out j))
                            {
                                argsColumns.Add(ParmSplit[p]);
                            }
                        }
                        argsString = new string[argsColumns.Count];
                        for (int q=0;q<argsColumns.Count;q++)
                        {
                            int foundColumn = -1;
                            for(int fc=0;((fc<listofColumns.Count)&&(foundColumn==-1));fc++)
                            {
                                if(listofColumns[fc].columnIEN == argsColumns[q])
                                {
                                    int j;
                                    if (Int32.TryParse(listofColumns[fc].columnIEN, out j))
                                    {
                                        foundColumn = fc;
                                    }
                                }
                            }
                            if (foundColumn > -1)
                            {
                                //may have to get data from "hidden" column...
                                DataGridViewCell parameterCell = (sender as DataGridView)[foundColumn, thisRowIndex];                                                                                         
                                argsString[q] = parameterCell.Value.ToString();                                
                            }                            
                            //Next I need to convert argsString to the format for args
                        }
                        
                        THIS_ARG = RpcFormatter.FormatArgs(true, argsString);
                    }
                    
                    
                }
                string res9 = GlobalVars.RunRPC(RPCToRun, THIS_ARG);
                string[] results = Common.Split(res9);
                if (results.Length > 0)
                {                    
                    TextDetails od = new TextDetails();
                    for (int i = 0; i < results.GetLength(0); i++)
                    {
                        if (i == 0)
                        {
                            od.textBoxOrderDetails.AppendText(results[i]);
                        }
                        else
                        {
                            od.textBoxOrderDetails.AppendText("\r\n" + results[i]);
                        }
                    }
                    od.textBoxOrderDetails.BackColor = Color.White;                    
                    od.ShowDialog();
                }
            }   
        }       
    }

    interface GetFilterables
    {
        String RPCFilterAbles();
    }

    

    interface GetModuleFilterables
    {
        List<String> AllModuleFilterables();
    }

    interface GetModuleShownFilterables
    {
        List<String> ModuleShownFilterables();
    }

    class Module : GetFilterables, GetModuleFilterables, GetModuleShownFilterables 
    {        
        public DataGridView dynamicGridView;
        private RichTextBox dynamicRichText;
        public List<dataGridColumn> ModuleColumns;
        private String RPCFilterPopulateValues = "";        
        private List<String> ModuleAllFilterStrings;
        private List<String> ModuleShowThese;
        private String RPCToPopulateValues = "";
        private ComboBox thisLocationsCombo;
        private String ButtonActionText = "";
        private String ButtonActionRPC = "";
        public String ThisModuleIEN = "";
        public String ThisModuleType = "";
        public Panel ThisParentPanel;
        public int Modulegridsortcolumnindex = -1;
        public System.ComponentModel.ListSortDirection Modulegridsortorder = System.ComponentModel.ListSortDirection.Ascending;
        public int ModulelocComboIndex = -1;
        public Label lastRefreshedLabel;

        public String GetButtonActionText()
        {
            return ButtonActionText;
        }

        public String GetButtonActionRPC()
        {
            return ButtonActionRPC;
        }

        public String GetRPCToPopulateValues()
        {
            return RPCToPopulateValues;
        }

        public ComboBox GetLocationsCombo()
        {
            return thisLocationsCombo;
        }

        public List<String> ModuleShownFilterables()
        {
            return ModuleShowThese;
        }

        public List<String> AllModuleFilterables()
        {
            return ModuleAllFilterStrings;
        }

        public String RPCToGetFilterables()
        {
            return RPCFilterPopulateValues;
        }

        

        


        public String RPCFilterAbles()
        { 
            return RPCFilterPopulateValues; 
        }
                
        private void AddOneRichTextBox(Color BoxColor, String thisText, Panel parentpanel, String ActionText, String ActionRPC, String RPCData)
        {
            this.dynamicRichText = new RichTextBox();
            this.dynamicRichText.Parent = parentpanel;
            this.dynamicRichText.BackColor = BoxColor;
            this.dynamicRichText.Text = thisText;
            this.dynamicRichText.Left = 4;
            this.dynamicRichText.Top = 30;
            this.dynamicRichText.Height = 100;
            this.dynamicRichText.Width = parentpanel.Width;
            this.dynamicRichText.Dock = DockStyle.Top;
            this.dynamicRichText.ReadOnly = true;
            this.dynamicRichText.Cursor = Cursors.Default;

            if ((ActionText != "") && (ActionRPC != "") && (RPCData !=""))
            {
                //Add an action button
                Button ActionButton = new Button();
                ActionButton.Parent = this.dynamicRichText;
                ActionButton.FlatStyle = FlatStyle.Flat;
                ActionButton.Text = ActionText;
                ActionButton.AutoSize = true;
                ActionButton.Top = 3;
                ActionButton.Left = (this.dynamicRichText.Width - 10) - ActionButton.Width;
                ActionButton.BackColor = Control.DefaultBackColor;
                ActionButton.ForeColor = Control.DefaultForeColor;
                ActionButton.Anchor = AnchorStyles.Right;
                ActionButton.TabStop = false;
                ActionButton.Click += delegate (object sender, EventArgs e) { ActionButton_Click(sender, e, ActionRPC, RPCData); };
            }
        }

        public void ActionButton_Click(object sender, EventArgs e, String ActionRPC, String RPCData)
        {
            if (DialogResult.Yes == MessageBox.Show((sender as Button).Text + "?", "Are you sure?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                string[] parms = RPCData.Split(':');
                if (parms.Count() == 2)
                {
                    string THIS_ARG = RpcFormatter.FormatArgs(true, parms[0], parms[1]);
                    string res9 = GlobalVars.RunRPC(ActionRPC, THIS_ARG);
                    string[] results3 = Common.Split(res9);
                    if (results3.Count() > 0)
                    {
                        if ((results3[0] == null) || (results3[0] == "0") || (results3[0] == ""))
                        {
                            MessageBox.Show("Error in RPC: " + ActionRPC);
                        }
                        else
                        {
                            //success so dispose of this one for now (may need to revisit to be more generic and just rebuild the list instead)
                            if (sender is Button)
                            {
                                if (((Button)sender).Parent is RichTextBox)
                                {
                                    ((Button)sender).Parent.Dispose();
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error in RPC: " + ActionRPC);
                    }
                }
            }
        }

        public void UpdateRichTextList(string RPCPopulateValues, Panel parentpanel, ComboBox myLocationsCombo, string ModuleThatClickedIEN, string actText, string actRPC)
        {
            RPCToPopulateValues = RPCPopulateValues;

            string argsAll = "";

            if ((RPCPopulateValues != "RPCToPopulate") && ((myLocationsCombo.Text != "") || (myLocationsCombo.Visible == false)))
            {

                Application.UseWaitCursor = true;  
                Application.DoEvents();

                List<String> myFilters = new List<String>();

                string[] args1 = { ModuleThatClickedIEN };  //this is ModuleIEN;Instance
                string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                string res9 = GlobalVars.RunRPC("C9C DASHBOARD GET USER FILTERS", THIS_ARG);
                string[] results3 = Common.Split(res9);
                if (results3.Length > 0)
                {
                    for (int m = 0; m < results3.Length; m++)
                    {
                        myFilters.Add(results3[m].Trim());
                    }
                }



                if ((myLocationsCombo.Visible == true) && (myFilters.Count > 0))
                {
                    //This broker sends list type parameters as zero-based arrays which could
                    //cause issues with programmers trying to write modules in mumps as they
                    //normally expect one-based arrays!  Be sure they are aware of this!
                    string[] strFilters = new string[myFilters.Count];
                    for (int s = 0; s < myFilters.Count; s++)
                    {
                        strFilters[s] = myFilters[s];
                    }
                    argsAll = RpcFormatter.FormatArgs(true, myLocationsCombo.Text, strFilters);
                }
                else if (myLocationsCombo.Visible == true)
                {
                    argsAll = RpcFormatter.FormatArgs(true, myLocationsCombo.Text);
                }
                else
                {
                    argsAll = RpcFormatter.FormatArgs(true, new string[0]);
                }

                int rtcount = parentpanel.Controls.Count;
                //Need to remove existing rich text boxes first
                for (int l = (rtcount - 1); l > -1; l--)
                {
                    if (parentpanel.Controls[l] is RichTextBox)
                    {
                        parentpanel.Controls[l].Dispose();
                    }
                }
                

                string res3 = GlobalVars.RunRPC(RPCPopulateValues, argsAll);
                string[] results = Common.Split(res3);
                if (results.Length > 0)
                {
                    List<string> AllRichTextBoxes = new List<string>();
                    for (int m = 0; m < results.Length; m++)
                    {
                        AllRichTextBoxes.Add(results[m]);
                    }

                    for (int k = 0; k < AllRichTextBoxes.Count; k++)
                    {
                        Color thiscolor = Color.FromName("LightYellow");
                        string[] strsplit = AllRichTextBoxes[k].Split('^');
                        String RPCData = strsplit[0];  //for Sticky Notes this is Patient IEN:Sticky Note Multiple IEN
                        string colorstring = strsplit[1];
                        if (colorstring != "")
                        {
                            thiscolor = Color.FromName(colorstring);
                        }
                        string thismessage = "";
                        if (strsplit.Length > 1)
                        {
                            for (int b = 2; b < strsplit.Length; b++)
                            {
                                if (thismessage == "")
                                {
                                    thismessage = strsplit[b];
                                }
                                else
                                {
                                    thismessage = thismessage + Environment.NewLine + strsplit[b];
                                }
                            }
                        }
                        AddOneRichTextBox(thiscolor, thismessage, parentpanel, actText, actRPC, RPCData);
                    }
                    parentpanel.AutoSize = false;
                    parentpanel.AutoScroll = true;
                }
                Application.UseWaitCursor = false;                
                Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                Application.DoEvents();
                
            }
        }

        public void UpdateLocationIndex(String moduleIEN, int SelIndex)
        {           
            if (SelIndex > -1)
            {
                for (int i = 0; i < GlobalVars.RunningModuleList.Count; i++)
                {
                    if (GlobalVars.RunningModuleList[i].ThisModuleIEN == moduleIEN)
                    {
                        GlobalVars.RunningModuleList[i].ModulelocComboIndex = SelIndex;
                        //need to update last refresh label here
                        
                        GlobalVars.RunningModuleList[i].lastRefreshedLabel.Text =
                            "Last Refreshed at: " + DateTime.Now.ToString("h:mm tt");
                    }
                }
            }
        }


        public void UpdateSortMode(String ModuleIEN, int sortedColumnIndex, SortOrder columnsortorder)       
        {
            System.ComponentModel.ListSortDirection thisDirection;
            if (columnsortorder == SortOrder.Ascending)
            {
                thisDirection = System.ComponentModel.ListSortDirection.Ascending;
            }
            else
            {
                thisDirection = System.ComponentModel.ListSortDirection.Descending;
            }            
            for (int i = 0; i < GlobalVars.RunningModuleList.Count; i++)
            {
                if (GlobalVars.RunningModuleList[i].ThisModuleIEN == ModuleIEN)
                {
                    GlobalVars.RunningModuleList[i].Modulegridsortcolumnindex = sortedColumnIndex;
                    GlobalVars.RunningModuleList[i].Modulegridsortorder = thisDirection;
                }
            }
        }

        public void UpdateDataGrid(string RPCPopulateValues, ref DataGridView thisDataGridView, 
                                    List<dataGridColumn> thesecolumns,
                                    ComboBox myLocationsCombo, string ModuleThatClickedIEN)
        {
            RPCToPopulateValues = RPCPopulateValues;
            int scol = -1;
            if (dynamicGridView.SortedColumn != null)  //save sort column
            {
                scol = dynamicGridView.SortedColumn.Index;
            }
            //Run RPC here
            if ((RPCPopulateValues != "RPCToPopulate")&&((myLocationsCombo.Text != "")||(myLocationsCombo.Visible ==false)))
            {
                thisDataGridView.Rows.Clear();
                thisDataGridView.ReadOnly = true;
                thisDataGridView.AllowUserToResizeColumns = true;

                Application.UseWaitCursor = true;  //from the Form/Window instance
                Application.DoEvents();

                List<String> myFilters = new List<String>();
                
                string[] args1 = { ModuleThatClickedIEN };  //this is ModuleIEN;Instance
                string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                string res9 = GlobalVars.RunRPC("C9C DASHBOARD GET USER FILTERS", THIS_ARG);
                string[] results3 = Common.Split(res9);
                if (results3.Length > 0)
                {
                    for (int m = 0; m < results3.Length; m++)
                    {
                        myFilters.Add(results3[m].Trim());
                    }
                } 
                
                string argsAll = "";                
                if ((myLocationsCombo.Visible == true) && (myFilters.Count > 0))
                {
                    //This broker sends list type parameters as zero-based arrays which could
                    //cause issues with programmers trying to write modules in mumps as they
                    //normally expect one-based arrays!  Be sure they are aware of this!
                    string[] strFilters = new string[myFilters.Count];
                    for (int s = 0; s < myFilters.Count; s++)
                    {
                        strFilters[s] = myFilters[s];
                    }
                    argsAll = RpcFormatter.FormatArgs(true, myLocationsCombo.Text, strFilters);
                }
                else if (myLocationsCombo.Visible == true)
                {
                    argsAll = RpcFormatter.FormatArgs(true, myLocationsCombo.Text);
                }
                else if ((myLocationsCombo.Visible == false) && (myFilters.Count > 0))
                {
                    string[] strFilters = new string[myFilters.Count];
                    for (int s = 0; s < myFilters.Count; s++)
                    {
                        strFilters[s] = myFilters[s];
                    }
                    argsAll = RpcFormatter.FormatArgs(true, "To Avoid Formatting Bug", strFilters);
                }
                else
                {
                    argsAll = RpcFormatter.FormatArgs(true, new string[0]);
                }                
                string res3 = GlobalVars.RunRPC(RPCPopulateValues, argsAll);
                string[] results = Common.Split(res3);
                if (results.Length > 0)
                {
                    List<string> ColumnsData = new List<string>();
                    int numberOfColumns = 0;
                    int rowColorColumn = -1;
                    for (int k = 0; k < thesecolumns.Count(); k++)
                    {                        
                        string thisColumnType = thesecolumns[k].columnType;
                        if (thisColumnType == "")                       
                        {
                            thisColumnType = "TEXT";
                        }
                        if (thisColumnType == "TEXT")
                        {
                            thisDataGridView.Columns[k].ValueType = typeof(String);
                        }
                        else if (thisColumnType == "DATE")
                        {
                            thisDataGridView.Columns[k].ValueType = typeof(DateTime);
                            thisDataGridView.Columns[k].DefaultCellStyle.Format = "MMM dd, yyyy  hh:mm tt";
                        }
                        else if (thisColumnType == "NUMERIC")
                        {
                            thisDataGridView.Columns[k].ValueType = typeof(System.Double);
                        }
                        else if (thisColumnType == "ROW COLOR")
                        {
                            thisDataGridView.Columns[k].ValueType = typeof(String);
                            rowColorColumn = k;
                        }

                        if (thesecolumns[k].IsColumnDisplayed)
                        {
                            thisDataGridView.Columns[k].Visible = true;
                        }
                        else
                        {
                            thisDataGridView.Columns[k].Visible = false;
                        }

                        numberOfColumns++;
                        string thisColumnData = k.ToString();
                            
                        if ((thesecolumns[k].columnClickRPC != "") || (thesecolumns[k].columnType == "WEBSITE"))
                        {                                
                            thisColumnData = thisColumnData + "^" + "Blue";                                
                        }
                        else
                        {
                            thisColumnData = thisColumnData + "^" + "Black";
                        }
                        if (thesecolumns[k].columnHint != "")
                        {                                
                            thisColumnData = thisColumnData + "^" + thesecolumns[k].columnHint;
                        }
                        else
                        {
                            thisColumnData = thisColumnData + "^" + "";
                        }
                        ColumnsData.Add(thisColumnData); 
                    }

                    for (int q=0; q<results.Length; q++)
                    {
                        string[] thisrowdata = results[q].Split('^');
                        //here I need to load each cell with the proper data type for display and sorting
                        DataGridViewRow newrow = new DataGridViewRow();
                        thisDataGridView.Rows.Add(newrow);
                        
                        int thisrow = thisDataGridView.Rows.Count - 1;
                        for (int r=0;r<thisrowdata.Count();r++)
                        {
                            
                            string thisStringData = thisrowdata[r];
                            
                            if (thisDataGridView.Columns[r].ValueType == typeof(String))
                            {
                                thisDataGridView.Rows[thisrow].Cells[r].Value = thisStringData;
                            }
                            else if (thisDataGridView.Columns[r].ValueType == typeof(DateTime))
                            {
                                thisStringData = thisStringData.Replace("@", " ");
                                if (thisStringData != "")
                                {
                                    thisDataGridView.Rows[thisrow].Cells[r].Value = DateTime.Parse(thisStringData);
                                }
                            }
                            else if (thisDataGridView.Columns[r].ValueType == typeof(System.Double))
                            {
                                double thisDouble;
                                if (Double.TryParse(thisStringData, out thisDouble))
                                {
                                    thisDataGridView.Rows[thisrow].Cells[r].Value = thisDouble;
                                } 
                            }
                            else
                            {
                                thisDataGridView.Rows[thisrow].Cells[r].Value = thisStringData;
                            }
                        }                        
                        
                        thisrow = thisDataGridView.Rows.Count - 1;
                        for(int c=0;c<ColumnsData.Count;c++)
                        {
                            string[] SplitData = ColumnsData[c].Split('^');
                            string strForeColor = SplitData[1];
                            string hint = SplitData[2];
                            if (strForeColor != "")
                            {
                                thisDataGridView.Rows[thisrow].Cells[c].Style.ForeColor = Color.FromName(strForeColor);
                            }
                            if (hint != "")
                            {
                                thisDataGridView.Rows[thisrow].Cells[c].ToolTipText = hint;
                            }
                        }
                        if (rowColorColumn > -1)
                        {
                            if (thisDataGridView.Rows[thisrow].Cells[rowColorColumn].Value != null)
                            {
                                string rowColor = thisDataGridView.Rows[thisrow].Cells[rowColorColumn].Value.ToString();
                                if (rowColor != "")
                                {
                                    thisDataGridView.Rows[thisrow].DefaultCellStyle.BackColor =
                                        Color.FromName(rowColor);
                                }
                            }                        
                        }                        
                    }
                }
                Application.UseWaitCursor = false;
                Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                Application.DoEvents();
            }

            thisDataGridView.AutoResizeColumns();
            for (int d = 0; d < thesecolumns.Count(); d++)
            {
                thesecolumns[d].parentDataGridView = thisDataGridView;                
            }
            
            thisDataGridView.EnableHeadersVisualStyles = false;
            thisDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            thisDataGridView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(thisDataGridView.Font, FontStyle.Bold);            
            thisDataGridView.ColumnHeadersHeight = 30;
            thisDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            thisDataGridView.RowHeadersVisible = false;
            thisDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            thisDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            if ((scol > -1) && (dynamicGridView.Columns[scol].Visible == true))
            {
                dynamicGridView.Sort(dynamicGridView.Columns[scol], Modulegridsortorder);
            }

            thisDataGridView.ClearSelection();
            this.ModuleColumns = thesecolumns;
        }
        private ComboBox AddOneLocationCombo(Panel parentpanel, String RPCLocations, String typeOfModule, List<dataGridColumn> mycolumns, 
                                             string moduleIEN, string RPCPopulateData, String locationlabel, String actText, String actRPC)
        {
            ComboBox LocationsCombo = new ComboBox();
            Panel grandparentpanel = (Panel)parentpanel.Parent;
            int moduleLabelWidth = 0;
            for (int p = 0; p < grandparentpanel.Controls.Count; p++)
            {
                if (grandparentpanel.Controls[p] is Panel)
                {
                    if (grandparentpanel.Controls[p].Name.IndexOf("panelTop") == 0)
                    {
                        for (int l = 0; l < grandparentpanel.Controls[p].Controls.Count; l++)
                        {
                            if (grandparentpanel.Controls[p].Controls[l] is Label)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("panelLabel") == 0)
                                {
                                    moduleLabelWidth = grandparentpanel.Controls[p].Controls[l].Width;
                                }
                            }
                        }
                        Label LocationsLabel = new Label();
                        LocationsLabel.Parent = grandparentpanel.Controls[p];
                        LocationsLabel.Top = 5;
                        LocationsLabel.Left = moduleLabelWidth + 60;
                        LocationsLabel.AutoSize = true;
                        LocationsLabel.Name = "locationsLabel";
                        LocationsLabel.Text = "Select Ward";
                        if (locationlabel != "")
                        {
                            LocationsLabel.Text = locationlabel;
                        }
                        
                        LocationsCombo.Parent = grandparentpanel.Controls[p];
                        LocationsCombo.Top = 3;
                        LocationsCombo.Left = moduleLabelWidth + 80 + LocationsLabel.Width + 12;
                        LocationsCombo.Width = 250;
                        LocationsCombo.Name = "locationsCombo";
                        if(typeOfModule == "DataGrid")
                        {                            
                            LocationsCombo.SelectedIndexChanged += (sender, e) => { UpdateDataGrid(RPCPopulateData, ref dynamicGridView, mycolumns, LocationsCombo, moduleIEN); };
                            LocationsCombo.SelectedIndexChanged += (sender, e) => { UpdateLocationIndex(moduleIEN, LocationsCombo.SelectedIndex); };
                        }

                        if (typeOfModule == "RichTextList")
                        {
                            Panel locgrandparentpanel = (Panel)LocationsCombo.Parent.Parent;                            
                            Panel myParent;
                            for (int gp = 0; gp < grandparentpanel.Controls.Count; gp++)
                            {
                                if (locgrandparentpanel.Controls[gp] is Panel)
                                {
                                    if (locgrandparentpanel.Controls[gp].Name.IndexOf("panelBody") == 0)
                                    {
                                        myParent = (Panel)locgrandparentpanel.Controls[gp];

                                        
                                        LocationsCombo.SelectedIndexChanged += (sender, e) => { UpdateRichTextList(RPCPopulateData, myParent, LocationsCombo, moduleIEN, actText, actRPC); };
                                        LocationsCombo.SelectedIndexChanged += (sender, e) => { UpdateLocationIndex(moduleIEN, LocationsCombo.SelectedIndex); };
                                    }
                                }
                            }                            
                        }
                        //Run RPC here to fill in combobox
                        if ((RPCLocations != "RPCGetAllLocations")&(RPCLocations != ""))
                        {
                            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                            string res3 = GlobalVars.RunRPC(RPCLocations, NO_ARG);
                            string[] results = Common.Split(res3);

                            if (results.GetLength(0) > 0)
                            {                                
                                for (int w = 0; w < results.GetLength(0); w++)
                                {
                                    if ((results[w].IndexOf("CERT NON-CERT") < 0) && (results[w].IndexOf("ON MEDICAL LEAVE") < 0))
                                    {
                                        string[] LocSplit = results[w].Split('^');
                                        if (LocSplit.Length > 1)
                                        {
                                            LocationsCombo.Items.Add(LocSplit[1]);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {                            
                            LocationsCombo.Visible = false;
                            LocationsCombo.Enabled = false;
                            LocationsLabel.Visible = false;
                        }
                    }
                }
            }
            return LocationsCombo;
        }

        
        private void AddOneFilterButton(Panel parentpanel, string parentModuleIEN)
        {
            Panel grandparentpanel = (Panel)parentpanel.Parent;
            int ExistingControlsRight = 0;
            
            for (int p = 0; p < grandparentpanel.Controls.Count; p++)
            {
                if (grandparentpanel.Controls[p] is Panel)
                {
                    if (grandparentpanel.Controls[p].Name.IndexOf("panelTop") == 0)
                    {
                        for (int l = 0; l < grandparentpanel.Controls[p].Controls.Count; l++)
                        {
                            if (grandparentpanel.Controls[p].Controls[l] is Label)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("panelLabel") == 0)
                                {
                                    ExistingControlsRight = grandparentpanel.Controls[p].Controls[l].Right;
                                }                                
                            }
                            else if (grandparentpanel.Controls[p].Controls[l] is ComboBox)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("locationsCombo") == 0)
                                {
                                    ExistingControlsRight = grandparentpanel.Controls[p].Controls[l].Right; 
                                }
                            }
                        }
                        
                        Button filterButton = new Button();
                        filterButton.Parent = grandparentpanel.Controls[p];
                        filterButton.Name = "filterButton_" + p.ToString();
                        filterButton.Top = 3;
                        filterButton.Left = ExistingControlsRight + 20;                       
                        filterButton.AutoSize = true;                        
                        filterButton.Text = "Filter";
                        filterButton.BackColor = Color.LightGreen;
                        filterButton.Click += delegate (object sender, EventArgs e) { filterButton_Click(sender, e, parentModuleIEN, RPCFilterPopulateValues); }; 
                    }
                }
            }
        }
        private void addLastRefreshedLabel(Panel parentpanel, string parentModuleIEN)
        {
            Panel grandparentpanel = (Panel)parentpanel.Parent;
            int ExistingControlsRight = 0;

            for (int p = 0; p < grandparentpanel.Controls.Count; p++)
            {
                if (grandparentpanel.Controls[p] is Panel)
                {
                    if (grandparentpanel.Controls[p].Name.IndexOf("panelTop") == 0)
                    {
                        for (int l = 0; l < grandparentpanel.Controls[p].Controls.Count; l++)
                        {
                            if (grandparentpanel.Controls[p].Controls[l] is Label)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("panelLabel") == 0)
                                {
                                    ExistingControlsRight = grandparentpanel.Controls[p].Controls[l].Right;
                                }
                            }
                            else if (grandparentpanel.Controls[p].Controls[l] is Button)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("filterButton") == 0)
                                {
                                    ExistingControlsRight = grandparentpanel.Controls[p].Controls[l].Right;
                                }
                            }
                            else if (grandparentpanel.Controls[p].Controls[l] is ComboBox)
                            {
                                if (grandparentpanel.Controls[p].Controls[l].Name.IndexOf("locationsCombo") == 0)
                                {
                                    ExistingControlsRight = grandparentpanel.Controls[p].Controls[l].Right;
                                }
                            }
                        }

                        lastRefreshedLabel = new Label();
                        lastRefreshedLabel.Name = "lastRefreshedLabel";
                        lastRefreshedLabel.Parent = grandparentpanel.Controls[p];
                        lastRefreshedLabel.Top = 8;
                        lastRefreshedLabel.Left = ExistingControlsRight + 20;                        
                        lastRefreshedLabel.AutoSize = true;                        
                        lastRefreshedLabel.Text = "Last Refreshed at: " + DateTime.Now.ToString("h:mm tt");
                    }
                }
            }
        }

        private void filterButton_Click(object sender, EventArgs e, string ClickingModuleIEN, string RPCAllFilterValues)
        {
            

            //Check to see if the list of filterables has changed:

            if (RPCAllFilterValues != "") //RPC Name Provided
            {
                string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);

                string res3 = GlobalVars.RunRPC(RPCAllFilterValues, NO_ARG);
                string[] results = Common.Split(res3);

                if (results.GetLength(0) > 0)
                {
                    ModuleAllFilterStrings = new List<String>();
                    for (int tf = 0; tf < results.GetLength(0); tf++)
                    {
                        ModuleAllFilterStrings.Add(results[tf].Trim());
                    }                   
                    
                }
            }

            FilterGeneric fs = new FilterGeneric(AllModuleFilterables(), ModuleShownFilterables());
            fs.buttonSave.Click += delegate (object newsender, EventArgs newe) { fs.buttonSave_Click(sender, e, ClickingModuleIEN); };            
            fs.ModuleThatClickedIEN = ClickingModuleIEN; //moduleIEN;Instance
            fs.ShowDialog();
            if(fs.FiltersJustSaved)
            {
                //refresh the data
                string modtype = "";
                int moduleIndex = -1;
                for (int i = 0; i < GlobalVars.RunningModuleList.Count; i++)
                {
                    if (GlobalVars.RunningModuleList[i].ThisModuleIEN == ClickingModuleIEN)
                    {
                        modtype = GlobalVars.RunningModuleList[i].ThisModuleType;
                        moduleIndex = i;
                    }
                }
                
                
                if (moduleIndex > -1)
                {
                    if ((((ComboBox)GetLocationsCombo()).SelectedIndex > -1) || 
                        (((ComboBox)GetLocationsCombo()).Visible == false))
                    {
                        if (modtype == "DataGrid")
                        {
                            UpdateDataGrid(GetRPCToPopulateValues(), ref dynamicGridView,
                                        ModuleColumns, GetLocationsCombo(), ClickingModuleIEN);
                        }
                        else if (modtype == "RichTextList")
                        {
                            
                            UpdateRichTextList(GetRPCToPopulateValues(), GlobalVars.RunningModuleList[moduleIndex].ThisParentPanel,
                                GetLocationsCombo(), ClickingModuleIEN, GlobalVars.RunningModuleList[moduleIndex].ButtonActionText,
                                GlobalVars.RunningModuleList[moduleIndex].ButtonActionRPC);
                        }
                    }
                }
                
                
                

                if (((Button)sender).Parent.Controls.ContainsKey("lastRefreshedLabel"))
                {
                    Label labelToUpdate = ((Label)((Button)sender).Parent.Controls.Find("lastRefreshedLabel",true).FirstOrDefault());
                    labelToUpdate.Text = "Last Refreshed at: " + DateTime.Now.ToString("h:mm tt");
                }

            }
            
        }

        public void dynamicGridView_CellContentClick(object sender, DataGridViewCellEventArgs e, List<dataGridColumn> columnsToSend)
        {
            int currentColumnIndex = e.ColumnIndex;
            ModuleColumns[currentColumnIndex].dataGridColumn_CellContentClick(sender, e, columnsToSend);
            
        }
        //Below code adds DataGrid modules
        public Module(string moduletype, List<dataGridColumn> columns, int rowcount, Panel parentpanel, 
                      String ModuleName, String RPCToPopulate, String RPCGetAllLocations, 
                      String RPCFilterGetAllValues,String newModuleIEN,
                      int gridsortcolumnindex, System.ComponentModel.ListSortDirection gridsortorder, int locComboIndex,
                      String loclbl)
        {
            
            if (moduletype == "DataGrid")
            {
                ThisModuleType = moduletype;
                ThisModuleIEN = newModuleIEN;
                ThisParentPanel = parentpanel;
                Modulegridsortcolumnindex = gridsortcolumnindex;
                if ((gridsortorder == System.ComponentModel.ListSortDirection.Ascending) ||
                    (gridsortorder == System.ComponentModel.ListSortDirection.Descending))
                {
                    Modulegridsortorder = gridsortorder;
                }
                else
                {
                    Modulegridsortorder = System.ComponentModel.ListSortDirection.Ascending;
                    gridsortorder = System.ComponentModel.ListSortDirection.Ascending;
                }
                ModulelocComboIndex = locComboIndex;                
                thisLocationsCombo = new ComboBox();
                this.dynamicGridView = new DataGridView();                
                
                dynamicGridView.ColumnCount = columns.Count;
                int thisIndex = -1;
                for (int c = 0; c < columns.Count(); c++)
                {
                    if (columns[c].IsColumnDisplayed)
                    {
                        dynamicGridView.Columns[c].Visible = true;
                    }
                    else
                    {
                        dynamicGridView.Columns[c].Visible = false;
                    }
                    thisIndex++;
                    dynamicGridView.Columns[thisIndex].HeaderText = columns[c].headerText;
                    
                    
                    if(columns[c].columnClickRPC != "")
                    {
                        dynamicGridView.Columns[thisIndex].DefaultCellStyle.ForeColor = Color.AliceBlue;
                    }
                    if (columns[c].columnHint != "")
                    {
                        dynamicGridView.Columns[thisIndex].ToolTipText = columns[c].columnHint;
                    }
                    if (columns[c].columnType == "WEBSITE")
                    {
                        dynamicGridView.Columns[thisIndex].DefaultCellStyle.ForeColor = Color.AliceBlue;
                    }
                }
                if ((gridsortcolumnindex > -1) && (dynamicGridView.Columns[gridsortcolumnindex].Visible == true))
                {
                    dynamicGridView.Sort(dynamicGridView.Columns[gridsortcolumnindex], Modulegridsortorder);
                }
                dynamicGridView.RowCount = rowcount;
                dynamicGridView.AllowUserToAddRows = false;
                this.dynamicGridView.Parent = parentpanel;
                this.dynamicGridView.Dock = DockStyle.Fill;


                String actionText = ""; //not yet used in datagrid modules
                String actionRPC = ""; //not yet used in datagrid modules
                
                thisLocationsCombo = AddOneLocationCombo(parentpanel, RPCGetAllLocations, moduletype, 
                    columns, newModuleIEN, RPCToPopulate, loclbl, actionText, actionRPC);              
                if (locComboIndex > -1)
                {
                  if (thisLocationsCombo.Items.Count > locComboIndex)
                  {
                    thisLocationsCombo.SelectedIndex = locComboIndex;
                  }
                }
                if (RPCToPopulate != "")
                {
                    RPCToPopulateValues = RPCToPopulate;
                }
                if ((columns != null) & (columns.Count > 0))
                {
                    this.ModuleColumns = columns;
                }

                if (RPCFilterGetAllValues != "") //RPC Name Provided
                {                    
                    string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                    string res3 = GlobalVars.RunRPC(RPCFilterGetAllValues, NO_ARG);
                    string[] results = Common.Split(res3);

                    if (results.GetLength(0) > 0)
                    {
                        ModuleAllFilterStrings = new List<String>();
                        for(int tf=0; tf<results.GetLength(0); tf++)
                        {
                            ModuleAllFilterStrings.Add(results[tf].Trim());
                        }
                        ModuleShowThese = new List<String>();
                        
                        //Run the RPC and add the results to ModuleShowThese
                        //RPC has to return array of IEN^Name
                        string[] args1 = { newModuleIEN };  //this is moduleien;instance
                        string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                        string res9 = GlobalVars.RunRPC("C9C DASHBOARD GET USER FILTERS", THIS_ARG);
                        results = Common.Split(res9);
                        if (results.Length > 0)
                        {
                            for (int m = 0; m < results.Length; m++)
                            {
                                ModuleShowThese.Add(results[m].Trim());
                            }
                        } 
                        AddOneFilterButton(parentpanel, newModuleIEN);
                    }
                }

                addLastRefreshedLabel(parentpanel, newModuleIEN);
                
                dynamicGridView.CellContentClick += delegate (object sender, DataGridViewCellEventArgs e) { this.dynamicGridView_CellContentClick(sender, e, columns); };
                                
                dynamicGridView.Sorted += (sender, e) => { UpdateSortMode(newModuleIEN, dynamicGridView.SortedColumn.Index, dynamicGridView.SortOrder); };

                if (thisLocationsCombo.Visible == false)
                {
                    UpdateDataGrid(RPCToPopulate, ref dynamicGridView, columns, thisLocationsCombo, newModuleIEN);
                } 
            }
         }
       

        //Below code adds RichTextList modules 
        public Module(string moduletype, Panel parentpanel, String ModuleName, String RPCToPopulate,
                      String RPCGetAllLocations, String RPCFilterGetAllValues,
                      String newModuleIEN, int locComboIndex, String loclbl, String ActionText, String ActionRPC)
        {
            if (moduletype == "RichTextList")
            {
                ThisParentPanel = parentpanel;
                ThisModuleType = moduletype;
                ThisModuleIEN = newModuleIEN;
                ButtonActionText = ActionText;
                ButtonActionRPC = ActionRPC;

                thisLocationsCombo = new ComboBox();
                List<dataGridColumn> columns = new List<dataGridColumn>(); //not really used
                if (RPCGetAllLocations != "")  //probably return a list of wards
                {
                    thisLocationsCombo = AddOneLocationCombo(parentpanel, RPCGetAllLocations, moduletype, 
                        columns, newModuleIEN, RPCToPopulate, loclbl, ActionText, ActionRPC);

                }

                if (locComboIndex > -1)
                {
                    if (thisLocationsCombo.Items.Count > locComboIndex)
                    {
                        thisLocationsCombo.SelectedIndex = locComboIndex;
                    }
                }
                
                
                if (RPCFilterGetAllValues != "") //RPC Name Provided
                {                    
                    string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                    string res3 = GlobalVars.RunRPC(RPCFilterGetAllValues, NO_ARG);
                    string[] results = Common.Split(res3);

                    if (results.GetLength(0) > 0)
                    {
                        ModuleAllFilterStrings = new List<String>();
                        for (int tf = 0; tf < results.GetLength(0); tf++)
                        {
                            ModuleAllFilterStrings.Add(results[tf].Trim());
                        }
                        ModuleShowThese = new List<String>();
                        
                        //Run the RPC and add the results to ModuleShowThese
                        //RPC has to return array of IEN^Name
                        string[] args1 = { newModuleIEN };  //this is moduleien;instance
                        string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                        string res9 = GlobalVars.RunRPC("C9C DASHBOARD GET USER FILTERS", THIS_ARG);
                        results = Common.Split(res9);
                        if (results.Length > 0)
                        {
                            for (int m = 0; m < results.Length; m++)
                            {
                                ModuleShowThese.Add(results[m].Trim());
                            }
                        }                        
                        AddOneFilterButton(parentpanel, newModuleIEN);
                    }
                }
                addLastRefreshedLabel(parentpanel, newModuleIEN);
            }
        } 

    }
}
