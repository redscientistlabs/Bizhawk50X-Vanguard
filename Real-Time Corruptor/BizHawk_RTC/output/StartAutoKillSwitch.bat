@echo off

taskkill /F /IM AutoKillSwitch.exe > nul
taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM libsneshawk-32-compatibility.exe > nul
taskkill /F /IM libsneshawk-32-performance.exe > nul
taskkill /F /IM libsneshawk-64-compatibility.exe > nul
taskkill /F /IM libsneshawk-64-performance.exe > nul
taskkill /F /IM ffmpeg.exe > nul

start AutoKillSwitch.exe
start EmuHawk.exe
exit