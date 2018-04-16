using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;  //序列化

namespace MyCinema
{
    [Serializable]  //可序列化
    public class Cinema  //电影院类
    {
        public Schedule Schedule { get; set; }   //放映日程
        public Dictionary<string, Seat> Seats { get; set; }//座位集合
        public List<TicKet> SoldTickets { get; set; }  //已售电影票集合

        public Cinema() 
        {
            this.Schedule = new Schedule();
            this.Seats = new Dictionary<string, Seat>();
            this.SoldTickets = new List<TicKet>();
            CreateSeats();
        }
        public Cinema(Schedule schedule, Dictionary<string, Seat> seats, List<TicKet> soldTickets) 
        {
            this.Schedule = schedule;
            this.Seats = seats;
            this.SoldTickets = soldTickets;
			CreateSeats();
        }
		private void CreateSeats()
		{
			for (int i = 1; i <= 5; i++)
			{
				for (int j = 1; j <= 7; j++)
				{
					Seat s = new Seat(Color.Yellow, i + "-" + j); //创建座位对象
					this.Seats.Add(s.SeatNum,s);  //把座位放进电影院集合
				}
			}
		}
        public void Load(bool isNew)
        {
            //第一种做法，从文件中读取
            this.Schedule.LoadItems();
            if (!isNew)
            {
                if (!Directory.Exists(Environment.CurrentDirectory + "\\SaveTicket"))//判断当前文件目录下指定的文件夹是否存在
                {
                    return;
                }
                if (!File.Exists("SaveTicket\\SaveTicket.txt"))
                {
                    return;
                }
                FileStream fs = new FileStream("SaveTicket\\SaveTicket.txt", FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.Default);
                string line = sr.ReadLine();
                while (line.Trim() != "The End")
                {
                    Movie m = new Movie(
                        line, sr.ReadLine(), sr.ReadLine(),
                        (MovieType)Enum.Parse(typeof(MovieType), sr.ReadLine()), sr.ReadLine(), double.Parse(sr.ReadLine())
                        );
                    ScheduleItem si = new ScheduleItem(m, sr.ReadLine());
                    Seat s = new Seat(Color.Red, sr.ReadLine());
                    TicKet t = new TicKet(si, s);
                    this.SoldTickets.Add(t);
                    line = sr.ReadLine();
                }
                sr.Close();
                fs.Close();
            }
            else
            {
                this.SoldTickets.Clear();
            }

            //第二种，以序列化方式存储，再读取
            //if (isNew)
            //{
            //    this.Schedule.LoadItems();
            //}
            //else
            //{
            //    //从二进制文件读取信息
            //    if (File.Exists("SaveTicKet.bin"))
            //    {
            //        BinaryFormatter bf = new BinaryFormatter();
            //        FileStream fs = new FileStream("SaveTicKet.bin", FileMode.Open);
            //        Cinema c = (Cinema)bf.Deserialize(fs);
            //        this.Seats = c.Seats;
            //        this.SoldTickets = c.SoldTickets;
            //        this.Schedule = c.Schedule;
            //        fs.Close();
            //    }
            //}
        }
        public void Save()
        {
            //一、存储成文本文件
            if (!Directory.Exists(Environment.CurrentDirectory + "\\SaveTicket"))//判断当前文件目录下指定的文件夹是否存在
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\SaveTicket");// 创建当前目录下文件夹
            }
            FileStream fs = new FileStream("SaveTicket\\SaveTicket.txt", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs,Encoding.Default);
            foreach (TicKet t in this.SoldTickets)
            {
                sw.WriteLine(t.ScheduleItem.Movie.Actor);
                sw.WriteLine(t.ScheduleItem.Movie.Director);
                sw.WriteLine(t.ScheduleItem.Movie.MovieName);
                sw.WriteLine(t.ScheduleItem.Movie.MovieType);
                sw.WriteLine(t.ScheduleItem.Movie.Poster);
                sw.WriteLine(t.ScheduleItem.Movie.Price);
                sw.WriteLine(t.ScheduleItem.Time);
                sw.WriteLine(t.Seat.SeatNum);
            }
            sw.WriteLine("The End");
            sw.Close();
            fs.Close();


            //二、用序列器直接保存成二进制文件
            //BinaryFormatter bf = new BinaryFormatter();  // 序列器
            //FileStream fs = new FileStream("SaveTicKet.bin", FileMode.Create);
            //bf.Serialize(fs,this);   
            //fs.Close();
        }
    }
}
