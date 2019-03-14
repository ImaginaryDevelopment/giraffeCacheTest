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

module Geoffrey =
    let nodeCount = 1_000
    let iterCount = 100
    let holes = [0..10] |> List.map(sprintf "%i")
    let generate count =
        div [] [ str "root" ]
        |> Seq.unfold(fun part ->
            let next = div [] [part]
            Some(next, next)
        )
        |> Seq.truncate count
        |> List.ofSeq
        |> List.tail
        |> List.head
    let glue head hole tail =
        // div [] [ yield Generate count; yield str "hole";yield GiraffeMarks.Generate count] |> Giraffe.GiraffeViewEngine.renderHtmlNode
        div [] [ yield head; yield hole; yield tail]
    let generateFull hole = glue (generate nodeCount) (str hole) (generate nodeCount)
open Geoffrey

type GiraffeMarks() =
    [<Benchmark>]
    member __.Uncached () =
        for _ in [1..iterCount] do
            holes
            |> List.map (generateFull >> renderHtmlNode)
            |> ignore<string list>
            ()
        ()
    [<Benchmark>]
    member __.Cached () =
        let cache1,cache2 = (lazy(generate nodeCount), lazy(generate nodeCount))
        for _ in [1..iterCount] do
            holes
            |> List.map(fun hole ->
                glue cache1.Value (str hole) cache2.Value
                |> renderHtmlNode
            )
            |> ignore<string list>


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
    // BenchmarkRunner.Run<SleepMarks>(config) |> ignore
    0 // return an integer exit code
