using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    [SerializeField] private int _charPerSeconds = 10;
    [SerializeField] private GameObject _endCursor;

    private Text _messageText;
    private string _targetMessage;
    private int _index;
    private float _interval;

    private AudioSource _audioSource;
    private bool _isAnimation;
    public bool IsAnimation => _isAnimation;

    private void Awake()
    {
        _messageText = GetComponent<Text>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetMessage(string msg)
    {
        if (_isAnimation)
        {
            _messageText.text = _targetMessage;
            CancelInvoke();
            EffectEnd();
        }
        else
        {
            _targetMessage = msg;
            EffectStart();
        }
    }

    private void EffectStart()
    {
        _messageText.text = "";
        _index = 0;
        _endCursor.SetActive(false);

        //Start Animation
        _interval = 1.0f / _charPerSeconds;

        _isAnimation = true;

        Invoke("Effecting", _interval);
    }

    private void Effecting()
    {
        // End Animation
        if (_messageText.text == _targetMessage)
        {
            EffectEnd();

            return;
        }

        _messageText.text += _targetMessage[_index];

        // Sound
        if (_targetMessage[_index] != ' ' || _targetMessage[_index] != '.')
        {
            _audioSource.Play();
        }

        _index++;

        // Recursive
        Invoke("Effecting", _interval);
    }

    private void EffectEnd()
    {
        _isAnimation = false;
        _endCursor.SetActive(true);
    }
}
