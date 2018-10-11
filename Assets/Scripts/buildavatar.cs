/*
 * Lauren Cairco Dukes
 * January 2014
 * 
 * BuildAvatar.cs
 * Enables realtime loading of a humanoid avatar into a Unity project and automatically rigs it for the
 * Mecanim system given certain parameters.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// class InternalTransform
// can't instantiate a Transform without a GameObject
// so instead we use this local class where we can keep track of position, rotation, and scale
// to later assign to a gmaeobject
struct InternalTransform
{
    public InternalTransform(Transform t){
        localPosition = new Vector3(t.localPosition.x, t.localPosition.y, t.localPosition.z);
        localRotation = new Quaternion(t.localRotation.x, t.localRotation.y, t.localRotation.z, t.localRotation.w);
        localScale = new Vector3(t.localScale.x, t.localScale.y, t.localScale.z);
    }
    public Vector3 localPosition;
    public Quaternion localRotation;
    public Vector3 localScale;
}

public class BuildAvatar : MonoBehaviour {

    //the createCharacter function takes in the parameters to import in a character from the Resources folder
    // parameters:
    // string assetName: the file name (minus the extension) of the rigged model for the humanoid character you want to import in (required)
    // the rest of the parameters are optional, but recommended
    //  string skeletonfile: text file name (minus the extension) in the Resources folder that describes a mapping from your bone names to Unity's bone names
    //  string tposefile: text file name (minus extension) that contains the transforms that put your character in tpose. can use AvatarUtils to do this
    //  bool useLocalPositionAndScale: if you are using multiple characters with the same rig but potentially different sizes, you'll want to set this to true
    //                              effectively this uses the tposefile rotations only, ignoring the position and rotation
    //  string animatorfile: file name of an AnimatorController in the Resources folder to add to your character upon instantiation
    GameObject createCharacter(string assetName, string skeletonfile = "", string tposefile = "", string animatorfile = "", bool useLocalPositionAndScale = true)
    {
        //Import in a FBX file from resources and set position
        GameObject character = (GameObject)Instantiate(Resources.Load(assetName));
        GameObject root = GameObject.Find(assetName);
        if (character == null)
        {
            Debug.LogError("File " + assetName + " not loaded in from resources correctly. Cannot make character.");
            return null;
        }

        //set up the description for the humanoid, given parameters
        HumanDescription desc = setupHumanDescription(character, skeletonfile: skeletonfile, tposefile: tposefile, useLocalPositionAndScale: useLocalPositionAndScale);

        //if the gameobject we created for our character doesn't have an animator component, add one
        if (character.GetComponent<Animator>() == null)
        {
            character.AddComponent<Animator>();
        }

        //create the avatar using that gameobject and the human description we created
        Avatar a = AvatarBuilder.BuildHumanAvatar(root, desc);
        if (a == null)
        {
            Debug.LogError("Something went wrong building the avatar from the provided character and human description. Check your error log for clues.");
            return null;
        }
        character.GetComponent<Animator>().avatar = a;

        //if there was a file specified to use as its animator controller, load it in from resources
        if (animatorfile != "")
        {
            RuntimeAnimatorController c = (RuntimeAnimatorController)Instantiate(Resources.Load(animatorfile));
            if (c == null)
            {
                Debug.LogError("File " + assetName + " not loaded in from resources correctly. Cannot add animation controller.");
                return null;
            }
            character.GetComponent<Animator>().runtimeAnimatorController = c;
        }

        return character;
    }

    /*
     this function creates a HumanDescription given the parameters for your character
       required:
           GameObject character: the GameObject that will be your character
       optional:
      string skeletonfile: text file name (minus the extension) in the Resources folder that describes a mapping from your bone names to Unity's bone names
      string tposefile: text file name (minus extension) that contains the transforms that put your character in tpose. can use AvatarUtils to do this
      bool useLocalPositionAndScale: if you are using multiple characters with the same rig but potentially different sizes, you'll want to set this to true
                                     effectively this uses the tposefile rotations only, ignoring the position and rotation
     */
    private HumanDescription setupHumanDescription(GameObject character, string skeletonfile = "", string tposefile = "", bool useLocalPositionAndScale = true)
    {
        HumanDescription desc = new HumanDescription();

        //first load in the bone mapping from the file, if that file is provided
        List<HumanBone> mappedBones = new List<HumanBone>();
        List<SkeletonBone> allBones = new List<SkeletonBone>();
        if (skeletonfile != "")
        {
            loadSkeletonCorrespondences(character, skeletonfile, ref mappedBones, ref allBones);
        }

        //then load in the tpose mapping that transforms your character's skeleton to the tpose, if that file is provided
        Dictionary<string, InternalTransform> tposeTransforms = new Dictionary<string, InternalTransform>();
        if (tposefile != "")
        {
            loadTposeMapping(tposefile, ref tposeTransforms);
        }
        
        //Set up the parameters for the human description

        //the human bone array is the list we've already composed using the loadSkeletonCorrespondences function
        HumanBone[] human = mappedBones.ToArray();

        //we have to do some extra work for the skeleton bones since this is where the tpose transform is stored and we haven't
        //yet read that in from a file (if it exists)
        SkeletonBone[] sk = allBones.ToArray();

        //for all the bones in the skeleton
        for (int i = 0; i < sk.Length; i++)
        {

            string mappedName = "";
            //When Unity loads in the character from file, it automatically affixes a "(Clone)" on the end of the filename
            //the way my skeletons are set up, the parent of the hip bone is named the name of the file
            //but because each character's name is different but my skeleton file stays the same, in the mapping file
            //I call it "root", while in the hierarchy it's called whatever the filename is
            //this little if-statement sequence resolves that
            if (sk[i].name == character.name.Substring(0,character.name.IndexOf('(')))
            {
                mappedName = "root";
            }
            else
            {
                mappedName = sk[i].name;
            }

            //we need to add in the transform information to make it into t-pose
            //get the transforms that came from the file
            InternalTransform fileTransform;
            if (tposeTransforms.ContainsKey(mappedName))
            {
                fileTransform = tposeTransforms[mappedName];
            }
            //warn if there was a tpose file but it did not contain any transform
            else
            {
                if (tposefile != "")
                {
                    Debug.LogWarning("Warning: tpose mapping for bone " + sk[i].name + " not found in tpose file " + tposefile + ". Defaulting back to the local transforms.");
                }
                fileTransform = new InternalTransform(recursiveSearch(sk[i].name, character.transform));
            }

            //get the transforms that come from the local skeleton
            Transform foundTransform = recursiveSearch(sk[i].name, character.transform);
            InternalTransform localTransform;
            //if we didn't find a transform for that bone name
            if (foundTransform == null)
            {
                //warn and default to 0,0,0
                Debug.Log("Did not find bone transform " + sk[i].name + " in hierarchy. Defaulting to empty transform.");
                localTransform = new InternalTransform();
            }
            else
            {
                //otherwise assign that transform that was found in the gameobject hierarchy
                localTransform = new InternalTransform(foundTransform);
            }


            ///once you've found the transforms go ahead and assign them to the skeleton bone attributes
            /// rotation always comes from the tpose file (which of course falls back to the local rotation if not set)
            sk[i].rotation = new Quaternion(fileTransform.localRotation.x, fileTransform.localRotation.y, fileTransform.localRotation.z, fileTransform.localRotation.w);

            //if the user wanted to use the existing local scale and position do that
            if (useLocalPositionAndScale)
            {
                sk[i].position = new Vector3(localTransform.localPosition.x, localTransform.localPosition.y, localTransform.localPosition.z);
                sk[i].scale = new Vector3(localTransform.localScale.x, localTransform.localScale.y, localTransform.localScale.z);
            }
            //otherwise grab what was loaded in from the tpose file
            else
            {
                sk[i].position = new Vector3(fileTransform.localPosition.x, fileTransform.localPosition.y, fileTransform.localPosition.z);
                sk[i].scale = new Vector3(fileTransform.localScale.x, fileTransform.localScale.y, fileTransform.localScale.z);
            }

            /*
             *you could uncomment this to set the character to t-pose
            recursiveSearch(sk[i].name, character.transform).localPosition = sk[i].position;
            recursiveSearch(sk[i].name, character.transform).localRotation = sk[i].rotation;
            recursiveSearch(sk[i].name, character.transform).localScale = sk[i].scale;
             * */

        }

        //set the bone arrays right
        desc.human = human;
        desc.skeleton = sk;

        //set the default values for the rest of the human descriptor parameters
        desc.upperArmTwist = 0.5f;
        desc.lowerArmTwist = 0.5f;
        desc.upperLegTwist = 0.5f;
        desc.lowerLegTwist = 0.5f;
        desc.armStretch = 0.05f;
        desc.legStretch = 0.05f;
        desc.feetSpacing = 0.0f;

        //return the human description
        return desc;
    }

    /*
     * our goal is to return a mapping from the unity bone names to our file bone names, along with a list of any extra bones that we want to include

            GameObject character: the GameObject that will be your character
            string skeletonfile: text file name (minus the extension) in the Resources folder that describes a mapping from your bone names to Unity's bone names
            List<HumanBone> mappedBones: pass in an already-created (but empty) list of HumanBones to populate. This array will hold those bones in your character
     *                                      that are mapped to a standard Unity bone
     *      List<SkeletonBone> allBones: pass in an already-created (but empty) list of SkeletonBones to populate. This array will hold the bones mapped to standard bones
     *                                      in your character, plus any additional bones you specified in the file that you want to keep around
     */

    void loadSkeletonCorrespondences(GameObject character, string skeletonFile, ref List<HumanBone> mappedBones, ref List<SkeletonBone> allBones){
        //load in the skeleton definition file from an asset
        TextAsset skel = (TextAsset)Resources.Load(skeletonFile);

        //check to make sure it was loaded in okay
        if (skel == null)
        {
            Debug.LogError("File " + skeletonFile + " not loaded in from resources correctly. Cannot make skeleton definition.");
        }
        else
        {
            //split the file into an array of each line
            string[] lines = skel.text.Split(new char[] { '\n' });

            //each line has the following format:
            //unitybonename, modelbonename
            //for those whose bone name is "none", these are not mapped to a standard bone but you wanted to keep them around

            //for eavery line
            for (int i = 0; i < lines.Length; i++)
            {
                //if it doesn't have a comma, skip it
                if (lines[i].IndexOf(',') == -1)
                {
                    Debug.LogWarning("Skipping line " + i + " of skeleton file " + skeletonFile + " because there is no comma in that line.");
                }
                else                
                {
                    //split the line by comma
                    string[] bones = lines[i].Split(new char[] { ',' });

                    //if the bone has a mapping, add it to the mappedBones list as a HumanBone
                    if (bones[0].Trim() != "none")
                    {
                        //otherwise add it to the mapping
                        HumanBone b = new HumanBone();
                        b.boneName = bones[1].Trim();
                        b.humanName = bones[0].Trim();
                        //set the bone limit to use default values
                        b.limit.useDefaultValues = true;
                        mappedBones.Add(b);
                    }

                    //regardless of whether there's a mapping or not, add it to the SkeletonBone list of all the bones
                    SkeletonBone sb = new SkeletonBone();
                    //again, because of the format of my skeleton files, each parent of the hip bone is different from model to model
                    //this if-statement cleans that up for me
                    if (bones[1].Trim() != "root")
                    {
                        sb.name = bones[1].Trim();
                    }
                    else
                    {
                        sb.name = character.name.Substring(0, character.name.IndexOf('('));
                    }
                    allBones.Add(sb);
                }
            }
        }
    }

    /*
     * loads in the transforms for each bone from a text file to put them into a t-pose
     * 
     */

    void loadTposeMapping(string filename, ref Dictionary<string, InternalTransform> tpose)
    {
        //load in the tpose file from an asset
        TextAsset posetext = (TextAsset)Resources.Load(filename);

        //check to make sure it was loaded in okay
        if (posetext == null)
        {
            Debug.LogError("File " + filename + " not loaded in from resources correctly. Cannot make tpose transforms.");
            return;
        }


        string[] lines = posetext.text.Split(new char[] { '\n' });

        //each line is in the following format:
        //bonename, localX, localY, localZ, localQuatX, localQuatY, localQuatZ, localQuatW, localScaleX, localScaleY, localScaleZ
        for (int i = 0; i < lines.Length; i++)
        {
            //if it's not a valid line, skip
            if (lines[i].IndexOf(',') == -1)
            {
                Debug.LogWarning("Skipping line " + i + " of tpose file " + filename + " because there is no comma in that line.");
            }
            else if(lines[i].Split(new char[] { ',' }).Length != 11){
                Debug.LogWarning("Skipping line " + i + " of tpose file " + filename + " because there are not 11 values.");
            }
            //otherwise
            else
            {
                //split by commas
                string[] xforms = lines[i].Split(new char[] { ',' });
                string boneName = xforms[0].Trim();
                //parse it all into a transform
                InternalTransform t;
                t.localPosition = new Vector3(float.Parse(xforms[1]), float.Parse(xforms[2]), float.Parse(xforms[3]));
                t.localRotation = new Quaternion(float.Parse(xforms[4]), float.Parse(xforms[5]), float.Parse(xforms[6]), float.Parse(xforms[7]));
                t.localScale = new Vector3(float.Parse(xforms[8]), float.Parse(xforms[9]), float.Parse(xforms[10]));
                tpose.Add(boneName, t);
            }
        }
    }

    //recursively searches the children of the transform passed in for a child with the name passed in
    Transform recursiveSearch(string name, Transform current)
    {
        if (current.name == name)
        {
            return current;
        }
        else
        {
            for (int i = 0; i < current.childCount; ++i)
            {
                Transform found = recursiveSearch(name, current.GetChild(i));

                if (found != null)
                {
                    return found;
                }
            }
        }

        return null;
    }

    // Use this for initialization
    void Start()
    {
        //create the character
        GameObject character = createCharacter("basemodel_clothed", skeletonfile: "makehumanskeleton", tposefile: "makehumantpose", animatorfile: "Simple");

        //put it where I can see it
        character.transform.position = new Vector3(0, 0, 0);
        character.transform.localScale = new Vector3(10, 10, 10);
	}  

}
