using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflactionSystem 
{
    public static T FindClassByName<T>(string className) where T : class
    {
        var type = Type.GetType(className);

        if (type == null)
        {
             LogCenter.LogError($"找不到类名：{className}");
            return null;
        }

        if (!typeof(T).IsAssignableFrom(type))
        {
             LogCenter.LogError($"{className} 不是 {typeof(T).Name} 的子类");
            return null;
        }
         LogCenter.Log("找到类"+className);
        return Activator.CreateInstance(type) as T;
    }

    /*public static T FindClassByName<T>(string className) where T : class
    {
        // 尝试直接查找（加上程序集名）
        string fullName = $"{className}, Assembly-CSharp";
        var type = Type.GetType(fullName);

        if (type == null)
        {
             LogCenter.LogWarning($"找不到类型 {className}，尝试在所有程序集中搜索...");

            // 遍历所有程序集查找
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(className);
                if (type != null)
                    break;
            }
        }

        if (type == null)
        {
             LogCenter.LogError($"❌ 仍然找不到类名：{className}");
            return null;
        }

        if (!typeof(T).IsAssignableFrom(type))
        {
             LogCenter.LogError($"❌ {className} 不是 {typeof(T).Name} 的子类");
            return null;
        }

         LogCenter.Log($"✅ 成功找到类：{className}");
        return Activator.CreateInstance(type) as T;
    }*/
}
