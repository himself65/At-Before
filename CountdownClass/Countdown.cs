using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownClass
{
    public class Countdown
    {
        // ID
        // 说明：数据存储至本地使用
        public int ID { get; set; }
        // Title
        // 倒数日的标题
        public string Title { get; set; }
        // Date
        // 倒数日的日期
        public DateTimeOffset Date { get; set; }
        // AllDay
        // 是否为全天事件
        public bool AllDay { get; set; }
        // 倒计时类型
        // 事件、爱情、工作等
        public Classification Classification { get; set; }
        // 重复类型
        //
        public Repeat Repeat { get; set; }
        // 间隔时刻
        // 当前时间减去Date时间
        public TimeSpan EndLine { get => Date - DateTimeOffset.Now; }

        // 将EndLine转化成各种内容
        public string DateToString { get => Date.ToString("yyy-MM-dd hh:mm"); }
        public double EndLineToDays { get => (Date - DateTimeOffset.Now).TotalDays; }
        public string EndLineToStringOfDays { get => ToStringOfDays(EndLineToDays); }

        public static string ToStringOfDays(double Days)
        {
            string Content = String.Empty;
            if ((int)Days < Days)
            {
                Content = "不到" + (int)Days;
                if ((int)Days <= 0)
                {
                    Content = "不到1";
                }
            }
            else
            {
                Content = "大约" + (int)Days;
            }

            return Content + "天";
        }
    }
    public class Classification
    {
        private static string[] States = { "Event", "Life", "Love", "Birthday", "Festival", "Entertainment", "Work", "Stuty", "Others" };
        private static string[] Translation = { "事件", "生活", "爱情", "生日", "节日", "娱乐", "工作", "学习", "其他" };

        public ClassificationCase Case { get; set; }
        public string _Case { get => Enum.Format(typeof(ClassificationCase), Case, "G"); }
        public string _Case_cn{ get => Translation[Convert.ToInt32(Case)]; }
        public Classification(ClassificationCase Case)
        {
            this.Case = Case;
        }
        public Classification()
        {
            this.Case = ClassificationCase.Event;
        }
    }
    public class Repeat
    {
        public RepeatCase Case { get; set; }
        public string _Case { get => Enum.Format(typeof(RepeatCase), Case, "G"); }
        public string _Case_cn { get => Translation[Convert.ToInt32(Case)]; }
        private static string[] Translation = { "不重复", "每周", "每月", "每年" };

        public Repeat(RepeatCase Case)
        {
            this.Case = Case;
        }
        public Repeat()
        {
            this.Case = RepeatCase.None;
        }
    }

    /// <summary>
    /// Enum数据
    /// </summary>
    public enum ClassificationCase
    {
        Event = 0,
        Life = 1,
        Love = 2,
        Birthday = 3,
        Festival = 4,
        Entertainment = 5,
        Work = 6,
        Stuty = 7,
        Others = 8
    }
    public enum RepeatCase
    {
        None,
        EveryWeek,
        EveryMonth,
        EveryYear,
    };
}
