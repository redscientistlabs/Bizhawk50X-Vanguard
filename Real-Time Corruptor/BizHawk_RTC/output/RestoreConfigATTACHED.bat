@echo off

taskkill /F /IM StandaloneRTC.exe >nul
taskkill /F /IM WerFault.exe >nul
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM libsneshawk-32-compatibility.exe > nul
taskkill /F /IM libsneshawk-32-performance.exe > nul
taskkill /F /IM libsneshawk-64-compatibility.exe > nul
taskkill /F /IM libsneshawk-64-performance.exe > nul
taskkill /F /IM ffmpeg.exe > nul

del config.ini /F
ren backup_config.ini config.ini

start EmuHawk.exe
exit