namespace sdu;

public static class AnsiExtensions {
	public static string ToPixelBar(this double usedRatio, int width = 42) {
		double clampedRatio = Math.Clamp(usedRatio, 0.0, 1.0);
		int filled = (int)Math.Round(clampedRatio * width);

		string color = clampedRatio > 0.8 ? "red" : (clampedRatio > 0.6 ? "yellow" : "green");

		string bar = $"[{color}]{new string('█', filled)}[/]" +
					 $"[grey]{new string('▒', width - filled)}[/]";

		return bar;
	}
}
