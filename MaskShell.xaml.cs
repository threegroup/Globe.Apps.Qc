using Globe.QcApp.Common.Core;
using Globe.QcApp.Common.Helpers.Themes;
using Globe.QcApp.Common.Helpers.Windows;
using Globe.QcApp.Common.VO;
using Globe.QcApp.SubWindows;
using SuperMap.Data;
using SuperMap.Realspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using System.Drawing;
using System.Windows.Media;
using Globe.QcApp.Common.Utils;
using SuperMap.UI;
using System.Collections;


namespace Globe.QcApp
{
	/// <summary>
	/// MaskWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MaskShell : Window
	{
		/// <summary>
		/// 飞行路径路径Key
		/// </summary>
		private string RoutesPath = "RoutesPathKey";
		/// <summary>
		/// 查询字段名称Key
		/// </summary>
		private string queryNameField = "QueryNameFieldKey";
		/// <summary>
		/// 查询字段编码Key
		/// </summary>
		private string queryCodeField = "QueryCodeFieldKey";
		/// <summary>
		/// 数据源Key
		/// </summary>
		private string queryDataSource = "DataSourceName";
		/// <summary>
		/// 系统标题Key
		/// </summary>
		private string systemTitle = "SystemTitle";
		/// <summary>
		/// 要素图片目录Key
		/// </summary>
		private string FeatureImageKey = "FeatureImagesPathKey";
		/// <summary>
		/// 要素视频目录Key
		/// </summary>
		private string FeatureVideoKey = "FeatureVideosPathKey";
		/// <summary>
		/// 三维场景要素默认高度
		/// </summary>
		private double defaultHeight = 50;
		/// <summary>
		/// 三维场景要素默认比例
		/// </summary>
		private double defaultScale = 1;

		#region 图标高亮相关变量
		private GeoStyle3D normalStyle = null;
		private GeoStyle3D highLightStyle = null;
		private GeoPoint3D lastGPot = null;
		private string lastTag = "";
		private int lastIndex = -1;
		#endregion

		/// <summary>
		/// 三维量算工具
		/// </summary>
		private MeasureUtil measureUtil = new MeasureUtil();

		/// <summary>
        /// 空间查询通用
        /// </summary>
        private SpatialQueryUtil spatialQueryUtil = new SpatialQueryUtil();
		/// <summary>
		/// 追踪图层中属性或空间查询图标 tag 后缀
		/// </summary>
		private const string QUERY = "Query";

		/// <summary>
		/// 属性查询
		/// </summary>
		private const string ATTRIBUTE_QUERY = "Attribute";

		/// <summary>
		/// 空间查询
		/// </summary>
		private const string SPATIAL_QUERY = "Spatial";

		public MaskShell()
		{
			InitializeComponent();

			//Mask窗口Loaded事件
			this.Loaded += MaskShell_Loaded;
		}

		//Mask窗口的Loaded事件
		private void MaskShell_Loaded(object sender, RoutedEventArgs e)
		{
			//系统名称
			this.SystemTitleId.Text = ConfigurationManager.AppSettings.Get(systemTitle);

			//加载飞行路径
			this.LoadRoutes(System.Environment.CurrentDirectory);

			//加载图层
			this.LayerListBox.ItemsSource = SysModelLocator.getInstance().LayerList;

			//初始化查询图层
			InitQueryLayerList();

			//控制经纬只能输入数字
			this.ControlTextBoxContent();

			SmObjectLocator.getInstance().GlobeObject.MouseDown += GlobeObject_MouseDown;

			this.InitGeoPoint3DParams();
		}

		#region Mask窗口相关的函数
		/// <summary>
		/// 初始化要素图标样式
		/// </summary>
		private void InitGeoPoint3DParams(bool isReset = false)
		{
			if (isReset)
			{
				this.lastGPot = null;
				this.lastTag = "";
				this.lastIndex = -1;
			}
			else
			{
				GeoStyle3D sty3D = new GeoStyle3D();
				sty3D.MarkerFile = "Images/point.png";
				sty3D.MarkerScale = defaultScale;
				sty3D.MarkerSize = 32;
				sty3D.AltitudeMode = AltitudeMode.ClampToGround;
				normalStyle = sty3D;

				GeoStyle3D sty3DHigh = new GeoStyle3D();
				sty3DHigh.MarkerFile = "Images/bluepot.png";
				sty3DHigh.MarkerScale = defaultScale * 1.5;
				sty3DHigh.MarkerSize = 32;
				sty3DHigh.AltitudeMode = AltitudeMode.ClampToGround;
				highLightStyle = sty3DHigh;
			}
		}

		/// <summary>
		/// 控制经纬度输入框只能输入数字
		/// </summary>
		private void ControlTextBoxContent()
		{
			this.latitudeTxt.PreviewTextInput += latitudeTxt_PreviewTextInput;
			this.longitudeTxt.PreviewTextInput += longitudeTxt_PreviewTextInput;
			this.altitudeTxt.PreviewTextInput += altitudeTxt_PreviewTextInput;
		}

		private void altitudeTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex re = new Regex("[^0-9.-]+");
			e.Handled = re.IsMatch(e.Text);
		}

		private void longitudeTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex re = new Regex("[^0-9.-]+");
			e.Handled = re.IsMatch(e.Text);
		}

		private void latitudeTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex re = new Regex("[^0-9.-]+");
			e.Handled = re.IsMatch(e.Text);
		}

		//坐标定位
		private void LocaltionBt_Click(object sender, RoutedEventArgs e)
		{
			if (this.latitudeTxt.Text.ToString().Trim() != "" &&
				this.longitudeTxt.Text.ToString().Trim() != "" &&
				this.altitudeTxt.Text.ToString().Trim() != "")
			{
				double lat = Convert.ToDouble(this.latitudeTxt.Text.ToString().Trim());
				double lon = Convert.ToDouble(this.longitudeTxt.Text.ToString().Trim());
				double height = Convert.ToDouble(this.altitudeTxt.Text.ToString().Trim());
				if (!Double.IsNaN(lat) && !Double.IsNaN(lon) && !Double.IsNaN(height))
				{
					this.JumpCamera(lat, lon, height);
				}
				this.locatorInfo.Text = "";
			}
			else
			{
				this.locatorInfo.Text = "坐标为空或输入问题，请重新输入！";
			}
		}

		/// <summary>
		/// 快速定位窗口
		/// </summary>
		private void JumpCamera(double lat, double lon, double aititude = 400)
		{
			Camera camera = new Camera();
			camera.Altitude = aititude;
			camera.Longitude = lon;
			camera.Latitude = lat;
			camera.Heading = 0;
			camera.Tilt = 45;

			SmObjectLocator.getInstance().GlobeObject.Scene.Fly(camera, 10);
		}

		/// <summary>
		/// 获取系统运行根路径并加载路径文件
		/// </summary>
		/// <param name="sysDirectory"></param>
		private void LoadRoutes(string sysDirectory)
		{
			string routesPath = ConfigurationManager.AppSettings.GetValues(RoutesPath)[0];
			string temp = System.IO.Path.Combine(sysDirectory, routesPath);
			//判断目录是否存在
			if (Directory.Exists(temp))
			{
				string[] routes = Directory.GetFiles(temp);
				if (routes != null && routes.Length > 0)
				{
					ObservableCollection<RouteVO> RouteList = new ObservableCollection<RouteVO>();
					for (int i = 0; i < routes.Length; i++)
					{
						RouteVO route = new RouteVO();
						route.RouteCode = i;
						route.RoutePath = routes[i];
						route.RouteName = System.IO.Path.GetFileNameWithoutExtension(routes[i]);
						RouteList.Add(route);
					}

					this.RouteListBox.ItemsSource = RouteList;
				}
			}
		}

		/// <summary>
		/// 加载要素多媒体信息
		/// </summary>
		/// <param name="featureName">要素名称</param>
		/// <param name="type">多媒体类型</param>
		private void LoadFeatureMedias(string featureName, string type = "IMAGE")
		{
			//获取系统运行目录
			string sysDirectory = System.Environment.CurrentDirectory;
			//图片目录
			string imagesPath = ConfigurationManager.AppSettings.GetValues(FeatureImageKey)[0];
			string tempImagePath = System.IO.Path.Combine(sysDirectory, imagesPath);
			//视频目录
			string videosPath = ConfigurationManager.AppSettings.GetValues(FeatureVideoKey)[0];
			string tempVideoPath = System.IO.Path.Combine(sysDirectory, videosPath);

			switch (type)
			{
				case "IMAGE":
					{
						//判断目录是否存在
						if (Directory.Exists(tempImagePath))
						{
							string[] images = Directory.GetFiles(tempImagePath);
							if (images != null && images.Length > 0)
							{
								ObservableCollection<MediaVO> ImageList = new ObservableCollection<MediaVO>();
								for (int i = 0; i < images.Length; i++)
								{
									MediaVO imageVo = new MediaVO();
									imageVo.Id = i.ToString();
									imageVo.Name = System.IO.Path.GetFileNameWithoutExtension(images[i]);
									imageVo.ImageUrl = images[i];
									ImageList.Add(imageVo);
								}

								this.ImageTileViewID.ItemsSource = ImageList;
							}
						}
						break;
					}
				case "VIDEO":
					{
						//判断目录是否存在
						if (Directory.Exists(tempVideoPath))
						{
							string[] videos = Directory.GetFiles(tempVideoPath);
							if (videos != null && videos.Length > 0)
							{
								ObservableCollection<MediaVO> VideoList = new ObservableCollection<MediaVO>();
								for (int i = 0; i < videos.Length; i++)
								{
									MediaVO videoVo = new MediaVO();
									videoVo.Id = i.ToString();
									videoVo.Name = System.IO.Path.GetFileNameWithoutExtension(videos[i]);
									videoVo.VideoUrl = videos[i];
									VideoList.Add(videoVo);
								}

								this.VideoTileViewID.ItemsSource = VideoList;
							}
						}
						break;
					}
				default:
					break;
			}
		}

		private void StackPanel_Click(object sender, RoutedEventArgs e)
		{
			if (e.Source is Button)
			{
                this.spatialQueryUtil.removeListener();
				string name = (e.Source as Button).Name.ToString();
				switch (name)
				{
					case "DistanceMeasure"://距离测量
						{
							measureUtil.BeginMeasureDistance();
							break;
						}
					case "AreaMeasure"://面积测量
						{
							measureUtil.BeginMeasureArea();
							break;
						}
					case "HeightMeasure"://高度测量
						{
							measureUtil.BeginMeasureAltitude();
							break;
						}
					case "Clear"://清除
						{
							//清除量算结果
							this.measureUtil.ClearResult();
							//清除查询结果
							this.ClearQueryResult();
							break;
						}
					case "StartRoute"://开始飞行路径
						{
							if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
							{
								SmObjectLocator.getInstance().FlyManagerObject.Play();
							}
							break;
						}
					case "PauseRoute"://暂停飞行路径
						{
							if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
							{
								SmObjectLocator.getInstance().FlyManagerObject.Pause();
							}
							break;
						}
					case "StopRoute"://停止飞行路径
						{
							if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
							{
								SmObjectLocator.getInstance().FlyManagerObject.Stop();
							}
							break;
						}
					case "ZoomIn"://放大
						{
							Camera currentCamera = SmObjectLocator.getInstance().GlobeObject.Scene.Camera;
							currentCamera.Altitude *= 0.5;
							SmObjectLocator.getInstance().GlobeObject.Scene.Fly(currentCamera, 1000);
							break;
						}
					case "ZoomOut"://缩小
						{
							Camera currentCamera = SmObjectLocator.getInstance().GlobeObject.Scene.Camera;
							currentCamera.Altitude /= 0.5;
							SmObjectLocator.getInstance().GlobeObject.Scene.Fly(currentCamera, 1000);
							break;
						}
					case "Stop"://停止
						{
							SmObjectLocator.getInstance().GlobeObject.Scene.StopFly();
							break;
						}
					case "FullExent"://全幅
						{
							SmObjectLocator.getInstance().GlobeObject.Scene.ViewEntire();
							break;
						}
					case "FlyRoute"://漫游飞行
						{
							if (this.PanelRegion.Visibility == Visibility.Collapsed)
							{
								this.PanelRegion.Visibility = Visibility.Visible;
							}
							else
							{
								this.PanelRegion.Visibility = Visibility.Collapsed;

								//重置飞行路径相关信息
								this.RouteListBox.SelectedIndex = -1;
								SmObjectLocator.getInstance().FlyManagerObject.Routes.Clear();
							}
							break;
						}
					case "FullScreen"://全屏
						{
							WindowsHelper winHelper = new WindowsHelper();
							if (this.WindowState == System.Windows.WindowState.Normal)
							{
								winHelper.adjustWindow(Application.Current.MainWindow, this, true);
							}
							else
							{
								winHelper.adjustWindow(Application.Current.MainWindow, this, false);
							}
							break;
						}
					case "ExitSystem"://退出系统
						{
							ConfirmWindow confirmWin = new ConfirmWindow();
							confirmWin.Owner = Application.Current.MainWindow;
							confirmWin.ShowDialog();
							break;
						}
					default:
						break;
				}
			}
		}

		/// <summary>
		/// 显示浮动面板
		/// </summary>
		private void ShowFloatPanel()
		{
			//执行动画
			DoubleAnimation animation0 = new DoubleAnimation();
			animation0.From = 32;
			animation0.To = 400;
			animation0.Duration = TimeSpan.FromSeconds(0.1);

			this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);

			//显示面板背景
			this.PanelRegion.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#7F000000"));
		}

		/// <summary>
		/// 隐藏浮动面板
		/// </summary>
		private void HideFloatPanel()
		{
			//执行动画
			DoubleAnimation animation0 = new DoubleAnimation();
			animation0.From = 400;
			animation0.To = 32;
			animation0.Duration = TimeSpan.FromSeconds(0.1);

			this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);

			//面板背景透明
			this.PanelRegion.Background = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#00000000"));
		}

		/// <summary>
		/// 显示或隐藏浮动面板
		/// </summary>
		private void ControlPanelButton_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button)
			{
				Button controlBtn = sender as Button;
				if (controlBtn.Content.ToString() == "HIDE")
				{
					controlBtn.SetResourceReference(Button.StyleProperty, "LeftButtonStyle");
					controlBtn.Content = "SHOW";
					this.HideFloatPanel();
				}
				else
				{
					controlBtn.SetResourceReference(Button.StyleProperty, "RightButtonStyle");
					controlBtn.Content = "HIDE";
					this.ShowFloatPanel();
				}
			}

			//重置飞行路径相关信息
			//if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
			//{
			//	SmObjectLocator.getInstance().FlyManagerObject.Stop();
			//	SmObjectLocator.getInstance().FlyManagerObject.Routes.Clear();
			//}
			//this.RouteListBox.SelectedIndex = -1;
		}

		/// <summary>
		/// ListBox选中事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RouteListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			RouteVO routeVo = this.RouteListBox.SelectedItem as RouteVO;
			if (routeVo != null)
			{
				//清空已有路径
				SmObjectLocator.getInstance().FlyManagerObject.Routes.Clear();
				//加载飞行路径
				SmObjectLocator.getInstance().FlyManagerObject.Routes.FromFile(routeVo.RoutePath);

				if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
				{
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsStopsVisible = false;
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsLinesVisible = false;
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsFlyAlongTheRoute = true;
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsHeadingFixed = true;
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsAltitudeFixed = true;
					SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute.IsTiltFixed = true;
					SmObjectLocator.getInstance().FlyManagerObject.Play();
				}
			}
		}

		/// <summary>
		/// 图层显示控制
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox)
			{
				CheckBox cBox = sender as CheckBox;
				int count = SmObjectLocator.getInstance().GlobeObject.Scene.Layers.Count;
				if (count > 0)
				{
					Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[cBox.Tag.ToString()];
					if (layer != null)
					{
						layer.IsVisible = (bool)cBox.IsChecked;
					}
				}
			}
		}
		/// <summary>
		/// 图层隐藏控制
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox)
			{
				CheckBox cBox = sender as CheckBox;
				int count = SmObjectLocator.getInstance().GlobeObject.Scene.Layers.Count;
				if (count > 0)
				{
					Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[cBox.Tag.ToString()];
					if (layer != null)
					{
						layer.IsVisible = (bool)cBox.IsChecked;
					}
				}
			}
		}

		/// <summary>
		/// 选中并定位图层
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			LayerVO layerItem = this.LayerListBox.SelectedItem as LayerVO;
			if (layerItem != null)
			{
				int count = SmObjectLocator.getInstance().GlobeObject.Scene.Layers.Count;
				if (count > 0)
				{
					Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[layerItem.LayerOrigin];
					if (layer != null)
					{
						SmObjectLocator.getInstance().GlobeObject.Scene.EnsureVisible(layer.Bounds, 10);
					}
				}
			}
		}
		#endregion

		/// <summary>
		/// 切换查询图层时清除已有结果
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryLayerList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			this.ClearQueryResult();
		}

		/// <summary>
		/// 查询事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryBt_Click(object sender, RoutedEventArgs e)
		{
			ExecuteQuery(ATTRIBUTE_QUERY);
		}

		/// <summary>
		/// 执行查询
		/// </summary>
		/// <param name="type"></param>
		/// <param name="TempPoints3Ds"></param>
		/// <param name="actionStr"></param>
		public void ExecuteQuery(string type = "attribute", Point3Ds TempPoints3Ds = null, string actionStr = null)
		{
			InitGeoPoint3DParams(true);
			ExecuteQueryListBox(type, TempPoints3Ds, actionStr);
			InitQueryListOnMap();

			this.ResetQueryState();
		}

		/// <summary>
		/// 重置查询面板状态
		/// </summary>
		private void ResetQueryState()
		{
			this.DetailPanel.Visibility = System.Windows.Visibility.Collapsed;
			this.DetailRadGridView.ItemsSource = null;
			this.ShowLegendTitle.Text = "";
		}

		/// <summary>
		/// 清除查询结果
		/// </summary>
		private void ClearQueryResult()
		{
			this.DetailPanel.Visibility = System.Windows.Visibility.Collapsed;
			this.DetailRadGridView.ItemsSource = null;
			this.ShowLegendTitle.Text = "";
			this.QueryNameTxt.Text = "";

			InitGeoPoint3DParams(true);

			//清空追踪图层数据
			SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Clear();
			this.QueryListBox.ItemsSource = null;
			this.QueryInfo.Text = "";
		}

		/// <summary>
		/// 在地图上初始化显示查询结果列表
		/// </summary>
		private void InitQueryListOnMap()
		{
			//清空追踪图层数据
			this.measureUtil.ClearAllResult(QUERY);

			ObservableCollection<QueryRecordVO> recordList = SysModelLocator.getInstance().recordList;
			if (recordList.Count > 0)
			{
				double centerX = 0.0;
				double centerY = 0.0;
				double heightZ = defaultHeight;

				double minX = 0;
				double minY = 0;
				double maxX = 0;
				double maxY = 0;
				for (int i = 0; i < recordList.Count; i++)
				{
					QueryRecordVO vo = vo = recordList[i];
					if (Double.TryParse(vo.RecordCenterX, out centerX) == true && Double.TryParse(vo.RecordCenterY, out centerY) == true)
					{
						GeoPoint3D gpt = new GeoPoint3D(centerX, centerY, heightZ);
						gpt.Style3D = normalStyle;
						SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Add(gpt, string.Format("{0}#{1}#{2}", vo.RecordName, vo.RecordIndex, QUERY));

						if (minX == 0)
						{
							minX = centerX;
							minY = centerY;
							maxX = centerX;
							maxY = centerY;
						}
						else
						{
							minX = Math.Min(minX, centerX);
							minY = Math.Min(minY, centerY);
							maxX = Math.Max(maxX, centerX);
							maxY = Math.Max(maxY, centerY);
						}
					}
				}
				Rectangle2D rect2D = new Rectangle2D(minX - 0.05, minY - 0.05, maxX + 0.05, maxY + 0.05);
				SmObjectLocator.getInstance().GlobeObject.Scene.EnsureVisible(rect2D, 500);
			}
		}

		/// <summary>
		/// mark的鼠标点击事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void GlobeObject_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				int fid = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.HitTest(e.Location);
				if (fid >= 0)
				{
					GeoPoint3D p = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Get(fid) as GeoPoint3D;

					this.HighLightFeature(p, fid);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// 高亮显示要素并显示要素详情
		/// </summary>
		/// <param name="p"></param>
		/// <param name="fid"></param>
		private void HighLightFeature(GeoPoint3D p, int fid)
		{
			if (p != null)
			{
				string tag = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.GetTag(fid);
				if (this.lastGPot == null)
				{
					this.lastGPot = p;
					this.lastTag = tag;
					this.lastIndex = fid;
				}
				else if (p != this.lastGPot)
				{
					this.lastGPot.Style3D = normalStyle;
					SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Set(this.lastIndex, this.lastGPot);
					SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.SetTag(this.lastIndex, this.lastTag);
				}

				p.Style3D = highLightStyle;
				SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Set(fid, p);
				SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.SetTag(fid, tag);

				this.lastGPot = p;
				this.lastTag = tag;
				this.lastIndex = fid;

				//显示要素详情
				ShowMarkDetailInfo(tag);

				//显示要素多媒体信息
				LoadFeatureMedias(tag, "IMAGE");
				LoadFeatureMedias(tag, "VIDEO");
			}
		}

		/// <summary>
		/// 显示mark的详细信息属性窗
		/// </summary>
		private void ShowMarkDetailInfo(string tag)
		{
			if (this.QueryLayerList.Items.Count > 0)
			{
				string layerName = this.QueryLayerList.SelectedValue.ToString();
				Workspace ws = MainWindow.m_workspace;
				if (ws != null)
				{
					string sourceName = ConfigurationManager.AppSettings.Get(queryDataSource);
					Datasource dSource = ws.Datasources[sourceName];
					if (dSource != null)
					{
						DatasetVector dSetV = (DatasetVector)dSource.Datasets[layerName];
						if (dSetV != null)
						{
							string fieldName = ConfigurationManager.AppSettings.Get(queryNameField);
							string fieldCode = ConfigurationManager.AppSettings.Get(queryCodeField);
							QueryParameter queryParameter = new QueryParameter();
							queryParameter.CursorType = SuperMap.Data.CursorType.Static;
							queryParameter.HasGeometry = true;
							string[] tempArr = tag.Split('#');
							queryParameter.AttributeFilter = fieldName + " = '" + tempArr[0] + "' and " + fieldCode + " = '" + tempArr[1] + "'";

							Recordset recordset = dSetV.Query(queryParameter);
							if (recordset != null && recordset.RecordCount > 0)
							{
								bool isExist = false;
								FieldInfos fis = recordset.GetFieldInfos();
								for (int i = 0; i < fis.Count; i++)
								{
									FieldInfo fi = fis[i];
									if (fi != null)
									{
										if (fi.Name.ToString().ToUpper() == fieldName)
										{
											isExist = true;
											break;
										}
										continue;
									}
								}

								if (isExist)
								{
									ObservableCollection<DetailVO> detailList = new ObservableCollection<DetailVO>();
									for (recordset.MoveFirst(); recordset.IsEOF == false; recordset.MoveNext())
									{
										FieldInfos fs = recordset.GetFieldInfos();
										for (int j = 0; j < fs.Count; j++)
										{
											FieldInfo fi = fs[j];
											if (fi != null && fi.Name.ToLower().IndexOf("sm") == -1)
											{
												DetailVO dv = new DetailVO();
												if (fi.Name != null)
												{
													dv.FeatureField = fi.Name;
													if (recordset.GetFieldValue(fi.Name) != null)
													{
														dv.FeatureValue = recordset.GetFieldValue(fi.Name).ToString();
														if (fi.Name.ToString().ToUpper() == fieldName)
														{
															this.ShowLegendTitle.Text = recordset.GetFieldValue(fieldName).ToString();
														}
													}
													detailList.Add(dv);
												}
											}
										}
									}
									this.DetailRadGridView.ItemsSource = detailList;
									this.DetailPanel.Visibility = System.Windows.Visibility.Visible;
								}

							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 在查询结果面板上初始化查询结果列表
		/// </summary>
		/// <param name="type">查询方式，默认为属性查询-Attribute，空间查询-Spatial</param>
		private void ExecuteQueryListBox(string type, Point3Ds TempPoints3Ds=null, string actionStr=null)
		{
			string layerName = "";
			if (this.QueryLayerList.Items.Count > 0)
			{
				layerName = this.QueryLayerList.SelectedValue.ToString();
			}
			
			if (layerName != "")//关键字为空时查询全部要素
			{
				string queryTxt = this.QueryNameTxt.Text.Trim();
				Workspace ws = MainWindow.m_workspace;
				if (ws != null)
				{
					string sourceName = ConfigurationManager.AppSettings.Get(queryDataSource);
					Datasource dSource = ws.Datasources[sourceName];
					if (dSource != null)
					{
						DatasetVector dSetV = (DatasetVector)dSource.Datasets[layerName];
						if (dSetV != null)
						{
							string fieldName = ConfigurationManager.AppSettings.Get(queryNameField);
							string fieldCode = ConfigurationManager.AppSettings.Get(queryCodeField);
							Recordset recordset = null;

							if (type != ATTRIBUTE_QUERY)
							{
								double queryBuffer = 0.0;
								SuperMap.Data.Geometry queryGeometry = null;
								
								switch (actionStr)
								{
									case "createpoint":
                                        queryBuffer = 0.01;
										if (TempPoints3Ds.Count == 1)
										{
											GeoPoint geoPoint = new GeoPoint(TempPoints3Ds[0].X, TempPoints3Ds[0].Y);
											queryGeometry = geoPoint;
										}
										break;
									case "createline":
										if (TempPoints3Ds.Count == 2)
										{
											Point2D point2D = new Point2D(TempPoints3Ds[0].X, TempPoints3Ds[0].Y);
                                            Point2D point2D1 = new Point2D(TempPoints3Ds[1].X, TempPoints3Ds[1].Y);
                                            double radius = Geometrist.Distance(new GeoPoint(point2D), new GeoPoint(point2D1));
                                            GeoCircle geoCircle = new GeoCircle(point2D, radius);
                                            GeoRegion geoRegion = geoCircle.ConvertToRegion(72);
                                            queryGeometry = geoRegion;
										}
										break;
									case "createpolygon":
										if (TempPoints3Ds.Count > 2)
										{
											Point2Ds tempPoint2Ds = new Point2Ds();
											for (int i = 0; i < TempPoints3Ds.Count; i++)
											{
												tempPoint2Ds.Add(new Point2D(TempPoints3Ds[i].X, TempPoints3Ds[i].Y));
											}
											GeoRegion geoRegion = new GeoRegion(tempPoint2Ds);
											queryGeometry = geoRegion;
										}
										break;
									default:
										break;
								}
                                if (queryGeometry != null)
                                {
                                    recordset = dSetV.Query(queryGeometry, queryBuffer, SuperMap.Data.CursorType.Static);
                                }
							}
							else
							{
								QueryParameter queryParameter = new QueryParameter();
								queryParameter.CursorType = SuperMap.Data.CursorType.Static;
								queryParameter.HasGeometry = true;
								if (this.QueryNameTxt.Text.Trim() != "")
								{
									queryParameter.AttributeFilter = fieldName + " like '%" + queryTxt + "%'";
								}

								recordset = dSetV.Query(queryParameter);
							}
							
							if (recordset != null && recordset.RecordCount > 0)
							{
								ObservableCollection<QueryRecordVO> recordList = SysModelLocator.getInstance().recordList;
								recordList.Clear();
								bool isExist = false;
								FieldInfos fis = recordset.GetFieldInfos();
								for (int j = 0; j < fis.Count; j++)
								{
									FieldInfo fi = fis[j];
									if (fi != null)
									{
										if (fi.Name.ToString().ToUpper() == fieldName)
										{
											isExist = true;
											break;
										}
										continue;
									}
								}

								if (isExist)
								{
									for (recordset.MoveFirst(); recordset.IsEOF == false; recordset.MoveNext())
									{
										QueryRecordVO qVO = new QueryRecordVO();
										qVO.RecordLayerId = layerName;
										qVO.RecordName = recordset.GetFieldValue(fieldName).ToString();
										qVO.RecordIndex = recordset.GetFieldValue(fieldCode).ToString();
										qVO.RecordCenterX = recordset.GetGeometry().InnerPoint.X.ToString();
										qVO.RecordCenterY = recordset.GetGeometry().InnerPoint.Y.ToString();
										recordList.Add(qVO);
									}
									this.QueryListBox.ItemsSource = recordList;
									this.QueryInfo.Text = "查询结果合计：" + recordList.Count + "条";
								}
								else
								{
									this.QueryListBox.ItemsSource = null;
									this.QueryInfo.Text = "";
								}
							}
							else
							{
                                SysModelLocator.getInstance().recordList.Clear();
								this.QueryListBox.ItemsSource = null;
								this.QueryInfo.Text = "查询结果合计：0条";
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 初始化查询图层的列表
		/// </summary>
		private void InitQueryLayerList()
		{
			this.QueryLayerList.ItemsSource = SysModelLocator.getInstance().LayerList.Where(p => p.IsQueryLayer == true && p.LayerCaption.IndexOf("@") == -1);
			if (this.QueryLayerList.Items.Count > 0)
			{
				this.QueryLayerList.SelectedIndex = 0;
			}

		}

		/// <summary>
		/// 选中定位要素
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueryListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (sender is RadListBox)
			{
				if (this.QueryListBox.SelectedItem != null)
				{
					try
					{
						QueryRecordVO qVO = this.QueryListBox.SelectedItem as QueryRecordVO;
						double lat = Convert.ToDouble(qVO.RecordCenterX.Trim());
						double lon = Convert.ToDouble(qVO.RecordCenterY.Trim());
						double height = Convert.ToDouble(defaultHeight.ToString().Trim());
						if (!Double.IsNaN(lat) && !Double.IsNaN(lon) && !Double.IsNaN(height))
						{
							//this.JumpCamera(lat, lon, height);
							Rectangle2D rect2D = new Rectangle2D(new Point2D(lat, lon), new Size2D(0.4, 0.4));
							SmObjectLocator.getInstance().GlobeObject.Scene.EnsureVisible(rect2D, 500);
						}

						//高亮要素，显示详情
						int fid = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.IndexOf(string.Format("{0}#{1}#{2}", qVO.RecordName, qVO.RecordIndex, QUERY));
						if (fid >= 0)
						{
							GeoPoint3D p = SmObjectLocator.getInstance().GlobeObject.Scene.TrackingLayer.Get(fid) as GeoPoint3D;
							this.HighLightFeature(p, fid);
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message + "要素坐标错误！");
					}
				}
			}
		}

		/// <summary>
		/// 切换面板内容时停止飞行浏览功能
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RadTabControl_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
		{
			RadTabItem rtItem = this.SysRadTabControl.SelectedItem as RadTabItem;
			if (rtItem.Header.ToString() != "飞行漫游")
			{
				if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
				{
					SmObjectLocator.getInstance().FlyManagerObject.Stop();
				}
			}
		}

		/// <summary>
		/// 关闭属性详情面板
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Image_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.DetailPanel.Visibility = System.Windows.Visibility.Collapsed;
			this.DetailRadGridView.ItemsSource = null;
			this.ImageTileViewID.ItemsSource = null;
			this.VideoTileViewID.ItemsSource = null;
			this.ShowLegendTitle.Text = "";
		}

        /// <summary>
        /// 多边形查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PolygonQueryBt_Click(object sender, RoutedEventArgs e)
        {
            SmObjectLocator.getInstance().GlobeObject.Cursor = System.Windows.Forms.Cursors.Cross;
            //清除量算结果
            this.measureUtil.ClearResult();
            //清除查询结果
            this.ClearQueryResult();
			this.ResetQueryState();
            SpatialQueryUtil.ms = this;
            spatialQueryUtil.BeginPolygonQuery();
        }

        /// <summary>
        /// 圆查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircleQueryBt_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Cross;
            this.Owner.Cursor = Cursors.Cross;
            //清除量算结果
            this.measureUtil.ClearResult();
            //清除查询结果
            this.ClearQueryResult();
			this.ResetQueryState();
            SpatialQueryUtil.ms = this;
            spatialQueryUtil.BeginCircleQuery();
        }

        /// <summary>
        /// 点查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PointQueryBt_Click(object sender, RoutedEventArgs e)
        {
            //清除量算结果
            this.measureUtil.ClearResult();
            //清除查询结果
            this.ClearQueryResult();
			this.ResetQueryState();
            SpatialQueryUtil.ms = this;
            spatialQueryUtil.BeginPointQuery();
        }
	}
}
