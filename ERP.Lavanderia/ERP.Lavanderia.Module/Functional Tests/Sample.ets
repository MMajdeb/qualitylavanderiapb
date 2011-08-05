;#DropDB ERP.LavanderiaEasyTest

#Application ERP.LavanderiaWin
#Application ERP.LavanderiaWeb

*Action Navigation(User)

*ProcessRecord
 Full Name = ''

#IfDef ERP.LavanderiaWeb
*Action Edit
#EndIf
 
*FillForm
 Full Name = Test User
 
*Action Cancel

#IfDef ERP.LavanderiaWin
*HandleDialog
 Respond = Yes
#EndIf

*CheckFieldValues
 Full Name = ''
