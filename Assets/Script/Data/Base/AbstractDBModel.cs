using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDBModel<T,P>
    where T:class,new()
    where P:AbstractEntity
{
    protected List<P> m_List;

    protected Dictionary<int, P> m_dic;

    public AbstractDBModel()
    {
        m_List = new List<P>();
        m_dic = new Dictionary<int, P>();
        LoadData();

    }
    public static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
               // instance.;
            }
            return instance;

        }


    }
    //��ȡ�ļ�����
    protected abstract string FileName { get; }

    //��ȡʵ��
    protected abstract P MakeEntity(GameDataTableParser parse);
    /// <summary>
    /// ��������
    /// </summary>
    private void LoadData()
    {
        string path = null;

#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
        path = Application.dataPath + "/Download/DataTable/" + FileName;
#else

        path = Application.persistentDataPath + "/Download/DataTable/" + FileName;
#endif



        //���ļ�
        using (GameDataTableParser parse = new GameDataTableParser(path))
        {
            while (!parse.Eof)
            {

                //����ʵ��
                P p = MakeEntity(parse);

                m_List.Add(p);
                m_dic[p.Id] = p;
                parse.Next();
            }


        }
        //��ѹ��

    }
    /// <summary>
    /// ��ȡ�����б�
    /// </summary>
    /// <returns></returns>
    public List<P> GetList()
    {
        return m_List;
    }
    /// <summary>
    /// ���ݱ�Ų�ѯ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public P Get(int id)
    {
        if (m_dic.ContainsKey(id))
        {
            return m_dic[id];
        }
        return null;
    }



}
