using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private PlayerInput _input;
    private AnimatorClipInfo[] _animInfo;
    private List<GameObject> attackedEnemys = new List<GameObject>();
    private bool attackCd = false;
    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Attack.performed += context => Attack();
    }
    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        _animInfo = _animator.GetCurrentAnimatorClipInfo(0);
        if (_animInfo[0].clip.name == "attack_1" || _animInfo[0].clip.name == "attack_2")
        {
            _playerMovement.setSpeed(_playerMovement.speed * 0.1f);
        }
        else _playerMovement.setSpeed(_playerMovement.speed);
    }
    private void Attack()
    {
        if (_animInfo[0].clip.name != "dodge" && !attackCd)
        {
            attackCd = true;
            attackedEnemys.Clear();
            _animator.SetTrigger("attack");
            if(_animator.GetInteger("attackCount") == 1) _animator.SetInteger("attackCount", 2);
            else _animator.SetInteger("attackCount", 1);
            StartCoroutine("AttackCdReset");
        }
    }
    private IEnumerator AttackCdReset()
    {
        yield return new WaitForSeconds(0.25f);
        attackCd = false;
    }
    public void Hit(Collider collider)
    {
        if (_animInfo[0].clip.name == "attack_1" || _animInfo[0].clip.name == "attack_2" && collider.gameObject.GetComponent<HealthModule>()!= null)
        {
            bool alreadyAttacked = false;
            for (int i = 0; i < attackedEnemys.Count; i++)
            {
                if (collider.gameObject == attackedEnemys[i])
                {
                    alreadyAttacked = true;
                }
            }
            if (alreadyAttacked == false)
            {
                collider.gameObject.GetComponent<HealthModule>().GetHit(damage);
                attackedEnemys.Add(collider.gameObject);
                Debug.Log("attacked: " + collider.gameObject.name);
            }
        }
    }
}
