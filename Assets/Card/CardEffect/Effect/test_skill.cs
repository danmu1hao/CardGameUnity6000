using System.Threading.Tasks;
using UnityEngine;

public class test_skill : AtomicEffect
{
    protected override async Task OnExecute()
    {

        Debug.Log("Test");
    }
}