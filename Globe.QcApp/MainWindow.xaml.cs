using Globe.QcApp.Common.Core;
using Globe.QcApp.Common.Helpers;
using Globe.QcApp.Common.Helpers.Themes;
using Globe.QcApp.Common.Helpers.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Globe.QcApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Mask窗口
        /// </summary>
        private MaskShell maskShell = null;

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
            this.Deactivated += MainWindow_Deactivated;
            this.Activated += MainWindow_Activated;
            this.StateChanged += MainWindow_StateChanged;

        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //初始化三维球体
            InitGlobe();

            //默认加载的系统主题
            ApplicationThemeManager.GetInstance().EnsureResourcesForTheme(ApplicationThemeManager.DefaultThemeNameTouch);

            InitMaskShell();

            WindowsHelper winHelper = new WindowsHelper();
            winHelper.adjustWindow(this, this.maskShell, false);
        }

        /// <summary>
        /// 实现mask面板状态与主面板状态同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.maskShell != null)
            {
                if (this.WindowState == System.Windows.WindowState.Maximized ||
                    this.WindowState == System.Windows.WindowState.Normal)
                {
                    this.maskShell.Topmost = true;
                    this.maskShell.WindowState = System.Windows.WindowState.Maximized;
                }
                else if (this.WindowState == System.Windows.WindowState.Minimized)
                {
                    this.maskShell.Topmost = false;
                    this.maskShell.WindowState = System.Windows.WindowState.Minimized;
                }
            }
        }

        void MainWindow_Activated(object sender, EventArgs e)
        {
            if (this.maskShell != null)
            {
                this.maskShell.Topmost = true;
            }
        }

        void MainWindow_Deactivated(object sender, EventArgs e)
        {
            if (this.maskShell != null)
            {
                this.maskShell.Topmost = false;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(-1);
        }

        /// <summary>
        /// 初始化三维球体
        /// </summary>
        private void InitGlobe()
        {
            SuperMapObjectLocator.getInstance().GlobeObject.Scene.LatLonGrid.IsVisible = false;
            this.hostSceneControl.Child = SuperMapObjectLocator.getInstance().GlobeObject;
        }

        /// <summary>
        /// 初始化遮罩
        /// </summary>
        private void InitMaskShell()
        {
            if (maskShell == null)
            {
                maskShell = new MaskShell();
                maskShell.Owner = this;
                maskShell.WindowState = WindowState.Maximized;
            }

            maskShell.Show();
        }
    }
}
