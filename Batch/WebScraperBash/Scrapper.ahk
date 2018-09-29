
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe"
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "C:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Program Files\Mozilla Firefox\firefox.exe"

try  ; Attempts to execute code.
{

	if A_Args.Length() < 3
	{
		URL := "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true"
		SearchId := "126"
		;BrowserExe := "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe"
		BrowserExe := "C:\Program Files\Mozilla Firefox\firefox.exe"
	}
	else
	{
		URL := A_Args[1]
		SearchId := A_Args[2]
		BrowserExe := A_Args[3]
	}
	SetTitleMatchMode, 2
	Title := "Edreams"
	WaitBetweenClick :=300
	PathToSaveFile := "D:\Html"
	SetWorkingDir, D:\Html\Logs
	

	Run %BrowserExe% -new-window %URL%
	WinWaitActive, eDreams
	WinMaximize
	Sleep,22000
	MouseClick, right, 200, 300
	Sleep,%WaitBetweenClick%
	MouseClick, left, 252, 352
	Sleep,%WaitBetweenClick%
	MouseClick, left, 400, 50
	sleep,%WaitBetweenClick%
	Send %PathToSaveFile%
	Sleep,%WaitBetweenClick%
	SendInput {enter}
	sleep,%WaitBetweenClick%
	MouseClick, left, 252, 465
	Sleep,%WaitBetweenClick%
	Send search_%SearchId%
	Sleep,100
	SendInput {enter}
	Sleep,1000
}
catch e  ; Handles the first error/exception raised by the block above.
{
	FormatTime, now,, yyyy/M/d HH:mm:ss
	FileAppend %now%
        ,log_%SearchId%.txt
	FileAppend % " : Error on line " e.Line " & File = " e.File " & What = " e.What " & Extra = " e.Extra " & Message = " e.Message "`n"
        ,log_%SearchId%.txt
}	
WinClose
ExitApp 
return