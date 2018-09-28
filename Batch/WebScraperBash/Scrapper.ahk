
SetTitleMatchMode, 2
URL := "https://www.frontfundr.com/"
Title := FrontFundr
SearchId := 125
WaitBetweenClick :=200
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