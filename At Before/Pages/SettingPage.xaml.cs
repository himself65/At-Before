using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
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

namespace At_Before.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        ApplicationData applicationData = null;
        ApplicationDataContainer localSettings = null;
        public SettingPage()
        {
            this.InitializeComponent();
            applicationData = ApplicationData.Current;
            localSettings = ApplicationData.Current.LocalSettings;
            DisplayOutPut();
        }

        private void DisplayOutPut()
        {
            PasswordToggleSwitch.IsOn = (bool)localSettings.Values["testSetting"];
        }


        private void Page_UnLoad(object sender, RoutedEventArgs e)
        {
            localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["testSetting"] = PasswordToggleSwitch.IsOn;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTasks.BlogFeedBackgroundTask.RunNow();
        }
    }
}
