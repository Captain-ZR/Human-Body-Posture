#include <Arduino.h>
#include "HAL/HAL.h"
#include "Tasks.h"

#include <U8g2lib.h>
#include <Wire.h>

static U8G2_SSD1306_128X64_NONAME_F_HW_I2C u8g2(U8G2_R2, /* reset=*/U8X8_PIN_NONE, /* clock=*/OLEDSCL, /* data=*/OLEDSDA); 

void TaskInit()
{
    xTaskCreate(Task1, "TaskOne", 8*1024, NULL, 1, NULL); 
    // xTaskCreate(Task2, "TaskTwo", 8*1024, NULL, 1, NULL); 
    // xTaskCreate(Task3, "TaskThree", 8*1024, NULL, 1, NULL); 
    // xTaskCreate(Task4, "TaskFour", 8*1024, NULL, 1, NULL); 
    // xTaskCreate(Task5, "TaskFive", 8*1024, NULL, 1, NULL); 

    // u8g2.begin();
    // u8g2.enableUTF8Print();
    // u8g2.clearBuffer();

    // u8g2.setFont(u8g2_font_unifont_t_chinese2);
    // u8g2.setCursor(30, 35);
    // u8g2.print("你好世界");
    // u8g2.sendBuffer();
}

void Task1(void *task1)
{
    while(1)
    {
        // HAL::IMU_Uptate(1);
        // Serial.println("1");
        delay(100);
    }
}

// void Task2(void *task2)
// {
//     while(1)
//     {
//         // Serial.println("2");
//         function(2);
//         delay(200);
//     }
// }

// void Task3(void *task3)
// {
//     while(1)
//     {
//         // Serial.println("3");
//         function(3);
//         delay(500);
//     }
// }

// void Task4(void *task4)
// {
//     while(1)
//     {
//         // Serial.println("4");
//         function(4);
//         delay(800);
//     }
// }

// void Task5(void *task5 )
// {
//     while(1)
//     {
//         // Serial.println("5");
//         function(5);
//         delay(1000);
//     }
// }

// void function(int i)
// {
//     Serial.print(i);
//     Serial.println(":hello");
// }