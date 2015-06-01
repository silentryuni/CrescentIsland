namespace CrescentIsland.Website.Helpers
{
    public static class MathHelper
    {
        public static int CalcPercentage(int curValue, int maxValue)
        {
            double percent = (double)curValue / (double)maxValue;
            percent = percent * 100;

            return (int)percent;
        }
    }
}