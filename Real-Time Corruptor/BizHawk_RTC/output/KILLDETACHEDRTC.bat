@echo off

taskkill /F /IM WerFault.exe >nul
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM ffmpeg.exe > nul

exit