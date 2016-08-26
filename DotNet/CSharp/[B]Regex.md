# 正则表达式
### 语法表
|元字符|说明|
|:-:|:-:|
|^|匹配开始|
|$|匹配结束|
|?|在限制符(*,+,?，{n}，{n,}，{n,m})后时表示非贪婪匹配，即最少匹配|
|\b|匹配单词边界，如'er\b'可以匹配nerver,但不能匹配verb|
|\s|不可见字符如[\f\n\r\t\v]|
|\S|与\s正好相反|
|\w|与[A-Za-z0-9_]类似|
|\w|与\w相反|
|(pattern)|匹配pattern并获取这一匹配，在C#通过Groups访问匹配值|
|(?:pattern)|非获取匹配，匹配pattern但不获取匹配结果|
|(?=pattern)|匹配以pattern结束，不获取pattern|
|(?!pattern)|匹配不以pattern结束|
|(?<=pattern)|匹配以pattern开始，不获取pattern|
|(?<!pattern)|匹配不以pattern开始|
### 平衡组，递归匹配
匹配嵌套字符串，如匹配类中的大括号，HTML中的尖括号等。
- (?'group'exp)
- (?'-group'exp)
- (?(group)yes|no) 如果堆栈上存在以名为group的捕获内容的话，继续匹配yes部分的表达式，否则继续匹配no部分
- (?!) 零宽负向先行断言，由于没有后缀表达式，试图匹配总是失败

**示例：**  
```cs
string regexMethodContentStr = @"{[^{}]*((?<open>{)|(?<-open>})|[^{}])*(?(open)(?!))[^{}]*}" ;
//匹配实例函数
string regexMethodStr = string.Concat(@"public\s+\S+\s+\S+\([\s\S]*?\)" , @"\s*" , regexMethodContentStr);
//匹配静态函数
string regexStaticMethodStr = string.Concat(@"public\s+static\s+\S+\s+\S+\([\s\S]*?\)" , @"\s*" , regexMethodContentStr);
```
