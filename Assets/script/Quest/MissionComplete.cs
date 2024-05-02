using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionComplete : MonoBehaviour
{
    public QuestManager theQM;
    public SpotScript theSpot;
    public bool MStart;
    public int DeadMonster;
    public int itemID;
    
    void Start()
    {
        DeadMonster = 0;
        itemID = 0;
    }

    public void MissionCheck()
    {
        if(MStart)
        {
            switch (theQM.questcompleteIndex)
            {
                case 0:
                    if (theSpot.MSCheck)//�̼ǳ���
                    {
                        MStart = false;
                        theSpot.MSCheck = false;
                        theQM.completeArr[theQM.questcompleteIndex] = true;
                        DeadMonster = 0; //�ι�°�̼� ���� �� ������ ���� ���� �� �ʱ�ȭ
                    }
                    break;
                case 1:
                    if (DeadMonster >= 1)//�̼ǳ���
                    {
                        MStart = false;
                        theSpot.MSCheck = false;
                        theQM.completeArr[theQM.questcompleteIndex] = true;
                    }

                    break;
                case 2:
                    if (itemID == 11)//�̼ǳ���
                    {
                        //MStart = false;
                        theSpot.MSCheck = false;
                        theQM.completeArr[theQM.questcompleteIndex] = true;
                    }
                    break;
                case 3:
                    //theQM.completeArr[theQM.questcompleteIndex] = true;
                    break;
            }
        }


    }

}
