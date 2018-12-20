@echo off

echo.
echo    -=-=-=-=-=-=-=[RTC Factory Cleaner]=-=-=-=-=-=-=-
echo.
echo    This will delete all RTC save data, 
echo    BizHawk config, emulator SaveStates and SaveRAM
echo.
echo    BizHawk will restart after the script has finished.
echo.
echo    To abort this procedure, Close the window.
echo.
echo.
echo.
echo.
Pause

rem !!!!!!!!!!!!!!!!!!!!!!!!!!!
rem DO NOT EDIT THIS BATCHFILE
rem !!!!!!!!!!!!!!!!!!!!!!!!!!!

cls
taskkill /F /IM "EmuHawk.exe"
taskkill /F /IM "StandaloneRTC.exe" > nul

del config.ini /F
del backup_config.ini /F
del stockpile_config.ini /F
del CorruptedROM.rom /F
del VinesauceROMCorruptor.txt /F

del RTC/MEMORYDUMPS/*.* /F /Q
del RTC/RENDEROUTPUT/*.* /F /Q
del RTC/WORKING/*.* /F /Q /S
del RTC/PARAMS/*.* /F /Q
	
del WGH/PARAMS/*.* /F /Q

echo.
echo.
