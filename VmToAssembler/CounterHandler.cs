using System.Collections.Generic;
using System.Linq;
using VmToAssembler.Utils;

namespace VmToAssembler;

public class CounterHandler
{
    private Dictionary<int, List<string>> Counters { get; }
    
    private const string FalseCondition = "FALSE";

    private const string ContinueCondition = "CONTINUE";
    
    private int Counter { get; set; }

    public int CreateNewCounters()
    {
        int oldCounter = Counter;
        
        var counters = new List<string>
        {
            $"{FalseCondition + Counter}",
            $"{ContinueCondition + Counter}"
        };
        
        AddToCounters(Counter, counters);

        return oldCounter;
    }

    public CounterHandler()
    {
        Counters = new Dictionary<int, List<string>>();
        Counter = 0;
    }

    public List<string> GetCounters(int key)
    {
        return Counters.GetValueFromDict(key);
    }

    public void AddToCounters(int key, List<string> counters)
    {
        Counters.Add(key, counters);

        Counter++;
    }

    public string GetFalse(int key)
    {
        var counters = GetCounters(key);

        return counters.FirstOrDefault(counter => counter.Contains("FALSE"));
    }

    public string GetContinue(int key)
    {
        var counters = GetCounters(key);


        return counters.FirstOrDefault(counter => counter.Contains("CONTINUE"));
    }
}