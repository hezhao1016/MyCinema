using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCinema
{
    [Serializable]  //可序列化
    public class ScheduleItem  //放映场次类
    {
        public Movie Movie { get; set; }  //电影
        public string Time { get; set; }//时间

        public ScheduleItem()
        { }

        public ScheduleItem(Movie movie,string time)
        {
            this.Movie = movie;
            this.Time = time;
        }
    }
}
