@echo off

echo ****************************************
echo *            PROXY FIREFOX             *
echo ****************************************

set chemin_firefox="C:\Users\franc\AppData\Local\Mozilla Firefox\firefox.exe"
set port=3129
set host=140.227.69.20
:SUITE

REM // Affichage des choix
echo.
echo 1 - Firefox sans proxy
echo 2 - Firefox avec configuration proxy %host%:%port%
echo 3 - Just launch firefox
echo.

REM // Declaration variable choix
set choix=

REM // Affichage a l'ecran avec saisie
set /p choix=Choisir une option svp: 

REM // Tests
if /I %choix% EQU 1 (goto :NOPROXY) 
if /I %choix% EQU 2 (goto :CONFIG1) 
if /I %choix% EQU 3 (goto :FIN) 

:NOPROXY 

REm user_pref("network.proxy.type", 0);

cd /D "%APPDATA%\Mozilla\Firefox\Profiles"
cd *.default
set ffile=%cd%
type "%ffile%\prefs.js" | findstr /v "user_pref("network.proxy.type", 1);" >"%ffile%\prefs_.js"
rename "%ffile%\prefs.js" "prefs__.js"
rename "%ffile%\prefs_.js" "prefs.js"
del "%ffile%\prefs__.js"
set ffile=
cd %windir%
goto :FIN

:CONFIG1

REM user_pref("network.proxy.ftp", "140.227.69.20");
REM user_pref("network.proxy.ftp_port", 3129);
REM user_pref("network.proxy.http", "140.227.69.20");
REM user_pref("network.proxy.http_port", 3129);
REM user_pref("network.proxy.share_proxy_settings", true);
REM user_pref("network.proxy.socks", "140.227.69.20");
REM user_pref("network.proxy.socks_port", 3129);
REM user_pref("network.proxy.ssl", "140.227.69.20");
REM user_pref("network.proxy.ssl_port", 3129);
REM user_pref("network.proxy.type", 1);

REM cd /D "%APPDATA%\Mozilla\Firefox\Profiles"
REm C:\Users\franc\AppData\Roaming\Mozilla\Firefox\Profiles\mwpd1npn.default
echo "%APPDATA%\Mozilla\Firefox\Profiles"
cd *.default

set ffile=%cd%

echo %ffile%


echo user_pref("network.proxy.http", %host%);>>"%ffile%\prefs.js"
echo user_pref("network.proxy.http_port", %port%);>>"%ffile%\prefs.js"
echo user_pref("network.proxy.type", 1);>>"%ffile%\prefs.js"
set ffile=
cd %windir%
goto :FIN

:FIN
REM // Lance Firefox - Les guillemets avant la variable sont indispensables car dans le cas contraire la console reste ouverte
start "" %chemin_firefox% "https://staging.frontfundr.com/experiment/checkip"
pause

