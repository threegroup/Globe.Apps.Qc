﻿using Globe.QcApp.Common.Core;
using Globe.QcApp.Common.Helpers.Themes;
using Globe.QcApp.Common.Helpers.Windows;
using Globe.QcApp.Common.VO;
using Globe.QcApp.SubWindows;
using SuperMap.Realspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Globe.QcApp
{
    /// <summary>
    /// MaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaskShell : Window
    {
        public MaskShell()
        {
            InitializeComponent();


            //Mask窗口Loaded事件
            this.Loaded += MaskShell_Loaded;
        }

        //Mask窗口的Loaded事件
        private void MaskShell_Loaded(object sender, RoutedEventArgs e)
        {
            //场景初始化默认位置
            this.ResetCamera();

            //加载飞行路径
            this.LoadRoutes(Environment.CurrentDirectory);

            //加载图层
            this.LayerListBox.ItemsSource = SysModelLocator.getInstance().LayerList;
        }

        #region Mask窗口相关的函数
        /// <summary>
        /// 快速重置窗口位置为默认位置
        /// </summary>
        private void ResetCamera()
        {
            Camera camera = new Camera();
            camera.Altitude = 285.05341279041;
            camera.Longitude = 116.391305696988;
            camera.Latitude = 39.9933447121584;
            camera.Heading = 2.76012171129487;
            camera.Tilt = 75.2282529563474;

            SmObjectLocator.getInstance().GlobeObject.Scene.Fly(camera, 0);
        }

        /// <summary>
        /// 获取系统运行根路径并加载路径文件
        /// </summary>
        /// <param name="sysDirectory"></param>
        private void LoadRoutes(string sysDirectory)
        {
            string temp = sysDirectory + "\\Routes";
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

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                string name = (e.Source as Button).Name.ToString();
                switch (name)
                {
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
        /// 显示路径面板
        /// </summary>
        private void ShowFloatPanel()
        {
            //执行动画
            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = 32;
            animation0.To = 480;
            animation0.Duration = TimeSpan.FromSeconds(0.5);

            this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);
        }

        /// <summary>
        /// 隐藏路径面板
        /// </summary>
        private void HideFloatPanel()
        {
            //执行动画
            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = 480;
            animation0.To = 32;
            animation0.Duration = TimeSpan.FromSeconds(0.5);

            this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);
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
            if (SmObjectLocator.getInstance().FlyManagerObject.Routes.CurrentRoute != null)
            {
                SmObjectLocator.getInstance().FlyManagerObject.Stop();
                SmObjectLocator.getInstance().FlyManagerObject.Routes.Clear();
            }
            this.RouteListBox.SelectedIndex = -1;
            //this.ResetCamera();
        }

        /// <summary>
        /// ListBox选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RouteListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                   Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[cBox.Content.ToString()];
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
                    Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[cBox.Content.ToString()];
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
        private void LayerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LayerVO layerItem = this.LayerListBox.SelectedItem as LayerVO;
            if (layerItem != null)
            {
                int count = SmObjectLocator.getInstance().GlobeObject.Scene.Layers.Count;
                if (count > 0)
                {
                    Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[layerItem.LayerName];
                    if (layer != null)
                    {
                        SmObjectLocator.getInstance().GlobeObject.Scene.EnsureVisible(layer.Bounds, 10);
                    }
                }
            }
        }
        #endregion
    }
}
