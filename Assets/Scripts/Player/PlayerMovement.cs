using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _dodgeSpeed = 30f, _jumpHeight = 60f;
    public float speed = 20f;
    private float _realySpeed;
    private float velocityY = 0;
    private Vector2 _dodgeDir = new Vector2(0,0);
    private PlayerInput _input;
    private CharacterController _controller;
    private Animator _animator;
    private AnimatorClipInfo[] _animInfo;
    private void Awake()
    {
        _input = new PlayerInput();
        _input.Player.Jump.performed += context => Jump();
        _input.Player.Dodge.performed += context => Dodge();
    }
    private void OnEnable()
    {
        _input.Enable();
    }
    private void OnDisable()
    {
        _input.Disable();
    }
    private void Start()
    {
        _realySpeed = speed;
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _animInfo = _animator.GetCurrentAnimatorClipInfo(0);
    }
    private void FixedUpdate()
    {
        Vector2 direction = _input.Player.Move.ReadValue<Vector2>();

        if (direction != Vector2.zero)
        {
            _animator.SetBool("isWalking", true);
        }
        else  _animator.SetBool("isWalking", false);
        bool isDodging = _animInfo[0].clip.name == "dodge";
        if (!isDodging)
        {
            _controller.Move(new Vector3(direction.x, 0, direction.y) * _realySpeed * Time.deltaTime);
            if (direction != Vector2.zero)
            {
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg, 0);
                
            }
        }
        else  _controller.Move(new Vector3(_dodgeDir.x, 0, _dodgeDir.y) * _dodgeSpeed * Time.deltaTime);
        if (_controller.isGrounded == false) velocityY -= 3;
        _controller.Move(new Vector3(0, velocityY * Time.deltaTime, 0));
    }
    private void Dodge()
    {
        _dodgeDir = _input.Player.Move.ReadValue<Vector2>();
        if (_dodgeDir == Vector2.zero) _dodgeDir = new Vector2(transform.forward.x, transform.forward.z);
        _animator.SetTrigger("dodge");
        Debug.Log("dodged");
    }
    public void setSpeed(float speed)
    {
        _realySpeed = speed;
    }
    private void Jump()
    {
        if (_animInfo[0].clip.name != "dodge" && _controller.isGrounded)
        {
            _animator.SetTrigger("jump");
            velocityY = _jumpHeight;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            velocityY = 0;
        }
    }
}
