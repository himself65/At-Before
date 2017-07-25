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
#region 初始化
        public HomePage()
        {
            this.InitializeComponent();
            applicationData = ApplicationData.Current;
            roamingSettings = applicationData.LocalSettings;
            ClassificationItems = CountdownManage.GetClassifications();
            RepeatCaseItems = CountdownManage.GetRepeats();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            CountdownList = CountdownManage.GetCountdowns();
        }
#endregion

        /// <summary>
        /// 用户点下确定输入的按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void InputClassIficationBox_Loaded(object sender, RoutedEventArgs e)
        {
            InputClassIficationBox.SelectedIndex = 0;
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

        /// <summary>
        /// 是否需要输入24小时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            InputTimePicker.IsEnabled = false;
        }
        private void InputTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            InputTimePicker.IsEnabled = true;
        }
#endregion

        /// <summary>
        /// 用户选中某view的item后显示的编辑控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 此控件将内容写到flyout编辑
        /// 完成按键是EditFinishedButton
        /// 
        /// 完成都都需要隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            CountdownManage.SaveCountdowns(CountdownList);

            EditButton.Flyout.Hide();
            EditButton.Visibility = Visibility.Collapsed;
        }

        //删除控件
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

        private void ChangeInputTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ChangeInputTimePicker.IsEnabled = false;
        }

        private void ChangeInputTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeInputTimePicker.IsEnabled = true;
        }


        #endregion


        /// <summary>
        /// 下方显示的ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 在退出的时候保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            CountdownManage.SaveCountdowns(CountdownList);
        }


    }
}
