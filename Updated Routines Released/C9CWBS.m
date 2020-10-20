C9CWBS	;CRH JHT 2017 - RPC's for Rules Engine
	;Copyright [2020] [Central Regional Hospital, State of North Carolina];;;;;Build 11
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
	Q ;no entry from top;;;;;Build 5
RPCDR(C9CRET,IENSTRING)	;delete a rule
	;IENSTRING - "RECORD IEN FOR TIU TITLE;SEPARATOR;FORMULA IEN;SEPARATOR;RULE IEN"
	S C9CRET=0
	Q:$G(IENSTRING)=""
	N TIEN
	S TIEN=$P(IENSTRING,";",1)
	Q:TIEN'>0
	N SEP1
	S SEP1=$P(IENSTRING,";",2)
	Q:SEP1'>0
	N FIEN
	S FIEN=$P(IENSTRING,";",3)
	Q:FIEN'>0
	N SEP2
	S SEP2=$P(IENSTRING,";",4)
	Q:SEP2'>0
	N RIEN
	S RIEN=$P(IENSTRING,";",5)
	Q:RIEN'>0
	N DIK,DA
	S DA(2)=TIEN
	S DA(1)=FIEN
	S DA=RIEN
	S DIK="^C9C(300890,"_DA(2)_","_SEP1_","_DA(1)_","_SEP2_","
	D ^DIK
	S C9CRET=1
	Q
RPCAR(C9CRET,COHORT,PTRRULEIEN,IENSTRING,XDAYSA,XHOURSA,CDATE,DATEOFY,DATEOFM,DAYOW,XDAYSE,XHOURSE,ORDABLE,PASSTHIEN)	;add a rule
	;COHORT - "WARD" or "UNIT" or "HOSPITAL"
	;PTRRULEIEN - pointer to RULES file
	;IENSTRING - "RECORD IEN FOR TIU TITLE;SEPARATOR;FORMULA IEN;SEPARATOR;RULE IEN"
	;XDAYSA - number of days from admit date
	;XHOURSA - number of hours from admit date
	;CDATE - external calendar date
	;DATEOFY - date of year like "0709"
	;DATEOFM - date of month 1-28
	;DAYOW - Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday
	;XDAYSE - number of days from event date
	;XHOURSE - number of hours from event date
	;ORDABLE - pointer to orderable items file
	       ;PASSTHIEN - pointer to reminder definition file
	S C9CRET=0
	Q:PTRRULEIEN'>0
	N TIEN
	S TIEN=$P(IENSTRING,";",1)
	Q:TIEN'>0
	N FIEN
	S FIEN=$P(IENSTRING,";",3)
	Q:FIEN'>0
	I CDATE'="" D
	.N X,Y
	.S X=CDATE D ^%DT
	.S CDATE=Y
	N SUBFILE
	S SUBFILE=0
	I COHORT="WARD" S SUBFILE=300890.11
	I COHORT="UNIT" S SUBFILE=300890.21
	I COHORT="HOSPITAL" S SUBFILE=300890.31
	Q:SUBFILE=0
	N JFDA,JMSG,JFIEN
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",.01)=PTRRULEIEN
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",1)=$G(XDAYSA)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",6)=$G(XHOURSA)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",2)=$G(CDATE)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",3)=$G(DATEOFY)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",4)=$G(DATEOFM)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",5)=$G(DAYOW)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",7)=$G(ORDABLE)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",8)=$G(XDAYSE)
	S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",9)=$G(XHOURSE)
	       S JFDA(42,SUBFILE,"+3,"_FIEN_","_TIEN_",",10)=$G(PASSTHIEN)
	D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	I '$D(JMSG) S C9CRET=1
	;I $D(JMSG) M ^JHT($J,"RPCAR")=JMSG
	Q
RPCSF(C9CRET,RIEN,COHORT,FORNAME)	;save a new formula name
	;RIEN - IEN of TIU DOCUMENT DEFINITION this formula applies to
	;COHORT - WARD, UNIT, or HOSPITAL
	;FORNAME - New Formula Name no greater than 80 characters, unique for this cohort
	S C9CRET=0
	Q:$G(COHORT)=""
	Q:$G(FORNAME)=""
	Q:$G(RIEN)=0
	N SUBFILE
	S SUBFILE=0
	I COHORT="WARD" S SUBFILE=300890.01
	I COHORT="UNIT" S SUBFILE=300890.02
	I COHORT="HOSPITAL" S SUBFILE=300890.03
	Q:SUBFILE=0
	N SEPARATOR
	S SEPARATOR=0
	I COHORT="WARD" S SEPARATOR=1
	I COHORT="UNIT" S SEPARATOR=2
	I COHORT="HOSPITAL" S SEPARATOR=3
	Q:SEPARATOR=0
	;check to be sure this is a unique formula name for this cohort
	N DUP
	S DUP=0
	N AX
	S AX=0 F  S AX=$O(^C9C(300890,RIEN,SEPARATOR,AX)) Q:AX'>0  D
	.I FORNAME=$P($G(^C9C(300890,RIEN,SEPARATOR,AX,0)),"^",1) D
	..S DUP=1
	Q:DUP=1 ;duplicate name for cohort
	N JFDA,JMSG,JFIEN
	S JFDA(42,300890,"?+1,",.01)=RIEN_";TIU(8925.1,"
	D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	I $D(JMSG) M ^JHT($J,"RPCSF",1)=JMSG
	I '$D(JMSG) D
	.N RECNO
	.S RECNO=$G(JFIEN(1))
	.I RECNO>0 D
	..N JFDA,JMSG,JFIEN
	..S JFDA(42,SUBFILE,"?+3,"_RECNO_",",.01)=FORNAME
	..D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	..I '$D(JMSG) D
	...I COHORT="HOSPITAL" D
	....I $G(JFIEN(3))>0 D ;third level
	.....N THISREC
	.....S THISREC=JFIEN(3)
	.....;M ^JHT("JFIEN")=JFIEN
	.....I $G(DUZ(2))>0 D
	......N JFDA,JMSG,JFIEN
	......S JFDA(42,300890.06,"?+2,"_THISREC_","_RECNO_",",.01)=DUZ(2)
	......D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	......I '$D(JMSG) D
	.......S C9CRET=1
	...I COHORT'="HOSPITAL" D
	....S C9CRET=1
	Q
RPCDF(C9CRET,RIEN,FORIEN,COHORT)	;delete a formula
	;RIEN - IEN of TIU DOCUMENT DEFINITION this formula applies to
	;COHORT - WARD, UNIT, or HOSPITAL
	S C9CRET=0
	Q:$G(COHORT)=""
	Q:$G(RIEN)=0
	N SUBFILE
	S SUBFILE=0
	I COHORT="WARD" S SUBFILE=300890.01
	I COHORT="UNIT" S SUBFILE=300890.02
	I COHORT="HOSPITAL" S SUBFILE=300890.03
	Q:SUBFILE=0
	N SEPARATOR
	S SEPARATOR=0
	I COHORT="WARD" S SEPARATOR=1
	I COHORT="UNIT" S SEPARATOR=2
	I COHORT="HOSPITAL" S SEPARATOR=3
	Q:SEPARATOR=0
	N DIK,DA
	S DA(1)=RIEN
	S DA=FORIEN
	S DIK="^C9C(300890,"_DA(1)_","_SEPARATOR_","
	D ^DIK
	S C9CRET=1
	Q
RPCSU(C9CRET,UNITNAMES,RIEN)	;save included units for a rules engine formula
	;UNITIENS - "IEN^IEN^IEN..."
	;RIEN - "RECORD IEN FOR TIU TITLE;1;FORMULA IEN;1;RULE IEN"
	S C9CRET=1 ;succeeded
	N RECIEN
	S RECIEN=$P($G(RIEN),";",1)
	Q:RECIEN'>0
	N FORIEN
	S FORIEN=$P($G(RIEN),";",3)
	Q:FORIEN'>0
	N UNITS
	N THISUNIT
	S THISUNIT="EMPTY"
	N CNT
	S CNT=0 F  S CNT=CNT+1 Q:THISUNIT=""  D
	.S THISUNIT=$P(UNITNAMES,"^",CNT)
	.I THISUNIT'="" D
	..S UNITS(THISUNIT)=""
	;first delete the current units for this record
	N DX
	S DX=0 F  S DX=$O(^C9C(300890,RECIEN,2,FORIEN,2,DX)) Q:DX'>0  D
	.N DIK,DA
	.S DA(2)=RECIEN
	.S DA(1)=FORIEN
	.S DA=DX
	.S DIK="^C9C(300890,"_DA(2)_",2,"_DA(1)_",2,"
	.D ^DIK
	;now add the new ones
	;M ^JHT($J,"UNITSAVE")=UNITS
	N WX
	S WX="" F  S WX=$O(UNITS(WX)) Q:WX=""  D
	.N JFDA,JMSG,JFIEN
	.S JFDA(42,300890.05,"?+3,"_FORIEN_","_RECIEN_",",.01)=WX
	.D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	.;S ^JHT($J,"UNITS",WX)=$G(JMSG)
	.I $D(JMSG) S C9CRET=0
	Q
RPCSW(C9CRET,WARDIENS,RIEN)	;save included wards for a rules engine formula
	;WARDIENS - "IEN^IEN^IEN..."
	;RIEN - "RECORD IEN FOR TIU TITLE;1;FORMULA IEN;1;RULE IEN"
	S C9CRET=1 ;succeeded
	N RECIEN
	S RECIEN=$P($G(RIEN),";",1)
	Q:RECIEN'>0
	N FORIEN
	S FORIEN=$P($G(RIEN),";",3)
	Q:FORIEN'>0
	N WARDS
	N THISIEN
	S THISIEN=9999999
	N CNT
	S CNT=0 F  S CNT=CNT+1 Q:THISIEN'>0  D
	.S THISIEN=$P(WARDIENS,"^",CNT)
	.I THISIEN>0 D
	..S WARDS(THISIEN)=""
	;first delete the current wards for this record
	N DX
	S DX=0 F  S DX=$O(^C9C(300890,RECIEN,1,FORIEN,2,DX)) Q:DX'>0  D
	.N DIK,DA
	.S DA(2)=RECIEN
	.S DA(1)=FORIEN
	.S DA=DX
	.S DIK="^C9C(300890,"_DA(2)_",1,"_DA(1)_",2,"
	.D ^DIK
	;now add the new ones
	N WX
	S WX=0 F  S WX=$O(WARDS(WX)) Q:WX'>0  D
	.N JFDA,JMSG,JFIEN
	.S JFDA(42,300890.04,"?+3,"_FORIEN_","_RECIEN_",",.01)=WX
	.D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	.I $D(JMSG) S C9CRET=0
	Q
RPCEOI(C9CRET)	;get eligible orderable items
	N CX
	S CX=0
	N OLIST
	N OIT
	S OIT="" F  S OIT=$O(^ORD(101.43,"B",OIT)) Q:OIT=""  D
	.N THISONE
	.S THISONE=0 F  S THISONE=$O(^ORD(101.43,"B",OIT,THISONE)) Q:THISONE'>0  D
	..I $P($G(^ORD(101.43,THISONE,300002)),"^",1)>0 D
	...N ONAME
	...S ONAME=$P($G(^ORD(101.43,THISONE,0)),"^",1)
	...I ONAME'="" D
	....S OLIST(ONAME)=THISONE ;added block level jht - was a bug
	I $D(OLIST) D
	.N TX
	.S TX="" F  S TX=$O(OLIST(TX)) Q:TX=""  D
	..S CX=CX+1
	..S C9CRET(CX)=OLIST(TX)_"~"_TX
	Q
RPCWB(Y)	; RETURN LIST OF ACTIVE WARDS with BEDSECTION
	N I,IEN,NAME,D0
	S I=1,NAME=""
	N JHTWW
	D GETNWRDS(.JHTWW,DUZ(2))  ;;Get wards in current institution
	F  S NAME=$O(^DIC(42,"B",NAME)) Q:NAME=""  S IEN=$O(^(NAME,0)) D
	.S D0=IEN D WIN^DGPMDDCF ;is this ward location currently inactive
	.I (X=0)&($D(JHTWW(IEN))) D
	..N UNIT
	..S UNIT=$P($G(^DIC(42,IEN,0)),"^",2)
	..I UNIT'="" D
	...I ((UNIT'="CERT")&(UNIT'="OML")) D
	....S Y(I)=IEN_"^"_NAME_"^"_UNIT,I=I+1
	Q
GETNWRDS(JHTW,JHTIEN)	
	   ;Input - variable to hold wards
	   ;Input - IEN of entry in INSTITUTION FILE #4
	   ;JHTW is set to number of nursing wards from 211.4 in this INSTITUTION
	   ;JHTW(IEN),JHTW(IEN),etc. is IEN in file 211.4 and the MAS Ward from
	   ;file WARD LOCATION FILE #42, like JHTW(28)="WARD A154"
	   ;If SERVICE in WARD LOCATION FILE #42 is set to "NC" for Non-Count,
	   ;that ward is NOT returned.
	   ;If Field 1.5 (INACTIVE FLAG) in file 211.4 is not set to "A" (ACTIVE),
	   ;that ward is not returned
	   ;Set up array to use to check Inactive ward flag
	   N JTX,IIX,IIX2
	   S IIX=0
	   F  S IIX=$O(^NURSF(211.4,IIX)) Q:IIX'>0  I $D(^NURSF(211.4,IIX,3,1,0)) S IIX2=^NURSF(211.4,IIX,3,1,0) S JTX(IIX)=IIX2
	   N JHTNW,%,%1
	   S %=0,%1=0
	   ;F  S %=$O(^NURSF(211.4,%)) Q:%'>0  I $D(JTX(%)) I ^NURSF(211.4,%,"I")="A" S JHTNW(^NURSF(211.4,%,3,1,0))=^NURSF(211.4,%,0) ;commented PGH jhthurber 2020
           F  S %=$O(^NURSF(211.4,%)) Q:%'>0  I $D(JTX(%)) I $G(^NURSF(211.4,%,"I"))'="I" S JHTNW(^NURSF(211.4,%,3,1,0))=^NURSF(211.4,%,0) ;added PGH jhthurber 2020
	   S %=0
	   F  S %=$O(JHTNW(%)) Q:%'>0  I $P(^SC(JHTNW(%),0),"^",4)=JHTIEN,$P(^DIC(42,%,0),"^",3)'="NC" S JHTW(%)=$P(^DIC(42,%,0),"^",1),%1=%1+1
	   S JHTW=%1
	   Q
GETRMDRS(C9CRET)	;get reminders for cohort logic only	
	N CNT
	S CNT=0
	N RBN
	S RBN="DASHBOAR" F  S RBN=$O(^PXD(811.9,"B",RBN)) Q:((RBN="")!($F(RBN,"DASHBOARD")'=10))  D
	.N RX 
	.S RX=0 F  S RX=$O(^PXD(811.9,"B",RBN,RX)) Q:RX'>0  D
	..N RNAME
	..S RNAME=$P($G(^PXD(811.9,RX,0)),"^",1)
	..I RNAME'="" D
	...S C9CRET($I(CNT))=RNAME_"^"_RX 
	Q
GETPTRM(C9CRET)	;get reminders for pass-through logic only
	N CNT
	S CNT=0
	N RBN
	S RBN="PTDASHBOAR" F  S RBN=$O(^PXD(811.9,"B",RBN)) Q:((RBN="")!($F(RBN,"PTDASHBOARD")'=12))  D
	.N RX
	.S RX=0 F  S RX=$O(^PXD(811.9,"B",RBN,RX)) Q:RX'>0  D
	..N RNAME
	..S RNAME=$P($G(^PXD(811.9,RX,0)),"^",1)
	..I RNAME'="" D
	...S C9CRET($I(CNT))=RNAME_"^"_RX
	Q
GETCFT(C9CRET,VPTR)	;get cohort for title - RPC=C9C GET COHORT FOR TITLE
	;VPTR = Ruled Object variable pointer in C9C RULES ENGINE FILE
	;returns ien^name from file 811.9 for given title ien
	Q:+$G(VPTR)'>0
	N FPTD
	S FPTD=$P(VPTR,";",2)
	I ((FPTD'="TIU(8925.1,")&(FPTD'="AUTTIMM(,")) Q ;not a tiu document def or immunization
	N TIEN
	S TIEN=$O(^C9C(300890,"B",VPTR,0))
	Q:TIEN'>0 ;This object not defined in file 300890
	N REMIEN
	S REMIEN=$P($G(^C9C(300890,TIEN,6)),"^",1)
	N REMNAME
	S REMNAME=""
	I REMIEN>0 D
	.S REMNAME=$P($G(^PXD(811.9,REMIEN,0)),"^",1)
	N PTONLY ;added JHT 2020 to allow pass-through processing even when cohort evaluates false
	S PTONLY=+$P($G(^C9C(300890,TIEN,7)),"^",1)
	S C9CRET=REMIEN_"^"_REMNAME_"^"_PTONLY
	Q
SETCFT(C9CRET,VPTR,COHIEN,PTONLY)	;set cohort for title - RPC=C9C SET COHORT FOR TITLE
	;PTONLY added JHT 2020 to allow pass-through processing even when cohort evaluates false
	S C9CRET=0
	Q:+$G(VPTR)'>0
	N FPTD
	S FPTD=$P(VPTR,";",2)
	I ((FPTD'="TIU(8925.1,")&(FPTD'="AUTTIMM(,")) Q ;not a tiu document def or immunization
	N JFDA,JMSG,JFIEN
	S JFDA(42,300890,"?+1,",.01)=VPTR
	I $G(COHIEN)>0 D
	.S JFDA(42,300890,"?+1,",6)=+COHIEN
	E  D
	.S JFDA(42,300890,"?+1,",6)="@"
	S JFDA(42,300890,"?+1,",7)=+$G(PTONLY) 
	D UPDATE^DIE(,"JFDA(42)","JFIEN","JMSG")
	I '$D(JMSG) S C9CRET=1
	Q
