using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCinema
{
    public class TicketUtil
    {
		
		
		//工厂模式
		public static TicKet CreateTicket(string type, ScheduleItem item, Seat seat,string customerName,double disCount) //买票
        {
			TicKet t = null;
				switch (type)
				{
				case "TicKet":
						t = new TicKet(item, seat);
					break;
				case "FreeTicKet":
					t = new FreeTicket(item, seat, customerName);
					break;
				case "StuTicKet":
					t = new StudentTicket(item, seat, disCount);
					break;
				}
				return t;
        }
    }
}
