using UnityEngine;

namespace utils
{
    public class CommonUtils
    {
        public static int[] GetRandomInt(int min, int max)
        {

            int randomNumber1 = Random.Range(min, max);
            int randomNumber2;

            // Keep generating a new number until it's different from randomNumber1.
            do
            {
                randomNumber2 = Random.Range(min, max);
            } while (randomNumber2 == randomNumber1);

            return new[] {randomNumber1, randomNumber2};
    }
    }
}