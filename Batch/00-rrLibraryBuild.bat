@echo off

	echo ---- DELETING...
	del "D:\Documents\GitHub\Source\Repository\WPF\Suite\Bin\rr*.dll" 
	
	echo ---- COPYING...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr*.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Suite\Bin" /V
	
	pause