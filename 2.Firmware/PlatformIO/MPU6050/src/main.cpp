#include "HAL/HAL.h"
#include "App/App.h"

void setup() 
{
    HAL::Init();
}

void loop() 
{
    HAL::Update();
    // delay(20);
}