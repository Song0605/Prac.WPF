using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Prac.WPF
{
    public class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;


        public void Execute(object? parameter)
        {
            DoExcute?.Invoke(parameter);
        }
        public Action<object> DoExcute { get; set; }
    }
}
