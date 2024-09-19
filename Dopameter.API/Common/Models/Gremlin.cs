namespace Dopameter.Common.Models;

public class Gremlin
{
    public int gremlinID { get; set; }
    public string name { get; set; }
    public string activityName { get; set; }
    public int kindOfGremlin { get; set; }
    public int pleasurePain { get; set; }
    public int intensity { get; set; }
    public int lastSetWeight { get; set; }
    public DateTime dateOfBirth { get; set; }
    public DateTime lastFedDate { get; set; }
}