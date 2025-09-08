using System;
using System.Collections.Generic;

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        Random rng = new Random(); // 创建随机数生成器
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // 生成一个0到n之间的随机数
            (list[k], list[n]) = (list[n], list[k]); // 交换元素
        }
    }
}