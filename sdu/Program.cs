using sdu;
using Spectre.Console;

List<DriveInfo> allDrives = [.. DriveInfo.GetDrives()
										 .Where(d => d.DriveType == DriveType.Fixed && d.IsReady)];

Table table = new Table().Border(TableBorder.Rounded)
						 .AddColumn("Drive")
						 .AddColumn("Label")
						 .AddColumn("Usage")
						 .AddColumn("Free / Total", col => col.RightAligned())
						 .AddColumn("Format");

foreach (DriveInfo d in allDrives) {
	double usedRatio = (double)(d.TotalSize - d.TotalFreeSpace) / d.TotalSize;
	int barWidth = 50;
	int filled = (int)(usedRatio * barWidth);

	string bar = $"[green]{new string('█', filled)}[/][grey]{new string('░', barWidth - filled)}[/]";
	string percent = $"{(usedRatio * 100):0}%";

	table.AddRow(
		$"[bold cyan]{d.Name}[/]",
		$"{d.VolumeLabel}",
		$"{usedRatio.ToPixelBar()} {(usedRatio * 100):0}%",
		$"{d.TotalFreeSpace.ToHumanReadableSize()} / {d.TotalSize.ToHumanReadableSize()}",
		$"[grey]{d.DriveFormat}[/]"
	);
}

AnsiConsole.Write(table);

MultiSelectionPrompt<DriveInfo> prompt = new MultiSelectionPrompt<DriveInfo>()
	.Title("Select [blue]drives[/] to scan:")
	.Required(true)
	.PageSize(10)
	.UseConverter(d => $"{d.Name} ({d.VolumeLabel}) - {d.TotalFreeSpace.ToHumanReadableSize()} free")
	.AddChoices(allDrives)
	.MoreChoicesText("[grey](Move up and down to reveal more drives)[/]")
	.InstructionsText("[grey](Press [blue]<space>[/] to toggle a drive, [green]<enter>[/] to accept)[/]");

var selectedDrives = AnsiConsole.Prompt(prompt);

AnsiConsole.MarkupLine("[bold]You selected the following drives:[/]");

selectedDrives
	.Select((d, i) => $"[green]{i + 1}.[/] [cyan]{d.Name}[/]")
	.ToList()
	.ForEach(line => AnsiConsole.MarkupLine(line));
