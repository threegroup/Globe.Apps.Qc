using Globe.QcApp.Common.Core;
using Globe.QcApp.Common.Helpers.Themes;
using Globe.QcApp.Common.Helpers.Windows;
using Globe.QcApp.SubWindows;
using SuperMap.Realspace;
using System;
using System.Collections.Generic;
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
            this.LoadRoutes(Environment.CurrentDirectory);
        }

        #region Mask窗口相关的函数
        /// <summary>
        /// 获取系统运行根路径
        /// </summary>
        /// <param name="sysDirectory"></param>
        private void LoadRoutes(string sysDirectory)
        {
            //判断目录是否存在
            if (Directory.Exists(sysDirectory))
            {
                string[] routes = Directory.GetFiles(sysDirectory);
                if (routes != null && routes.Length > 0)
                {
                    
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
                                string controlVal = this.ControlPanelButton.Content.ToString();
                                if (controlVal == "+")
                                {
                                    this.ControlPanelButton.Content = "-";
                                    this.ShowLegendPanel();
                                }
                            }
                            else
                            {
                                this.PanelRegion.Visibility = Visibility.Collapsed;
                            }
                            break;
                        }
                    case "SearchLocation"://查询定位
                        {
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
        /// 显示图例面板
        /// </summary>
        private void ShowLegendPanel()
        {
            //执行动画
            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = 30;
            animation0.To = 300;
            animation0.Duration = TimeSpan.FromSeconds(0.5);

            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.From = 30;
            animation1.To = 400;
            animation1.Duration = TimeSpan.FromSeconds(0.5);

            this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);
            this.PanelRegion.BeginAnimation(Border.HeightProperty, animation1);
        }

        /// <summary>
        /// 隐藏图例面板
        /// </summary>
        private void HideLegendPanel()
        {
            //执行动画
            DoubleAnimation animation0 = new DoubleAnimation();
            animation0.From = 300;
            animation0.To = 30;
            animation0.Duration = TimeSpan.FromSeconds(0.5);

            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.From = 400;
            animation1.To = 30;
            animation1.Duration = TimeSpan.FromSeconds(0.5);

            this.PanelRegion.BeginAnimation(Border.WidthProperty, animation0);
            this.PanelRegion.BeginAnimation(Border.HeightProperty, animation1);
        }

        /// <summary>
        /// 显示或隐藏图例
        /// </summary>
        private void ControlPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button controlBtn = sender as Button;
                if (controlBtn != null)
                {
                    string controlVal = controlBtn.Content.ToString();
                    if (controlVal == "+")
                    {
                        controlBtn.Content = "-";
                        this.ShowLegendPanel();
                    }
                    else
                    {
                        controlBtn.Content = "+";
                        this.HideLegendPanel();
                    }
                }
            }
        }
        #endregion
    }
}
