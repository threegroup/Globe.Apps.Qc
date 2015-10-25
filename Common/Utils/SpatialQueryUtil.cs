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

        private string spatialTag = "spatial";

        private string spatialTempTag = "spatialTemp";

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
            }
        }

        private void EndSpatialQuery()
        {
            if (this.TempPoints3Ds.Count > 2)
            {
                GeoRegion3D geoRegion3D = new GeoRegion3D(TempPoints3Ds);
                geoRegion3D.Style3D = GetGeoStyle3D();
                SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoRegion3D, spatialTag);
            }
            Point3Ds queryPoints = TempPoints3Ds.Clone();
            TempPoints3Ds.Clear();
            removeListener();
            SpatialQueryByPoint3Ds(queryPoints, SmObjectLocator.getInstance().GlobeObject.Action);
        }

        /// <summary>
        /// 进行空间查询
        /// </summary>
        /// <param name="TempPoints3Ds"></param>
        /// <param name="action3D"></param>
        private void SpatialQueryByPoint3Ds(Point3Ds TempPoints3Ds, Action3D action3D)
        {
            string actionStr = action3D.ToString().ToLower();
            switch (actionStr)
            {
                case "createpoint":

                    break;
                case "createline":
                    break;
                case "createpolygon":
                    break;
                default:
                    break;
            }
        }


        private void TrackedHandler(object sender, Tracked3DEventArgs e)
        {
            if (this.TempPoints3Ds.Count > 2)
            {
                GeoRegion3D geoRegion3D = e.Geometry.Clone() as GeoRegion3D;
                geoRegion3D.Style3D = GetGeoStyle3D();
                SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(geoRegion3D, spatialTag);
            }
        }

        private void TrackingHandler(object sender, Tracking3DEventArgs e)
        {
            string actionStr = SmObjectLocator.getInstance().GlobeObject.Action.ToString().ToLower();
            switch (actionStr)
            {
                case "createpoint":

                    break;
                case "createline":
                    break;
                case "createpolygon":
                    if (this.TempPoints3Ds.Count == 1)
                    {
                        m_tempPoint = new Point3D(e.X, e.Y, e.Z);
                        TempPoints3Ds.Add(m_tempPoint);
                    }
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
        /// 查询的风格
        /// </summary>
        /// <returns></returns>
        private GeoStyle3D GetGeoStyle3D()
        {
            GeoStyle3D geoStyle = new GeoStyle3D();
            geoStyle.AltitudeMode = AltitudeMode.ClampToGround;
            geoStyle.LineColor = System.Drawing.Color.Yellow;
            geoStyle.LineWidth = 1;
            geoStyle.FillBackColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.FillForeColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.MarkerColor = System.Drawing.Color.FromArgb(180, 255, 255, 0);
            geoStyle.FillMode = FillMode3D.Fill;
            return geoStyle;
        }

    }
}
