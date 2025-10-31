using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Animator _talkPanel;
    [SerializeField] private TalkManager _talkManager;
    [SerializeField] private QuestManager _questManager;
    [SerializeField] private TypeEffect _typeEffect;

    [SerializeField] private Image _portraitImage;
    [SerializeField] private Animator _portraitAnimator;

    [SerializeField] private GameObject _menuSet;
    [SerializeField] private Text _questText;

    [SerializeField] private GameObject _player;

    private GameObject _scanObject;

    private int _talkIndex;

    private Sprite _prevPortrait;

    private bool _isAction;
    public bool IsAction => _isAction;

    private void Start()
    {
        GameLoad();

        _menuSet.SetActive(false);

        _questText.text = _questManager.CheckQuest();
        Debug.Log(_questManager.CheckQuest());
    }

    public void Action(GameObject scanObj)
    {
        // Get Current Object
        _scanObject = scanObj;
        ObjectData objData = _scanObject.GetComponent<ObjectData>();
        Talk(objData._id, objData._isNPC);

        // Visible Talk for Action
        _talkPanel.SetBool("isShow", _isAction);
    }

    void Talk(int id, bool isNPC)
    {
        int questTalkIndex = 0;
        string talkData = "";

        // Set Talk Data
        if (_typeEffect.IsAnimation)
        {
            _typeEffect.SetMessage("");

            return;
        }
        else
        {
            questTalkIndex = _questManager.GetQuestTalkIndex(id);
            talkData = _talkManager.GetTalk(id + questTalkIndex, _talkIndex);
        }

        // End Talk
        if (talkData == null)
        {
            _isAction = false;
            _talkIndex = 0;

            _questText.text = _questManager.CheckQuest();
            Debug.Log(_questManager.CheckQuest(id));

            return;
        }

        // Continue Talk
        if (isNPC)
        {
            _typeEffect.SetMessage(talkData.Split("|")[0]);

            // Show Portrait
            _portraitImage.sprite = _talkManager.GetPortrait(id, int.Parse(talkData.Split("|")[1]));
            _portraitImage.color = new Color(1, 1, 1, 1);

            if (_prevPortrait != _portraitImage.sprite)
            {
                _portraitAnimator.SetTrigger("doEffect");
                _prevPortrait = _portraitImage.sprite;
            }
        }
        else
        {
            _typeEffect.SetMessage(talkData);

            // Hide Portrait
            _portraitImage.color = new Color(1, 1, 1, 0);
        }

        // Next Talk
        _isAction = true;
        _talkIndex++;
    }

    public void SubMenuActive()
    {
        // Sub Menu
        if (_menuSet.activeSelf)
        {
            _menuSet.SetActive(false);
        }
        else
        {
            _menuSet.SetActive(true);
        }
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", _player.transform.position.x); // _player.x
        PlayerPrefs.SetFloat("PlayerY", _player.transform.position.y); // _player.y
        PlayerPrefs.SetInt("QuestId", _questManager._questId); // Quest Id
        PlayerPrefs.SetInt("QuestActionIndex", _questManager._questActionIndex); // Quest Action Index
        PlayerPrefs.Save();

        _menuSet.SetActive(false);
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");
        
        _player.transform.position = new Vector2(x, y);
        _questManager._questId = questId;
        _questManager._questActionIndex = questActionIndex;

        _questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
