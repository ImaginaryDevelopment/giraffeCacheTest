``` ini

BenchmarkDotNet=v0.10.12, OS=macOS 10.14.3 (18D109) [Darwin 18.2.0]
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical cores and 6 physical cores
.NET Core SDK=2.1.403
  [Host]       : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT DEBUG
  NetCoreApp21 : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT

Job=NetCoreApp21  Runtime=Core  Toolchain=.NET Core 2.1  
LaunchCount=2  TargetCount=15  WarmupCount=10  

```
|   Method |              Mean |              Error |            StdDev |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|--------- |------------------:|-------------------:|------------------:|-----------:|-----------:|----------:|------------:|
| Uncached | 902,190,566.86 ns | 17,583,332.5943 ns | 26,317,904.310 ns | 39875.0000 | 14625.0000 | 2125.0000 | 208000932 B |
|   Cached |          90.30 ns |          0.7649 ns |          1.145 ns |     0.0695 |          - |         - |       328 B |
