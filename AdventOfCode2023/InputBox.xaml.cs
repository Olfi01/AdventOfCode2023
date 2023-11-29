﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdventOfCode2023
{
    /// <summary>
    /// Interaktionslogik für InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public string Text { get => input.Text; set => input.Text = value; }

        public InputBox(string title, string message, bool multiline = false)
        {
            InitializeComponent();
            Title = title;
            messageLabel.Content = message;
            if (multiline)
            {
                input.AcceptsReturn = true;
                input.VerticalAlignment = VerticalAlignment.Stretch;
            }
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
