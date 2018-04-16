using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyCinema
{
    [Serializable]  //可序列化
   public class StudentTicket:TicKet  //学生票
    {
       public double DisCount { get; set; }//折扣
       double a;
        public override void CalcPrice()//计算价格
        {
            base.Price = base.ScheduleItem.Movie.Price * this.DisCount;
        }
        public override void Show()
        {

        }
        public override void Print()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\SoldTicKet"))//判断当前文件目录下指定的文件夹是否存在
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\SoldTicKet");// 创建当前目录下文件夹
            }
            string fileName = "SoldTicKet\\" + this.ScheduleItem.Movie.MovieName + "(学生票)-" + this.Seat.SeatNum + "-" + DateTime.Now.Millisecond + DateTime.Now.Second + ".txt";
            fileName.Replace(":", "-"); //替换字符
            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("***************************");
            sw.WriteLine("       " + "青鸟影院(学生)");
            sw.WriteLine("------------------------");
            sw.WriteLine("电影名：   " + this.ScheduleItem.Movie.MovieName);
            sw.WriteLine("时间：      " + this.ScheduleItem.Time);
            sw.WriteLine("座位号：   " + this.Seat.SeatNum);
            sw.WriteLine("价格：      " + base.Price);
            sw.WriteLine("**************************");
            sw.Close();
            fs.Close();
        }
        public StudentTicket() { }

        public StudentTicket( ScheduleItem item, Seat seat, double disCount)
            : base(item, seat)
        {
            this.DisCount = disCount;
            CalcPrice();
        }
    }
}
