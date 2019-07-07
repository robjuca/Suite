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
	title rebuild Suite 
	echo SUITE 
	 
	rem "do not change this order"

    echo SUITE SERVER MODELS 
	echo --- SUITE SERVER MODELS INFRASTRUCTURE . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Models\Infrastructure\Suite Server Models Infrastructure.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SERVER MODELS COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Models\Component\Suite Server Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	

	echo SUITE SERVER CONTEXT
	echo --- SUITE SERVER CONTEXT COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Context\Component\Suite Server Context Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo SUITE SHARED
	echo --- SUITE SHARED RESOURCE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Resources\Suite Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- SUITE SHARED TYPES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Types\Suite Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED MESSAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Message\Suite Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- SUITE SHARED VIEWMODEL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\ViewModel\Suite Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- COMMUNICATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Communication\Suite Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED DASHBOARD . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\DashBoard\Suite Shared DashBoard.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Services\Suite Shared Services.sln" -t:rebuild -verbosity:minimal -nologo
	   
	echo SUITE SHARED GADGET
	echo --- SUITE SHARED GADGET DOCUMENT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Document\Suite Shared Gadget Document.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED GADGET IMAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Image\Suite Shared Gadget Image.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo SUITE SHARED LAYOUT
	echo --- SUITE SHARED LAYOUT BAG . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Bag\Suite Shared Layout Bag.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED LAYOUT SHELF . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Shelf\Suite Shared Layout Shelf.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- SUITE SHARED LAYOUT DRAWER . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Drawer\Suite Shared Layout Drawer.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE SHARED LAYOUT CHEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Shared\Chest\Suite Shared Layout Chest.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo SUITE SERVER SERVICES
	echo --- SUITE SERVER SERVICES COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Services\Component\Suite Server Services Component.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- SUITE LAUNCHER
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Launcher\Suite Launcher.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- SUITE MODULES
	echo --- SUITE MODULES SETTINGS . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Module\Settings\Suite Module Settings.sln" -t:rebuild -verbosity:minimal -nologo

	
	echo  -- SUITE GADGET
	echo --- SUITE GADGET DOCUMENT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Gadget\Document\Suite Gadget Document.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE GADGET IMAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Gadget\Image\Suite Gadget Image.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- SUITE LAYOUT
	echo --- SUITE LAYOUT BAG . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Bag\Suite Layout Bag.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE LAYOUT SHELF . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Shelf\Suite Layout Shelf.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE LAYOUT DRAWER . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Drawer\Suite Layout Drawer.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SUITE LAYOUT CHEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Layout\Chest\Suite Layout Chest.sln" -t:rebuild -verbosity:minimal -nologo
	

	echo --- SUITE BUILD DONE
	
pause
