using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Character", menuName = "Assets/Character", order = 2)]
public class CharacterSheet : ScriptableObject
{
    public string characterName;
    public Sprite baseSprite;
    public Sprite[] spritesArray;
    public Color nameColor = new Color(147, 61, 95, 255);
    public TMP_FontAsset speakingFont;
}
