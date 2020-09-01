C9CMODRM ;CRH JHT 2019 - reminders module for dashboard
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
RPCFILT(C9CRET);RPC=C9C C AND R FILTERS
 ;C9CRET - return array of ien^name
 N CNT
 S CNT=0
 N ALL
 D GETCANDR(.ALL) ;returns RNAME^RIEN for each
 N AX
 S AX=0 F  S AX=$O(ALL(AX)) Q:AX'>0  D
 .S C9CRET($I(CNT))=$P(ALL(AX),"^",2)_"^"_$P(ALL(AX),"^",1) ;IEN^NAME
 Q
ALLINPTS(C9CRET,AVOIDBUG,FILTERS) ;check -C and -R reminders for all inpatients
 ;C9CRET - return array
 ;FILTERS - array of reminders to check
 Q:$G(DUZ)'>0
 Q:$G(DUZ(2))'>0 ;no division
 N REMS ;reminder array to check if no filters
 N CNT
 S CNT=0
 I $G(FILTERS(0))="" D ;no filters
 .D GETCANDR(.REMS) ;returns RNAME^RIEN for each
 .N FCNT
 .S FCNT=0
 .N FX
 .S FX=0 F  S FX=$O(REMS(FX)) Q:FX'>0  D
 ..S FCNT=FCNT+1
 ..S FILTERS(FCNT)=$P(REMS(FX),"^",2)_"^"_$P(REMS(FX),"^",1)
 N WARD ;from ward location file
 S WARD="" F  S WARD=$O(^DPT("ACN",WARD)) Q:WARD=""  D
 .I ((WARD'="ZZTESTWARD")&(WARD'="ZZBCMATRAIN")) D
 ..N WIEN ;ward ien
 ..S WIEN=$O(^DIC(42,"B",WARD,0))
 ..I WIEN>0 D
 ...N MCD ;medical center division pointer
 ...S MCD=$P($G(^DIC(42,WIEN,0)),"^",11)
 ...I MCD>0 D
 ....N INST ;institution pointer
 ....S INST=$P($G(^DG(40.8,MCD,0)),"^",7)
 ....I INST>0 D
 .....I INST=DUZ(2) D ;matches user institution
 ......N PAT
 ......S PAT=0 F  S PAT=$O(^DPT("ACN",WARD,PAT)) Q:PAT'>0  D
 .......N THISREM
 .......N AX
 .......S AX="" F  S AX=$O(FILTERS(AX)) Q:AX=""  D
 ........N RNAME
 ........S RNAME=$P(FILTERS(AX),"^",2)
 ........S THISREM=$P(FILTERS(AX),"^",1)
 ........I THISREM>0 D
 .........N RET
 .........D REM(.RET,PAT,THISREM)
 .........I +RET>0 D ;if applicable and due
 ..........N PATNAME
 ..........S PATNAME=$P($G(^DPT(PAT,0)),"^",1)
 ..........N DUEDATE
 ..........S DUEDATE=$$FMTE^XLFDT($P(RET,"^",2))
 ..........N WEBSITE
 ..........S WEBSITE=""
 ..........N WSIEN
 ..........S WSIEN=$O(^PXD(811.9,THISREM,50,0)) ;we only get the first one
 ..........I WSIEN>0 D
 ...........S WEBSITE=$P($G(^PXD(811.9,THISREM,50,WSIEN,0)),"^",1)
 ..........;DFN^PATNAME^WARD^REMINDERNAME^DUEDATE^COLOR^WEBSITE
 ..........S C9CRET($I(CNT))=PAT_"^"_PATNAME_"^"_WARD_"^"_RNAME_"^"_DUEDATE_"^"_$P(RET,"^",3)_"^"_WEBSITE
 .........K RET
 Q
WARDPATS(C9CRET,WARD,FILTERS) ;check -C and -R reminders for patients on one ward
 ;C9CRET - return array
 ;WARD - ward name from ward/location file
 ;FILTERS - array of reminders to check
 N REMS ;reminder array to check if no filters
 N CNT
 S CNT=0
 I $G(FILTERS(0))="" D ;no filters
 .D GETCANDR(.REMS) ;returns RNAME^RIEN for each
 .N FCNT
 .S FCNT=0
 .N FX
 .S FX=0 F  S FX=$O(REMS(FX)) Q:FX'>0  D
 ..S FCNT=FCNT+1
 ..S FILTERS(FCNT)=$P(REMS(FX),"^",2)_"^"_$P(REMS(FX),"^",1)
 N PAT
 S PAT=0 F  S PAT=$O(^DPT("ACN",WARD,PAT)) Q:PAT'>0  D
 .N THISREM
 .N AX
 .S AX="" F  S AX=$O(FILTERS(AX)) Q:AX=""  D
 ..N RNAME
 ..S RNAME=$P(FILTERS(AX),"^",2)
 ..S THISREM=$P(FILTERS(AX),"^",1)
 ..I THISREM>0 D
 ...N RET
 ...D REM(.RET,PAT,THISREM)
 ...I +RET>0 D ;if applicable and due
 ....N PATNAME
 ....S PATNAME=$P($G(^DPT(PAT,0)),"^",1)
 ....N DUEDATE 
 ....S DUEDATE=$$FMTE^XLFDT($P(RET,"^",2))
 ....N WEBSITE
 ....S WEBSITE=""
 ....N WSIEN
 ....S WSIEN=$O(^PXD(811.9,THISREM,50,0)) ;we only get the first one
 ....I WSIEN>0 D
 .....S WEBSITE=$P($G(^PXD(811.9,THISREM,50,WSIEN,0)),"^",1)
 ....;DFN^PATNAME^REMINDERNAME^DUEDATE^COLOR^WEBSITE
 ....S C9CRET($I(CNT))=PAT_"^"_PATNAME_"^"_RNAME_"^"_DUEDATE_"^"_$P(RET,"^",3)_"^"_WEBSITE
 ...K RET
 Q
REM(C9CRET,PAT,REMIEN) ;check the resolution for one reminder
 ;C9CRET - piece 1 - returns 0 if not due, 1 if due
 ;         piece 2 - date due
 ;         piece 3 - color of row in dashboard if yellow or red
 ;PAT - patient dfn
 ;REMIEN - reminder ien
 ;once reminder name is retrieved, different logic will apply based on the name
 ;DASHBOARD-C - cohort logic only
 ;DASHBOARD-R - resolution logic only
 S C9CRET="0^0" ;assume false unless proven otherwise below
 N DEFARR,FIEVAL,NODE,ONCOHORT,PXRMDEFS,RDATEDUE,REMNAME,RNAME,RSTATUS
 S RDATEDUE=0
 S ONCOHORT=""
 I REMIEN>0 D
 .S REMNAME=$P($G(^PXD(811.9,REMIEN,0)),"^",1)
 .S RNAME=$P($G(^PXD(811.9,REMIEN,0)),"^",3) ;print name
 .I RNAME="" S RNAME=REMNAME
 .N DATE ;must be newed but not populated to return current value
 .I $G(PXRMID)'="" K ^TMP(PXRMID,$J)
 .K ^TMP("PXRHM",$J,REMIEN)
 .D DEF^PXRMLDR(REMIEN,.DEFARR) ;set up variables for this reminder check
 .D EVAL^PXRM(PAT,.DEFARR,5,1,.FIEVAL)
 .S NODE=$G(^TMP("PXRHM",$J,REMIEN,RNAME))
 .S RSTATUS=$P(NODE,"^",1)
 .S RDATEDUE=$P(NODE,"^",2)
 .S ONCOHORT=$$REMSTAT(RSTATUS)
 .I ONCOHORT="" Q
 .K FIEVAL
 .I $F(REMNAME,"DASHBOARD-C")=12 D ;cohort logic only
 ..I ONCOHORT'="" D
 ...S C9CRET="1^"_$P($$NOW^XLFDT,".",1)_".2359"_"^LightCoral"
 .I $F(REMNAME,"DASHBOARD-R")=12 D ;resolution logic only
 ..I RSTATUS="N/A" S C9CRET=0
 ..E  I ((RSTATUS="DUE NOW")&(RDATEDUE'>0)) D
 ...S C9CRET="1^"_$P($$NOW^XLFDT,".",1)_".2359"_"^LightCoral"
 ..E  I ((RDATEDUE>0)&(RDATEDUE<$$NOW^XLFDT)) D
 ...S C9CRET="1^"_RDATEDUE_"^LightCoral"
 ..E  I RDATEDUE>0 D
 ...N PHRDUE ;phrase in advance due field
 ...S PHRDUE=$P($G(^PXD(811.9,REMIEN,0)),"^",4)
 ...N INTDUE,ONETHIRD
 ...S INTDUE=+PHRDUE
 ...I INTDUE>0 D
 ....N UNITDUE,INDX,DAYS,HOURS
 ....S INDX=$F(PHRDUE,INTDUE)
 ....S UNITDUE=$E(PHRDUE,INDX)
 ....I UNITDUE="D" D
 .....S ONETHIRD=INTDUE\3
 .....I $$FMADD^XLFDT($$NOW^XLFDT,ONETHIRD)>RDATEDUE D
 ......S C9CRET="1^"_RDATEDUE_"^Yellow"
 .....E  D
 ......I $$FMADD^XLFDT($$NOW^XLFDT,INTDUE)>RDATEDUE D
 .......S C9CRET="1^"_RDATEDUE
 ....E  I UNITDUE="H" D
 .....S ONETHIRD=INTDUE\3
 .....I $$FMADD^XLFDT($$NOW^XLFDT,,ONETHIRD)>RDATEDUE D
 ......S C9CRET="1^"_RDATEDUE_"^Yellow"
 .....E  D
 ......I $$FMADD^XLFDT($$NOW^XLFDT,,INTDUE)>RDATEDUE D
 .......S C9CRET="1^"_RDATEDUE
 ....E  I UNITDUE="M" D
 .....S INTDUE=INTDUE*30
 .....S ONETHIRD=INTDUE\3
 .....I $$FMADD^XLFDT($$NOW^XLFDT,ONETHIRD)>RDATEDUE D
 ......S C9CRET="1^"_RDATEDUE_"^Yellow"
 .....E  D
 ......I $$FMADD^XLFDT($$NOW^XLFDT,INTDUE)>RDATEDUE D
 .......S C9CRET="1^"_RDATEDUE
 ....E  I UNITDUE="W" D
 .....S INTDUE=INTDUE*7
 .....S ONETHIRD=INTDUE\3
 .....I $$FMADD^XLFDT($$NOW^XLFDT,ONETHIRD)>RDATEDUE D
 ......S C9CRET="1^"_RDATEDUE_"^Yellow"
 .....E  D
 ......I $$FMADD^XLFDT($$NOW^XLFDT,INTDUE)>RDATEDUE D
 .......S C9CRET="1^"_RDATEDUE
 ....E  I UNITDUE="Y" D
 .....S INTDUE=INTDUE*364
 .....S ONETHIRD=INTDUE\3
 .....I $$FMADD^XLFDT($$NOW^XLFDT,ONETHIRD)>RDATEDUE D
 ......S C9CRET="1^"_RDATEDUE_"^Yellow"
 .....E  D
 ......I $$FMADD^XLFDT($$NOW^XLFDT,INTDUE)>RDATEDUE D
 .......S C9CRET="1^"_RDATEDUE
 Q
REMSTAT(RSTAT)
 I $$STATMTCH^PXRMORCH(RSTAT,"D") Q "CD"
 I $$STATMTCH^PXRMORCH(RSTAT,"A") Q "C"
 Q ""
GETCANDR(C9CRET) ;get DASHBOARD-C and DASHBOARD-R reminders
 N CNT
 S CNT=0
 N RBN
 S RBN="DASHBOARD-C" F  S RBN=$O(^PXD(811.9,"B",RBN)) Q:((RBN="")!($F(RBN,"DASHBOARD-C")'=12))  D
 .N RX
 .S RX=0 F  S RX=$O(^PXD(811.9,"B",RBN,RX)) Q:RX'>0  D
 ..N RNAME
 ..S RNAME=$P($G(^PXD(811.9,RX,0)),"^",3) ;get print name if exists
 ..I RNAME="" S RNAME=$P($G(^PXD(811.9,RX,0)),"^",1) ;name if print name not found 
 ..I RNAME'="" D
 ...I $P($G(^PXD(811.9,RX,0)),"^",6)'>0 D ;not inactive
 ....S C9CRET($I(CNT))=RNAME_"^"_RX
 S RBN="DASHBOARD-R" F  S RBN=$O(^PXD(811.9,"B",RBN)) Q:((RBN="")!($F(RBN,"DASHBOARD-R")'=12))  D
 .N RX
 .S RX=0 F  S RX=$O(^PXD(811.9,"B",RBN,RX)) Q:RX'>0  D
 ..N RNAME
 ..S RNAME=$P($G(^PXD(811.9,RX,0)),"^",3) ;get print name if exists
 ..I RNAME="" S RNAME=$P($G(^PXD(811.9,RX,0)),"^",1) ;name if print name not found
 ..I RNAME'="" D
 ...I $P($G(^PXD(811.9,RX,0)),"^",6)'>0 D ;not inactive
 ....S C9CRET($I(CNT))=RNAME_"^"_RX
 Q
 
