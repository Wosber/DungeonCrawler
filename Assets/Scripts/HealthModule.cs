using UnityEngine;
public class HealthModule : MonoBehaviour
{
    [SerializeField] private bool _haveHitAnim = false;
    [SerializeField] private int hp = 6;
    private int maxHp;
    private bool godMode = false;
    private Animator _animator;
    void Start()
    {
        if (_haveHitAnim) _animator = GetComponent<Animator>();
        maxHp = hp;
    }
    public void GetHit(int damage)
    {
        if (!godMode)
        {
            if (_haveHitAnim) _animator.SetTrigger("hit");
            hp -= damage;
            if (hp <= 0) Destroy(gameObject);
        }
    }
    public void GetHeal(int healAmount)
    {
        hp += healAmount;
        if (hp > maxHp) hp = maxHp;
    }
    public void GodModeOn()
    {
        godMode = true;
    }
    public void GodModeOff()
    {
        godMode = false;
    }
}
