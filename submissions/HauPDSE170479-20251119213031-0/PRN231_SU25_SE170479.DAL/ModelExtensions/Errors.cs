namespace PRN231_SU25_SE170479.DAL.ModelExtensions
{
	public static class Errors
	{
		public static Result<T> ValidationError<T>(string message) => Result<T>.Fail("HB40001", message);
		public static Result<T> NotFound<T>(string message = "Handbag not found") => Result<T>.Fail("HB40401", message);
		public static Result<T> Unauthorized<T>() => Result<T>.Fail("HB40101", "Token missing/invalid");
		public static Result<T> Forbidden<T>() => Result<T>.Fail("HB40301", "Permission denied");
		public static Result<T> ServerError<T>() => Result<T>.Fail("HB50001", "Internal server error");
	}
}