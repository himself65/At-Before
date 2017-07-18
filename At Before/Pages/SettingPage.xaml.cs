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

namespace At_Before.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {

        ApplicationData applicationData = null;
        ApplicationDataContainer roamingSettings = null;

        public SettingPage()
        {
            this.InitializeComponent();
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.RoamingSettings;

        }

        private void DisplayOutPut()
        {

        }

        private void PasswordToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            //设置Flyout内容
            if (PasswordToggleSwitch.IsOn == true)
            {
                Flyout f = new Flyout();
                TextBlock HavenotFinished = new TextBlock();
                HavenotFinished.Text = "暂未开发完成";
                f.Content = HavenotFinished;
                f.Placement = FlyoutPlacementMode.Bottom;
                //显示未完成
                f.ShowAt(PasswordToggleSwitch);
                f.Closed += Flyout_Closed;
            }
        }

        private void Flyout_Closed(object sender, object e)
        {
            PasswordToggleSwitch.IsOn = false;
        }
    }
}
