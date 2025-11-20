using System.Threading.Tasks;
using UnityEngine;

public class test : AtomicEffect
{
    protected override async Task OnExecute()
    {

        Debug.Log("Test");
    }
}