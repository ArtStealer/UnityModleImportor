https://blog.csdn.net/qq_29579137/article/details/76598929

rem �ҵĴ������
@echo off
echo ���� Unity.exe ���Ժ�...
echo ������
rd /s /q "D:\UnityProjects_4.6\HYProjects\HYPTProject\Assets\Resources\Models"
echo �½�Ŀ¼
md "D:\UnityProjects_4.6\HYProjects\HYPTProject\Assets\Resources\Models"
echo ��ʼ����
start /min D:\Unity\Editor\Unity.exe -batchmode -projectPath D:\MyProject\BatchProject -executeMethod MyClass.QuicklyBuild
echo �������,��������˳�...
Pause
taskkill /f /im unity.exe