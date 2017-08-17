using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
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
using At_Before.Sources;
using At_Before.Pages;
using At_Before.Pages.CountdownPage;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace At_Before
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static int TableWidth = 769;

        List<HambugerItem> HambugerViewItems = null;
        List<HambugerItem> SettingViewItems = null;
        FrameControl frameControl = null;
        public MainPage()
        {
            this.InitializeComponent();
            Initailize();
        }
        private void Initailize()
        {
            frameControl = new FrameControl()
            {
                RightFrame = RightFrame,
                InViewFrame = InViewFrame,
                MainFrame = Frame,
                IsNarrow = false,
            };

            HambugerViewItems = ListItemManage.GetHambugerListItems();
            SettingViewItems = ListItemManage.GetSettingListItem();
            RightFrame.Navigate(typeof(HelloPage),frameControl);
            InViewFrame.Navigate(typeof(ShowCountdownPage), frameControl);
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;

            InViewFrame.Navigated += Frame_Navigated;
            RightFrame.Navigated += Frame_Navigated;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (RightFrame.CanGoBack || InViewFrame.CanGoBack)
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (frameControl == null)
                return;

            if (frameControl.RightFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                frameControl.RightFrame.GoBack();
            }
            if (frameControl.InViewFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                frameControl.InViewFrame.GoBack();
            }
        }

        private void HambugerButton_Click(object sender, RoutedEventArgs e)
        {
            LeftView.IsPaneOpen = !LeftView.IsPaneOpen;
        }

        private void HambugerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var item = (HambugerItem)HambugerListView.SelectedItem;
            if (item == null)
                return;
            if (HambugerListView.SelectedIndex == -1)
                return;
            
            InViewFrame.Navigate(item.Page, frameControl);

            SettingListView.SelectedIndex = -1;
        }

        private void SettingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SettingListView = sender as ListView;
            if (SettingListView.SelectedIndex == -1)
                return;

            var item = (HambugerItem)SettingListView.SelectedItem;

            InViewFrame.Navigate(item.Page, frameControl);

            HambugerListView.SelectedIndex = -1;
        }

        //页面大小改变时
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            bool IsNarrow = ActualWidth <= TableWidth;

            //如果宽窄屏模式没有变，那么不要执行后面代码
            if (IsNarrow == true && ActualWidth <= TableWidth)
                return;
            else if (IsNarrow == false && ActualWidth > TableWidth)
                return;

            frameControl.IsNarrow = ActualWidth <= TableWidth ? true : false;

            //右侧Frame什么也没有时候必须显示左侧
            if (RightFrame == null)
            {
                RightFrame.Navigate(typeof(HelloPage));
            }

            ////fixed
            ////判断宽模式下的Frame
            //if (InViewFrame.CurrentSourcePageType == typeof(ShowOneCountdownPage) && !IsNarrow)  
            //{
            //    if (InViewFrame.CanGoBack)
            //        InViewFrame.GoBack();
            //}

            //宽屏模式下的Frame转换
            if (FrameControl.CanChangeFrame(InViewFrame.CurrentSourcePageType) && IsNarrow == false)  
            {
                if (InViewFrame.CanGoBack)
                    InViewFrame.GoBack();
            }

            //判断窄屏模式
            if (FrameControl.CanChangeFrame(RightFrame.CurrentSourcePageType) && IsNarrow == true)
            {
                InViewFrame.Navigate(RightFrame.CurrentSourcePageType, frameControl);
                //fixed
                //if (InViewFrame.CurrentSourcePageType != typeof(ShowOneCountdownPage))
                //    InViewFrame.Navigate(typeof(ShowOneCountdownPage), frameControl);
            }
        }
        
        #region 后台任务代码
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShowBackButton();
            RegisterBackgroundTask();
        }

        private void ShowBackButton()
        {
            if (frameControl.RightFrame.CanGoBack || frameControl.InViewFrame.CanGoBack) 
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void RegisterBackgroundTask()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.AlwaysAllowed ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                    {
                        task.Value.Unregister(true);
                    }
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder()
                {
                    Name = taskName,
                    TaskEntryPoint = taskEntryPoint
                };
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                var registration = taskBuilder.Register();
            }
        }

        private const string taskName = "BlogFeedBackgroundTask";
        private const string taskEntryPoint = "BackgroundTasks.BlogFeedBackgroundTask";


        #endregion

 
    }
}
