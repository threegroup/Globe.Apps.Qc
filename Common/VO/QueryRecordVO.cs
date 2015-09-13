using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe.QcApp.Common.VO
{
    public class QueryRecordVO
    {
        /// <summary>
        /// 记录图层名称
        /// </summary>
        private string recordLayerId;

        public string RecordLayerId
        {
            get { return recordLayerId; }
            set { recordLayerId = value; }
        }

        /// <summary>
        /// 记录名称
        /// </summary>
        private string recordName;

        public string RecordName
        {
            get { return recordName; }
            set { recordName = value; }
        }

        /// <summary>
        /// 记录索引
        /// </summary>
        private string recordIndex;

        public string RecordIndex
        {
            get { return recordIndex; }
            set { recordIndex = value; }
        }


        /// <summary>
        /// 记录中心点X
        /// </summary>
        private string recordCenterX;

        public string RecordCenterX
        {
            get { return recordCenterX; }
            set { recordCenterX = value; }
        }


        /// <summary>
        /// 记录中心点Y
        /// </summary>
        private string recordCenterY;

        public string RecordCenterY
        {
            get { return recordCenterY; }
            set { recordCenterY = value; }
        }


    }
}
