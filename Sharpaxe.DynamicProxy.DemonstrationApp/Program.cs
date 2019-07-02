using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Sharpaxe.DynamicProxy.DemonstrationApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MainCore(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(Main)} exception has occured:{Environment.NewLine}{ex}");
            }

            Console.ReadKey();
        }

        private static void MainCore(string[] args)
        {
            BeforeDecoratorDemonstration();
            AfterDecoratorDemostration();
            PairDecoratorsDemonstration();
            PairDecoratorsDemonstrationOnException();

            ProxyDemonstrationNullGuard();
            ProxyDemonstrationArgumentLimiter();
        }

        private static void BeforeDecoratorDemonstration()
        {
            Console.WriteLine(nameof(BeforeDecoratorDemonstration));

            var proxyBuilder = new ReflectionProxyBuilder<IDbRepository>();
            proxyBuilder.AddBeforeActionDecorator(dbr => dbr.CommitChanges,
                () =>
                {
                    Console.WriteLine("Open connection");
                });

            var decoratedInstance = proxyBuilder.Build(new DbRepository());

            decoratedInstance.CommitChanges();

            Console.WriteLine();
        }

        private static void AfterDecoratorDemostration()
        {
            Console.WriteLine(nameof(AfterDecoratorDemostration));

            var proxyBuilder = new ReflectionProxyBuilder<IDbRepository>();
            proxyBuilder.AddAfterActionDecorator(dbr => dbr.CommitChanges,
                () =>
                {
                    Console.WriteLine("Clear connection");
                });

            var decoratedInstance = proxyBuilder.Build(new DbRepository());

            decoratedInstance.CommitChanges();

            Console.WriteLine();
        }

        private static void PairDecoratorsDemonstration()
        {
            Console.WriteLine(nameof(PairDecoratorsDemonstration));

            var stopwatch = new Stopwatch();
            var proxyBuilder = new ReflectionProxyBuilder<IDbRepository>();
            proxyBuilder.AddPairActionDecorators(dbr => dbr.CommitChanges,
                () =>
                {
                    stopwatch.Start();
                },
                () =>
                {
                    stopwatch.Stop();
                    Console.WriteLine($"Commit changes took '{stopwatch.ElapsedMilliseconds}' ms");
                });

            var decoratedInstance = proxyBuilder.Build(new DbRepositoryWithSleep());

            decoratedInstance.CommitChanges();

            Console.WriteLine();
        }

        private static void PairDecoratorsDemonstrationOnException()
        {
            Console.WriteLine(nameof(PairDecoratorsDemonstrationOnException));

            var synchronization = new object();
            var proxyBuilder = new ReflectionProxyBuilder<IDbRepository>();
            proxyBuilder.AddPairActionDecorators(dbr => dbr.CommitChanges,
                () =>
                {
                    Monitor.Enter(synchronization);
                    Console.WriteLine("Locked");
                },
                () =>
                {
                    Monitor.Exit(synchronization);
                    Console.WriteLine("Unlocked");
                });

            var decoratedInstance = proxyBuilder.Build(new DbRepositoryThrowingException());

            try
            {
                decoratedInstance.CommitChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Catched exception: {ex.Message}");
            }

            Console.WriteLine();
        }

        private static void ProxyDemonstrationNullGuard()
        {
            Console.WriteLine(nameof(ProxyDemonstrationNullGuard));

            var proxyBuilder = new ReflectionProxyBuilder<IConnection>();
            proxyBuilder.SetActionProxy<byte[]>(c => c.SendData,
                (f, d) =>
                {
                    if (d != null)
                    {
                        f.Invoke(d);
                    }
                });

            var proxiedInstance = proxyBuilder.Build(new Connection());

            proxiedInstance.SendData(null);
            proxiedInstance.SendData(new byte[10]);
            proxiedInstance.SendData(null);
            proxiedInstance.SendData(new byte[20]);

            Console.WriteLine();
        }

        private static void ProxyDemonstrationArgumentLimiter()
        {
            Console.WriteLine(nameof(ProxyDemonstrationArgumentLimiter));

            var proxyBuilder = new ReflectionProxyBuilder<IConnection>();
            proxyBuilder.SetActionProxy<byte[]>(c => c.SendData,
                (c, d) =>
                {
                    for (int i = 0; i < (d.Length / 10); i++)
                    {
                        c.Invoke(d.Skip(i * 10).Take(10).ToArray());
                    }
                    c.Invoke(d.Skip((d.Length / 10) * 10).Take(d.Length - (d.Length / 10) * 10).ToArray());
                });

            var proxiedInstance = proxyBuilder.Build(new Connection());

            proxiedInstance.SendData(new byte[9]);
            proxiedInstance.SendData(new byte[12]);
            proxiedInstance.SendData(new byte[15]);
            proxiedInstance.SendData(new byte[38]);

            Console.WriteLine();
        }
    }

    #region DbRepository

    public interface IDbRepository
    {
        void CommitChanges();
    }

    class DbRepository : IDbRepository
    {
        public void CommitChanges()
        {
            Console.WriteLine("Commit changes...");
        }
    }

    class DbRepositoryWithSleep : IDbRepository
    {
        public void CommitChanges()
        {
            Console.WriteLine("Commit changes...");
            Thread.Sleep(1000);
        }
    }

    class DbRepositoryThrowingException : IDbRepository
    {
        public void CommitChanges()
        {
            Console.WriteLine("Throwing exception...");
            throw new Exception("Demonstration DdRepository exception");
        }
    }

    #endregion DbRepository

    #region 

    public interface IConnection
    {
        void SendData(byte[] data);
    }

    class Connection : IConnection
    {
        public void SendData(byte[] data)
        {
            Console.WriteLine($"Sending {data.Length} bytes of data");
        }
    }

    #endregion
}
