using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualizzatoreWPF.ViewModels
{
    public class ViewModel : BaseViewModel
    {


        public ICommand LoadFileCommand { get; }

        public ViewModel()
        {
            LoginCommand = new Command((parameter) => LoginAsync(), (parameter) => CanLogin);
        }

    }
}
