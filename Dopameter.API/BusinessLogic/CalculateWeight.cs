using Dopameter.Common.DTOs;

namespace Dopameter.BusinessLogic;

public class CalculateWeight
{
    public static int CalculateGremlinWeight(int oldLastSetWeight, DateTime lastFedDate, int percentFed)
    {
        // Get the current date
        DateTime today = DateTime.Now;

        // Calculate the number of days since the gremlin was last fed
        int daysSinceLastFed = (today - lastFedDate).Days;

        // Calculate the current weight of the gremlin
        // Formula: (last_set_weight / 28) * (today - last_fed_day)
        double currentWeight = (oldLastSetWeight / 28.0) * (28.0 - daysSinceLastFed);
        
        double maxOfWeights = double.Max(currentWeight, percentFed);
        double minOfWeights = double.Min(currentWeight, percentFed);

        // Recalculate the weight after feeding
        // Formula: currentWeight + (percentFed / 100.0) * oldLastSetWeight
        double newWeight = maxOfWeights + (minOfWeights / 5);

        if (newWeight>100.0)
        {
            newWeight = 100.0;
        }

        // Return the newly calculated weight convert it to an int
        return (int)newWeight;
        
    }
    
    // Calculate the weight of the gremlin based on the intensity of the gremlin
    // and the kind of gremlin
    // So you keep track of the current last_set_weight of the gremlin.
    // But the current weight is the (last_set_weight/28) * (today - last_fed_day)
    // whenever you feed it, we recalculate weight based on (last_set_weight/28) * (today - last_fed_day) as current weight.
    //          and we update last fed date.
    
    
    // TO-DO:
    // Add the last_set_weight to gremlin modal.
    // On front-end, set current weight as (last_set_weight/28) * (today - last_fed_day)
    // On back-end handle the what to do on feed the gremlin. that is...
    //          set the last_fed_date. And then last_set_weight to a new weight based on how much they fed (use formula)
    // IF USER changes Intensity... itsokay because last_Set_weight is a PERCENT from 1-100. 
    
}