/*
 *  Lauren Cairco Dukes
 *  January 2014
 *  
 * AvatarUtils.cs
 * 
 * Utilities for avatars to be loaded with BuildAvatar script
 * 
 * Right now all it does is export out some t-pose data, if your skeleton isn't in tpose to begin with
 * Steps to make this work:
 * 
 * Load an an avatar with the same skeletal structure as you need to import at runtime
 * Select it in the project tab, then go through the humanoid rigging process, ensuring everything is correct
 * Select "Enforce T-Pose"
 * GO to the hierarchy tab and drag your gameobject into your project tab
 * It will turn blue in the hierarchy and show up as a prefab in your project
 * Put that prefab in a scene. It should be your character standing in tpose
 * Run. Select your character and its root. Click the button.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarUtils : MonoBehaviour {

    List<GameObject> characters;
    List<string> characterNames;
    int selectedCharacter;
    int root;

	// Use this for initialization
	void Start () {
        //Get all the characters in the scene that are humanoid avatars and store them
        selectedCharacter = 0;
        root = 0;
        characters = new List<GameObject>();
        characterNames = new List<string>();
        foreach (GameObject obj in UnityEngine.Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.GetComponent<Animator>() != null && obj.transform.parent == null && obj.GetComponent<Animator>().avatar != null && obj.GetComponent<Animator>().avatar.isHuman)
            {
                characters.Add(obj);
                characterNames.Add(obj.name);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnGUI()
    {
        //Show a list of the humanoid avatars in the scene
        GUILayout.BeginArea(new Rect(0, 0, Screen.width / 4.0f, Screen.height));
        GUILayout.BeginVertical();
        GUILayout.Label("Please select the character you'd like to try and export the info for. Be sure your character is rigged using Mechanim and is in a t-pose.");
        selectedCharacter =  GUILayout.SelectionGrid(selectedCharacter, characterNames.ToArray(), 1);

        //Show a list of that character's children.
        GUILayout.Label("Please select the character's root (the parent to the hips bone):");
        List<string> names = new List<string>();
        for (int i = 0; i < characters[selectedCharacter].transform.childCount; i++)
        {
            names.Add(characters[selectedCharacter].transform.GetChild(i).name);
        }
        root = GUILayout.SelectionGrid(root, names.ToArray(), 1);

        GUILayout.FlexibleSpace();

        //show a button for exporting the tpose file
        if (GUILayout.Button("Export tpose file"))
        {
            exportData(selectedCharacter, names[root]);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    //Gets the transforms of the children of the given character and prints them out to file
    void exportData(int selectedCharacter, string root){
        GameObject character = characters[selectedCharacter];
        Transform xform = character.transform.Find(root);
        Dictionary<string, Transform> xforms = new Dictionary<string, Transform>();

        //recursively visit the children, gathering their transform data into the xforms dictionary
        recursiveTransform(xform, ref xforms);

        List<string> lines = new List<string>();

        //write out to file the names with all the transform data
        foreach (KeyValuePair<string, Transform> t in xforms)
        {
            string s = t.Key + ",";
            s = s + t.Value.localPosition.x + "," + t.Value.localPosition.y + "," + t.Value.localPosition.z + ",";
            s = s + t.Value.localRotation.x + "," + t.Value.localRotation.y + "," +t.Value.localRotation.z + "," +t.Value.localRotation.w + ",";
            s = s + t.Value.localPosition.x + "," + t.Value.localPosition.y + "," + t.Value.localPosition.z;
            lines.Add(s);
        }

        System.IO.File.WriteAllLines("tpose.txt", lines.ToArray());
    }

    void recursiveTransform(Transform current, ref Dictionary<string, Transform> xforms)
    {
        xforms.Add(current.name, current);
        for (int i = 0; i < current.childCount; ++i)
        {
            recursiveTransform(current.GetChild(i), ref xforms);
        }
    }
}
