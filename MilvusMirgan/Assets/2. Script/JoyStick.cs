using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{

    // 공개
    public Transform Stick;         // 조이스틱.
    public Transform Player;         

    // 비공개
    private Vector2 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector2 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반 지름.
    private bool moveFlag;

    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        moveFlag = false;
    }
    private void Update()
    {
        if (moveFlag)
            Player.transform.Translate(Mathf.Abs(JoyVec.x) > 0.5f ?  0.7f * JoyVec * Time.deltaTime * 10f : 0.25f * JoyVec * Time.deltaTime * 10f);
    }

    // 드래그
    public void Drag(BaseEventData _Data)
    {
        moveFlag = true;
        PointerEventData Data = _Data as PointerEventData;
        Vector2 Pos = Data.position;

        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - StickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector2.Distance(Pos, StickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;
    }

    // 드래그 끝.
    public void DragEnd()
    {
        moveFlag = false;
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector2.zero;          // 방향을 0으로.
    }
}