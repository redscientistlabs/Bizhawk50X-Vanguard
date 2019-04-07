@echo off

taskkill /F /IM AutoKillSwitch.exe > nul 2>&1
taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul 2>&1

start AutoKillSwitch.exe
start EmuHawk.exe
exit