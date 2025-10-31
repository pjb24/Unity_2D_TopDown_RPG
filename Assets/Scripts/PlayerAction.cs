using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private float _speed = 5;

    private Rigidbody2D _rigid;
    private Animator _animator;

    private Vector2 _input;
    private bool _isHorizonMove;

    private bool _isHorizontalInputDown = false;
    private bool _isVerticalInputDown = false;

    private bool _isRightButtonHold = false;
    private bool _isLeftButtonHold = false;
    private bool _isUpButtonHold = false;
    private bool _isDownButtonHold = false;

    private Vector2 _dirVec;

    private GameObject _scanObject;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Move
        Vector2 moveVec = _isHorizonMove ? new Vector2(_input.x, 0) : new Vector2(0, _input.y);
        _rigid.linearVelocity = moveVec * _speed;

        // Animation
        _animator.SetBool("isChange", false);
        if (_animator.GetInteger("hAxisRaw") != moveVec.x)
        {
            _animator.SetBool("isChange", true);
            _animator.SetInteger("hAxisRaw", (int)moveVec.x);
        }
        else if (_animator.GetInteger("vAxisRaw") != moveVec.y)
        {
            _animator.SetBool("isChange", true);
            _animator.SetInteger("vAxisRaw", (int)moveVec.y);
        }

        // Ray
        Debug.DrawRay(_rigid.position, _dirVec * 0.7f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(_rigid.position, _dirVec, 0.7f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null)
        {
            _scanObject = rayHit.collider.gameObject;
        }
        else
        {
            _scanObject = null;
        }
    }

    public void OnMove(InputValue value)
    {
        Debug.Log("OnMove Called");

        // Move Value
        _input = value.Get<Vector2>();

        // Check Button Down & Up
        bool horizontalDown = false;
        bool verticalDown = false;
        bool horizontalUp = false;
        bool verticalUp = false;

        if (_input.x != 0)
        {
            horizontalDown = true;
        }
        if (_input.y != 0)
        {
            verticalDown = true;
        }

        if (_input.x == 0)
        {
            horizontalUp = true;
        }
        if (_input.y == 0)
        {
            verticalUp = true;
        }

        // Check Horizontal Move
        if (horizontalDown || verticalUp)
        {
            _isHorizonMove = true;
        }
        else if (verticalDown || horizontalUp)
        {
            _isHorizonMove = false;
        }
    }

    public void OnMoveRight(InputValue value)
    {
        if (value.isPressed)
        {
            _input.x = 1;
            _isHorizontalInputDown = true;
            _isRightButtonHold = true;
        }

        if (!value.isPressed)
        {
            _input.x = 0;
            _isRightButtonHold = false;
        }

        CheckButtonInput();
    }
    public void OnMoveLeft(InputValue value)
    {
        if (value.isPressed)
        {
            _input.x = -1;
            _isHorizontalInputDown = true;
            _isLeftButtonHold = true;
        }

        if (!value.isPressed)
        {
            _input.x = 0;
            _isLeftButtonHold = false;
        }

        CheckButtonInput();
    }
    public void OnMoveUp(InputValue value)
    {
        if (value.isPressed)
        {
            _input.y = 1;
            _isVerticalInputDown = true;
            _isUpButtonHold = true;
        }

        if (!value.isPressed)
        {
            _input.y = 0;
            _isUpButtonHold = false;
        }

        CheckButtonInput();
    }
    public void OnMoveDown(InputValue value)
    {
        if (value.isPressed)
        {
            _input.y = -1;
            _isVerticalInputDown = true;
            _isDownButtonHold = true;
        }

        if (!value.isPressed)
        {
            _input.y = 0;
            _isDownButtonHold = false;
        }

        CheckButtonInput();
    }

    private void CheckButtonInput()
    {
        // Hold Key Move
        if (_isHorizontalInputDown == false && _isVerticalInputDown == false)
        {
            if (_isRightButtonHold && !_isLeftButtonHold)
            {
                _input.x = 1;
                _isHorizontalInputDown = true;
            }
            else if (_isLeftButtonHold && !_isRightButtonHold)
            {
                _input.x = -1;
                _isHorizontalInputDown = true;
            }

            if (_isUpButtonHold && !_isDownButtonHold)
            {
                _input.y = 1;
                _isVerticalInputDown = true;
            }
            else if (_isDownButtonHold && !_isUpButtonHold)
            {
                _input.y = -1;
                _isVerticalInputDown = true;
            }
        }

        if (_gameManager.IsAction)
        {
            _input.x = 0;
            _input.y = 0;
        }

        // Direction
        if (_isVerticalInputDown && _input.y == 1)
        {
            _dirVec = Vector2.up;
        }
        else if (_isVerticalInputDown && _input.y == -1)
        {
            _dirVec = Vector2.down;
        }
        else if (_isHorizontalInputDown && _input.x == -1)
        {
            _dirVec = Vector2.left;
        }
        else if (_isHorizontalInputDown && _input.x == 1)
        {
            _dirVec = Vector2.right;
        }

        // Check Horizontal Move
        if (_isHorizontalInputDown)
        {
            _isHorizonMove = true;
        }
        else if (_isVerticalInputDown)
        {
            _isHorizonMove = false;
        }

        _isHorizontalInputDown = false;
        _isVerticalInputDown = false;
    }

    public void OnScan(InputValue value)
    {
        // Scan Object
        Scan();
    }

    void Scan()
    {
        // Scan Object
        if (_scanObject != null)
        {
            //Debug.Log("This is : " + _scanObject.name);
            _gameManager.Action(_scanObject);
        }
    }

    public void ButtonDown(string type)
    {
        switch (type)
        {
            case "U":
                {
                    _input.y = 1;
                    _isVerticalInputDown = true;
                    _isUpButtonHold = true;
                    break;
                }
            case "D":
                {
                    _input.y = -1;
                    _isVerticalInputDown = true;
                    _isDownButtonHold = true;
                    break;
                }
            case "L":
                {
                    _input.x = -1;
                    _isHorizontalInputDown = true;
                    _isLeftButtonHold = true;
                    break;
                }
            case "R":
                {
                    _input.x = 1;
                    _isHorizontalInputDown = true;
                    _isRightButtonHold = true;
                    break;
                }
            case "A":
                {
                    Scan();
                    break;
                }
            case "C":
                {
                    _gameManager.SubMenuActive();
                    break;
                }
            default:
                break;
        }

        CheckButtonInput();
    }

    public void ButtonUp(string type)
    {
        switch (type)
        {
            case "U":
                {
                    _input.y = 0;
                    _isUpButtonHold = false;
                    break;
                }
            case "D":
                {
                    _input.y = 0;
                    _isDownButtonHold = false;
                    break;
                }
            case "L":
                {
                    _input.x = 0;
                    _isLeftButtonHold = false;
                    break;
                }
            case "R":
                {
                    _input.x = 0;
                    _isRightButtonHold = false;
                    break;
                }
            default:
                break;
        }

        CheckButtonInput();
    }

    public void OnEscape(InputValue value)
    {
        // Sub Menu
        _gameManager.SubMenuActive();
    }
}
