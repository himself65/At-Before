using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Storage;
using Windows.UI.StartScreen;
using Windows.ApplicationModel.Background;

namespace At_Before.Sources
{
    class TitleUpdateManage
    {
        public static void UpdateTitle(ObservableCollection<Countdown> Countdowns)
        {
            TileContent content = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                             {
                                  new AdaptiveText() { Text = Countdowns[0].Title },
                                  new AdaptiveText() { Text = Countdowns[0].EndLine.TotalDays.ToString()},
                              }
                        }
                    },

                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                {
                    new AdaptiveText() { Text = "Medium" }
                }
                        }
                    },

                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                {
                    new AdaptiveText() { Text = "Wide" }
                }
                        }
                    },

                    TileLarge = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            Children =
                {
                    new AdaptiveText() { Text = "Large" }
                }
                        }
                    }
                }
            };             // Create the tile notification

            var notification = new TileNotification(content.GetXml());

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);

            //SecondaryTile secondTile = new SecondaryTile("SecondaryTile");
            if (SecondaryTile.Exists("SecondaryTile"))
            {
                // Get its updater
                var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile("MySecondaryTile");

                // And send the notification
                updater.Update(notification);
            }
            

        }

        private const string LIVETILETASK = "LIVETILETAKS";
        private async void RegisterLiveTileTask()
        {
            var status = await BackgroundExecutionManager.RequestAccessAsync();
            if (status == BackgroundAccessStatus.Unspecified || status == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == LIVETILETASK)
                {
                    task.Value.Unregister(true);
                }
            }

            var taskBuilder = new BackgroundTaskBuilder
            {
                Name = LIVETILETASK,
                TaskEntryPoint = typeof(TitleUpdate).FullName
            };
            taskBuilder.AddCondition(new SystemCondition(SystemConditionType.InternetAvailable));

            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.Clear();
            var updater2 = TileUpdateManager.CreateTileUpdaterForSecondaryTile("appdota2");
            updater2.Clear();
            taskBuilder.SetTrigger(new TimeTrigger(60, false));
            taskBuilder.Register();
        }
    }

}
