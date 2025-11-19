using System.Threading.Tasks;
using UnityEngine;

public class test : AtomicEffect
{
    protected override async Task OnExecute()
    {

         LogCenter.Log("Test");
    }
}