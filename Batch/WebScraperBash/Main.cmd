echo OFF
echo %date%-%time% *** START ***
set WID=C:\webdrivers\xdotool.exe search "FrontFundr"
C:\webdrivers\xdotool.exe windowactivate %WID2%
C:\webdrivers\xdotool.exe key "ctrl+s" type "save"
echo %date%-%time% *** END *** 
pause