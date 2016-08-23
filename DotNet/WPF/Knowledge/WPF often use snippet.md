1. Xaml中添加后台代码
![](Resources/csharpcodeinxaml.png)
2. Binding中的显示更新到源或UI
```cs   
   BindingExpression binding = tb.GetBindingExpression(TextBox. TextProperty);
   binding.UpdateSource();//将UI上的值更新到后台数据
   binding.UpdateTarget();//将后台数据更新到UI
```
3. 获取某一依赖属性的binding
`var binding= BindingOperations.GetBinding(tb, TextBox.TextProperty);`
4. StringFormat使用
```cs
< TextBlock Text="{ Binding Source={ x:Static system: DateTime.Now },StringFormat= Date:{0 :MMdd}} " VerticalAlignment= "Top" Width="120"/>
```  
没有特殊文本时，需要添加额外的大括号：StringFormat= {}{0 : MMdd}

5. 字符渲染 http://www.wpf-tutorial.com/control-concepts/text-rendering/  
WPF中有两个属性是用来控制字符渲染的，分别为TextOptions.TextFormattingMode和TextOptions.TextRenderingMode,分别定义字符串如何转换及在控件级别渲染，一般不需要定义，根据实际情况处理
6. BitmapToBitmapSource
```cs 
 public BitmapSource ToBmpSrc(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
            var bi = new BitmapImage();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
```
7. 碰撞测试
可用于获取特定位置所有的可视控件或特定组件
```cs
        void PreTest()        
        {
            Point pt = Mouse .GetPosition(this);
            VisualTreeHelper.HitTest(this , OnFilter, OnResult, new PointHitTestParameters (pt));
        }

        /// <summary>
        /// filter some uielement
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private HitTestFilterBehavior OnFilter(DependencyObject o)
        {
            if (o is Border)
                return HitTestFilterBehavior .ContinueSkipSelf;
            return HitTestFilterBehavior .Continue;
        }

        private HitTestResultBehavior OnResult(HitTestResult o)
        {
            Console.WriteLine(o.VisualHit.ToString());
            return HitTestResultBehavior .Continue;
        }
```
8. 获取具有焦点的控件
`FocusManager .GetFocusedElement(this)`
9. 获取鼠标下面的元素
`Mouse.DirectlyOver as UIElement`
10. WPF中右键菜单及Tooltip由InputManager控制，与鼠标右键点击的事件没有特定的关联关系
11. xaml控件的访问级别     
默认为internal，可以使用x:FieldModifier="public" 修改为公有的，使用的前提是需要使用x:Name才可以让外部访问。
