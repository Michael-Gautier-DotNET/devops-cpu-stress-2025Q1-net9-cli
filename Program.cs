/*

This code aims to provide a down-to-earth snapshot of how many fundamental operations per second can be achieved by .NET code under typical desktop conditions. This process intentionally allow normal OS scheduling, I/O logging, and mild pauses to reflect the unpredictability real users experience every day. By running these informal one-second loops across multiple cycles, we capture a range of iteration counts that serve as a practical reference for desktop-grade capacity planning and intuitive performance instincts—without claiming the rigorous precision of specialized microbenchmarks.

*/
string LogDetailFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CycleLogDetail");
string IterationLogFullPath = Path.Combine(Directory.GetCurrentDirectory(), "CycleLog");

Directory.CreateDirectory(LogDetailFilePath);
Directory.CreateDirectory(IterationLogFullPath);

if( false == Directory.Exists (LogDetailFilePath) || false == Directory.Exists (IterationLogFullPath) ) {
	Console.WriteLine ("Directories do not exist");
	Console.WriteLine ("Missing:");

	if( false == Directory.Exists (LogDetailFilePath) ) {
		Console.WriteLine (LogDetailFilePath);
	}
	
	if( false == Directory.Exists (IterationLogFullPath) ) {
		Console.WriteLine (IterationLogFullPath);
	}
	
	return;
}

IterationLogFullPath = Path.Combine(IterationLogFullPath, "Iteration.txt");

Console.WriteLine(new string('*', 50));
Console.WriteLine("Gautier Iteration Test");
Console.WriteLine("Provides an informal assessment of operations per second on a given system");
Console.WriteLine("Essentially how fast can C# code execute today");
Console.WriteLine("Helps in building better estimates for capacity planning and design");
Console.WriteLine(new string('*', 50));
Console.WriteLine("How many times you want the test to run?");
Console.Write("Type number then <enter>:  ");

string? CyclesText = Console.ReadLine();

int Cycles = 1;
int Cycle = 0;

int.TryParse(CyclesText, out Cycles);

Console.WriteLine($"Running {Cycles} test runs");

File.AppendAllText(IterationLogFullPath, $"{new string('*', 33)}\n");
File.AppendAllText(IterationLogFullPath, $"Cycles: {Cycles}\t{DateTime.Now}\n");
File.AppendAllText(IterationLogFullPath, $"{new string('*', 28)}\n");

int SumOfIterations = 0;

DateTime CycleStart = DateTime.Now;

while(Cycle < Cycles) {
	Cycle++;
	
	Console.WriteLine(new string('*', 76));
	Console.WriteLine($"Running Cycle {Cycle:D2} of {Cycles:D2}");
	Console.WriteLine(new string('*', 44));

	string FileName = DateTime.Now.ToString("yyyyMMdd_hh_mm_ss");
	string FullPath = Path.Combine(LogDetailFilePath, $"T {FileName} {Cycles:D2} - {Cycle:D2}.txt");

	System.Text.StringBuilder Buffer = new();

	﻿int Iterations = 0;

	int PauseMs = DateTime.Now.Second * 100;

	while (DateTime.Now.Second % 8 == 0){
		Console.WriteLine($"Pausing for {PauseMs}ms");
		Thread.Sleep (PauseMs);
		
		Buffer.AppendLine($"Paused for {PauseMs}ms");
		
		PauseMs = DateTime.Now.Second * 100;
	}

	Console.WriteLine($"Ready to go ... {DateTime.Now}");

	DateTime Start = DateTime.Now;
	DateTime Now = DateTime.Now;

	for(int i = 0; Now.Second == Start.Second; i++) {
		Iterations = 1 + i;

		if(i % 100_000 == 0){
			Now = DateTime.Now;
			Console.WriteLine ($"Cycle {Cycle} of {Cycles} Iteration {i:N0} {Now}");
			Buffer.AppendLine ($"Iteration {i:N0} {Now}");
		}
	}

	SumOfIterations = SumOfIterations + Iterations;

	DateTime End = DateTime.Now;

	Console.WriteLine ($"Iterations {Iterations:N0} Start {Start} ... End {End}");
	Buffer.AppendLine ($"Iterations {Iterations:N0} Start {Start} ... End {End}");

	Console.WriteLine(FullPath);
	File.WriteAllText(FullPath, Buffer.ToString());
	File.AppendAllText(IterationLogFullPath, $"***\t{Cycle:N0}\t");
	File.AppendAllText(IterationLogFullPath, $"{new string('*', 60)}\n");
	File.AppendAllText(IterationLogFullPath, $"{Start}\n");
	File.AppendAllText(IterationLogFullPath, $"{Iterations:N0}\n");
	File.AppendAllText(IterationLogFullPath, $"{End}\n");
	File.AppendAllText(IterationLogFullPath, "\n\n");

}

DateTime CycleEnd = DateTime.Now;

TimeSpan TimeDifference = CycleEnd - CycleStart;

Console.WriteLine($"******\tSum: {SumOfIterations:N0} operations across {Cycles} cycles *********\n");
Console.WriteLine($"Average: {(SumOfIterations/Cycles):N0} operations per second **********\n");
Console.WriteLine($"Cycle started: {CycleStart} ... Cycle ended: {CycleEnd} **********\n");
Console.WriteLine($"Time: {TimeDifference.TotalDays} days {TimeDifference.Hours} hrs {TimeDifference.Minutes} min {TimeDifference.Seconds} sec {TimeDifference.Milliseconds} ms");

File.AppendAllText(IterationLogFullPath, $"******\tSum: {SumOfIterations:N0} operations across {Cycles} cycles *********\n");
File.AppendAllText(IterationLogFullPath, $"Cycle started: {CycleStart} ... Cycle ended: {CycleEnd} **********\n");
File.AppendAllText(IterationLogFullPath, $"Average: {(SumOfIterations/Cycles):N0} operations per second **********\n");
File.AppendAllText(IterationLogFullPath, $"Time: {TimeDifference.TotalDays} days {TimeDifference.Hours} hrs {TimeDifference.Minutes} min {TimeDifference.Seconds} sec {TimeDifference.Milliseconds} ms");
File.AppendAllText(IterationLogFullPath, $"{new string('_', 33)}\n\n");

