#include "I2Cdev.h"
#include "Wire.h"
#include "HAL/HAL.h"

#define DINA4C 0x4c
#define DINACD 0xcd
#define DINA6C 0x6c

#define DINA0C 0x0c
#define DINAC9 0xc9
#define DINA2C 0x2c

#define DINA36 0x36
#define DINA56 0x56
#define DINA76 0x76

#define DINA26 0x26
#define DINA46 0x46
#define DINA66 0x66

#define FCFG_2 (1066)
#define FCFG_1 (1062)
#define FCFG_3 (1088)
#define FCFG_7 (1073)

// #define gyro_reg_bank_sel 0x6D
// #define gyro_reg_mem_r_w  0x6F

// static uint8_t gyro_mtx; // 0 for none, 1..4 for any of 0 to 3

unsigned short HAL::inv_row_2_scale(const signed char *row)   
{   
    unsigned short b;   
   
    if (row[0] > 0)   
        b = 0;   
    else if (row[0] < 0)   
        b = 4;   
    else if (row[1] > 0)   
        b = 1;   
    else if (row[1] < 0)   
        b = 5;   
    else if (row[2] > 0)   
        b = 2;   
    else if (row[2] < 0)   
        b = 6;   
    else   
        b = 7;      // error   
    return b;   
}   

unsigned short HAL::inv_orientation_matrix_to_scalar(const signed char *mtx)   
{   
    unsigned short scalar;    
    /*  
       XYZ  010_001_000 Identity Matrix  
       XZY  001_010_000  
       YXZ  010_000_001  
       YZX  000_010_001  
       ZXY  001_000_010  
       ZYX  000_001_010  
     */   
   
    scalar = inv_row_2_scale(mtx);   
    scalar |= inv_row_2_scale(mtx + 3) << 3;   
    scalar |= inv_row_2_scale(mtx + 6) << 6;   
   
    return scalar;   
} 

int HAL::dmp_set_orientation(unsigned short orient)
{
    unsigned char gyro_regs[3], accel_regs[3];
    const unsigned char gyro_axes[3] = {DINA4C, DINACD, DINA6C};
    const unsigned char accel_axes[3] = {DINA0C, DINAC9, DINA2C};
    const unsigned char gyro_sign[3] = {DINA36, DINA56, DINA76};
    const unsigned char accel_sign[3] = {DINA26, DINA46, DINA66};

    unsigned char tmp[2];

    gyro_regs[0] = gyro_axes[orient & 3];
    gyro_regs[1] = gyro_axes[(orient >> 3) & 3];
    gyro_regs[2] = gyro_axes[(orient >> 6) & 3];
    accel_regs[0] = accel_axes[orient & 3];
    accel_regs[1] = accel_axes[(orient >> 3) & 3];
    accel_regs[2] = accel_axes[(orient >> 6) & 3];

    tmp[0] = (unsigned char)(FCFG_1 >> 8);
    tmp[1] = (unsigned char)(FCFG_1 & 0xFF);
    I2Cdev::writeBytes(0x68, 0x6D, 2, tmp);
    I2Cdev::writeBytes(0x68, 0x6F, 3, gyro_regs);

    tmp[0] = (unsigned char)(FCFG_2 >> 8);
    tmp[1] = (unsigned char)(FCFG_2 & 0xFF);
    I2Cdev::writeBytes(0x68, 0x6D, 2, tmp);
    I2Cdev::writeBytes(0x68, 0x6F, 3, accel_regs);

    memcpy(gyro_regs, gyro_sign, 3);
    memcpy(accel_regs, accel_sign, 3);
    if (orient & 4) {
        gyro_regs[0] |= 1;
        accel_regs[0] |= 1;
    }
    if (orient & 0x20) {
        gyro_regs[1] |= 1;
        accel_regs[1] |= 1;
    }
    if (orient & 0x100) {
        gyro_regs[2] |= 1;
        accel_regs[2] |= 1;
    }

    tmp[0] = (unsigned char)(FCFG_3 >> 8);
    tmp[1] = (unsigned char)(FCFG_3 & 0xFF);
    I2Cdev::writeBytes(0x68, 0x6D, 2, tmp);
    I2Cdev::writeBytes(0x68, 0x6F, 3, gyro_regs);

    tmp[0] = (unsigned char)(FCFG_7 >> 8);
    tmp[1] = (unsigned char)(FCFG_7 & 0xFF);
    I2Cdev::writeBytes(0x68, 0x6D, 2, tmp);
    I2Cdev::writeBytes(0x68, 0x6F, 3, accel_regs);

    return 0;
}