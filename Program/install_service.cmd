@ECHO OFF
PUSHD "%~dp0"

echo This script should be run with administrator privileges.
echo Right click - run as administrator.
echo Press any key if you're running it as administrator.
pause
sc stop "UnbanPluginsCN"
sc delete "UnbanPluginsCN"
sc create "UnbanPluginsCN" binPath= "\"%CD%\UnbanPluginsCN.exe\"" start= "auto"
sc description "UnbanPluginsCN" "UnbanPluginsCN service"
sc start "UnbanPluginsCN"

POPD

pause