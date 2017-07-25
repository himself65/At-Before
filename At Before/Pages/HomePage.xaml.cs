using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Storage;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using CountdownClass;

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
            Initialize();
        }
        #region 初始化

        private void Initialize()
        {
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.LocalSettings;
            ClassificationItems = CountdownManage.GetClassifications();
            RepeatCaseItems = CountdownManage.GetRepeats();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            //页面加载是读取CountdownList
            CountdownList = CountdownManage.GetCountdowns();
        }
#endregion


        // 用户点下确定输入的按键
        private void CountdownItemFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            //如果输入错误
            if (InputTitleBox.Text.Length < 1 || InputClassIficationBox.SelectedIndex == -1 || InputRepeatCaseBox.SelectedIndex == -1 || InputDatePicker.Date == null) 
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
                    AllDay = (bool)InputTimeCheckBox.IsChecked,
                    });

                CountdownManage.SaveCountdowns(CountdownList);
            }
            InputButton.Flyout.Hide();
        }

        #region Input控件系列

        // 判断输入是否正确
        private void InputTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var InputBox = (TextBox)sender;
            string InputString = InputBox.Text;
            bool isEmptyOrNull = string.IsNullOrEmpty(InputString);

            
            bool InputError = isEmptyOrNull;
            if (InputError)
            {
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

        // 加载时默认选中第一项
        private void InputClassIficationBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputClassIficationBox.SelectedIndex = 0;
        }

        // 加载时默认选中第一项
        private void InputRepeatCaseBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputRepeatCaseBox.SelectedIndex = 0;
        }

        // 根据选择的改变来改变TimeBox
        private void InputRepeatCaseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var repeat = (Repeat)combox.SelectedItem;
            //
            //根据情况判断哪些Picker不可设置
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

        
        // 是否需要输入24小时
        private void InputTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            InputTimePicker.IsEnabled = false;
        }
        private void InputTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            InputTimePicker.IsEnabled = true;
        }
        #endregion


        // 用户选中某view的item后显示的编辑控件
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var Item = (Countdown)OutputListView.SelectedItem;
            if (Item == null)
                return;
            ChangeInputDatePicker.Date = Item.Date;
            ChangeInputTimePicker.Time = Item.Date.Offset;
            ChangeInputTitleBox.Text = Item.Title;
            ChangeInputTimeCheckBox.IsChecked = Item.AllDay;
            ChangeInputClassIficationBox.SelectedIndex = (int)Item.Classification.Case;
            ChangeInputRepeatCaseBox.SelectedIndex = (int)Item.Repeat.Case; 


            EditFinishedButton.IsEnabled = true;
        }

        #region EditButton的flyout控件

        // 将EditButton.flyout的内容保存到本地
        // 完成按键是EditFinishedButton
        private void EditFinishedButton_Click(object sender, RoutedEventArgs e)
        {
            Countdown NewOne = new Countdown()
            {
                ID = CountdownList[OutputListView.SelectedIndex].ID,
                Date = ChangeInputDatePicker.Date + ChangeInputTimePicker.Time,
                Title = ChangeInputTitleBox.Text,
                Classification = (Classification)ChangeInputClassIficationBox.SelectedItem,
                Repeat = (Repeat)ChangeInputRepeatCaseBox.SelectedItem,
                AllDay = (bool)ChangeInputTimeCheckBox.IsChecked,
            };
            CountdownList[OutputListView.SelectedIndex] = NewOne;

            // 隐藏flyout防止 throw
            EditButton.Flyout.Hide();
            EditButton.Visibility = Visibility.Collapsed;
        }

        // 将选中的item删除
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OutputListView.SelectedItem == null)
                return;
            var DeleteItem = CountdownList[OutputListView.SelectedIndex];

            CountdownManage.DeleteCountdownFromList(DeleteItem);

            CountdownList.RemoveAt(OutputListView.SelectedIndex);
            EditButton.Flyout.Hide();
            EditButton.Visibility = Visibility.Collapsed;
        }

        // 是否enable timepicker控件
        private void ChangeInputTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ChangeInputTimePicker.IsEnabled = false;
        }
        private void ChangeInputTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeInputTimePicker.IsEnabled = true;
        }


        #endregion

        

        
        // 下方显示的ListView
        private void OutputListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选中某项时候显示EditButton
            EditButton.Visibility = Visibility.Visible;
        }

        
        // 在退出的时候保存数据
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CountdownManage.SaveCountdowns(CountdownList);
        }


    }
}
