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
	title rebuild 3-Suite Server Models at D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Services
	echo SUITE SERVER SERVICES
	 
	rem "do not change this order"

	
	echo --- COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Suite\Server\Services\Component\Suite Server Services Component.sln" -t:rebuild -verbosity:minimal -nologo
	

	
    GOTO:rebuild_2

:rebuild_2
	rem start "D:\Documents\GitHub\Source\Repository\WPF\Suite\Batch\4-SuiteBuildShared.bat"
	
pause    
