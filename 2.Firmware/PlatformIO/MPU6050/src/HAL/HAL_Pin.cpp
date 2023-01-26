#include "HAL/HAL.h"

void HAL::pin_Init()
{
    pinMode(IMU1, OUTPUT);
    pinMode(IMU2, OUTPUT);
    pinMode(IMU3, OUTPUT);
    pinMode(IMU4, OUTPUT);
    pinMode(IMU5, OUTPUT);
    pinMode(IMU6, OUTPUT);
    pinMode(IMU7, OUTPUT);
    pinMode(IMU8, OUTPUT);
    pinMode(IMU9, OUTPUT);
    pinMode(IMU10, OUTPUT);
    // pinMode(IMU11, OUTPUT);

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
}