using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace At_Before
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FirstLoadPage : Page
    {
        object Parameter;
        ApplicationDataContainer settings = null;
        public FirstLoadPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Parameter = e.Parameter;
            settings = ApplicationData.Current.LocalSettings;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), Parameter);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (settings.Values.ContainsKey("FirstLoad"))
            {
                return;
            }
            else
            {
                settings.Values["FirstLoad"] = true;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (settings.Values.ContainsKey("FirstLoad"))
            {
                Frame.Navigate(typeof(MainPage), Parameter);
            }
        }
    }
}
