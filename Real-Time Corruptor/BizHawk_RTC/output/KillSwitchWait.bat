@echo off
:start

echo.
echo  ----------------
echo   RTC KILL SWITCH
echo  ----------------
echo.
echo  Press any key to activate
pause > nul

taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul 2>&1

goto start
exit