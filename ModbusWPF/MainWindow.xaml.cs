using NModbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NModbus.Extensions.Enron;
using System.Windows.Controls;

//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Threading.Tasks;

//using NModbus.Extensions.Enron;
//using NModbus.Utility;

namespace ModbusWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort port;

        public MainWindow()
        {
            InitializeComponent();
            // Get a list of serial port names.
            string[] ports = SerialPort.GetPortNames();
            comPorts.ItemsSource = ports;
        }

        /// <summary>
        ///     Simple Modbus serial RTU slave example.
        /// </summary>
        private void StartModbusSerialRtuSlave(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                // using (SerialPort slavePort = new SerialPort("COM1"))
                {
                    // configure serial port
                    //slavePort.BaudRate = 9600;
                    //slavePort.DataBits = 8;
                    //slavePort.Parity = Parity.None;
                    //slavePort.StopBits = StopBits.One;
                    //slavePort.Open();

                    var factory = new ModbusFactory();
                    var slaveNetwork = factory.CreateRtuSlaveNetwork(port);

                    IModbusSlave slave1 = factory.CreateSlave(1);
                    IModbusSlave slave2 = factory.CreateSlave(2);

                    slaveNetwork.AddSlave(slave1);
                    slaveNetwork.AddSlave(slave2);

                    slaveNetwork.ListenAsync().GetAwaiter().GetResult();
                }
            });
        }


        /// <summary>
        ///     Simple Modbus serial RTU master write holding registers example.
        /// </summary>
        public void ModbusSerialRtuMasterWriteRegisters(object sender, RoutedEventArgs e)
        {
            using (port = new SerialPort(comPorts.SelectedItem.ToString()))
            {
                // configure serial port
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateRtuMaster(port);

                byte slaveId = 1;
                ushort startAddress = ushort.Parse(RegisterAddress3.Text);
                var registerValues = RegisterValues3.Text;
                ushort[] registers = registerValues.Split(',').Select(ushort.Parse).ToArray(); //new ushort[] { 1, 2, 3 }; 

                // write three registers
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
            }

        }

        public void ModbusSocketSerialMasterWriteRegisters(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // configure socket
                    var serverIP = IPAddress.Parse("127.0.0.1");
                    var serverFullAddr = new IPEndPoint(serverIP, 502);
                    sock.Connect(serverFullAddr);

                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(sock);

                    byte slaveId = 1;
                    ushort startAddress = ushort.Parse(RegisterAddress.Text);
                    ushort value = ushort.Parse(RegisterValue.Text);
                    master.WriteSingleRegister(slaveId, startAddress, value);

                    //  ushort[] registers = new ushort[] { 10, 20, 30 };

                    // write three registers
                    // master.WriteMultipleRegisters(slaveId, startAddress, registers);
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Simple Read registers using socket and expecting RTU fromatted response messages.
        /// </summary>
        public void ModbusSocketSerialMasterReadRegisters(object sender, RoutedEventArgs e)
        {

            try
            {
                using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    // configure socket
                    var serverIP = IPAddress.Parse("127.0.0.1");
                    var serverFullAddr = new IPEndPoint(serverIP, 502);
                    sock.Connect(serverFullAddr);

                    var factory = new ModbusFactory();
                    IModbusMaster master = factory.CreateMaster(sock);

                    byte slaveId = 1;
                    ushort startAddress = 100;
                    ushort numInputs = ushort.Parse(numberOfPoints.Text);

                    ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numInputs);

                    var registersValues = string.Empty;
                    for (int i = 0; i < numInputs; i++)
                    {
                        registersValues += $"{(startAddress + i)}={registers[i]},  ";
                    }

                    ReadRegisterValues.Text = registersValues;
                }


            }
            catch (Exception ex)
            {

            }

        }
    }
}
