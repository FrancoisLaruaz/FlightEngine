echo OFF
REm  C:\DEV\FlightEngine\Batch\WebScraperBash\Main.cmd    "C:\DEV\FlightEngine\Batch\WebScraperBash\Scrapper.ahk" "https://www.edreams.com/#results/type=R;dep=2018-10-22;from=YVR;to=LON;ret=2018-11-19;collectionmethod=false;airlinescodes=false;internalSearch=true" "126" "C:\Program Files\Mozilla Firefox\firefox.exe" "eDreams" "" ""

echo %date%-%time% *** START ***

REM PARAMETERS
SET AutoHotKeyFile=%1
SET Url=%2
SET SearchId=%3
SET BrowserExe=%4
SET Provider=%5
SET ProxyPort=%6
SET ProxyHost=%7

REM delete former files or process
DEL D:\Html\search_%SearchId%.html
rmdir /s /q "D:\Html\search_%SearchId%_files"
rmdir  D:\Html\search_%SearchId%_files
taskkill /IM firefox.exe
if /I %ProxyPort% EQU "" (goto :EXECUTE) 

REM BEGIN CHANGE PROXY
REM https://stackoverflow.com/questions/11245144/replace-whole-line-containing-a-string-using-sed
set PrefFilePath="C:\Users\franc\AppData\Roaming\Mozilla\Firefox\Profiles\b8n5foan.default"\pref.js;
change_line "user_pref("network.proxy.http" "This line is removed by the admin." yourFile

start "" %BrowserExe% "https://staging.frontfundr.com/experiment/checkip"
goto :EXECUTE
REM END CHANGE PROXY


:EXECUTE
rem "C:\Program Files\AutoHotkey\AutoHotkey.exe" /ErrorStdOut  %AutoHotKeyFile% %Url% %SearchId% %BrowserExe% %Provider%

REM kill process
taskkill /IM AutoHotkey.exe
taskkill /IM firefox.exe
rmdir /s /q "D:\Html\search_%SearchId%_files"
rmdir  D:\Html\search_%SearchId%_files
echo %date%-%time% *** END *** 
pause

function escape_slashes {
    sed 's/\//\\\//g' 
}

function change_line {
    local OLD_LINE_PATTERN=$1; shift
    local NEW_LINE=$1; shift
    local FILE=$1

    local NEW=$(echo "${NEW_LINE}" | escape_slashes)
    sed -i .bak '/'"${OLD_LINE_PATTERN}"'/s/.*/'"${NEW}"'/' "${FILE}"
    mv "${FILE}.bak" /tmp/
}