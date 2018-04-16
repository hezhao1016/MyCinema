using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace MyCinema
{
    [Serializable]  //可序列化
    public class Schedule //放映日程类
    {
        public Dictionary<string,ScheduleItem> Items  { get; set; }  //场次集合

        public void LoadItems()
        {
            this.Items.Clear();
            //解析XML文档的场次对象
            if (!File.Exists("ShowList.xml"))
            {
                System.Windows.Forms.MessageBox.Show("系统文件已损坏......");
                return;
            }
            XmlDocument xml = new XmlDocument();
            xml.Load("ShowList.xml");
            XmlNode root = xml.DocumentElement;
            foreach (XmlNode xn in root.ChildNodes)
            {
                //创建电影对象
                Movie movie = new Movie(
                    xn["Actor"].InnerText,
                    xn["Director"].InnerText,
                    xn["Name"].InnerText,
                   (MovieType)Enum.Parse(typeof (MovieType),xn["Type"].InnerText), //转换为枚举
                    xn["Poster"].InnerText,
                   double.Parse(xn["Price"].InnerText)
                    );
                foreach (XmlNode xn1 in xn["Schedule"].ChildNodes)
                {
                    string time = xn1.InnerText;
                    ScheduleItem item = new ScheduleItem(movie,time);
                    this.Items.Add(movie.MovieName+"-"+time,item);
                }
            }
        }

        public Schedule() 
        {
            this.Items = new Dictionary<string, ScheduleItem>();
        }

        public Schedule(Dictionary<string, ScheduleItem> items)
        {
            this.Items = items;
        }
    }
}
