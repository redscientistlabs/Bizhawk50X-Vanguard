@echo off

taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul

start EmuHawk.exe -REMOTERTC
exit