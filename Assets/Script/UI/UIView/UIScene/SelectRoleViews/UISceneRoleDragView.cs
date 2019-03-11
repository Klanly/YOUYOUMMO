using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISceneRoleDragView : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    /// <summary>
    /// ��ʼ��ק��λ��
    /// </summary>
    private Vector2 m_DragBeginPos = Vector2.zero;
    /// <summary>
    /// ������ק��λ��
    /// </summary>
    private Vector2 m_DragEndPos = Vector2.zero;
    /// <summary>
    ///��קί�� 0=Left 1=Right
    /// </summary>
    public Action<int> OnSelctDrag;

  

    void Start ()
    {
		
	}
	

	void Update ()
    {
		
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_DragBeginPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_DragEndPos= eventData.position;

        float x = m_DragBeginPos.x - m_DragEndPos.x;
        //�ݴ�Χ
        if (x>20)
        {
            OnSelctDrag(0);
        }
        else if (x < -20)
        {
            OnSelctDrag(1);
        }
    }
}
