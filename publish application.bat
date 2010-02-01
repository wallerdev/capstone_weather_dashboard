@rem BATCH FILE
@echo off
@rem * Copy from source to destination including subdirs and hidden
@rem * File
xcopy "C:\Documents and Settings\Administrator\My Documents\Visual Studio 2008\Projects\CapstoneWeatherDashboard\CapstoneWeatherDashboard\*" "C:\Inetpub\cse498websiteroot\demo" /S /E /H /Y
:endofprogram