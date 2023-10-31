using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Modbus.Device;
using XFC.Model;
namespace XFC.Helper
{
    public  class NModubs4Helper
    {

        private static NModubs4Helper instance;
        public static NModubs4Helper Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        
        //串口对象
        private  static SerialPort serialPort = new SerialPort();
        //modbus通信对象
        public  static ModbusSerialMaster serialMaster = null;
        private SerialPortParams Params { set; get; }
       
        public NModubs4Helper()
        {
            
        }
        public NModubs4Helper(SerialPortParams param)
        {
            SetParams(param);
        }
        public bool Open()
        {

            if(Params != null) {

                try
                {
                    SetSerialPort();
                    serialPort.Open();
                    ConstantValue.gkStatus = GkStatus.Run;
                    
                    
                    serialMaster = ModbusSerialMaster.CreateRtu(serialPort);
                    return true;

                }
                catch(Exception ex)
                {
                    return false;
                    MessageBox.Show(ex.Message);
                }
               
            }
            else
            {
                MessageBox.Show("未设定串口通信参数，请检查");
                
            }
            return false;
            
            
        }
        public int GetValue16( byte slaveaddress, ushort startAddress) 
        {
            try
            {
                ushort[] data = serialMaster.ReadHoldingRegisters(slaveaddress, startAddress, 1);
                return data[0];
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public int GetValue32(byte slaveaddress, ushort startAddress)
        {
            try
            {
                ushort[] data = serialMaster.ReadHoldingRegisters(slaveaddress, startAddress, 2);
                return data[0];
            }
            catch(Exception ex)
            {
                return -1;
            }
           
        }
        public void Close()
        {
            try
            {
                
                serialPort.Close();
                ConstantValue.gkStatus = GkStatus.Selected;
            }
            catch( Exception ex ) 
            { 
                    MessageBox.Show(ex.ToString()); 
            }
         
            
        }
        public bool SetParams(SerialPortParams param)
        {
            if (param != null)
            {
                Params=param;
                return true;
            }
            return false;
        }
        public void SetSerialPort()
        {
            serialPort.BaudRate = Params.BaudRate;
            serialPort.DataBits = Params.DataBits;
            serialPort.StopBits = Params.StopBits;
            serialPort.PortName = Params.serialPortName;
            serialPort.Parity= Params.Parity;
        }
        public bool PortIsOpen()
        {
            if(serialPort.IsOpen) 
                return true;
            else 
                return false;
        }
    }
}
