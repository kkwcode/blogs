# Behavior、TriggerAction、TargetedTriggerAction
这三种类都是用来扩展现有控件的行为。使用时需要引用`System.Windows.Interactivity.dll`。
`Behavior`及`TriggerAction`都是用来扩展自身行为,而`TargetdTriggerAction`则是根据自身行为扩展目标行为。
一个简单的使用例子如下（参见 http://www.codeproject.com/Tips/401707/Behavior-and-Trigger-in-WPF-Silverlight）：
```cs
public class MouseEnterBehavior: Behavior< TextBlock>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.MouseEnter += AssociatedObject_MouseEnter;
            this.AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        void AssociatedObject_MouseLeave( object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.AssociatedObject.Background = Brushes.Transparent;
        }

        void AssociatedObject_MouseEnter( object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.AssociatedObject.Background = Brushes.Aquamarine;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.MouseEnter -= AssociatedObject_MouseEnter;
            this.AssociatedObject.MouseLeave -= AssociatedObject_MouseLeave;
        }
    }

    public class MouseEnterAction : TriggerAction<TextBlock >
    {
        protected override void Invoke( object parameter)
        {
            this.AssociatedObject.Text = "MouseEnterAction";
        }
    }

    public class TargetMouseEnterAction : TargetedTriggerAction<TextBlock >
    {
        protected override void Invoke( object parameter)
        {
            this.Target.Text = ( this.AssociatedObject as TextBlock).Text;
            this.Target.Background = ( this.AssociatedObject as TextBlock).Background;
        }
    }
```
```cs
       <TextBlock HorizontalAlignment ="Left" Height="17" Margin="32,39,0,0" TextWrapping="Wrap" Text="MouseEnterBehavior" VerticalAlignment ="Top" Width="124">
            <i: Interaction.Behaviors>
                <behaviorSample: MouseEnterBehavior/>
            </i: Interaction.Behaviors>
            <i: Interaction.Triggers>
                <i: EventTrigger EventName="MouseDown">
                    <behaviorSample: MouseEnterAction/>
                    <behaviorSample: TargetMouseEnterAction TargetName="AssociateTb"/>
                </i: EventTrigger>
            </i: Interaction.Triggers>
        </TextBlock>
        <TextBlock x :Name="AssociateTb" HorizontalAlignment="Left" TextWrapping="Wrap" Text="TargetTb" VerticalAlignment ="Top" Margin="32,71,0,0" Width="113"/>
```