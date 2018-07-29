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

cd RTC

	cd MEMORYDUMPS
	del *.* /F /Q
	cd..
	
	cd RENDEROUTPUT
	del *.* /F /Q
	cd..
	
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

cd WGH

	cd PARAMS
	del *.* /F /Q
	cd..

cd..

cd "Apple II"

	cd State
	del *.* /F /Q
	cd..

cd..

cd "Atari 2600"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "Atari 7800"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..


cd "Coleco"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "Game Gear"

	cd State
	del *.* /F /Q
	cd..

cd..

cd "Gameboy"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "GBA"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "Genesis"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..


cd "Intellivision"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..


cd "Libretro"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "Lynx"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "N64"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
	cd Cheats
	del *.* /F /Q
	cd..
	
cd..

cd "NES"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
	cd Screenshots
	del *.* /F /Q
	cd..
	
	cd Cheats
	del *.* /F /Q
	cd..
	
cd..

cd "PC Engine"

	cd State
	del *.* /F /Q
	cd..
	
cd..

cd "PSX"

	cd State
	del *.* /F /Q
	cd..

cd..

cd "Saturn"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "SG-1000"

	cd State
	del *.* /F /Q
	cd..

cd..

cd "SMS"

	cd State
	del *.* /F /Q
	cd..

cd..

cd "SNES"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
	cd Cheats
	del *.* /F /Q
	cd..
	
cd..

cd "VB"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

cd "WonderSwan"

	cd State
	del *.* /F /Q
	cd..
	
	cd SaveRAM
	del *.* /F /Q
	cd..
	
cd..

echo.
echo.
