@echo off
 rem �����ļ�
 for /f "delims=" %%i in ('dir /b ".\*.proto"') do echo %%i
 for /f "delims=" %%i in ('dir /b/a "*.proto"') do protogen -i:%%i -o:%%~ni.cs
 pause