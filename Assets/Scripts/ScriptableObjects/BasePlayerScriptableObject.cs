using UnityEngine;

public class BasePlayerScriptableObject : ScriptableObject
{
    [Header("Prefab Settings")]
    public BasePlayer prefab = null;

    [Header("Player Settings")]
    public float speed = 0.1f;
    public float rotateSpeed = 2f;
    public int health = 100;
    public int arrmor = 100;
}
