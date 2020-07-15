using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/EnemySettings", fileName ="EnemySettings")]
public class EnemyScriptableObject : BasePlayerScriptableObject
{
    [Header("Enemy Settings")]
    public float hitDistance = 2f;
    public int damage = 1;

    [Header("Animator Settings")]
    public int hitAnimationsCount = 3;
    public string hitInt = "";
    public string hitBool = "";
    public int deadAnimationsCount = 2;
    public string deadInt = "";
    public string deadBool = "";
    public float switchHitAnimationTimer = 2f;
}
