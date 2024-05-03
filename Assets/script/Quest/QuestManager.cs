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
        questList.Add(10, new QuestData("조명 아래로 갔다 오기", new int[] {1000, 1000})); //퀘스트를 수행할 npcID 1000
        questList.Add(20, new QuestData("좀비 한마리 죽이기 (0/1)", new int[] {1000, 1000}));
        questList.Add(30, new QuestData("잃어버린 반지 찾기", new int[] {1000, 1000}));
    }

    public int GetQuestTalkIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string CheckQuest(int id)
    {
        //ControlObject();

        //if (id == questList[questId].npcId[questActionIndex]) //다음 npc랑대화해
        //    questActionIndex++;
        //if (questActionIndex == questList[questId].npcId.Length) //더이상 대화할 npc 없음
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
            return "진행중인 퀘스트가 없습니다.";
        }

        else
            return questList[questId].questName;
    }
    public string CheckQuest() //이 함수를 사용해서 조이스틱 패널에 퀘스트 아이콘 클릭 시 퀘스트들 나열 하기
    {
        if (questId <= 30 && questId >=10)
            return questList[questId].questName;
        else
        {
            QuestIcon.SetActive(false);
            return "진행중인 퀘스트가 없습니다.";
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
