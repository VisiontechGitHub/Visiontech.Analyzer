using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualizzatoreWPF.ViewModels
{
    public class ViewModel : BaseViewModel
    {


        public ICommand LoginCommand { get; }

        public ViewModel()
        {
            LoginCommand = new Command((parameter) => LoginAsync(), (parameter) => CanLogin);
        }

    }
}
