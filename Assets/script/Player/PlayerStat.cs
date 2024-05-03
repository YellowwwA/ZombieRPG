using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStat : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerStat instance;
    public static Zombie theZombie;
    public static EnemyStat theZombieStat;


    public int character_Lv = 1;
    public int[] needExp;
    public int currentExp;

    public int hp;
    public int currentHp;
    public int mp;
    public int currentMp;

    public int atk;
    public int def;

    public int recover_hp;
    public int recover_mp;
    public GameObject effect; //회복 이펙트
    public GameObject prefabs_Floating_Text;
    public GameObject parent;

    public int dmg;

    public float time;
    private float current_time;

    public Slider hpSlider;
    //public Slider mpSlider;

    Vector3 UIdirection = new Vector3(0, 0, 100);

    Vector3 curPos;

    private bool NotHitCoroutine = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentHp = hp;
        currentMp = mp;
        theZombie = FindObjectOfType<Zombie>();
        theZombieStat = FindObjectOfType<EnemyStat>();
    }

    IEnumerator Hit(int _enemyAtk)
    {
        if (def >= _enemyAtk)
            dmg = 1;
        else
            dmg = _enemyAtk - def;

        currentHp -= dmg;

        if (currentHp <= 0)
            Debug.Log("체력 0미만, 게임 오버");
        
        yield return new WaitForSeconds(1);
        NotHitCoroutine = true;
    }
    
    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("맞음");
        if ((collision.gameObject.tag == "enemy") && theZombie.zAttack == true)
        {
            //Debug.Log("22");
            if(NotHitCoroutine)
            {
                NotHitCoroutine = false;
                StartCoroutine(Hit(theZombieStat.atk));

                Vector3 vector = collision.transform.position;
                vector.y += 0.8f;
                GameObject clone = Instantiate(prefabs_Floating_Text, vector, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = dmg.ToString();
                clone.GetComponent<FloatingText>().text.color = Color.red;
                clone.GetComponent<FloatingText>().text.fontSize = 150;
                clone.GetComponent<FloatingText>().transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                clone.transform.SetParent(parent.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.maxValue = hp;
        //mpSlider.maxValue = mp;

        hpSlider.value = currentHp;
        //mpSlider.value = currentMp;
        hpSlider.transform.LookAt(UIdirection);

        if (currentExp >= needExp[character_Lv])
        {
            character_Lv++;
            hp += character_Lv * 2;
            mp += character_Lv + 2;

            currentHp = hp;
            currentMp = mp;
            atk++;
            def++;
        }
    }

    public void RecoverHp(int num)
    {
        if (currentHp + num <= hp)
            currentHp += num;
        else
            currentHp = hp;
        Vector3 vector = transform.position;
        
        effect.transform.localScale = new Vector3(0.3f, 0.4f, 0.3f);
        Instantiate(effect, vector, Quaternion.Euler(Vector3.zero));

    }
    public void RecoverMp(int num)
    {
        if (currentMp + num <= mp)
            currentMp += num;
        else
            currentMp = mp;

    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        hpSlider.transform.LookAt(UIdirection);
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(hpSlider.value);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            hpSlider.value = (float)stream.ReceiveNext();
        }
    }
}