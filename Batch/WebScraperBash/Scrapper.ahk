
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams"
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "C:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Program Files\Mozilla Firefox\firefox.exe" "eDreams"

; "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.exe" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "Edreams"
; "C:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.exe" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "16" "C:\Program Files\Mozilla Firefox\firefox.exe" "Edreams"

try  ; Attempts to execute code.
{

	if A_Args.Length() < 4
	{
		URL := "https://www.kayak.com/flights/YVR-HNL/2019-03-01/2019-03-06?sort=bestflight_a"
		SearchId := "126"
		;BrowserExe := "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe"
		BrowserExe := "C:\Program Files\Mozilla Firefox\firefox.exe"
		Provider := "Kayak"
	}
	else
	{
		URL := A_Args[1]
		SearchId := A_Args[2]
		BrowserExe := A_Args[3]
		Provider := A_Args[4]
	}

	SetTitleMatchMode, 2

	ShortWaitBetweenClick :=200
	WaitBetweenClick :=1600
	LongWaitBetweenClick :=6000

	WindowsFound:="KO"

	Run %BrowserExe% -new-window %URL%

	if(Provider="FrontFundr")
	{
		PageLoadTime :=3500
		if WinExist("FrontFundr") {
			WinWaitActive, FrontFundr
			WindowsFound:="OK"
		}
	}
	if(Provider="Edreams")
	{
		Sleep,17000
		IfWinExist, eDreams
		{
			WindowsFound:="OK"
		}			
	
		PageLoadTime :=25000
	}
	
	if(Provider="Kayak")
	{
		Sleep,18000
		IfWinExist, to 
		{
			WindowsFound:="OK"
		}			
	
		PageLoadTime :=15000
		LongWaitBetweenClick :=5000
	}	

	if(WindowsFound = "OK")
	{
		Sleep,%PageLoadTime%
		MouseClick, right, 900, 500
		Sleep,%WaitBetweenClick%
		MouseClick, left, 940,560
		;Sleep,%WaitBetweenClick%
		;MouseClick, left, 252, 80
		Sleep,%WaitBetweenClick%
		Send search_%SearchId%
		Sleep,2200
		SendInput {enter}
		Sleep,%LongWaitBetweenClick%
	}
	else{
		SetWorkingDir, D:\Html
		FileAppend % "INCORRECT PROXY"
			,stopsearch_%SearchId%.txt
	}

}
catch e  ; Handles the first error/exception raised by the block above.
{
	SetWorkingDir, D:\Html\Logs
	FormatTime, now,, yyyy/M/d HH:mm:ss
	FileAppend %now%
        ,log_%SearchId%.txt
	FileAppend % " : Error on line " e.Line " & File = " e.File " & What = " e.What " & Extra = " e.Extra " & Message = " e.Message "`n"
        ,log_%SearchId%.txt
	SetWorkingDir, D:\Html
	FileAppend % "ERROR"
			,stopsearch_%SearchId%.txt		
}
WinClose
ExitApp
return