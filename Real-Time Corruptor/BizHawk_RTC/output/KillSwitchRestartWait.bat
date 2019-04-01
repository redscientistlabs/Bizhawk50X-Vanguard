@echo off
:start

echo.
echo  ----------------
echo   RTC KILL SWITCH
echo  ----------------
echo.
echo  Press any key to restart RTC.
pause > nul

taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul

start EmuHawk.exe
goto start
exit