rem ��������
@echo off
echo ���� Unity.exe ���Ժ�...
echo ������
::set path = "D:/workspace/UnityEditor/Test/Export"
rd /s /q "D:/workspace/UnityEditor/Test/Export"
echo �½�Ŀ¼
md "D:/workspace/UnityEditor/Test/Export"
echo ��ʼ����
"D:\Program Files\Unity\Editor\Unity.exe" -quit -logFile log.txt -batchmode -projectPath D:\workspace\UnityEditor -executeMethod MyClass.QuicklyBuild
Pause
#taskkill /f /im unity.exe


::start /min "D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -serial SB-XXXX-XXXX-XXXX-XXXX-XXXX -username "JoeBloggs@example.com" -password "MyPassw0rd"
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -serial F4-QFUH-NY4U-G6FD-NEGP-XXXX -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -logFile log.txt -batchmode -serial U3-0Q3U-DDAV-1AQM-F6E7-XXXX -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2