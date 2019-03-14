open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Diagnosers
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Jobs
open BenchmarkDotNet.Running
open BenchmarkDotNet.Validators
open BenchmarkDotNet.Exporters
open BenchmarkDotNet.Environments
open System.Reflection
open BenchmarkDotNet.Configs
open Giraffe.GiraffeViewEngine
open BenchmarkDotNet.Toolchains.CsProj
open BenchmarkDotNet.Toolchains.DotNetCli

type SleepMarks () =
    [<Params(0, 1, 15, 100)>]
    member val public sleepTime = 0 with get, set

    // [<GlobalSetup>]
    // member self.GlobalSetup() =
    //     printfn "%s" "Global Setup"

    // [<GlobalCleanup>]
    // member self.GlobalCleanup() =
    //     printfn "%s" "Global Cleanup"

    // [<IterationSetup>]
    // member self.IterationSetup() =
    //     printfn "%s" "Iteration Setup"
    // [<IterationCleanup>]
    // member self.IterationCleanup() =
    //     printfn "%s" "Iteration Cleanup"

    [<Benchmark>]
    member this.Thread () = System.Threading.Thread.Sleep(this.sleepTime)

    [<Benchmark>]
    member this.Task () = System.Threading.Tasks.Task.Delay(this.sleepTime)


    [<Benchmark>]
    member this.AsyncToTask () = Async.Sleep(this.sleepTime) |> Async.StartAsTask
    [<Benchmark>]
    member this.AsyncToSync () = Async.Sleep(this.sleepTime) |> Async.RunSynchronously

type GiraffeMarks() =
    static let count = 1_000_000
    static let fullyCached =
        lazy(
            GiraffeMarks.Generate count
            |> Cacher.cache
        )
    static member Generate count =
        div [] [ Giraffe.GiraffeViewEngine.str "root" ]
        |> Seq.unfold(fun part ->
            let next = div [] [part]
            Some(next, next)
        )
        |> Seq.truncate count
        |> List.ofSeq
        |> List.tail
        |> List.head
    [<Benchmark>]
    member __.Uncached () =
        let _ = GiraffeMarks.Generate count |> Giraffe.GiraffeViewEngine.renderHtmlNode
        ()
    [<Benchmark>]
    member this.Cached () =
        fullyCached.Value
        |> Giraffe.GiraffeViewEngine.renderHtmlNode
        |> ignore<string>






let config =
     ManualConfig
            .Create(DefaultConfig.Instance)
            // .With(Job.ShortRun.With(Runtime.Core))
            .With(Job.MediumRun
                .With(Runtime.Core)
                .With(CsProjCoreToolchain.From(NetCoreAppSettings.NetCoreApp21))
                .WithId("NetCoreApp21")
            )
            .With(MemoryDiagnoser.Default)
            .With(MarkdownExporter.GitHub)
            .With(ExecutionValidator.FailOnError)

let defaultSwitch () =
    Assembly.GetExecutingAssembly().GetTypes() |> Array.filter (fun t ->
        t.GetMethods ()|> Array.exists (fun m ->
            m.GetCustomAttributes (typeof<BenchmarkAttribute>, false) <> [||] ))
    |> BenchmarkSwitcher


[<EntryPoint>]
let main argv =
    // GiraffeMarks().Uncached()
    defaultSwitch().Run(argv, config) |>ignore
    BenchmarkRunner.Run<SleepMarks>(config) |> ignore
    0 // return an integer exit code
