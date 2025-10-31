using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int _questId;
    public int _questActionIndex;
    public GameObject[] _questObject;

    Dictionary<int, QuestData> _questList;

    private void Awake()
    {
        _questList = new Dictionary<int, QuestData>();

        GenerateData();
    }

    private void GenerateData()
    {
        _questList.Add(10, new QuestData("���� ������ ��ȭ�ϱ�."
            , new int[] { 1000, 2000 }));
        _questList.Add(20, new QuestData("�絵�� ���� ã���ֱ�."
            , new int[] { 5000, 2000 }));
        _questList.Add(30, new QuestData("����Ʈ �� Ŭ����!"
            , new int[] { 0 }));
    }

    public int GetQuestTalkIndex(int id)
    {
        return _questId + _questActionIndex;
    }

    public string CheckQuest(int id)
    {
        // Next Talk Target
        if (id == _questList[_questId]._npcId[_questActionIndex])
        {
            _questActionIndex++;
        }

        // Control Quest Object
        ControlObject();

        // Talk Complete & Next Quest
        if (_questActionIndex == _questList[_questId]._npcId.Length)
        {
            NextQuest();
        }
        
        // Quest Name
        return _questList[_questId]._questName;
    }

    public string CheckQuest()
    {
        // Quest Name
        return _questList[_questId]._questName;
    }

    private void NextQuest()
    {
        _questId += 10;
        _questActionIndex = 0;
    }

    public void ControlObject()
    {
        switch (_questId)
        {
            case 10:
                {
                    if (_questActionIndex == 2)
                    {
                        _questObject[0].SetActive(true);
                    }
                    break;
                }
            case 20:
                {
                    if (_questActionIndex == 0)
                    {
                        _questObject[0].SetActive(true);
                    }
                    if (_questActionIndex == 1)
                    {
                        _questObject[0].SetActive(false);
                    }
                    break;
                }
            default:
                break;
        }
    }
}
