using Globe.QcApp.Common.Helpers.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button)
            {
                //ApplicationThemeManager.GetInstance().EnsureResourcesForTheme((e.Source as Button).Content.ToString());
                string name = (e.Source as Button).Name.ToString();
                switch (name)
                {
                    case "ZoomIn":
                        {
                            break;
                        }
                    case "ZoomOut":
                        {
                            break;
                        }
                    case "Pan":
                        {
                            break;
                        }
                    case "FullExent":
                        {
                            break;
                        }
                    case "SearchLocation":
                        {
                            break;
                        }
                    case "FullScreen":
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
                    case "ExitSystem":
                        {
                            Application.Current.Shutdown(-1);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void OnConfirmClosed(object sender, WindowClosedEventArgs e)
        {
        }		
    }
}
