using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.AI;

public class Zombie : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody RB;
    public Animator AN;
    public PhotonView PV;
    public Image HealthImage;
    public Image HealthBarImage;
    public Text NickNameText;

    public float speed = 0.2f;
    protected Vector3 vector;

    private bool notCoroutine = false;
   // public Queue<string> queue;
    public BoxCollider boxCollider;
    public LayerMask layerMask;

    public float inter_MoveWaitTime; // 몬스터의 공격 쿨타임
    private float current_interMWT;

    private int random_int;
    private string direction;

    Vector3 moveVec;
    Vector3 dir;
    private bool walk = true;
    Vector3 UIdirection = new Vector3(0, 0, 100);

    public float hp;

    public float attackDelay;//공격 유예
    private Vector3 playerPos;
    private bool notMove = false;

    private Player thePlayer;
    public bool zAttack;

    public GameObject target = null;
    public GameObject[] Players;
    float shortestDistance = float.MaxValue;
    GameObject closestPlayer = null;

    public string walkSound;
    public string attackSound;
    private AudioManager theAudio;


    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        NickNameText.transform.LookAt(UIdirection);
        HealthBarImage.transform.LookAt(UIdirection);
        HealthImage.transform.LookAt(UIdirection);
    }

    void Awake()
    {
        AN = GetComponentInChildren<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //AN.SetBool("isWalk", true);
        //queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
        thePlayer = FindObjectOfType<Player>();
        theAudio = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NearPlayer() == 1)
        {
            StartCoroutine(ChasePlayer());
        }
        else if (NearPlayer() == 2)
        {
            StartCoroutine(MonsterAttack());
        }
        else if(NearPlayer() == 0)
        {
            target = null;
            notMove = false;
        }

        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MovingAI());
        }
        if (!notMove)
            StartCoroutine(MovingAICoroutine());

        NickNameText.transform.LookAt(UIdirection);
        HealthBarImage.transform.LookAt(UIdirection);
        HealthImage.transform.LookAt(UIdirection);


    }
    IEnumerator MovingAI()
    {
        RandomD();
        yield return new WaitForSeconds(4f);
        notCoroutine = false;
    }

    IEnumerator MovingAICoroutine()
    {
        while (true)
        {
            bool checkCollisionFlag = CheckCollision();
            if (checkCollisionFlag && walk == true)
            {   
                AN.SetBool("isWalk", false);
                
                yield return new WaitForSeconds(1f);
                walk = false;
            }
            else
            {
                transform.Translate(new Vector3(0, 0, 0.01f) * speed);
                AN.SetBool("isWalk", true);

                walk = true;
                yield return new WaitForSeconds(0.01f);

                break;
            }
        }
    }

    protected bool CheckCollision()
    {
        //RaycastHit hit;
        bool hit;
        Vector3 start = transform.position;
        Vector3 end = start + transform.forward;

        boxCollider.enabled = false;
        hit = Physics.Raycast(start, transform.forward, 1f);
        boxCollider.enabled = true;

        return hit;
    }

    private void RandomD()
    {
        dir.Set(0, 0, 0);
        random_int = Random.Range(0, 2);
        int random_rotation = Random.Range(0, 180);

        switch(random_int)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, random_rotation, 0);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, -random_rotation, 0);
                break;
        }
    }

    private int NearPlayer()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        if (Players.Length > 1)
        {
            for(int i = 0; i < Players.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, Players[i].transform.position);
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPlayer = Players[i];
                }
            }
                playerPos = closestPlayer.transform.position;
            if ((Vector3.Distance(transform.position, playerPos) < 8f) && (Vector3.Distance(transform.position, playerPos) > 1f))
            {
                target = closestPlayer;
                return 1;
            }
            else if (Vector3.Distance(transform.position, playerPos) <= 1f)
            {
                target = closestPlayer;
                return 2;
            }
            else
                return 0;
        }
        else if (Players.Length == 1)
        {
            target = Players[0];
            playerPos = target.transform.position;
            if ((Vector3.Distance(transform.position, playerPos) < 8f) && (Vector3.Distance(transform.position, playerPos) > 1f))
            {
                return 1;
            }
            else if (Vector3.Distance(transform.position, playerPos) <= 1f)
            {
                return 2;
            }
            else
                return 0;
        }
        else
        {
            return 0;
        }
    }

    IEnumerator MonsterAttack()
    {
        notMove = true;
        AN.SetBool("isWalk", false);
        zAttack = true;
        if( target != null)
           transform.LookAt(target.transform.position);

        AN.SetBool("isAttack", true);
        theAudio.Play(attackSound);
        yield return new WaitForSeconds(1f);
        AN.SetBool("isAttack", false);
        zAttack = false;
        notMove = false;
    }
    IEnumerator ChasePlayer()
    {
        if(target == null)
            target = GameObject.FindGameObjectWithTag("Player");

        notMove = true;
        transform.LookAt(target.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * 5f * Time.deltaTime);
        AN.SetBool("isWalk", true);
        theAudio.Play(walkSound);
        yield return new WaitForSeconds(0.01f);
    }
}
