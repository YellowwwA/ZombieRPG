using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Pattack : MonoBehaviourPunCallbacks, IPunObservable
{
    private PlayerStat thePlayerStat;
    private Player thePlayer;
    private NetworkManager theNet;

    public PhotonView PV;

    public static Pattack instance;
    public GameObject WeaponA;
    public GameObject WeaponB;
    public GameObject WeaponC;
    public GameObject WeaponD;
    public GameObject WeaponE;

    public GameObject prefabs_Floating_Text;
    public GameObject parent;
    public GameObject effect;
    private int bAtk = 5;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        thePlayerStat = FindObjectOfType<PlayerStat>();
        thePlayer = FindObjectOfType<Player>();
        theNet = FindObjectOfType<NetworkManager>();
    }

    private void OnTriggerEnter(Collider collision)
    {

        if ((collision.gameObject.tag == "enemy") && thePlayer.attack == true)
        {
            collision.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            //AudioManager.instance.Play(atkSound);
                
            Debug.Log("enemyhurt "+ theNet.NickNameInput.text);
            //Vector3 vector = collision.transform.position;

            //Instantiate(effect, vector, Quaternion.Euler(Vector3.zero));
            Vector3 vector = collision.transform.position;
            vector.y += 1.5f;
            Instantiate(effect, vector, Quaternion.Euler(Vector3.zero));
            effect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
            vector.y -= 0.7f;
            GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = thePlayerStat.dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.white;
            clone.GetComponent<FloatingText>().text.fontSize = 150;
            clone.GetComponent<FloatingText>().transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            clone.transform.SetParent(parent.transform);
            
        }
        
    }

    public void EquipWeapon(string index)
    {
        if(int.Parse(index) == 0)
        {
            thePlayerStat.atk = bAtk + 5;
            WeaponA.SetActive(true);
            WeaponB.SetActive(false);
            WeaponC.SetActive(false);
            WeaponD.SetActive(false);
            WeaponE.SetActive(false);
        }
        else if (int.Parse(index) == 1)
        {
            thePlayerStat.atk = bAtk + 13;
            WeaponA.SetActive(false);
            WeaponB.SetActive(true);
            WeaponC.SetActive(false);
            WeaponD.SetActive(false);
            WeaponE.SetActive(false);
        }
        else if (int.Parse(index) == 2)
        {
            thePlayerStat.atk = bAtk + 9;
            WeaponA.SetActive(false);
            WeaponB.SetActive(false);
            WeaponC.SetActive(true);
            WeaponD.SetActive(false);
            WeaponE.SetActive(false);
        }
        else if (int.Parse(index) == 3)
        {
            thePlayerStat.atk = bAtk + 11;
            WeaponA.SetActive(false);
            WeaponB.SetActive(false);
            WeaponC.SetActive(false);
            WeaponD.SetActive(true);
            WeaponE.SetActive(false);
        }
        else if (int.Parse(index) == 10)
        {
            thePlayerStat.atk = bAtk + 7;
            WeaponA.SetActive(false);
            WeaponB.SetActive(false);
            WeaponC.SetActive(false);
            WeaponD.SetActive(false);
            WeaponE.SetActive(true);
        }

    }
}
