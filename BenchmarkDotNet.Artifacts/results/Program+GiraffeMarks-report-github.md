``` ini

BenchmarkDotNet=v0.10.12, OS=macOS 10.14.3 (18D109) [Darwin 18.2.0]
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical cores and 6 physical cores
.NET Core SDK=2.1.403
  [Host]       : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT DEBUG
  NetCoreApp21 : .NET Core 2.1.5 (Framework 4.6.26919.02), 64bit RyuJIT

Job=NetCoreApp21  Runtime=Core  Toolchain=.NET Core 2.1  
LaunchCount=2  TargetCount=15  WarmupCount=10  

```
|   Method | IterCount |        Mean |        Error |       StdDev |      Gen 0 |   Gen 1 |    Allocated |
|--------- |---------- |------------:|-------------:|-------------:|-----------:|--------:|-------------:|
| **Uncached** |         **1** |  **5,227.5 us** |    **47.612 us** |    **68.283 us** |  **1945.3125** |  **7.8125** |   **8981.86 KB** |
|   Cached |         1 |    261.3 us |     3.037 us |     4.451 us |    93.2617 |  1.9531 |    431.95 KB |
| **Uncached** |        **15** | **42,873.4 us** |   **868.036 us** | **1,299.236 us** | **15562.5000** | **62.5000** |  **71854.33 KB** |
|   Cached |        15 |    368.9 us |     8.068 us |    11.826 us |   130.3711 |       - |    602.47 KB |
| **Uncached** |        **25** | **68,075.9 us** | **1,590.125 us** | **2,229.133 us** | **25312.5000** | **62.5000** | **116763.23 KB** |
|   Cached |        25 |    427.1 us |     5.943 us |     8.711 us |   156.7383 |  0.4883 |    724.27 KB |
