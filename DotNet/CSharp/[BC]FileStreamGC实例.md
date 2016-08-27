# FileStream GC实例
**使用代码**
```cs
        static void S2()
        {
            Out( "begin");
            var fs1 = new FileStream("s21.txt" , FileMode .OpenOrCreate, FileAccess.ReadWrite);
            var fs2 = new FileStream("s22.txt" , FileMode .OpenOrCreate, FileAccess.ReadWrite);
            Out( "fs1 = null");
            fs1 = null;
            Out( "gc");
            GC.Collect();
            Out( "gc");
            GC.Collect();
        }
```
**程序刚启动时堆中情况**
```
0:000> g
Breakpoint 0 hit
000007fe`8f350602 90              nop
0:000> !dumpheap -type SafeFileHandle
         Address               MT     Size
0000000002714680 000007feed9f0fe8       32      （System.IO.__ConsoleStream）
00000000027180f8 000007feed9f0fe8       32       (fs1)
0000000002718210 000007feed9f0fe8       32      (fs2)

set fs1=null

0:000> !finalizequeue
SyncBlocks to be cleaned up: 0
Free-Threaded Interfaces to be released: 0
MTA Interfaces to be released: 0
STA Interfaces to be released: 0
----------------------------------
generation 0 has 10 finalizable objects (0000000000780930->0000000000780980)
generation 1 has 0 finalizable objects (0000000000780930->0000000000780930)
generation 2 has 0 finalizable objects (0000000000780930->0000000000780930)
Ready for finalization 0 objects (0000000000780980->0000000000780980)
Statistics for all finalizable objects (including all objects ready for finalization):
              MT    Count    TotalSize Class Name
000007feed9f2ec0        1           32 Microsoft.Win32.SafeHandles.SafeFileMappingHandle
000007feed9f2e30        1           32 Microsoft.Win32.SafeHandles.SafeViewOfFileHandle
000007feed9d1028        1           32 Microsoft.Win32.SafeHandles.SafePEFileHandle
000007feed9d0fc0        1           64 System.Threading.ReaderWriterLock
000007feed9f0fe8        3           96 Microsoft.Win32.SafeHandles.SafeFileHandle
000007feed9eee60        1           96 System.Threading.Thread
000007feed9de1e8        2          208 System.IO.FileStream
```
**第一次垃圾回收**
```
0:000> !finalizequeue
SyncBlocks to be cleaned up: 0
Free-Threaded Interfaces to be released: 0
MTA Interfaces to be released: 0
STA Interfaces to be released: 0
----------------------------------
generation 0 has 0 finalizable objects (0000000000780970->0000000000780970)
generation 1 has 8 finalizable objects (0000000000780930->0000000000780970)
generation 2 has 0 finalizable objects (0000000000780930->0000000000780930)
Ready for finalization 2 objects (0000000000780970->0000000000780980)
Statistics for all finalizable objects (including all objects ready for finalization):
              MT    Count    TotalSize Class Name
000007feed9f2ec0        1           32 Microsoft.Win32.SafeHandles.SafeFileMappingHandle
000007feed9f2e30        1           32 Microsoft.Win32.SafeHandles.SafeViewOfFileHandle
000007feed9d1028        1           32 Microsoft.Win32.SafeHandles.SafePEFileHandle
000007feed9d0fc0        1           64 System.Threading.ReaderWriterLock
000007feed9f0fe8        3           96 Microsoft.Win32.SafeHandles.SafeFileHandle
000007feed9eee60        1           96 System.Threading.Thread
000007feed9de1e8        2          208 System.IO.FileStream
```
**查看Finalize队列中的对象**
```
0:000> dd 00000000005f0960
00000000`005f0960  028680f8 00000000 02867ac0 00000000
00000000`005f0970  baadf00d baadf00d baadf00d baadf00d
00000000`005f0980  baadf00d baadf00d baadf00d baadf00d
00000000`005f0990  baadf00d baadf00d baadf00d baadf00d
00000000`005f09a0  baadf00d baadf00d baadf00d baadf00d
00000000`005f09b0  baadf00d baadf00d baadf00d baadf00d
00000000`005f09c0  baadf00d baadf00d baadf00d baadf00d
00000000`005f09d0  baadf00d baadf00d baadf00d baadf00d
0:000> !do 028680f8
Name:        Microsoft.Win32.SafeHandles.SafeFileHandle
MethodTable: 000007feed9f0fe8
EEClass:     000007feed514458
Size:        32(0x20) bytes
File:        C:\windows\Microsoft.Net\assembly\GAC_64\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll
Fields:
              MT    Field   Offset                 Type VT     Attr            Value Name
000007feed9ebee8  4002a09        8        System.IntPtr  1 instance              1f8 handle
000007feed9f03d0  4002a0a       10         System.Int32  1 instance                4 _state
000007feed9dd6f8  4002a0b       14       System.Boolean  1 instance                1 _ownsHandle
000007feed9dd6f8  4002a0c       15       System.Boolean  1 instance                1 _fullyInitialized
0:000> !do 02867ac0
Name:        System.IO.FileStream
MethodTable: 000007feed9de1e8
EEClass:     000007feed3fc1f8
Size:        104(0x68) bytes
File:        C:\windows\Microsoft.Net\assembly\GAC_64\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll
Fields:
              MT    Field   Offset                 Type VT     Attr            Value Name
000007feed9ee068  4000577        8        System.Object  0 instance 0000000000000000 __identity
000007feed9b7a58  400089a       10 ...eam+ReadWriteTask  0 instance 0000000000000000 _activeReadWriteTask
000007feed9def78  400089b       18 ...ing.SemaphoreSlim  0 instance 0000000000000000 _asyncActiveSemaphore
000007feed9f2108  4000898      400     System.IO.Stream  0   shared           static Null
                                 >> Domain:Value  0000000000566d90:0000000002866cd8 <<
000007feed9f27a8  4000817       20        System.Byte[]  0 instance 0000000000000000 _buffer
000007feed9eda88  4000818       28        System.String  0 instance 0000000002867e90 _fileName
000007feed9dd6f8  4000819       58       System.Boolean  1 instance                0 _isAsync
000007feed9dd6f8  400081a       59       System.Boolean  1 instance                1 _canRead
000007feed9dd6f8  400081b       5a       System.Boolean  1 instance                1 _canWrite
000007feed9dd6f8  400081c       5b       System.Boolean  1 instance                1 _canSeek
000007feed9dd6f8  400081d       5c       System.Boolean  1 instance                0 _exposedHandle
000007feed9dd6f8  400081e       5d       System.Boolean  1 instance                0 _isPipe
000007feed9f03d0  400081f       48         System.Int32  1 instance                0 _readPos
000007feed9f03d0  4000820       4c         System.Int32  1 instance                0 _readLen
000007feed9f03d0  4000821       50         System.Int32  1 instance                0 _writePos
000007feed9f03d0  4000822       54         System.Int32  1 instance             4096 _bufferSize
000007feed9f0fe8  4000823       30 ...es.SafeFileHandle  0 instance 00000000028680f8 _handle
000007feed9db5c0  4000824       38         System.Int64  1 instance 0 _pos
000007feed9db5c0  4000825       40         System.Int64  1 instance -1 _appendStart
000007feed9ddc20  4000826      3b0 System.AsyncCallback  0   shared           static s_endReadTask
                                 >> Domain:Value  0000000000566d90:NotInit  <<
000007feed9ddc20  4000827      3b8 System.AsyncCallback  0   shared           static s_endWriteTask
                                 >> Domain:Value  0000000000566d90:NotInit  <<
000007feed990d90  4000828      3c0 ...bject, mscorlib]]  0   shared           static s_cancelReadHandler
                                 >> Domain:Value  0000000000566d90:NotInit  <<
000007feed990d90  4000829      3c8 ...bject, mscorlib]]  0   shared           static s_cancelWriteHandler
                                 >> Domain:Value  0000000000566d90:NotInit  <<
```
**两个FileStream都被提升到1代**
```
0:000> !dumpheap -type FileStream
         Address               MT     Size
0000000002867ac0 000007feed9de1e8      104     
0000000002868118 000007feed9de1e8      104     

Statistics:
              MT    Count    TotalSize Class Name
000007feed9de1e8        2          208 System.IO.FileStream
Total 2 objects
0:000> !eeheap -gc
Number of GC Heaps: 1
generation 0 starts at 0x0000000002868230
generation 1 starts at 0x0000000002861018
generation 2 starts at 0x0000000002861000
ephemeral segment allocation context: none
         segment             begin         allocated              size
0000000002860000  0000000002861000  0000000002868248  0x7248(29256)
Large object heap starts at 0x0000000012861000
         segment             begin         allocated              size
0000000012860000  0000000012861000  0000000012869948  0x8948(35144)
Total Size:              Size: 0xfb90 (64400) bytes.
------------------------------
GC Heap Size:            Size: 0xfb90 (64400) bytes.
```
**第二次垃圾回收**
```
0:000> !dumpheap -type SafeFileHandle
         Address               MT     Size
0000000002714680 000007feed9f0fe8       32     
0000000002718210 000007feed9f0fe8       32     

Statistics:
              MT    Count    TotalSize Class Name
000007feed9f0fe8        2           64 Microsoft.Win32.SafeHandles.SafeFileHandle
```
