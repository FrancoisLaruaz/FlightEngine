
; "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.frontfundr.com/","126"


if A_Args.Length() < 2
{
    ExitApp
}


SetTitleMatchMode, 2
URL := A_Args[1]
Title := FrontFundr
SearchId := A_Args[2]
WaitBetweenClick :=300
PathToSaveFile := "D:\Html"

Run "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" -new-window %URL%
WinWaitActive, FrontFundr
Sleep,1000
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
WinClose
; AutoHotKey
; https://autohotkey.com/board/topic/68989-command-line-switchparameter/
return