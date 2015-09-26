﻿using Globe.QcApp.Common.Core;
using Globe.QcApp.Common.Helpers;
using Globe.QcApp.Common.Helpers.Themes;
using Globe.QcApp.Common.Helpers.Windows;
using Globe.QcApp.Common.VO;
using SuperMap.Data;
using SuperMap.Realspace;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        //工作空间
        public static Workspace m_workspace;

        //data path
        private string globeDataPathKey = "GlobeDataPath";

        //scene name
        private string sceneNameKey = "SceneName";

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
            //默认加载的系统主题
            ApplicationThemeManager.GetInstance().EnsureResourcesForTheme(ApplicationThemeManager.DefaultThemeNameTouch);

            //初始化三维球体
            InitGlobe();
      
            //加载三维场景
            string dataPath = ConfigurationManager.AppSettings.GetValues(globeDataPathKey)[0];
            string sceneName = ConfigurationManager.AppSettings.GetValues(sceneNameKey)[0];
            OpenScene(dataPath, sceneName);

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
            SmObjectLocator.getInstance().GlobeObject.Scene.LatLonGrid.IsVisible = false;
            this.hostSceneControl.Child = SmObjectLocator.getInstance().GlobeObject;
            m_workspace = new Workspace();
        }

        /// <summary>
        /// 打开三维场景
        /// </summary>
        /// <param name="path">工作空间路径</param>
        /// <param name="sceneName">三维场景名称</param>
        private void OpenScene(string path, string sceneName)
        {
            try
            {
                WorkspaceConnectionInfo conInfo = new WorkspaceConnectionInfo(path);
				conInfo.Type = WorkspaceType.SXWU;
                bool isOpened = m_workspace.Open(conInfo);
				if (isOpened)
				{
					SmObjectLocator.getInstance().GlobeObject.Scene.Workspace = m_workspace;

					SmObjectLocator.getInstance().GlobeObject.Scene.Open(sceneName);

					for (int i = 0; i < SmObjectLocator.getInstance().GlobeObject.Scene.Layers.Count; i++)
					{
						Layer3D layer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[i];
						layer.IsSelectable = false;
						LayerVO layerVo = new LayerVO();
						layerVo.LayerBounds = layer.Bounds;
						layerVo.LayerCenter = layer.Bounds.Center;
						layerVo.LayerName = layer.Name.Substring(0, layer.Name.IndexOf("@"));
						layerVo.LayerType = layer.Type.ToString();
						layerVo.LayerId = i.ToString();
						layerVo.LayerVisible = layer.IsVisible;
						layerVo.IsQueryLayer = true;
						layerVo.LayerCaption = layer.Caption;
						SysModelLocator.getInstance().LayerList.Add(layerVo);
					}

					//范围全幅显示
					//Layer3D olympicGreenLayer = SmObjectLocator.getInstance().GlobeObject.Scene.Layers[0];
					//SmObjectLocator.getInstance().GlobeObject.Scene.EnsureVisible(olympicGreenLayer.Bounds, 10);
				}
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
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
