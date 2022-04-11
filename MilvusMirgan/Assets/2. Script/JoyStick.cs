using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{

    // ����
    public Transform Stick;         // ���̽�ƽ.
    public Transform Player;         

    // �����
    private Vector2 StickFirstPos;  // ���̽�ƽ�� ó�� ��ġ.
    private Vector2 JoyVec;         // ���̽�ƽ�� ����(����)
    private float Radius;           // ���̽�ƽ ����� �� ����.
    private bool moveFlag;

    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // ĵ���� ũ�⿡���� ������ ����.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        moveFlag = false;
    }
    private void Update()
    {
        if (moveFlag)
            Player.transform.Translate(0.7f * JoyVec * Time.deltaTime * 10f);
    }

    // �巡��
    public void Drag(BaseEventData _Data)
    {
        moveFlag = true;
        PointerEventData Data = _Data as PointerEventData;
        Vector2 Pos = Data.position;

        // ���̽�ƽ�� �̵���ų ������ ����.(������,����,��,�Ʒ�)
        JoyVec = (Pos - StickFirstPos).normalized;

        // ���̽�ƽ�� ó�� ��ġ�� ���� ���� ��ġ�ϰ��ִ� ��ġ�� �Ÿ��� ���Ѵ�.
        float Dis = Vector2.Distance(Pos, StickFirstPos);

        // �Ÿ��� ���������� ������ ���̽�ƽ�� ���� ��ġ�ϰ� �ִ°����� �̵�. 
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // �Ÿ��� ���������� Ŀ���� ���̽�ƽ�� �������� ũ�⸸ŭ�� �̵�.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;
    }

    // �巡�� ��.
    public void DragEnd()
    {
        moveFlag = false;
        Stick.position = StickFirstPos; // ��ƽ�� ������ ��ġ��.
        JoyVec = Vector2.zero;          // ������ 0����.
    }
}