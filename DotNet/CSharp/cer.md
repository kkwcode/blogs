**PrepareConstrainedRegions**  
JIT编译器发现在try之前调用该方法时，就会提前编译与try关联的catch和finally块中的代码，加载相关程序集，创建相关类型对象，调用相关静态构造器，并对方法进行JIT编译，若有异常，这个异常会在线程进入try之前发生。  
提前调用的方法需要应用**ReliabilityContractAttribute**
```cs
        private static void Demo1()
        {
            Console.WriteLine("In Demo1");
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                Console.WriteLine("In try");
            }
            finally
            {
                // Type1’s static constructor is implicitly called in here
                Type1.M();
            }
        }

        private sealed class Type1
        {
            static Type1()
            {
                // if this throws an exception, M won’t get called
                Console.WriteLine("Type1's static ctor called");
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            public static void M()
            {
            }
        }
```
**ExecuteCodeWithGuaranteedCleanup**  
在资源保证得到清理的前提下才执行代码。  
```cs
public static function ExecuteCodeWithGuaranteedCleanup (
	code : TryCode, 
	backoutCode : CleanupCode, 
	userData : Object
)
```
```cs
            try
            {
                RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(
                    userData => { },
                    (userData, exceptionThrown) =>
                    {
                        //一定会执行
                    }, null);
            }
            catch
            {

            }
```
**CriticalFinalizerObject**
