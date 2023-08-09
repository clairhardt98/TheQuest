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
        //������ ����
        radius = rect_Background.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // ���콺 ��ǥ���� background�� ��ǥ�� ���� �� ��ŭ joystick�� position�� �̵�
        Vector2 value = eventData.position - (Vector2)rect_Background.position;
        //��������ŭ ���α� -> background �ٱ����� joystick�� ������ ���ϵ���
        value = Vector2.ClampMagnitude(value, radius);
        rect_Joystick.localPosition = value;
        //���̽�ƽ�� ��ġ�� Ư�� ���� �Ѿ��� ���
        if (value.x > 40.0f && !flag)
        {
            flag = true;
            EventManager.instance.TriggerEvent(EventType.UIJoystickRight);
            //�̺�Ʈ ������
        }
        if (value.x < -40.0f && !flag)
        {
            flag = true;
            //�̺�Ʈ ������
            EventManager.instance.TriggerEvent(EventType.UIJoystickLeft);
        }
        if (value.y > 40.0f && !flag)
        {
            flag = true;
            //�̺�Ʈ ������
            EventManager.instance.TriggerEvent(EventType.UIJoystickUp);
        }
        if (value.y < -40.0f && !flag)
        {
            flag = true;
            //�̺�Ʈ ������
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
