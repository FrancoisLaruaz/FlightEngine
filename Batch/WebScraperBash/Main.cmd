echo OFF
REm  C:\DEV\FlightEngine\Batch\WebScraperBash\Main.cmd    "C:\DEV\FlightEngine\Batch\WebScraperBash" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Program Files\Mozilla Firefox\firefox.exe" "eDreams" "" ""
REm  D:\DEV\FlightEngine\Batch\WebScraperBash\Main.cmd    "D:\DEV\FlightEngine\Batch\WebScraperBash" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams" "37.191.78.2" 52307
REm  D:\DEV\FlightEngine\Batch\WebScraperBash\Main.cmd    "D:\DEV\FlightEngine\Batch\WebScraperBash" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams" "193.71.255.234" 3128


echo %date%-%time% *** START ***

REM PARAMETERS
SET Repo=%1
SET Url=%2
SET SearchId=%3
SET BrowserExe=%4
SET Provider=%5
SET ProxyPort=%7
SET ProxyHost=%6

Set AutoHotKeyFile=%1\Scrapper.exe

REM delete former files or process
DEL D:\Html\search_%SearchId%.xht
DEL D:\Html\search_%SearchId%.html
rmdir /s /q "D:\Html\search_%SearchId%_files"
rmdir  D:\Html\search_%SearchId%_files
taskkill /IM firefox.exe
taskkill /IM Scrapper.exe
taskkill /IM AutoHotkey.exe

if /I %ProxyPort% EQU "" (goto :EXECUTE) 

REM BEGIN CHANGE PROXY
rem https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list.txt
cd /D "C:\Users\franc\AppData\Roaming\Mozilla\Firefox\Profiles"
cd *.default
set RepoPrefJs=%cd%
set ffile=%RepoPrefJs%\prefs.js
echo %ffile%


del %RepoPrefJs%\temporary\prefs.js
del "%RepoPrefJs%\temporary\prefs_template.js"
rd /Q "%RepoPrefJs%\temporary"
md %RepoPrefJs%\temporary
del "%RepoPrefJs%\prefs.js"
echo f | xcopy /f /y "%Repo%\prefs_template.js" "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.ftp", %ProxyHost%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.ftp_port", %ProxyPort%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.http", %ProxyHost%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.http_port", %ProxyPort%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.socks", %ProxyHost%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.socks_port", %ProxyPort%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.ssl", %ProxyHost%); >> "%RepoPrefJs%\temporary\prefs_template.js"
echo user_pref("network.proxy.ssl_port", %ProxyPort%); >> "%RepoPrefJs%\temporary\prefs_template.js"
rename "%RepoPrefJs%\temporary\prefs_template.js" "prefs.js"
del %ffile%
echo f | xcopy /f /y "%RepoPrefJs%\temporary\prefs.js" "%RepoPrefJs%\prefs.js"
del "%RepoPrefJs%\temporary\prefs.js"
rd /Q "%RepoPrefJs%\temporary"

goto :EXECUTE
REM END CHANGE PROXY


:EXECUTE
cd /D %Repo%
"C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  "D:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe" "eDreams"

rem %AutoHotKeyFile% %Url% %SearchId% %BrowserExe% %Provider%

REM kill process
taskkill /IM Scrapper.exe
taskkill /IM firefox.exe
taskkill /IM AutoHotkey.exe

rmdir /s /q "D:\Html\search_%SearchId%_files"
rmdir  D:\Html\search_%SearchId%_files
echo %date%-%time% *** END *** 
pause

