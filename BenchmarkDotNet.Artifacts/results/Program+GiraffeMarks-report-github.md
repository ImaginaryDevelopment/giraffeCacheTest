``` ini

BenchmarkDotNet=v0.10.12, OS=macOS 10.14.3 (18D109) [Darwin 18.2.0]
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical cores and 6 physical cores
.NET Core SDK=2.1.403
  [Host]       : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT DEBUG
  NetCoreApp21 : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT

Job=NetCoreApp21  Runtime=Core  Toolchain=.NET Core 2.1  
LaunchCount=2  TargetCount=15  WarmupCount=10  

```
|   Method |               Mean |             Error |            StdDev |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|--------- |-------------------:|------------------:|------------------:|-----------:|-----------:|----------:|------------:|
| Uncached | 1,051,337,948.2 ns | 13,321,188.054 ns | 19,526,024.728 ns | 40062.5000 | 14750.0000 | 2250.0000 | 208001024 B |
|   Cached |           104.6 ns |          1.575 ns |          2.308 ns |     0.0695 |          - |         - |       328 B |
