namespace Interfaces.Core
{
    public class ScoreCalculation
    {
        public int Default(int wins, int loses, int draws)
        {
            return wins*3 + draws;
        }
    }
}
