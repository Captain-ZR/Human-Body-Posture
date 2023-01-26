#ifndef _HAL_H
#define _HAL_H

#include <Arduino.h>
#include "App/Configs/Config.h"

namespace HAL
{
    void Init();
    void Update();

/* pinMode */
    void pin_Init();

/* OLED */
    void OLED_Init();
    void OLED_Update(int times);

/* WiFi */
    void WiFi_Init();

/* OTA */
    void OTA_Init();
    void OTA_Update();
    

/* I2C */
    void I2C_Init();

/* IMU */
    void IMU_Init();
    void IMU_Uptate();
    void MPU_Init(int imuNumber);
    void MPU_Update(int imuNumber);
    void SwitchPin(int i);

/* Set Orientation*/
    unsigned short inv_row_2_scale(const signed char *row);   
    unsigned short inv_orientation_matrix_to_scalar(const signed char *mtx);   
    int dmp_set_orientation(unsigned short orient);

/* ??? */
}

#endif