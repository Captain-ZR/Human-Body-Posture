#include "HAL/HAL.h"
#include <Wire.h>

void HAL::I2C_Init()
{
    Wire.begin();
    Wire.setClock(400000); // 400kHz I2C clock. Comment this line if having compilation difficulties
}