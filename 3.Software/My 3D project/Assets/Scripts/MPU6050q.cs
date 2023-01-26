using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class MPU6050q : MonoBehaviour
{

    SerialPort stream = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);

    //int count = 3;
    public string strReceived;

    public string[] strData = new string[12];
    public float[] strData_received = new float[12];
    public float[] Q1 = new float[4];
    public float[] Q2 = new float[4];
    public float[] Q3 = new float[4];
    public Quaternion quat = new Quaternion(1, 0, 0, 0);
    //public Vector3 angle = new Vector3();

    public GameObject figure1;
    public GameObject figure2;
    public GameObject figure3;

    //public Quaternion quat = new Quaternion(1, 0, 0, 0);

    void Start()
    {
        stream.Open(); //Open the Serial Stream.
        //stream.Write(" ");

        /*while (!(stream.ReadLine() == "DMP ready! Waiting for first interrupt..."))
        {
            print("...");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        strReceived = stream.ReadLine(); //Read the information  
        strData = strReceived.Split(',');

        if (strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != "")//make sure data are reday
        {
            strData_received[0] = float.Parse(strData[0]);
            strData_received[1] = float.Parse(strData[1]);
            strData_received[2] = float.Parse(strData[2]);
            strData_received[3] = float.Parse(strData[3]);
            //strData_received[4] = float.Parse(strData[4]);
            //strData_received[5] = float.Parse(strData[5]);
            //strData_received[6] = float.Parse(strData[6]);
            //strData_received[7] = float.Parse(strData[7]);

            Q1[0] = strData_received[0];
            Q1[1] = strData_received[1];
            Q1[2] = strData_received[2];
            Q1[3] = strData_received[3];
            //Q2[0] = strData_received[4];
            //Q2[1] = strData_received[5];
            //Q2[2] = strData_received[6];
            //Q2[3] = strData_received[7];

            figure1.transform.rotation = new Quaternion(-Q1[1], -Q1[3], -Q1[2], Q1[0]);
            //figure2.transform.rotation = new Quaternion(Q2[0], -Q2[2], Q2[3], Q2[1]);
            //figure3.transform.rotation = new Quaternion(Q3[0], -Q3[2], Q3[3], Q3[1]);

            print(Q1[0] + " " + Q1[1] + " " + Q1[2] + " " + Q1[3]);
            //print(Q2[0] + " " + Q2[1] + " " + Q2[2] + " " + Q2[3]);
            //print(Q3[0] + " " + Q3[1] + " " + Q3[2] + " " + Q3[3]);
        }



        //if (strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != ""
        //    && strData[4] != "" && strData[5] != "" && strData[6] != "" && strData[7] != "")//make sure data are reday
        //{
        //    strData_received[0] = int.Parse(strData[0]);
        //    strData_received[1] = int.Parse(strData[1]);
        //    strData_received[2] = int.Parse(strData[2]);
        //    strData_received[3] = int.Parse(strData[3]);
        //    strData_received[4] = int.Parse(strData[4]);
        //    strData_received[5] = int.Parse(strData[5]);
        //    strData_received[6] = int.Parse(strData[6]);
        //    strData_received[7] = int.Parse(strData[7]);

        //    q[0] = ((strData_received[0] << 8) | strData_received[1]) / 16384.0f;
        //    q[1] = ((strData_received[2] << 8) | strData_received[3]) / 16384.0f;
        //    q[2] = ((strData_received[4] << 8) | strData_received[5]) / 16384.0f;
        //    q[3] = ((strData_received[6] << 8) | strData_received[7]) / 16384.0f;
        //    for (int i = 0; i < 4; i++) if (q[i] >= 2) q[i] = -4 + q[i];

        //    quat = new Quaternion(q[0], -q[2], q[3], q[1]);
        //    //quat = new Quaternion(q[0], -q[1], q[3], -q[2]);
        //    print(q[0] + " " + q[1] + " " + q[2] + " " + q[3]);

        //    //float[] axis = quat.ToAxisAngle();



        //    transform.rotation = quat;

        //    print(q[0] + " ");
        //    print(q[1] + " ");
        //    print(q[2] + " ");
        //    print(q[3]);

        //}
    }
}