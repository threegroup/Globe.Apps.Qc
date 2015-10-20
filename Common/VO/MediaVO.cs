using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe.QcApp.Common.VO
{
	/// <summary>
	/// 多媒体资源对象，包括影像和图片
	/// </summary>
	public class MediaVO
	{
		private string name;
		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string imageUrl;
		/// <summary>
		/// 图片地址
		/// </summary>
		public string ImageUrl
		{
			get { return imageUrl; }
			set { imageUrl = value; }
		}
		private string videoUrl;
		/// <summary>
		/// 视频地址
		/// </summary>
		public string VideoUrl
		{
			get { return videoUrl; }
			set { videoUrl = value; }
		}
		private string date;
		/// <summary>
		/// 日期
		/// </summary>
		public string Date
		{
			get { return date; }
			set { date = value; }
		}
		private string desc;
		/// <summary>
		/// 描述
		/// </summary>
		public string Desc
		{
			get { return desc; }
			set { desc = value; }
		}
		private string address;
		/// <summary>
		/// 地址
		/// </summary>
		public string Address
		{
			get { return address; }
			set { address = value; }
		}
		private string id;
		/// <summary>
		/// 序号
		/// </summary>
		public string Id
		{
			get { return id; }
			set { id = value; }
		}
	}
}
