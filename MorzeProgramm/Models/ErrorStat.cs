using MorzeProgramm.Models;

public class ErrorStat
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Symbol { get; set; }
    public int Count { get; set; }

    public ApplicationUser User { get; set; }
}