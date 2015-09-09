using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe.QcApp.Common.VO
{
    public class RouteVO
    {
        private string routeName;

        /// <summary>
        /// 路径名称
        /// </summary>
        public string RouteName
        {
            get { return routeName; }
            set { routeName = value; }
        }
        private string routePath;

        /// <summary>
        /// 路径文件路径
        /// </summary>
        public string RoutePath
        {
            get { return routePath; }
            set { routePath = value; }
        }
        private int routeCode;

        /// <summary>
        /// 路径编码
        /// </summary>
        public int RouteCode
        {
            get { return routeCode; }
            set { routeCode = value; }
        }
    }
}
