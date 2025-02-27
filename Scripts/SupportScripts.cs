
public class SupportScripts
{
    

    public static void ShuffleArray<T>(T[] a)
    {
        System.Random rng = new System.Random();
        int n = a.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1); 
            (a[i], a[j]) = (a[j], a[i]);
        }
    }
}
