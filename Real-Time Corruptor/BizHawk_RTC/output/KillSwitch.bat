@echo off

taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul

exit