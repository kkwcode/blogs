## 一个Condition的使用示例
### Source
```python
import threading, time
import random

def consumer(con : threading.Condition, array : list):
    while True:
        with con:
            if len(array) <1:
                con.wait()
            else:
                print('consume one: '+ str(array.pop()))
                con.notify()


def producer(con : threading.Condition, array : list):
    while True:
        with con:
            if len(array) >10:
                con.wait()
            else:
                time.sleep(0.5) # 模拟产生一个数字需要0.5秒
                newint = random.randint(1, 100)
                array.append(newint)
                print('produce one:' + str(newint))
                con.notify()
        time.sleep(0.001)   #该句是需要的，否则，producer线程不会挂起，致使producer不断生产，而consumer线程得不到执行

if __name__ == '__main__':
    array = []
    cond = threading.Condition()
    p = threading.Thread(target=producer, args=(cond, array))
    c = threading.Thread(target=consumer, args=(cond, array))
    p.start()
    c.start()
```
### Output
produce one:58  
consume one: 58  
produce one:26  
consume one: 26  
produce one:50  
consume one: 50  
produce one:64  
consume one: 64  
produce one:86  
consume one: 86  
produce one:74  
consume one: 74  

### Question
1. 上述代码中有一句time.sleep(0.001), 不加这句时输出如下：
produce one:42  
produce one:63  
produce one:19  
produce one:25  
produce one:72  
produce one:35  
produce one:2  
produce one:8  
produce one:71  
produce one:54  
produce one:32  
consume one: 32  
consume one: 54  
consume one: 71  
consume one: 8  
consume one: 2  
consume one: 35  
consume one: 72  
consume one: 25  
consume one: 19  
consume one: 63  
可能与GIL有关，python中的多线程不是真正的多线程，consumer线程已经被挂起，也就是处于阻塞状态，当producer线程添加sleep后，consumer切换到就绪状态，得到cpu时间片后执行。

