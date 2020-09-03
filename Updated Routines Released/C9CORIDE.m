C9CORIDE ;CRH JHT 2019 - override code for notes due rules shown logic, nearly due logic, overdue logic, and acceptable base date delta logic
 ;CRH 20200901;Build 2
 ;Copyright [2020] [Central Regional Hospital, State of North Carolina]
 ;
 ;Licensed under the Apache License, Version 2.0 (the "License")
 ; you may not use this file except in ;compliance with the License.
 ;You may obtain a copy of the License at
 ;
 ;http://www.apache.org/licenses/LICENSE-2.0
 ;
 ;Unless required by applicable law or agreed to in writing, software
 ;distributed under the License is ;distributed on an "AS IS" BASIS,
 ;WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 ;See the License for the specific language governing permissions and
 ;limitations under the License.
 Q ;no entry from top
 
 ;NEARLY DUE LOGIC: I (($$NOW^XLFDT>$$FMADD^XLFDT(BASEDATE,-1))&($$NOW^XLFDT<BASEDATE)) S
 ;C9CCHECK=1
 ;OVERDUE LOGIC: I (($$NOW^XLFDT>BASEDATE)!($$NOW^XLFDT=BASEDATE)) S C9CCHECK=1
 ;ACCEPTABLE BASEDATE DELTA: S ABD="2.0"
 
SHOWN(RPOINT,TITLIEN,BASEDATE,OFFSET,DBDAYS,DBHOURS,DADAYS,DAHOURS) ;return 0 for false, 1 for true
 ;RPOINT - pointer to rule definition if file 300892
 ;TITLIEN - pointer to document definition in file 8925.1
 ;BASEDATE - current due date being evaluated in fileman format
 ;OFFSET - number of days or hours for X days, X hours rules, otherwise zero
 ;DBDAYS - default number of days before
 ;DBHOURS - default number of hours before
 ;DADAYS - default number of days after
 ;DAHOURS - default number of hours after
 N C9CRET ;return value for this function
 S C9CRET=0
 I $G(TITLIEN)'>0 Q C9CRET
 I $G(RPOINT)'>0 Q C9CRET
 I $G(BASEDATE)'>0 Q C9CRET
 S OFFSET=+$G(OFFSET)
 S DBDAYS=+$G(DBDAYS)
 S DBHOURS=+$G(DBHOURS)
 S DADAYS=+$G(DADAYS)
 S DAHOURS=+$G(DAHOURS)
 N TITLE
 S TITLE=$$TRIM^XLFSTR($P($G(^TIU(8925.1,TITLIEN,0)),"^",1))
 I TITLE="" Q C9CRET
 N C9CNOW
 S C9CNOW=$$NOW^XLFDT
 ;Following sets the default logic for each rule
 N RULETEXT
 S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 I RULETEXT="" Q C9CRET
 ;Following runs the default code 
 ;Default days before and/or after trump default hours
 I ((DBDAYS>0)&(DADAYS>0)) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-DBDAYS))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,DADAYS))) S C9CRET=1
 E  I ((DBDAYS>0)&(DAHOURS>0)) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-DBDAYS))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,DAHOURS))) S C9CRET=1
 E  I ((DBHOURS>0)&(DADAYS>0)) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-DBHOURS))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,DADAYS))) S C9CRET=1
 E  I ((DBHOURS>0)&(DAHOURS>0)) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-DBHOURS))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,DAHOURS))) S C9CRET=1
 E  D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-7))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1
 ;Following sections are for individual titles.  If the title does not show up here, defaults remain
SWORIDE ;label used for override logic RPC
 I TITLE="MEDICAL REVIEW-PERIODIC" D
 .I ((RULETEXT="ONE TIME - ADMIT DATE + X DAYS")!(RULETEXT="ANNUALLY BASED ON ADMIT DATE + X DAYS")) D
 ..S C9CRET=0
 ..I OFFSET>90 D
 ...I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-15))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ....S C9CRET=1
 ..I OFFSET<91 D
 ...I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-7))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ....S C9CRET=1
 I TITLE="MEDICAL HISTORY AND PHYSICAL ASSESSMENT" D
 .I (RULETEXT="ANNUALLY BASED ON ADMIT DATE") D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-15))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ...S C9CRET=1
 I TITLE="NURSING ASSESSMENT 24/48 HOUR" D
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;shows up at midnight
 I TITLE="NURSING ASSESSMENT MONTHLY/WEEKLY" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I (((C9CNOW\1)=(BASEDATE\1))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ...S C9CRET=1 ;shows up on the day due
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;means this is a weekly, basedate is noon
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;shows up 12 hours ahead (midnight)
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-5,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;five days and 12 hours before
 I TITLE="NURSING ASSESSMENT INITIAL ANNUAL" D
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0
 ..I ((C9CNOW>BASEDATE)&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;shows up at admission time - code may allow two hours prior
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-15))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ...S C9CRET=1 ;shows up 15 days prior
 I TITLE="NURSING CARE PLANS" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I (((C9CNOW\1)=(BASEDATE\1))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ...S C9CRET=1 ;shows up on the day due
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0
 ..I ((C9CNOW>BASEDATE)&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;shows up at admission time - code may allow two hours prior
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;shows up 12 hours ahead (midnight)
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-5,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) S C9CRET=1 ;five days and 12 hours before
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-15))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,350))) D
 ...S C9CRET=1
 Q C9CRET 
NEARLY(RPOINT,TITLIEN,BASEDATE,OFFSET,DBDAYS,DBHOURS) ;return 0 for false, 1 for true
 ;RPOINT - pointer to rule definition if file 300892
 ;TITLIEN - pointer to document definition in file 8925.1
 ;BASEDATE - current due date being evaluated in fileman format
 ;OFFSET - number of days or hours for X days, X hours rules, otherwise zero
 ;DBDAYS - default number of days before
 ;DBHOURS - default number of hours before
 N C9CRET ;return value for this function
 S C9CRET=0
 I $G(TITLIEN)'>0 Q C9CRET
 I $G(RPOINT)'>0 Q C9CRET
 I $G(BASEDATE)'>0 Q C9CRET
 S DBDAYS=+$G(DBDAYS)
 S DBHOURS=+$G(DBHOURS)
 N TITLE
 S TITLE=$$TRIM^XLFSTR($P($G(^TIU(8925.1,TITLIEN,0)),"^",1))
 I TITLE="" Q C9CRET
 N C9CNOW
 S C9CNOW=$$NOW^XLFDT
 ;Following sets the default logic for each rule
 N RULETEXT
 S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 I RULETEXT="" Q C9CRET
 ;Following runs the default code
 ;Default days before and/or after trump default hours
 I (DBDAYS>0) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-DBDAYS))&(C9CNOW<BASEDATE)) S C9CRET=1
 E  I (DBHOURS>0) D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-DBHOURS))&(C9CNOW<BASEDATE)) S C9CRET=1
 E  D
 .I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,-1))&(C9CNOW<BASEDATE)) S C9CRET=1
 ;Following sections are for individual titles.  If the title does not show up here, defaults remain
NORIDE ;label used for override logic RPC
 I TITLE="MEDICAL REVIEW-PERIODIC" D
 .I ((RULETEXT="ONE TIME - ADMIT DATE + X DAYS")!(RULETEXT="ANNUALLY BASED ON ADMIT DATE + X DAYS")) D
 ..S C9CRET=0
 ..I (OFFSET<180) D
 ...I (((C9CNOW\1)=(BASEDATE\1))!((BASEDATE<C9CNOW)&(C9CNOW<($$FMADD^XLFDT(BASEDATE,7))))) D
 ....S C9CRET=1
 ..I (OFFSET>179) D
 ...I (((C9CNOW\1)=(BASEDATE\1))!((BASEDATE<C9CNOW)&(C9CNOW<($$FMADD^XLFDT(BASEDATE,15))))) D
 ....S C9CRET=1
 I TITLE="MEDICAL HISTORY AND PHYSICAL ASSESSMENT" D
 .I (RULETEXT="ANNUALLY BASED ON ADMIT DATE") D
 ..S C9CRET=0
 ..I (((C9CNOW\1)=(BASEDATE\1))!((BASEDATE<C9CNOW)&(C9CNOW<($$FMADD^XLFDT(BASEDATE,15))))) D
 ...S C9CRET=1
 I TITLE="NURSING ASSESSMENT 24/48 HOUR" D
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,12))) S C9CRET=1 ;goes yellow at noon, stays yellow until midnight
 I TITLE="NURSING ASSESSMENT MONTHLY/WEEKLY" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I ((C9CNOW\1)=(BASEDATE\1)) D
 ...S C9CRET=1 ;turns yellow on the day due only
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;means this is a weekly, basedate is noon
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,12))) S C9CRET=1 ;goes yellow at noon, stays yellow until midnight
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,5,12))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for five days after
 I TITLE="NURSING ASSESSMENT INITIAL ANNUAL" D
 .I RULETEXT="ANNUALLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I (((C9CNOW\1)=(BASEDATE\1))!((BASEDATE<C9CNOW)&(C9CNOW<($$FMADD^XLFDT(BASEDATE,15))))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for 15 days after
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0 ;this rule never turns yellow, just goes from white to red
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,15,12))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for fifteen days after
 I TITLE="NURSING CARE PLANS" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I ((C9CNOW\1)=(BASEDATE\1)) D
 ...S C9CRET=1 ;turns yellow on the day due only
 .I RULETEXT="ANNUALLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I (((C9CNOW\1)=(BASEDATE\1))!((BASEDATE<C9CNOW)&(C9CNOW<($$FMADD^XLFDT(BASEDATE,15))))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for 15 days after
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0 ;this rule never turns yellow, just goes from white to red
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I (OFFSET<3) D
 ...I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,12))) S C9CRET=1 ;goes yellow at noon, stays yellow until midnight
 ..I (OFFSET>2) D
 ...I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,,12))) S C9CRET=1 ;yellow on the day due
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,5,12))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for five days after
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I ((C9CNOW>$$FMADD^XLFDT(BASEDATE,,-12))&(C9CNOW<$$FMADD^XLFDT(BASEDATE,15,12))) D
 ...S C9CRET=1 ;turns yellow on the day due, stays yellow for fifteen days after
 Q C9CRET
OVERDUE(RPOINT,TITLIEN,BASEDATE,OFFSET) ;return 0 for false, 1 for true
 ;RPOINT - pointer to rule definition if file 300892
 ;TITLIEN - pointer to document definition in file 8925.1
 ;BASEDATE - current due date being evaluated in fileman format
 ;OFFSET - number of days or hours for X days, X hours rules, otherwise zero
 N C9CRET ;return value for this function
 S C9CRET=0
 I $G(TITLIEN)'>0 Q C9CRET
 I $G(RPOINT)'>0 Q C9CRET
 I $G(BASEDATE)'>0 Q C9CRET
 N TITLE
 S TITLE=$$TRIM^XLFSTR($P($G(^TIU(8925.1,TITLIEN,0)),"^",1))
 I TITLE="" Q C9CRET
 N C9CNOW
 S C9CNOW=$$NOW^XLFDT
 ;Following sets the default logic for each rule
 N RULETEXT
 S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 I RULETEXT="" Q C9CRET
 ;Following runs the default code
 I ((C9CNOW>BASEDATE)!(C9CNOW=BASEDATE)) S C9CRET=1
 ;Following sections are for individual titles.  If the title does not show up here, defaults remain
OVORIDE ;label used for override logic RPC
 I TITLE="MEDICAL REVIEW-PERIODIC" D
 .I ((RULETEXT="ONE TIME - ADMIT DATE + X DAYS")!(RULETEXT="ANNUALLY BASED ON ADMIT DATE + X DAYS")) D
 ..S C9CRET=0
 ..I (OFFSET<180) D
 ...I ((BASEDATE<C9CNOW)&(C9CNOW>($$FMADD^XLFDT(BASEDATE,7)))) D
 ....S C9CRET=1
 ..I (OFFSET>179) D
 ...I ((BASEDATE<C9CNOW)&(C9CNOW>($$FMADD^XLFDT(BASEDATE,15)))) D
 ....S C9CRET=1
 I TITLE="MEDICAL HISTORY AND PHYSICAL ASSESSMENT" D
 .I (RULETEXT="ANNUALLY BASED ON ADMIT DATE") D
 ..S C9CRET=0
 ..I ((BASEDATE<C9CNOW)&(C9CNOW>($$FMADD^XLFDT(BASEDATE,15)))) D
 ...S C9CRET=1
 I TITLE="NURSING ASSESSMENT 24/48 HOUR" D
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D
 ..S C9CRET=0
 ..I (($$FMADD^XLFDT(BASEDATE,,12))<C9CNOW) S C9CRET=1 ;noon plus 12 hours
 I TITLE="NURSING ASSESSMENT MONTHLY/WEEKLY" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I ((C9CNOW\1)>(BASEDATE\1)) D
 ...S C9CRET=1 ;turns red after the day due
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D
 ..S C9CRET=0
 ..I (($$FMADD^XLFDT(BASEDATE,,12))<C9CNOW) S C9CRET=1 ;noon plus 12 hours
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I (($$FMADD^XLFDT(BASEDATE,5,12))<C9CNOW) S C9CRET=1 ;5 days and 12 hours
 I TITLE="NURSING ASSESSMENT INITIAL ANNUAL" D
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0
 ..I (C9CNOW>($$FMADD^XLFDT(BASEDATE,,8))) S C9CRET=1 ;overdue after the 8 hours
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I (C9CNOW>$$FMADD^XLFDT(BASEDATE,15,12)) D ;overdue 15 days and 12 hours after
 ...S C9CRET=1
 I TITLE="NURSING CARE PLANS" D
 .I RULETEXT="WEEKLY BASED ON ADMIT DATE" D
 ..S C9CRET=0
 ..I ((C9CNOW\1)>(BASEDATE\1)) D
 ...S C9CRET=1 ;turns red after the day due
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET=0
 ..I (C9CNOW>($$FMADD^XLFDT(BASEDATE,,8))) S C9CRET=1 ;overdue after the 8 hours
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I (OFFSET<3) D
 ...I (C9CNOW>$$FMADD^XLFDT(BASEDATE,,12)) S C9CRET=1 ;goes red at midnight
 ..I (OFFSET>2) D
 ...I (C9CNOW>$$FMADD^XLFDT(BASEDATE,5,12)) D ;overdue 5 days and 12 hours after
 ...S C9CRET=1
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET=0
 ..I (C9CNOW>$$FMADD^XLFDT(BASEDATE,5,12)) D ;overdue 5 days and 12 hours after
 ...S C9CRET=1
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET=0
 ..I (C9CNOW>$$FMADD^XLFDT(BASEDATE,15,12)) D ;overdue 15 days and 12 hours after
 ...S C9CRET=1
 Q C9CRET
GETABD(RPOINT,TITLIEN,OFFSET,DEFLT) ;return ACCEPTABLE BASEDATE DELTA
 ;RPOINT - pointer to rule definition if file 300892
 ;TITLIEN - pointer to document definition in file 8925.1
 ;OFFSET - number of days or hours chosen in the gui for X DAYS or X HOURS, never both
 ;DEFLT - default ABD if not overridden below in the individual titles section
 N C9CRET ;return value for this function
 S C9CRET=$G(DEFLT)
 I C9CRET="" S C9CRET="2.4"
 I $G(TITLIEN)'>0 Q C9CRET
 I $G(RPOINT)'>0 Q C9CRET
 N TITLE
 S TITLE=$$TRIM^XLFSTR($P($G(^TIU(8925.1,TITLIEN,0)),"^",1))
 I TITLE="" Q C9CRET
 N C9CNOW
 S C9CNOW=$$NOW^XLFDT
 ;Following sets the default logic for each rule
 N RULETEXT
 S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 I RULETEXT="" Q C9CRET
 ;Default return value is set above
 ;Following sections are for individual titles.  If the title and rule do not show up here, defaults remain
ABDORIDE ;label used for override logic RPC
 I TITLE="NURSING ASSESSMENT 24/48 HOUR" D
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D
 ..S C9CRET="0.12" ;12 hours before noon and 12 hours after noon
 I TITLE="NURSING ASSESSMENT MONTHLY/WEEKLY" D
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D
 ..S C9CRET="0.12" ;12 hours before noon and 12 hours after noon
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET="5.5"
 I TITLE="NURSING ASSESSMENT INITIAL ANNUAL" D
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET="0.8" ;eight hours before and after admission
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET="15.5" ;15 days and 12 hours prior and after
 I TITLE="NURSING CARE PLANS" D
 .I RULETEXT="ONE TIME - ADMIT DATE + X HOURS" D
 ..S C9CRET="0.8" ;eight hours before and after admission
 .I RULETEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D ;for this rule, basedate is always noon
 ..S C9CRET=0
 ..I (OFFSET<3) D
 ...S C9CRET="0.12" ;12 hours before noon and 12 hours after noon
 ..I (OFFSET>2) D
 ...S C9CRET="0.12" ;12 hours before noon and 12 hours after noon
 .I RULETEXT="EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12" D
 ..S C9CRET="5.5" ;5 days and 5 hours prior and after
 .I RULETEXT="ANNUALLY BASED ON ADMISSION DAY" D
 ..S C9CRET="15.5" ;15 days and 12 hours prior and after
 I TITLE="MEDICAL REVIEW-PERIODIC" D
 .I ((RULETEXT="ONE TIME - ADMIT DATE + X DAYS")!(RULETEXT="ANNUALLY BASED ON ADMIT DATE + X DAYS")) D
 ..I OFFSET<91 S C9CRET="7.0"
 ..I OFFSET>90 S C9CRET="15.0"
 Q C9CRET 
GETOR(C9CRET,RPOINT,TITLIEN) ;get associated override code sections
 Q:$G(RPOINT)'>0
 Q:$G(TITLIEN)'>0
 ;first get title name
 N TITLE
 S TITLE=$$TRIM^XLFSTR($P($G(^TIU(8925.1,TITLIEN,0)),"^",1))
 Q:TITLE=""
 N RULETEXT
 S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 Q:RULETEXT=""
 N CNT
 S CNT=0
 S C9CRET($I(CNT))="Title: "_TITLE
 S C9CRET($I(CNT))="Rule: "_RULETEXT
 N BLNKCNT ;counts blank lines
 S BLNKCNT=0
 N DONE
 S DONE=0
 N TX
 ;start with shown logic
 S C9CRET($I(CNT))=""
 S C9CRET($I(CNT))="Following is override logic that determines timeframe for showing this entry:"
 S C9CRET($I(CNT))="-----------------------------------------------------------------------------"
 F TX=1:1 Q:((BLNKCNT>3)!(DONE>0))  D
 .N TEXTLINE
 .S TEXTLINE=$T(SWORIDE+TX)
 .I TEXTLINE="" S BLNKCNT=BLNKCNT+1
 .I $F(TEXTLINE,"NEARLY(")=8 S DONE=1
 .I $F(TEXTLINE,"TITLE="""_TITLE_"")>0 D
 ..S C9CRET($I(CNT))=TEXTLINE
 ..N BLINES ;blank lines
 ..S BLINES=0
 ..N SUBDONE
 ..S SUBDONE=0
 ..F TX=TX+1:1 Q:((BLINES>3)!(SUBDONE>0))  D
 ...S TEXTLINE=$T(SWORIDE+TX)
 ...I TEXTLINE="" S BLINES=BLINES+1 Q
 ...I $F(TEXTLINE,"NEARLY(")=8 S SUBDONE=1 S DONE=1 Q
 ...I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SUBDONE=1 Q
 ...I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")>0 D
 ....S C9CRET($I(CNT))=TEXTLINE
 ....N BL ;blank lines
 ....S BL=0
 ....N SD
 ....S SD=0
 ....F TX=TX+1:1 Q:((BL>3)!(SD>0))  D
 .....S TEXTLINE=$T(SWORIDE+TX)
 .....I TEXTLINE="" S BL=BL+1 Q
 .....I $F(TEXTLINE,"NEARLY(")=8 S SD=1 S SUBDONE=1 S DONE=1 Q
 .....I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SD=1 S SUBDONE=1 Q
 .....I $F(TEXTLINE,"RULETEXT=")>0 I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")'>0 S SD=1 S SUBDONE=1 Q
 .....S C9CRET($I(CNT))=TEXTLINE
 ;then check nearly due logic
 S C9CRET($I(CNT))=""
 S C9CRET($I(CNT))="Following is override logic that determines coloring this entry yellow:"
 S C9CRET($I(CNT))="-----------------------------------------------------------------------------"
 S BLNKCNT=0
 S DONE=0
 F TX=1:1 Q:((BLNKCNT>3)!(DONE>0))  D
 .N TEXTLINE
 .S TEXTLINE=$T(NORIDE+TX)
 .I TEXTLINE="" S BLNKCNT=BLNKCNT+1
 .I $F(TEXTLINE,"OVERDUE(")=9 S DONE=1
 .I $F(TEXTLINE,"TITLE="""_TITLE_"")>0 D
 ..S C9CRET($I(CNT))=TEXTLINE
 ..N BLINES ;blank lines
 ..S BLINES=0
 ..N SUBDONE
 ..S SUBDONE=0
 ..F TX=TX+1:1 Q:((BLINES>3)!(SUBDONE>0))  D
 ...S TEXTLINE=$T(NORIDE+TX)
 ...I TEXTLINE="" S BLINES=BLINES+1 Q
 ...I $F(TEXTLINE,"OVERDUE(")=9 S SUBDONE=1 S DONE=1 Q
 ...I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SUBDONE=1 Q
 ...I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")>0 D
 ....S C9CRET($I(CNT))=TEXTLINE
 ....N BL ;blank lines
 ....S BL=0
 ....N SD
 ....S SD=0
 ....F TX=TX+1:1 Q:((BL>3)!(SD>0))  D
 .....S TEXTLINE=$T(NORIDE+TX)
 .....I TEXTLINE="" S BL=BL+1 Q
 .....I $F(TEXTLINE,"OVERDUE(")=9 S SD=1 S SUBDONE=1 S DONE=1 Q
 .....I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SD=1 S SUBDONE=1 Q
 .....I $F(TEXTLINE,"RULETEXT=")>0 I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")'>0 S SD=1 S SUBDONE=1 Q
 .....S C9CRET($I(CNT))=TEXTLINE
 ;then check overdue logic
 S C9CRET($I(CNT))=""
 S C9CRET($I(CNT))="Following is override logic that determines coloring this entry red:"
 S C9CRET($I(CNT))="-----------------------------------------------------------------------------"
 S BLNKCNT=0
 S DONE=0
 F TX=1:1 Q:((BLNKCNT>3)!(DONE>0))  D
 .N TEXTLINE
 .S TEXTLINE=$T(OVORIDE+TX)
 .I TEXTLINE="" S BLNKCNT=BLNKCNT+1
 .I $F(TEXTLINE,"GETABD(")=8 S DONE=1
 .I $F(TEXTLINE,"TITLE="""_TITLE_"")>0 D
 ..S C9CRET($I(CNT))=TEXTLINE
 ..N BLINES ;blank lines
 ..S BLINES=0
 ..N SUBDONE
 ..S SUBDONE=0
 ..F TX=TX+1:1 Q:((BLINES>3)!(SUBDONE>0))  D
 ...S TEXTLINE=$T(OVORIDE+TX)
 ...I TEXTLINE="" S BLINES=BLINES+1 Q
 ...I $F(TEXTLINE,"GETABD(")=8 S SUBDONE=1 S DONE=1 Q
 ...I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SUBDONE=1 Q
 ...I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")>0 D
 ....S C9CRET($I(CNT))=TEXTLINE
 ....N BL ;blank lines
 ....S BL=0
 ....N SD
 ....S SD=0
 ....F TX=TX+1:1 Q:((BL>3)!(SD>0))  D
 .....S TEXTLINE=$T(OVORIDE+TX)
 .....I TEXTLINE="" S BL=BL+1 Q
 .....I $F(TEXTLINE,"GETABD(")=8 S SD=1 S SUBDONE=1 S DONE=1 Q
 .....I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SD=1 S SUBDONE=1 Q
 .....I $F(TEXTLINE,"RULETEXT=")>0 I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")'>0 S SD=1 S SUBDONE=1 Q
 .....S C9CRET($I(CNT))=TEXTLINE
 ;then check grace days(abd) logic
 S C9CRET($I(CNT))=""
 S C9CRET($I(CNT))="Following is override logic that determines grace days for this entry:"
 S C9CRET($I(CNT))="-----------------------------------------------------------------------------"
 S BLNKCNT=0
 S DONE=0
 F TX=1:1 Q:((BLNKCNT>3)!(DONE>0))  D
 .N TEXTLINE
 .S TEXTLINE=$T(ABDORIDE+TX)
 .I TEXTLINE="" S BLNKCNT=BLNKCNT+1
 .I $F(TEXTLINE,"GETOR(")=7 S DONE=1
 .I $F(TEXTLINE,"TITLE="""_TITLE_"")>0 D
 ..S C9CRET($I(CNT))=TEXTLINE
 ..N BLINES ;blank lines
 ..S BLINES=0
 ..N SUBDONE
 ..S SUBDONE=0
 ..F TX=TX+1:1 Q:((BLINES>3)!(SUBDONE>0))  D
 ...S TEXTLINE=$T(ABDORIDE+TX)
 ...I TEXTLINE="" S BLINES=BLINES+1 Q
 ...I $F(TEXTLINE,"GETOR(")=7 S SUBDONE=1 S DONE=1 Q
 ...I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SUBDONE=1 Q
 ...I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")>0 D
 ....S C9CRET($I(CNT))=TEXTLINE
 ....N BL ;blank lines
 ....S BL=0
 ....N SD
 ....S SD=0
 ....F TX=TX+1:1 Q:((BL>3)!(SD>0))  D
 .....S TEXTLINE=$T(ABDORIDE+TX)
 .....I TEXTLINE="" S BL=BL+1 Q
 .....I $F(TEXTLINE,"GETOR(")=7 S SD=1 S SUBDONE=1 S DONE=1 Q
 .....I $F(TEXTLINE,"TITLE=")>0 I $F(TEXTLINE,"TITLE="""_TITLE_"")'>0 S SD=1 S SUBDONE=1 Q
 .....I $F(TEXTLINE,"RULETEXT=")>0 I $F(TEXTLINE,"RULETEXT="""_RULETEXT_"")'>0 S SD=1 S SUBDONE=1 Q
 .....S C9CRET($I(CNT))=TEXTLINE
 Q 
