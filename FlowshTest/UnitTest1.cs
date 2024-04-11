using CXSoft.Flowsh;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading;

namespace FlowshTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FlowshFactory.CreateFlowsh().
                AddHandler(LogicDelegateInfo.CreateInstance().Register(ActionC.Test0)).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int>(ActionC.Test1, "t1:1")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int>(ActionC.Test2, "t2:2")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int>(ActionC.Test3, "t3:3")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int>(ActionC.Test4, "t4:4")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int>(ActionC.Test5, "t5:5")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int>(ActionC.Test6, "t6:6")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int, int>(ActionC.Test7, "t7:7")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int, int, int>(ActionC.Test8, "t8:8")).
                Execute();
        }

        [TestMethod]
        public void TestMethod2()
        {
            var res = FlowshFactory.CreateFlowsh().
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int>(FuncC.Test0, "r1")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int>(FuncC.Test1, "t1:1", "r2")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int>(FuncC.Test2, "t2:2", "r3")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int>(FuncC.Test3, "t3:3", "r4")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int>(FuncC.Test4, "t4:4", "r5")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int>(FuncC.Test5, "t5:5", "r6")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int, int>(FuncC.Test6, "t6:6", "r7")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int, int, int>(FuncC.Test7, "t7:7", "r8")).
                AddHandler(LogicDelegateInfo.CreateInstance().Register<int, int, int, int, int, int, int, int, int>(FuncC.Test8, "", "r9").
                AddParamConfigs("--t8--6--Test6--r7--out--")).
                Execute();
        }

        [TestMethod]
        public void TestMethod3()
        {
            ActionC.asyncDelegateInfo = (AsyncDelegateInfo)AsyncDelegateInfo.CreateInstance().Register<int>(ActionC.TestAsync, "t1:1");

            FlowshFactory.CreateFlowsh().
                AddHandler(ActionC.asyncDelegateInfo).
                Execute();
        }
    }

    public class ActionC
    {
        public static AsyncDelegateInfo asyncDelegateInfo;

        public static void Test0()
        {
            Console.WriteLine("无参方法0");
        }

        public static void Test1(int t1)
        {
            Console.WriteLine("无参方法1:" + t1);
        }

        public static void Test2(int t1, int t2)
        {
            Console.WriteLine("无参方法2:" + t1 + " " + t2);
        }

        public static void Test3(int t1, int t2, int t3)
        {
            Console.WriteLine("无参方法3:" + t1 + " " + t2 + " " + t3);
        }

        public static void Test4(int t1, int t2, int t3, int t4)
        {
            Console.WriteLine("无参方法4:" + t1 + " " + t2 + " " + t3 + " " + t4);
        }

        public static void Test5(int t1, int t2, int t3, int t4, int t5)
        {
            Console.WriteLine("无参方法5:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5);
        }

        public static void Test6(int t1, int t2, int t3, int t4, int t5, int t6)
        {
            Console.WriteLine("无参方法6:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6);
        }

        public static void Test7(int t1, int t2, int t3, int t4, int t5, int t6, int t7)
        {
            Console.WriteLine("无参方法7:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6 + " " + t7);
        }

        public static void Test8(int t1, int t2, int t3, int t4, int t5, int t6, int t7, int t8)
        {
            Console.WriteLine("无参方法8:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6 + " " + t7 + " " + t8);
        }

        public static void TestAsync(int t1)
        {
            Console.WriteLine("无参异步方法0_"+ t1);
            (new Action(() => { TestCallBack(t1); })).BeginInvoke(null,null);
        }

        public static void TestCallBack(int t1)
        {
            Console.WriteLine("无参异步方法1_" + t1);
            ActionC.asyncDelegateInfo?.DoMethodComplate("cc:" + t1);
        }
    }

    public class FuncC
    {
        public static int Test0()
        {
            Console.WriteLine("有参方法0");
            return 1000;
        }

        public static int Test1(int t1)
        {
            Console.WriteLine("有参方法1:" + t1);
            return t1 * 1000;
        }

        public static int Test2(int r1, int t2)
        {
            Console.WriteLine("有参方法2:" + r1 + " " + t2);
            return t2 * 1000;
        }

        public static int Test3(int t1, int t2, int t3)
        {
            Console.WriteLine("有参方法3:" + t1 + " " + t2 + " " + t3);
            return t3 * 1000;
        }

        public static int Test4(int t1, int t2, int t3, int t4)
        {
            Console.WriteLine("有参方法4:" + t1 + " " + t2 + " " + t3 + " " + t4);
            return t4 * 1000;
        }

        public static int Test5(int t1, int t2, int t3, int t4, int t5)
        {
            Console.WriteLine("有参方法5:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5);
            return t5 * 1000;
        }

        public static int Test6(int t1, int t2, int t3, int t4, int t5, int t6)
        {
            Console.WriteLine("有参方法6:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6);
            return t6 * 1000;
        }

        public static int Test7(int t1, int t2, int t3, int t4, int t5, int t6, int t7)
        {
            Console.WriteLine("有参方法7:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6 + " " + t7);
            return t7 * 1000;
        }

        public static int Test8(int t1, int t2, int t3, int t4, int t5, int t6, int t7, int t8)
        {
            Console.WriteLine("有参方法8:" + t1 + " " + t2 + " " + t3 + " " + t4 + " " + t5 + " " + t6 + " " + t7 + " " + t8);
            return t8 * 1000;
        }

        public static int JudgeFunc(int t)
        {
            if (t == 1) return t * 10000;
            if (t == 2) return t * 10000;
            if (t == 3) return t * 10000;
            if (t == 4) return t * 10000;
            if (t == 5) return t * 10000;
            if (t == 6) return t * 10000;
            if (t == 7) return t * 10000;
            if (t == 8) return t * 10000;
            return 10000;
        }
    }
}
