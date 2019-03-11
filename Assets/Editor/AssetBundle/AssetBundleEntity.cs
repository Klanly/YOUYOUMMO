using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ABʵ��
/// </summary>
public class AssetBundleEntity
{

    /// <summary>
    /// ���ڴ��ʱ��ѡ�� Ψһ��key
    /// </summary>
    public string Key;

    /// <summary>
    /// ����
    /// </summary>
    public string Name;
    /// <summary>
    /// ���
    /// </summary>
    public string Tag;
    /// <summary>
    /// �Ƿ��ʼ��Դ
    /// </summary>
    public bool IsFirstData;
    /// <summary>
    /// �Ƿ��ļ���
    /// </summary>
    public bool IsFolder;
    /// <summary>
    /// �Ƿ�ѡ��
    /// </summary>
    public bool IsChecked;


    private List<string> m_PathList = new List<string>();

    public List<string> PathList
    {
        get { return m_PathList; }
    }
}
