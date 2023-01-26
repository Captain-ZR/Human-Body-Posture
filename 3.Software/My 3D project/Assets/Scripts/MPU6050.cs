using System.Threading;
using UnityEngine;
using System.IO.Ports;
using System;

public class MPU6050 : MonoBehaviour
{

    public GameObject target;//Ŀ���������
    public int angle = 0;
    private SerialPort sp;
    private Thread recvThread;//�߳�
    //float w, x, y, z;//���ŷ����  
    char[] array;
    public float qw = 0, qx = 0, qy = 0, qz = 0;

    // Use this for initialization   
    void Start()
    {
        sp = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
        //���ڳ�ʼ��  
        if (!sp.IsOpen)
        {
            sp.Open();
        }
        recvThread = new Thread(ReceiveData); //���߳����ڽ��մ�������  
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

            //���е�ģʽ��ȡ��������
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
                /* print(s); //��ӡ��ȡ����ÿһ������*/
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();//�رմ���
    }

}