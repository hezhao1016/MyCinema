using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyCinema
{
    [Serializable]  //可序列化
    public class TicKet         
    {
        public double Price { get; set; }  //票价格
        public ScheduleItem ScheduleItem { get; set; }  //放映场次
        public Seat Seat { get; set; } //座位

        public virtual void CalcPrice()//计算价格
        {
            this.Price = this.ScheduleItem.Movie.Price;
        }
        public virtual void Show()
        {

        }
        public virtual void Print()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\SoldTicKet")) //判断当前文件目录下指定的文件夹是否存在
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\SoldTicKet");// 创建当前目录下文件夹
            }
            string fileName = "SoldTicKet\\" +this.ScheduleItem.Movie.MovieName+"(普通票)-"+this.Seat.SeatNum+"-"+DateTime.Now.Millisecond+DateTime.Now.Second+".txt";
            fileName.Replace(":", "-");//替换字符
            FileStream fs = new FileStream(fileName,FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("***************************");
            sw.WriteLine("       " + "青鸟影院");
            sw.WriteLine("------------------------");
            sw.WriteLine("电影名：   " + this.ScheduleItem.Movie.MovieName);
            sw.WriteLine("时间：      " + this.ScheduleItem.Time);
            sw.WriteLine("座位号：   " + this.Seat.SeatNum);
            sw.WriteLine("价格：      " + this.ScheduleItem.Movie.Price);
            sw.WriteLine("**************************");
            sw.Close();
            fs.Close();
        }
        public TicKet() { }

        public TicKet(ScheduleItem item,Seat seat)
        {
            this.ScheduleItem = item;
            this.Seat = seat;
            CalcPrice();
        }
    }
}
