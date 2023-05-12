using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tags
{
    public const string UNTAGGED = "Untagged";
    public const string ENEMY = "Enemy";
    public const string BULLET = "Bullet";
    public const string PLAYER = "Player";
}

public enum TagsEnum
{
    Untagged,
    Enemy,
    Bullet,
    Player,
    Nexus,
    ForestEnvironment,
    SlotArma,
    SlotWeaponShop,
    SlotProduct,
    Terrain,
    SlotProductSpecial,
    Death
}

public enum LayerEnum
{
    IgnoreCollider
}