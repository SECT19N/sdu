namespace sdu;

public static class ByteSizeExtensions {
	public static string ToHumanReadableSize(this long bytes) {
		string[] suffixes = [
			"B",
			"KiB",
			"MiB",
			"GiB",
			"TiB",
			"PiB"
		];

		int count = 0;
		double number = (double)bytes;

		while (number >= 1024 && count < suffixes.Length - 1) {
			number /= 1024;
			count++;
		}

		return $"{number:0.##} {suffixes[count]}";
	}
}
