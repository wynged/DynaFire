del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\DynaFire_ViewExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\DynaFire_bin\"

xcopy /y /q "%~dp0DynaFire_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions"
xcopy /s /q "%~dp0DynaFire_bin" "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\DynaFire_bin\"

rem xcopy /y /q "%~dp0shortcuts.txt" "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\DynaFire_bin\"
xcopy /y /q "%~dp0shortcuts.txt" "%appdata%\DynaFire\"

pause