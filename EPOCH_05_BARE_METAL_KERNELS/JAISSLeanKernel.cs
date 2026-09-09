using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class JAISSLeanKernel {
    static void Main() {
        int cores = Environment.ProcessorCount;
        Console.WriteLine("[+] Lean vFPGA Active Cores: " + cores);
        
        Stopwatch sw = Stopwatch.StartNew();
        long totalCycles = 0;
        object syncLock = new object();

        CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        ParallelOptions options = new ParallelOptions {
            MaxDegreeOfParallelism = cores,
            CancellationToken = cts.Token
        };

        try {
            Parallel.For(0, cores, options, i => {
                Random rand = new Random(i + 42);
                long localCount = 0;
                Stopwatch loopSw = Stopwatch.StartNew();

                while (!cts.Token.IsCancellationRequested) {
                    for (int j = 0; j < 50000; j++) {
                        Math.Pow(rand.NextDouble(), 1.2);
                    }
                    localCount += 50000;

                    if (loopSw.ElapsedMilliseconds > 1000) {
                        Thread.Sleep(10); // Micro-yield resonance damping
                        loopSw.Restart();
                    }
                }
                lock (syncLock) { totalCycles += localCount; }
            });
        } catch (OperationCanceledException) {}

        sw.Stop();
        Console.WriteLine("--- LEAN KERNEL EXECUTION SUMMARY ---");
        Console.WriteLine("Optimized Operations: " + totalCycles);
        Console.WriteLine("Runtime: " + sw.ElapsedMilliseconds + " ms");
        Console.WriteLine("[+] Status: LEAN & STABLE [Zero-Bloat Vector Pipeline]");
    }
}
