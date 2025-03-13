using System.ComponentModel.DataAnnotations;

namespace ShootyGameAPI.DTOs
{
    public class ScoreRequest
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "ScoreValue must be a positive value")]
        public int ScoreValue { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "AverageAccuracy must be 0 to 100")]
        public float AverageAccuracy { get; set; }

        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "RoundTime must be a positive value")]
        public float RoundTime { get; set; }

        [Required]
        public int MoneyEarned { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "UserId must be a positive value")]
        public int UserId { get; set; }
    }
}
