using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    public int NPCID;
    public bool MSCSign;
    public bool MSCS;

    void Start()
    {
        //Debug.Log(questManager.CheckQuest());

    }

    public void Action()
    {
        Talk(NPCID, false);
        talkPanel.SetActive(isAction);
    }

    void Talk(int id, bool isNpc)
    {
        if(questManager.questActionIndex == 0)
            questManager.CheckQuest(id);

        //Set Talk Data
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        if (talkData == null)//End Talk
        {
            isAction = false;
            talkIndex = 0;

            questManager.CheckQuest(id);
            if (questTalkIndex < 40)
                Debug.Log(questManager.CheckQuest());
            return;
        }
        
        talkText.text = talkData;
        isAction = true;
        talkIndex++;
    }
}
