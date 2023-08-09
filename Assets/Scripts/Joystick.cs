using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private RectTransform rect_Background;
    [SerializeField]
    private RectTransform rect_Joystick;

    private float radius;
    private bool isTouch;

    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        //반지름 설정
        radius = rect_Background.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 좌표에서 background의 좌표를 빼서 그 만큼 joystick의 position을 이동
        Vector2 value = eventData.position - (Vector2)rect_Background.position;
        //반지름만큼 가두기 -> background 바깥으로 joystick이 나가지 못하도록
        value = Vector2.ClampMagnitude(value, radius);
        rect_Joystick.localPosition = value;
        //조이스틱의 위치가 특정 값을 넘었을 경우
        if (value.x > 40.0f && !flag)
        {
            flag = true;
            EventManager.instance.TriggerEvent(EventType.UIJoystickRight);
            //이벤트 날리기
        }
        if (value.x < -40.0f && !flag)
        {
            flag = true;
            //이벤트 날리기
            EventManager.instance.TriggerEvent(EventType.UIJoystickLeft);
        }
        if (value.y > 40.0f && !flag)
        {
            flag = true;
            //이벤트 날리기
            EventManager.instance.TriggerEvent(EventType.UIJoystickUp);
        }
        if (value.y < -40.0f && !flag)
        {
            flag = true;
            //이벤트 날리기
            EventManager.instance.TriggerEvent(EventType.UIJoystickDown);
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        rect_Joystick.localPosition = Vector3.zero;
        flag = false;
    }
}
