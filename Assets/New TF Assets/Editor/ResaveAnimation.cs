using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ResaveAnimation : EditorWindow
{
    AnimationClip clip;

    [MenuItem("Window/Anim Saver")]
    public static void ShowWindow()
	{
        GetWindow<ResaveAnimation>("Resave Animations");
	}

    void OnGUI()
    {
        clip = (AnimationClip)EditorGUILayout.ObjectField(clip, typeof(AnimationClip));
        if(GUILayout.Button("Resave!"))
		{
            //Debug.Log(AssetDatabase.ExtractAsset(clip, "Assets/Resaved/" + clip.name + ".anim"));
            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(clip), "Assets/Resaved/" + clip.name + ".anim");
        }
    }
}
