/*
using BenchmarkDotNet.Running;
using Benchmarks6;

BenchmarkRunner.Run<Benchmarks>(args: args);
*/
using System.Runtime.CompilerServices;

void Test(object p, [CallerArgumentExpression("p")]string e = "")
{
    Console.WriteLine(e);
}

void Test2([CallerArgumentExpression("p")] string e = "", params object[] p)
{
    Console.WriteLine(e);
}

Test(new TestClass(1, 2, 3));

class TestClass 
{
    public string Field1;
    public int Field2;

    public TestClass(params object[] p)
    {

    }
}



