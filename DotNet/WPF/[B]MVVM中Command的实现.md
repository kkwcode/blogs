

## MVVM 中Command的实现    
```cs   
   public class RelayCommand : ICommand
    {
        #region Fields

        readonly Action< object> _execute;
        readonly Predicate< object> _canExecute;

        #endregion // Fields

        #region Constructors

        public RelayCommand( Action< object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand( Action< object> execute, Predicate< object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute" );

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        [ DebuggerStepThrough]
        public bool CanExecute( object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value ; }
            remove { CommandManager.RequerySuggested -= value ; }
        }

        public void Execute( object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
    /// <summary>
    /// this command class need manual operator CanExecutedChanged
    /// </summary>
    public class DelegateCommand : ICommand
    {
        #region Fields

        readonly Action< object> _execute;
        readonly Predicate< object> _canExecute;

        #endregion

        #region Constructors

        public DelegateCommand( Action< object> execute)
            : this(execute, null)
        {
        }

        public DelegateCommand( Action< object> execute, Predicate< object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute" );

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        public bool CanExecute( object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged;

        // The CanExecuteChanged is automatically registered by command binding, we can assume that it has some execution logic
        // to update the button's enabled\disabled state(though we cannot see). So raises this event will cause the button's state be updated.
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged( this, EventArgs.Empty);
        }

        public void Execute( object parameter)
        {
            _execute(parameter);
        }

        #endregion
    }
```
注：
1. 怎么理解 CommandManager.RequerySuggested，参见http://bbs.csdn.net/topics/340007445。  `CommandManager`认为当前的某个改变或动作有可能改变`Command`的能否执行的状态时，就触发该事件。如焦点改变。缺点是会运行多次，影响性能。如`RelayCommand`的实现。而对于第二个`ICommand`的实现`DelegateCommand`，刚需要手动进行按钮的可用状态更新。而这种方式需要人为确定什么时候会修改可用状态。
2. Command的CanExecuted返回条件已经更改，但绑定的按钮本身没有变化，常见于命令中进行一些异步耗时操作，操作完成会影响CanExecute事件结果，但WPF不会立即反应，这时需要调用`CommandManager.InvalidateRequerySuggested`对命令系统进行一次刷新。（必须是UI线程调用 ，否则会无效）
## 任意事件绑定Command
这里分别心TextBlock和ListBox为例。相关的xaml代码如下（比较重要的部分用特殊背景标出）：
`xmlns:i ="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"`
```xml
      <TextBlock HorizontalAlignment ="Left" Margin="10,99,0,0" TextWrapping="Wrap" Text="AnyCommandOfTb"
                   Background="Aquamarine" VerticalAlignment ="Top" Height="22" Width="128" TextAlignment="Center"
                   IsHitTestVisible="True">
            <i: Interaction.Triggers>
                <i: EventTrigger EventName="MouseDown">
                    <i: InvokeCommandAction Command="{ Binding AnyCommandOfTb}" CommandParameter="wow"></ i:InvokeCommandAction >
                </i: EventTrigger>
            </i: Interaction.Triggers>
        </TextBlock>
        <ListBox x :Name="lb"
            HorizontalAlignment="Left" Height ="227" Margin="224,34,0,0" VerticalAlignment="Top" Width="221"
                 FontSize="20" SelectionMode ="Extended" >
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <ContentPresenter Height ="23" Content="{Binding}"></ ContentPresenter>
                </DataTemplate>
            </ListBox.ItemTemplate>
            <i: Interaction.Triggers>
                <!--<i:EventTrigger EventName="SelectionChanged">
                    <i:InvokeCommandAction Command="{Binding AnyCommandOfLb}" CommandParameter="{Binding SelectedItems,ElementName=lb}"></i:InvokeCommandAction>
                </i:EventTrigger>-->
                <!--MvvmLightk提供了一种可以传递事件参数的解决方案，但不能和CommandParameter同时使用，否则只有后都生效-->
                <i: EventTrigger EventName="MouseEnter">
                    <command: EventToCommand Command="{ Binding TestCmd}" PassEventArgsToCommand="True" CommandParameter="2"/>
                </i: EventTrigger>
                <i: EventTrigger EventName="MouseDoubleClick">
                    <i: InvokeCommandAction Command="{ Binding AnyCommandOfLb}" CommandParameter="{Binding SelectedItems ,ElementName=lb}"></ i:InvokeCommandAction >
                </i: EventTrigger>
            </i: Interaction.Triggers>
            <system: String>list item one </system: String>
            <system: String>list item two </system: String>
            <system: String>list item three </system: String>
            <system: String>list item four </system: String>
            <system: String>list item five </system: String>
            <system: String>list item six </system: String>
        </ListBox>
```
     这里用到的比较重要的类库是System.Windows.Interactivity.dll,需要自行下载引用，或通过Nuget引用。
     MouseDoubleClick中传入的参数是IList类型，需要后台处理，当然也可以通过Converter转换处理。
## 对命令添加快捷操作      
两种命令，一种是RoutedUICommand，一种是VM中的RelayCommand  
相关代码片段如下：
```xml        
<Window x :Class="EasyCommandBinding.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x ="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height ="350" Width="525">
    <Window.Resources >
        <RoutedUICommand x :Key="RcCommandOne" Text="RcCommandOne"></ RoutedUICommand>
    </Window.Resources >
    <Window.InputBindings >
        <KeyBinding Gesture ="Ctrl+7" Command="{ StaticResource RcCommandOne}"></ KeyBinding>
        <KeyBinding Gesture ="Ctrl+8" Command="{ Binding RcCommandTwo}"></ KeyBinding>
    </Window.InputBindings >
    <Window.CommandBindings >
        <CommandBinding Command ="{StaticResource RcCommandOne }" Executed="RcCommandOne_OnExecuted"></ CommandBinding>
    </Window.CommandBindings >
    <Grid >
        <Button Content ="RcCommandOne" Command="{ StaticResource RcCommandOne}" HorizontalAlignment="Left" Margin="52,84,0,0" VerticalAlignment ="Top" Width="128"/>
        <Button Content ="RcCommandTwo" Command="{ Binding RcCommandTwo}" HorizontalAlignment="Left" Margin="52,122,0,0" VerticalAlignment ="Top" Width="128"/>
    </Grid >
</Window>
```
注：快捷命令是针对当前的window而言的，如果弹出了二级窗口，再按快捷操作是没有任何作用的。
## WPF中全局快捷命令
```cs
           从名字上看，全局快捷命令与上面不同的是就算弹出二级窗口，仍然可以触发，示例代码如下:
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey( IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [ DllImport( "user32.dll")]
        public static extern bool UnregisterHotKey( IntPtr hWnd, int id);
        //重写wpf中的SourceInitialized方法
        protected override void OnSourceInitialized( EventArgs e)
        {
            base.OnSourceInitialized(e);

            IntPtr handler = new WindowInteropHelper(this ).Handle;
            //0x9999 热键的唯一标识
            //0x0001 Alt键，还有其它值，如Ctrl、Shift,见http://msdn.microsoft.com/en-us/library/windows/desktop/ms646309%28v=vs.85%29.aspx
            //0x46 代表F键，其它按钮值见http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731%28v=vs.85%29.aspx
            RegisterHotKey(handler, 0x9999, 0x0001, 0x46);

            HwndSource source = PresentationSource .FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handle)
        {
            if (wParam.ToInt32() == 0x9999)
            {
                //所要进行的全局操作命令
                MessageBox.Show("no......." );
            }

            return IntPtr.Zero;
        }
```
## 资源
>http://www.codeproject.com/Tips/478643/Mouse-Event-Commands-for-MVVM
>http://www.cnblogs.com/chenxizhang/archive/2011/10/01/2197786.html 