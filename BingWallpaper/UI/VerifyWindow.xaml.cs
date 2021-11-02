﻿using BingWallpaper.Core;
using BingWallpaper.Core.Abstractions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BingWallpaper
{
    /// <summary>
    /// VerifyWindow.xaml 的交互逻辑
    /// </summary>
    public partial class VerifyWindow : Window
    {
        private readonly EndpointDataBinding _bindings = new();

        public VerifyWindow()
        {
            InitializeComponent();
        }

        private void Wnd_verify_Loaded(object sender, RoutedEventArgs e)
        {
            Tb_endpoint.DataContext = _bindings;
        }

        private void Btn_set_Click(object sender, RoutedEventArgs e)
        {
            if (_bindings.Getter.SetBingWallpaperAPIEndpoint(Tb_endpoint.Text))
            {
                Close();
            }
            else
            {
                SwitchVerifyState(VerifyState.Failed);
            }
        }

        private void Btn_verify_Click(object sender, RoutedEventArgs e)
        {
            if (_bindings.Getter.VerifyBingWallpaperAPIEndpoint(Tb_endpoint.Text))
            {
                SwitchVerifyState(VerifyState.Succeed);
            }
            else
            {
                SwitchVerifyState(VerifyState.Failed);
            }
        }

        private void Tb_endpoint_TextChanged(object sender, TextChangedEventArgs e)
        {
            SwitchVerifyState(VerifyState.None);
        }

        private void SwitchVerifyState(VerifyState state)
        {
            if (state is VerifyState.Succeed)
            {
                Btn_verify.Content = "✔️";
                Btn_verify.SetResourceReference(StyleProperty, "ButtonSuccess");
            }
            else if (state is VerifyState.Failed)
            {
                Btn_verify.Content = "❌";
                Btn_verify.SetResourceReference(StyleProperty, "ButtonDanger");
            }
            else
            {
                Btn_verify.Content = "验证";
                Btn_verify.SetResourceReference(StyleProperty, "ButtonInfo");
            }
        }
    }

    public class EndpointDataBinding : INotifyPropertyChanged
    {
        private readonly IBingWallpaperGetter _getter = BingWallpaperGetter.Instance;

        internal IBingWallpaperGetter Getter { get => _getter; }

        public string BingWallpaperAPIEndpoint
        {
            get => _getter.GetBingWallpaperAPIEndpoint();
            set
            {
                _getter.SetBingWallpaperAPIEndpoint(value);

                if (PropertyChanged is not null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(BingWallpaperAPIEndpoint)));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
