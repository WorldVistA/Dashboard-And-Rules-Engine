C9CMODNT ;module RPCs for C9C DASHBOARD MODULE Notes Due by Team
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
RPCGT(C9CRET) ;get teams
 Q:DUZ'>0
 N TMS
 D PLTEAMS^ORWTPT(.TMS,DUZ)
 N CNT
 S CNT=0
 N TX
 S TX="" F  S TX=$O(TMS(TX)) Q:TX'>0  D
 .S CNT=CNT+1
 .S C9CRET(CNT)=$P(TMS(TX),"^",1)_"^"_$P(TMS(TX),"^",1)_": "_$P(TMS(TX),"^",2)
 Q
RPCGAT(C9CRET) ;get psychiatrist auto teams and subscribed teams
 Q:DUZ'>0
 Q:DUZ(2)'>0
 ;first get psychiatrist auto teams  TEAMIEN^TEAM
 N PTRET 
 N ARET,RX,HAVE,AIEN
 S RX=$$GETATTS^C9CALST(.ARET)
 I +RX>0 D
 .S RX=0 F  S RX=$O(ARET(RX)) Q:RX'>0  D
 ..S AIEN=$P(ARET(RX),"^",2)
 ..I AIEN>0 D
 ...N THISLIST
 ...S THISLIST=$$CHECK^C9CALST(AIEN)
 ...S HAVE=+THISLIST
 ...I HAVE>0 D
 ....I $$ACTIVE^XUSER(AIEN)>0 D ;active user
 .....I $P(THISLIST,"^",2)'="" D
 ......S PTRET($P(THISLIST,"^",2),$P(THISLIST,"^",1))=THISLIST ;TEAMIEN^TEAM
 N TMS
 D PLTEAMS^ORWTPT(.TMS,DUZ) ;gets user's subscribed teams TEAMIEN^TEAM
 ;now alphabetize these teams
 N AX
 S AX=0 F  S AX=$O(TMS(AX)) Q:AX'>0  D
 .I $P(TMS(AX),"^",2)'="" D
 ..S PTRET($P(TMS(AX),"^",2),$P(TMS(AX),"^",1))=TMS(AX)
 N CNT
 S CNT=0
 N TNAME
 S TNAME="" F  S TNAME=$O(PTRET(TNAME)) Q:TNAME=""  D
 .N TIEN
 .S TIEN=0 F  S TIEN=$O(PTRET(TNAME,TIEN)) Q:TIEN'>0  D
 ..S CNT=CNT+1
 ..S C9CRET(CNT)=TIEN_"^"_TNAME_"~"_TIEN
 Q
RPCND(C9CRET,TEAM,FILTERS) ;get notes due data
 ;C9CRET - return array
 ;TEAM - team name~team ien
 ;FILTERS - zero-based list of TIU DOCUMENT DEFINITION ien^Name to include in results
 ;Patient^Note Title^Due^Ward
 Q:$G(TEAM)=""
 Q:$G(DUZ(2))'>0
 N C9CI
 S C9CI=0
 I $G(FILTERS(0))="" D ;no filters so use all titles that have rules
 .;First get all titles with rules engine rules defined
 .N TITLES
 .D RPCAF(.TITLES)
 .;remove trailing spaces since some titles seem to have them
 .N TX
 .S TX=0 F  S TX=$O(TITLES(TX)) Q:TX'>0  D
 ..N TL
 ..S TL=$L(TITLES(TX))
 ..I $E(TITLES(TX),TL)=" " S TITLES(TX)=$E(TITLES(TX),1,TL-1)
 .N RP
 .S RP=0 F  S RP=$O(TITLES(RP)) Q:RP'>0  D
 ..S FILTERS(RP-1)=TITLES(RP)
 Q:($G(FILTERS(0)))=""
 ;For each of these titles compare ward patients against cohort rules, then ward, then unit, then hospital
 N CIEN
 S CIEN=$O(^TIU(8925.6,"B","COMPLETED",0)) ;note status
 Q:CIEN'>0
 ;get patients on the team DFN^PATIENT NAME
CHKTEAM
 N TEAMIEN
 S TEAMIEN=$P($G(TEAM),"~",2)
 Q:TEAMIEN'>0
 N TPATS
 D TEAMPTS^ORQPTQ1(.TPATS,TEAMIEN)
 Q:'$D(TPATS(1))
 Q:TPATS(1)="^No patients found."
 ;only need the DFNs
 N TD
 S TD=0 F  S TD=$O(TPATS(TD)) Q:TD'>0  D
 .S TPATS(TD)=+TPATS(TD)
 N RNOW
 S RNOW=$$NOW^XLFDT
 N YEAR ;year
 S YEAR=$E(RNOW,2,3)
 N CENTURY
 S CENTURY=$E(RNOW,1,1)
 N DHMS; month day hour minutes seconds
 S DHMS=$E(RNOW,4,99)
 ;Now loop through the patients on the selected team
 N PX
 S PX=0 F  S PX=$O(TPATS(PX)) Q:PX'>0  D        
 .N PAT
 .S PAT=TPATS(PX)
 .Q:PAT'>0
 .;get the patient's ward and unit
 .N WARD
 .N WARDIEN
 .S WARDIEN=-1
 .S WARD=$P($G(^DPT(PAT,.1)),"^",1)
 .I WARD'="" D ;must be an inpatient 
 ..S WARDIEN=$O(^DIC(42,"B",WARD,0))
 .I WARDIEN=-1 S WARD="Outpatient"
 .N FX	
 .N UNIT ;bedsection in file 42
 .S UNIT="Undefined"
 .I WARDIEN>0 D
 ..S UNIT=$P($G(^DIC(42,WARDIEN,0)),"^",2)
 .N DIV
 .S DIV=DUZ(2)
 .Q:DIV'>0
 .S FX="" F  S FX=$O(FILTERS(FX)) Q:FX=""  D ;if included in filters (this is a zero-based array)
 ..;go through each formula
 ..;find this title in C9C RULES ENGINE FILE
 ..N OFFSET ;CRH JHT 2019 - for override logic
 ..S OFFSET=0 ;CRH JHT 2019 - for override logic
 ..N TITLIEN,TITLNAME
 ..S TITLIEN=$P(FILTERS(FX),"^",1)
 ..S TITLNAME=$P(FILTERS(FX),"^",2)
 ..N REIEN S REIEN=$O(^C9C(300890,"B",TITLIEN_";TIU(8925.1,",0))
 ..I (REIEN>0) I ($P($G(^C9C(300890,REIEN,4)),"^",1)'>0) D ;not inactive
 ...;check for required health factor
 ...N HFACTIEN ;health factor check
 ...S HFACTIEN=$P($G(^C9C(300890,REIEN,5)),"^",1) ;health factor check
 ...;check for reminder cohort
 ...N REMIEN
 ...S REMIEN=$P($G(^C9C(300890,REIEN,6)),"^",1) ;reminder cohort to check
 ...K DEFARR ;JHT 08-24-2020 for new PXRM logic repair
 ...N DEFARR
 ...N RESOLU
 ...S RESOLU=1 ;assume in cohort unless proven otherwise below
 ...N CLEVEL ;cohort level, 0=no cohort, 1=in cohort, 2=not in cohort ;mar 3,2020
 ...S CLEVEL=0 ;start by assuming no cohort ;mar 3,2020
 ...I REMIEN>0 D
 ....N PXRMDEFS ;JHT 08-24-2020 for new PXRM logic repair
 ....K FIEVAL,^TMP("PXRHM",$J,REMIEN) I $G(PXRMID)'="" K ^TMP(PXRMID,$J) ;JHT 08-24-2020 for new PXRM logic repair
 ....S CLEVEL=1 ;start by assuming in cohort ;mar 3,2020
 ....N DATE ;must be newed but not populated to return current value
 ....N PXRMDEBG ;must be defined and set or PXRMID won't be set
 ....S PXRMDEBG=1
 ....D DEF^PXRMLDR(REMIEN,.DEFARR) ;set up variables for this reminder check
 ....N FIEVAL
 ....D EVAL^PXRM(PAT,.DEFARR,5,1,.FIEVAL)
 ....I $G(PXRMID)'="" D
 .....N RESULT
 .....S RESULT=$P($G(^TMP(PXRMID,$J,REMIEN,"PATIENT COHORT LOGIC")),"^",1)
 .....I RESULT=0 D
 ......S RESOLU=0
 ......S CLEVEL=2 ;not in cohort ;mar 3,2020
 ......;check to see if we need to process pass-through reminders anyway
 ......I $P($G(^C9C(300890,REIEN,7)),"^",1)>0 D
 .......S RESOLU=2 ;yes, process pass-thru rules only
 .....K ^TMP(PXRMID,$J) K FIEVAL ;needs to be new each time
 ...Q:RESOLU=0 ;patient not in reminder cohort for this title
 ...N PTONLY
 ...S PTONLY=0
 ...I RESOLU=2 S PTONLY=1 ;pass-throughs only
 ...;see if any of the rules involve special orderable item cohorts as this is the most granular case
 ...N OCOHORT ;FORMULA IEN^RULE IEN^ORDERABLE ITEM IEN
 ...N OCNT ;OCOHORT counter
 ...S OCNT=0
 ...I WARDIEN>0 D
 ....N WF ;ward formula
 ....S WF=0 F  S WF=$O(^C9C(300890,REIEN,1,WF)) Q:WF'>0  D
 .....;check formula wards to see if applicable
 .....I $O(^C9C(300890,REIEN,1,WF,2,"B",WARDIEN,0))>0 D ;ward included in this formula
 ......N FR ;formula rule
 ......S FR=0 F  S FR=$O(^C9C(300890,REIEN,1,WF,1,FR)) Q:FR'>0  D
 .......N RP ;rule pointer value
 .......S RP=$P($G(^C9C(300890,REIEN,1,WF,1,FR,0)),"^",1)
 .......I RP>0 D
 ........N ORIEN ;orderable item ien
 ........S ORIEN=$P($G(^C9C(300890,REIEN,1,WF,1,FR,7)),"^",1)
 ........I ORIEN>0 D ;orderable item cohort found
 .........S OCNT=OCNT+1
 .........S OCOHORT("WARD",OCNT)=WF_"^"_FR_"^"_ORIEN_"^"_RP
 ...I UNIT'="Undefined" D
 ....N UF ;unit formula
 ....S UF=0 F  S UF=$O(^C9C(300890,REIEN,2,UF)) Q:UF'>0  D
 .....;check formula units to see if applicable
 .....I $O(^C9C(300890,REIEN,2,UF,2,"B",UNIT,0))>0 D ;unit included in this formula
 ......N FR ;formula rule
 ......S FR=0 F  S FR=$O(^C9C(300890,REIEN,2,UF,1,FR)) Q:FR'>0  D
 .......N RP ;rule pointer value
 .......S RP=$P($G(^C9C(300890,REIEN,2,UF,1,FR,0)),"^",1)
 .......I RP>0 D
 ........N ORIEN ;orderable item ien
 ........S ORIEN=$P($G(^C9C(300890,REIEN,2,UF,1,FR,7)),"^",1)
 ........I ORIEN>0 D ;orderable item cohort found
 .........S OCNT=OCNT+1
 .........S OCOHORT("UNIT",OCNT)=UF_"^"_FR_"^"_ORIEN_"^"_RP
 ...N HF ;hospital formula
 ...S HF=0 F  S HF=$O(^C9C(300890,REIEN,3,HF)) Q:HF'>0  D
 ....;check formula hospital to see if applicable using DUZ(2)
 ....I $O(^C9C(300890,REIEN,3,HF,2,"B",DIV,0))>0 D ;hospital included in this formula
 .....N FR ;formula rule
 .....S FR=0 F  S FR=$O(^C9C(300890,REIEN,3,HF,1,FR)) Q:FR'>0  D
 ......N RP ;rule pointer value
 ......S RP=$P($G(^C9C(300890,REIEN,3,HF,1,FR,0)),"^",1)
 ......I RP>0 D
 .......N ORIEN ;orderable item ien
 .......S ORIEN=$P($G(^C9C(300890,REIEN,3,HF,1,FR,7)),"^",1)
 .......I ORIEN>0 D ;orderable item cohort found
 ........S OCNT=OCNT+1
 ........S OCOHORT("HOSPITAL",OCNT)=HF_"^"_FR_"^"_ORIEN_"^"_RP
 ...;use the APT cross-reference ^TIU(8925,"APT",PAT,TITLIEN,DOCSTATUS(zero node, piece 5 P:8925.6),INVERSE DATE(node 13, piece 1)
 ...N DUEDATE ;next note due date
 ...S DUEDATE=0
 ...N FNOTEIEN ;found note ien
 ...S FNOTEIEN=""
 ...N ROWCOLOR
 ...S ROWCOLOR=""
 ...N ADATE ;latest patient admit date
 ...S ADATE=0
 ...N ADIEN ;movement ien
 ...S ADIEN=$G(^DPT(PAT,.105))
 ...I ADIEN>0 D
 ....S ADATE=$P($G(^DGPM(ADIEN,0)),"^",1)
 ...N NONOTE ;set to one if this note has not been completed during this visit or within two days before
 ...S NONOTE=1 ;set to one for now, will check for completed note below
 ...;get the latest note (will also need to check to be sure it is during this visit)
 ...N NOTEOK ;has the health factor if required
 ...S NOTEOK=0
 ...N INVDATE
 ...S INVDATE=0
 ...N NDATE ;inverse note date for note ien being checked
 ...S NDATE=0 F  S NDATE=$O(^TIU(8925,"APT",PAT,TITLIEN,CIEN,NDATE)) Q:((NDATE'>0)!(NOTEOK>0))  D ;if found, there has been a note with this title, check health factor if required
 ....N NIEN
 ....S NIEN=0 F  S NIEN=$O(^TIU(8925,"APT",PAT,TITLIEN,CIEN,NDATE,NIEN)) Q:NIEN'>0  D
 .....;we now have the latest note ien, now check visit and rules
 .....;note - what if event based, but event happened last visit?
 .....S FNOTEIEN=NIEN
 .....I (((9999999-NDATE)'<$$FMADD^XLFDT(ADATE,-2))&($P($G(^TIU(8925,FNOTEIEN,15)),"^",2)>0)) D ;this means that the note was during this visit or within two days before - should cover notes written in SAU and it is a signed note
 ......I (HFACTIEN'>0) D
 .......S NONOTE=0 ;has a completed note this admission for this title
 .......S INVDATE=NDATE
 .......S NOTEOK=1 ;july 2
 ......E  D ;need to check the health factor required
 .......N HFDATE
 .......S HFDATE=9999999-NDATE
 .......S HFDATE=HFDATE\1
 .......S HFDATE=9999999-HFDATE
 .......N ENTRYD S ENTRYD=$P($G(^TIU(8925,FNOTEIEN,12)),"^",1)
 .......S ENTRYD=ENTRYD\1
 .......S ENTRYD=9999999-ENTRYD
 .......I (($D(^AUPNVHF("AA",PAT,HFACTIEN,HFDATE)))!($D(^AUPNVHF("AA",PAT,HFACTIEN,NDATE)))!($D(^AUPNVHF("AA",PAT,HFACTIEN,ENTRYD)))) D ;health factor exists for this note
 ........S NONOTE=0
 ........S INVDATE=NDATE
 ........S NOTEOK=1 ;july 2
 .......E  D
 ........N VPTR
 ........S VPTR=$P($G(^TIU(8925,FNOTEIEN,0)),"^",3)
 ........I VPTR>0 D
 .........N VDATE
 .........S VDATE=$P($G(^AUPNVSIT(VPTR,0)),"^",1)
 .........I VDATE>0 D
 ..........S VDATE=VDATE\1
 ..........S VDATE=9999999-VDATE
 ..........I $D(^AUPNVHF("AA",PAT,HFACTIEN,VDATE)) D
 ...........S NONOTE=0
 ...........S INVDATE=NDATE
 ...........S NOTEOK=1
 ...I INVDATE'>0 D
 ....S INVDATE=9999999
 ...;check all the rules and find the next one due
 ...;first check the cohort rules
 ...N ACR ;applicable cohort rules
 ...N EVNTDATE
 ...I PTONLY'>0 D ;but not if we're just checking pass-through reminders
 ....N CH
 ....I WARDIEN>0 D
 .....S CH=0 F  S CH=$O(OCOHORT("WARD",CH)) Q:CH'>0  D
 ......N ACNT ;applicable cohort rules counter
 ......S ACNT=0
 ......N OIEN
 ......S OIEN=$P(OCOHORT("WARD",CH),"^",3)
 ......I OIEN>0 D
 .......;note - what about items that were ordered on prior visits and are still active?
 .......;allowing it for now
 .......N HASIT ;set to one if active order found
 .......S HASIT=0
 .......S EVNTDATE=0
 .......N RPNTR
 .......S RPNTR=$P(OCOHORT("WARD",CH),"^",2)
 .......N ID ;inverse date
 .......S ID=0 F  S ID=$O(^OR(100,"AC",PAT_";DPT(",ID)) Q:((ID'>0)!(HASIT=1))  D
 ........N TN ;order ien
 ........S TN=0 F  S TN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN)) Q:((TN'>0)!(HASIT=1))  D
 .........N IN ;instance
 .........S IN=0 F  S IN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN,IN)) Q:((IN'>0)!(HASIT=1))  D
 ..........N OIF
 ..........S OIF=0 ;orderable item not found
 ..........N OX S OX=0 F  S OX=$O(^OR(100,TN,.1,OX)) Q:OX'>0  D
 ...........I $P($G(^OR(100,TN,.1,OX,0)),"^",1)=OIEN D
 ............S OIF=1
 ..........I OIF=1 D
 ...........N STOPD ;stopdate
 ...........S STOPD=$P($G(^OR(100,TN,0)),"^",9)
 ...........N STRTD
 ...........S STRTD=$P($G(^OR(100,TN,0)),"^",8)
 ...........I ((STRTD<$$NOW^XLFDT)&((STOPD="")!(STOPD'<$$NOW^XLFDT))) D
 ............S HASIT=1 ;has active orderable item
 ............S EVNTDATE=STRTD
 ............S ACNT=ACNT+1
 ............S ACR("WARD",ACNT)=OCOHORT("WARD",CH)_"^"_TN_"^"_EVNTDATE ;Now OCOHORT("WARD",CH)=Formula IEN^Rule IEN^Orderable Item IEN^Rule Pointer^Order IEN^Order Start Date
 ....I UNIT'="Undefined" D
 .....S CH=0 F  S CH=$O(OCOHORT("UNIT",CH)) Q:CH'>0  D
 ......N ACNT ;applicable cohort rules counter
 ......S ACNT=0
 ......N OIEN
 ......S OIEN=$P(OCOHORT("UNIT",CH),"^",3)
 ......I OIEN>0 D
 .......;note - what about items that were ordered on prior visits and are still active?
 .......;allowing it for now
 .......N HASIT ;set to one if active order found
 .......S HASIT=0
 .......S EVNTDATE=0
 .......N RPNTR
 .......S RPNTR=$P(OCOHORT("UNIT",CH),"^",2)
 .......N ID ;inverse date
 .......S ID=0 F  S ID=$O(^OR(100,"AC",PAT_";DPT(",ID)) Q:((ID'>0)!(HASIT=1))  D
 ........N TN ;order ien
 ........S TN=0 F  S TN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN)) Q:((TN'>0)!(HASIT=1))  D
 .........N IN ;instance
 .........S IN=0 F  S IN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN,IN)) Q:((IN'>0)!(HASIT=1))  D
 ..........N OIF
 ..........S OIF=0 ;orderable item not found
 ..........N OX S OX=0 F  S OX=$O(^OR(100,TN,.1,OX)) Q:OX'>0  D
 ...........I $P($G(^OR(100,TN,.1,OX,0)),"^",1)=OIEN D
 ............S OIF=1
 ..........I OIF=1 D
 ...........N STOPD ;stopdate
 ...........S STOPD=$P($G(^OR(100,TN,0)),"^",9)
 ...........N STRTD
 ...........S STRTD=$P($G(^OR(100,TN,0)),"^",8)
 ...........I ((STRTD<$$NOW^XLFDT)&((STOPD="")!(STOPD'<$$NOW^XLFDT))) D
 ............S HASIT=1 ;has active orderable item
 ............S EVNTDATE=STRTD
 ............S ACNT=ACNT+1
 ............S ACR("UNIT",ACNT)=OCOHORT("UNIT",CH)_"^"_TN_"^"_EVNTDATE ;;Now OCOHORT("UNIT",CH)=Formula IEN^Rule IEN^Orderable Item IEN^Rule Pointer^Order IEN^Order Start Date
 ....S CH=0 F  S CH=$O(OCOHORT("HOSPITAL",CH)) Q:CH'>0  D
 .....N ACNT ;applicable cohort rules counter
 .....S ACNT=0
 .....N OIEN
 .....S OIEN=$P(OCOHORT("HOSPITAL",CH),"^",3)
 .....I OIEN>0 D
 ......;note - what about items that were ordered on prior visits and are still active?
 ......;allowing it for now
 ......N HASIT ;set to one if active order found
 ......S HASIT=0
 ......S EVNTDATE=0
 ......N RPNTR
 ......S RPNTR=$P(OCOHORT("HOSPITAL",CH),"^",2)
 ......N ID ;inverse date
 ......S ID=0 F  S ID=$O(^OR(100,"AC",PAT_";DPT(",ID)) Q:((ID'>0)!(HASIT=1))  D
 .......N TN ;order ien
 .......S TN=0 F  S TN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN)) Q:((TN'>0)!(HASIT=1))  D
 ........N IN ;instance
 ........S IN=0 F  S IN=$O(^OR(100,"AC",PAT_";DPT(",ID,TN,IN)) Q:((IN'>0)!(HASIT=1))  D
 .........N OIF
 .........S OIF=0 ;orderable item not found
 .........N OX S OX=0 F  S OX=$O(^OR(100,TN,.1,OX)) Q:OX'>0  D
 ..........I $P($G(^OR(100,TN,.1,OX,0)),"^",1)=OIEN D
 ...........S OIF=1
 .........I OIF=1 D
 ..........N STOPD ;stopdate
 ..........S STOPD=$P($G(^OR(100,TN,0)),"^",9)
 ..........N STRTD
 ..........S STRTD=$P($G(^OR(100,TN,0)),"^",8)
 ..........I ((STRTD<$$NOW^XLFDT)&((STOPD="")!(STOPD'<$$NOW^XLFDT))) D
 ...........S HASIT=1 ;has active orderable item
 ...........S EVNTDATE=STRTD
 ...........S ACNT=ACNT+1
 ...........S ACR("HOSPITAL",ACNT)=OCOHORT("HOSPITAL",CH)_"^"_TN_"^"_EVNTDATE ;;Now OCOHORT("HOSPITAL",CH)=Formula IEN^Rule IEN^Orderable Item IEN^Rule Pointer^Order IEN^Order Start Date
 ....;ACR() now contains cohort rules applicable to this patient
 ....;cohort rules trump everything - no other rules are considered if these are present!
 ....;like the other rules, ward cohort trumps unit cohort trumps hospital cohort
 ....;all three levels are contained in ACR, but they may be for varying orderable item iens
 ...;so for each orderable item ien, use only the most granular, i.e. ward,unit,hospital
 ...N APRULE ;applicable rule definition pointer
 ...S APRULE=0
 ...N RULELEVELFOUND ;use to track whether an applicable granular rule was found
 ...S RULELEVELFOUND="" ;WARD, UNIT, or HOSPITAL
 ...I $D(ACR) D
 ....;use only these cohort rules - ward, unit, or hospital in that order
 ....N BASEDATE
 ....S BASEDATE=0
 ....I WARDIEN>0 D ACRWUH(1,WARDIEN,.APRULE,.RULELEVELFOUND,.DUEDATE,.ACR,NONOTE,INVDATE,.BASEDATE,TITLIEN,REIEN,.OFFSET) ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding
 ....I RULELEVELFOUND="" D
 .....I UNIT'="Undefined" D ACRWUH(2,UNIT,.APRULE,.RULELEVELFOUND,.DUEDATE,.ACR,NONOTE,INVDATE,.BASEDATE,TITLIEN,REIEN,.OFFSET) ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding
 ....I RULELEVELFOUND="" D
 .....D ACRWUH(3,DIV,.APRULE,.RULELEVELFOUND,.DUEDATE,.ACR,NONOTE,INVDATE,.BASEDATE,TITLIEN,REIEN,.OFFSET) ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding
 ...I '$D(ACR) D 
 ....;use ward, unit, or hospital rules in that order
 ....S APRULE=0
 ....I WARDIEN>0 D NAFWUH(1,WARDIEN,.APRULE,.RULELEVELFOUND,.DUEDATE,$G(ADATE),NONOTE,TITLIEN,REIEN,.OFFSET,PTONLY,CLEVEL) ;ward rules ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding ;CRH JHT 2020 - added PTONLY to allow pass-through only processing ;CRH JHT 2020 - added CLEVEL for cohort level mar 3,2020
 ....I RULELEVELFOUND="" D
 .....I UNIT'="Undefined" D NAFWUH(2,UNIT,.APRULE,.RULELEVELFOUND,.DUEDATE,$G(ADATE),NONOTE,TITLIEN,REIEN,.OFFSET,PTONLY,CLEVEL) ;unit rules ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding ;CRH JHT 2020 - added PTONLY to allow pass-through only processing ;CRH JHT 2020 - added CLEVEL for cohort level mar 3,2020
 ....I RULELEVELFOUND="" D
 .....D NAFWUH(3,DIV,.APRULE,.RULELEVELFOUND,.DUEDATE,$G(ADATE),NONOTE,TITLIEN,REIEN,.OFFSET,PTONLY,CLEVEL) ;hospital rules ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding ;CRH JHT 2020 - added PTONLY to allow pass-through only processing ;CRH JHT 2020 - added CLEVEL for cohort level mar 3,2020
 ...I ((DUEDATE>0)&(+APRULE>0)) D
 ....;show this entry
 ....;find out what color to display the row
 ....S BASEDATE=DUEDATE
 ....;begin CRH JHT 2019 - for override logic
 ....N RPOINT
 ....S RPOINT=+APRULE
 ....;end CRH JHT 2019 - for override logic
 ....N AHEAD
 ....S AHEAD=$P(APRULE,"^",2)
 ....I AHEAD'="" D
 .....S C9CCHECK=$$ALMOST^C9CPTR(AHEAD,BASEDATE)
 ....E  D
 .....S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,+APRULE,4)) I CKCODE'="" X CKCODE
 ....I C9CCHECK>0 S ROWCOLOR="Yellow"
 ....I C9CCHECK=0 D
 .....S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,+APRULE,6)) I CKCODE'="" X CKCODE
 .....I C9CCHECK>0 S ROWCOLOR="LightCoral"
 ....N PATNAME
 ....S PATNAME=$P($G(^DPT(PAT,0)),"^",1)
 ....N MRN
 ....S MRN=$P($G(^DPT(PAT,"MSCD")),"^",1)
 ....N NOTEDATE
 ....S NOTEDATE=(9999999-INVDATE)
 ....I $P(NOTEDATE,".",1)>0 I $P(NOTEDATE,".",2)="" S NOTEDATE=NOTEDATE_".2359"
 ....S NOTEDATE=$$FMTE^XLFDT(NOTEDATE)
 ....I NOTEDATE=0 S NOTEDATE=""
 ....I $P(DUEDATE,".",1)>0 I $P(DUEDATE,".",2)="" S DUEDATE=DUEDATE_".2359"
 ....S DUEDATE=$$FMTE^XLFDT(DUEDATE)
 ....I DUEDATE=0 S DUEDATE=""
 ....N NOTEIEN
 ....S NOTEIEN=$G(FNOTEIEN)
 ....N OUTADMIT
 ....S OUTADMIT=$$FMTE^XLFDT(ADATE)
 ....N ATTEND
 ....S ATTEND=$G(^DPT(PAT,.1041)) I ATTEND S ATTEND=$P($G(^VA(200,ATTEND,0)),U)
 ....I WARDIEN=-1 S WARD="Outpatient"
 ....N ABBREV
 ....S ABBREV=""
 ....I $G(RPOINT)>0 D
 .....S ABBREV=$P($G(^C9C(300892,RPOINT,0)),"^",3)
 .....N OPOS
 .....I $G(OFFSET)'=0 D
 ......I $F(ABBREV,"X DAYS")>1 D
 .......S OPOS=$F(ABBREV,"X DAYS")
 .......I OPOS>7 D
 ........S ABBREV=$E(ABBREV,1,(OPOS-7))_OFFSET_$E(ABBREV,(OPOS-5),50)
 ......I $F(ABBREV,"X HOURS")>1 D
 .......S OPOS=$F(ABBREV,"X HOURS")
 .......I OPOS>8 D
 ........S ABBREV=$E(ABBREV,1,(OPOS-8))_OFFSET_$E(ABBREV,(OPOS-6),50)
 ....S C9CI=C9CI+1
 ....S C9CRET(C9CI)=PAT_"^"_PATNAME_"^"_MRN_"^"_OUTADMIT_"^"_ATTEND_"^"_TITLIEN_"^"_TITLNAME_"^"_NOTEDATE_"^"_WARD_"^"_NOTEIEN_"^"_DUEDATE_"^"_ROWCOLOR_"^"_ABBREV_"^"_RPOINT
 Q
ACRWUH(SEPAR,LEVPTR,APRULE,RULELEVELFOUND,DUEDATE,ARRACR,NONOTE,INVDATE,BASEDATE,TITLIEN,REIEN,OFFSET) ;CRH JHT 2019 - added TITLIEN and REIEN and OFFSET for override coding
 ;SEPAR - 1 for ward, 2 for unit, 3 for hospital
 ;LEVPTR - ward ien, bedsection, or DUZ(2)
 Q:SEPAR'>0
 N LEVEL
 S LEVEL=""
 I SEPAR=1 S LEVEL="WARD"
 I SEPAR=2 S LEVEL="UNIT"
 I SEPAR=3 S LEVEL="HOSPITAL"
 N WL
 S WL=0 F  S WL=$O(ARRACR(LEVEL,WL)) Q:WL'>0  D ;check ward rules first
 .S BASEDATE=+$P(ARRACR(LEVEL,WL),"^",6)
 .N EDATE ;event date
 .S EDATE=BASEDATE
 .I BASEDATE>0 D
 ..N RULETEXT
 ..S RULETEXT=""
 ..N RPOINT ;changed RPTR to RPOINT this subroutine for override coding
 ..S RPOINT=$P(ARRACR(LEVEL,WL),"^",4)
 ..I RPOINT>0 D
 ...N RUACT ;rule active?
 ...S RUACT=$P($G(^C9C(300892,RPOINT,0)),"^",2)
 ...I RUACT>0 D
 ....S RULETEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 ....;begin CRH JHT 2019 - change delta to mumps code for override coding
 ....;N OFFSET ;offset in days or hours - do not new here as it is passed by reference
 ....S OFFSET=0
 ....N FIEN
 ....S FIEN=+$P(ARRACR(LEVEL,WL),"^",1)
 ....I FIEN>0 D
 .....N RFIEN
 .....S RFIEN=+$P(ARRACR(LEVEL,WL),"^",2)
 .....I RFIEN>0 D
 ......I $F(RULETEXT,"X DAYS")>0 D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,FIEN,1,RFIEN,8)),"^",1)
 ......I $F(RULETEXT,"X HOURS")>0 D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,FIEN,1,RFIEN,9)),"^",1)
 ....N ABD ;ACCEPTABLE BASEDATE DELTA formatted as DAYS.HOURS
 ....N MABD ;mumps code
 ....S MABD=$G(^C9C(300892,RPOINT,7))
 ....;was S ABD=$P($G(^C9C(300892,RPOINT,7)),"^",1)
 ....I MABD'="" X MABD
 ....I ((ABD'>0)&(RULETEXT'="DUE NOW")) S ABD="2.2" ;default if code does not return proper value
 ....;end CRH JHT 2019 - change delta to mumps code
 ....N ABDAYS S ABDAYS=+$P(ABD,".",1)
 ....N ABHOURS S ABHOURS=+$P(ABD,".",2)
 ....;start X DAYS
 ....I $F(RULETEXT,"EVERY X DAYS FROM EVENT DATE")>0 D
 .....N FORMIEN
 .....S FORMIEN=+$P(ARRACR(LEVEL,WL),"^",1)
 .....I FORMIEN>0 D
 ......N RUIEN
 ......S RUIEN=+$P(ARRACR(LEVEL,WL),"^",2)
 ......I RUIEN>0 D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,FORMIEN,1,RUIEN,8)),"^",1)
 .......I ((BASEDATE>0)&(OFFSET>0)) D
 ........S BASEDATE=$$FMADD^XLFDT(BASEDATE,OFFSET)
 ........I BASEDATE>0 D
 .........I OFFSET>0 D
 ..........N NUMDAYS
 ..........S NUMDAYS=OFFSET
 ..........N DSAD ;days since event date
 ..........S DSAD=$$FMDIFF^XLFDT(RNOW,BASEDATE,1) ;difference in days
 ..........N MDSAD
 ..........S MDSAD=DSAD#NUMDAYS
 ..........S BASEDATE=$$FMADD^XLFDT(RNOW\1,MDSAD)_.2359
 ..........I BASEDATE'=0 D
 ...........S RULELEVELFOUND=LEVEL
 ...........N C9CCHECK
 ...........S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,RPOINT,5)) I CKCODE'="" X CKCODE
 ...........I C9CCHECK>0 D
 ............;check to see if there is a completed note in this range or after
 ............I NONOTE=1 D ;no completed notes of this title for this patient, this visit
 .............I DUEDATE<BASEDATE D
 ..............S DUEDATE=BASEDATE
 ..............S APRULE=RUIEN ;applicable rule
 ............I NONOTE=0 D
 .............;check to see if there is a completed note of this title within or after this time period
 .............;first check within the time period
 .............N LASTDATE
 .............S LASTDATE=(9999999-INVDATE) ;date of last completed note of this title
 .............I (LASTDATE'>$$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)) D  ;this means a note has not been done during or after the time period
 ..............I DUEDATE<BASEDATE D
 ...............S APRULE=RUIEN
 ...............S DUEDATE=BASEDATE
 ....;end X DAYS
 ....;start X HOURS
 ....I $F(RULETEXT,"EVERY X HOURS FROM EVENT DATE")>0 D
 .....N FORMIEN
 .....S FORMIEN=+$P(ARRACR(LEVEL,WL),"^",1)
 .....I FORMIEN>0 D
 ......N RUIEN
 ......S RUIEN=+$P(ARRACR(LEVEL,WL),"^",2)
 ......I RUIEN>0 D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,FORMIEN,1,RUIEN,9)),"^",1)
 .......I ((BASEDATE>0)&(OFFSET>0)) D
 ........S BASEDATE=$$FMADD^XLFDT(BASEDATE,,OFFSET)
 ........I BASEDATE>0 D
 .........I OFFSET>0 D
 ..........N NUMHOURS
 ..........S NUMHOURS=OFFSET
 ..........N SSAD ;seconds since event date
 ..........S SSAD=$$FMDIFF^XLFDT(RNOW,EDATE,2) ;difference in seconds
 ..........I SSAD>0 D
 ...........N HSAD ;hours since event date
 ...........S HSAD=SSAD\3600
 ...........N MHSAD
 ...........S MHSAD=HSAD#NUMHOURS ;how many whole hours till next due time
 ...........S BASEDATE=$$FMADD^XLFDT(EDATE,,HSAD+MHSAD) ;next due time
 ...........I BASEDATE'=0 D
 ............S RULELEVELFOUND=LEVEL
 ............N C9CCHECK
 ............S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,RPOINT,5)) I CKCODE'="" X CKCODE
 ............I C9CCHECK>0 D
 .............;check to see if there is a completed note in this range or after
 .............I NONOTE=1 D ;no completed notes of this title for this patient, this visit
 ..............I DUEDATE<BASEDATE D
 ...............S DUEDATE=BASEDATE
 ...............S APRULE=RUIEN ;applicable rule
 .............I NONOTE=0 D
 ..............;check to see if there is a completed note of this title within or after this time period
 ..............;first check within the time period
 ..............N LASTDATE
 ..............S LASTDATE=(9999999-INVDATE) ;date of last completed note of this title
 ..............I (LASTDATE'>$$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)) D  ;this means a note has not been done during or after the time period
 ...............I DUEDATE<BASEDATE D
 ................S APRULE=RUIEN
 ................S DUEDATE=BASEDATE
 ....;end X HOURS 
 Q
NAFWUH(SEPAR,LEVPTR,APRULE,RULELEVELFOUND,DUEDATE,ADATE,NONOTE,TITLIEN,REIEN,OFST,PTONLY,CLEVEL) ;CRH JHT 2019 - added TITLIEN and REIEN and OFST for override coding ;CRH JHT 2020 - added PTONLY to allow pass-through only processing ;CRH JHT 2020 - added CLEVEL for cohort level mar 3,2020
 ;SEPAR - 1 for ward, 2 for unit, 3 for hospital
 ;LEVPTR - ward ien, bedsection, or DUZ(2)
 Q:SEPAR'>0
 N LEVEL
 S LEVEL=""
 I SEPAR=1 S LEVEL="WARD"
 I SEPAR=2 S LEVEL="UNIT"
 I SEPAR=3 S LEVEL="HOSPITAL"
 ;----------------------
 N WF 
 S WF=0 F  S WF=$O(^C9C(300890,REIEN,SEPAR,WF)) Q:WF'>0  D
 .;check each formula included to see if LEVPTR included
 .I $O(^C9C(300890,REIEN,SEPAR,WF,2,"B",LEVPTR,0))>0 D ;LEVPTR included in this formula
 ..N C9CCHECK ;this will be set as 0 or 1 from the mumps code fields in C9C RULES file
 ..S C9CCHECK=0
 ..N BASEDATE
 ..S BASEDATE=0
 ..N PREVADAYS
 ..S PREVADAYS=0
 ..N PREVAHOURS
 ..S PREVAHOURS=0
 ..N RTRACK ;track whether to keep processing rules in this formula ;mar 3,2020
 ..S RTRACK=0 ;mar 3,2020
 ..I CLEVEL=0 S RTRACK=1 ;there is no cohort defined, process all ;mar 3,2020
 ..I CLEVEL=1 S RTRACK=2 ;in cohort, process up to, but not including first pass-through ;mar 3,2020
 ..I CLEVEL=2 I PTONLY=1 S RTRACK=3 ;not in cohort, PTONLY checked, start processing pass-throughs ;mar 3,2020
 ..I CLEVEL=2 I PTONLY=0 S RTRACK=0 ;not in cohort, PTONLY not checked, process nothing ;mar 3,2020
 ..;RTRACK: 0=stop no matter what, 1=process no matter what, 2=process up to but not including first pass-through, 3=stop until next pass-through, 4=process all rules until next pass-through ;mar 3,2020
 ..N FR ;formula rule
 ..S FR=0 F  S FR=$O(^C9C(300890,REIEN,SEPAR,WF,1,FR)) Q:((FR'>0)!(C9CCHECK>0)!(RTRACK=0))  D ;for each rule in the formula ;added RTRACK mar 3,2020
 ...N RPOINT ;rule pointer
 ...S RPOINT=$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,0)),"^",1)
 ...I RPOINT>0 D
 ....N RTEXT
 ....S RTEXT=$P($G(^C9C(300892,RPOINT,0)),"^",1)
 ....I RTEXT'="" I $F(RTEXT,"PASS-THROUGH REMINDER")>0 I RTRACK=2 S RTRACK=0 ;found the first pass-through, so stop processing all rules ;mar 3,2020
 ....I RTEXT'="" I $F(RTEXT,"PASS-THROUGH REMINDER")>0 I RTRACK=4 S RTRACK=0 ;finished processing rules after resolved pass-through ;mar 3,2020
 ....I RTEXT'="" I (RTRACK=1)!(RTRACK=2)!(RTRACK=4)!((RTRACK=3)&($F(RTEXT,"PASS-THROUGH REMINDER")>0)) D ;mar 5,2020 more complex reminder processing
 .....;begin CRH JHT 2019 - change delta to mumps code for override coding
 .....N OFFSET
 .....S OFFSET=0
 .....I $F(RTEXT,"X DAYS")>0 D
 ......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1)
 .....I $F(RTEXT,"X HOURS")>0 D
 ......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,6)),"^",1)
 .....N ABD ;ACCEPTABLE BASEDATE DELTA formatted as DAYS.HOURS
 .....N MABD ;mumps code
 .....S MABD=$G(^C9C(300892,RPOINT,7))
 .....;was S ABD=$P($G(^C9C(300892,RPOINT,7)),"^",1)
 .....I MABD'="" X MABD
 .....I ((ABD'>0)&(RTEXT'="DUE NOW")) S ABD="2.2" ;default if code does not return proper value
 .....;end CRH JHT 2019 - change delta to mumps code
 .....N ABDAYS S ABDAYS=+$P(ABD,".",1)
 .....N ABHOURS S ABHOURS=+$P(ABD,".",2)
 .....N ABMINS
 .....S ABMINS=0
 .....S BASEDATE=0
 .....N REMIEN ;expose this for show/color checking
 .....;begin reminder pass-through logic
 .....I $F(RTEXT,"PASS-THROUGH REMINDER")>0 D
 ......S BASEDATE=0
 ......I $G(TITLIEN)>0 D
 .......S REMIEN=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,10)),"^",1)
 .......K DEFARR  ;JHT 08-24-2020 for new PXRM logic repair
 .......N DEFARR
 .......I REMIEN'>0 I RTRACK=4 S RTRACK=3 ;this one is bogus for some reason ;mar 3,2020
 .......I REMIEN>0 D
 ........N PXRMDEFS  ;JHT 08-24-2020 for new PXRM logic repair 
 ........K FIEVAL,^TMP("PXRHM",$J,REMIEN) I $G(PXRMID)'="" K ^TMP(PXRMID,$J)  ;JHT 08-24-2020 for new PXRM logic repair
 ........N DEFNAME
 ........S DEFNAME=$P($G(^PXD(811.9,REMIEN,0)),"^",3) ;has to be print name, not name
 ........I DEFNAME="" I RTRACK=4 S RTRACK=3 ;this one is bogus for some reason ;mar 3,2020
 ........I DEFNAME'="" D
 .........N PXRMDEBG ;must be defined and set or PXRMID won't be set
 .........S PXRMDEBG=1
 .........N DATE ;must be newed but not populated to return current value
 .........D DEF^PXRMLDR(REMIEN,.DEFARR) ;set up variables for this reminder check
 .........N FIEVAL
 .........D EVAL^PXRM(PAT,.DEFARR,5,1,.FIEVAL)
 .........I $G(PXRMID)="" I RTRACK=4 S RTRACK=3 ;this one is bogus for some reason ;mar 3,2020
 .........I $G(PXRMID)'="" D
 ..........N RESOLU,COHORT
 ..........S RESOLU=+$P($G(^TMP(PXRMID,$J,REMIEN,"RESOLUTION LOGIC")),"^",1)
 ..........S COHORT=$P($G(^TMP(PXRMID,$J,REMIEN,"PATIENT COHORT LOGIC")),"^",1)
 ..........K ^TMP(PXRMID,$J) K FIEVAL ;needs to be new each time
 ..........I (COHORT=0) I (RTRACK=4) S RTRACK=3 ;skip to next pass-through ;mar 3,2020
 ..........I (COHORT=1) D
 ...........I RESOLU=1 I ((RTRACK=3)!(RTRACK=4)) S RTRACK=0 ;process no more rules ;mar 3,2020
 ...........I RESOLU=0 I ((RTRACK=3)!(RTRACK=4)) S RTRACK=2 ;process until next pass-through ;mar 3,2020
 ...........I CLEVEL=0 D ;only allow this to set a date if no reminder cohort is set ;mar 9,2020
 ............N RSTATUS
 ............S RSTATUS=$P($G(^TMP("PXRHM",$J,REMIEN,DEFNAME)),"^",1)
 ............N RDATEDUE
 ............S RDATEDUE=$P($G(^TMP("PXRHM",$J,REMIEN,DEFNAME)),"^",2)
 ............I RSTATUS="N/A" S BASEDATE=0
 ............E  I (((RSTATUS="DUE SOON")!(RESOLU=0))&(RDATEDUE'>0)) S BASEDATE=$P($$NOW^XLFDT,".",1)
 ............E  I (((RSTATUS="DUE SOON")!(RESOLU=0))&(RDATEDUE>0)) S BASEDATE=RDATEDUE
 ............E  I (((RSTATUS="DUE SOON")&(RESOLU=1))&(RDATEDUE'>0)) S BASEDATE=$P($$NOW^XLFDT,".",1)
 ............E  I (((RSTATUS="DUE SOON")&(RESOLU=1))&(RDATEDUE>0)) S BASEDATE=RDATEDUE
 ............E  I (((RSTATUS="DUE NOW")&(RESOLU=1))&(RDATEDUE'>0)) S BASEDATE=$P($$NOW^XLFDT,".",1)
 ............E  I (((RSTATUS="DUE NOW")&(RESOLU=1))&(RDATEDUE>0)) S BASEDATE=RDATEDUE
 ...........K ^TMP("PXRHM",$J,REMIEN)
 ...........K FIEVAL ;needs to be new each time
 .....;end reminder pass-through logic
 .....;start due now logic
 .....I RTEXT="DUE NOW" D
 ......S BASEDATE=$P($$NOW^XLFDT,".",1)
 .....;end due now logic
 .....;start annually based on admission day
 .....I $F(RTEXT,"ANNUALLY BASED ON ADMISSION DAY")>0 D
 ......S BASEDATE=ADATE
 ......I ($F(RTEXT,"X DAYS")>0) D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1)
 .......;I ((BASEDATE>0)&(OFFSET>0)) D
 .......I BASEDATE>0 D ;April 23,2020
 ........S BASEDATE=$$FMADD^XLFDT(BASEDATE,OFFSET)
 ........I OFFSET>(ABDAYS+2) D
 .........I PREVADAYS=0 D ;if this is the first X DAYS rule for this title/patient
 ..........S PREVADAYS=OFFSET
 ..........S ABDAYS=OFFSET+2 ;we need to cover two days prior to admission as well
 ......I ABD="" S ABD="14.0"
 ......S ABDAYS=+$P(ABD,".",1)
 ......S ABHOURS=+$P(ABD,".",2)
 ......N YY S YY=YEAR ;$E(BASEDATE,2,3)
 ......N CC S CC=CENTURY ;$E(BASEDATE,1,1)
 ......N MONT S MONT=$E(BASEDATE,4,5)
 ......N DD S DD=$E(BASEDATE,6,7)
 ......N THEREST S THEREST=".1200" ;noon for this rule
 ......I ((($$FMADD^XLFDT(CENTURY_YEAR_THEREST,ABDAYS,ABHOURS))>(CENTURY_YEAR_DHMS))&(($$FMADD^XLFDT(CENTURY_YEAR_THEREST,-ABDAYS,-ABHOURS))<(CENTURY_YEAR_DHMS))) D ;are we currently in this years cycle ;mar 11,2020
 .......S BASEDATE=CENTURY_YEAR_THEREST ;mar 11,2020
 ......I (($$FMADD^XLFDT(CENTURY_YEAR_MONT_DD_THEREST,ABDAYS,ABHOURS))<(CENTURY_YEAR_DHMS)) D ;DHMS=days hours minutes seconds
 .......S BASEDATE=CENTURY_YEAR_MONT_DD_THEREST ;try this year
 .......S YY=YY+1
 .......I YY=100 S YY="00" S CC=CC+1
 .......I (($$FMADD^XLFDT(CC_YY_MONT_DD_THEREST,-ABDAYS,-ABHOURS)))<RNOW D ;are we in next years cycle yet ;JHT 7/1/2019
 ........S BASEDATE=CC_YY_MONT_DD_THEREST ;set to next years cycle
 ......I (($$FMADD^XLFDT(CENTURY_YEAR_MONT_DD_THEREST,-ABDAYS,-ABHOURS))>(CENTURY_YEAR_DHMS)) D ;DHMS=days hours minutes seconds ;are we in this year's cycle? ;mar 6,2020
 .......;if not, try last year ;mar 6,2020
 .......S YY=YY-1 ;mar 6,2020
 .......I YY=-1 S YY=99 S CC=CC-1 ;mar 6,2020
 .......S BASEDATE=CC_YY_MONT_DD_THEREST ;set to last year's cycle ;mar 6,2020
 ......I (BASEDATE\1)'>(ADATE\1) S BASEDATE=0 ;not due same day as admission in this case
 .....;end annually based on admission day
 .....I $F(RTEXT,"ADMIT DATE")>0 D
 ......S BASEDATE=ADATE
 ......I (($F(RTEXT,"X DAYS")>0)&($F(RTEXT,"EVERY")'>0)) D
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1)
 .......;I ((BASEDATE>0)&(OFFSET>0)) D
 .......I BASEDATE>0 D ;April 23,2020
 ........S BASEDATE=$$FMADD^XLFDT(BASEDATE,OFFSET)
 ........I OFFSET>(ABDAYS+2) D
  ........I PREVADAYS=0 D ;july 12, 2019
 .........S PREVADAYS=OFFSET ;july 12, 2019
 .........S ABDAYS=OFFSET+2 ;we need to cover two days prior to admission as well ;july 12, 2019
 ......;start annually
 ......I $F(RTEXT,"ANNUALLY")>0 D
 .......I ABD="" S ABD="14.0"
 .......S ABDAYS=+$P(ABD,".",1)
 .......S ABHOURS=+$P(ABD,".",2)
 .......N YY S YY=YEAR ;$E(BASEDATE,2,3) ;JHT 7/1/2019
 .......N CC S CC=CENTURY ;$E(BASEDATE,1,1) ;JHT 7/1/2019
 .......N THEREST S THEREST=$E(BASEDATE,4,99)
 .......I ((($$FMADD^XLFDT(CENTURY_YEAR_THEREST,ABDAYS,ABHOURS))>(CENTURY_YEAR_DHMS))&(($$FMADD^XLFDT(CENTURY_YEAR_THEREST,-ABDAYS,-ABHOURS))<(CENTURY_YEAR_DHMS))) D ;are we currently in this years cycle ;mar 11,2020
 ........S BASEDATE=CENTURY_YEAR_THEREST ;mar 11,2020
 .......I (($$FMADD^XLFDT(CENTURY_YEAR_THEREST,ABDAYS,ABHOURS))<(CENTURY_YEAR_DHMS)) D ;DHMS=days hours minutes seconds ;are we past this year's cycle? ;added mar 6,2020
 ........S BASEDATE=CENTURY_YEAR_THEREST ;try this year
 ........S YY=YY+1
 ........I YY=100 S YY="00" S CC=CC+1
 ........I (($$FMADD^XLFDT(CC_YY_THEREST,-ABDAYS,-ABHOURS)))<RNOW D ;are we in next years cycle yet ;JHT 7/1/2019
 .........S BASEDATE=CC_YY_THEREST ;set to next years cycle
 .......I (($$FMADD^XLFDT(CENTURY_YEAR_THEREST,-ABDAYS,-ABHOURS))>(CENTURY_YEAR_DHMS)) D ;DHMS=days hours minutes seconds ;are we in this year's cycle? ;mar 6,2020
 ........;if not, try last year ;mar 6,2020
 ........S YY=YY-1 ;mar 6,2020
 ........I YY=-1 S YY=99 S CC=CC-1 ;mar 6,2020
 ........S BASEDATE=CC_YY_THEREST ;set to last year's cycle ;mar 6,2020
 .......I BASEDATE'>ADATE S BASEDATE=0 ;not due same day as admission in this case
 ......;end annually
 ......;start each month based on admit date starting month 3 skip each month 12
 ......I $F(RTEXT,"EACH MONTH BASED ON ADMIT DATE STARTING MONTH THREE SKIP EACH MONTH 12") D
 .......I $$FMADD^XLFDT(ADATE,82)<RNOW D ;start on the third month but leave some room prior to due
 ........I ABD="" S ABD="2.0"
 ........S ABDAYS=+$P(ABD,".",1)
 ........S ABHOURS=+$P(ABD,".",2)
 ........N DD S DD=$E(BASEDATE,6,7)
 ........N HMS S HMS=$P(RNOW,".",2)
 ........N THEREST S THEREST=$P(BASEDATE,".",2)
 ........S THEREST="1200" ;changed during override coding - noon for this rule
 ........N MONT S MONT=$E(RNOW,4,5)
 ........N MOA ;month of admission
 ........S MOA=$E(ADATE,4,5)
 ........;start by checking the current month
 ........N YEA S YEA=YEAR
 ........N CEN S CEN=CENTURY
 ........; Following line added by RMS to fix uncaught date issues 20191015
 ........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 ........;begin CRH JHT 2019 - bug fix during override coding
 ........I ((MONT=MOA)!($$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)>RNOW)!($$FMADD^XLFDT(BASEDATE,ABDAYS,ABHOURS)<RNOW)) D
 .........I ((MONT=MOA)!($$FMADD^XLFDT(BASEDATE,ABDAYS,ABHOURS)<RNOW)) D
 ..........;try next month first
 ..........S MONT=MONT+1
 ..........I MONT=13 S MONT=1 S YEA=YEA+1 I YEA=100 S YEA=0 S CEN=CEN+1
 ..........I $L(YEA)=1 S YEA="0"_YEA
 ..........I $L(MONT)=1 S MONT="0"_MONT
 ..........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ..........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 .........I ($$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)>RNOW) D
 ..........;not in this or next month's cycle yet, so try last month
 ..........S MONT=MONT-1
 ..........I MONT=0 D
 ...........S MONT=12
 ...........S YEA=YEA-1 I YEA=-1 D
 ............S YEA=99 S CEN=CEN-1
 ..........I $L(YEA)=1 S YEA="0"_YEA
 ..........I $L(MONT)=1 S MONT="0"_MONT
 ..........; Following line added by RMS to fix uncaught date issues 20191015
 ..........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ..........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 ..........I (MONT=MOA) D
 ...........;go to the previous month
 ...........S MONT=MONT-1
 ...........I MONT=0 D
 ............S MONT=12
 ............S YEA=YEA-1 I YEA=-1 D
 .............S YEA=99 S CEN=CEN-1
 ...........I $L(YEA)=1 S YEA="0"_YEA
 ...........I $L(MONT)=1 S MONT="0"_MONT
 ...........; Following line added by RMS to fix uncaught date issues 20191015
 ...........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ...........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 .........;end CRH JHT 2019 - bug fix during override coding
 ......;end each month based on admit date starting month 3 skip each month 12
 ......;start monthly
 ......I $F(RTEXT,"MONTHLY")>0 D
 .......I ABD="" S ABD="2.0"
 .......S ABDAYS=+$P(ABD,".",1)
 .......S ABHOURS=+$P(ABD,".",2)
 .......N DD S DD=$E(BASEDATE,6,7)
 .......N HMS S HMS=$P(RNOW,".",2)
 .......N THEREST S THEREST=$P(BASEDATE,".",2)
 .......N MONT S MONT=$E(RNOW,4,5)
 .......N MOA ;month of admission
 .......S MOA=$E(ADATE,4,5)
 .......;start by checking the current month
 .......N YEA S YEA=YEAR
 .......N CEN S CEN=CENTURY
 .......; Following line added by RMS to fix uncaught date issues 20191015
 .......I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 .......S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 .......;begin CRH JHT 2019 - bug fix during override coding
 .......I ((($F(RTEXT,"SKIP MONTH 12"))&(MONT=MOA))!($$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)>RNOW)!($$FMADD^XLFDT(BASEDATE,ABDAYS,ABHOURS)<RNOW)) D
 ........I ((($F(RTEXT,"SKIP MONTH 12"))&(MONT=MOA))!($$FMADD^XLFDT(BASEDATE,ABDAYS,ABHOURS)<RNOW)) D
 .........;try next month first
 .........S MONT=MONT+1
 .........I MONT=13 S MONT=1 S YEA=YEA+1 I YEA=100 S YEA=0 S CEN=CEN+1
 .........I $L(YEA)=1 S YEA="0"_YEA
 .........I $L(MONT)=1 S MONT="0"_MONT
 .........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 .........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 .........I ($$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS)>RNOW) D
 ..........;not in this or next month's cycle yet, so try last month
 ..........S MONT=MONT-1
 ..........I MONT=0 D
 ...........S MONT=12
 ...........S YEA=YEA-1 I YEA=-1 D
 ............S YEA=99 S CEN=CEN-1
 ..........I $L(YEA)=1 S YEA="0"_YEA
 ..........I $L(MONT)=1 S MONT="0"_MONT
 ..........; Following line added by RMS to fix uncaught date issues 20191015
 ..........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ..........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 ..........I (($F(RTEXT,"SKIP MONTH 12"))&(MONT=MOA)) D
 ...........;go to the previous month
 ...........S MONT=MONT-1
 ...........I MONT=0 D
 ............S MONT=12
 ............S YEA=YEA-1 I YEA=-1 D
 .............S YEA=99 S CEN=CEN-1
 ...........I $L(YEA)=1 S YEA="0"_YEA
 ...........I $L(MONT)=1 S MONT="0"_MONT
 ...........; Following line added by RMS to fix uncaught date issues 20191015
 ...........I DD>28 S DD=$$FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years
 ...........S BASEDATE=CEN_YEA_MONT_DD_"."_THEREST
 ...........;end CRH JHT 2019 - bug fix during override coding
 ......;end monthly
 ......;start weekly
 ......;start weekly
 ......I $F(RTEXT,"WEEKLY")>0 D
 .......I ABD="" S ABD="2.0"
 .......S ABDAYS=+$P(ABD,".",1)
 .......S ABHOURS=23 ;in this case we allow the full two days before ;august 1
 .......S ABMINS=59 ;august 1
 .......I BASEDATE>0 D
 ........N NUMDAYS
 ........S NUMDAYS=7
 ........N DSAD
 ........S DSAD=$$FMDIFF^XLFDT(RNOW,BASEDATE,1) ;difference in days
 ........I DSAD>0 D
 .........N MDSAD
 .........S MDSAD=DSAD#NUMDAYS
 .........S BASEDATE=$$FMADD^XLFDT(RNOW\1,7-MDSAD)_.2359
 .........I ((DSAD>7)&((MDSAD=0)!(MDSAD>ABD))) D ;still within last week's cycle
 ..........S BASEDATE=$$FMADD^XLFDT(BASEDATE,-7)
 .........I $F(RTEXT,"SKIP ADMISSION ANNIVERSAY WEEK")>0 D
 ..........N YY S YY=YEAR
 ..........N CC S CC=CENTURY
 ..........N THEREST S THEREST=$E(BASEDATE,4,99)
 ..........N ANNDATE
 ..........S ANNDATE=CENTURY_YEAR_THEREST ;admission anniversary date
 ..........I ((ANNDATE>$$FMADD^XLFDT(BASEDATE,-ABDAYS))&(ANNDATE<$$FMADD^XLFDT(BASEDATE,ABDAYS))) D
 ...........;skip this week since it is the anniversary week
 ...........S BASEDATE=$$FMADD^XLFDT(BASEDATE,-7) ;go to the previous week if this is anniversary week
 ......;end weekly
 ......I (RULELEVELFOUND="")&(($F(RTEXT,"X HOURS")>0)&($F(RTEXT,"EVERY")'>0)) D
 .......I ABD="" S ABD="0.8"
 .......S ABDAYS=+$P(ABD,".",1)
 .......S ABHOURS=+$P(ABD,".",2)
 .......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,6)),"^",1)
 .......I OFFSET>(ABHOURS+48) D
 ........I PREVAHOURS=0 D ;july 12, 2019
 .........S PREVAHOURS=OFFSET ;july 12, 2019
 .........S ABHOURS=OFFSET+48 ;we need to cover two days prior to admission as well ;july 12, 2019
 .......;I ((BASEDATE>0)&(OFFSET>0)) D
 .......I BASEDATE>0 D ;April 23,2020
 ........S BASEDATE=$$FMADD^XLFDT(BASEDATE,,OFFSET)
 .....;start every X hours based on admit date
 .....I RTEXT="EVERY X HOURS BASED ON ADMIT DATE" D
 ......S BASEDATE=$G(ADATE)
 ......I BASEDATE>0 D
 .......N NUMHOURS
 .......S NUMHOURS=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,6)),"^",1)
 .......I NUMHOURS>0 D
 ........N SSAD ;seconds since admit date
 ........S SSAD=$$FMDIFF^XLFDT(RNOW,ADATE,2) ;difference in seconds
 ........I SSAD>0 D
 .........N HSAD ;hours since admit date
 .........S HSAD=SSAD\3600
 .........N MHSAD
 .........S MHSAD=HSAD#NUMHOURS ;how many whole hours till next due time
 .........S BASEDATE=$$FMADD^XLFDT(ADATE,,HSAD+MHSAD) ;next due time
 .....;end every X hours based on admit date
 .....;start CRH JHT 2019 new rules during override logic coding
 .....;start ONE TIME - ADMISSION DAY PLUS X DAYS
 .....I RTEXT="ONE TIME - ADMISSION DAY PLUS X DAYS" D
 ......S BASEDATE=ADATE
 ......S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1)
 ......;I ((BASEDATE>0)&(OFFSET>0)) D
 ......I BASEDATE>0 D ;April 23,2020
 .......S BASEDATE=$$FMADD^XLFDT(BASEDATE,OFFSET)
 .......I BASEDATE>0 S BASEDATE=(BASEDATE\1)_".1200" ;set due time to noon
 .......;will have to set overdue logic to offset plus 12 hours to give the whole day
 .....;end ONE TIME - ADMISSION DAY PLUS X DAYS
 .....;end CRH JHT 2019 new rules during override logic coding
 .....;start every X days based on admit date
 .....I RTEXT="EVERY X DAYS BASED ON ADMIT DATE" D
 ......S BASEDATE=$G(ADATE)
 ......I BASEDATE>0 D
 .......N NUMDAYS
 .......S NUMDAYS=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1)
 .......I NUMDAYS>0 D
 ........N DSAD ;days since admit date
 ........S DSAD=$$FMDIFF^XLFDT(RNOW,BASEDATE,1) ;difference in days
 ........I DSAD>0 D
 .........N MDSAD
 .........S MDSAD=DSAD#NUMDAYS
 .........S BASEDATE=$$FMADD^XLFDT(RNOW\1,MDSAD)_.2359
 .........S ABHOURS=23 ;august 1
 .........S ABMINS=59 ;august 1
 .....;end every X days based on admit date
 .....I (RTEXT="DATE OF YEAR") D
 ......;use CENTURY and YEAR from above
 ......S BASEDATE=CENTURY_YEAR_$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,3)),"^",1)
 .....;start DAILY AT 0830
 .....I (RTEXT="DAILY AT 0830") D
 ......S BASEDATE=$P(RNOW,".",1)_".0830"
 .....;end DAILY AT 0830
 .....;start DAY OF WEEK
 .....I (RTEXT="DAY OF WEEK") D
 ......N NDOW S NDOW=$$DOW^XLFDT(RNOW)
 ......I NDOW'="" D
 .......N DOW S DOW=$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,5)),"^",1)
 .......I DOW'="" D
 ........;first find the most recent day that matches DOW
 ........S BASEDATE=RNOW
 ........I NDOW'=DOW  D ;today doesn't match - check most recent to see if within range
 .........N DFOUND
 .........S DFOUND=0
 .........N DTX
 .........N SAVEDATE
 .........S SAVEDATE=BASEDATE
 .........F DTX=1:1:7 Q:(DFOUND=1)  D
 ..........S BASEDATE=$$FMADD^XLFDT(BASEDATE,-1)
 ..........I $$DOW^XLFDT(BASEDATE)=DOW D
 ...........S DFOUND=1
 .........I DFOUND=1 D
 ..........;check to see if within range
 ..........;was I $$FMADD^XLFDT(BASEDATE,ABD)<RNOW D ;most recent no good, find next
 ..........I $$FMADD^XLFDT(BASEDATE,ABDAYS)<RNOW D ;most recent no good, find next ;changed ABD to ABDAYS during override coding
 ...........S BASEDATE=SAVEDATE
 ...........S DFOUND=0
 ...........F DTX=1:1:7 Q:(DFOUND=1)  D
 ............S BASEDATE=$$FMADD^XLFDT(BASEDATE,1)
 ............I $$DOW^XLFDT(BASEDATE)=DOW D
 .............S DFOUND=1
 .........S SAVEDATE=""
 ........S BASEDATE=$P(BASEDATE,".",1)_".2359"
 ........S ABHOURS=23 ;august 1
 ........S ABMINS=59 ;august 1
 .....;end DAY OF WEEK
 .....I (RTEXT="DATE OF MONTH") D
 ......N DOM S DOM=$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,4)),"^",1)
 ......I DOM'="" D
 .......I $L(DOM)=1 S DOM="0"_DOM
 .......I $L(DOM)=2 D
 ........N DNOW S DNOW=$E(DHMS,3,4) ;day now
 ........N MNOW S MNOW=$E(DHMS,1,2) ;month now
 ........N REMA S REMA=$P(DHMS,".",2) ;hours, minutes, and seconds
 ........;adjust for days in month
 ........I (DOM>28) D
 .........I (MNOW="02") D ;february
 ..........S DOM="28"
 .........I (DOM>30) D
 ..........I ((MNOW="04")!(MNOW="06")!(MNOW="09")!(MNOW="11")) D
 ...........S DOM="30"
 ........;check the acceptable range
 ........N ADAYS S ADAYS=$P(ABD,".",1)
 ........N CKD S CKD=CENTURY_YEAR_MNOW_DOM_"."_REMA
 ........I ((RNOW>$$FMADD^XLFDT(CKD,(-1*ADAYS)))&(RNOW<$$FMADD^XLFDT(CKD,ADAYS))) D
 .........S BASEDATE=CKD
 ........E  D
 .........I MNOW<12 S MNOW=MNOW+1
 .........I MNOW=12 S MNOW="01" S YEAR=YEAR+1
 .........I $L(YEAR)=1 S YEAR="0"_YEAR
 .........I $L(MNOW)=1 S MNOW="0"_MNOW
 .........I YEAR=100 S YEAR="00" S CENTURY=CENTURY+1
 .........S BASEDATE=CENTURY_YEAR_MNOW_DOM_"."_2359
 .........S ABHOURS=23 ;august 1
 .........S ABMINS=59 ;august 1
 .....I (RTEXT="ONE TIME - CALENDAR DATE") D
 ......S BASEDATE=$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,2)),"^",1)
 .....;I ((BASEDATE'=ADATE)&(BASEDATE'=0)&($F(RTEXT,"PASS-THROUGH REMINDER")'>0)) D ;not a pass through reminder ;added basedate'=adate during override coding - never due exactly at admit time and shows that no rules were applicable
 .....I ((($F(RTEXT,"X DAYS")>0)!($F(RTEXT,"X HOURS")>0)!(BASEDATE'=ADATE))&(BASEDATE'=0)&($F(RTEXT,"PASS-THROUGH REMINDER")'>0)) D ;April 23,2020 JHT - allow zero days or hours offset
 ......S RULELEVELFOUND=LEVEL
 ......N C9CCHECK
 ......S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,RPOINT,5)) I CKCODE'="" X CKCODE
 ......I C9CCHECK>0 D
 .......;check to see if there is a completed note in this range or after
 .......I NONOTE=1 D ;no completed notes of this title for this patient, this visit
 ........I DUEDATE<BASEDATE D
 .........S DUEDATE=BASEDATE
 .........S APRULE=RPOINT ;applicable rule
 .........I $F(RTEXT,"X DAYS")>0 D ;added during override coding
 ..........S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1) ;added during override coding
 ..........S OFST=OFFSET ;added during override coding
 .........I $F(RTEXT,"X HOURS")>0 D ;added during override coding
 ..........S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,6)),"^",1) ;added during override coding
 ..........S OFST=OFFSET ;added during override coding
 .......I NONOTE=0 D
 ........;check to see if there is a completed note of this title within or after this time period
 ........;first check within the time period
 ........N LASTDATE
 ........S LASTDATE=(9999999-INVDATE) ;date of last completed note of this title
 ........I (LASTDATE'>$$FMADD^XLFDT(BASEDATE,-ABDAYS,-ABHOURS,-ABMINS)) D  ;this means a note has not been done during or after the time period
 .........I DUEDATE<BASEDATE D
 ..........S APRULE=RPOINT
 ..........S DUEDATE=BASEDATE
 ..........I $F(RTEXT,"X DAYS")>0 D ;added during override coding
 ...........S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,1)),"^",1) ;added during override coding
 ...........S OFST=OFFSET ;added during override coding
 ..........I $F(RTEXT,"X HOURS")>0 D ;added during override coding
 ...........S OFFSET=+$P($G(^C9C(300890,REIEN,SEPAR,WF,1,FR,6)),"^",1) ;added during override coding
 ...........S OFST=OFFSET ;added during override coding
 .....E  D ;pass through reminder
 ......I ((BASEDATE'=0)&($F(RTEXT,"PASS-THROUGH REMINDER")>0)) D ;added find text during override coding to help eliminate due at exact admit time bug
 .......S RULELEVELFOUND=LEVEL
 .......N PHRASE
 .......S PHRASE=""
 .......I $G(REMIEN)>0 D
 ........S PHRASE=$P($G(^PXD(811.9,REMIEN,0)),"^",4)
 .......N C9CCHECK
 .......I PHRASE'="" D
 ........S C9CCHECK=$$SHOW^C9CPTR(PHRASE,BASEDATE)
 .......E  D
 ........S C9CCHECK=0 N CKCODE S CKCODE=$G(^C9C(300892,RPOINT,5)) I CKCODE'="" X CKCODE
 .......I C9CCHECK>0 D
 ........I DUEDATE<BASEDATE D
 .........S APRULE=RPOINT_"^"_$G(PHRASE)
 .........S DUEDATE=BASEDATE
 ;----------------------
 Q
RPCGNOTE(C9CRET,NOTEIEN) ;RPC to bring back text of note
 Q:$G(NOTEIEN)'>0
 N GRET
 N CNT
 S CNT=0
 D TGET^TIUSRVR1(.GRET,+NOTEIEN)
 I GRET'="" D
 .N PATIEN
 .S PATIEN=$P($G(^TIU(8925,+NOTEIEN,0)),"^",2)
 .I PATIEN>0 D
 ..N PATNAME
 ..S PATNAME=$P($G(^DPT(PATIEN,0)),"^",1)
 ..S CNT=CNT+1
 ..S C9CRET(CNT)="Patient: "_PATNAME        
 ..N NX
 ..S NX=0 F  S NX=$O(@GRET@(NX)) Q:NX'>0  D
 ...S CNT=CNT+1
 ...S C9CRET(CNT)=(@GRET@(NX))
 Q
RPCAF(C9CRET) ;get all possible filter values - in this case, note titles that have rules engine rules
 ;RPC-C9C NOTES DUE ALL FILTER VALUES
 N CNT
 S CNT=0
 N RX
 S RX=0 F  S RX=$O(^C9C(300890,RX)) Q:RX'>0  D
 .I $P($G(^C9C(300890,RX,4)),"^",1)'>0 D ;not inactive
 ..N INCLUDE
 ..S INCLUDE=0
 ..N FX
 ..S FX=0 F  S FX=$O(^C9C(300890,RX,1,FX)) Q:FX'>0  D ;check for ward rules
 ...N AX
 ...S AX=$O(^C9C(300890,RX,1,FX,1,0))
 ...I AX>0 S INCLUDE=1
 ..I INCLUDE=0 D
 ...S FX=0 F  S FX=$O(^C9C(300890,RX,2,FX)) Q:FX'>0  D ;check for unit rules
 ....N AX
 ....S AX=$O(^C9C(300890,RX,2,FX,1,0))
 ....I AX>0 S INCLUDE=1
 ..I INCLUDE=0 D
 ...S FX=0 F  S FX=$O(^C9C(300890,RX,3,FX)) Q:FX'>0  D ;check for hospital rules
 ....N AX
 ....S AX=$O(^C9C(300890,RX,3,FX,1,0))
 ....I AX>0 S INCLUDE=1
 ..I INCLUDE=1 D ;include this formula in returned data
 ...N VPTR
 ...S VPTR=$P($G(^C9C(300890,RX,0)),"^",1)
 ...I VPTR'="" D
 ....N ENTRY S ENTRY=$P(VPTR,";",1)
 ....N FILEG S FILEG=$P(VPTR,";",2)
 ....I ((ENTRY'="")&(FILEG'="")) D
 .....N FN S FN="^"_FILEG_ENTRY_",0)"
 .....N ENTNAME S ENTNAME=$P($G(@(FN)),"^",1)
 .....I ENTNAME'="" D
 ......S CNT=CNT+1
 ......S C9CRET(CNT)=ENTRY_"^"_ENTNAME
 Q
FIXDAY(DD,MONT,YEA,CEN) ; Fix the day for non-31 day months including leap years - RMS for CRH 20191016
 Q:DD<29 DD
 I (MONT="02"),(DD>28) S DD=28+$$LEAP^%DTC(1700+(CEN_YEAR))  ; 1 if leap year, 0 if not
 I (DD>30),((MONT="04")!(MONT="06")!(MONT="09")!(MONT="11")) S DD=30
 Q DD
