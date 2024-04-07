using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody RB;
    public Animator AN;
    //public SpriteRenderer SR;
    public PhotonView PV;
    public Text NickNameText;
    public Image HealthImage;
    //public Camera CM;

    bool isGround;
    Vector3 curPos;

    private JoyStick thejoystick;

    float hAxis;
    float vAxis;
    public float speed;

    Vector3 moveVec;
    
    Animator anim;
    bool wDown;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }


    void Awake()
    {
        NickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NickNameText.color = PV.IsMine ? Color.green : Color.red;
        anim = GetComponentInChildren<Animator>();
        transform.position = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        thejoystick = FindObjectOfType<JoyStick>();
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.LeftShift);

        if(thejoystick.isTouch)
        {
            transform.position += thejoystick.movePosition;
        }
        //moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if(wDown)
        {
            //transform.position += moveVec * speed*2f * Time.deltaTime;
        }
        else
            //transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isWalk", thejoystick.movePosition != Vector3.zero);
        //anim.SetBool("isRun", wDown);

        transform.LookAt(transform.position + thejoystick.movePosition);
    }
}
