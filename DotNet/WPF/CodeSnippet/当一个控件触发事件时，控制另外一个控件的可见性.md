# 当一个控件触发事件时，控制另外一个控件的可见性
## 使用EventTrigger
```cs
<RadioButton x:Name="radioButton" Content="R1" HorizontalAlignment="Left" Margin="179,66,0,0" VerticalAlignment="Top"/>
        <RadioButton x:Name="radioButton1" Content="R2" IsChecked="True"  HorizontalAlignment="Left" Margin="231,66,0,0" VerticalAlignment="Top">
            <RadioButton.Triggers>
                <EventTrigger RoutedEvent="RadioButton.Checked">
                    <BeginStoryboard>
                        <Storyboard>
                            <ObjectAnimationUsingKeyFrames
                                Storyboard.TargetProperty="(UIElement.Visibility)"
                                Storyboard.TargetName="button">
                                <DiscreteObjectKeyFrame
                                    KeyTime="0"
                                    Value="{x:Static Visibility.Visible}" />
                            </ObjectAnimationUsingKeyFrames>
                        </Storyboard>
                    </BeginStoryboard>
                </EventTrigger>
                <EventTrigger RoutedEvent="RadioButton.Unchecked">
                    <BeginStoryboard>
                        <Storyboard>
                            <ObjectAnimationUsingKeyFrames
                                Storyboard.TargetProperty="(UIElement.Visibility)"
                                Storyboard.TargetName="button">
                                <DiscreteObjectKeyFrame
                                    KeyTime="0"
                                    Value="{x:Static Visibility.Collapsed}" />
                            </ObjectAnimationUsingKeyFrames>
                        </Storyboard>
                    </BeginStoryboard>
                </EventTrigger>
            </RadioButton.Triggers>
        </RadioButton>
        <Button x:Name="button" Content="Button" HorizontalAlignment="Left" Margin="179,104,0,0" VerticalAlignment="Top" Width="104"/>
```

## 使用VisualStateManager
```cs
<VisualStateManager.VisualStateGroups>
            <VisualStateGroup>
                <VisualState x:Name="r1checked">
                    <Storyboard>
                        <ObjectAnimationUsingKeyFrames
                                Storyboard.TargetProperty="(UIElement.Visibility)"
                                Storyboard.TargetName="button">
                            <DiscreteObjectKeyFrame
                                    KeyTime="0"
                                    Value="{x:Static Visibility.Visible}" />
                        </ObjectAnimationUsingKeyFrames>
                    </Storyboard>
                </VisualState>
                <VisualState x:Name="r1unchecked">
                    <Storyboard>
                        <ObjectAnimationUsingKeyFrames
                                Storyboard.TargetProperty="(UIElement.Visibility)"
                                Storyboard.TargetName="button">
                            <DiscreteObjectKeyFrame
                                    KeyTime="0"
                                    Value="{x:Static Visibility.Collapsed}" />
                        </ObjectAnimationUsingKeyFrames>
                    </Storyboard>
                </VisualState>
            </VisualStateGroup>
        </VisualStateManager.VisualStateGroups>
        <RadioButton x:Name="radioButton1" Content="R2" IsChecked="True"  HorizontalAlignment="Left" Margin="231,66,0,0" VerticalAlignment="Top">
            <i:Interaction.Triggers>
                <i:EventTrigger EventName="Checked">
                    <ei:GoToStateAction  StateName="r1checked"></ei:GoToStateAction>
                </i:EventTrigger>
                <i:EventTrigger EventName="Unchecked">
                    <ei:GoToStateAction  StateName="r1unchecked"></ei:GoToStateAction>
                </i:EventTrigger>
            </i:Interaction.Triggers>
        </RadioButton>
```
