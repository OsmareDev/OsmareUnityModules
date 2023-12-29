using UnityEngine;

[CreateAssetMenu(menuName = "Osmare/WeaponStats")]
public class WeaponStats : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float fireRate;
    public float bulletSpeed;
    public float range;

    public float shootAngle;
    public int nBulletsPerShoot;
    public bool randomPositionInRange;
}