@echo off

taskkill /F /IM EmuHawk.exe > nul 2>&1
taskkill /F /IM WerFault.exe > nul 2>&1
taskkill /F /IM ffmpeg.exe > nul 2>&1

del config.ini /F
ren backup_config.ini config.ini

start EmuHawk.exe -REMOTERTC
exit