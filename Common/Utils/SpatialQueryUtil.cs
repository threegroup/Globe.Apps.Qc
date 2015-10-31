using Globe.QcApp.Common.Core;
using SuperMap.Data;
using SuperMap.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Globe.QcApp.Common.Utils
{
    class SpatialQueryUtil
    {

        private Point3Ds TempPoints3Ds = new Point3Ds();

        private Point3D m_tempPoint;

        private string spatialTag = "Spatial";

        private string spatialTempTag = "SpatialTemp";

        public static MaskShell ms = null;

        /// <summary>
        /// 开始点查询
        /// </summary>
        public void BeginPointQuery()
        {
            SmObjectLocator.getInstance().GlobeObject.Action = Action3D.CreatePoint;
            addListener();
        }

        /// <summary>
        /// 开始圆查询
        /// </summary>
        public void BeginCircleQuery()
        {
            SmObjectLocator.getInstance().GlobeObject.Action = Action3D.CreateLine;
            addListener();
        }

        /// <summary>
        /// 开始多边形查询
        /// </summary>
        public void BeginPolygonQuery()
        {
            SmObjectLocator.getInstance().GlobeObject.Action = Action3D.CreatePolygon;
            addListener();
        }

        /// <summary>
        /// 结束空间查询
        /// </summary>
        private void EndSpatialQuery()
        {
            string actionStr = SmObjectLocator.getInstance().GlobeObject.Action.ToString().ToLower();
            switch (actionStr)
            {
                case "createpoint":
                    if (this.TempPoints3Ds.Count == 1)
                    {
                        GeoPoint3D geoPoint3D = new GeoPoint3D(TempPoints3Ds[0]);
                        geoPoint3D.Style3D = GetGeoStyle3D();
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoPoint3D, spatialTag);
                    }
                    break;
                    //此时划线就是为了画圆
                case "createline":
                    int index = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.IndexOf(spatialTempTag);
                    if (index >= 0)
                    {
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Remove(index);
                    }
                    if (this.TempPoints3Ds.Count == 2)
                    {
                        Point3D point3D = new Point3D(TempPoints3Ds[0].X, TempPoints3Ds[0].Y, TempPoints3Ds[0].Z);
                        double radius = GetLengthBy2Point(TempPoints3Ds[0], TempPoints3Ds[1]);
                        GeoCircle3D geoCircle3D = new GeoCircle3D(point3D, radius);
                        GeoModel geoModel = geoCircle3D.GetGeoModel(72, 72);
                        geoModel.Style3D = GetGeoStyle3D();
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoModel, spatialTag);
                    }
                    break;
                case "createpolygon":
                    if (this.TempPoints3Ds.Count > 2)
                    {
                        GeoRegion3D geoRegion3D = new GeoRegion3D(TempPoints3Ds);
                        geoRegion3D.Style3D = GetGeoStyle3D();
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoRegion3D, spatialTag);
                    }
                    else
                    {
                        int index1 = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.IndexOf(spatialTempTag);
                        if (index1 >= 0)
                        {
                            SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Remove(index1);
                        }
                    }
                    break;
                default:
                    break;
            }
            Point3Ds queryPoints = TempPoints3Ds.Clone();
            TempPoints3Ds.Clear();
            removeListener();
            if (ms != null)
            {
				ms.ExecuteQuery(spatialTag, queryPoints, actionStr);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GlobeObject_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                EndSpatialQuery();
            }
            if (e.Clicks == 1 && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Point3D point3d = SmObjectLocator.getInstance().GlobeObject.Scene.PixelToGlobe(e.Location);
                TempPoints3Ds.Add(point3d);
                string actionStr = SmObjectLocator.getInstance().GlobeObject.Action.ToString().ToLower();
                switch (actionStr)
                {
                    case "createpoint":
                        EndSpatialQuery();
                        break;
                    default:
                        break;
                }
            }
        }
       
        private void TrackedHandler(object sender, Tracked3DEventArgs e)
        {
            string actionStr = SmObjectLocator.getInstance().GlobeObject.Action.ToString().ToLower();
            switch (actionStr)
            {
                case "createline":
                    if (TempPoints3Ds.Count == 2)
                    {
                        EndSpatialQuery();
                    }
                    break;
                default:
                    break;
            }
        }

        private void TrackingHandler(object sender, Tracking3DEventArgs e)
        {
            string actionStr = SmObjectLocator.getInstance().GlobeObject.Action.ToString().ToLower();
            if (this.TempPoints3Ds.Count == 1)
            {
                m_tempPoint = new Point3D(e.X, e.Y, e.Z);
                TempPoints3Ds.Add(m_tempPoint);
            }
            switch (actionStr)
            {
                case "createline":
                    if (this.TempPoints3Ds.Count == 2)
                    {
                        int index = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.IndexOf(spatialTempTag);
                        if (index >= 0)
                        {
                            SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Remove(index);
                        }
                        GeoLine3D geoLine3D = new GeoLine3D(TempPoints3Ds);
                        geoLine3D.Style3D = GetGeoStyle3D();
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoLine3D, spatialTempTag);
                        TempPoints3Ds.Remove(1);
                    }
                    break;
                case "createpolygon":
                    if (m_tempPoint != Point3D.Empty)
                    {
                        TempPoints3Ds.Remove(TempPoints3Ds.Count - 1);
                    }
                    m_tempPoint = new Point3D(e.X, e.Y, e.Z);
                    TempPoints3Ds.Add(m_tempPoint);

                    if (this.TempPoints3Ds.Count > 1)
                    {
                        int index = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.IndexOf(spatialTempTag);
                        if (index >= 0)
                        {
                            SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Remove(index);
                        }
                        TempPoints3Ds.Add(TempPoints3Ds[0]);
                        GeoLine3D geoLine3D = new GeoLine3D(TempPoints3Ds);
                        geoLine3D.Style3D = GetGeoStyle3D();
                        SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoLine3D, spatialTempTag);
                        TempPoints3Ds.Remove(TempPoints3Ds.Count - 1);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        public void addListener()
        {
            SmObjectLocator.getInstance().GlobeObject.Tracking -= new Tracking3DEventHandler(TrackingHandler);
            SmObjectLocator.getInstance().GlobeObject.Tracked -= new Tracked3DEventHandler(TrackedHandler);
            SmObjectLocator.getInstance().GlobeObject.MouseClick -= GlobeObject_MouseClick;

            SmObjectLocator.getInstance().GlobeObject.Tracking += new Tracking3DEventHandler(TrackingHandler);
            SmObjectLocator.getInstance().GlobeObject.Tracked += new Tracked3DEventHandler(TrackedHandler);
            SmObjectLocator.getInstance().GlobeObject.MouseClick += GlobeObject_MouseClick;
        }


        /// <summary>
        /// 移除事件
        /// </summary>
        public void removeListener()
        {
            SmObjectLocator.getInstance().GlobeObject.Tracking -= new Tracking3DEventHandler(TrackingHandler);
            SmObjectLocator.getInstance().GlobeObject.Tracked -= new Tracked3DEventHandler(TrackedHandler);
            SmObjectLocator.getInstance().GlobeObject.MouseClick -= GlobeObject_MouseClick;
            SmObjectLocator.getInstance().GlobeObject.Action = Action3D.Pan2;
        }

        /// <summary>
        /// 获取两点之间的距离
        /// </summary>
        /// <param name="point3D1"></param>
        /// <param name="point3D2"></param>
        /// <returns></returns>
        public double GetLengthBy2Point(Point3D point3D1, Point3D point3D2)
        {
            double tempR = 0.0;
            GeoLine3D tempL = new GeoLine3D();
            Point3Ds temp3Ds = new Point3Ds();
            temp3Ds.Add(point3D1);
            temp3Ds.Add(point3D2);
            tempL.AddPart(temp3Ds);
            tempR = Geometrist.ComputeLength(tempL, new PrjCoordSys(PrjCoordSysType.EarthLongitudeLatitude));
            return tempR == 0.0 ? 10000.0 : tempR;
        }


        /// <summary>
        ///默认的空间查询图形风格
        /// </summary>
        /// <returns></returns>
        private GeoStyle3D GetGeoStyle3D()
        {
            GeoStyle3D geoStyle = new GeoStyle3D();
            geoStyle.AltitudeMode = AltitudeMode.ClampToGround;
            geoStyle.BottomAltitude = 20;
            //geoStyle.ExtendedHeight = 20;
            geoStyle.LineColor = System.Drawing.Color.Yellow;
            geoStyle.LineWidth = 1;
            geoStyle.FillBackColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.FillForeColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.MarkerColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.FillMode = FillMode3D.Fill;
            geoStyle.FillGradientMode = FillGradientMode.Linear;
            geoStyle.MarkerSize = 15;
            return geoStyle;
        }

        ///// <summary>
        ///// 半透明没有实现
        ///// </summary>
        ///// <returns></returns>
        //private GeoStyle GetGeoStyle2D()
        //{
        //    GeoStyle geoStyle = new GeoStyle();
        //    //geoStyle.LineColor = System.Drawing.Color.Yellow;
        //    //geoStyle.LineWidth = 1;
        //    geoStyle.FillBackColor = System.Drawing.Color.FromArgb(20, 255, 255, 0);
        //    geoStyle.FillForeColor = System.Drawing.Color.FromArgb(20, 255, 255, 0);
        //    //geoStyle.FillOpaqueRate = 0;
        //    //geoStyle.FillBackOpaque = true;
        //    //geoStyle.FillGradientMode = FillGradientMode.Linear;
        //    return geoStyle;
        //}

    }
}
