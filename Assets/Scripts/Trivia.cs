using Postgrest.Models;
using Postgrest.Attributes;

public class Trivia : BaseModel
{
    [Column("id"), PrimaryKey]
    public int id { get; set; }

    [Column("question")]
    public string question { get; set; }

    [Column("answer1")]
    public string answer1 { get; set; }

    [Column("answer2")]
    public string answer2 { get; set; }

    [Column("answer3")]
    public string opcion3 { get; set; }

    [Column("correct_answer")]
    public int correct_answer { get; set; }
}
