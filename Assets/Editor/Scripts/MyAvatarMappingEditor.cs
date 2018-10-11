﻿// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Linq;

using Object = UnityEngine.Object;

namespace MyUnityEditor
{
    //public enum BoneState
    //{
    //    None,
    //    NotFound,
    //    Duplicate,
    //    InvalidHierarchy,
    //    BoneLenghtIsZero,
    //    Valid
    //};

    [System.Serializable]
    internal class MyAvatarMappingEditor
    {
        //internal class Styles
        //{
        //    public GUIContent[] BodyPartMapping =
        //    {
        //        EditorGUIUtility.TrTextContent("Avatar"),
        //        EditorGUIUtility.TrTextContent("Body"),
        //        EditorGUIUtility.TrTextContent("Head"),
        //        EditorGUIUtility.TrTextContent("Left Arm"),
        //        EditorGUIUtility.TrTextContent("Left Fingers"),
        //        EditorGUIUtility.TrTextContent("Right Arm"),
        //        EditorGUIUtility.TrTextContent("Right Fingers"),
        //        EditorGUIUtility.TrTextContent("Left Leg"),
        //        EditorGUIUtility.TrTextContent("Right Leg")
        //    };

        //    public GUIContent RequiredBone = EditorGUIUtility.TrTextContent("Optional Bones");
        //    public GUIContent DoneCharacter = EditorGUIUtility.TrTextContent("Done");

        //    public GUIContent mapping = EditorGUIUtility.TrTextContent("Mapping");
        //    public GUIContent clearMapping = EditorGUIUtility.TrTextContent("Clear");
        //    public GUIContent autoMapping = EditorGUIUtility.TrTextContent("Automap");
        //    public GUIContent bipedMapping = EditorGUIUtility.TrTextContent("Biped");
        //    public GUIContent loadMapping = EditorGUIUtility.TrTextContent("Load");
        //    public GUIContent saveMapping = EditorGUIUtility.TrTextContent("Save");

        //    public GUIContent pose = EditorGUIUtility.TrTextContent("Pose");
        //    public GUIContent resetPose = EditorGUIUtility.TrTextContent("Reset");
        //    public GUIContent sampleBindPose = EditorGUIUtility.TrTextContent("Sample Bind-Pose");
        //    public GUIContent enforceTPose = EditorGUIUtility.TrTextContent("Enforce T-Pose");
        //    public GUIContent bipedPose = EditorGUIUtility.TrTextContent("Biped Pose");

        //    public GUIContent ShowError = EditorGUIUtility.TrTextContent("Show Error (s)...");
        //    public GUIContent CloseError = EditorGUIUtility.TrTextContent("Close Error (s)");

        //    public GUIContent dotFill = EditorGUIUtility.IconContent("AvatarInspector/DotFill");
        //    public GUIContent dotFrame = EditorGUIUtility.IconContent("AvatarInspector/DotFrame");
        //    public GUIContent dotFrameDotted = EditorGUIUtility.IconContent("AvatarInspector/DotFrameDotted");
        //    public GUIContent dotSelection = EditorGUIUtility.IconContent("AvatarInspector/DotSelection");

        //    public GUIStyle box = new GUIStyle("box");
        //    public GUIStyle toolbar = "TE Toolbar";
        //    public GUIStyle toolbarDropDown = "TE ToolbarDropDown";

        //    public GUIStyle errorLabel = new GUIStyle(EditorStyles.wordWrappedMiniLabel);

        //    public Styles()
        //    {
        //        box.padding = new RectOffset(0, 0, 0, 0);
        //        box.margin = new RectOffset(0, 0, 0, 0);
        //        errorLabel.normal.textColor = new Color(0.6f, 0, 0, 1);
        //    }
        //}

        //internal static Styles styles { get { if (s_Styles == null) s_Styles = new Styles(); return s_Styles; } }
        //static Styles s_Styles;

        //protected bool[] m_BodyPartToggle;
        //protected bool[] m_BodyPartFoldout;
        //protected int m_BodyView = 0;

        [SerializeField]
        protected MyAvatarSetupTool.BoneWrapper[] m_Bones;
        //internal static int s_SelectedBoneIndex = -1;
        //internal static bool s_DirtySelection = false;
        //internal static int s_KeyboardControl = 0;
        //protected bool m_HasSkinnedMesh;
        //bool m_IsBiped;

        //Editor m_CurrentTransformEditor;
        //bool m_CurrentTransformEditorFoldout;

        public GameObject gameObject;
        protected Dictionary<Transform, bool> modelBones; // MyAvatarSetupTool.GetModelBones(m_GameObject.transform, false, null);
        SerializedObject serializedObject; // SerializedObject so = new SerializedObject(assetImporter);
        //protected Dictionary<Transform, bool> modelBones { get { return m_Inspector.m_ModelBones; } }
        protected Transform root { get { return gameObject == null ? null : gameObject.transform; } }
        //protected SerializedObject serializedObject { get { return m_Inspector.serializedAssetImporter; } }
        //protected Avatar avatarAsset; // { get { return m_Inspector.avatar; } }

        // This list containt the mecanim's human bones id for each body part
        //protected int[][] m_BodyPartHumanBone =
        //{
        //    new int[] {-1},
        //    new int[] { (int)HumanBodyBones.Hips, (int)HumanBodyBones.Spine, (int)HumanBodyBones.Chest, (int)HumanBodyBones.UpperChest},
        //    new int[] { (int)HumanBodyBones.Neck, (int)HumanBodyBones.Head, (int)HumanBodyBones.LeftEye, (int)HumanBodyBones.RightEye, (int)HumanBodyBones.Jaw},
        //    new int[] { (int)HumanBodyBones.LeftShoulder, (int)HumanBodyBones.LeftUpperArm, (int)HumanBodyBones.LeftLowerArm, (int)HumanBodyBones.LeftHand},
        //    new int[]
        //    {
        //        (int)HumanBodyBones.LeftThumbProximal , (int)HumanBodyBones.LeftThumbIntermediate, (int)HumanBodyBones.LeftThumbDistal,
        //        (int)HumanBodyBones.LeftIndexProximal , (int)HumanBodyBones.LeftIndexIntermediate, (int)HumanBodyBones.LeftIndexDistal,
        //        (int)HumanBodyBones.LeftMiddleProximal , (int)HumanBodyBones.LeftMiddleIntermediate, (int)HumanBodyBones.LeftMiddleDistal,
        //        (int)HumanBodyBones.LeftRingProximal , (int)HumanBodyBones.LeftRingIntermediate, (int)HumanBodyBones.LeftRingDistal,
        //        (int)HumanBodyBones.LeftLittleProximal , (int)HumanBodyBones.LeftLittleIntermediate, (int)HumanBodyBones.LeftLittleDistal
        //    },
        //    new int[] { (int)HumanBodyBones.RightShoulder, (int)HumanBodyBones.RightUpperArm, (int)HumanBodyBones.RightLowerArm, (int)HumanBodyBones.RightHand},
        //    new int[]
        //    {
        //        (int)HumanBodyBones.RightThumbProximal , (int)HumanBodyBones.RightThumbIntermediate, (int)HumanBodyBones.RightThumbDistal,
        //        (int)HumanBodyBones.RightIndexProximal , (int)HumanBodyBones.RightIndexIntermediate, (int)HumanBodyBones.RightIndexDistal,
        //        (int)HumanBodyBones.RightMiddleProximal , (int)HumanBodyBones.RightMiddleIntermediate, (int)HumanBodyBones.RightMiddleDistal,
        //        (int)HumanBodyBones.RightRingProximal , (int)HumanBodyBones.RightRingIntermediate, (int)HumanBodyBones.RightRingDistal,
        //        (int)HumanBodyBones.RightLittleProximal , (int)HumanBodyBones.RightLittleIntermediate, (int)HumanBodyBones.RightLittleDistal
        //    },
        //    new int[] { (int)HumanBodyBones.LeftUpperLeg, (int)HumanBodyBones.LeftLowerLeg, (int)HumanBodyBones.LeftFoot, (int)HumanBodyBones.LeftToes},
        //    new int[] { (int)HumanBodyBones.RightUpperLeg, (int)HumanBodyBones.RightLowerLeg, (int)HumanBodyBones.RightFoot, (int)HumanBodyBones.RightToes}
        //};

        //public AvatarMappingEditor()
        //{
        //    m_BodyPartToggle = new bool[(int)BodyPart.Last];
        //    m_BodyPartFoldout = new bool[(int)BodyPart.Last];
        //    for (int i = 0; i < (int)BodyPart.Last; i++)
        //    {
        //        m_BodyPartToggle[i] = false;
        //        m_BodyPartFoldout[i] = true;
        //    }
        //}

        
        public void Init(GameObject go, SkeletonBone[] bones)
        {
            if (go == null)
                return;
            gameObject = go;
            //serializedObject = so; // SerializedObject so = new SerializedObject(assetImporter);
            modelBones = MyAvatarSetupTool.GetModelBones(gameObject.transform, false, null);

            //m_IsBiped = AvatarBipedMapper.IsBiped(gameObject.transform, null);

            // Handle human bones
            if (m_Bones == null)
            {
                m_Bones = MyAvatarSetupTool.GetHumanBones(bones, modelBones);
            }
            //ValidateMapping();

            //m_CurrentTransformEditorFoldout = true;
            //m_HasSkinnedMesh = (gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null);

            // Handle pose
            //InitPose();

            //// Repaint
            //SceneView.RepaintAll();
        }

        public void Init(GameObject go, SerializedObject so)
        {
            if (go == null)
                return;
            gameObject = go;
            serializedObject = so; // SerializedObject so = new SerializedObject(assetImporter);
            modelBones = MyAvatarSetupTool.GetModelBones(gameObject.transform, false, null);

            //m_IsBiped = AvatarBipedMapper.IsBiped(gameObject.transform, null);

            // Handle human bones
            if (m_Bones == null)
            {
                m_Bones = MyAvatarSetupTool.GetHumanBones(serializedObject, modelBones);
            }
            //ValidateMapping();

            //m_CurrentTransformEditorFoldout = true;
            //m_HasSkinnedMesh = (gameObject.GetComponentInChildren<SkinnedMeshRenderer>() != null);

            // Handle pose
            //InitPose();

            //// Repaint
            //SceneView.RepaintAll();
        }

        protected void ResetBones()
        {
            for (int i = 0; i < m_Bones.Length; i++)
                m_Bones[i].Reset(serializedObject, modelBones);
        }

        //protected bool IsValidHuman()
        //{
        //    Animator animator = gameObject.GetComponent<Animator>();
        //    if (animator == null)
        //        return false;

        //    Avatar avatar = animator.avatar;
        //    return avatar != null && avatar.isHuman;
        //}

        //protected void InitPose()
        //{
        //    if (IsValidHuman())
        //    {
        //        Animator animator = gameObject.GetComponent<Animator>();
        //        animator.WriteDefaultPose();
        //        MyAvatarSetupTool.TransferDescriptionToPose(serializedObject, root);
        //    }
        //}

        protected void ValidateMapping()
        {
            for (int i = 0; i < m_Bones.Length; i++)
            {
                string error;
                m_Bones[i].state = GetBoneState(i, out error);
                m_Bones[i].error = error;
            }
        }

        //private void EnableBodyParts(bool[] toggles, params int[] parts)
        //{
        //    for (int i = 0; i < m_BodyPartToggle.Length; i++)
        //        toggles[i] = false;
        //    foreach (int i in parts)
        //        toggles[i] = true;
        //}

        //private void HandleBodyView(int bodyView)
        //{
        //    if (bodyView == 0)
        //        EnableBodyParts(m_BodyPartToggle, 1, 3, 5, 7, 8);
        //    if (bodyView == 1)
        //        EnableBodyParts(m_BodyPartToggle, 2);
        //    if (bodyView == 2)
        //        EnableBodyParts(m_BodyPartToggle, 4);
        //    if (bodyView == 3)
        //        EnableBodyParts(m_BodyPartToggle, 6);
        //}

        //Vector2 m_FoldoutScroll = Vector2.zero;


        //protected void CopyPrefabPose()
        //{
        //    MyAvatarSetupTool.CopyPose(gameObject, prefab);
        //    AvatarSetupTool.TransferPoseToDescription(serializedObject, root);
        //    m_Inspector.Repaint();
        //}

        //protected void SampleBindPose()
        //{
        //    AvatarSetupTool.SampleBindPose(gameObject);
        //    AvatarSetupTool.TransferPoseToDescription(serializedObject, root);
        //    m_Inspector.Repaint();
        //}

        //protected void BipedPose()
        //{
        //    AvatarBipedMapper.BipedPose(gameObject, m_Bones);

        //    AvatarSetupTool.TransferPoseToDescription(serializedObject, root);
        //    m_Inspector.Repaint();
        //}

        public MyAvatarSetupTool.BoneWrapper[] MakePoseValid()
        {
            MyAvatarSetupTool.MakePoseValid(m_Bones);
            //MyAvatarSetupTool.TransferPoseToDescription(serializedObject, root);
            return m_Bones;
        }

        //protected void PerformAutoMapping()
        //{
        //    AutoMapping();
        //    ValidateMapping();
        //    SceneView.RepaintAll();
        //}

        //protected void PerformBipedMapping()
        //{
        //    BipedMapping();
        //    ValidateMapping();
        //    SceneView.RepaintAll();
        //}

        //protected void AutoMapping()
        //{
        //    Dictionary<int, Transform> mapping = AvatarAutoMapper.MapBones(gameObject.transform, modelBones);
        //    foreach (KeyValuePair<int, Transform> kvp in mapping)
        //    {
        //        MyAvatarSetupTool.BoneWrapper bone = m_Bones[kvp.Key];
        //        bone.bone = kvp.Value;
        //        bone.Serialize(serializedObject);
        //    }
        //}

        //protected void BipedMapping()
        //{
        //    Dictionary<int, Transform> mapping = AvatarBipedMapper.MapBones(gameObject.transform);
        //    foreach (KeyValuePair<int, Transform> kvp in mapping)
        //    {
        //        MyAvatarSetupTool.BoneWrapper bone = m_Bones[kvp.Key];
        //        bone.bone = kvp.Value;
        //        bone.Serialize(serializedObject);
        //    }
        //}

        //protected void ClearMapping()
        //{
        //    if (serializedObject != null)
        //    {
        //        Undo.RegisterCompleteObjectUndo(this, "Clear Mapping");
        //        MyAvatarSetupTool.ClearHumanBoneArray(serializedObject);
        //        ResetBones();
        //        ValidateMapping();
        //        SceneView.RepaintAll();
        //    }
        //}

        //protected Vector4 QuaternionToVector4(Quaternion rot)
        //{
        //    return new Vector4(rot.x, rot.y, rot.z, rot.w);
        //}

        //protected Quaternion Vector4ToQuaternion(Vector4 rot)
        //{
        //    return new Quaternion(rot.x, rot.y, rot.z, rot.w);
        //}

        //protected bool IsAnyBodyPartActive()
        //{
        //    // Avatar body part has nothing to show
        //    for (int i = 1; i < m_BodyPartToggle.Length; i++)
        //    {
        //        if (m_BodyPartToggle[i])
        //            return true;
        //    }
        //    return false;
        //}

        //private void UpdateSelectedBone()
        //{
        //    // Look for selected bone slot
        //    int oldSelectedBoneIndex = s_SelectedBoneIndex;
        //    if (s_SelectedBoneIndex < 0 || s_SelectedBoneIndex >= m_Bones.Length || m_Bones[s_SelectedBoneIndex].bone != Selection.activeTransform)
        //    {
        //        s_SelectedBoneIndex = -1;
        //        if (Selection.activeTransform != null)
        //        {
        //            for (int i = 0; i < m_Bones.Length; i++)
        //            {
        //                if (m_Bones[i].bone == Selection.activeTransform)
        //                {
        //                    s_SelectedBoneIndex = i;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    if (s_SelectedBoneIndex != oldSelectedBoneIndex)
        //    {
        //        // If selected bone changed, change visual to show image that contains that bone.
        //        List<int> views = AvatarControl.GetViewsThatContainBone(s_SelectedBoneIndex);
        //        if (views.Count > 0 && !views.Contains(m_BodyView))
        //            m_BodyView = views[0];
        //    }
        //}

        //protected void DisplayFoldout()
        //{
        //    Dictionary<Transform, bool> bones = modelBones;

        //    EditorGUIUtility.SetIconSize(Vector2.one * 16);

        //    // Legend
        //    EditorGUILayout.BeginHorizontal();
        //    GUI.color = Color.grey;
        //    GUILayout.Label(styles.dotFrameDotted.image, GUILayout.ExpandWidth(false));
        //    GUI.color = Color.white;
        //    GUILayout.Label("Optional Bone", GUILayout.ExpandWidth(true));
        //    EditorGUILayout.EndHorizontal();

        //    // Avatar body part has nothing to show
        //    for (int i = 1; i < m_BodyPartToggle.Length; i++)
        //    {
        //        if (m_BodyPartToggle[i])
        //        {
        //            //  Unfold body part ui whenever a new selection is made.
        //            if ((s_DirtySelection == true) && (m_BodyPartFoldout[i] == false))
        //            {
        //                for (int j = 0; j < m_BodyPartHumanBone[i].Length; j++)
        //                {
        //                    int boneIndex = m_BodyPartHumanBone[i][j];
        //                    if (s_SelectedBoneIndex == boneIndex)
        //                        m_BodyPartFoldout[i] = true;
        //                }
        //            }

        //            m_BodyPartFoldout[i] = GUILayout.Toggle(m_BodyPartFoldout[i], styles.BodyPartMapping[i], EditorStyles.foldout);
        //            EditorGUI.indentLevel++;
        //            if (m_BodyPartFoldout[i])
        //            {
        //                for (int j = 0; j < m_BodyPartHumanBone[i].Length; j++)
        //                {
        //                    int boneIndex = m_BodyPartHumanBone[i][j];
        //                    if (boneIndex == -1)
        //                        continue;

        //                    AvatarSetupTool.BoneWrapper bone = m_Bones[boneIndex];
        //                    string displayBoneName = bone.humanBoneName;

        //                    // @TODO@MECANIM: do properly
        //                    if ((BodyPart)i == BodyPart.RightArm || (BodyPart)i == BodyPart.RightFingers || (BodyPart)i == BodyPart.RightLeg)
        //                        displayBoneName = displayBoneName.Replace("Right", "");
        //                    if ((BodyPart)i == BodyPart.LeftArm || (BodyPart)i == BodyPart.LeftFingers || (BodyPart)i == BodyPart.LeftLeg)
        //                        displayBoneName = displayBoneName.Replace("Left", "");

        //                    displayBoneName = ObjectNames.NicifyVariableName(displayBoneName);

        //                    Rect r = EditorGUILayout.GetControlRect();

        //                    Rect selectRect = r;
        //                    selectRect.width -= 15;

        //                    Rect rect = new Rect(r.x + EditorGUI.indent, r.y - 1, AvatarSetupTool.BoneWrapper.kIconSize, AvatarSetupTool.BoneWrapper.kIconSize);

        //                    bone.BoneDotGUI(rect, selectRect, boneIndex, true, false, true, serializedObject, this);
        //                    r.xMin += AvatarSetupTool.BoneWrapper.kIconSize;

        //                    Transform newBoneTransform = EditorGUI.ObjectField(r, new GUIContent(displayBoneName), bone.bone, typeof(Transform), true) as Transform;
        //                    if (newBoneTransform != bone.bone)
        //                    {
        //                        Undo.RegisterCompleteObjectUndo(this, "Avatar mapping modified");
        //                        bone.bone = newBoneTransform;
        //                        bone.Serialize(serializedObject);

        //                        // User adding a bone manually, if it not in the modelBones dict, we must explictly add it
        //                        if (newBoneTransform != null && !bones.ContainsKey(newBoneTransform))
        //                            bones[newBoneTransform] = true;
        //                    }

        //                    if (!string.IsNullOrEmpty(bone.error))
        //                    {
        //                        GUILayout.BeginHorizontal();
        //                        GUILayout.Space(EditorGUI.indent + AvatarSetupTool.BoneWrapper.kIconSize + 4);
        //                        GUILayout.Label(bone.error, s_Styles.errorLabel);
        //                        GUILayout.EndHorizontal();
        //                    }
        //                }
        //            }
        //            EditorGUI.indentLevel--;
        //        }
        //    }

        //    s_DirtySelection = false;

        //    EditorGUIUtility.SetIconSize(Vector2.zero);
        //}

        //bool TransformChanged(Transform tr)
        //{
        //    SerializedProperty bone = MyAvatarSetupTool.FindSkeletonBone(serializedObject, tr, false, false);
        //    if (bone != null)
        //    {
        //        SerializedProperty positionP = bone.FindPropertyRelative(MyAvatarSetupTool.sPosition);
        //        if (positionP != null && positionP.vector3Value != tr.localPosition)
        //            return true;

        //        SerializedProperty rotationP = bone.FindPropertyRelative(MyAvatarSetupTool.sRotation);
        //        if (rotationP != null && rotationP.quaternionValue != tr.localRotation)
        //            return true;

        //        SerializedProperty scaleP = bone.FindPropertyRelative(MyAvatarSetupTool.sScale);
        //        if (scaleP != null && scaleP.vector3Value != tr.localScale)
        //            return true;
        //    }
        //    return false;
        //}

        protected BoneState GetBoneState(int i, out string error)
        {
            error = string.Empty;
            MyAvatarSetupTool.BoneWrapper bone = m_Bones[i];

            if (bone.bone == null)
                return BoneState.None;

            int ancestorIndex = MyAvatarSetupTool.GetFirstHumanBoneAncestor(m_Bones, i);

            MyAvatarSetupTool.BoneWrapper ancestor = m_Bones[ancestorIndex > 0 ? ancestorIndex : 0];

            if (i == 0 && bone.bone.parent == null)
            {
                error = bone.messageName + " cannot be the root transform";
                return BoneState.InvalidHierarchy;
            }

            if (ancestor.bone != null && !bone.bone.IsChildOf(ancestor.bone))
            {
                error = bone.messageName + " is not a child of " + ancestor.messageName + ".";
                return BoneState.InvalidHierarchy;
            }

            if (i == (int)HumanBodyBones.UpperChest)
            {
                MyAvatarSetupTool.BoneWrapper chest = m_Bones[(int)HumanBodyBones.Chest];

                if (chest.bone == null)
                {
                    error = "Chest must be assigned before assigning UpperChest.";
                    return BoneState.InvalidHierarchy;
                }
            }

            // Hips bone is a special case, for hips GetFirstAnscestor return hips
            if (i != (int)HumanBodyBones.Jaw && ancestor.bone != null && ancestor.bone != bone.bone && (bone.bone.position - ancestor.bone.position).sqrMagnitude < Mathf.Epsilon)
            {
                error = bone.messageName + " has bone length of zero.";
                return BoneState.BoneLenghtIsZero;
            }

            // Does this transfrom is already set
            //List<BoneWrapper> match = ArrayUtility.FindAll (m_BoneWrappers, delegate (Transform t) { return t == m_BoneWrappers[i].bone; });
            IEnumerable<MyAvatarSetupTool.BoneWrapper> match = m_Bones.Where(f => f.bone == bone.bone);
            // when we search in the list we must add 1 because the bone is in this list
            if (match.Count() > 1)
            {
                error = bone.messageName + " is also assigned to ";
                bool first = true;
                for (int j = 0; j < m_Bones.Length; j++)
                {
                    if (i != j && m_Bones[i].bone == m_Bones[j].bone)
                    {
                        if (first)
                            first = false;
                        else
                            error += ", ";
                        error += ObjectNames.NicifyVariableName(m_Bones[j].humanBoneName);
                    }
                }
                error += ".";
                return BoneState.Duplicate;
            }
            else
                return BoneState.Valid;
        }

        //protected AvatarControl.BodyPartColor IsValidBodyPart(BodyPart bodyPart)
        //{
        //    AvatarControl.BodyPartColor ik = AvatarControl.BodyPartColor.Off;
        //    bool hasAnyBone = false;

        //    int idx = (int)bodyPart;
        //    if (bodyPart != BodyPart.LeftFingers && bodyPart != BodyPart.RightFingers)
        //    {
        //        int i;
        //        for (i = 0; i < m_BodyPartHumanBone[idx].Length; i++)
        //        {
        //            if (m_BodyPartHumanBone[idx][i] != -1)
        //            {
        //                BoneState state = m_Bones[m_BodyPartHumanBone[idx][i]].state;
        //                hasAnyBone |= state == BoneState.Valid;

        //                // if it a required bone and no bone is set this body part is not valid
        //                if (HumanTrait.RequiredBone(m_BodyPartHumanBone[idx][i]) == true && state == BoneState.None)
        //                    return AvatarControl.BodyPartColor.Red;
        //                else if (state != BoneState.Valid && state != BoneState.None)
        //                    return AvatarControl.BodyPartColor.Red;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        bool hasAllFinger = true;

        //        int i;
        //        int phalangeCount = 3;
        //        for (i = 0; i < m_BodyPartHumanBone[idx].Length / phalangeCount; i++)
        //        {
        //            bool hasFinger = false;
        //            int fingerIndex = i * phalangeCount;
        //            int j;
        //            for (j = phalangeCount - 1; j >= 0; j--)
        //            {
        //                bool hasBone = m_Bones[m_BodyPartHumanBone[idx][fingerIndex + j]].state == BoneState.Valid;
        //                hasAllFinger &= hasBone;
        //                if (hasFinger)
        //                {
        //                    if (!hasBone)
        //                        return AvatarControl.BodyPartColor.Red | AvatarControl.BodyPartColor.IKRed;
        //                }
        //                else
        //                    hasAnyBone |= hasFinger = !hasFinger && hasBone;
        //            }
        //        }

        //        ik = hasAllFinger ? AvatarControl.BodyPartColor.IKGreen : AvatarControl.BodyPartColor.IKRed;
        //    }

        //    if (!hasAnyBone)
        //        return AvatarControl.BodyPartColor.Off | AvatarControl.BodyPartColor.IKRed;

        //    return AvatarControl.BodyPartColor.Green | ik;
        //}

        //private HumanTemplate OpenHumanTemplate()
        //{
        //    string assetsDirectory = "Assets/";

        //    // Go forward with presenting user a save clip dialog
        //    string templatePath = EditorUtility.OpenFilePanel("Open Human Template", assetsDirectory, "ht");

        //    // If user canceled or save path is invalid, we can't create a template
        //    if (templatePath == "")
        //        return null;

        //    // At this point we know that we can create a template
        //    string relativeTemplatePath = FileUtil.GetProjectRelativePath(templatePath);
        //    HumanTemplate template = AssetDatabase.LoadMainAssetAtPath(relativeTemplatePath) as HumanTemplate;

        //    // Asset not found in project, try to import it into the project
        //    if (template == null)
        //    {
        //        if (EditorUtility.DisplayDialog("Human Template not found in project", "Import asset \'" + templatePath + "\' into project", "Yes", "No"))
        //        {
        //            string newFileName = assetsDirectory + FileUtil.GetLastPathNameComponent(templatePath);

        //            newFileName = AssetDatabase.GenerateUniqueAssetPath(newFileName);

        //            // Copy file
        //            FileUtil.CopyFileOrDirectory(templatePath, newFileName);

        //            AssetDatabase.Refresh();

        //            template = AssetDatabase.LoadMainAssetAtPath(newFileName) as HumanTemplate;

        //            if (template == null)
        //            {
        //                Debug.Log("Failed importing file \'" + templatePath + "\' to \'" + newFileName + "\'");
        //            }
        //        }
        //    }
        //    return template;
        //}

        //static public bool MatchName(string transformName, string boneName)
        //{
        //    string delimStr = ":";
        //    char[] delimiter = delimStr.ToCharArray();

        //    string[] transformSplit = transformName.Split(delimiter);
        //    string[] boneSplit = boneName.Split(delimiter);

        //    return transformName == boneName ||
        //        (transformSplit.Length > 1 && transformSplit[1] == boneName) ||  // transform does have a prefix
        //        (boneSplit.Length > 1 && transformName == boneSplit[1]) ||  // boneName does have a prefix
        //        (transformSplit.Length > 1 && boneSplit.Length > 1 && transformSplit[1] == boneSplit[1]);  // both have a different prefix
        //}

        //protected void ApplyTemplate()
        //{
        //    Undo.RegisterCompleteObjectUndo(this, "Apply Template");

        //    HumanTemplate humanTemplate = OpenHumanTemplate();
        //    if (humanTemplate == null)
        //        return;

        //    for (int i = 0; i < m_Bones.Length; i++)
        //    {
        //        string boneName = humanTemplate.Find(m_Bones[i].humanBoneName);
        //        if (boneName.Length > 0)
        //        {
        //            Transform transform = modelBones.Keys.FirstOrDefault(f => AvatarMappingEditor.MatchName(f.name, boneName));
        //            m_Bones[i].bone = transform;
        //        }
        //        else
        //        {
        //            m_Bones[i].bone = null;
        //        }
        //        m_Bones[i].Serialize(serializedObject);
        //    }

        //    ValidateMapping();
        //    SceneView.RepaintAll();
        //}

        //private void SaveHumanTemplate()
        //{
        //    // Go forward with presenting user a save clip dialog
        //    string newTemplatePath = EditorUtility.SaveFilePanelInProject("Create New Human Template", "New Human Template", "ht", "Create a new human template");

        //    // If user canceled or save path is invalid, we can't create a template
        //    if (newTemplatePath == "")
        //        return;

        //    // At this point we know that we can create a template
        //    HumanTemplate humanTemplate = new HumanTemplate();
        //    humanTemplate.ClearTemplate();

        //    for (int i = 0; i < m_Bones.Length; i++)
        //    {
        //        if (m_Bones[i].bone != null)
        //            humanTemplate.Insert(m_Bones[i].humanBoneName, m_Bones[i].bone.name);
        //    }

        //    AssetDatabase.CreateAsset(humanTemplate, newTemplatePath);
        //}

        //public override void OnSceneGUI()
        //{
        //    // It seems styles can't safely be created from inside OnSceneGUI so not much we can do here. :(
        //    if (s_Styles == null)
        //        return;

        //    AvatarSkeletonDrawer.DrawSkeleton(root, modelBones, m_Bones);

        //    if (EditorGUIUtility.hotControl == 0)
        //        TransferPoseIfChanged();
        //}
    }
}
