   M S B A M L   `   `   `  ����F  Bxabbo-music, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:xabbo_music.Formsxabbo_music.Forms  5   	   ;"clr-namespace:xabbo_music.Controlsxabbo_music.Controls  5   	   *    $xabbo_music.Forms.WiredSpaceSelector   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:xabbo_music.Forms     5   	   7controls"clr-namespace:xabbo_music.Controls     5   	     � Title$  Music Build Settings��5	   	   $	��460��6&   $	��300��63    � 
ResizeMode$ NoResize=�5
   	   
������  � AllowsTransparency$
 True��5   	    � WindowStyle$
 None=�5   	   .��5      �� $��1q�6    �� CornerRadius$ 10��6!   ���#6794a7
���   �._�5   
   �� $��
0, 0, 1, 1q�6   $ 10��6.   ���#345e6f._�5      �� $��
1, 1, 0, 0q�6   $ 10��62   ���#79aec4._�5      �  � RowDefinitions 5      �� 5      $��19 �6(   �� 5      $	��105 �6(   �� 5      .+�5      � $��0��6   .+�5      �� -   5        $	��299��5   !   $ 10, 10, 0, 0��6-   $��Stretch=�6f   $��Stretch=�6�   ��5      �� 5   "    j� Stretch$
 None=�6-    �� ImageSource$+ %/Images/room_selector_topbar_dots.png��6<   ._�5      � ���#6794a7$��Center=�68   .+�5   "   �� $��Song Builder Settings��6,   $��Center=�6I   $��Center=�6f   
�������$��	10,0,10,0q�6�   5      5       5!      � 5#      $��1��6   $��15, 11, 15, 14q�5$       5%      �� 5&      �� 5'      $��40 �6,   .+�5*      �� $a��[Please, indicate the positions where you want to place the wireds, SFX furni and controller��6$   $	��Top=�6�   $
��Wrap=�6�    �� TextAlignment$ Justify=�6�   � 5,      $��1��6   .+�5-      (   "xabbo_music.Controls.InventoryItem  -   5        	  Name$	 	WiredItem��5-   5   $
��Left=�6H     5.      -   5        $	 SFXItem��5.   5   $
��Left=�6F   $��44, 0, 0, 0q�6a     5/      -   5        $	 ControllerItem��5/   5   $
��Left=�6M   $��88, 0, 0, 0q�6h   50      51      � 53      -   5        
 �Name$
 MainGrid��53      $��2��54      $��15, 0, 15, 5q�55       57      �� 58      $	��250 �6,   �� 59      $��35 �6,   �� 5:      .+�5=      � .+�5>      '   !xabbo_music.Controls.HabboTileMap  -   5          Name$ HabboTileMap��5>   4   �� 5?      $ 10��6%   ���	#30000000$��7,7,0,0q�6O   5@      � 5B      $��1��6   $��Center=�5C      .+�5D      �� $��Magic Tile ID��6(   $
��Left=�6=   $��Center=�6X   �� 5E      $
��Left=�6%   $��80, 0, 0, 0q�6@   
�������
�������$��2q�6|   $ 5��6�   $��90��6�   $��20��6�   ._�5F   "   �� -   5         ��Name$ TB_MagicTile��5F   *    �� TextAlignment$ Center=�6g   $��0q�6~   5G      5H      � 5J      $��2��6    � ColumnDefinitions 5K      �� 5L   "   �� 5M   "   .+�5P      &    xabbo_music.Controls.HabboButton  -   5          Name$ CancelButton��5P   3     5Q      -	   5        $ BuildButton��5Q   3   $��1��6H   5R      5S      5T      5U      5V      5W      5X      