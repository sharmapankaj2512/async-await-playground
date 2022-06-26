using BenchmarkDotNet.Running;
using PlayGround;

// COMMAND: dotnet run -p PlayGround.csproj -c Release

// await PrepareBeverage();
// await PrepareBeverageInAnyOrder();

BenchmarkRunner.Run<Beverage>();