#include "HAL/HAL.h"
#include <WiFi.h>

#define ssid      "ZRong"           // WiFi名称
#define password  "88888888"        // WiFi密码

static IPAddress localIP(192, 168, 137, 10);
static IPAddress gateway(192, 168, 137, 1);
static IPAddress subnet(255, 255, 255, 0);
static IPAddress DNS(192, 168, 137, 1); //optional

WiFiUDP udp;//实例化WiFiUDP对象
static unsigned int localUdpPort = 1234;  // 自定义本地监听端口
// char incomingPacket[255];  // 保存Udp工具发过来的消息

void HAL::WiFi_Init()
{
    Serial.printf("正在连接 %s ", ssid);

    WiFi.config(localIP, gateway, subnet, DNS, DNS);
    WiFi.begin(ssid, password);//连接到wifi

    while (WiFi.status() != WL_CONNECTED)//等待连接
    {
        delay(500);
        Serial.print(".");
    }
    Serial.println("连接成功");
    // Serial.println(WiFi.dnsIP());
    // Serial.println(WiFi.gatewayIP());

    if(udp.begin(localUdpPort))
    {//启动Udp监听服务
        Serial.println("监听成功");
        
        //打印本地的ip地址，在UDP工具中会使用到
        //WiFi.localIP().toString().c_str()用于将获取的本地IP地址转化为字符串   
        Serial.printf("现在收听IP: %s, UDP端口: %d\n", WiFi.localIP().toString().c_str(), localUdpPort);
    }
    else
    {
        Serial.println("监听失败");
    }
}