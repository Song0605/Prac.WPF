using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Prac.WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Modbus.Device.ModbusIpMaster master;

        public event PropertyChangedEventHandler? PropertyChanged;

        private ushort _value;
        public ushort Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }

        private ushort _input;

        public ushort Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public ICommand BtnCommand { get; set; }


        public MainViewModel()
        {
            //如果做监控 持续拿寄存器值
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 502);
            //主站访问对象
            master = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);
            BtnCommand = new CommandBase() { DoExcute = DoBtnCommand };

            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(500);

                    //SlaveID Address Count
                    var values = master.ReadHoldingRegisters(1, 0, 1);//读湿度

                    Value = values[0];
                }
            });
        }
        private void DoBtnCommand(object obj)
        {
            master.WriteSingleRegister(1, 1, Input);
        }
    }
}
