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
            go_Player.transform.position += movePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        // 조이스틱이 조이스틱백그라운드 이미지를 벗어나지 않도록 범위만큼 가두기
        value = Vector2.ClampMagnitude(value, radius/2);  
        rect_Joystick.localPosition = value;

        // 조이스틱이 중심에서 벗어나는 거리와 플레이어 속도가 비례함
        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / radius;  
        
        value = value.normalized; // 방향만 뽑아냄
        movePosition = new Vector3(value.x * moveSpeed * 0.0015f * distance * Time.deltaTime, 0f,
                                    value.y * moveSpeed * 0.0015f * distance * Time.deltaTime);
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
