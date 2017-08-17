using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CountdownClass;
using Windows.Storage;
using System.Collections.ObjectModel;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Core;
using At_Before.Sources;
using At_Before.Pages;
using At_Before.Pages.CountdownPage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace At_Before.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ShowCountdownPage : Page
    {

        ApplicationData applicationData = null;
        ApplicationDataContainer roamingSettings = null;

        ObservableCollection<Countdown> Countdowns = null;
        Countdown FirstCountdown = null;
        ObservableCollection<Countdown> OtherCountdowns = null;

        FrameControl frameControl = null;
        public ShowCountdownPage()
        {
            this.InitializeComponent();
            Initialize();
        }
        //返回菜单
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 得到frameControl的引用
            frameControl = (FrameControl)e.Parameter;
        }

        //初始化得到数据
        private void Initialize()
        {
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.LocalSettings;

            Countdowns = CountdownManage.GetCountdowns();

            if (Countdowns.Count != 0)
                FirstCountdown = Countdowns[0];

            OtherCountdowns = Countdowns;
            if (OtherCountdowns.Count != 0)
                OtherCountdowns.RemoveAt(0);

        }

        //实现背景z轴滚动
        private void MainScollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                CompositionPropertySet scrollerManipProps = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(MainScollViewer);
                Compositor compositor = scrollerManipProps.Compositor;
                ExpressionAnimation expression = compositor.CreateExpressionAnimation("scroller.Translation.Y * parallaxFactor");

                expression.SetScalarParameter("parallaxFactor", 0.3f);

                expression.SetReferenceParameter("scroller", scrollerManipProps);

                Visual backgroundVisual = ElementCompositionPreview.GetElementVisual(FirstCDImage);
                backgroundVisual.StartAnimation("Offset.Y", expression);
            }
        }


        //添加一个item
        private void AddBotton_Click(object sender, RoutedEventArgs e)
        {
            if (!frameControl.IsNarrow)
                frameControl.RightFrame.Navigate(typeof(EditPage), null);
            else
                frameControl.InViewFrame.Navigate(typeof(EditPage), null);
        }

        //立刻更新到磁贴
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            BackgroundTasks.BlogFeedBackgroundTask.RunNow();
        }

        //封面被按下时候
        private void HeadView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!frameControl.IsNarrow)
                frameControl.RightFrame.Navigate(typeof(ShowOneCountdownPage), FirstCountdown);
            else
                frameControl.InViewFrame.Navigate(typeof(ShowOneCountdownPage), FirstCountdown);
        }

        //列表被选择时
        private void OutputListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var Viewer = sender as ListView;
            if (Viewer.SelectedIndex == -1)
            {
                EditButton.Visibility = Visibility.Collapsed;
                return;
            }
            EditButton.Visibility = Visibility.Visible;
            if (!frameControl.IsNarrow)
                frameControl.RightFrame.Navigate(typeof(ShowOneCountdownPage), Viewer.SelectedItem);
            else
                frameControl.InViewFrame.Navigate(typeof(ShowOneCountdownPage), Viewer.SelectedItem);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (OutputListView.SelectedIndex == -1)
                return;
            if (!frameControl.IsNarrow)
                frameControl.RightFrame.Navigate(typeof(EditPage), OutputListView.SelectedItem);
            else
                frameControl.InViewFrame.Navigate(typeof(EditPage), OutputListView.SelectedItem);
        }
    }
}
