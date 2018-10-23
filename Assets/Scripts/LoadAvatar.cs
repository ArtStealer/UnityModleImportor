//using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using YamlDotNet.Serialization;

public class LoadAvatar : MonoBehaviour {

    public string ModelURL = "http://127.0.0.1/AssetBundle/Windows/2";
    public string ModelName = "2";
    public RuntimeAnimatorController animatorController;
    //public Avatar commonHumanAvatar;

    // Use this for initialization
    void Start () {
        StartCoroutine(loadModel());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator loadModel()
    {
        WWW www = new WWW(ModelURL);
        //WWW www = WWW.LoadFromCacheOrDownload(ModelURL,0);
        yield return www;
        if (www.error == null)
        {
            AssetBundle bundle = www.assetBundle;

            AssetBundleRequest request = bundle.LoadAssetAsync(ModelName, typeof(GameObject));

            yield return request;

            

            //GameObject go = request.asset as GameObject;
            GameObject go = Instantiate(request.asset) as GameObject;

            //GameObject parent = GameObject.Find("GameObject");
            //go.transform.SetParent(parent.transform);
            //Debug.Log("CommonAvatar:" + (commonHumanAvatar.isHuman ? "is human" : "is generic"));

            //RigBoneMapping.

            //HumanPoseHandler.SetHumanPose(HumanPose)

            //HumanDescription humDes = new HumanDescription();

            //go.transform.parent = null;
            //go.transform.position = Vector3.zero;
            //go.transform.rotation = Quaternion.identity;

            //HumanDescription humDes = CreateHumanDescription(go);

            //Avatar avatar = AvatarBuilder.BuildHumanAvatar(go, humDes);
            //Avatar avatar = AvatarBuilder.BuildGenericAvatar(go, "");
            //avatar.name = "avatar";
            //Debug.Log(avatar.isHuman ? "is human" : "is generic");

            //Avatar avatar = CreateAvatar(go);

            go.transform.localScale = new Vector3(100,100,100);

            Animator animator = go.GetComponent<Animator>() as Animator;

            //animator.GetBoneTransform(HumanBodyBones.)
            //animator.InterruptMatchTarget();
            //animator.avatar = avatar;
            //animator.avatar = commonHumanAvatar;
            animator.runtimeAnimatorController = animatorController;

            go.transform.localScale = new Vector3(100, 100, 100);
            //animator.Play("RoundKick");
            //animator.Play("Walk");
            animator.Play("Run");
            //animator.Play("Idle");
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    
    ////////////////////////////////////
    
    private void AddSkeletonBone(Transform t, List<SkeletonBone> skeletonBones)
    {
        SkeletonBone bone = new SkeletonBone();
        bone.name = t.name;
        bone.position = t.localPosition;
        bone.rotation = t.localRotation;
        bone.scale = t.localScale;
        skeletonBones.Add(bone);
    }

    private void AddSkeletonBoneRecursive(Transform t, List<SkeletonBone> skeletonBones)
    {
        AddSkeletonBone(t, skeletonBones);
        foreach (Transform item in t)
        {
            AddSkeletonBoneRecursive(item, skeletonBones);
        }
    }

    private Avatar CreateAvatar(GameObject go)
    {
        HumanDescription desc = CreateHumanDescription(go);
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

    private HumanDescription CreateHumanDescription(GameObject skeletonRoot)
    {
        HumanDescription desc = new HumanDescription();

        List<HumanBone> humanBones = new List<HumanBone>();
        List<SkeletonBone> skeletonBones = new List<SkeletonBone>();
        
        Dictionary<string, string> boneName = new System.Collections.Generic.Dictionary<string, string>();
        boneName["Hips"] = "Bip001 Pelvis";
        boneName["Spine"] = "Bip001 Spine";
        boneName["UpperChest"] = "Bip001 Spine2";
        boneName["Chest"] = "Bip001 Spine1";
        boneName["Head"] = "Bip001 Head";
        boneName["Left Thumb Proximal"] = "Bip001 L Finger0";
        boneName["LeftFoot"] = "Bip001 L Foot";
        boneName["LeftHand"] = "Bip001 L Hand";
        boneName["LeftLowerArm"] = "Bip001 L Forearm";
        boneName["LeftLowerLeg"] = "Bip001 L Calf";
        boneName["LeftShoulder"] = "Bip001 L Clavicle";
        boneName["LeftToes"] = "Bip001 L Toe0";
        boneName["LeftUpperArm"] = "Bip001 L UpperArm";
        boneName["LeftUpperLeg"] = "Bip001 L Thigh";
        boneName["Neck"] = "Bip001 Neck";
        boneName["Right Thumb Proximal"] = "Bip001 R Finger0";
        boneName["RightFoot"] = "Bip001 R Foot";
        boneName["RightHand"] = "Bip001 R Hand";
        boneName["RightLowerArm"] = "Bip001 R Forearm";
        boneName["RightLowerLeg"] = "Bip001 R Calf";
        boneName["RightShoulder"] = "Bip001 R Clavicle";
        boneName["RightToes"] = "Bip001 R Toe0";
        boneName["RightUpperArm"] = "Bip001 R UpperArm";
        boneName["RightUpperLeg"] = "Bip001 R Thigh";
        
        string[] humanName = HumanTrait.BoneName;
        int i = 0;
        while (i < humanName.Length)
        {
            if (boneName.ContainsKey(humanName[i]))
            {
                HumanBone humanBone = new HumanBone();
                humanBone.humanName = humanName[i];
                humanBone.boneName = boneName[humanName[i]];
                humanBone.limit.useDefaultValues = true;
                humanBones.Add(humanBone);
            }
            i++;
        }
        //foreach (KeyValuePair<string, string> item in boneName)
        //{
        //    HumanBone humanBone = new HumanBone();
        //    humanBone.humanName = item.Key;
        //    humanBone.boneName = item.Value;
        //    humanBone.limit.useDefaultValues = true;
        //    humanBones.Add(humanBone);
        //}
        AddSkeletonBoneRecursive(skeletonRoot.transform, skeletonBones);

        desc.human = humanBones.ToArray();
        desc.skeleton = skeletonBones.ToArray();

        //desc.upperArmTwist = 0.5f;
        //desc.lowerArmTwist = 0.5f;
        //desc.upperLegTwist = 0.5f;
        //desc.lowerLegTwist = 0.5f;
        //desc.armStretch = 0.05f;
        //desc.legStretch = 0.05f;
        //desc.feetSpacing = 0.0f;

        return desc;
    }

    //private void InitBoneTransforms()
    //{
    //    leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
    //    rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
    //    leftArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
    //    rightArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
    //    leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
    //    rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    //    hips = animator.GetBoneTransform(HumanBodyBones.Hips);
    //    head = animator.GetBoneTransform(HumanBodyBones.Head);
    //    rightEye = animator.GetBoneTransform(HumanBodyBones.RightEye);
    //    leftEye = animator.GetBoneTransform(HumanBodyBones.LeftEye);
    //    spine = animator.GetBoneTransform(HumanBodyBones.Spine);
    //    chest = animator.GetBoneTransform(HumanBodyBones.Chest);
    //    jawTransform = animator.GetBoneTransform(HumanBodyBones.Jaw);
    //}
}
