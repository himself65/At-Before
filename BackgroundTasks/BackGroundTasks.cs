using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Web.Syndication;
using Microsoft.Toolkit.Uwp.Notifications;
using BackgroundTasks;
using CountdownClass;
using Windows.Storage;

namespace BackgroundTasks
{
    public sealed class BlogFeedBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {

            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            var Countdowns = GetCountdownList();

            UpdateTile(Countdowns);
            
            deferral.Complete();
        }
        public static void RunNow()
        {
            var Countdowns = GetCountdownList();

            UpdateTile(Countdowns);
        }

        private static List<Countdown> GetCountdownList()
        {
            List<Countdown> Countdowns = null;
            try
            {
                Countdowns = CountdownManage.GetCountdowns().ToList();
            }
            catch
            {
                throw new Exception("Can't Get List of countdowns");
            }
            return Countdowns;
        }

        private static void UpdateTile(List<Countdown> Countdowns)
        {
            //计数器，五次之后会退出循环
            int itemCount = 0;


            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);


            foreach (var item in Countdowns)
            {
                string Title = item.Title;
                string Classfication = item.Classification._Case_cn;
                string Date = "至" + item.Date.Year + "年" + item.Date.Month + "月" + item.Date.Day + "日";

                //比较实际天数，显示“不到”“大约”等词汇
                var TotalDays = item.EndLine.TotalDays;
                var TotalHours = item.EndLine.TotalHours;
                string EndLine = "还有" + (int)item.EndLine.TotalDays + "天";
                if ((int)TotalDays < TotalDays)
                {
                    EndLine = "不到" + (int)TotalDays + "天";
                }
                else
                {
                    EndLine = "还有大约" + (int)TotalDays;
                }
                if (TotalDays < 1)
                {
                    EndLine = "还有大约" + (int)TotalHours + "小时";
                    if ((int)TotalHours <= 0)
                    {
                        //事件过去
                        EndLine = "已经过去很长时间";
                    }
                }
                
                //
                var tileContent = new TileContent()
                {
                    Visual = new TileVisual()
                    {
                        TileSmall = new TileBinding()
                        {
                            Content = new TileBindingContentAdaptive()
                            {
                                TextStacking = TileTextStacking.Center,
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = Title,
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle= AdaptiveTextStyle.Body
                    },
                    new AdaptiveText()
                    {
                        Text = ((int)item.EndLine.TotalDays).ToString(),
                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
                        HintAlign = AdaptiveTextAlign.Left
                    }
                }
                            }
                        },
                        TileMedium = new TileBinding()
                        {
                            Branding = TileBranding.Name,
                            DisplayName = Title,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = Date,
                        HintWrap = true,
                        HintMaxLines = 2
                    },
                    new AdaptiveText()
                    {
                        Text = EndLine,
                        HintStyle = AdaptiveTextStyle.Base
                    }
                }
                            }
                        },
                        TileWide = new TileBinding()
                        {
                            Branding = TileBranding.NameAndLogo,
                            DisplayName = Title,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveText()
                    {
                        Text = Date
                    },
                    new AdaptiveText()
                    {
                        Text = Classfication,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },
                    new AdaptiveText()
                    {
                        Text = EndLine,
                        HintStyle = AdaptiveTextStyle.Title
                    }
                }
                            }
                        },
                        TileLarge = new TileBinding()
                        {
                            Branding = TileBranding.NameAndLogo,
                            DisplayName = Title,
                            Content = new TileBindingContentAdaptive()
                            {
                                Children =
                {
                    new AdaptiveGroup()
                    {
                        Children =
                        {
                            new AdaptiveSubgroup()
                            {
                                Children =
                                {
                                    new AdaptiveText()
                                    {
                                        Text = Date,
                                        HintWrap = true
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = Classfication,
                                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                                    },
                                    new AdaptiveText()
                                    {
                                        Text = EndLine,
                                        HintStyle = AdaptiveTextStyle.Title
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveText()
                    {
                        Text = ""
                    }
                }
                            }
                        }
                    }
                };

                var tileNotif = new TileNotification(tileContent.GetXml());

                updater.Update(tileNotif);
                //跳出循环
                itemCount++;
                if (itemCount > 5) break;
            }
        }

        private static TileUpdater GetUpdaterSettings()
        {
            ApplicationData applicationData = ApplicationData.Current;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            TileUpdater updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue((bool)localSettings.Values["testSetting"]);

            return updater;
        }
    }
}