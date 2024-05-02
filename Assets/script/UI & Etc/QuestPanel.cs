using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    public QuestManager theQM;

    public GameObject Quest;
    private string[] QuestNum = new string[5];


    // Update is called once per frame
    void Update()
    {

        if (theQM.CheckQuest() != null)
        {
            QuestNum[0] = theQM.CheckQuest();
        }

        Quest.transform.GetChild(2).GetComponent<Text>().text = QuestNum[0];
    }

    public void CloseTab()
    {
        Quest.SetActive(false);
    }
    public void OpenTab()
    {
        Quest.SetActive(true);
    }
}
