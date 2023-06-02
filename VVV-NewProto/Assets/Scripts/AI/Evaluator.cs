public interface IEvaluator
{
    int GetEvaluation(IRepresentation representation); // who is winning in terms of who has a higher score 

    int CheckIfScored (IRepresentation representation);
}
