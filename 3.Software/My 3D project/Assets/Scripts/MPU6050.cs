using System.Threading;
using UnityEngine;
using System.IO.Ports;
using System;

public class MPU6050 : MonoBehaviour
{

    public GameObject target;//目标操作物体
    public int angle = 0;
    private SerialPort sp;
    private Thread recvThread;//线程
    //float w, x, y, z;//存放欧拉角  
    char[] array;
    public float qw = 0, qx = 0, qy = 0, qz = 0;

    // Use this for initialization   
    void Start()
    {
        sp = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
        //串口初始化  
        if (!sp.IsOpen)
        {
            sp.Open();
        }
        recvThread = new Thread(ReceiveData); //该线程用于接收串口数据  
        recvThread.Start();
    }
    void Update()
    {
        //...
        //this.transform.localEulerAngles = new Vector3(axisY, axisP, axisR);
        transform.rotation = new Quaternion(qw, qx, qy, qz);
    }

    private void ReceiveData()
    {
        try
        {
            string s = "";
            char[] bt;
            int i, j, k = 0;
            string w = "", x = "", y = "", z = "";

            //以行的模式读取串口数据
            while ((s = sp.ReadLine()) != null)
            {
                k = 0;
                bt = s.ToCharArray();
                j = bt.GetLength(0);

                for (i = 0; i < j; i++)
                {
                    if (bt[i] == ',')
                    {
                        i++;
                        k++;
                    }
                    if (k == 0)
                    {
                        w += bt[i];
                    }
                    else if (k == 1)
                    {
                        x += bt[i];
                    }
                    else if (k == 2)
                    {
                        y += bt[i];
                    }
                    else if(k == 3)
                    {
                        z += bt[i];
                    }
                    else
                    {
                        k = 0;
                    }
                }
                qw = Convert.ToSingle(w);
                qx = Convert.ToSingle(x);
                qy = Convert.ToSingle(y);
                qz = Convert.ToSingle(z);

                w = "";
                x = "";
                y = "";
                z = "";
                print(qw + " " + qx + " " + qy + "" + qz);
                /* print(s); //打印读取到的每一行数据*/
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();//关闭串口
    }

}