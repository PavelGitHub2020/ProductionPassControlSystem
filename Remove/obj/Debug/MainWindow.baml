   M S B A M L   `   `   `  ????A  =Remove, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:RemoveRemove  5   	       Remove.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Remove     5   	     ? Title$  Remove??5   	   $	??460??6   $
??1020??6%   
???   ?$	??460??6E   $
??1020??6U    ? 	Resources 5	      )   #Remove.DepartmentNameToColorConvert    DepartmentNameToColorConvert)  #System.Windows.Controls.DataGridRow ?? 
TargetType ?? Property ?? Value ?? 	Converter  soundOn  soundOf  eng  ru??%?  &	        '       7   &	 j     &	 ?     &	     &	 Q      5
   
   6-   ?? 5   
   "  6   .?5      ??  ? ?? 6+   ?? *Name1+8   5      ?? 5   
   $??Resources\sound_on.png??6    $??25??6@   $??25??6L   ?? 5   
   $??Resources\sound_of.png??6    $??25??6@   $??25??6L   ?? 5   
   $??Resources\englishIcon.jpg??6   $??25??6?   $??25??6K   ?? 5   
   $ ??Resources\russianIcon3.jpg??6   $??25??6?   $??25??6K   5      .??5      ? .+?5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ removeWorkerGrid??5      $??Center=?6-   $	??600??6J   $	??380??6V   $??-390,25,0,0q?6c   ?? 5   
   $??30??6   $??20(?6    $??Center=?6.   $??-720,-395,0,0q?6K   
???  ??#!To delete an worker, enter its ID5   H   ?? 5   
   -   5         ??Name$ 	textBoxId??5      $??20(?6%   $	??100??63   $??25??6?   $??Center=?6K   $??-330,-395,0,0q?6h   6?   ?? 5   
   -   5         ??Name$ buttonRemove??5      $??70??6'   $??25??62   $??Center=?6>   $??15(?6[   $??-150,-395,0,0q?6i     ChangeColor#??[ 5   &   .??6K   Remove6S   ?? 5   
   -   5        $ sound??5      $??25??6!   $??25??6,    	 Sound#??[ 68   $??Center=?5      $??-50,-395,0,0q?6]   	 ? ToolTip$	 Sound on??5      .??5       C? 
 C? ResourceKey$
 soundOn??6   5!      ?? 5#   
   -   5        $ language??5#      $??25??6#   $??25??6.   $??Center=?5$      $??10,-395,0,0q?6X   $	 English for sound??5%      .??5&      C? $	
 eng??6   5'      ?? 5)   
   -   5        $ theWholeList??5)      $	??130??6'   $??30??63   $??Center=?6?   $??360,-320,0,0q?6\   $??15(?5*      #??[ 6<   .??6b   The whole list6r   ?? 5,   
   -   5        $ serviceM??5,      $	??130??6#   $??30??6/   
??? E??$??Center=?6R   $??15(?6o   $??360,-220,0,0q?6}   .??5-   )   View separately6:   ?? 5.   
   -   5        $ serviceH??5.      $	??130??6#   $??30??6/   
???/???$??Center=?6T   $??15(?6q   $??360,-120,0,0q?6   .??5/   )   View separately6:   ?? 50   
   -	   5        $ trafficServiceClick??50      $	??130??6.   $??30??6:   
????? ?$??Center=?6X   $??15(?6u   $??360,-20,0,0q?6?   .??51   /   View separately6@   ?? 52   
   -
   5        $ electroMechanicalService??52      $	??130??63   $??30??6?   
???????$??Center=?6^   $??15(?6{   $??
360,80,0,0q?6?   .??53   9   View separately6J   ?? 54   
   -   5        $ securityService??54      $	??130??6*   $??30??66   
???????$??Center=?6Z   $??15(?6w   $??360,180,0,0q?6?   .??55   0   View separately6A   ?? 56   
   -   5        $ economicDepartment??56      $	??130??6-   $??30??69   
???????$??Center=?6V   $??15(?6s   $??360,280,0,0q?6?   .??57   3   View separately6D   ?? 58   
   -   5        $ computerDepartment??58      $	??130??6-   $??30??69   
??? ???$??Center=?6W   $??15(?6t   $??360,370,0,0q?6?   .??59   3   View separately6D   ?? 5;   
   -   5         ??Name$ all_Services??5;      $??15(?6*   $??Center=?68   $	??100??6U   $??20??6a   $??650,-320,0,0q?6m   
???????All sevices5<   4   ?? 5>   
   -   5        $ 	service_M??5>      $??15(?6'   $??Center=?65   $	??100??6R   $??20??6^   $??650,-220,0,0q?6j   
??? E??	Service M5?   6   ?? 5@   
   -   5        $ 	service_H??5@      $??15(?6'   $??Center=?65   $	??100??6R   $??20??6^   $??650,-120,0,0q?6j   
???/???	Service H5A   8   ?? 5B   
   -   5        $ traffic_Service??5B      $??15(?6-   $??Center=?6;   $	??100??6X   $??20??6d   $??650,-20,0,0q?6p   
????? ?Traffic service5C   7   ?? 5D   
   -   5        $  electro_Mechanical_Service??5D      $??15(?68   $??Center=?6F   $	??175??6c   $??20??6o   $??
730,80,0,0q?6{   
???????Electro-mechanical service5E   C   ?? 5F   
   -   5        $ security_Service??5F      $??15(?6.   $??Center=?6<   $	??105??6Y   $??20??6e   $??660,180,0,0q?6q   
???????Security service5G   >   ?? 5H   
   -   5        $ economic_Department??5H      $??15(?61   $??Center=?6?   $	??150??6\   $??20??6h   $??700,280,0,0q?6t   
???????Economic department5I   :   ?? 5J   
   -   5        $ computer_Department??5J      $??15(?61   $??Center=?6?   $	??150??6\   $??20??6h   $??700,366,0,0q?6t   
??? ???Computer department5K   ;   5L      5M      