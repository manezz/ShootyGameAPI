namespace ShootyGameAPI.DTOs
{
    public class ScoreResponse
    {
        public int ScoreId { get; set; }
        public int ScoreValue { get; set; }
        public float AverageAccuracy { get; set; }
        public float RoundTime { get; set; }
        public int UserId { get; set; }
    }
}
