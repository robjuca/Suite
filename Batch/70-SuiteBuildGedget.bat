

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
	title rebuild 7-Suite Launcher at D:\Documents\GitHub\Source\Repository\WPF\Suite\Gadget
	echo  -- SUITE GADGET
	
	rem "do not change this order"
	
	echo --- DOCUMENT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Gadget\Document\Suite Gadget Document.sln" -t:rebuild -verbosity:minimal -nologo
	
    GOTO:rebuild_2

:rebuild_2
	    GOTO:eof
	





