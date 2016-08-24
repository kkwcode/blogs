# 依赖属性
#### 引用
> http://www.snippetsource.net/Snippet/20/define-a-custom-dependencyproperty

#### 获取对象附加属性
```cs
public IEnumerable<DependencyProperty> GetAttachedProperties(DependencyObject element)
{
    var markupObject = MarkupWriter.GetMarkupObjectFor(element);
    foreach (MarkupProperty mp in markupObject.Properties)
    {
        if (mp.IsAttached && mp.DependencyProperty != null)
        {
            var dpd = DependencyPropertyDescriptor.FromProperty(mp.DependencyProperty, element.GetType());
            if (dpd != null)
            {
                yield return dpd.DependencyProperty;
            }
        }
    }
}
```
#### 监听依赖属性更改
```cs
var descriptor = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
 
if (descriptor != null)
{
    descriptor.AddValueChanged(myTextBox, delegate
    {
        // Add your propery changed logic here...    });
} 
```
#### 定义附加属性
```cs
public static readonly DependencyProperty TopProperty =
    DependencyProperty.RegisterAttached("Top", 
    typeof(double), typeof(Canvas),
    new FrameworkPropertyMetadata(0d,
        FrameworkPropertyMetadataOptions.Inherits));
 
public static void SetTop(UIElement element, double value)
{
    element.SetValue(TopProperty, value);
}
 
public static double GetTop(UIElement element)
{
    return (double)element.GetValue(TopProperty);
}
```
#### 定义依赖属性
```cs
// Dependency Property Declaration
public static readonly DependencyProperty BackgroundProperty =
    DependencyProperty.Register("Background", typeof(Brush), typeof(MyControl),
    new FrameworkPropertyMetadata(Brushes.Transparent));

// Property Wrapperpublic Brush Background
{
    get { return (Brush)GetValue(BackgroundProperty); }
    set { SetValue(BackgroundProperty, value); }
}
```
#### 只读依赖属性
```cs
// Register the private key to set the valueprivate static readonly DependencyPropertyKey IsMouseOverPropertyKey = 
      DependencyProperty.RegisterReadOnly("IsMouseOver", 
      typeof(bool), typeof(MyClass), 
      new FrameworkPropertyMetadata(false));
 
// Register the public property to get the valuepublic static readonly DependencyProperty IsMouseoverProperty = 
      IsMouseOverPropertyKey.DependencyProperty;    
 
// .NET Property wrapperpublic int IsMouseOver
{
   get { return (bool)GetValue(IsMouseoverProperty); }
   private set { SetValue(IsMouseOverPropertyKey, value); }
}
```


 
