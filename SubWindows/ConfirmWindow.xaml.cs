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
using System.Windows.Shapes;
using Globe.QcApp.Common.Core;

namespace Globe.QcApp.SubWindows
{
    /// <summary>
    /// ConfirmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmWindow : Window
    {
        public ConfirmWindow()
        {
            InitializeComponent();
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                string name = (e.Source as Button).Name.ToString();
                switch (name)
                {
                    case "Ok"://确定退出
                        {
							if (SmObjectLocator.getInstance().GlobeObject != null)
							{
								SmObjectLocator.getInstance().GlobeObject.Dispose();
							}
							if (MainWindow.m_workspace != null)
							{
								MainWindow.m_workspace.Close();
								MainWindow.m_workspace.Dispose();
								MainWindow.m_workspace = null;
							}
                            Application.Current.Shutdown(-1);
                            break;
                        }
                    case "Cancel"://取消退出
                        {
                            this.Close();
                            break;
                        }

                    default:
                        break;
                }
            }
        }
    }
}
