using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int questId;
    public int questActionIndex;
    public GameObject[] questObject;
    public Dictionary<int, QuestData> questList;

    public GameObject QuestIcon;
    public bool[] completeArr;
    public int questcompleteIndex;
    public MissionComplete theMC;
    public DialogueManager theDM;

    public string clearSound;
    private AudioManager theAudio;

    // Start is called before the first frame update
    void Awake()
    {
        theAudio = FindObjectOfType<AudioManager>();
        questList = new Dictionary<int, QuestData>();
        GenerateData();
        completeArr = new bool[questList.Count];
        questcompleteIndex = 0;
    }
    
    void GenerateData()
    {
        questList.Add(10, new QuestData("���� �Ʒ��� ���� ����", new int[] {1000, 1000})); //����Ʈ�� ������ npcID 1000
        questList.Add(20, new QuestData("���� �Ѹ��� ���̱� (0/1)", new int[] {1000, 1000}));
        questList.Add(30, new QuestData("�Ҿ���� ���� ã��", new int[] {1000, 1000}));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //ControlObject();

        //if (id == questList[questId].npcId[questActionIndex]) //���� npc����ȭ��
        //    questActionIndex++;
        //if (questActionIndex == questList[questId].npcId.Length) //���̻� ��ȭ�� npc ����
        //    NextQuest();
        theMC.MissionCheck();
        if ((questcompleteIndex < questList.Count) && (completeArr[questcompleteIndex]))
        {
            if (id == questList[questId].npcId[questActionIndex])
            {
                questActionIndex++;
            }
            if (questActionIndex == questList[questId].npcId.Length)
            {
                theAudio.Play(clearSound);
                NextQuest();
            }
        }
        if (!questList.ContainsKey(questId))
        {
            QuestIcon.SetActive(false);
            return "�������� ����Ʈ�� �����ϴ�.";
        }

        else
            return questList[questId].questName;
    }
    public string CheckQuest() //�� �Լ��� ����ؼ� ���̽�ƽ �гο� ����Ʈ ������ Ŭ�� �� ����Ʈ�� ���� �ϱ�
    {
        if (questId <= 30 && questId >=10)
            return questList[questId].questName;
        else
        {
            QuestIcon.SetActive(false);
            return "�������� ����Ʈ�� �����ϴ�.";
        }
            
    }

    void NextQuest()
    {
        //if (questList.ContainsKey(questId + 10))
        {
            questId += 10;
            questActionIndex = 0;
            questcompleteIndex++;

        }
    }
}
