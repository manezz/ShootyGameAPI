namespace ShootyGameAPI.DTOs
{
    public class ScoreResponse
    {
        public int ScoreId { get; set; }
        public int ScoreValue { get; set; }
        public float AverageAccuracy { get; set; }
        public float RoundTime { get; set; }
        public Score_UserResponse User { get; set; } = new();
    }

    public class Score_UserResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PlayerTag { get; set; }
    }
}
