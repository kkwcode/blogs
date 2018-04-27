# encoding: utf-8

import multiprocessing as mp
import time

# 循环向共享内存中写入数字，从1开始
class Reader:
    def __init__(self, sa: mp.Array, rse: mp.Semaphore, wse: mp.Semaphore):
        self.sa = sa
        self.totalcount = len(sa)
        self.rindex = 0
        self.rse = rse
        self.wse = wse

    def prun(self):
        while True:
            self.rse.acquire()
            time.sleep(1)
            print('reader num is: '+str(self.sa[self.rindex]))
            self.rindex = (self.rindex + 1) % self.totalcount
            self.wse.release()  # 处理完一个数字后,通知写进程,可以继续写数字
    def run(self):
        p = mp.Process(target=self.prun)
        p.start()

# 循环的读出共享内存中的数字，并输出
class Writer:
    def __init__(self, sa: mp.Array, rse: mp.Semaphore, wse: mp.Semaphore):
        self.sa = sa
        self.totalcount = len(sa)
        self.windex = 0
        self.num = 1
        self.rse = rse
        self.wse = wse

    def prun(self):
        while True:
            self.wse.acquire()
            time.sleep(0.1)
            print('writer num : ' + str(self.num))
            self.sa[self.windex] = self.num
            self.windex = (self.windex + 1) % self.totalcount
            self.num += 1
            self.rse.release()  #写完一个数字后,通知读进程,可以多读一个数字

    def run(self):
        p = mp.Process(target=self.prun)
        p.start()

if __name__ == '__main__':
    sa = mp.Array('i', 10)
    wse = mp.Semaphore(10) # 写共享内存的信号量
    rse = mp.Semaphore(0)   # 读共享内存的信号量

    writer = Writer(sa, rse, wse)
    writer.run()

    reader = Reader(sa, rse, wse)
    reader.run()
