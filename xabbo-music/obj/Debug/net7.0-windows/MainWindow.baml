   M S B A M L   `   `   `  ����F  Bxabbo-music, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:xabbo_music.Controlsxabbo_music.Controls  5   	       xabbo_music.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7controls"clr-namespace:xabbo_music.Controls     5   	     � Title$  
MainWindow��5   	   $	��785��6   $	��465��6(    � WindowStyle$
 None=�5	   	    � AllowsTransparency$
 True��5
   	   
������  � UseLayoutRounding$
 True��5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �� -   5        
���   ����#587580$��1q�5   O    �� CornerRadius$ 10��6c   ._�5   
   �� $��
0, 0, 1, 1q�6   $ 10��6.   ���#445b63._�5      �� $��
1, 1, 0, 0q�6   $ 10��62   ���#aeb5bb._�5      � -   5         �Name$ MainGrid��5       � RowDefinitions 5      �� 5      $��19 �6(   �� 5      .+�5      �� -   5        	 ��Name$	 TopBar��5      $	��783��6-   $ 10, 10, 0, 0��69   $��Stretch=�6r   $��Stretch=�6�   ��5      �� 5      
 j� Stretch$

 None=�6)    �� ImageSource$% /Images/jukebox_topbar_dots.png��68   ._�5      � .+�5      � ���#587580$��Center=�68   .+�5   "   �� $��Xabbo Music��6,   $��Center=�6?   $��Center=�6\   
�������$��	10,0,10,0q�6�   5       � 5"      $��Right=�6#   $��0,0,11,0q�6?   .+�5#   "   �� -   5         � Cursor$
 Hand��5#   C   $��11��6Q   $��11��6]   $��0q�6h   ���	#00000000��5$   &   �� 5%   *    �� 
TargetType" ��6:   .R�5&   .   �� $��/Images/button_close.png��64   5'   +   5)   #   5*      5+      5,      � 5.      -   5        $ 	MainGrid2��5.      $��1��5/      $��15, 11, 14, 14q�50       52      �� 53      �� 54      $	��175 �6,   .+�57      � $��0��6   $��0��58      $��3��59       K� ZIndex$
 1000��5:      .+�5;      � -   5        $ UpperMainGrid��5;   #    � ColumnDefinitions 5<   "   �� 5=   &   $��20 �67   �� 5>   &   $	��200 �67   �� 5?   &   �� 5@   &   $��20 �67    5C   "   �� 5D   &   �� 5E   &   $��35 �64   .+�5H   "   � $��1��6'   $��2��5I   '   $	��Top=�5J   '   .+�5K   &   *   $xabbo_music.Controls.EffectInventory  -   5          Name$ EffectInventory��5K   X   $	��192��6?   $	��235��6K   6X   5L   #   � 5N   "   $��2��6'    5O   &   �� 5P   *   $	��511 �6;   �� 5Q   *   .+�5T   &   � -   5        $ Grid_NoteInventories��5T   +   $
 2000��5U   +   .+�5V   *   �� ���#3a4d54$��1��6C   �� 5W   *   ���#7097a6$��1��6C   $��
0, 1, 0, 0q�6N   5X   '   �� 5Z   &   -	   5         ��Name$ TB_HorizontalOffset��5Z   0   5[   #   � 5]   "   -
   5        $ TimelineButtonsGrid��5]   D   $��1��6'   $��2��64   6D   5^      5_      *   $xabbo_music.Controls.TimelineControl  5a      -   5          Name$ Timeline��5a   @   $��2��63   6@   5b      5c      5d      5e      5f      5g      