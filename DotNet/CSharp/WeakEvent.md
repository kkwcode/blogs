[MSDN示例](https://msdn.microsoft.com/en-us/library/aa970850.aspx)
#### Why
传统的事件监听语法如：`source.SomeEvent += new SomeEventHandler(MyEventHandler)`, 这使source建立了监听对象的强引用，即Listener的生命周期变得于source一样长，这可能会引起内存泄漏。**弱事件模型即为解决这个问题。**
#### 弱事件模型的实现方式
- 使用现有的`WeakEventManager`实现类
```cs
System.Object
  System.Windows.Threading.DispatcherObject
    System.Windows.WeakEventManager
      System.Collections.Specialized.CollectionChangedEventManager
      System.ComponentModel.CurrentChangedEventManager
      System.ComponentModel.CurrentChangingEventManager
      System.ComponentModel.ErrorsChangedEventManager
      System.ComponentModel.PropertyChangedEventManager
      System.Windows.Data.DataChangedEventManager
      System.Windows.Input.CanExecuteChangedEventManager
      System.Windows.LostFocusEventManager
      System.Windows.WeakEventManager<TEventSource, TEventArgs>
```
- 使用`WeakEventManager<TEventSource,TEventArgs>`
- 自定义`WeakEventManager`

@[示例代码](https://github.com/DullSpy/blogs/blob/master/DotNet/CSharp/CodeSnippet/WeakEventManager.cs)
