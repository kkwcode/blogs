# 定时器总结
## System.Threading的Timer类
要在一个线程池上执行定时的（周期性发生的）后台任务时，它是最好的定时器
`Timer.Change(int dueTime, int period)`
dueTime 多久后开始启动定时器；
period 每次执行间隔  
若dueTime=0, 回调方法立即执行；若`dueTime = Timeout.Infinite`, 回调方法不执行，但可以通过Change更改DueTime以使执行。  
若Period=0 或`Timeout.Infinite`, 且`dueTime！=Timeout.Infinite`，回调方法执行一次，但可以通过Change更改period以使执行。
## System.Windows.Forms的Timer类
构造这个类的一个实例，相当于告诉Windows将一个定时器和调用线程关联（参见Win32_SetTimer）。这个定时器出发时，Windows将一条计时器消息（WM_TIMER）注入线程的消息队列。线程必须将执行消息泵提出这些消息，并把                  它们派遣给想要的回调方法。注意，这里的工作都有一个线程完成---设置计算器的线程保证就是执行回调方法的线程，这还意味着你的计时器不会由多个线层并发执行。
## System.Windows.Threading的DispatcherTimer类
这个是`System.Windows.Forms`的Timer类在在silverlight 和WPF中的等价物。
## System.Timers的Timer类
基本上是`System.Threading`的`Timer`类的包装。这个类应该删除。除非真的想在设计平面中添加一个计时器。
