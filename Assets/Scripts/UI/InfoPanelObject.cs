using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Info Panel Object", menuName = "Info Panel Object", order = 1)]
public class InfoPanelObject : ScriptableObject
{
    public Sprite aboutSprite;
    public string title;
    public string description;
}
