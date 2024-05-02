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
        //current_interMWT -= Time.deltaTime;

        //if(current_interMWT <= 0)
        //{
        //   current_interMWT = inter_MoveWaitTime;
        //RandomDirection();

        // if (CheckCollision())
        //    return;

        //Move(direction);
        //}
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(vector.x, 0, vector.z), speed);
        if (NearPlayer() == 1)
        {
            //Debug.Log(target.transform.position + "   1111");
            StartCoroutine(ChasePlayer());
        }
        else if (NearPlayer() == 2)
        {
            //Debug.Log(target.transform.position + "   2222");
            StartCoroutine(MonsterAttack());
        }

        else if(NearPlayer() == 0)
        {
            //Debug.Log(target);
            target = null;
            notMove = false;
        }



        if (!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MovingAI());
        }
        //this.transform.LookAt(transform.position + dir);
        //if(!notMove)
        //    StartCoroutine(MovingAICoroutine());
        NickNameText.transform.LookAt(UIdirection);
        HealthBarImage.transform.LookAt(UIdirection);
        HealthImage.transform.LookAt(UIdirection);
        if (!notMove)
            StartCoroutine(MovingAICoroutine());

    }
    IEnumerator MovingAI()
    {
        //AN.SetBool("isWalk", true);
        RandomD();
        
        //moveVec = new Vector3(0, 0, 5);

        //RandomD();
        yield return new WaitForSeconds(4f);
        //AN.SetBool("isWalk", false);
        notCoroutine = false;

    }

    IEnumerator MovingAICoroutine()
    {
        /*
        moveVec = dir;
        Debug.Log(moveVec);
        transform.position += moveVec * speed * Time.deltaTime;
        AN.SetBool("isWalk", true);
        */

        while (true)
        {
            
            bool checkCollisionFlag = CheckCollision();
            if (checkCollisionFlag && walk == true)
            {   
                AN.SetBool("isWalk", false);
                //walk = false;
                yield return new WaitForSeconds(1f);
                walk = false;
            }
            else
            {
                
                transform.Translate(new Vector3(0, 0, 0.01f) * speed);
                AN.SetBool("isWalk", true);
                //if(transform.position.x == )
                // AN.SetBool("isWalk", false);

                walk = true;
                yield return new WaitForSeconds(0.01f);
                //notCoroutine = false;

                break;
            }
        }



    }

    public void Move(string _dir, int _frequency = 5)
    {
        //queue.Enqueue(_dir);
        if(!notCoroutine)
        {
            notCoroutine = true;
            StartCoroutine(MoveCoroutine(_dir, _frequency));
        }
    }

    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
       // while(queue.Count != 0)
        {

            vector.Set(0, 0, 0);

            switch(direction)
            {
                case "FORWARD":
                    vector.x = 10f;
                    break;
                case "BACK":
                    vector.x = -10f;
                    break;
                case "RIGHT":
                    vector.z = 10f;
                    break;
                case "LEFT":
                    vector.z = -10f;
                    break;
            }
            AN.SetBool("isWalk", true);
            this.transform.LookAt(vector);
            while (true)
            {
                bool checkCollisionFlag = CheckCollision();
                if (checkCollisionFlag)
                {
                    AN.SetBool("isWalk", false);
                    yield return new WaitForSeconds(1f);
                }
                else
                    break;
            }

            AN.SetBool("isWalk", true); //
                                        //boxCollider.offset = new Vector3(vector.x * 0.7f * speed, 0, vector.z * 0.7f * speed);

            //while(currentWalkCount < walkCount)
            //{
            //transform.Translate(vector.x * speed, 0, vector.z * speed);
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(vector.x, 0, vector.z), 0.1f);            
            yield return new WaitForSeconds(0.01f);
            //}
            //if (_frequency == 5)
            //    AN.SetBool("isWalk", false);
        }
        AN.SetBool("isWalk", false);
        notCoroutine = false;
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
        //Debug.Log(hit);
        return hit;
        //if (hit.transform != null)
        //    return true;
        //return false;
    }

    private void RandomDirection()
    {
        vector.Set(0, 0, 0);
        random_int = Random.Range(0, 4);
        switch(random_int)
        {
            case 0:
                vector.x = 0.5f;
                direction = "FORWARD";
                break;
            case 1:
                vector.x = -0.5f;
                direction = "BACK";
                break;
            case 2:
                vector.z = 0.5f;
                direction = "RIGHT";
                break;
            case 3:
                vector.z = -0.5f;
                direction = "LEFT";
                break;
        }
    }
    private void RandomD()
    {
        dir.Set(0, 0, 0);
        random_int = Random.Range(0, 2);
        int random_rotation = Random.Range(0, 180);
        /*
        switch (random_int)
        {
            case 0:
                dir.Set(0.5f, 0, 0);
                //direction = "FORWARD";
                break;
            case 1:
                dir.Set(-0.5f, 0, 0);
                //direction = "BACK";
                break;
            case 2:
                dir.Set(0, 0, 0.5f);
                //direction = "RIGHT";
                break;
            case 3:
                dir.Set(0, 0, -0.5f);
                //direction = "LEFT";
                break;
        }*/

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

        //playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

    }

    IEnumerator MonsterAttack()
    {
        //Debug.Log("MonsterAttackInside");
        //notCoroutine = true;
        notMove = true;
        AN.SetBool("isWalk", false);
        zAttack = true;
        //transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
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
        //notCoroutine = true;
        notMove = true;
        //transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        //transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position, 1f*Time.deltaTime);
        transform.LookAt(target.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * 5f * Time.deltaTime);
        AN.SetBool("isWalk", true);
        theAudio.Play(walkSound);
        yield return new WaitForSeconds(0.01f);
        //notMove = false;
    }
}
