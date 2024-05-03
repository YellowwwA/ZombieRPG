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
        talkData.Add(1000, new string[] { "지금은 필요한게 없어" });

        talkData.Add(2000, new string[] { "어서 피해", "밤이 되면 좀비가 강해질거야" });

        //QuestData
        talkData.Add(10 + 1000, new string[] { "안녕?", "첫번째퀘스트 조명밟기를하렴" }); // 퀘스트번호 + npcID
        talkData.Add(11 + 1000, new string[] { "첫번째 퀘스트를 완료했구나", "고생했어" }); // 퀘스트번호 + npcID
        talkData.Add(20 + 1000, new string[] { "두번째 퀘스트 좀비 한마리 죽이기를 하렴", "기다릴게" }); // 퀘스트번호 + npcID
        talkData.Add(21 + 1000, new string[] { "두번째 퀘스트를 완료했구나", "고생고생했어" }); // 퀘스트번호 + npcID
        talkData.Add(30 + 1000, new string[] { "마지막이야", "세번째퀘스트 좀비를 죽여 반지 아이템을 가져다줘" }); // 퀘스트번호 + npcID
        talkData.Add(31 + 1000, new string[] { "세번째 퀘스트를 완료했구나", "고생고생고생했어" }); // 퀘스트번호 + npcID

    }

    public string GetTalk(int id, int talkIndex)
    {
        if(!talkData.ContainsKey(id))
        { 
            if(!talkData.ContainsKey(id - id%10))
            {
                //퀘스트 맨 처음 대사마저 없을 때
                //기본 대사를 가지고 온다.
                if (talkIndex == talkData[id - id % 100].Length)
                    return null;
                else
                    return talkData[id - id % 100][talkIndex];
            }
            else
            {
                //해당 퀘스트 진행 순서 대사가 없을 때
                //퀘스트 맨 처음 대사를 가져온다.
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
