using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class item : MonoBehaviourPunCallbacks, IPunObservable
{

    public int itemID;
    public int _count;
    private GameManager theGM;
    private MissionComplete theMC;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    private void Start()
    {
        theGM = FindObjectOfType<GameManager>();
        theMC = FindObjectOfType<MissionComplete>();
        //itemID = Random.Range(4,6);
        //_count = 1;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            theMC.itemID = itemID;
            theGM.GetItem(itemID, _count);
            Destroy(this.gameObject);
        }
    }
}
