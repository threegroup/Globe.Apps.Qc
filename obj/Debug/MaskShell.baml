   M S B A M L   `   `   `  ����  Globe.QcApp    Globe.QcApp.MaskShell   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   k gTelerik.Windows.Controls.Input, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7a ]Telerik.Windows.Data, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7e aTelerik.Windows.Controls, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7n	 jTelerik.Windows.Controls.GridView, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7p
 lTelerik.Windows.Controls.Navigation, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7j fTelerik.Windows.Controls.Data, Version=2015.2.728.45, Culture=neutral, PublicKeyToken=5803cfa389c90ce7Itelerik1http://schemas.telerik.com/2008/xaml/presentation    	 
  5   	     � Title$  
MaskWindow��5   	    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�5   	    � Topmost .   � SizeToContent$ Manual=�5	   	    � AllowsTransparency .  � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterOwner=�5   	   ��5   	   Z�  � 	Resources 5      	 �� MergedDictionaries
 �� Source   LayerListBoxItemStyle �� 
TargetType �� Property �� Value  MarginValue  LayerListBoxItemTemplate �� Color  StackPanelStyle d� 	IsChecked
 � Tag  headerTemplate  imageContentTemplate  videoContentTemplate r� Source�� 5   
   	 5      �� 5      $/
 )ResourceDictionary/ToolBarDictionary.xaml?�6%   %  &	        7  &	 X    7  7   &	 
    &	 G    &	 �    �� 5      " ��62   �5      �� 5       � ��$
 Left=�6<   �� 5       � ��$ Center=�6:   �� 5       � ��8   6/   �� 5       � ��$ Microsoft YaHei UI Light)�63   �� 5       � ��$ 14(�61   �� 5       � ��
 �   �63   �� 5       � ��$ 10,0,0,0q�6/   -   5        5       �� 5#      .R�5$      �� $��itemBg��6   $��Stretch=�6'   $��0,0,0,1q�6E   ��5%      �� 5&      $ 	#FFEC6D51��6*   ._�5(      �� $��0��6!   8��  .+�5)      �� $-��'/Globe.QcApp;component/Images/layer.png��6    $��32��6Q   $��32��6\   $��UniformToFill=�6h   �� 5*      8�� ��6#   �� *LayerCaption+ 6D   �� *LayerVisible+ 6g   �� *LayerOrigin+6�   5+      5,      5-      �� 50      .R�51      �� ��6   �� *Id+52      �� 54      .R�55      �� ��6   �� *
ImageUrl+56      �� 58      .R�59      r�  6   �� *
VideoUrl+5:      5;      .��5=      �  � RowDefinitions 5>   
   �� 5?      $
��Auto �6   �� 5@      $��* �6   �� 5A      $��20 �6   .+�5C   
   �� $��Center=�6   $
��Left=�60    �� Orientation$ 
Horizontal=�6K   .+�5D      �� $��Images/logo.ico��6   $��UniformToFill=�6-   �� 5E      -   5        $��SystemTitleId��5E      $��36(�6-   $
��Bold"�6;   
�������$��Center=�6`   $��Microsoft YaHei UI Light)�6{   5F      � 5I   
   -   5        $��DetailPanel��5I      $��1��6"   $	��500��6/   $	��300��6;   $
��Left=�6H   $��Bottom=�6c   ���	#7F000000$y�	Collapsed=�6�    5J      �� 5K      $
��Auto �6    �� 5L      $��* �6    .+�5N      �� $��Stretch=�6   $��0,0,0,1q�63   ��5O      �� 5P      $ 	#FFEC6D51��6&   ._�5R      � $��Stretch=�6   $��Stretch=�65   $��30��6Q    � ColumnDefinitions 5S      �� 5T      $��* �6+   �� 5U      $
��auto �6+   .+�5W      �� -   5        $��ShowLegendTitle��5W       $��0��67   
�������$��Center=�6Z   $��16(�6u   $��Microsoft YaHei UI Light)�6�   $��Center=�6�   �� 5X      -   5        $��1��5X      $��Images/close.png��6,    � Cursor$
 Hand��6F   $��24��6T   $��24��6_   $��UniformToFill=�6k   6�   5Y      5Z      , 
 &Telerik.Windows.Controls.RadTabControl  5[      $��1��6$   $��0q�61    �� DisplayMemberPath$ Content��6<      
ScrollMode$ Viewport=�6X   $��0q�6n    J� Opacity$	 0.9Q�6�      TabOrientation$ 
Horizontal=�6�      TabStripPlacement$
 Left=�6�   $
 Hand��6�   .@�5\      ) 
 #Telerik.Windows.Controls.RadTabItem  $��基本信息��6%   $
 Hand��63    J� RenderTransformOrigin$ 0.5,0.5&�6A   z�5]      V� 5^      .~�5_      �� �� 5`      �� 5a       �� Angle$	 180Q�6.   T� 5b      5c      .��5e      * 	 $Telerik.Windows.Controls.RadGridView  -   5          Name$ DetailRadGridView��5e   *   $��14(�5f   "   $��Microsoft YaHei UI Light)�5g   "   $	 0.9Q�5h   "   $��Stretch=�5i   "   $��Stretch=�5j   "   8 	 2Telerik.Windows.Controls.GridView.BaseItemsControl   GroupRenderMode$
 Flat=�5k   "   ; 	 5Telerik.Windows.Controls.GridView.GridViewDataControl   AutoGenerateColumns .      AutoExpandGroups  .  !   CanUserFreezeColumns! .  "   RowIndicatorVisibility$" 	Collapsed=�5o      #   CanUserResizeColumns# .  !$   EnableColumnVirtualization$ .  '%    EnableColumnGroupsVirtualization% .  +  %Telerik.Windows.Controls.StyleManager&   Theme-  'Telerik.Windows.Controls.ThemeConverter$& Windows8Touch 5s   "   '   CanUserDeleteRows' .  (   CanUserInsertRows( .  )   CanUserReorderColumns) .  *   CanUserSortColumns* .  +   CanUserSortGroups+ .  #,   EnableLostFocusSelectedState, .  -   EnableRowVirtualization- .  .   IsFilteringAllowed. .  */   #IsPropertyChangedAggregationEnabled/ .  0   ShowGroupPanel0 .  1   
IsReadOnly1 . 2   GridLinesVisibility$
2 Both=�5   "     DataGridLineColor3   VerticalGridLinesBrush#3 [ 5�   "   4   HorizontalGridLinesBrush#4 [ 5�   "   ���	#7F000000*  $Telerik.Windows.Controls.DataControl5   CanUserSelect5 .  6   EditTriggers$
6 None=�5�   "   7   DragElementAction$
7 None=�5�   "   8   ValidatesOnDataErrors$
8 None=�5�   "   9   ShowColumnHeaders9 .  $
 Hand��5�   "   :   Columns: 5�      1	 	 +Telerik.Windows.Controls.GridViewDataColumn	  5�      6
 	 0Telerik.Windows.Controls.GridViewBoundColumnBase; 
  Header$; 名称��69   - 	 'Telerik.Windows.Controls.GridViewColumn<   Width? 	 9Telerik.Windows.Controls.GridView.GridViewLengthConverter$
< auto 6E   = 
  DataMemberBinding= 5�   1   �� *FeatureField+	  5�      $; 内容��69   $< * 6E   = 5�      �� *FeatureValue+5�      5�        5�      $��图片信息��6%   $
 Hand��63   $ 0.5,0.5&�6A   z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�      $	 180Q�6.   T� 5�      5�      .��5�      * 
 $Telerik.Windows.Controls.RadTileView  -   5        $��ImageTileViewID��5�   *   $��0q�6A   >   IsAutoScrollingEnabled> . ?   IsDockingEnabled? . @   IsVirtualizing@ . A   ColumnsCount$A 2��5�      B   ColumnWidth$	B 230 �5�      C   	RowHeight$	C 120 �5�      D   MinimizedColumnWidth$D 80 �5�      E   MinimizedRowHeight$	E 100 �5�      $F   PreservePositionWhenMaximizedF . #��[ 5�      G   ContentTemplate#G [ 5�      5�        5�      $��视频信息��6%   $
 Hand��63   $ 0.5,0.5&�6A   z�5�      V� 5�      .~�5�      �� �� 5�      �� 5�      $	 180Q�6.   T� 5�      5�      .��5�        -   5        $��VideoTileViewID��5�   *   $��0q�6A   > . ? . @ . $A 2��5�      $	B 230 �5�      $	C 120 �5�      $D 80 �5�      $	E 100 �5�      F . #��[ 5�      #G [ 5�      5�      5�      5�      � 5�   
   -	   5        $��PanelRegion��5�      $��1��6"   $	��400��6/   $	��500��6;   $��0q�6H   $��Right=�6S   $��Bottom=�6o   ���	#7F000000 5�      �� 5�      $
��Auto �6    �� 5�      $��* �6     5�      �� 5�      $
��Auto �6#   �� 5�      $��* �6#   .+�5�      �� -
   5        $��ControlPanelButton��5�      $
��HIDE��60   $��1��6?   $��0��6L   $��Center=�6\   $	��Top=�6y     RightButtonStyle#��[ 6�   5�      �� 5�      -   5        $��0��5�      $��1��6'   $ 
Horizontal=�67   $��Center=�5�      $��Center=�66   .+�5�      �� -   5        $��DistanceMeasure��5�       	 DistanceButtonStyle#��[	 60   H � ToolTipH 5�      _� 5�       
 ToolTipStyle#��[
 6"   .��5�      �� 距离测量6.   5�      5�      �� 5�      -   5        $��AreaMeasure��5�        AreaButtonStyle#��[ 6,   H 5�      _� 5�      #��[
 6"   .��5�      �� 面积测量6.   5�      5�      �� 5�      -   5        $��HeightMeasure��5�        HeightButtonStyle#��[ 6.   H 5�      _� 5�      #��[
 6"   .��5�      �� 高度测量6.   5�      5�      �� 5�      -   5        $��Clear��5�        ClearButtonStyle#��[ 6&   H 5�      _� 5�      #��[
 6"   .��5�      �� 清除6,   5�      5�      �� 5�      -   5        $��	FullExent��5�        FullExtentButtonStyle#��[ 6*   H 5�      _� 5�      #��[
 6"   .��5�      �� 场景全幅6.   5�      5�      �� 5�      -   5        $��
FullScreen��5�        FullButtonStyle#��[ 6+   H 5�      _� 5�      #��[
 6"   .��5�      �� 全屏显示6.   5�      5�      �� 5�      -   5        $��
ExitSystem��5�        CloseButtonStyle#��[ 6+   H 5�      _� 5�      #��[
 6"   .��5�      �� 退出系统6.   5�      5      5       5     -   5        $��SysRadTabControl��5  $   $��1��6<   $��1��6J   $ Content��6Z   $ Viewport=�6v   $��0q�6�   $	 0.9Q�6�   $ 
Horizontal=�6�   $ Right=�6�   $
 Hand��6�   .@�5       $��图层管理��6%   $
 Hand��63   .��5     � .+�5     )  #Telerik.Windows.Controls.RadListBox  -   5        $��LayerListBox��5  -   $��Disabled=�6A   $��Stretch=�6w   $��Stretch=�6�   $��0q�6�   #��[ 6�   ��6�   Z� 5     5     5	       5
     $��坐标定位��6%   $
 Hand��63   .��5     �  5     �� 5     $
��Auto �6,   �� 5     $
��Auto �6,   �� 5     $
��Auto �6,   �� 5     $
��Auto �6,   �� 5     $
��Auto �6,   �� 5     $��* �6,   .+�5     �� $��0��6!   ���	#FFEC6D51$��0,0,0,1q�6F   ._�5     �� $
��Left=�6)   $ 
Horizontal=�6D   $��Center=�6]   .+�5  "   �� $��经纬度坐标：��6(   $��Microsoft YaHei UI Light)�69   6a   5     5     �� 5     $��1��6!   ���	#FFEC6D51$��0,0,0,1q�6F   ._�5     �� $
��Left=�6)   $ 
Horizontal=�6E   $��Center=�6^   .+�5  "   �� $��经度[米]：��6(   $��Microsoft YaHei UI Light)�69   $��Center=�6_   6|   �� 5  "   -   5        $��longitudeTxt��5  +   $	��200��6@   $��3q�6L   I �� IsInputMethodEnabledI .  6�   5     5     �� 5!     $��2��6!   ���	#FFEC6D51$��0,0,0,1q�6F   ._�5"     �� $
��Left=�6)   $ 
Horizontal=�6D   $��Center=�6]   .+�5#  "   �� $��纬度[米]：��6(   $��Microsoft YaHei UI Light)�69   $��Center=�6_   6|   �� 5$  "   -   5        $��latitudeTxt��5$  +   $	��200��6>   $��3q�6J   I .  6�   5%     5&     �� 5(     $��3��6!   ���	#FFEC6D51$��0,0,0,1q�6F   ._�5)     �� $
��Left=�6)   $ 
Horizontal=�6D   $��Center=�6]   .+�5*  "   �� $��高度[米]：��6(   $��Microsoft YaHei UI Light)�69   $��Center=�6_   6|   �� 5+  "   -   5        $��altitudeTxt��5+  +   $
��5000��6>   $	��200��6J   $��3q�6V   I .  6�   5,     5-     �� 5.     $��4��6%   $ Vertical=�63   $��Center=�6J   $��Center=�6g   .+�5/     �� -   5        $��LocaltionBt��5/  2   $��定位��6%   $��30��6E   6Q   �� 50     -   5        $��locatorInfo��50  (   $��Microsoft YaHei UI Light)�6;   
���  ��6a   51     53     54       55     $��要素查询��6%   $
 Hand��63   .��56     �  57     �� 58     $
��Auto �6,   �� 59     $
��Auto �6,   �� 5:     $
��Auto �6,   �� 5;     $
��Auto �6,   �� 5<     $��* �6,   .+�5>     �� $��0��6"   $��0,0,0,1q�6/   ���	#FFEC6D51._�5?     �� $
��Left=�6)   $ 
Horizontal=�6D   .+�5@  "   �� $��选择要素：��6(   $��Microsoft YaHei UI Light)�68   $��Center=�6^   6{   *  $Telerik.Windows.Controls.RadComboBox  5A  "   -   5        $��QueryLayerList��5A  6   $	��222��6L   $��3q�6Y   $ LayerCaption��6d   J �� SelectedValuePath$J 	LayerName��6�   6�   5B     5C     � 5D     $��1��6     5E     �� 5F  "   $
��Auto �60   �� 5G  "   $
��Auto �60   .+�5I     �� $��0��6&   $��0,0,0,1q�63   ���	#FFEC6D51._�5J  "   �� $ 
Horizontal=�6-   .+�5K  &   �� $��属性查询：��6,   $��Microsoft YaHei UI Light)�6<   $��Center=�6b   �� 5L  &   -   5        $��QueryNameTxt��5L  .   $	��146��6B   $��3q�6O   $
��Left=�6Z   6w   �� 5M  &   -   5        $��QueryBt��5M  :   $��查询��6-   $��30��6I   $��70��6U   6`   5N  #   5O     �� 5P     $��1��6&   $��0,0,0,1q�63   ���	#FFEC6D51._�5Q  "   �� $ 
Horizontal=�6.   .+�5R  &   �� $��空间查询：��6,   $��Microsoft YaHei UI Light)�6<   $��Center=�6b   �� 5S  &   -   5        $��PointQueryBt��5S  ;   $��	点查询��6-   $��30��6O   $��70��6[   6f   �� 5T  &   -   5        $��CircleQueryBt��5T  ;   $��	圆查询��6-   $��30��6P   $��70��6\   6g   �� 5U  &   -   5        $��PolygonQueryBt��5U  =   $��多边形查询��6-   $��30��6S   $��70��6_   6j   5V  #   5W     5X     �� 5Y     -    5        $��	QueryInfo��5Y  $   $��3��65   $�� ��6C   K �� TextAlignment$
K Left=�6K   $��Stretch=�5Z  $   $��Center=�5[  $   $��Microsoft YaHei UI)�6?   $��6,3,0,3q�6_     5\     -!   5        $��QueryListBox��5\  -   $��4��6A   $��Disabled=�6N   $��Stretch=�6�   $��Stretch=�6�   $��0q�6�     QueryListItemTemplate#��[ 5]     6S   5^     5_       5`     $��飞行漫游��6%   $
 Hand��63   .��5a     �  5b     �� 5c     $��* �6,   �� 5d     $
��Auto �6,   .+�5f     �� -"   5        $��PanelContentRegion��5f  !   $��0,0,0,1q�6;   $��0��6U   ���	#FFEC6D51._�5g       -#   5        $��RouteListBox��5g  1   $��Disabled=�6E   $��Stretch=�6{   $��Stretch=�6�   $��0q�6�     ListBoxItemTemplate#��[ 6�   ��6�   Z� 6  5h     �� 5i     -$   5        $��1��5i  %   $ 
Horizontal=�62   $��Center=�5j     $��Center=�66   .+�5k     �� -%   5        $��
StartRoute��5k  %     StartRouteButtonStyle#��[ 67   �� 5l     -&   5        $��
PauseRoute��5l  %     PauseRouteButtonStyle#��[ 67   �� 5m     -'   5        $��	StopRoute��5m  %     StopRouteButtonStyle#��[ 66   5n     5o     5p     5q     5r     5s     5t     