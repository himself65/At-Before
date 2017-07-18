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
using Windows.Storage;
using At_Before.Sources;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace At_Before.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        #region 各种数据
        ApplicationData applicationData;
        ApplicationDataContainer roamingSettings;
        List<Classification> ClassificationItems;
        List<Repeat> RepeatCaseItems;
        ObservableCollection<Countdown> CountdownList;
#endregion
        public HomePage()
        {
            this.InitializeComponent();
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.LocalSettings;
            ClassificationItems = CountdownManage.GetClassifications();
            RepeatCaseItems = CountdownManage.GetRepeats();
        }

        private void CountdownItemFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            //如果输入错误
            if (InputTitleBox.Text.Length <= 2 || InputClassIficationBox.SelectedIndex == -1 || InputDatePicker.Date == null) 
            {
                Flyout flyerror = new Flyout();
                TextBlock texterror = new TextBlock()
                {
                    Text = "请重新检查输入"
                };
                flyerror.Placement = FlyoutPlacementMode.Bottom;
                flyerror.Content = texterror;
                flyerror.ShowAt(CountdownItemFinishedButton);
            }
            else
            {
                CountdownList.Add(new Countdown() {
                    ID = CountdownList.Count,
                    Title = InputTitleBox.Text,
                    Date = InputDatePicker.Date,
                    //Classification = (Classification)InputClassIficationBox.SelectedItem
                    Classification = (Classification)InputClassIficationBox.SelectedItem,
                    Repeat = (Repeat)InputRepeatCaseBox.SelectedItem,
                    });

                CountdownManage.SaveCountdowns(CountdownList);
            }
        }

        private void InputTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var InputBox = (TextBox)sender;
            string InputString = InputBox.Text;
            bool isEmptyOrNull = string.IsNullOrEmpty(InputString);
            bool InputIsShort = false;
            if (InputString.Length <= 2)
                InputIsShort = true;

            bool InputError = isEmptyOrNull || InputIsShort;
            if (InputError)
            {
                if (InputIsShort)
                    JudgeInputTitleBox.Text = "输入内容过短（不得小于两个字），请重新输入";
                if (isEmptyOrNull)
                    JudgeInputTitleBox.Text = "标题不能为空";

                JudgeInputTitleBox.Visibility = Visibility.Visible;
                CountdownItemFinishedButton.IsEnabled = false;
            }
            else
            {
                JudgeInputTitleBox.Visibility = Visibility.Collapsed;
                CountdownItemFinishedButton.IsEnabled = true;
            }
        }

        private void CountdownItemFinishedButton_Loaded(object sender, RoutedEventArgs e)
        {
            CountdownItemFinishedButton.IsEnabled = false;
        }

        private void InputClassIficationBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputClassIficationBox.SelectedIndex = 0;
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            CountdownList = CountdownManage.GetCountdowns();
        }

        private void InputRepeatCaseBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputRepeatCaseBox.SelectedIndex = 0;
        }

        private void InputRepeatCaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = (ComboBox)sender;
            var repeat = (Repeat)combox.SelectedItem;
            switch (repeat.Case)
            {
                case RepeatCase.None:
                    InputDatePicker.YearVisible = true;
                    InputDatePicker.DayVisible = true;
                    InputDatePicker.MonthVisible = true;
                    break;
                case RepeatCase.EveryMonth:
                    InputDatePicker.YearVisible = false;
                    InputDatePicker.DayVisible = true;
                    InputDatePicker.MonthVisible = true;
                    break;
                case RepeatCase.EveryWeek:
                    InputDatePicker.YearVisible = false;
                    InputDatePicker.DayVisible = true;
                    InputDatePicker.MonthVisible = true;
                    break;
                case RepeatCase.EveryYear:
                    InputDatePicker.YearVisible = true;
                    InputDatePicker.DayVisible = true;
                    InputDatePicker.MonthVisible = true;
                    break;
                default:
                    throw new Exception("Error");
            }
        }
    }
}
