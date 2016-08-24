管理控件的状态及用于状态过渡的逻辑
这里直接给出官网给出的一个示例。  
**前台代码**
```cs
<Rectangle Name="rect" 
           Width="100" Height="100"
           MouseEnter="rect_MouseEvent" 
           MouseLeave="rect_MouseEvent">
  <VisualStateManager.VisualStateGroups>
    <VisualStateGroup Name="MouseStates">
      <VisualState Name="MouseEnter">
        <Storyboard>
          <ColorAnimation To="Green" 
                          Storyboard.TargetName="rectBrush" 
                          Storyboard.TargetProperty="Color"/>
        </Storyboard>
      </VisualState>
      <VisualState Name="MouseLeave" />
      <VisualStateGroup.Transitions>
        <VisualTransition To="MouseLeave" GeneratedDuration="00:00:00"/>

        <VisualTransition To="MouseEnter" GeneratedDuration="00:00:00.5">
          <VisualTransition.GeneratedEasingFunction>
            <ExponentialEase EasingMode="EaseOut" Exponent="10"/>
          </VisualTransition.GeneratedEasingFunction>
        </VisualTransition>

      </VisualStateGroup.Transitions>
    </VisualStateGroup>
  </VisualStateManager.VisualStateGroups>

  <Rectangle.Fill>
    <SolidColorBrush x:Name="rectBrush" Color="Red"/>
  </Rectangle.Fill>
</Rectangle>
```
**后台代码**
```cs
private void rect_MouseEvent(object sender, MouseEventArgs e)
{
    if (rect.IsMouseOver)
    {
        VisualStateManager.GoToElementState(rect, "MouseEnter", true);
    }
    else
    {
        VisualStateManager.GoToElementState(rect, "MouseLeave", true);
    }
}
```  
在控件的Template设置VisualState,其Name即表示触发事件本身。
**引用**
> https://msdn.microsoft.com/zh-cn/library/system.windows.visualstatemanager(v=vs.110).aspx
> https://msdn.microsoft.com/zh-cn/library/ee230084(v=vs.110).aspx 关于storyboard使用的一些实例
