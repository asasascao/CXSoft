# CXSoft
流程化编程框架

针对情况：
    1.针对一些小公司开发过程中不写文档注释人员又流动大的情况下,代码逻辑写的比较乱
    2.部分框架写法升级导致一个程序当中出现几种甚至几十种写法

定义：
    按照流程图的方式对处理逻辑进行调用集中管理处理，按照 顺序分支循环三种结构处理对应的逻辑

使用方式：
	Flowsh=FlowshFactory.CreateFlowsh() //创建流程线
	Flowsh.AddHandler(handler);//增加流程步骤
	Flowsh.AddLevelHandlers(n,handler,handler,handler);//增加同层级流程 同层级流程并行
	Flowsh.Execute();//执行流程
	目前流程步骤实现了逻辑步骤LogicHandler和判断步骤JudgeHandler
	
	异步流程目前暂未实现
