﻿using Microsoft.Plugin.WindowWalker.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Microsoft.Plugin.WindowWalker.Views
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : UserControl, IDisposable
    {
        public SettingWindow(Settings settings)
        {
            Settings = settings;
            InitializeComponent();
            QuickAccessWindowKeySet.Text = Settings.QuickAccessHotKey.ToString();
        }


        public Settings Settings { get; }
        private CancellationTokenSource updateSource;
        private bool disposedValue;

        private void OnAccessKeyChange(object sender, KeyEventArgs e)
        {
            updateSource?.Cancel();
            updateSource?.Dispose();

            updateSource = new();

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;


            updateSource = new CancellationTokenSource();
            if (key != Key.None &&
                key < Key.LeftShift &&
                Keyboard.Modifiers != ModifierKeys.None)
            {
                var currentKey = new KeyModel(key);
                QuickAccessWindowKeySet.Text = currentKey.ToString();
                Task.Delay(500).ContinueWith(_ =>
                Settings.QuickAccessHotKey = currentKey, updateSource.Token);
            }
            e.Handled = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    updateSource?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~SettingWindow()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
