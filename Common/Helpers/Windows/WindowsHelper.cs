using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Globe.QcApp.Common.Helpers.Windows
{
    /// <summary>
    /// windows窗口处理工具
    /// </summary>
    public class WindowsHelper
    {
        /// <summary>
        /// 根据需要调整窗口大小
        /// </summary>
        /// <param name="isFullScreen">是否开启全屏</param>
        /// <param name="hoolWin">需要调整的窗口</param>
        public void adjustWindow(Window hoolWin, Window subWin, bool isFullScreen = false)
        {
            if (!isFullScreen)
            {
                Rect rc = SystemParameters.WorkArea;//获取工作区大小

                hoolWin.WindowState = System.Windows.WindowState.Normal;
                hoolWin.Left = 0;//设置位置
                hoolWin.Top = 0;
                hoolWin.Width = rc.Width;
                hoolWin.Height = rc.Height;

                subWin.WindowState = System.Windows.WindowState.Normal;
                subWin.Left = 0;//设置位置
                subWin.Top = 0;
                subWin.Width = rc.Width;
                subWin.Height = rc.Height;
            }
            else
            {
                hoolWin.WindowState = System.Windows.WindowState.Maximized;
                subWin.WindowState = System.Windows.WindowState.Maximized;
            }
        }
    }
}
