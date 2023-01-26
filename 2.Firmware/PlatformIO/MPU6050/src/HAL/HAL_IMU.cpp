#include "HAL/HAL.h"
// #include "MPU6050_6Axis_MotionApps20.h"
#include "MPU6050_6Axis_MotionApps_V6_12.h"
#include <WiFi.h>

static MPU6050 mpu;

extern WiFiUDP udp;
static IPAddress remoteIp(192, 168, 137, 1);
static unsigned int remoteUdpPort = 8888;  // 自定义远程监听端口

static unsigned long previousMillis = 0;
static const long period = 1000;

static uint16_t packetSize;
static uint16_t fifoCount;

static bool dmpReady = false;  // DMP状态
static int loopTimes;                  // 用于循环初始化和获取DMP数据
// static int i, j;
static int times = 0;
static uint8_t fifoBuffer[64]; // FIFO storage buffer
static uint8_t devStatus;      // 设备状态

// static float euler[3];         // [psi, theta, phi]    Euler angle container
// static float ypr[3];           // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector
// static float quat[12];

static Quaternion q;   
static String IMUData[10][9] = {"IMU1: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU2: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU3: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU4: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU5: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU6: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU7: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU8: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU9: " , "", ",", "", ",", "", ",", "", "\n",
                                "IMU10: ", "", ",", "", ",", "", ",", "", "\n"};

// 自定义方向矩阵，更改默认方向轴，适配不同安装位置
static signed char gyro_orientation[4][9] = {
                                                {0, 0, 1,   // 腰部、头部
                                                 0, 1, 0,   
                                                -1, 0, 0},

                                                {0, 1, 0,   // 腿部
                                                 0, 0, 1,   
                                                 1, 0, 0},
};

void HAL::IMU_Init()
{
    for(loopTimes=1; loopTimes <= IMUCount; loopTimes++)
    {
        SwitchPin(loopTimes);           // 轮询引脚控制
        MPU_Init(loopTimes);            // MPU初始化
    }    
}

void HAL::IMU_Uptate()
{
    for(loopTimes=1; loopTimes <= IMUCount; loopTimes++)
    {
        SwitchPin(loopTimes);           // 轮询引脚控制
        MPU_Update(loopTimes-1);        // 获取MPU数据
    }    
       
    // for(i=0; i<IMUCount; i++)       // 发送所有IMU数据
    // {
    //     udp.beginPacket(remoteIp, remoteUdpPort);//配置远端ip地址和端口
    //     for(j=0; j<9; j++)
    //     {
    //         // Serial.print(IMUData[i][j]);
    //         udp.print(IMUData[i][j]);
    //     }
    //     udp.endPacket();//发送数据
    // }

    // 刷新率计算
    times++;
    if(millis() - previousMillis >= period)
    {
        previousMillis = millis();
        Serial.print("times:");
        Serial.println(times);
        times = 0;
    }
}

void HAL::MPU_Init(int imuNumber)
{
    mpu.initialize();   // MPU初始化
    Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));

    /* DMP Init */
    Serial.print("IMU");
    Serial.print(loopTimes);
    Serial.print(": ");
    Serial.println(F("Initializing DMP..."));
    devStatus = mpu.dmpInitialize(); 

    // 设置方向矩阵
    if((imuNumber == 5) || (imuNumber == 6))
    {
        dmp_set_orientation(inv_orientation_matrix_to_scalar(gyro_orientation[0]));
    }
    else if(imuNumber > 6)
    {
        dmp_set_orientation(inv_orientation_matrix_to_scalar(gyro_orientation[1]));
    }

    dmpReady = true;

    mpu.setXGyroOffset(86);
    mpu.setYGyroOffset(26);
    mpu.setZGyroOffset(-11);
    mpu.setZAccelOffset(1788);

    if (devStatus == 0) 
    {
        mpu.CalibrateAccel(6, imuNumber);           // 校准程序，imuNunber用于修改校准轴
        mpu.CalibrateGyro(6, imuNumber);
        mpu.PrintActiveOffsets();                   // 输出校准信息
        mpu.setDMPEnabled(true);
        packetSize = mpu.dmpGetFIFOPacketSize();    // 获取dmp数据长度
    } 
    else 
    {
        // ERROR!
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
    }
}

void HAL::MPU_Update(int imuNumber)
{  
    fifoCount = mpu.getFIFOCount();                 // 读取FIFO缓冲区已填充大小
    
    if(fifoCount >= 1024)                           // 缓冲区满，清除FIFO
    {
        mpu.resetFIFO();
        Serial.println("FIFO overflow");
    }
    else
    {
        if (fifoCount < packetSize)                 // 没有数据写入或在写入时清除FIFO
        {
            return;
        }
        else
        {
            if(fifoCount % packetSize == 0)         // FIFO计数是否为42倍数，检查FIFO重置是否出错
            {
                // times++;

                mpu.getFIFOBytes(fifoBuffer, 14);   //读取FIFO前14位, 获得Quaternion数据

                mpu.resetFIFO();                    // 清空FIFO
                mpu.dmpGetQuaternion(&q, fifoBuffer);
                
                IMUData[imuNumber][1] = q.w;        // 将对应的Q值存入数组
                IMUData[imuNumber][3] = q.x;
                IMUData[imuNumber][5] = q.y;
                IMUData[imuNumber][7] = q.z;

                // IMU序号
                // Serial.print("IMU");
                // Serial.print(loopTimes);            // 打印IMU序号
                // Serial.print(": ");

                // Serial.print(q.w);
                // Serial.print(",");
                // Serial.print(q.x);
                // Serial.print(",");
                // Serial.print(q.y);
                // Serial.print(",");
                // Serial.println(q.z);

                udp.beginPacket(remoteIp, remoteUdpPort);   //配置远端ip地址和端口
                udp.print("IMU");
                udp.print(loopTimes);                       // 打印IMU序号
                udp.print(": ");

                udp.print(q.w);
                udp.print(",");
                udp.print(q.x);
                udp.print(",");
                udp.print(q.y);
                udp.print(",");
                udp.println(q.z);
                udp.endPacket();                            //发送数据
            }
            else
            {
                mpu.resetFIFO();
            }
        }
    }
}

void HAL::SwitchPin(int i)
{
    digitalWrite(IMU1, HIGH);
    digitalWrite(IMU2, HIGH);
    digitalWrite(IMU3, HIGH);
    digitalWrite(IMU4, HIGH);
    digitalWrite(IMU5, HIGH);
    digitalWrite(IMU6, HIGH);
    digitalWrite(IMU7, HIGH);
    digitalWrite(IMU8, HIGH);
    digitalWrite(IMU9, HIGH);
    digitalWrite(IMU10, HIGH);
    // digitalWrite(IMU11, HIGH);

    switch(i)
    {
        case 1:
        {
            digitalWrite(IMU1, LOW);
            break;
        }
        case 2:
        {
            digitalWrite(IMU2, LOW);    
            break;
        }
        case 3:
        {
            digitalWrite(IMU3, LOW);       
            break;
        }
        case 4:
        {
            digitalWrite(IMU4, LOW);               
            break;
        }
        case 5:
        {
            digitalWrite(IMU5, LOW);               
            break;   
        }
        case 6:
        {
            digitalWrite(IMU6, LOW);                        
            break;
        }
        case 7:
        {
            digitalWrite(IMU7, LOW);              
            break;
        }
        case 8:
        {
            digitalWrite(IMU8, LOW);               
            break;
        }
        case 9:
        {
            digitalWrite(IMU9, LOW);              
            break;
        }
        case 10:
        {
            digitalWrite(IMU10, LOW);              
            break;
        }
    }
}