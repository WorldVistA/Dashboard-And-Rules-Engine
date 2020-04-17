# Introduction to the VistA Clinical Dashboard
This is software that consists of a dashboard container, rules engine software
for some of the dashboard modules, as well as RPCs and mumps routines to access
VistA information for dashboard display. The dashboard was designed to provide
list-centric, real time information from VistA to the user working directly
with the patients. It can also be used by other key staff that need timely
clinical metrics.

 1. The design is modular and extensible
 2. Programmers can write modules
 3. Some modules can be configured by Clinical Applications Coordinators using
    the included Rules Engine and/or the Reminders Package in VistA
 4. End users do not need to access character-based server side of VistA.
    Instead they choose the same lists they use in CPRS.
 5. Windows-based GUI.

# Download the Install
[Download zip here](https://github.com/WorldVistA/Dashboard-And-Rules-Engine/releases/download/1.0.0.5/GUIs-dlls-KIDs.zip).

To install, load `Dashboard and Rules Engine Open Source Release.KID` into your
VistA system, and put the exes on a Windows Machine. No installation is necessary.

The exes need to find `Medsphere.OpenVista.Remoting.dll` and
`Medsphere.OpenVista.Shared.dll` on your %PATH% or in the same directory as
your exes.

There are two exes:

 * `Dashboard.exe`
 * `RulesEngine.exe`

To run them, invoke them with the `s=` and `p=` arguments used for CPRS.
