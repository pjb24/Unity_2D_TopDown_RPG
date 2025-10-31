using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> _talkData;
    Dictionary<int, Sprite> _portraitData;

    public Sprite[] _portraitArray;

    private void Awake()
    {
        _talkData = new Dictionary<int, string[]>();
        _portraitData = new Dictionary<int, Sprite>();

        GenerateData();
    }

    private void GenerateData()
    {
        // Talk Data
        // NPC A: 1000, NPC B: 2000
        // Box: 100, Desk: 200

        _talkData.Add(1000, new string[] { "�ȳ�?|0"
            , "�� ���� ó�� �Ա���?|1"
            , "�ѹ� �ѷ������� ��.|0" });
        _talkData.Add(2000, new string[] { "����.|1"
            , "�� ȣ���� ���� �Ƹ�����?|0"
            , "��� �� ȣ������ ������ ����� �������ִٰ� ��.|1" });

        _talkData.Add(100, new string[] { "����� �������ڴ�." });
        _talkData.Add(200, new string[] { "������ ����ߴ� ������ �ִ� å���̴�." });

        // Quest Talk
        _talkData.Add(10 + 1000, new string[] { "� ��|0"
            , "�� ������ ���� ������ �ִٴµ�|1"
            , "������ ȣ�� �ʿ� �絵�� �˷��ٰž�.|0" });
        _talkData.Add(11 + 2000, new string[] { "����.|1"
            , "�� ȣ���� ������ ������ �°ž�?|0"
            , "�׷� �� �� �ϳ� ���ָ� �����ٵ�...|1"
            , "�� �� ��ó�� ������ ���� �� �ֿ������� ��.|1" });

        _talkData.Add(20 + 1000, new string[] { "�絵�� ����?|1"
            , "���� �긮�� �ٴϸ� ������!|3"
            , "���߿� �絵���� �Ѹ��� �ؾ߰ھ�.|3" });
        _talkData.Add(20 + 2000, new string[] { "ã���� �� �� ������ ��.|1" });
        _talkData.Add(20 + 5000, new string[] { "��ó���� ������ ã�Ҵ�." });
        
        _talkData.Add(21 + 2000, new string[] { "��, ã���༭ ����.|2" });

        // Portrait Data
        // 0: Idle, 1: Talk, 2: Smile, 3: Angry
        _portraitData.Add(1000 + 0, _portraitArray[0]);
        _portraitData.Add(1000 + 1, _portraitArray[1]);
        _portraitData.Add(1000 + 2, _portraitArray[2]);
        _portraitData.Add(1000 + 3, _portraitArray[3]);

        _portraitData.Add(2000 + 0, _portraitArray[4]);
        _portraitData.Add(2000 + 1, _portraitArray[5]);
        _portraitData.Add(2000 + 2, _portraitArray[6]);
        _portraitData.Add(2000 + 3, _portraitArray[7]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (!_talkData.ContainsKey(id))
        {
            if (!_talkData.ContainsKey(id - (id % 10)))
            {
                // ����Ʈ �� ó�� ��縶�� ���� ��.
                // �⺻ ��縦 ������ �´�.
                return GetTalk(id - (id % 100), talkIndex); // Get First Talk
            }
            else
            {
                // �ش� ����Ʈ ���� ���� ��簡 ���� ��.
                // ����Ʈ �� ó�� ��縦 ������ �´�.
                return GetTalk(id - (id % 10), talkIndex); // Get First Quest Talk
            }
        }

        if (talkIndex == _talkData[id].Length)
        {
            return null;
        }

        return _talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return _portraitData[id + portraitIndex];
    }
}
