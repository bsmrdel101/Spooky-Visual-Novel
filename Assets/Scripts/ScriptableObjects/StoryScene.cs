using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StorySceneData", menuName = "ScriptableObjects/StoryScene", order = 1)]
public class StoryScene : ScriptableObject
{
    public Sprite BackgroundImage;
    public Dialogue[] DialogueList;
}
