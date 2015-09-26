using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe.QcApp.Common.VO
{
	public class DetailVO
	{
		private string featureField;
		/// <summary>
		/// 要素属性字段
		/// </summary>
		public string FeatureField
		{
			get { return featureField; }
			set { featureField = value; }
		}
		private string featureValue;
		/// <summary>
		/// 要素属性值
		/// </summary>
		public string FeatureValue
		{
			get { return featureValue; }
			set { featureValue = value; }
		}
	}
}
