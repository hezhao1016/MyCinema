using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyCinema
{
    [Serializable]  //可序列化
   public class FreeTicket:TicKet  //赠票类
    {
        public string CustomerName { get; set; }//赠送者姓名
        double a;
        public override void CalcPrice()//计算价格
        {
            base.Price = 0;
            a = base.Price;
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
            string fileName = "SoldTicKet\\" + this.ScheduleItem.Movie.MovieName + "(赠票)-" + this.Seat.SeatNum + "-" + DateTime.Now.Millisecond + DateTime.Now.Second + ".txt";
            fileName.Replace(":", "-");//替换字符
            FileStream fs = new FileStream(fileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("***************************");
            sw.WriteLine("       " + "青鸟影院(赠票)");
            sw.WriteLine("------------------------");
            sw.WriteLine("电影名：   " + this.ScheduleItem.Movie.MovieName);
            sw.WriteLine("时间：      " + this.ScheduleItem.Time);
            sw.WriteLine("座位号：   " + this.Seat.SeatNum);
            sw.WriteLine("姓名：      " + this.CustomerName);
            sw.WriteLine("**************************");
            sw.Close();
            fs.Close();
        }
        public FreeTicket() { }
        public FreeTicket( ScheduleItem item, Seat seat, string customerName)
            : base( item, seat)
        {
            this.CustomerName = customerName;
            CalcPrice();
        }
    }   
}
