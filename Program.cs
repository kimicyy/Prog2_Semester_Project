using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using static System.Console;
using static System.Math;

class Event{
    public double priority;
    public string eventName;
    public Dictionary<string, double> priorityStrToDouble = new Dictionary<string, double>{
        { "Urgent", 10.0 }, 
        { "Normal", 5.0 }, 
        { "Trivial", 1.0 }
    };
    public Event(string eventName, string priorityStr){
        this.priority = priorityStrToDouble[priorityStr];
        this.eventName = eventName;
    }
}

class Calendar{
    static void Main(){

    }
}