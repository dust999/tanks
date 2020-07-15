using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PlayerSettingsAsset", fileName ="PlayerSettings")]
public class PlayerScriptableObject : BasePlayerScriptableObject
{
    [Header("Weapon Settings")]
    public BaseWeaponScriptableObject [] weapons = null;
}
