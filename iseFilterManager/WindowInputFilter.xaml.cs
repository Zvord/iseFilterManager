﻿using System;
using System.Collections.Generic;
using System.Windows;
namespace iseFilterManager
{
    /// <summary>
    /// Логика взаимодействия для WindowInputFilter.xaml
    /// </summary>
    public partial class WindowInputFilter : Window
    {
        public WindowInputFilter(List<string> args)
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.CanMinimize;
            if (args.Count >= 1)
            {
                TextBox1.IsEnabled = true;
                TextBox1.Text = args[0];
            }
            if (args.Count >= 2)
            {
                TextBox2.IsEnabled = true;
                TextBox2.Text = args[1];
            }
            if (args.Count >= 3)
            {
                TextBox3.IsEnabled = true;
                TextBox3.Text = args[2];
            }
            if (args.Count >= 4)
            {
                TextBox4.IsEnabled = true;
                TextBox4.Text = args[3];
            }
            if (args.Count >= 5)
            {
                TextBox5.IsEnabled = true;
                TextBox5.Text = args[4];
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            List<string> args = new List<string>();
            if (TextBox1.IsEnabled)
                args.Add(TextBox1.Text);
            if (TextBox2.IsEnabled)
                args.Add(TextBox2.Text);
            if (TextBox3.IsEnabled)
                args.Add(TextBox3.Text);
            if (TextBox4.IsEnabled)
                args.Add(TextBox4.Text);
            if (TextBox5.IsEnabled)
                args.Add(TextBox5.Text);

            ((MainWindow)Application.Current.MainWindow).DialogTuple = new Tuple<bool, List<string>>(true, args);

            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).DialogTuple = new Tuple<bool, List<string>>(false, null);
            this.Close();
        }
    }
}
