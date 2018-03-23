﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BasicModule.Models
{
    public class INotifyProperty
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
