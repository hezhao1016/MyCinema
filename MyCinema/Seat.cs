using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MyCinema
{
    [Serializable]  //可序列化
    public class Seat   //座位类
    {
        public Color Color { get; set; }   //座位颜色
        public string SeatNum { get; set; }  //座位编号
        public Seat(Color color, string seatNum)   //构造函数
        {
            this.Color = color;
            this.SeatNum = seatNum;
        }
    }
}
