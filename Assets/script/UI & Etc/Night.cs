using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour
{
    private EnemyStat theES;
    private Zombie theZombie;
    private SpotScript theSpotScript;

    public GameObject NightScreen;
    public GameObject MorningScreen;
    public GameObject SpotL;
    private bool Nn = false;
    private bool Nf = true;
    void Start()
    {
        theES = FindObjectOfType<EnemyStat>();
        theZombie = FindObjectOfType<Zombie>();
        theSpotScript = FindObjectOfType<SpotScript>();
    }

    void Update()
    {
        if(Nn == true)
            StartCoroutine(NightOn());
        else if(Nf == true)
            StartCoroutine(NightOff());
    }

    IEnumerator NightOn()
    {
        Nn = false;
        Nf = false;
        theES.hp = 100;
        theES.currentHp = 100;
        theES.atk = 50;
        theES.def = 10;
        theZombie.speed = 0.7f;
        theSpotScript.gameObject.SetActive(true);
        theSpotScript.NightStart = true;
        NightScreen.SetActive(true);
        MorningScreen.SetActive(false);
        SpotL.SetActive(true);
        yield return new WaitForSeconds(10f);
        Nn = false;
        Nf = true;
    }
    IEnumerator NightOff()
    {
        Nf = false;
        Nn = false;
        theES.hp = 20;
        theES.currentHp = 20;
        theES.atk = 10;
        theES.def = 2;
        theZombie.speed = 0.2f;
        theSpotScript.SpotActiveFalse();
        theSpotScript.gameObject.SetActive(false);
        NightScreen.SetActive(false);
        MorningScreen.SetActive(true);
        SpotL.SetActive(false);
        yield return new WaitForSeconds(10f);
        Nf = false;
        Nn = true;
    }

}
