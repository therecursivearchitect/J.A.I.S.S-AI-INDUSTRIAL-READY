using System;
using System.Diagnostics;

class JAISSKernel {
    static void Main() {
        int iterations = 10000000; // 10 million raw operations
        Stopwatch sw = Stopwatch.StartNew();
        
        double accumulator = 0;
        for (int i = 1; i <= iterations; i++) {
            accumulator += Math.Pow(i, 0.5) * Math.Sin(i);
        }
        
        sw.Stop();
        double opsPerSec = iterations / (sw.ElapsedMilliseconds / 1000.0);
        
        Console.WriteLine("--- RAW BINARY SANDBOX METRICS ---");
        Console.WriteLine("Operations: " + iterations);
        Console.WriteLine("Time (ms): " + sw.ElapsedMilliseconds);
        Console.WriteLine("Throughput: " + Math.Round(opsPerSec, 2) + " ops/sec");
    }
}
