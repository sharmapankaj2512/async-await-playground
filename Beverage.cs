using BenchmarkDotNet.Attributes;

namespace PlayGround;

[HtmlExporter]
public class Beverage
{
    private static readonly TimeSpan TimeSpan = TimeSpan.FromMilliseconds(10);

    async Task BoilMilk()
    {
        await DelayedAction(() => { Print("milk boiled"); });
    }

    async Task PourMilk()
    {
        await DelayedAction(() => { Print("milk poured"); });
    }

    async Task AddSugar()
    {
        await DelayedAction(() => { Print("sugar added"); });
    }

    private static async Task DelayedAction(Action action)
    {
        await Task
            .Run(async () => await Task.Delay(TimeSpan))
            .ContinueWith(_ => action());
    }

    private static void Print(string actionDone)
    {
        Console.WriteLine($"{Environment.CurrentManagedThreadId} - {actionDone}");
    }

    [Benchmark]
    public void PrepareBeverageSync()
    {
        Thread.Sleep(TimeSpan);
        Thread.Sleep(TimeSpan);
        Thread.Sleep(TimeSpan);
    }

    [Benchmark]
    public async Task PrepareBeverageAsync()
    {
        Console.WriteLine($"{Environment.CurrentManagedThreadId} - preparing something");
        await PourMilk(); // assign a thread to Continuation and let go the current thread, don't block it
        await BoilMilk(); // assign a thread to BoilMilk and let go the current thread, don't block it
        await AddSugar(); // assign a thread to Continuation and let go the current thread, don't block it
    }

    [Benchmark]
    public async Task PrepareBeverageAsyncWhenAll()
    {
        Console.WriteLine($"{Environment.CurrentManagedThreadId} - preparing something in any order");
        await Task.WhenAll(
            PourMilk(), // assign new thread to BoilMilk
            BoilMilk(), // assign new thread to BoilMilk
            AddSugar()); // assign new thread to BoilMilk
    }
}