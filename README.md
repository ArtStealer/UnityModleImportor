https://blog.csdn.net/qq_29579137/article/details/76598929

rem 我的打包工具
@echo off
echo 启动 Unity.exe 请稍后...
echo 清理缓存
rd /s /q "D:\UnityProjects_4.6\HYProjects\HYPTProject\Assets\Resources\Models"
echo 新建目录
md "D:\UnityProjects_4.6\HYProjects\HYPTProject\Assets\Resources\Models"
echo 开始操作
start /min D:\Unity\Editor\Unity.exe -batchmode -projectPath D:\MyProject\BatchProject -executeMethod MyClass.QuicklyBuild
echo 操作完成,按任意键退出...
Pause
taskkill /f /im unity.exe
