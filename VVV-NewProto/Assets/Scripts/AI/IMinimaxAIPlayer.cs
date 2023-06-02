using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Agents
{

    public class MinimaxAIPlayer : IAIPlayer
    {
        IEvaluator evaluator;

        int maxDepth;

        Move bestMove;

        



        public MinimaxAIPlayer(IEvaluator evaluator, int maxDepth = 11)
        {
            this.evaluator = evaluator;
            this.maxDepth = maxDepth;
           

        }

        public Move GetMove(IRepresentation representation, int player)
        {
            Minimax(representation, maxDepth, int.MinValue, int.MaxValue, player == 1);
            return bestMove;
        }

        int Minimax(IRepresentation rep, int depth, int alpha, int beta, bool isMaximizing)
        {
            if (depth == 0 || rep.GetGameOutcome() != GameOutcome.UNDETERMINED)
            {
                return evaluator.GetEvaluation(rep);
            }

            int bestEvaluation = isMaximizing ? int.MinValue : int.MaxValue;

            List<Move> possibleMoves = rep.GetPossibleMoves(isMaximizing ? 1 : -1);
            List<IRepresentation> possibleReps = new List<IRepresentation>();

            List<int> utilities = new List<int>(possibleMoves.Count); // List of Utility scores

            foreach (Move possibleMove in possibleMoves)
            {
                IRepresentation dupRep = rep.Duplicate();
                dupRep.MakeMove(possibleMove, isMaximizing ? 1 : -1);
                possibleReps.Add(dupRep);
                utilities.Add(evaluator.GetEvaluation(dupRep));
            }

            int index = 0;
            foreach (IRepresentation possibleRep in possibleReps)
            {
                int evaluation = Minimax(possibleRep, depth - 1, alpha, beta, !isMaximizing);

                if (isMaximizing)
                {
                    if (evaluation > bestEvaluation)
                    {
                        bestEvaluation = evaluation;
                        if (depth == maxDepth)
                        {
                            bestMove = possibleMoves[index];
                        }
                    }

                    alpha = Mathf.Max(alpha, bestEvaluation);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }

                else
                {
                    if (evaluation < bestEvaluation)
                    {
                        bestEvaluation = evaluation;
                        if (depth == maxDepth)
                        {
                            bestMove = possibleMoves[index];
                        }
                    }

                    beta = Mathf.Min(beta, bestEvaluation);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }

                index++;

            }

            return bestEvaluation;
        }
    }
}
