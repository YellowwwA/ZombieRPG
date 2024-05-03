using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        //talkData
        talkData.Add(1000, new string[] { "������ �ʿ��Ѱ� ����" });

        talkData.Add(2000, new string[] { "� ����", "���� �Ǹ� ���� �������ž�" });

        //QuestData
        talkData.Add(10 + 1000, new string[] { "�ȳ�?", "ù��°����Ʈ �����⸦�Ϸ�" }); // ����Ʈ��ȣ + npcID
        talkData.Add(11 + 1000, new string[] { "ù��° ����Ʈ�� �Ϸ��߱���", "����߾�" }); // ����Ʈ��ȣ + npcID
        talkData.Add(20 + 1000, new string[] { "�ι�° ����Ʈ ���� �Ѹ��� ���̱⸦ �Ϸ�", "��ٸ���" }); // ����Ʈ��ȣ + npcID
        talkData.Add(21 + 1000, new string[] { "�ι�° ����Ʈ�� �Ϸ��߱���", "�������߾�" }); // ����Ʈ��ȣ + npcID
        talkData.Add(30 + 1000, new string[] { "�������̾�", "����°����Ʈ ���� �׿� ���� �������� ��������" }); // ����Ʈ��ȣ + npcID
        talkData.Add(31 + 1000, new string[] { "����° ����Ʈ�� �Ϸ��߱���", "����������߾�" }); // ����Ʈ��ȣ + npcID

    }

    public string GetTalk(int id, int talkIndex)
    {
        if(!talkData.ContainsKey(id))
        { 
            if(!talkData.ContainsKey(id - id%10))
            {
                //����Ʈ �� ó�� ��縶�� ���� ��
                //�⺻ ��縦 ������ �´�.
                if (talkIndex == talkData[id - id % 100].Length)
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
            }
            else
            {
                //�ش� ����Ʈ ���� ���� ��簡 ���� ��
                //����Ʈ �� ó�� ��縦 �����´�.
                if (talkIndex == talkData[id - id % 10].Length)
                    return null;
                else
                    return talkData[id - id % 10][talkIndex];
            }
        }
        else
        {
            if (talkIndex == talkData[id].Length)
                return null;
            else
                return talkData[id][talkIndex];
        }
    }
}
