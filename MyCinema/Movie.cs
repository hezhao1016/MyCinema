using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCinema
{
    [Serializable]  //可序列化
    public class Movie  //电影类
    {
        public string Actor { get; set; }  //演员
        public string Director { get; set; } //导演
        public string MovieName { get; set; } //名称
        public MovieType MovieType { get; set; } //电影类型
        public string Poster { get; set; } //电影封面
        public double Price { get; set; } //价格

        public Movie()
        { }

        public Movie(string actor,string director,string movieName,MovieType type,string poster,double price)
        {
            this.Actor = actor;
            this.Director = director;
            this.MovieName = movieName;
            this.MovieType = type;
            this.Poster = poster;
            this.Price = price;
        }
    }
    /// <summary>
    /// 枚举  电影类型
    /// </summary>
    public enum MovieType
    {
        Comedy,
        War,
        Romance,
        Action,
        Cartoon,
        Thtiller,
        Adventure
    }
}
