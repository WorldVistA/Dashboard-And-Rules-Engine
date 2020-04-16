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
using Medsphere.OpenVista.Remoting;
using Medsphere.OpenVista.Shared;
using System.Windows.Forms;

namespace Dashboard
{
    class GlobalVars
    {
        public static DataConnection dc;
        public static string SelectedDivision;
        public static string[] AllModules;
        public static int NameCounter = 0;
        public static List<String> ActiveModules = new List<String>();
        public static List<Module> RunningModuleList = new List<Module>();
        public static int retries = 0;

        public static string RunRPC(string RPCName, string Arguments)
        {
            string strRet;
            strRet = "";
            try
            {
                strRet = GlobalVars.dc.CallRPC(RPCName, Arguments);
                retries = 0;
            }
            catch
            {
                retries++;
                if (retries > 3) //allow three total retries
                {
                    MessageBox.Show("Application Closed Due to Trouble Calling Remote Procedure: " + RPCName + "!");
                    Application.Exit();
                }
            }
            return strRet;
        }
     }
}
