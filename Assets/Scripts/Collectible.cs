using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Collectible", menuName = "Inventory/Item", order = 1)]
public class Collectible : ScriptableObject
{
    new public string name = "New MyScriptableObject"; //suprascrie atributul name
    public string objectName = "New MyScriptableObject";
    public bool colorIsRandom = false;
    public Color thisColor = Color.white;
    public Sprite icon;
    public Sprite selectedIcon;
    public Vector3[] spawnPoints;
    public GameObject prefab;
    public int value;
}
