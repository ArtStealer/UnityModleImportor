rem 发布工具
@echo off
echo 启动 Unity.exe 请稍后...
echo 清理缓存
::set path = "D:/workspace/UnityEditor/Test/Export"
rd /s /q "D:/Work/AutoRigModel/AssetBundle"
echo 新建目录
md "D:/Work/AutoRigModel/AssetBundle"
echo 开始操作
"D:\Program Files\Unity\Editor\Unity.exe" -quit -logFile log.txt -batchmode -projectPath D:\workspace\UnityEditor -executeMethod MyClass.QuicklyBuild
echo 操作完成
