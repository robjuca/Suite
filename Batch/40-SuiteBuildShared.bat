@echo off

IF EXIST D:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe (
    CALL :rebuild "D:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\"
    GOTO:eof
)


CALL :error "Could not find Visual Studio directory."


:error
    echo --- rebuild FAILED: %1
    GOTO:eof

:rebuild
    chdir /d D:\
    chdir %1
    GOTO:rebuild_1



:rebuild_1
	title rebuild 4-Suite Shared at D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared
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
	
	echo --- COMMUNICATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Communication\Suite Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- DASHBOARD . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\DashBoard\Suite Shared DashBoard.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Services\Suite Shared Services.sln" -t:rebuild -verbosity:minimal -nologo
	
	   
	echo SUITE SHARED GADGET
	echo --- DOCUMENT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Document\Suite Shared Gadget Document.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- IMAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Image\Suite Shared Gadget Image.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo SUITE SHARED LAYOUT
	echo --- BAG . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Bag\Suite Shared Layout Bag.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SHELF . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Shelf\Suite Shared Layout Shelf.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- DRAWER . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Drawer\Suite Shared Layout Drawer.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- CHEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Chest\Suite Shared Layout Chest.sln" -t:rebuild -verbosity:minimal -nologo
		
		
	
	GOTO:rebuild_2

:rebuild_2

	rem start "D:\Documents\GitHub\Source\Repository\WPF\Suite\Batch\5-SuiteBuildLauncher.bat"	
	
pause    



