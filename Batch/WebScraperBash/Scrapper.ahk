
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe"

try  ; Attempts to execute code.
{
	if A_Args.Length() < 3
	{
		ExitApp
	}


	SetTitleMatchMode, 2
	URL := A_Args[1]
	Title := "Edreams"
	SearchId := A_Args[2]
	WaitBetweenClick :=300
	PathToSaveFile := "D:\Html"
	BrowserExe := A_Args[3]

	Run %BrowserExe% -new-window %URL%
	WinWaitActive, "Your travel agency: Book cheap flights - eDreams International"
	WinMaximize
	Sleep,10000
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
	Sleep,%WaitBetweenClick%
}
catch e  ; Handles the first error/exception raised by the block above.
{
    MsgBox, An exception was thrown!`nSpecifically: %e%
}	
WinClose
return