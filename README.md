# Modbus
Modbus.org defined Modbus as follows...
> "MODBUS is an application-layer messaging protocol… It provides client/server communication between devices connected on different types of buses or networks."

According to WikiPedia...
> "Modbus is a data communications protocol originally published by Modicon (now Schneider Electric) in 1979 for use with its programmable logic controllers (PLCs). Modbus has become a de facto standard communication protocol and is now a commonly available means of connecting industrial electronic devices." [Read more](https://en.wikipedia.org/wiki/Modbus)

## How does a MODBUS work?
The devices which use MODBUS would communicate in a master/slave arrangement. In this type of communication, the master would communicate with one or multiple slaves. Mostly the master would be a PLC, PC, DCS, PAC, or RTU. The slaves are mostly the field devices, so in this configuration, the master would request to read or write a value. These values could be analog or digital, the slaves would respond to this request by giving the data to the master or by taking the action which is required. In case the slaves or Fieldbus wasn’t able to perform the requested action then the slave would create an error message and it would be transmitted to the master.

## Modbus Sample
This Modebus sample is the demonstration of how we can read and write into Registers using different Modbus communications types. 

![ModBus Sample Application UI](ModbusWPF/Docs/Images/ModbusSampleUI.png)

We can use Modbus Simulator [https://www.modbustools.com/modbus_slave.html](url) to test the sample.

This contains 3 sections. 

- Socket Serial Master Write Registers
  
- Socket Serial Master Read Registers
  
  ![Screenshot](ModbusWPF/Docs/Images/SocketSerialMasterReadRegisters.png)
  
- Serial Rtu Master WriteRegisters Using COM Port
  

