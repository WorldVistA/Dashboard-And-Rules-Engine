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

namespace RulesEngine
{
    class ClassFormula
    {
        public String FormulaName;
        public String FileSource;
        public String FormulaScope;
        public String IENString;
        public List<String> ScopeList;
        
                
        public class OneRule
        {
            public String ThisRuleText;
            public String ThisRuleIEN;
            public int ThisAdmitOffsetInDays;
            public String ThisstrCalendarDate;
            public String ThisstrDayofYear;
            public String ThisstrDayOfMonth;
            public String ThisstrDayOfWeek;
            public int ThisAdmitOffsetInHours;
            public int ThisEventOffsetInDays;
            public int ThisEventOffsetInHours;
            public String ThisActiveOrderableItem;

            public OneRule(String RuleText, int AdmitOffsetInDays, String strCalendarDate,
                           String strDayOfYear, String strDayOfMonth, String strDayOfWeek,
                           int AdmitOffsetInHours,
                           int EventOffsetInDays,
                           int EventOffsetInHours, String ActiveOrderableItem)
            {
                ThisRuleText = RuleText.Split('~')[1];
                ThisRuleIEN = RuleText.Split('~')[0];
                
                ThisAdmitOffsetInDays = AdmitOffsetInDays;
                ThisstrCalendarDate = strCalendarDate;
                ThisstrDayofYear = strDayOfYear;
                ThisstrDayOfMonth = strDayOfMonth;
                ThisstrDayOfWeek = strDayOfWeek;
                ThisAdmitOffsetInHours = AdmitOffsetInHours;
                ThisEventOffsetInDays = EventOffsetInDays;
                ThisEventOffsetInHours = EventOffsetInHours;
                ThisActiveOrderableItem = ActiveOrderableItem;
            }
        }

        public List<OneRule> Rules = new List<OneRule>();
        public ClassFormula(String ThisFileSource, String ThisFormulaName, String ThisFormulaScope, List<String> ThisScopeList,
                         List<String> ThisRuleList)
        {
            ScopeList = new List<String>();
            
            for (int i = 0; i < ThisRuleList.Count; i++)
            {
                FormulaName = ThisFormulaName;
                FileSource = ThisFileSource;
                String[] ScopeSplit = ThisFormulaScope.Split(';');
                FormulaScope = ScopeSplit[0];
                IENString = ScopeSplit[1] + ';' + ScopeSplit[2] + ';' + ScopeSplit[3] +';' +
                            ScopeSplit[4] + ';' + ScopeSplit[5];
                ScopeList.AddRange(ThisScopeList);
                

                String RuleText = "";
                int AdmitOffsetInDays = 0;
                String strCalendarDate = "";
                String strDayOfYear = "";
                String strDayOfMonth = "";
                String strDayOfWeek = "";
                int AdmitOffsetInHours = 0;
                int EventOffsetInDays = 0;
                int EventOffsetInHours = 0;
                String ActiveOrderableItem = "";
                                    
                String[] SplitList = ThisRuleList[i].Split('^');
                if ((SplitList.Count() > 6) & (SplitList[0] != ""))
                {
                    RuleText = SplitList[0];
                    int.TryParse(SplitList[1], out AdmitOffsetInDays);
                    strCalendarDate = SplitList[2];
                    strDayOfYear = SplitList[3];
                    strDayOfMonth = SplitList[4];
                    strDayOfWeek = SplitList[5];
                    int.TryParse(SplitList[6], out AdmitOffsetInHours);
                    int.TryParse(SplitList[7], out EventOffsetInDays);
                    int.TryParse(SplitList[8], out EventOffsetInHours);
                    ActiveOrderableItem = SplitList[9];

                    Rules.Add(new OneRule(RuleText, AdmitOffsetInDays, strCalendarDate,
                                          strDayOfYear, strDayOfMonth, strDayOfWeek,
                                          AdmitOffsetInHours, EventOffsetInDays, 
                                          EventOffsetInHours, ActiveOrderableItem));
                }                
                
            }
        }       

    }
}
