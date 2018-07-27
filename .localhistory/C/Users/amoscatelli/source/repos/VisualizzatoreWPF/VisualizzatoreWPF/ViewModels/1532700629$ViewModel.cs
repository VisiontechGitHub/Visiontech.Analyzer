﻿using System;
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
            LoadFileCommand = new Command((parameter) => LoginAsync(), (parameter) => !(parameter is null) && parameter is string[] && (parameter as string[]).Length > 0);
        }

        private void LoginAsync()
        {
            throw new NotImplementedException();
        }
    }
}
