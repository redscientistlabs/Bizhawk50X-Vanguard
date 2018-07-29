@echo off
FactoryClean.bat

echo Press any key to reload RTC.
Pause > NUL
start EmuHawk.exe
start StandaloneRTC.exe