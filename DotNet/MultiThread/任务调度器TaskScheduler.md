## 任务调度器 TaskScheduler
  - 线程池调度器 TaskScheduler.Default
  - 同步上下文调度器 TaskScheduler.FromCurrentSynchronizationContext()，这样的任务将会在UI线程中执行，即可以访问UI资源。

## 多个任务调度器的实例 **http://code.msdn.microsoft.com/ParExtSamples**
  - IOTaskScheduler
    排队给IO线程
  - LimitedConcurrencyLevelTaskScheduler
    不允许超过n个任务同时执行
  - OrderedTaskScheduler
    只允许一个任务执行
  - PrioritizingTaskScheduler
    可以指定添加任务的优先级
  - ThreadPerTaskScheduler
    每个任务创建一个单独的线程进行处理，不使用线程池
