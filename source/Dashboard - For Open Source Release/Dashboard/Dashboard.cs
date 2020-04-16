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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;
using System.IO;

namespace Dashboard
{
    public partial class Dashboard : Form
    {
        private int NumberOfPanels = 0;        
                
        private List<Panel> ExistingPanels = new List<Panel>();
        public Dashboard()
        {
            InitializeComponent();
            panelbottom.AllowDrop = true;
            panelmiddle.AllowDrop = true;                        
            panelmiddle.DragEnter += panelmiddle_DragEnter;
            panelmiddle.DragDrop += panelmiddle_DragDrop;            
        }

    

        void buttonModules_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            int X = System.Windows.Forms.Cursor.Position.X;
            int Y = System.Windows.Forms.Cursor.Position.Y;

            if (e.Clicks >= 2)  //Double Click
            {
                System.Windows.Forms.Cursor.Position = new Point(X, Y - 100);
                DoDragDrop(sender, DragDropEffects.Move);
                System.Windows.Forms.Cursor.Position = new Point(X, Y);
            }

            else 
            {
                DoDragDrop(sender, DragDropEffects.Move);                
            }
        }

        
        
        void panelmiddle_DragEnter(object sender, DragEventArgs e)
        {
          e.Effect = DragDropEffects.Move;
        }


        //In below module, will need to get information about the module from GlobalVars
        void panelmiddle_DragDrop(object sender, DragEventArgs e)
        {
            if (ExistingPanels.Count < 4)
            {
                NumberOfPanels++;
            }
            ((Button)e.Data.GetData(typeof(Button))).Parent = (Panel)sender;
            string ButtonTag = ((Button)e.Data.GetData(typeof(Button))).Tag.ToString(); //this is IEN^ModuleName            

            //first let's gather information about existing panels
            //cheat right now for buttons without IEN
            string[] tempsplit = ButtonTag.Split('^');
            if (tempsplit.Length == 1)
            {
                ButtonTag = "44^" + ButtonTag;
            }

            string[] IENName = ButtonTag.Split('^');
            create_module(IENName, ref e, "", "", "");
        }

        void create_module(string[] IEN_Name, ref DragEventArgs e, string scol, string sdir, string loc)
        { 
            if (IEN_Name.Length == 2)
            {
                string newPanelModuleIEN = IEN_Name[0];
                string newPanelModuleName = IEN_Name[1];
                int NumberExistingPanels = 0;
                for (int k = 0; k < panelmiddleleft.Controls.Count; k++)
                {
                    if (panelmiddleleft.Controls[k] is Panel)
                    {                        
                        NumberExistingPanels++;
                    }
                }
                for (int k = 0; k < panelmiddleright.Controls.Count; k++)
                {
                    if (panelmiddleright.Controls[k] is Panel)
                    {
                        NumberExistingPanels++;
                    }
                }
                if (NumberExistingPanels == 4)
                {
                    MessageBox.Show("No more than four panels can be shown at one time!");
                    if (e.Data != null)
                    {
                        ((Button)e.Data.GetData(typeof(Button))).Parent = panelbottom;
                        ((Button)e.Data.GetData(typeof(Button))).Visible = true;
                    }
                    return;
                }

                for (int p = 0; p < NumberOfPanels; p++)
                {
                    Panel newPanel;
                    if (p == (NumberOfPanels - 1))
                    {
                        newPanel = new Panel();
                        newPanel.AutoSize = false;
                        newPanel.AutoScroll = true;
                        GlobalVars.NameCounter++;
                        newPanel.Name = newPanelModuleName + "_" + GlobalVars.NameCounter.ToString();  //p.ToString();
                        
                    }
                    else
                    {
                        newPanel = ExistingPanels[p];
                        newPanel.Dock = DockStyle.None;                        
                        newPanel.Parent = panelbottom;                    
                    }
                    if ((p == 0) && (NumberOfPanels == 1))
                    {
                        panelmiddleright.Visible = false;
                        panelmiddleleft.Dock = DockStyle.Fill;
                        newPanel.Parent = panelmiddleleft;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = panelmiddleleft.Height - 2;
                        newPanel.Width = panelmiddleleft.Width;                        
                    }
                    if ((p == 0) && (NumberOfPanels == 2))
                    {
                        panelmiddleleft.Dock = DockStyle.Left;
                        panelmiddleright.Visible = true;
                        panelmiddleright.Dock = DockStyle.Fill;
                        newPanel.Parent = panelmiddleleft;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = panelmiddleleft.Height - 2;
                        newPanel.Width = panelmiddleleft.Width;
                    }
                    if ((p == 0) && (NumberOfPanels == 3))
                    {
                        panelmiddleleft.Dock = DockStyle.Left;
                        panelmiddleright.Visible = true;
                        panelmiddleright.Dock = DockStyle.Fill;
                        newPanel.Parent = panelmiddleleft;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = panelmiddleleft.Height - 2;
                        newPanel.Width = panelmiddleleft.Width;
                    }
                    if ((p == 0) && (NumberOfPanels == 4))
                    {
                        panelmiddleleft.Dock = DockStyle.Left;
                        panelmiddleright.Visible = true;
                        panelmiddleright.Dock = DockStyle.Fill;
                        newPanel.Parent = panelmiddleleft;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = (panelmiddleleft.Height / 2) - 4;
                        newPanel.Width = panelmiddleleft.Width;
                    }
                    if ((p == 1) && (NumberOfPanels > 2))
                    {
                        newPanel.Parent = panelmiddleright;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = (panelmiddleright.Height / 2) - 4;
                        newPanel.Width = panelmiddleright.Width;
                    }
                    if ((p == 1) && (NumberOfPanels <= 2))
                    {
                        newPanel.Parent = panelmiddleright;
                        newPanel.Top = 2;
                        newPanel.Left = 2;
                        newPanel.Height = panelmiddleright.Height - 2;
                        newPanel.Width = panelmiddleright.Width;
                    }
                    if ((p == 2) && (NumberOfPanels == 3))
                    {
                        newPanel.Parent = panelmiddleright;
                        newPanel.Top = (panelmiddleright.Height / 2) + 4;
                        newPanel.Left = 2;
                        newPanel.Height = (panelmiddleright.Height / 2) - 4;
                        newPanel.Width = panelmiddleright.Width;
                    }
                    if ((p == 2) && (NumberOfPanels == 4))
                    {
                        newPanel.Parent = panelmiddleleft;
                        newPanel.Top = (panelmiddleleft.Height / 2) + 4;
                        newPanel.Left = 2;
                        newPanel.Height = (panelmiddleleft.Height / 2) - 4;
                        newPanel.Width = panelmiddleleft.Width;
                    }
                    if (p == 3)
                    {
                        newPanel.Parent = panelmiddleright;
                        newPanel.Top = (panelmiddleright.Height / 2);
                        newPanel.Left = 2;
                        newPanel.Height = (panelmiddleright.Height / 2) - 4;
                        newPanel.Width = panelmiddleright.Width;
                    }

                    newPanel.Visible = true;

                    if (NumberOfPanels == 1)
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

                    }
                    else if (NumberOfPanels == 2)
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Right)
                        | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
                    }
                    else if (NumberOfPanels == 3)
                    {
                        if ((p == 0) || (p == 1))
                        {
                            newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                            | System.Windows.Forms.AnchorStyles.Right)
                            | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)));
                        }
                        if (p == 2)
                        {
                            newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                        }
                    }
                    else if (NumberOfPanels == 4)
                    {
                        if ((p == 0) || (p == 1))
                        {
                            newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                            | System.Windows.Forms.AnchorStyles.Right)
                            | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)));
                        }
                        if ((p == 2) || (p == 3))
                        {
                            newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
                        }
                    }

                    if (p == (NumberOfPanels - 1))
                    {
                        Panel newPanelTop = new Panel();
                        newPanelTop.Parent = newPanel;
                        newPanelTop.Name = "panelTop" + p.ToString();
                        newPanelTop.Top = 2;
                        newPanelTop.Height = 30;
                        newPanelTop.Width = newPanel.Width;
                        newPanelTop.BorderStyle = BorderStyle.Fixed3D;
                        newPanelTop.BackColor = Color.AntiqueWhite;
                        newPanelTop.AutoSize = true;
                        newPanelTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                                | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));


                        Label PanelLabel = new Label();
                        PanelLabel.Parent = newPanelTop;
                        PanelLabel.Text = newPanelModuleName;
                        PanelLabel.Name = "panelLabel" + p.ToString();
                        PanelLabel.Top = 5;                        
                        PanelLabel.Left = 23;
                        PanelLabel.AutoSize = true;

                        Button buttonClosePanel = new Button();
                        buttonClosePanel.Parent = newPanelTop;
                        buttonClosePanel.Text = "X";
                        buttonClosePanel.Width = 20;
                        buttonClosePanel.Height = 20;                        
                        buttonClosePanel.Left = 3;
                        buttonClosePanel.Top = 5;
                        buttonClosePanel.Font = new Font(buttonClosePanel.Font, FontStyle.Bold);                        
                        buttonClosePanel.Dock = DockStyle.Left;
                        buttonClosePanel.Click += buttonClosePanel_Click;

                        Panel newPanelBody = new Panel();
                        newPanelBody.Parent = newPanel;
                        newPanelBody.Name = "panelBody" + p.ToString();
                        newPanelBody.Top = 32;
                        newPanelBody.Height = newPanel.Height - newPanelTop.Height - 4;
                        newPanelBody.Width = newPanel.Width;                        
                        newPanelBody.BackColor = Color.Cornsilk;
                        newPanelBody.BorderStyle = BorderStyle.Fixed3D;
                        newPanelBody.AutoSize = true;
                        newPanelBody.AutoScroll = true;                         
                        newPanelBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                                | System.Windows.Forms.AnchorStyles.Bottom)
                                | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));


                        //This is where each individual module will be created based on some imported definitions
                        //Module(columncount, rowcount, parent)                        

                        int moduleIndex = -1;
                        int jj;
                        if (Int32.TryParse(newPanelModuleIEN, out jj))
                        {
                            //find module in GlobalVars.AllModules
                            for (int a = 0; ((a < GlobalVars.AllModules.Length) && (moduleIndex == -1)); a++)
                            {
                                string[] aSplit = GlobalVars.AllModules[a].Split('^');
                                if (aSplit[0] == newPanelModuleIEN)
                                {
                                    moduleIndex = a;
                                }
                            }
                        }
                        if (moduleIndex > -1)
                        {
                            string[] ModuleSplit = GlobalVars.AllModules[moduleIndex].Split('^');

                            if (ModuleSplit.Length > 10)
                            {
                                string ModuleType = ModuleSplit[3];
                                string RPCPopulate = ModuleSplit[4];
                                string RPCGetAllLocations = ModuleSplit[5];
                                string RPCGetAllFilterValues = ModuleSplit[8];
                                string LocationLabel = ModuleSplit[6];
                                string ActionButtonText = ModuleSplit[9];
                                string ActionButtonRPC = ModuleSplit[10];

                                if (ModuleType == "DataGrid")    
                                {
                                    int sortColumn = 0;
                                    int NumValue;
                                    if (int.TryParse(scol, out NumValue))
                                    {
                                        sortColumn = NumValue;
                                    }
                                    System.ComponentModel.ListSortDirection ColumnSortDirection = ListSortDirection.Ascending;
                                    if (sdir == "Descending")
                                    {
                                        ColumnSortDirection = ListSortDirection.Descending;
                                    }

                                    int thislocationComboIndex = -1;
                                    if (int.TryParse(loc, out NumValue))
                                    {
                                        thislocationComboIndex = NumValue;
                                    }                                   
                                    
                                    List<dataGridColumn> TheseColumns = new List<dataGridColumn>();
                                    //Run RPC to get the datagrid columns for this module
                                    string[] args1 = { newPanelModuleIEN };  //this needs to be the ien of the module
                                    string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                                    string res3 = GlobalVars.RunRPC("C9C DATAGRID COLUMN HEADERS", THIS_ARG);
                                    string[] results = Common.Split(res3);
                                    if (results.Length > 0)
                                    {
                                        for (int h = 0; h < results.Length; h++) //create column headers and add to list
                                        {
                                            dataGridColumn newColumn = new dataGridColumn();
                                            string[] newColumnInfo = results[h].Split('^');
                                            if (newColumnInfo.Length > 5)
                                            {
                                                string[] splitheader = newColumnInfo[0].Split(';');

                                                newColumn.headerText = splitheader[0];
                                                if (splitheader.Count() > 1)
                                                {
                                                    newColumn.columnType = splitheader[1];
                                                }
                                                newColumn.columnIndex = h;
                                                newColumn.parentModule = newPanelModuleIEN;
                                                newColumn.columnClickRPC = newColumnInfo[2];
                                                newColumn.columnIEN = newColumnInfo[4];
                                                newColumn.RPCParameterColumns = newColumnInfo[5];
                                                if (newColumn.columnClickRPC != "")
                                                {
                                                    newColumn.highlightColor = Color.Aqua;
                                                }
                                                if (newColumnInfo[1] == "1")
                                                {
                                                    newColumn.IsColumnDisplayed = true;
                                                }
                                                else
                                                {
                                                    newColumn.IsColumnDisplayed = false;
                                                }
                                                newColumn.columnHint = newColumnInfo[3];

                                                TheseColumns.Add(newColumn);
                                            }
                                        }
                                        

                                        if (ModuleType == "DataGrid")
                                        {
                                            //add the module identifier to the ien for filter saving purposes
                                            int thisModuleIdentifier = -1;
                                            List<int> likeModuleCounter = new List<int>();
                                            for (int mc = 0; mc < GlobalVars.ActiveModules.Count; mc++)
                                            {
                                                string[] amSplit = GlobalVars.ActiveModules[mc].Split('^');
                                                if (amSplit.Length > 1)
                                                {
                                                    string[] miSplit = amSplit[0].Split(';');
                                                    if (miSplit[0] == newPanelModuleIEN) //ien;instance
                                                    {

                                                        if (miSplit.Count() > 1)
                                                        {

                                                            if (int.TryParse(miSplit[1], out NumValue))
                                                            {
                                                                likeModuleCounter.Add(NumValue);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            for (int nmi = 1; ((nmi < 5) && (thisModuleIdentifier < 1)); nmi++)
                                            {
                                                if (likeModuleCounter.Contains(nmi))
                                                {
                                                    //do nothing since this number already exists
                                                }
                                                else
                                                {
                                                    thisModuleIdentifier = nmi;
                                                }
                                            }
                                            if (thisModuleIdentifier > 0)
                                            {
                                                newPanelModuleIEN = newPanelModuleIEN + ';' + thisModuleIdentifier.ToString();
                                                GlobalVars.ActiveModules.Add(newPanelModuleIEN + "^" + newPanelModuleName + "^"
                                                                             + newPanel.Name);


                                                Module newModule = new Module(ModuleType, TheseColumns, 1, newPanelBody,
                                                                              newPanelModuleName, RPCPopulate, RPCGetAllLocations,
                                                                              RPCGetAllFilterValues, newPanelModuleIEN,
                                                                          sortColumn, ColumnSortDirection, thislocationComboIndex, LocationLabel);
                                                GlobalVars.RunningModuleList.Add(newModule);
                                            }
                                        }
                                        
                                    }
                                 
                                }
                                else if (ModuleType == "RichTextList")
                                {
                                    int NumValue;
                                    int thislocationComboIndex = -1;
                                    if (int.TryParse(loc, out NumValue))
                                    {
                                        thislocationComboIndex = NumValue;
                                    }
                                    int thisModuleIdentifier = -1;
                                    List<int> likeModuleCounter = new List<int>();
                                    for (int mc = 0; mc < GlobalVars.ActiveModules.Count; mc++)
                                    {
                                        string[] amSplit = GlobalVars.ActiveModules[mc].Split('^');
                                        if (amSplit.Length > 1)
                                        {
                                            string[] miSplit = amSplit[0].Split(';');
                                            if (miSplit[0] == newPanelModuleIEN) //ien;instance
                                            {

                                                if (miSplit.Count() > 1)
                                                {

                                                    if (int.TryParse(miSplit[1], out NumValue))
                                                    {
                                                        likeModuleCounter.Add(NumValue);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    for (int nmi = 1; ((nmi < 5) && (thisModuleIdentifier < 1)); nmi++)
                                    {
                                        if (likeModuleCounter.Contains(nmi))
                                        {
                                            //do nothing since this number already exists
                                        }
                                        else
                                        {
                                            thisModuleIdentifier = nmi;
                                        }
                                    }
                                    if (thisModuleIdentifier > 0)
                                    {
                                        newPanelModuleIEN = newPanelModuleIEN + ';' + thisModuleIdentifier.ToString();
                                        GlobalVars.ActiveModules.Add(newPanelModuleIEN + "^" + newPanelModuleName + "^"
                                                                     + newPanel.Name);

                                        Module newModule = new Module(ModuleType, newPanelBody, newPanelModuleName, RPCPopulate, RPCGetAllLocations,
                                                                      RPCGetAllFilterValues,  newPanelModuleIEN, thislocationComboIndex, LocationLabel, ActionButtonText, ActionButtonRPC);
                                        GlobalVars.RunningModuleList.Add(newModule);

                                    }
                                }
                            }
                        }
                        ExistingPanels.Add(newPanel);

                    }
                    
                }
                
            }

            //Now let's dock the panels correctly
            int pnum;
            if (NumberOfPanels == 1)
            {
                splittervertical.Visible = false;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = false;
                pnum = 1;
                panelmiddleright.Visible = false;
                for (int p = 0; p < panelmiddleleft.Controls.Count; p++)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill; 
                        }
                    }
                }
            }
            else if (NumberOfPanels == 2)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = false;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 1;
                for (int p = panelmiddleleft.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;                            
                        }
                    }
                }
                pnum = 1;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleright.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;                            
                        }
                    }
                }
            }
            else if (NumberOfPanels == 3)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = true;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 1;
                for (int p = panelmiddleleft.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;                            
                        }
                    }
                }
                pnum = 2;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleright.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;
                        }
                        if (pnum == 1)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Bottom;
                        }
                    }
                }
            }
            else if (NumberOfPanels == 4)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = true;
                splitterhorizontal1.Visible = true;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 2;
                for (int p = panelmiddleleft.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;                            
                        }
                        if (pnum == 1)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Bottom;                            
                        }
                    }
                }
                pnum = 2;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleright.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;                            
                        }
                        if (pnum == 1)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Bottom;
                        }
                    }
                }
            }


            //Put the button back on the bottom panel in case user wants to see this in multiple panels
            if (e.Data != null)
            {
                ((Button)e.Data.GetData(typeof(Button))).Parent = panelbottom;
                ((Button)e.Data.GetData(typeof(Button))).Visible = true;
            }
            

        }

        void ModuleWasDroppedOrRemoved(object sender, EventArgs e)
        {                        
            //first let's gather information about existing panels
            int NumberExistingPanels = 0;
            for (int k = 0; k < panelmiddleleft.Controls.Count; k++)
            {
                if (panelmiddleleft.Controls[k] is Panel)
                {
                    NumberExistingPanels++;  
                }
            }
            for (int k = 0; k < panelmiddleright.Controls.Count; k++)
            {
                if (panelmiddleright.Controls[k] is Panel)
                {
                    NumberExistingPanels++;
                }
            }            
            
            for (int p = 0; p < NumberOfPanels; p++)
            {
                Panel newPanel;
                newPanel = ExistingPanels[p];
                newPanel.Dock = DockStyle.None;                
                newPanel.Parent = panelbottom;
                               
                if ((p == 0) && (NumberOfPanels == 1))
                {
                    panelmiddleright.Visible = false;
                    panelmiddleleft.Dock = DockStyle.Fill;                   
                    newPanel.Parent = panelmiddleleft;
                    newPanel.Top = 2;
                    newPanel.Left = 2;
                    newPanel.Height = panelmiddleleft.Height - 2;
                    newPanel.Width = panelmiddleleft.Width;
                }
                if ((p == 0) && (NumberOfPanels == 2))
                {
                    panelmiddleleft.Dock = DockStyle.Left;
                    panelmiddleright.Visible = true;
                    panelmiddleright.Dock = DockStyle.Fill;
                    newPanel.Parent = panelmiddleleft;
                    newPanel.Top = 2;
                    newPanel.Left = 2;
                    newPanel.Height = panelmiddleleft.Height - 2;
                    newPanel.Width = panelmiddleleft.Width;
                }
                if ((p == 0) && (NumberOfPanels == 3))
                {
                    panelmiddleleft.Dock = DockStyle.Left;
                    panelmiddleright.Visible = true;
                    panelmiddleright.Dock = DockStyle.Fill;
                    newPanel.Parent = panelmiddleleft;
                    newPanel.Top = 2;
                    newPanel.Left = 2;
                    newPanel.Height = panelmiddleleft.Height - 2;
                    newPanel.Width = panelmiddleleft.Width;
                }
                if ((p == 0) && (NumberOfPanels == 4))
                {
                    panelmiddleleft.Dock = DockStyle.Left;
                    panelmiddleright.Visible = true;
                    panelmiddleright.Dock = DockStyle.Fill;
                    newPanel.Parent = panelmiddleleft;
                    newPanel.Top = 2;
                    newPanel.Left = 2;
                    newPanel.Height = (panelmiddleleft.Height / 2) - 4;
                    newPanel.Width = panelmiddleleft.Width;
                }
                if ((p == 1) && (NumberOfPanels > 2))
                {
                    newPanel.Parent = panelmiddleright;
                    newPanel.Top = 2;                    
                    newPanel.Left = 2;
                    newPanel.Height = (panelmiddleright.Height / 2) - 4;
                    newPanel.Width = panelmiddleright.Width;
                }
                if ((p == 1) && (NumberOfPanels <= 2))
                {
                    newPanel.Parent = panelmiddleright;
                    newPanel.Top = 2;                   
                    newPanel.Left = 2;
                    newPanel.Height = panelmiddleright.Height - 2;
                    newPanel.Width = panelmiddleright.Width;
                }
                if ((p == 2) && (NumberOfPanels == 3)) 
                {
                    newPanel.Parent = panelmiddleright;                    
                    newPanel.Top = (panelmiddleright.Height / 2) + 4;                    
                    newPanel.Left = 2;
                    newPanel.Height = (panelmiddleright.Height / 2) - 4;
                    newPanel.Width = panelmiddleright.Width;
                }
                if ((p == 2) && (NumberOfPanels == 4))
                {                    
                    newPanel.Parent = panelmiddleleft;
                    newPanel.Top = (panelmiddleleft.Height / 2) + 4;
                    newPanel.Left = 2;
                    newPanel.Height = (panelmiddleleft.Height / 2) - 4;
                    newPanel.Width = panelmiddleleft.Width;
                }
                if (p == 3)
                {                    
                    newPanel.Parent = panelmiddleright;
                    newPanel.Top = (panelmiddleright.Height / 2);
                    newPanel.Left = 2;
                    newPanel.Height = (panelmiddleright.Height / 2) - 4;
                    newPanel.Width = panelmiddleright.Width;
                }

                newPanel.Visible = true;

                if (NumberOfPanels == 1)
                {                    
                    newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                    | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));                    
                }
                else if (NumberOfPanels == 2)
                {                    
                    newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                    | System.Windows.Forms.AnchorStyles.Right)
                    | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)));
                }
                else if (NumberOfPanels == 3)
                {                    
                    if ((p == 0) || (p == 1))
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Right)
                        | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
                    }
                    if (p == 2)
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                    }
                }
                else if (NumberOfPanels == 4)
                {
                    if ((p == 0) || (p == 1))
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Right)
                        | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
                    }
                    if ((p == 2) || (p == 3))
                    {
                        newPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                    }
                }                
            } 
            

            //Now let's dock the panels correctly
            int pnum;
            if (NumberOfPanels == 1)
            {
                splittervertical.Visible = false;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = false;
                pnum = 1;
                panelmiddleright.Visible = false;
                for (int p = 0; p < panelmiddleleft.Controls.Count; p++)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;
                        }
                    }
                }
            }
            else if (NumberOfPanels == 2)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = false;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 1;
                for (int p = panelmiddleleft.Controls.Count-1; p>=0; p--)  
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;
                        }
                    }                    
                }
                pnum = 1;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleright.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;
                        }
                    }
                }
            }
            else if (NumberOfPanels == 3)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = false;
                splitterhorizontal1.Visible = true;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 1;
                for (int p = panelmiddleleft.Controls.Count - 1; p >= 0; p--)
                {                    
                    if (panelmiddleleft.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;
                        }
                    }
                }
                pnum = 2;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {                    
                    if (panelmiddleright.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;
                        }
                        if (pnum == 1)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Bottom;
                        }
                    }
                }
            }
            else if (NumberOfPanels == 4)
            {
                splittervertical.Visible = true;
                splitterhorizontal2.Visible = true;
                splitterhorizontal1.Visible = true;
                //iterate backwards due to docking order requirements (last control in docks first)
                panelmiddleright.Visible = true;
                pnum = 2;
                for (int p = panelmiddleleft.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleleft.Controls[p] is Panel)
                    {
                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Fill;
                        }
                        if (pnum == 1)
                        {
                            panelmiddleleft.Controls[p].Dock = DockStyle.Bottom;
                        }
                    }
                }
                pnum = 2;
                for (int p = panelmiddleright.Controls.Count - 1; p >= 0; p--)
                {
                    if (panelmiddleright.Controls[p] is Panel)
                    {

                        pnum--;
                        if (pnum == 0)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Fill;
                        }
                        if (pnum == 1)
                        {
                            panelmiddleright.Controls[p].Dock = DockStyle.Bottom;
                        }
                    }
                }
            }            
        }

        
        
        private void Simulate_Drag_Drop(object sender)
        {
            MessageBox.Show("Simulating Drag Drop Event");
        }

        private void Dashboard_Shown(object sender, EventArgs e)
        {
            //Run RPC here to import all of the modules
            //Data that will be needed for each module:
            // 1) Unique Button Tag (name for module)
            // 2) What goes on the button- may want to create a button class with an icon instead of text
            //byte array for button picture in one field, text in another
            // 3) Module Type - is it DataGrid or RichTextList
            // 4) If DataGrid, List of DataGrid column headers
            // 5) If DataGrid, the rowcount (I believe this is the starting count only, always 1?)
            // 6) RPC Name used to populate the data
            // 7) RPC Name used to get all locations (likely wards) that could be chosen
            // 8) RPC Name used to get the saved user-selected locations for this module
            // 9) RPC to get all possible values to filter on
            //10) RPC to get filter values the user has saved
            //11) RPC to save filter values the user has chosen
            //12) RPC to run when clicking on the value in the column
            //13) Override text for locations label

            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res3 = GlobalVars.dc.CallRPC("XUS INTRO MSG", NO_ARG);
            string[] results = Common.Split(res3);
            if (results.Length > 0)
            {
                this.Text = results[0].Trim();
            }

            NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            res3 = GlobalVars.dc.CallRPC("C9C MODULE DEFINITIONS", NO_ARG);
            results = Common.Split(res3);
            if (results.Length > 0)
            {
                GlobalVars.AllModules = new string[results.Length];
                for (int m = 0; m < results.Length; m++)
                {                    
                    string[] moduleDef = results[m].Split('^');
                    if (moduleDef.Length > 10)
                    {
                        GlobalVars.AllModules[m] = results[m];
                        Button newButton = new Button();
                        newButton.Parent = panelbottom;
                        newButton.Location = new System.Drawing.Point((m * 62) + 4, 8);
                        newButton.Size = new System.Drawing.Size(60, 60);
                        newButton.UseVisualStyleBackColor = true;
                        newButton.Tag = moduleDef[0] + '^' + moduleDef[1];  //IEN^Name of the module to show as button tag;
                        newButton.Text = "";
                        newButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonModules_MouseDown);
                        
                        ToolTip thisToolTip = new ToolTip();
                        thisToolTip.SetToolTip(newButton, moduleDef[1]);

                        string[] args1 = { moduleDef[0] };  //this needs to be the ien of the module
                        string THIS_ARG = RpcFormatter.FormatArgs(true, args1);
                        res3 = GlobalVars.RunRPC("C9C BUTTON IMAGE BYTE STRING", THIS_ARG);
                        string[] results2 = Common.Split(res3);
                        if (results2.Length > 0)
                        {                            
                            string[] sarray = results2[0].Split('^');
                            byte[] barray = new byte[sarray.GetLength(0)];
                            for (int b = 0; b < sarray.GetLength(0); b++)
                            {
                                if (sarray[b] != "")
                                {
                                    barray[b] = (byte)Int32.Parse(sarray[b]);
                                }
                            }
                            if (barray.GetLength(0) > 0)
                            {
                                MemoryStream ms = new MemoryStream(barray);
                                Image image = Image.FromStream(ms);
                                newButton.Image = image;
                            }
                        }
                        
                    }
                }

                //---------------------------------Start Get User Settings-----------------------------------------
                NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
                string res9 = GlobalVars.RunRPC("C9C GET DASHBOARD PREFERENCES", NO_ARG);
                string[] settingsResults = Common.Split(res9);
                if (settingsResults.Length > 0)
                {
                    string[] splitPrefs = settingsResults[0].Split('^');
                    //WWIDTH^WHEIGHT^FONT^SPLITTER LEFT HORIZONTAL POSITION^SPLITTER RIGHT HORIZONTAL POSITION^SPLITTER VERTICAL POSITION^WINDOWLEFT^WINDOWTOP
                    if (splitPrefs.Count() == 8)
                    {
                        int oneScreenWidth = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Width /
                                         Screen.AllScreens.Count();
                        int oneScreenHeight = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Height;

                        int i = int.TryParse(splitPrefs[0], out i) ? i : oneScreenWidth;
                        if (i < 400)
                        {
                            i = oneScreenWidth;
                        }
                        
                        if (Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Width < i)
                        {
                            i = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Width;
                        }
                        this.Width = i;


                        i = int.TryParse(splitPrefs[1], out i) ? i : oneScreenHeight;
                        if (i < 400)
                        {
                            i = oneScreenHeight;
                        }
                        if (Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Height < i)
                        {
                            i = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Height;
                        }
                        this.Height = i;
                        
                        //Place the window in the saved position
                        i = int.TryParse(splitPrefs[6], out i) ? i : 0;
                        this.Left = i;

                        i = int.TryParse(splitPrefs[7], out i) ? i : 0;
                        this.Top = i;

                        //Check to be sure the top left corner is at least nearly on-screen
                        Boolean onScreen = false;
                        Point p = new Point(this.Left + 20, this.Top + 20);
                        
                        foreach (Screen s in Screen.AllScreens)
                        {
                            if (p.X < s.Bounds.Right && p.X > s.Bounds.Left && p.Y > s.Bounds.Top && p.Y < s.Bounds.Bottom)
                            {
                                onScreen = true;
                            }                            
                        }

                        if (onScreen == false)
                        {
                            this.Top = 0;
                            this.Left = 0;                            
                            this.Width = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Width / 
                                         Screen.AllScreens.Count();                            
                            this.Height = Screen.AllScreens.Select(Screen => Screen.Bounds).Aggregate(Rectangle.Union).Height;
                        }

                        int FontSize = 8;
                        switch (splitPrefs[2])
                        {
                            case "8":
                                FontSize = 8;
                                break;
                            case "9":
                                FontSize = 9;
                                break;
                            case "10":
                                FontSize = 10;
                                break;
                            case "11":
                                FontSize = 11;
                                break;
                            case "12":
                                FontSize = 12;
                                break;
                            default:
                                FontSize = 8;
                                break;
                        }
                        Font fnt = new Font(this.Font.FontFamily, FontSize);

                        foreach (Control c in this.Controls)
                        {
                            c.Font = fnt;
                        }

                    }
                    //Module IEN^Module Instance^Sort Column^Sort Direction^Location Selection IEN
                    for (int h = 1; h < settingsResults.Length; h++) //create the modules
                    {
                        string[] thismodule = settingsResults[h].Split('^');
                        if (thismodule.Count() == 5)
                        {
                            string mien = thismodule[0];
                            string inst = thismodule[1];
                            int jj;
                            if ((Int32.TryParse(mien, out jj)) && (Int32.TryParse(inst, out jj)))
                            {
                                string scol = thismodule[2];
                                string sdir = thismodule[3];
                                string loc = thismodule[4];

                                //now create the module
                                string ButtonTag = "";
                                for (int k = 0; ((k < panelbottom.Controls.Count) && (ButtonTag == "")); k++)
                                {
                                    if (panelbottom.Controls[k] is Button)
                                    {
                                        string[] splitTag = panelbottom.Controls[k].Tag.ToString().Split('^');
                                        if (splitTag.Count() > 1)
                                        {
                                            if (splitTag[0] == mien)
                                            {
                                                ButtonTag = panelbottom.Controls[k].Tag.ToString();
                                            }
                                        }
                                    }
                                }

                                string[] thisbuttontag = ButtonTag.Split('^');
                                DragEventArgs q = new DragEventArgs(null, 0, 0, 0, DragDropEffects.None, DragDropEffects.None);
                                if (ExistingPanels.Count < 4)
                                {
                                    NumberOfPanels++;
                                }
                                create_module(thisbuttontag, ref q, scol, sdir, loc);
                            }
                        }
                    }
                    
                    if (splitPrefs.Count() == 8)
                    { 
                        int i = int.TryParse(splitPrefs[3], out i) ? i : 291;
                        if (i < 0)
                        {
                            i = 291;
                        }
                        
                        splitterhorizontal1.SplitPosition = i;
                        

                        i = int.TryParse(splitPrefs[4], out i) ? i : 291;
                        if (i < 0)
                        {
                            i = 291;
                        }
                        
                        splitterhorizontal2.SplitPosition = i;

                        i = int.TryParse(splitPrefs[5], out i) ? i : 0;
                        if (i < 0)
                        {
                            i = 0;
                        }
                        
                        splittervertical.SplitPosition = i;
                    }
                }
                if (NumberOfPanels == 0)
                {
                    //if there are no saved user modules
                    
                    string ButtonTag = "";
                    for (int k = 0; ((k < panelbottom.Controls.Count) && (ButtonTag == "")); k++)
                    {
                        if (panelbottom.Controls[k] is Button)
                        {
                            string[] splitTag = panelbottom.Controls[k].Tag.ToString().Split('^');
                            if (splitTag.Count() > 1)
                            {
                                //use the first module button found to open one instance
                                ButtonTag = panelbottom.Controls[k].Tag.ToString();                                
                            }
                        }
                    }

                    string[] thisbuttontag = ButtonTag.Split('^');
                    DragEventArgs q = new DragEventArgs(null, 0, 0, 0, DragDropEffects.None, DragDropEffects.None);
                    if (ExistingPanels.Count < 4)
                    {
                        NumberOfPanels++;
                    }
                    create_module(thisbuttontag, ref q, "", "", "");
                }

                
                //---------------------------------End Get User Settings-----------------------------------------
            }
            
        }

        private void buttonClosePanel_Click(object sender, EventArgs e)
        {
            string thisPanelName = (sender as Button).Parent.Parent.Name;
            if (thisPanelName != "")
            {
                for (int i = ExistingPanels.Count - 1;i>=0; i--)
                {
                    if (ExistingPanels[i].Name == thisPanelName)
                    {                        
                        ExistingPanels.RemoveAt(i);
                    }
                }
                for (int j = GlobalVars.ActiveModules.Count - 1;j>=0; j--)
                {
                    string[] mSplit = GlobalVars.ActiveModules[j].Split('^');
                    if(mSplit.Length>2)
                    {
                        if(mSplit[2]==thisPanelName)  
                        {
                            GlobalVars.ActiveModules.RemoveAt(j);
                        }
                    }
                }
                //Also need to remove from GlobalVars.RunningModuleList
                for (int rm = GlobalVars.RunningModuleList.Count - 1; rm >= 0; rm--)
                {                    
                    if (GlobalVars.RunningModuleList[rm].ThisParentPanel.Parent.Name == thisPanelName) 
                    {
                        GlobalVars.RunningModuleList.RemoveAt(rm);
                    }
                }
            }
            
            
            (sender as Button).Parent.Parent.Dispose();
            if (NumberOfPanels > 0)
            {
                NumberOfPanels--;
            }
            if (NumberOfPanels > 0)
            {
                ModuleWasDroppedOrRemoved(sender, e);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int fntSize = 8;
            
            Font fnt = new Font(toolStripMenuItem2.Font.FontFamily, fntSize);

            foreach (Control c in this.Controls)

            {                
                c.Font = fnt;
            }
            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int fntSize = 9;

            Font fnt = new Font(toolStripMenuItem2.Font.FontFamily, fntSize);

            foreach (Control c in this.Controls)

            {
                c.Font = fnt;
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            int fntSize = 10;

            Font fnt = new Font(toolStripMenuItem2.Font.FontFamily, fntSize);

            foreach (Control c in this.Controls)

            {
                c.Font = fnt;
            }
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            int fntSize = 11;

            Font fnt = new Font(toolStripMenuItem2.Font.FontFamily, fntSize);

            foreach (Control c in this.Controls)

            {
                c.Font = fnt;
            }
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            int fntSize = 12;

            Font fnt = new Font(toolStripMenuItem2.Font.FontFamily, fntSize);

            foreach (Control c in this.Controls)

            {
                c.Font = fnt;
            }
        }

        private void timerKeepAlive_Tick(object sender, EventArgs e)
        {
            string NO_ARG = RpcFormatter.FormatArgs(true, new string[0]);
            string res3 = GlobalVars.dc.CallRPC("XOBV TEST PING", NO_ARG);
        }

        private void buttonRefreshAll_Click(object sender, EventArgs e)
        {
            buttonRefreshAll.Enabled = false;
            timerRefresh.Enabled = false;
            Application.UseWaitCursor = true;
            Application.DoEvents();

            //run the update RPC on each module that is displayed

            for (int rm = 0; rm < GlobalVars.RunningModuleList.Count; rm++)
            {
                String RPCToRun = GlobalVars.RunningModuleList[rm].GetRPCToPopulateValues();
                if (RPCToRun != "")
                {

                    if (GlobalVars.RunningModuleList[rm].ThisModuleType == "DataGrid")
                    {                        
                        GlobalVars.RunningModuleList[rm].UpdateDataGrid(RPCToRun,
                                                         ref GlobalVars.RunningModuleList[rm].dynamicGridView,
                                                         GlobalVars.RunningModuleList[rm].ModuleColumns,
                                                         GlobalVars.RunningModuleList[rm].GetLocationsCombo(),
                                                         GlobalVars.RunningModuleList[rm].ThisModuleIEN);
                    }
                    if (GlobalVars.RunningModuleList[rm].ThisModuleType == "RichTextList")
                    {
                        ComboBox LocCombo = GlobalVars.RunningModuleList[rm].GetLocationsCombo();
                        Panel grandparentpanel = (Panel)LocCombo.Parent.Parent;                        
                        Panel myParent;
                        for (int p = 0; p < grandparentpanel.Controls.Count; p++)
                        {
                            if (grandparentpanel.Controls[p] is Panel)
                            {
                                if (grandparentpanel.Controls[p].Name.IndexOf("panelBody") == 0)
                                {
                                    myParent = (Panel)grandparentpanel.Controls[p];
                                    String actText = GlobalVars.RunningModuleList[rm].GetButtonActionText();
                                    String actRPC = GlobalVars.RunningModuleList[rm].GetButtonActionRPC();
                                    GlobalVars.RunningModuleList[rm].UpdateRichTextList(RPCToRun, myParent,
                                            LocCombo, GlobalVars.RunningModuleList[rm].ThisModuleIEN, actText, actRPC);
                                }
                            }
                        }                         
                    }
                    if (GlobalVars.RunningModuleList[rm].GetLocationsCombo().Parent.Controls.ContainsKey("lastRefreshedLabel"))
                    {
                        Label labelToUpdate = ((Label)(GlobalVars.RunningModuleList[rm].GetLocationsCombo().Parent.Controls.Find("lastRefreshedLabel", true).FirstOrDefault()));
                        labelToUpdate.Text = "Last Refreshed at: " + DateTime.Now.ToString("h:mm tt");
                    }

                }
            }            
                        
            Application.DoEvents();
            buttonRefreshAll.Enabled = true;
            timerRefresh.Enabled = true;
            Application.UseWaitCursor = false;
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            String saveFont = "8";
            switch (buttonRefreshAll.Font.Size)         
            {
                case 8:
                    saveFont = "8";
                    break;
                case 9:
                    saveFont = "9";
                    break;
                case 10:
                    saveFont = "10";
                    break;
                case 11:
                    saveFont = "11";
                    break;
                case 12:
                    saveFont = "12";
                    break;
                default:
                    saveFont = "8";
                    break;
            }
            String saveWWidth = "0";
            if (this.Width>0)
            {
                saveWWidth = this.Width.ToString();
            }
            String saveWHeight = "0";
            if (this.Height>0)
            {
                saveWHeight = this.Height.ToString();
            }

            String saveLeft = this.Left.ToString();
            String saveTop = this.Top.ToString();
            
            String saveSLHoriz = "0";
            if (splitterhorizontal2.Top>0)
            {
                saveSLHoriz = splitterhorizontal2.Top.ToString();
            }
            String saveSRHoriz = "0";
            if (splitterhorizontal1.Top > 0)
            {
                saveSRHoriz = splitterhorizontal1.Top.ToString();
            }
            String saveSVert = "0";
            if (splittervertical.Left > 0)
            {
                saveSVert = splittervertical.Left.ToString();
            }
            int mcount = GlobalVars.RunningModuleList.Count;
            string[] SendListToRPC = new string[mcount]; //Send arguments as List Type

            for (int q = 0; q < mcount; q++)
            {
                string[] mienSplit = GlobalVars.RunningModuleList[q].ThisModuleIEN.Split(';'); //ModuleIEN;Instance
                if (mienSplit.Count() == 2)
                {                    
                    string sortcolumn = GlobalVars.RunningModuleList[q].Modulegridsortcolumnindex.ToString();
                    string sortdirection = GlobalVars.RunningModuleList[q].Modulegridsortorder.ToString();
                    string locationIEN = GlobalVars.RunningModuleList[q].ModulelocComboIndex.ToString();
                    SendListToRPC[q] = mienSplit[0] + "^" +
                                       mienSplit[1] + "^" +                                       
                                       sortcolumn + "^" +
                                       sortdirection + "^" +
                                       locationIEN;
                }                                   
            }

            string THIS_ARG = RpcFormatter.FormatArgs(true, saveFont, saveWWidth, saveWHeight, saveSLHoriz, saveSRHoriz, saveSVert, saveLeft, saveTop, SendListToRPC);  
            string res9 = GlobalVars.RunRPC("C9C SAVE DASHBOARD PREFERENCES", THIS_ARG);

            if (res9 == "1") //success
            {

            }

        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            Application.UseWaitCursor = true;
            
            Application.DoEvents();
            buttonRefreshAll.Enabled = false;
            timerRefresh.Enabled = false;

            buttonRefreshAll_Click(sender, e);

            buttonRefreshAll.Enabled = true;
            
            Application.DoEvents();
            timerRefresh.Enabled = true;
            Application.UseWaitCursor = false;
        }
    }
}
