using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
//using System;
//using System.Linq;
using MyUnityEditor;

class HumanoidModelImporter : AssetPostprocessor
{
    private static bool open = true;
    //public static string humanTemplateFile = "human"; // Resources/human
    public static string humanTemplateFile = "Models/human"; // Resources/human
    private static SkeletonBone[] skeletonDescription;
    private static AssetImporter thisImporter;
    private static bool secondPass = false;
    private static SerializedObject serializedObject;
    //private static GameObject gameObject;

    void OnPreprocessModel()
    {
        if (!open) { return; }

        thisImporter = null;
        if (!MyClass.IsWorking())
        {
            return;
        }
        Debug.Log("OnPreprocessModel:" + assetPath);
        
        if (assetPath.ToUpper().EndsWith(".FBX")) // (assetPath.Contains(MyClass.pathImport))
        {
            thisImporter = assetImporter;
            

            if (secondPass)
            {

                ModelImporter modelImporter = assetImporter as ModelImporter;
                //modelImporter.animationCompression = ModelImporterAnimationCompression.Off;
                //modelImporter.animationPositionError = 0.5f;
                //modelImporter.animationRotationError = 0.5f;
                //modelImporter.animationScaleError = 0.5f;
                //modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
                //modelImporter.importAnimation = true;
                //modelImporter.resampleCurves = true;
                modelImporter.animationType = ModelImporterAnimationType.Human;
                modelImporter.humanDescription = ReadHumanDescription();
            }
            //modelImporter.sourceAvatar = null;

            //PrepareSkeletonDescription(modelImporter);
        }
    }
    void OnPostprocessModel(GameObject go)
    {
        if (!open) { return; }

        if (thisImporter != assetImporter)
        {
            Debug.Log("ignor importer");
            return;
        }
        if (!MyClass.IsWorking())
        {
            return;
        }
        Debug.Log("OnPostprocessModel:" + assetPath);


        //gameObject = go;
        //if (skeletonDescription == null)
        //{
        //    PrepareSkeletonDescription(modelImporter);
        //}
        //if (secondPass)
        //{
        //    serializedObject = new SerializedObject(assetImporter);
        //    MyAvatarMappingEditor editor = new MyAvatarMappingEditor();
        //    editor.Init(go, serializedObject);
        //    MyAvatarSetupTool.BoneWrapper[] bones = editor.MakePoseValid();
        //}


        if (!secondPass)
        {

            //serializedObject = new SerializedObject(assetImporter);
            //MyAvatarMappingEditor editor = new MyAvatarMappingEditor();
            //editor.Init(go, serializedObject);
            //editor.MakePoseValid();

            //Animator a = go.GetComponent<Animator>();
            //if (a)
            //{
            //    a.Rebind();
            //}
            go.transform.parent = null;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;

            List<SkeletonBone> skeletonBones = new List<SkeletonBone>();
            AddSkeletonBoneRecursive(go.transform, skeletonBones);
            skeletonDescription = skeletonBones.ToArray();
            Debug.Log(skeletonBones);

            HumanTemplate template = Resources.Load(humanTemplateFile) as HumanTemplate;
            
            // TODO modify bone
            serializedObject = new SerializedObject(assetImporter);
            MyAvatarMappingEditor editor = new MyAvatarMappingEditor();
            editor.Init(go, skeletonDescription, template);
            //editor.Init(go, serializedObject);
            MyAvatarSetupTool.BoneWrapper[] bones = editor.MakePoseValid();
            for(int i = 0; i < skeletonDescription.Length; i++)
            {
                //SkeletonBone skeleton = skeletonDescription[i];
                for(int j = 0; j < bones.Length; j++)
                {
                    Transform bone = bones[j].bone;
                    if (bone && bone.name == skeletonDescription[i].name)
                    {
                        skeletonDescription[i].position = bone.localPosition;
                        skeletonDescription[i].rotation = bone.localRotation;
                        skeletonDescription[i].scale = bone.localScale;
                    }
                }
            }
            

            //ModelImporter modelImporter = assetImporter as ModelImporter;

            //HumanDescription humDesc = modelImporter.humanDescription;
            //humDesc.skeleton = skeletonBones.ToArray();



            AssetDatabase.ImportAsset(assetPath);
            secondPass = true;
        }
        else
        {
            secondPass = false;
            

            //Animator anim = go.GetComponent<Animator>();
            //Avatar avatar = anim.avatar;

            //HumanPose pose = new HumanPose();
            //HumanPoseHandler handler = new HumanPoseHandler(go.GetComponent<Animator>().avatar, go.transform);
            //handler.SetHumanPose(ref pose);

            //    AvatarMappingEditor
            //    AvatarSubEditor
            //    AvatarSetupTool.MakePoseValid(this.m_Bones);
            //AvatarSetupTool.TransferPoseToDescription(base.serializedObject, base.root);
        }
    }

    //private void PrepareSkeletonDescription(ModelImporter modelImporter)
    //{
    //    skeletonDescription = modelImporter.humanDescription.skeleton;
    //    List<SkeletonBone> tempSkeletonBoneList = skeletonDescription.ToList();

    //    //string refereneAvatarId = _referenceAvatarName.Split('@')[0];
    //    //for (int i = 0; i < tempSkeletonBoneList.Count; i++)
    //    //{
    //    //    if (!tempSkeletonBoneList[i].name.Contains(refereneAvatarId))
    //    //    {
    //    //        tempSkeletonBoneList.RemoveAt(i);
    //    //    }
    //    //}
    //    skeletonDescription = tempSkeletonBoneList.ToArray();
    //    Debug.Log(skeletonDescription);
    //}

    private void AddSkeletonBone(Transform t, List<SkeletonBone> skeletonBones)
    {
        SkeletonBone bone = new SkeletonBone();
        bone.name = t.name;
        bone.position = t.localPosition;
        bone.rotation = t.localRotation;
        bone.scale = t.localScale;
        skeletonBones.Add(bone);

        //AnimationUtility.SetAdditiveReferencePose
    }

    private void AddSkeletonBoneRecursive(Transform t, List<SkeletonBone> skeletonBones)
    {
        AddSkeletonBone(t, skeletonBones);
        foreach (Transform item in t)
        {
            AddSkeletonBoneRecursive(item, skeletonBones);
        }
    }

    private static HumanDescription ReadHumanDescription()
    {
        HumanDescription humanDescription = new HumanDescription();
        List<HumanBone> humanBones = new List<HumanBone>();

        HumanTemplate template = Resources.Load(humanTemplateFile) as HumanTemplate;
        string[] boneNames = HumanTrait.BoneName;

        //List<string> mapping = new List<string>();
        foreach (string boneName in boneNames)
        {
            HumanBone newBone = new HumanBone();
            newBone.humanName = boneName;
            newBone.boneName = template.Find(boneName);

            if (newBone.boneName != "")
            {
                HumanLimit limit = new HumanLimit();
                limit.useDefaultValues = true;
                newBone.limit = limit;
                humanBones.Add(newBone);
            }
        }

        humanDescription.human = humanBones.ToArray();
        humanDescription.upperArmTwist = 0.5f;
        humanDescription.lowerArmTwist = 0.5f;
        humanDescription.upperLegTwist = 0.5f;
        humanDescription.lowerLegTwist = 0.5f;
        humanDescription.armStretch = 0.05f;
        humanDescription.legStretch = 0.05f;
        humanDescription.feetSpacing = 0.0f;
        humanDescription.hasTranslationDoF = true;

        //if (secondPass && _isAnimation)
        {
            //skeletonDescription = modelImporter.humanDescription.skeleton;
            List<SkeletonBone> skeletonBones = new List<SkeletonBone>();
            for (int i = 0; i < skeletonDescription.Length; i++)
            {
                SkeletonBone newSkeletonBone = new SkeletonBone();
                newSkeletonBone.name = skeletonDescription[i].name;
                newSkeletonBone.position = skeletonDescription[i].position;
                newSkeletonBone.rotation = skeletonDescription[i].rotation;
                newSkeletonBone.scale = skeletonDescription[i].scale;
                //newSkeletonBone.transformModified = skeletonDescription[i].transformModified;

                skeletonBones.Add(newSkeletonBone);
            }

            humanDescription.skeleton = skeletonBones.ToArray();
        }
        return humanDescription;
    }

    private static Avatar CreateAvatar(GameObject go)
    {
        //HumanDescription desc = CreateHumanDescription(go);
        HumanDescription desc = ReadHumanDescription();
        Transform parent = go.transform.parent;
        Vector3 position = go.transform.position;
        Quaternion rotation = go.transform.rotation;
        go.transform.parent = null;
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        Avatar avatar = AvatarBuilder.BuildHumanAvatar(go, desc);
        go.transform.parent = parent;
        go.transform.position = position;
        go.transform.rotation = rotation;
        if (avatar == null)
        {
            Debug.LogError("Something went wrong building the avatar.");
            return null;
        }
        if (!avatar.isValid)
        {
            Debug.LogError("Avatar is invalid.");
            return null;
        }
        avatar.name = "avatar";
        return avatar;
    }
}
