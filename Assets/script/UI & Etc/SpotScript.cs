using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotScript : MonoBehaviour
{
    private PlayerStat thePlayerStat;
    private Zombie theZombie;
    public GameObject SpotL;
    public bool NightStart;
    public bool MSCheck;

    public MissionComplete theMC;

    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theZombie = FindObjectOfType<Zombie>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(theMC.MStart == true)
                    MSCheck = true;
            PlayerStat.instance.def = 500;
            //theZombie.speed = 1f;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
            PlayerStat.instance.def = 5;
    }

    public void SpotActiveFalse()
    {
        if (NightStart)
        {
            PlayerStat.instance.def = 5;
        }
    }
}
