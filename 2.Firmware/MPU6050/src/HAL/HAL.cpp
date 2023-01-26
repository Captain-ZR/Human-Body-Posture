#include "HAL/HAL.h"

void HAL::Init()
{
    Serial.begin(115200);   // 串口初始化
    while (!Serial);        // 等待串口初始化完成

    HAL::WiFi_Init();
    
    HAL::OTA_Init();
    HAL::pin_Init();
    HAL::I2C_Init();
    HAL::IMU_Init();
}

void HAL::Update()
{
    HAL::OTA_Update();
    HAL::IMU_Uptate();
}