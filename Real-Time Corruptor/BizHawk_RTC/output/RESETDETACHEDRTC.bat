@echo off

taskkill /F /IM WerFault.exe >nul
taskkill /F /IM EmuHawk.exe > nul
taskkill /F /IM libsneshawk-32-compatibility.exe > nul
taskkill /F /IM libsneshawk-32-performance.exe > nul
taskkill /F /IM libsneshawk-64-compatibility.exe > nul
taskkill /F /IM libsneshawk-64-performance.exe > nul
taskkill /F /IM ffmpeg.exe > nul

del config.ini

cd RTC

	cd TEMP
	del *.* /F /Q
	cd..
	
	cd TEMP2
	del *.* /F /Q
	cd..
	
	cd TEMP3
	del *.* /F /Q
	cd..
		
	cd TEMP4
	del *.* /F /Q
	cd..
	
	cd PARAMS
	del *.* /F /Q
	cd..

cd..

start EmuHawk.exe -REMOTERTC
exit