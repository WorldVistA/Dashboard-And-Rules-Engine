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

using System.Collections.Generic;
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;
using System.Windows.Forms;
using System.IO;

namespace RulesEngine
{
    class GlobalVars
    {
        public static DataConnection dc;
        public static string SelectedDivision;
        
        public static string[,] arrayRules;
        public static string strDUZ;
        public static string strAuthor;
        public static string[,] arrayTitles;
        public static string[,] arrayWards;
        public static string[,] arrayOrderables;
        public static List<string> listUniqueUnits;
        public static string[,] arrayUnits;
        public static List<ClassFormula> TheseFormulas = new List<ClassFormula>();
        public static string wardcurrentFormulaIENstring = "";
        public static string unitcurrentFormulaIENstring = "";
        public static string hospitalcurrentFormulaIENstring = "";

        public class LType
        {
            public string itemtext { get; set; }
            public string itemien { get; set; }
        }

        public static GlobalVars.LType chosenCohort;
        public static string chosenTitleIEN;
        public static int PTOnly = 0;

        public static string RunRPC(string RPCName, string Arguments)
        {
            string strRet;
            strRet = "";
            try
            {
                strRet = GlobalVars.dc.CallRPC(RPCName, Arguments);
            }
            catch
            {
                MessageBox.Show("Lost Connection to Server" + "When Running RPC: " + RPCName + "!");
                Application.Exit();
            }
            return strRet;
        }

        public static DialogResult ShowInputDialog(ref string input, string BoxLabel)
        {
            System.Drawing.Size size = new System.Drawing.Size(600, 100);
            Form inputBox = new Form();
            
            inputBox.StartPosition = FormStartPosition.CenterParent;

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.ClientSize = size;
            inputBox.Text = BoxLabel;

            Label lblBox = new Label();
            lblBox.Location = new System.Drawing.Point(5, 5);
            (lblBox as Label).Text = BoxLabel;
            lblBox.AutoSize = true;
            inputBox.Controls.Add(lblBox);

            TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 20 - lblBox.Width, 23);
            textBox.Location = new System.Drawing.Point(lblBox.Width + 5, 5);
            textBox.Text = input;
            textBox.MaxLength = 80;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 69);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 69);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }
    }
}
