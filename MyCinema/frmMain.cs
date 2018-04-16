using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace MyCinema
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            MainLoad();
            ShowMovieList(false); // 放映列表
        }
		Dictionary<string,Label> lables = new Dictionary<string,Label> ();  //座位集合
		//初始化
        public void MainLoad()
		{
			this.lblDaoYan.Text = "";
			this.lblMovieName.Text = "";
			this.lblNewPrice.Text = "";
			this.lblOldPrice.Text = "";
			this.lblStar.Text = "";
			this.lblTime.Text = "";
			this.lblType.Text = "";
			this.picBoxMovie.Image = null;
			this.rbtnPT.Checked = true;
			this.rbtnPT.Checked = true;
			this.cboZheKou.Text = "7";
			this.cboZheKou.Enabled = false;
			CreateSeats();
		}
		//打印座位
		public void CreateSeats()
		{
			this.lables.Clear();//清空
			this.tabPage2.Controls.Clear();
			int y = 38;
			for (int i = 1; i <= 5; i++)
			{
			int x = 35;
				for (int j = 1; j <= 7; j++)
				{
					Label l = new Label ();
					l.BackColor = Color.Yellow;
					l.TextAlign = ContentAlignment.MiddleCenter;
					l.AutoSize = false;
					l.Text = i+"-"+j;
					l.Size = new Size (70,30);
					l.Location = new Point (x,y);
					l.Click += l_Click;
					this.tabPage2.Controls.Add(l);
					this.lables.Add(l.Text,l);//放进lable集合
					x+=85;
				}
				y+=47;
			}
		}

		void l_Click(object sender, EventArgs e)
		{
            if (this.lblTime.Text.Length == 0)
            {
                MessageBox.Show("请选择一部电影及场次！");
                return;
            }
			Label l = sender as Label;
			if (l.BackColor == Color.Red)
			{
				MessageBox.Show("对不起，该座位已经售出");
				return;
			}
            if (MessageBox.Show("是否购买？","购买提示",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
			string type = "";
			if (this.rbtnPT.Checked)
			{
				type = "TicKet";
			}
			else if (this.rbtnZP.Checked)
			{
                type = "FreeTicKet";
			}
			else
			{
                type = "StuTicKet";
			}
            ScheduleItem s = null;
            //判断选中的是哪个节点
            if (this.treeView1.SelectedNode.Level == 0)
            {
                s = this.treeView1.SelectedNode.Nodes[0].Tag as ScheduleItem;//取当前节点下的第一个子节点的tag
            }
            else
            {
                s = this.treeView1.SelectedNode.Tag as ScheduleItem;
            }
			TicKet t = TicketUtil.CreateTicket(type,s,this.c.Seats[l.Text],this.txtPerson.Text,double.Parse(this.cboZheKou.Text)*0.1);
            this.c.SoldTickets.Add(t);
            this.c.Seats[l.Text].Color = Color.Red;//变颜色
            l.BackColor = Color.Red;//显示颜色
            t.Print();//打印
        }

        //若选中增票单选框，则赠送人框变得可用
        private void rbtnZP_CheckedChanged(object sender, EventArgs e)
        {
			if (this.lblOldPrice.Text.Length == 0)
			{
				return;
			}
			if (this.rbtnZP.Checked)
            {
                this.txtPerson.Enabled = true;
                this.lblNewPrice.Text = "0";
            }
            else
            {
                this.txtPerson.Enabled = false;
            }
        }
        //若选中学生票单选框，则折扣下拉框变得可用
        private void rbtnStudent_CheckedChanged(object sender, EventArgs e)
        {
			if (this.lblOldPrice.Text.Length == 0)
			{
				return;
			}
            if (this.rbtnStudent.Checked)
            {
                this.cboZheKou.Enabled = true;
                ColaPrice();
            }
            else
            {
                this.cboZheKou.Enabled = false;
            }
        }
		//选中普通单选框时，优惠价格为当前价格
		private void rbtnPT_CheckedChanged(object sender, EventArgs e)
		{
			if (this.lblOldPrice.Text.Length == 0)
			{
				return;
			}
			if (this.rbtnPT.Checked == true)
			{
				this.lblNewPrice.Text = this.lblOldPrice.Text;
			}
		}
		//折扣改变，价格改变
		private void cboZheKou_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.lblOldPrice.Text.Length == 0)
			{
				return;
			}
			ColaPrice();
		}
        private void 放映列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
			MainLoad();
            ShowMovieList(true);
        }
        Cinema c = new Cinema();//电影院集合
		/// <summary>
		/// 显示放映列表
		/// </summary>
        public void ShowMovieList(bool isNew)
        {
            this.treeView1.Nodes.Clear();
            this.c.Load(isNew);
            Movie m = null;
            TreeNode node = new TreeNode ();
            foreach (ScheduleItem s in this.c.Schedule.Items.Values)
            {
                if (m!=s.Movie)
                {
                    m = s.Movie;
                    node = new TreeNode(s.Movie.MovieName);
                    node.Tag = s.Movie;
                    this.treeView1.Nodes.Add(node);
                }
                TreeNode nodeTime = new TreeNode(s.Time);
				nodeTime.Tag = s;
                node.Nodes.Add(nodeTime);
            }
            this.treeView1.Nodes[0].ExpandAll();
            this.treeView1.SelectedNode = this.treeView1.Nodes[0].Nodes[0];
        }
		/// <summary>
		/// 计算优惠价格的方法
		/// </summary>
		public void ColaPrice()
		{
			if (this.treeView1.SelectedNode.Level == 0)
			{
				this.lblNewPrice.Text = ((this.treeView1.SelectedNode.Tag as Movie).Price * double.Parse(this.cboZheKou.Text) * 0.1).ToString();
			}
			else
			{
				this.lblNewPrice.Text = ((this.treeView1.SelectedNode.Tag as ScheduleItem).Movie.Price * double.Parse(this.cboZheKou.Text) * 0.1).ToString();
			}
		}
		//AfterSelect事件
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
			Movie m = null;
            ScheduleItem s = null;
            if (this.treeView1.SelectedNode.Level == 0)
            {
                if (this.treeView1.SelectedNode.Nodes.Count >=0)
                {
                    s = this.treeView1.SelectedNode.Nodes[0].Tag as ScheduleItem;
                }
            } 
			else
			{
				s = this.treeView1.SelectedNode.Tag as ScheduleItem;
			}
            m = s.Movie;
            this.lblTime.Text = s.Time;
			this.lblMovieName.Text = m.MovieName;
			this.lblDaoYan.Text = m.Director;
			this.lblStar.Text = m.Actor;
			this.lblType.Text = m.MovieType.ToString();
			this.lblOldPrice.Text = m.Price.ToString();
			//Image image;
			//image = new Bitmap("Poster\\" + m.Poster);  //System.Drawing命名空间，图片地址
			//this.picBoxMovie.Image = image;
			this.picBoxMovie.Image = Image.FromFile("Poster\\" + m.Poster);  //图片地址
			if (this.rbtnStudent.Checked)
				ColaPrice(); //计算价格
			else if(this.rbtnPT.Checked)
				this.lblNewPrice.Text = this.lblOldPrice.Text;
			else
				this.lblNewPrice.Text = "0";
            //切换电影时，放映厅座位也一并切换，并显示当前电影场次的售出状态
            //一、先把所有座位对象和lable颜色变为黄色
            foreach (Seat seat in this.c.Seats.Values)
            {
                seat.Color = Color.Yellow;
                this.lables[seat.SeatNum].BackColor = Color.Yellow;
            }

            //二、再把售出的票颜色变红
            //判断电影名字及时间   但如果是第二种做法就是判断对象是否相同
            foreach (TicKet t in this.c.SoldTickets)
            {
                if (this.treeView1.SelectedNode.Level == 0)
                {
                    if (t.ScheduleItem.Movie.MovieName == (this.treeView1.SelectedNode.Nodes[0].Tag as ScheduleItem).Movie.MovieName && t.ScheduleItem.Time == (this.treeView1.SelectedNode.Nodes[0].Tag as ScheduleItem).Time)
                    {
                        t.Seat.Color = Color.Red;
                        this.lables[t.Seat.SeatNum].BackColor = Color.Red;
                    }
                }
                else
                {
                    if (t.ScheduleItem.Time == (this.treeView1.SelectedNode.Tag as ScheduleItem).Time && t.ScheduleItem.Movie.MovieName == (this.treeView1.SelectedNode.Tag as ScheduleItem).Movie.MovieName)
                    {
                        t.Seat.Color = Color.Red;
                        this.lables[t.Seat.SeatNum].BackColor = Color.Red;
                    }
                }
            }
            ////二、再把售出的票颜色变红
            //foreach (TicKet t in this.c.SoldTickets)
            //{
            //    if (this.treeView1.SelectedNode.Level == 0)
            //    {
            //        if (t.ScheduleItem == this.treeView1.SelectedNode.Nodes[0].Tag as ScheduleItem)
            //        {
            //            t.Seat.Color = Color.Red;
            //            this.lables[t.Seat.SeatNum].BackColor = Color.Red;
            //        }
            //    }
            //    else
            //    {
            //        if (t.ScheduleItem == this.treeView1.SelectedNode.Tag as ScheduleItem)
            //        {
            //            t.Seat.Color = Color.Red;
            //            this.lables[t.Seat.SeatNum].BackColor = Color.Red;
            //        }
            //    }
            //}
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.c.Save();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.c.Save();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
                                     /*当前目录*/
            if (Directory.Exists(Environment.CurrentDirectory + "\\SoldTicKet"))
            {
                Directory.Delete(Environment.CurrentDirectory+ "\\SoldTicKet", true);
                MessageBox.Show("记录删除成功");
            }
        }
    }
}
