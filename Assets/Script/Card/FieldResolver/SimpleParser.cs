using System.Text.RegularExpressions;

public class SimpleParser
{
    //string=string or int = int
    public static bool Evaluate(string expression)
    {
        var match = Regex.Match(expression, @"(.+?)\s*(==|=|!=|>|<)\s*(.+)");
        if (!match.Success) return false;

        var left = match.Groups[1].Value.Trim();
        var op = match.Groups[2].Value.Trim();
        var right = match.Groups[3].Value.Trim();

        // 尝试做数字比较
        if (int.TryParse(left, out int leftNum) && int.TryParse(right, out int rightNum))
        {
            switch (op)
            {
                case ">": return leftNum > rightNum;
                case "<": return leftNum < rightNum;
                case "=":
                case "==": return leftNum == rightNum;
                case "!=": return leftNum != rightNum;
                default: return false;
            }
        }

        // 字符串比较
        switch (op)
        {
            case "=":
            case "==": return left == right;
            case "!=": return left != right;
            default: return false;
        }
    }
}

