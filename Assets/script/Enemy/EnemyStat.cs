using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class EnemyStat : MonoBehaviourPunCallbacks, IPunObservable
{
    public int hp;
    public int currentHp;
    public int atk;
    public int def;
    public int exp;
    public PhotonView PV;

    public GameObject healthBarBackground;
    public Image healthBarFilled;

    public GameObject Item;
    private GameObject life;

    Vector3 curPos;

    private MissionComplete theMC;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(this.gameObject);
            stream.SendNext(transform.position);
            stream.SendNext(healthBarFilled.fillAmount);
        }
        else
        {
            //this.gameObject = (GameObject)stream.ReceiveNext();
            //life = (GameObject)stream.ReceiveNext();
            curPos = (Vector3)stream.ReceiveNext();
            healthBarFilled.fillAmount = (float)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        theMC = FindObjectOfType<MissionComplete>();
        currentHp = hp;
        healthBarFilled.fillAmount = 1f;
    }

    public int Hit(int _playerAtk)
    {
        int playerAtk = _playerAtk;
        int dmg;
        if (def >= playerAtk)
            dmg = 1;
        else
            dmg = playerAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
        {
            theMC.DeadMonster++;
            StartCoroutine(ItemWaitCoroutine());
            Destroy(this.gameObject);
            PlayerStat.instance.currentExp += exp;
        }

        healthBarFilled.fillAmount = (float)currentHp / hp;
        healthBarBackground.SetActive(true);

        //Debug.Log(currentHp);
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine());
        return dmg;
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3f);
        healthBarBackground.SetActive(false);
    }
    IEnumerator ItemWaitCoroutine()
    {
        Item.transform.position = this.gameObject.transform.position + new Vector3(0,0,0); 
        Item.SetActive(true);
        yield return new WaitForSeconds(0.5f);
    }
}
