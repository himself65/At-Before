using CountdownClass;
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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace At_Before.Pages.CountdownPage
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditPage : Page
    {
        List<Classification> ClassificationItems;
        List<Repeat> RepeatCaseItems;
        Countdown countdown = null;

        public EditPage()
        {
            this.InitializeComponent();
            Initialize();
        }
        private void Initialize()
        {
            ClassificationItems = CountdownManage.GetClassifications();
            RepeatCaseItems = CountdownManage.GetRepeats();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            countdown = e.Parameter as Countdown;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsCurrentInput()) return;
            if (countdown == null)
            {
                countdown = new Countdown()
                {
                    Title = TitleBox.Text,
                    Date = DatePicker.Date + TimePicker.Time,
                    Classification = new Classification((ClassificationCase)ClassficationBox.SelectedItem),
                    Repeat = new Repeat(),
                    AllDay = false,
                };
                CountdownManage.SaveToLocal(countdown);
            }
            else
            {
                countdown.Title = TitleBox.Text;
                countdown.Date = DatePicker.Date + TimePicker.Time;
                countdown.Classification = new Classification((ClassificationCase)ClassficationBox.SelectedItem);
                countdown.Repeat = new Repeat();
                countdown.AllDay = false;

                var items = CountdownManage.GetCountdowns();
                foreach(var item in items)
                {
                    if(item.ID==countdown.ID)
                    {
                        item.Title = countdown.Title;
                        item.Date = countdown.Date;
                        item.Classification = countdown.Classification;

                        CountdownManage.SaveCountdowns(items);
                        break;
                    }
                }
            }


            //
            Frame.GoBack();
        }

        private bool IsCurrentInput()
        {
            bool Current = true;
            if (String.IsNullOrWhiteSpace(TitleBox.Text))
                Current = false;
            if (ClassficationBox.SelectedIndex == -1)
                Current = false;
            Flyout fly = new Flyout();
            TextBlock block = new TextBlock()
            {
                Text = "请重新检查输入，标题选择不能为空值",
                TextWrapping = TextWrapping.Wrap
            };
            fly.Content = block;
            fly.ShowAt(this);
            return Current;
        }
    }
}
