namespace CoronaDataHelper.JSON {
	public class JSONDailyData {
		public string date { get; set; }
		public float? total_cases { get; set; }
		public float? new_cases { get; set; }
		public float? new_deaths { get; set; }
		public float? total_cases_per_million { get; set; }
		public float? new_cases_per_million { get; set; }
		public float? new_deaths_per_million { get; set; }
		public float? stringency_index { get; set; }
		public float? new_cases_smoothed { get; set; }
		public float? new_deaths_smoothed { get; set; }
		public float? new_cases_smoothed_per_million { get; set; }
		public float? new_deaths_smoothed_per_million { get; set; }
		public float? total_deaths { get; set; }
		public float? total_deaths_per_million { get; set; }
	}
}
