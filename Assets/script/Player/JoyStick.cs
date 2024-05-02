using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_Joystick;

    private float radius;

    [SerializeField] private GameObject go_Player;
    [SerializeField] private float moveSpeed = 0.0025f;

    public bool isTouch = false;
    public Vector3 movePosition;


    // Start is called before the first frame update
    void Start()
    {
        radius = rect_Background.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouch)
        {
            go_Player.transform.position += movePosition;
        }
        //anim.SetBool("isWalk", movePosition != Vector3.zero);
        //anim.SetBool("isRun")
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        value = Vector2.ClampMagnitude(value, radius/2);  // ���̽�ƽ�� ���̽�ƽ��׶��� �̹����� ����� �ʵ��� ������ŭ ���α�
        rect_Joystick.localPosition = value;

        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / radius;  // ���̽�ƽ�� �߽ɿ��� ����� �Ÿ��� �÷��̾� �ӵ��� �����
        
        value = value.normalized; // ���⸸ �̾Ƴ�
        movePosition = new Vector3(value.x * moveSpeed * 0.0015f * distance * Time.deltaTime, 0f, value.y * moveSpeed * 0.0015f * distance * Time.deltaTime);
        //Debug.Log("distance" + distance);
        //Debug.Log("value.x" + value.x);
        //Debug.Log("value.y" + value.y);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        rect_Joystick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
    }
}
