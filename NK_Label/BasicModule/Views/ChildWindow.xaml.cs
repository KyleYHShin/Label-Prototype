﻿using BasicModule.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BasicModule.Views
{
    /// <summary>
    /// ChildWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChildWindow : Window
    {
        public ChildWindow()
        {
            InitializeComponent();
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var content = PART_ContentControl.Content;
            if (content is UserControl)
            {
                var iViewModel = (content as UserControl).DataContext as IOptionViewModel;
                if (iViewModel.isRight())
                    DialogResult = true;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
