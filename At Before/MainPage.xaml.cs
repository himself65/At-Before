using At_Before.Sources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace At_Before
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            //FirstFrame.Navigate(typeof(At_Before.Pages.HelloPage));
        }

        private void NavLinksList_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            HambugerSplitView.IsPaneOpen = !HambugerSplitView.IsPaneOpen;
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SettingList.SelectedItem = null;
            if (Home.IsSelected)
            {
                FirstFrame.Navigate(typeof(Pages.HomePage));
            }
            else if (AboutMe.IsSelected)
            {
                FirstFrame.Navigate(typeof(Pages.AboutMe));
            }
            HambugerSplitView.IsPaneOpen = false;

        }

        private void SettingItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MenuList.SelectedItem = null;
            SettingList.SelectedIndex = 0;
            FirstFrame.Navigate(typeof(At_Before.Pages.SettingPage));
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {

        }
    }
}
