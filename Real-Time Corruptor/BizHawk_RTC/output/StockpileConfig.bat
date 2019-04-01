@echo off

taskkill /F /IM StandaloneRTC.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM ffmpeg.exe > nul

del config.ini /F
ren stockpile_config.ini config.ini

start EmuHawk.exe -REMOTERTC
start StandaloneRTC.exe
exit