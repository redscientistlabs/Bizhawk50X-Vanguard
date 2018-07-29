@echo off
:start

echo.
echo  ----------------
echo   RTC KILL SWITCH
echo  ----------------
echo.
echo  Press any key to restart RTC.
pause > nul

taskkill /F /IM StandaloneRTC.exe >nul
taskkill /F /IM WerFault.exe >nul
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM libsneshawk-32-compatibility.exe > nul
taskkill /F /IM libsneshawk-32-performance.exe > nul
taskkill /F /IM libsneshawk-64-compatibility.exe > nul
taskkill /F /IM libsneshawk-64-performance.exe > nul
taskkill /F /IM ffmpeg.exe > nul

start EmuHawk.exe
goto start
exit