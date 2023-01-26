using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WiFiUDP : MonoBehaviour
{
    public GameObject figure1;
    public GameObject figure2;
    public GameObject figure3;
    public GameObject figure4;
    public GameObject figure5;
    public GameObject figure6;
    public GameObject figure7;
    public GameObject figure8;
    public GameObject figure9;
    public GameObject figure10;
    public GameObject figure11;

    static int mpuCount = 6;
    Thread m_Thread;
    UdpClient m_Client;

    string[] recieveTemp = new string[2];

    int IMUNumber = 0;
    public int port;

    string[] strData = new string[4];
    float[] strData_received = new float[mpuCount * 4];
    float[] Q1 = new float[4];
    string returnData;
    float hipAngle = 0;

    /* ---------------------------------------------------------------------------------- */
    string[] recieveArray = new string[10];
    string recieveArrayTemp;
    /* ---------------------------------------------------------------------------------- */

    //float[,] Q = new float[10, 4];
    static float[,] Q = new float[10, 4];

    void Start()
    {
        figure1 = GameObject.Find("mixamorig:RightForeArm");
        figure2 = GameObject.Find("mixamorig:RightArm");
        figure3 = GameObject.Find("mixamorig:LeftForeArm");
        figure4 = GameObject.Find("mixamorig:LeftArm");
        figure5 = GameObject.Find("mixamorig:Head");
        figure6 = GameObject.Find("mixamorig:Spine1");
        figure7 = GameObject.Find("mixamorig:RightLeg");
        figure8 = GameObject.Find("mixamorig:RightUpLeg");
        figure9 = GameObject.Find("mixamorig:LeftLeg");
        figure10 = GameObject.Find("mixamorig:LeftUpLeg");
        figure11 = GameObject.Find("mixamorig:Hips");

        m_Thread = new Thread(new ThreadStart(ReceiveData));
        m_Thread.IsBackground = true;
        m_Thread.Start();
        strData = null;
    }

    private void Update()
    {
        //udpSend();
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    udpSend();
        //    //print("1");
        //}

        //print("IMU1:" + Q[0, 0] + "," + Q[0, 1] + "," + Q[0, 2] + "," + Q[0, 3]);
        //print("IMU2:" + Q[1, 0] + "," + Q[1, 1] + "," + Q[1, 2] + "," + Q[1, 3]);
        //print("IMU3:" + Q[2, 0] + "," + Q[2, 1] + "," + Q[2, 2] + "," + Q[2, 3]);
        //print("IMU4:" + Q[3, 0] + "," + Q[3, 1] + "," + Q[3, 2] + "," + Q[3, 3]);
        //print("IMU5:" + Q[4, 0] + "," + Q[4, 1] + "," + Q[4, 2] + "," + Q[4, 3]);
        //print("IMU6:" + Q[5, 0] + "," + Q[5, 1] + "," + Q[5, 2] + "," + Q[5, 3]);
        //print("IMU7:" + Q[6, 0] + "," + Q[6, 1] + "," + Q[6, 2] + "," + Q[6, 3]);
        //print("IMU8:" + Q[7, 0] + "," + Q[7, 1] + "," + Q[7, 2] + "," + Q[7, 3]);
        //print("IMU9:" + Q[8, 0] + "," + Q[8, 1] + "," + Q[8, 2] + "," + Q[8, 3]);
        //print("IMU10:" + Q[9, 0] + "," + Q[9, 1] + "," + Q[9, 2] + "," + Q[9, 3]);

        //figure7.transform.rotation = new Quaternion(-Q[0, 1], -Q[0, 3], -Q[0, 2], Q[0, 0]);
        //figure8.transform.rotation = new Quaternion(-Q[1, 1], -Q[1, 3], -Q[1, 2], Q[1, 0]);
        //figure9.transform.rotation = new Quaternion(-Q[2, 1], -Q[2, 3], -Q[2, 2], Q[2, 0]);
        //figure10.transform.rotation = new Quaternion(-Q[3, 1], -Q[3, 3], -Q[3, 2], Q[3, 0]);


        //figure3.transform.rotation = new Quaternion(-Q[0, 1], -Q[0, 3], -Q[0, 2], Q[0, 0]);
        //figure4.transform.rotation = new Quaternion(-Q[1, 1], -Q[1, 3], -Q[1, 2], Q[1, 0]);
        //figure6.transform.rotation = new Quaternion(Q[2, 2], -Q[2, 3], -Q[2, 1], Q[2, 0]);

        figure1.transform.rotation = new Quaternion(Q[0, 1], -Q[0, 3], Q[0, 2], Q[0, 0]);
        figure2.transform.rotation = new Quaternion(Q[1, 1], -Q[1, 3], Q[1, 2], Q[1, 0]);
        figure3.transform.rotation = new Quaternion(-Q[2, 1], -Q[2, 3], -Q[2, 2], Q[2, 0]);
        figure4.transform.rotation = new Quaternion(-Q[3, 1], -Q[3, 3], -Q[3, 2], Q[3, 0]);
        figure5.transform.rotation = new Quaternion(Q[4, 2], -Q[4, 3], -Q[4, 1], Q[4, 0]);
        figure6.transform.rotation = new Quaternion(Q[5, 2], -Q[5, 3], -Q[5, 1], Q[5, 0]);
        figure7.transform.rotation = new Quaternion(-Q[6, 1], -Q[6, 3], -Q[6, 2], Q[6, 0]);
        figure8.transform.rotation = new Quaternion(-Q[7, 1], -Q[7, 3], -Q[7, 2], Q[7, 0]);
        figure9.transform.rotation = new Quaternion(-Q[8, 1], -Q[8, 3], -Q[8, 2], Q[8, 0]);
        figure10.transform.rotation = new Quaternion(-Q[9, 1], -Q[9, 3], -Q[9, 2], Q[9, 0]);

        // 设置臀部旋转角度，取腰部角度与两大腿角度融合计算 y-axis
        float upLegAngle, spineAngle, leftLegAngle, rightLegAngle;

        rightLegAngle = figure8.transform.localEulerAngles.y;
        leftLegAngle = figure10.transform.localEulerAngles.y;
        rightLegAngle = (rightLegAngle > 180) ? (rightLegAngle - 360) : rightLegAngle;
        leftLegAngle = (leftLegAngle > 180) ? (leftLegAngle - 360) : leftLegAngle;

        upLegAngle = (rightLegAngle + leftLegAngle) / 2;
        spineAngle = figure6.transform.localEulerAngles.y;
        spineAngle = (spineAngle > 180) ? spineAngle - 360 : spineAngle;
        hipAngle = (upLegAngle + spineAngle) / 2;
        //hipAngle = (hipAngle > 180) ? hipAngle - 360 : hipAngle;
        figure11.transform.localEulerAngles = new Vector3(0, hipAngle, 0);
    }

    void ReceiveData()
    {
        try
        {
            m_Client = new UdpClient(port);
            m_Client.EnableBroadcast = true;
            while (true)
            {

                IPEndPoint hostIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = m_Client.Receive(ref hostIP);
                returnData = Encoding.ASCII.GetString(data).ToString();
                //print(returnData);

                returnData = returnData.Replace("\r\n", "$");  // 用'\n'替换"\r\n"，后续以'\n'为分隔符
                recieveArray = returnData.Split('$');

                for (int i=0; i< mpuCount; i++)
                {
                    recieveArrayTemp = recieveArray[i];
                    recieveArrayTemp = recieveArrayTemp.Replace(" ","");
                    recieveArrayTemp = recieveArrayTemp.Replace("IMU", "");
                    recieveTemp = recieveArrayTemp.Split(':');
                    IMUNumber = int.Parse(recieveTemp[0]);
                    strData = recieveTemp[1].Split(',');

                    Q[IMUNumber - 1, 0] = float.Parse(strData[0]);
                    Q[IMUNumber - 1, 1] = float.Parse(strData[1]);
                    Q[IMUNumber - 1, 2] = float.Parse(strData[2]);
                    Q[IMUNumber - 1, 3] = float.Parse(strData[3]);
                }

                /* --------------------------------------- ESP32 原代码 ------------------------------------ */
                //returnData = returnData.Replace(" ", ""); // 去除空格
                //returnData = returnData.Replace("IMU", "");
                //recieveTemp = returnData.Split(':');    // 以":"为分隔符，对字符串进行拆分
                //IMUNumber = int.Parse(recieveTemp[0]);
                //strData = recieveTemp[1].Split(',');    //以","为分隔符进行拆分

                //Q[IMUNumber - 1, 0] = float.Parse(strData[0]);
                //Q[IMUNumber - 1, 1] = float.Parse(strData[1]);
                //Q[IMUNumber - 1, 2] = float.Parse(strData[2]);
                //Q[IMUNumber - 1, 3] = float.Parse(strData[3]);
                /* ---------------------------------------------------------------------------------- */

                //print(strData[0] + strData[1]+strData[2]+strData[3]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            OnApplicationQuit();
        }
    }

    private void OnApplicationQuit()
    {
        if (m_Thread != null)
        {
            m_Thread.Abort();
        }

        if (m_Client != null)
        {
            m_Client.Close();
        }
    }
    void udpSend()
    {
        var IP = IPAddress.Parse("192.168.137.10");

        int port = 1234;

        var udpClient1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var sendEndPoint = new IPEndPoint(IP, port);

        try
        {

            //Sends a message to the host to which you have connected.
            byte[] sendBytes = Encoding.ASCII.GetBytes("hello from unity");

            udpClient1.SendTo(sendBytes, sendEndPoint);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}