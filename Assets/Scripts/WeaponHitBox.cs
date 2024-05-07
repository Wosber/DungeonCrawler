using UnityEngine;
public class WeaponHitBox : MonoBehaviour
{
    public PlayerAttacks playerAttacks;
    private void OnTriggerEnter(Collider other)
    {
        playerAttacks.Hit(other);
    }
}
