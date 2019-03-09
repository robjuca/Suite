

@echo off

IF EXIST C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\msbuild.exe (
    CALL :rebuild "C:\Program Files (x86)\Microsoft Visual Studio\2019\Preview\MSBuild\Current\Bin\"
    GOTO:eof
)


CALL :error "Could not find Visual Studio directory."


:error
    echo --- rebuild FAILED: %1
    GOTO:eof

:rebuild
    chdir /d C:\
    chdir %1
    GOTO:rebuild_1

:rebuild_1
	title rebuild 8-Suite Layout at D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout
	echo  -- SUITE LAYOUT
	
	rem "do not change this order"
	
	echo --- BAG . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Bag\Suite Layout Bag.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SHELF . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Shelf\Suite Layout Shelf.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- DRAWER . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Drawer\Suite Layout Drawer.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- CHEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Chest\Suite Layout Chest.sln" -t:rebuild -verbosity:minimal -nologo
	
    GOTO:rebuild_2

:rebuild_2
	    pause
	





