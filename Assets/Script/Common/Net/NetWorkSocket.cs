﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetWorkSocket:SingletonMono<NetWorkSocket>
{

    /// <summary>
    /// 客户端Socket
    /// </summary>
    private Socket m_Client;
    // private byte[] buffer = new byte[10240];
    /// <summary>
    /// 压缩数组的长度界限
    /// </summary>
    private const int m_CompressLen=200;
    #region 发送消息变量
    /// <summary>
    /// 发送消息队列
    /// </summary>
    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();

    /// <summary>
    /// 检查队列委托
    /// </summary>
    private Action m_CheckSendQueue;


    #endregion
    #region 接收数据所需变量
    /// <summary>
    /// 接受数据包字节缓存区
    /// </summary>
    private byte[] m_ReceiveBuffer = new byte[2048];
    /// <summary>
    /// 接收数据包的缓存数据流
    /// </summary>
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();
    /// <summary>
    /// 接受消息的队列
    /// </summary>
    private Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();

    private int m_ReceiveCount = 0;
    #endregion
    public Action OnConectOk;
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        DisConnected();
    }

    public  void DisConnected()
    {
        if (m_Client != null && m_Client.Connected)

        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }
    protected override void OnUpdate()
    {
        base.OnUpdate();
        #region 接收数据
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                m_ReceiveCount++;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {


                        byte[] buffer = m_ReceiveQueue.Dequeue();
                        //异或之后的数组
                        byte[] newBuffer = new byte[buffer.Length - 3];

                        bool isCompress = false;

                        int crc = 0;
                        using (MMO_MemoryStream ms1 = new MMO_MemoryStream(buffer))
                        {
                            isCompress = ms1.ReadBool();

                            crc = ms1.ReadUShort();

                            ms1.Read(newBuffer, 0, newBuffer.Length);
                        }

                        int newCrc = Crc16.CalculateCrc16(newBuffer);
                        //传过来的crc是否=新包的crc
                        if (newCrc == crc)
                        {
                            //异或原始数据
                            newBuffer = SecurityUtil.Xor(newBuffer);

                            if (isCompress)
                            {
                                newBuffer = ZlibHelper.DeCompressBytes(newBuffer);
                            }


                            ushort protoCode = 0;
                            byte[] protoConent = new byte[buffer.Length - 2];
                            using (MMO_MemoryStream ms = new MMO_MemoryStream(newBuffer))
                            {
                                protoCode = ms.ReadUShort();
                                ms.Read(protoConent, 0, protoConent.Length);
                                SocketDispatcher.Instance.Dispatch(protoCode, protoConent);


                            }


                        }
                        else
                        {
                            break;
                        }




                    }
                    break;
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
        #endregion
    }



    #region Connect连接到服务器
    /// <summary>
    /// 连接到服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void Connect(string ip,int port)
    {
        if (m_Client != null && m_Client.Connected) return;

        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip),port));
            m_CheckSendQueue = OnCheckSendQueueCallBack;
            Debug.Log("连接成功");
            ReceiveNsg();
            if (OnConectOk!=null)
            {
                OnConectOk();
            }


        }
        catch (Exception ex)
        {
            Debug.Log("连接失败" + ex.Message);
            
        }
    }
    #endregion
    #region OnCheckSendQueueCallBack检查队列委托的回调
    /// <summary>
    /// 检查队列委托的回调
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        //如果队列中有数据包 则发送数据包
        if (m_SendQueue.Count>0)
        {
            Send(m_SendQueue.Dequeue());
        }
    }
    #endregion
    #region MakeData封装数据包
    /// <summary>
    /// 封装数据包
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;
        //压缩
        bool isCompress = data.Length > m_CompressLen ? true : false;
        if (isCompress)
        {
            data = ZlibHelper.CompressBytes(data);
        }

        //异或
        data = SecurityUtil.Xor(data);
        //crc校验
        ushort crc = Crc16.CalculateCrc16(data);

        

        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length+3));
         
            ms.WriteBool(isCompress);

           
            ms.WriteUShort(crc);
         
            ms.Write(data,0, data.Length);

            retBuffer = ms.ToArray();
        }
        return retBuffer;

    }

    #endregion
    #region SendMsg发送消息 把消息加入队列
    /// <summary>
    /// 发送消息 把消息加入队列
    /// </summary>
    /// <param name="data"></param>
    public void SendMsg(byte[] data)
    {
        byte[] sendBuffer = MakeData(data);

        lock(m_SendQueue)
        {
            m_SendQueue.Enqueue(sendBuffer);

            m_CheckSendQueue.BeginInvoke(null,null);


        }
    }
    #endregion
    #region Send真发送数据包的服务器
    /// <summary>
    /// 真发送数据包的服务器
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer)
    {

        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }
    #endregion
    #region SendCallBack发送消息回调
    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);

        OnCheckSendQueueCallBack();
    }
    #endregion

    //============================================

    #region ReceiveNsg 接收数据
    /// <summary>
    /// 接收数据
    /// </summary>
    private void ReceiveNsg()
    {
        //异步接收数据
        m_Client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveCallBack, m_Client);

    }
    #endregion
    #region ReceiveCallBack接受数据回调
    /// <summary>
    /// 接受数据回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            int len = m_Client.EndReceive(ar);


            if (len > 0)
            {//已经接收到数据


                //把接受到数据 写入缓冲数据流尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;
                //把指定长度的字节写入数据流
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                //如果缓存>2说明至少有一个不完整的包发送过来了,客户端定义Ushort就是2
                if (m_ReceiveMS.Length > 2)
                {
                    //循环 拆分包
                    while (true)
                    {
                        //把数据流指针位置放在0
                        m_ReceiveMS.Position = 0;
                        //包体的长度
                        int currMsgLen = m_ReceiveMS.ReadUShort();
                        //总包的长度
                        int currFullMsgLen = 2 + currMsgLen;
                        //如果缓存流的数据》=整包，说明至少接收有一个完整
                        if (m_ReceiveMS.Length >= currFullMsgLen)
                        {
                            //定义包体的数组
                            byte[] buffer = new byte[currMsgLen];
                            //把数据流指针位置放在2，也就是包体的位置
                            m_ReceiveMS.Position = 2;
                            //把数据流读到数组里buffer也就是我们要的数据
                            m_ReceiveMS.Read(buffer, 0, currMsgLen);

                            lock(m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(buffer);
                            }
                           


                            //===========================处理剩余字节====================================
                            //剩余字节
                            int reMainLen = (int)m_ReceiveMS.Length - currFullMsgLen;
                            if (reMainLen > 0)
                            {
                                m_ReceiveMS.Position = currFullMsgLen;
                                byte[] reMainBuffer = new byte[reMainLen];
                                m_ReceiveMS.Read(reMainBuffer, 0, reMainLen);


                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                //把剩余字节重新写入数据流
                                m_ReceiveMS.Write(reMainBuffer, 0, reMainBuffer.Length);




                                reMainBuffer = null;


                            }
                            else
                            {
                                //清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                break;
                            }


                        }
                        else
                        {//还没有收到完整的包


                            break;
                        }

                    }

                }


                ReceiveNsg();


            }
            else
            {
                //客户端断开


                Debug.Log(string.Format( "服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));


    




            }
        }
        catch
        {
            Debug.Log(string.Format("服务器{0}断开连接", m_Client.RemoteEndPoint.ToString()));


 
        }




    }
    #endregion
}
