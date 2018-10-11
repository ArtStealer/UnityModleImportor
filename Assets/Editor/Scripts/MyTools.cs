//using UnityEditor;
//using UnityEngine;

//public class MyTools : EditorWindow
//{
//    // 重写window面板
//    private void OnGUI()
//    {
//        if (GUILayout.Button("MyFunc1"))
//        {
//            MyFunc1();
//        }
//    }

//    // 打开窗口
//    [MenuItem("My Tools/My Window")]
//    private static void OpenMyWindow()
//    {
//        GetWindow<MyTools>(true, "My Window");
//    }

//    [MenuItem("My Tools/MyFunc1")]
//    private static void MyFunc1()
//    {
//        //..你的操作
//        Debug.Log("你的操作");
//    }

//    public static void MyFunc2()
//    {
//        //..你的操作
//        Debug.Log("你的操作");
//    }
//}
//using System.IO;

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

//using System.Collections.Generic;
//using System;
//using System.Linq;

//using UnityEngine.Windows;

//namespace UnityEditor
//{
//    public class Myssss : EditorWindow
//    {
//        public static void QuicklyBuild()
//        {
//            GameObject gameObject;
//            bool m_IsBiped = AvatarBipedMapper.IsBiped(gameObject.transform, null);
//            //// Handle human bones
//            //if (m_Bones == null)
//            //    m_Bones = AvatarSetupTool.GetHumanBones(serializedObject, modelBones);
//        }
//    }
//}
public class Utility
{
    public const string AssetBundlesOutputPath = "AssetBundles";

    public static string GetPlatformName()
    {
#if UNITY_EDITOR
        return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
			return GetPlatformForAssetBundles(Application.platform);
#endif
    }

#if UNITY_EDITOR
    private static string GetPlatformForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebGL:
                return "WebGL";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
#endif

    private static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            // Add more build targets for your own.
            // If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
            default:
                return null;
        }
    }
}

public class MyClass : EditorWindow
{
    public static bool auto = true;
    public static string pathImport = "D:/workspace/UnityEditor/Test/Import";
    public static string pathExport = "D:/workspace/UnityEditor/Test/Export";
    public static bool dealModelImport = false;



    // 重写window面板
    private void OnGUI()
    {
        if (GUILayout.Button("选择路径并导出资源文件"))
        {
            string savePath = EditorUtility.SaveFolderPanel("导出路径", "", "");
            Build(savePath);
        }
    }

    private static string _modelName = null;

    [MenuItem("My Tools/Quickly Build")]
    public static void QuicklyBuild()
    {
        Debug.Log("Import Flag Open");
        dealModelImport = true; // open flag
        string assetPath = pathImport;
        if (!auto)
        {
            GetWindow<MyClass>(true, "打包工具");
            assetPath = EditorUtility.OpenFolderPanel("选择资源路径", "", "");
        }
        
        if (!Directory.Exists(Application.dataPath + "/Resources/Models"))
            Directory.CreateDirectory(Application.dataPath + "/Resources/Models");
        Copy(assetPath, Application.dataPath + "/Resources/Models");

        //Dictionary<Transform, bool> bones = new Dictionary<Transform, bool>();

        AssetDatabase.Refresh();
        GetModelName();

        
        dealModelImport = false; // open flag
        Debug.Log("Import Flag Close");
        if (auto)
        {
            Build(pathExport);
        }
    }

    public static bool IsWorking()
    {
        return dealModelImport;
    }

    // 自动选取后缀为.fbx的模型命名
    private static void GetModelName()
    {
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Resources/Models");
        foreach (FileInfo file in info.GetFiles())
        {
            if (file.Extension.ToUpper() == ".FBX")
            {
                _modelName = file.Name.Split('.')[0]; // file.Name.Replace(".FBX", "");

                //string path = file.ToString();
                //path = path.Replace("\\", "/");
                //string assetpath = "Assets" + path.Substring(Application.dataPath.Length);
                ////ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(assetpath);
                //Avatar avatar = AssetDatabase.LoadAssetAtPath(assetpath, typeof(UnityEngine.Avatar)) as Avatar;
                //if (avatar)
                //{
                //    Debug.Log("xxx");
                //}
                //EditorUtility.SetDirty(inspector);
                //EditorUtility.SetDirty(modelImporter);


                //Debug.Log("Could not find avatar when importing : " + modelImporter.assetPath);
                //if (avatar != null && modelImporter != null)
                //modelImporter.UpdateHumanDescription(avatar, humanDescription);

                //Debug.Log(file.ToString());
                //Debug.Log(file.Directory.ToString());
                //Debug.Log(file.Name.ToString());
                //string path = file.ToString();
                //path = path.Replace("\\","/");
                //SetABName(path,"fbx");

                //string assetpath = "Assets" + path.Substring(Application.dataPath.Length);
                //AssetDatabase.ImportAsset(assetpath);
                //AssetImporter model = AssetImporter.GetAtPath(assetpath);
                //ModelImporter modelImporter = (ModelImporter)AssetImporter.GetAtPath(assetpath);
                //modelImporter.animationType = ModelImporterAnimationType.Human;
                //HumanDescription humDes = model.humanDescription;
                //Debug.Log(humDes);

                //ModelImporter modelImporter = assetImporter as ModelImporter;
                //modelImporter.animationType = ModelImporterAnimationType.Human;
                //modelImporter.animationCompression = ModelImporterAnimationCompression.Off;
                //modelImporter.animationPositionError = 0.5f;
                //modelImporter.animationRotationError = 0.5f;
                //modelImporter.animationScaleError = 0.5f;
                //modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
                //modelImporter.importAnimation = true;
                //modelImporter.resampleCurves = true;
                //modelImporter.humanDescription = ReadHumanDescription();
                //modelImporter.sourceAvatar = null;

                // if optimize game object then extraExposeTransform is need
                //modelImporter.optimizeGameObjects = true; 
                //modelImporter.extraExposedTransformPaths = new string[] { string.Format("{0}:Solving:Hips", _mocapActorId) };
                //modelImporter.motionNodeName = "<None>";

                //if (!secondPass && _isAnimation)
                //{
                //    string[] paths = AssetDatabase.FindAssets(_referenceAvatarName);
                //    string referenceAvatarPath = AssetDatabase.GUIDToAssetPath(paths[0]);
                //    Avatar avatar = AssetDatabase.LoadAssetAtPath<Avatar>(referenceAvatarPath);
                //    modelImporter.sourceAvatar = avatar;
                //}

                //GameObject model = Resources.Load<GameObject>("Models/" + _modelName);
                //CreateAvatar

                //string path = AssetDatabase.GetAssetPath(go);
                ////将路径中的模型资源导入
                //ModelImporter modelimporter = (ModelImporter)ModelImporter.GetAtPath(path);
                //ModelImporter model = (ModelImporter)ModelImporter.GetAtPath(path);
                //model.animationType = ModelImporterAnimationType.Human;

            }
        }   
    }

    // 打包资源
    public static void Build(string savePath)
    {
        Debug.Log(_modelName);
        if (_modelName == null)
        {
            return;
        }
        GameObject model = Resources.Load<GameObject>("Models/" + _modelName);
        string prefabPath = "Assets/Resources/" + model.name + ".prefab";
        Object oldPrefab = PrefabUtility.CreateEmptyPrefab(prefabPath);
        GameObject newPrefab = PrefabUtility.ReplacePrefab(model, oldPrefab);
        Caching.ClearCache();
        string modelName = newPrefab.name;
        //string assetBundlePath = savePath + "/" + modelName + ".assetbundle";
        
        //AssetImporter assetImporter2 = AssetImporter.GetAtPath("Assets/Resources/Models/2.FBX");
        
        AssetImporter assetImporter = AssetImporter.GetAtPath(prefabPath);  //得到Asset

        //string tempName = prefabPath.Substring(prefabPath.LastIndexOf(@"/") + 1);
        //string assetName = tempName.Remove(tempName.LastIndexOf(".")); //获取asset的文件名称
        string assetName = _modelName;
        assetImporter.assetBundleName = assetName;    //最终设置assetBundleName

        string outputPath = Path.Combine(savePath, Utility.GetPlatformName());
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        
        BuildTarget targetPlatform = EditorUserBuildSettings.activeBuildTarget;
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, targetPlatform);
        
        AssetDatabase.Refresh();
        //Delete();
    }

    // 清空文件夹
    public static void Delete()
    {
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Resources/Models");
        info.Delete(true);
    }

    // 文件导入
    public static void Copy(string sPath, string dPath)
    {
        DirectoryInfo info = new DirectoryInfo(sPath);
        foreach (DirectoryInfo directory in info.GetDirectories())
        {
            if (!Directory.Exists(dPath + "/" + directory.Name))
            {
                Directory.CreateDirectory(dPath + "/" + directory.Name);
            }
            foreach (FileInfo file in directory.GetFiles())
            {
                if (!File.Exists(dPath + "/" + directory.Name + "/" + file.Name))
                {
                    File.Copy(sPath + "/" + directory.Name + "/" + file.Name, dPath + "/" + directory.Name + "/" + file.Name);
                }
            }
        }
        foreach (FileInfo file in info.GetFiles())
        {
            if (!File.Exists(dPath + "/" + file.Name))
            {
                File.Copy(sPath + "/" + file.Name, dPath + "/" + file.Name);
            }
        }
    }

    /// <summary>
    /// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
    /// </summary>
    static void ClearAssetBundlesName()
    {
        //获取所有的AssetBundle名称
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();

        //强制删除所有AssetBundle名称
        for (int i = 0; i < abNames.Length; i++)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }
    }

    /// <summary>
    /// 设置所有在指定路径下的AssetBundleName
    /// </summary>
    static void SetAssetBundlesName(string _assetsPath)
    {
        //先获取指定路径下的所有Asset，包括子文件夹下的资源
        DirectoryInfo dir = new DirectoryInfo(_assetsPath);
        FileSystemInfo[] files = dir.GetFileSystemInfos(); //GetFileSystemInfos方法可以获取到指定目录下的所有文件以及子文件夹

        for (int i = 0; i < files.Length; i++)
        {
            if (files[i] is DirectoryInfo)  //如果是文件夹则递归处理
            {
                SetAssetBundlesName(files[i].FullName);
            }
            else if (!files[i].Name.EndsWith(".meta")) //如果是文件的话，则设置AssetBundleName，并排除掉.meta文件
            {
                SetABName(files[i].FullName);     //逐个设置AssetBundleName
            }
        }
    }

    /// <summary>
    /// 设置单个AssetBundle的Name
    /// </summary>
    /// <param name="filePath"></param>
    static void SetABName(string assetPath, string appendName = "")
    {
        string importerPath = "Assets" + assetPath.Substring(Application.dataPath.Length);  //这个路径必须是以Assets开始的路径
        AssetImporter assetImporter = AssetImporter.GetAtPath(importerPath);  //得到Asset
        if (assetImporter)
        {
            string tempName = assetPath.Substring(assetPath.LastIndexOf(@"/") + 1);
            string assetName = tempName.Remove(tempName.LastIndexOf(".")); //获取asset的文件名称
            //assetImporter.SetAssetBundleNameAndVariant()
            assetImporter.assetBundleName = assetName + appendName;    //最终设置assetBundleName
            //assetImporter.assetBundleVariant = ""
        }
    }


    //private void PrepareSkeletonDescription(ModelImporter modelImporter)
    //{
    //    skeletonDescription = modelImporter.humanDescription.skeleton;
    //    List<SkeletonBone> tempSkeletonBoneList = skeletonDescription.ToList();

    //    string refereneAvatarId = _referenceAvatarName.Split('@')[0];
    //    for (int i = 0; i < tempSkeletonBoneList.Count; i++)
    //    {
    //        if (!tempSkeletonBoneList[i].name.Contains(refereneAvatarId))
    //        {
    //            tempSkeletonBoneList.RemoveAt(i);
    //        }
    //    }
    //    skeletonDescription = tempSkeletonBoneList.ToArray();
    //}
    
    /////////////////////////////////////////////////////////////////////////////////////////////////
    
}