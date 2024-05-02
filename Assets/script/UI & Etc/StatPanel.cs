using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatPanel : MonoBehaviour
{
    public NetworkManager theNet;

    public GameObject Stat;
    private string[] StatNum = new string[5];

    // Start is called before the first frame update
    void Start()
    {
        theNet = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(PlayerStat.instance != null)
        {
            StatNum[0] = (PlayerStat.instance.character_Lv).ToString();
            StatNum[1] = (PlayerStat.instance.currentExp).ToString();
            StatNum[2] = (PlayerStat.instance.currentHp).ToString();
            StatNum[3] = (PlayerStat.instance.atk).ToString();
            StatNum[4] = (PlayerStat.instance.def).ToString();
        }

        Stat.transform.GetChild(2).GetComponent<Text>().text = theNet.NickNameInput.text;
        Stat.transform.GetChild(4).GetComponent<Text>().text = StatNum[0] + "\n" + StatNum[1] + "\n" + StatNum[2] + "\n" + StatNum[3] + "\n" + StatNum[4] + "\n";
    }

    public void CloseTab()
    {
        Stat.SetActive(false);
    }
    public void OpenTab()
    {
        Stat.SetActive(true);
    }
}
