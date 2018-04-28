# 用来计算大文件的md5值

import hashlib
import os
import datetime

def GetFileMd5(filename):
    if not os.path.isfile(filename):
        return
    hl = hashlib.md5()

    with open(filename, 'rb') as f:
        while True:
            b = f.read(8096)
            if not b:
                break
                hl.update(b)
    return hl.hexdigest()
