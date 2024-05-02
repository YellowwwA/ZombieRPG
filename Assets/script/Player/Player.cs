using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    static public Player instance;

    public Rigidbody RB;
    public Animator AN;
    //public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    public Image HealthImage;
    public Image HealthBarImage;
    //public Camera CM;

    bool isGround;
    Vector3 curPos;

    private JoyStick thejoystick;

    float hAxis;
    float vAxis;
    //public float speed;

    Vector3 moveVec;
    Vector3 UIdirection = new Vector3(0, 0, 100);


    Button attackButton;
    public bool attack = false;

    //Animator anim;
    bool wDown;
    bool walk = false;

    public string walkSound_1;
    public string walkSound_2;

    private AudioManager theAudio;
    private DialogueManager theDM;
    private MissionComplete theMC;
    public bool MStart;

    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
        AN = GetComponentInChildren<Animator>();
        //transform.position = Vector3.zero;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        NickNameText.transform.LookAt(UIdirection);
        HealthBarImage.transform.LookAt(UIdirection);
        HealthImage.transform.LookAt(UIdirection);
    }

    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
        theMC = FindObjectOfType<MissionComplete>();
        theAudio = FindObjectOfType<AudioManager>();
        thejoystick = FindObjectOfType<JoyStick>();

        attackButton = GameObject.FindGameObjectWithTag("Button").GetComponent<Button>();
        attackButton.onClick.AddListener(AttackBtn);
    }

    // Update is called once per frame
    void Update()
    {

        if(PV.IsMine)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            vAxis = Input.GetAxisRaw("Vertical");
            wDown = Input.GetKey(KeyCode.LeftShift);

            if (thejoystick.isTouch)
            {
                transform.position += thejoystick.movePosition;
                if(walk == false)
                    StartCoroutine(MoveSoundCoroutine());
                //NickNameText.transform.rotation = Quaternion.Euler(0,0,0);
                //HealthImage.transform.rotation = Quaternion.Euler(0, 0, 0);
                //HealthBarImage.transform.rotation = Quaternion.Euler(0, 0, 0);
                
            }
            //moveVec = new Vector3(hAxis, 0, vAxis).normalized;//

            //if(wDown)
            //{
            //    transform.position += moveVec * speed*2f * Time.deltaTime;//
            //}
            //else
            //transform.position += moveVec * speed * Time.deltaTime;//

            //AN.SetBool("isWalk", moveVec != Vector3.zero);
            AN.SetBool("isWalk", thejoystick.movePosition != Vector3.zero);
            //anim.SetBool("isRun", wDown);//

            transform.LookAt(transform.position + thejoystick.movePosition);
            //transform.LookAt(transform.position + moveVec);
            NickNameText.transform.LookAt(UIdirection);
            HealthBarImage.transform.LookAt(UIdirection);
            HealthImage.transform.LookAt(UIdirection);
            /*
            if ((Input.GetKeyDown(KeyCode.Space)) && (theDM.NPCID != 0))
            {
                theMC.MStart = true;
                theDM.Action();
            }*/
            if(theDM.NPCID != 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.gameObject.name == "QuestNotice")
                        {
                            theMC.MStart = true;
                            theDM.Action();
                        }
                    }
                }
            }


            /*if(Input.GetKeyDown(KeyCode.Space))
            {
                AN.SetBool("isShot", true);
            }*/

        }

    }
    public void AttackBtn()
    {
        if (PV.IsMine)
        {
            AN.SetBool("isShot", true);
            StartCoroutine(AttackCheck());
        }

    }
    IEnumerator AttackCheck()
    {
        yield return new WaitForSeconds(0.5f);
        attack = true;
        yield return new WaitForSeconds(1f);
        attack = false;
    }

    IEnumerator MoveSoundCoroutine()
    {
        walk = true;
        theAudio.Play(walkSound_1);
        yield return new WaitForSeconds(0.4f);
        theAudio.Play(walkSound_2);
        yield return new WaitForSeconds(0.4f);
        walk = false;
    }
}
