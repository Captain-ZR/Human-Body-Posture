#include "HAL/HAL.h"
#include <U8g2lib.h>
#include <Wire.h>

static U8G2_SSD1306_128X64_NONAME_F_HW_I2C u8g2(U8G2_R0, /* reset=*/U8X8_PIN_NONE, /* clock=*/4, /* data=*/16); 

void HAL::OLED_Init()
{
    u8g2.begin();
    u8g2.enableUTF8Print();
    u8g2.clearBuffer();

    u8g2.setFont(u8g2_font_unifont_t_chinese2);
    u8g2.setCursor(30, 35);
    u8g2.print("Hello World !");
    u8g2.sendBuffer();
}

void HAL::OLED_Update(int times)
{
    // u8g2.clearBuffer();
    // u8g2.setFont(u8g2_font_unifont_t_chinese2);
    // u8g2.setCursor(30, 35);
    // u8g2.print(times);
    // u8g2.sendBuffer();
}