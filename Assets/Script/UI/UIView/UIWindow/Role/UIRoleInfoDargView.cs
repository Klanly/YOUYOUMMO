using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRoleInfoDargView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
    /// Ŀ��
    /// </summary>
    /// 
    [SerializeField]
    private Transform m_Target;

    private float rotaSpeed=300;

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_DragBeginPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_DragEndPos = eventData.position;
        float x = m_DragBeginPos.x - m_DragEndPos.x;
        m_Target.Rotate(0,Time.deltaTime*rotaSpeed*(x>0?1:-1),0);

        m_DragBeginPos= m_DragEndPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       

    }


}
