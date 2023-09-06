using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    public string Speaker;
    [TextArea(10, 100)] public string BodyText;
    public Dialogue[] DialogueOptions;
}
