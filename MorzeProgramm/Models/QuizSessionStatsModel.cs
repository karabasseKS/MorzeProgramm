public class QuizSessionStatsModel
{
    public int BlockNumber { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalAttempts { get; set; }

    public int ProgressPercent => TotalAttempts == 0 ? 0 : (int)(CorrectAnswers * 100.0 / TotalAttempts);

    public int QuizLength => 10;
}