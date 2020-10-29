namespace CoronaDataHelper.JSON {
	public class JSONCountry {
		public string continent { get; set; }
		public string location { get; set; }
		public float population { get; set; }
		public float population_density { get; set; }
		public float median_age { get; set; }
		public float aged_65_older { get; set; }
		public float aged_70_older { get; set; }
		public float gdp_per_capita { get; set; }
		public float diabetes_prevalence { get; set; }
		public float life_expectancy { get; set; }
		public JSONDailyData[] data { get; set; }
	}
}
