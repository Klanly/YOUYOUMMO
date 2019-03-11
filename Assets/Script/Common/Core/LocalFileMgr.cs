using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �����ļ�����
/// </summary>
public class LocalFileMgr :Singleton<LocalFileMgr>
{

    public readonly string LocalFilePath = Application.persistentDataPath + "/";


    /// <summary>
    /// ��ȡ�����ļ���byte����
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetBuffer(string path)
    {
        
        byte[] buffer = null;
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }
        
}
