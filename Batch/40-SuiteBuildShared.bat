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
	title rebuild 4-Suite Server Models at D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared
	echo SUITE SHARED
	 
	rem "do not change this order"

	echo --- RESOURCE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Resources\Suite Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- TYPES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Types\Suite Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- MESSAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Message\Suite Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- VIEWMODEL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\ViewModel\Suite Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- DASHBOARD . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\DashBoard\Suite Shared DashBoard.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Services\Suite Shared Services.sln" -t:rebuild -verbosity:minimal -nologo
	
	   
	
	echo --- DOCUMENT . . .
	rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Document\Suite18 Shared Module Document.sln" /t:rebuild /verbosity:minimal /nologo 
	
	echo --- IMAGE . . .
	rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Image\Suite18 Shared Module Image.sln" /t:rebuild /verbosity:minimal /nologo 
		
	echo --- BAG . . .
    rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Bag\Suite18 Shared Module Bag.sln" /t:rebuild /verbosity:minimal /nologo 
	
	echo --- SHELF . . .
	rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Shelf\Suite18 Shared Module Shelf.sln" /t:rebuild /verbosity:minimal /nologo 
		
	echo --- DRAWER . . .
	rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Drawer\Suite18 Shared Module Drawer.sln" /t:rebuild /verbosity:minimal /nologo 
	
	echo --- CHEST . . .
	rem msbuild.exe "D:\Documents\Visual Studio 2017\Projects\WPF\2018\Suite18\Shared\Chest\Suite18 Shared Module Chest.sln" /t:rebuild /verbosity:minimal /nologo 
		
		
	
	GOTO:rebuild_2

:rebuild_2

	rem start "D:\Documents\GitHub\Source\Repository\WPF\Suite\Batch\5-SuiteBuildLauncher.bat"	
	
pause    



