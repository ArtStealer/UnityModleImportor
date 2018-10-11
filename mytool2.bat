rem 发布工具
@echo off
echo 启动 Unity.exe 请稍后...
echo 清理缓存
::set path = "D:/workspace/UnityEditor/Test/Export"
rd /s /q "D:/workspace/UnityEditor/Test/Export"
echo 新建目录
md "D:/workspace/UnityEditor/Test/Export"
echo 开始操作
"D:\Program Files\Unity\Editor\Unity.exe" -quit -logFile log.txt -batchmode -projectPath D:\workspace\UnityEditor -executeMethod MyClass.QuicklyBuild
Pause
#taskkill /f /im unity.exe


::start /min "D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -serial SB-XXXX-XXXX-XXXX-XXXX-XXXX -username "JoeBloggs@example.com" -password "MyPassw0rd"
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -serial F4-QFUH-NY4U-G6FD-NEGP-XXXX -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2
::"D:\Program Files\Unity\Editor\Unity.exe" -quit -logFile log.txt -batchmode -serial U3-0Q3U-DDAV-1AQM-F6E7-XXXX -username "jiangjunwei@chishine3d.com" -password "Chishine3d" -projectPath D:\workspace\UnityEditor -executeMethod MyTools.MyFunc2