using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using static System.Console;
using static System.Math;

class Event{
    public double priority;
    public string priorityStr;
    public string eventName;
    public int startYear; 
    public int startMonth;
    public int startDay;
    public int startHour;
    public int startMin;
    public int startSec;
    public int endYear;
    public int endMonth;
    public int endDay;
    public int endHour;
    public int endMin;
    public int endSec;
    public string? sideNote; 
    public Dictionary<string, double> priorityStrToDouble = new Dictionary<string, double>{
        { "Urgent", 1.0 }, { "Normal", 5.0 }, { "Trivial", 10.0 }
    };

    public Event(string eventName, string priorityStr, int startYear, int startMonth, int startDay, 
                    int startHour, int startMin, int startSec, int endYear, int endMonth, int endDay, 
                    int endHour, int endMin, int endSec, string? sideNote = null){
        this.priorityStr = priorityStr;
        this.priority = priorityStrToDouble[priorityStr];
        this.eventName = eventName;
        this.startYear = startYear;
        this.startMonth = startMonth;
        this.startDay = startDay;
        this.startHour = startHour;
        this.startMin = startMin;
        this.startSec = startSec;
        this.endYear = endYear;
        this.endMonth = endMonth;
        this.endDay = endDay;
        this.endHour = endHour;
        this.endMin = endMin;
        this.endSec = endSec;
        this.sideNote = sideNote;
    }
}

class Calendar{
    public PriorityQueue<Event, double> events;
    public List<Event> eventList;

    public Calendar(){
        this.events = new PriorityQueue<Event, double>();
        this.eventList = new List<Event>();
        }

    public void addEvent(Event eve){
        var eveStartDatetime = Convert.ToDateTime($"{eve.startYear}-{eve.startMonth}-{eve.startDay} {eve.startHour}:{eve.startMin}:{eve.startSec}");
        var eveEndDatetime = Convert.ToDateTime($"{eve.endYear}-{eve.endMonth}-{eve.endDay} {eve.endHour}:{eve.endMin}:{eve.endSec}");
        foreach (var e in eventList){
            var eStartDatetime = Convert.ToDateTime($"{e.startYear}-{e.startMonth}-{e.startDay} {e.startHour}:{e.startMin}:{e.startSec}");
            var eEndDatetime = Convert.ToDateTime($"{e.endYear}-{e.endMonth}-{e.endDay} {e.endHour}:{e.endMin}:{e.endSec}");
            bool samePriority = false;
            // overlapping detected
            if ((eveStartDatetime > eStartDatetime && eveEndDatetime > eEndDatetime) ||
                (eveStartDatetime < eStartDatetime && eveEndDatetime < eEndDatetime) ||
                (eveStartDatetime > eStartDatetime && eveEndDatetime < eEndDatetime) ||
                (eveStartDatetime < eStartDatetime && eveEndDatetime > eEndDatetime) ||
                (eveStartDatetime == eStartDatetime && eveEndDatetime == eEndDatetime)){
                    WriteLine($"Event {eve.eventName} and event {e.eventName} overlapping detected!");
                    if (eve.priority < e.priority){
                        WriteLine($"Event {eve.eventName}: {eve.priorityStr}");
                        WriteLine($"Event {e.eventName}: {e.priorityStr}");
                        WriteLine($"Event {e.eventName} will be deleted since it has lower priority");
                        eventList.Add(eve);
                        eventList.Remove(e);
                        return;
                    }
                    else if (eve.priority > e.priority){
                        WriteLine($"Event {e.eventName}: {e.priorityStr}");
                        WriteLine($"Event {eve.eventName}: {eve.priorityStr}");
                        WriteLine($"Event {eve.eventName} will be deleted since it has lower priority");
                        return;
                    }
                    else{
                        samePriority = true;
                    }
                if (samePriority){
                    WriteLine($"Event {eve.eventName}: {eve.priorityStr}");
                    WriteLine($"Event {e.eventName}: {e.priorityStr}");
                    WriteLine($"Event {eve.eventName} and Event {e.eventName} have the same priority");
                    WriteLine($"Please choose which event to delete! (1 for {eve.eventName}, 2 for {e.eventName})");
                    if (ReadLine()! == "2"){
                        eventList.Add(eve);
                        eventList.Remove(e);
                    }
                    return;
                }
            }
            eventList.Add(eve);
        }
    }
    public void generateSchedule(){
        foreach (var e in eventList){
            events.Enqueue(e, e.priority);
        }
        while (events.Count > 0){
            var eve = events.Dequeue();
            WriteLine($"Event {eve.eventName}: {eve.priorityStr}");
        }
    }
}

class User{
    public string name;
    public Calendar calendar;
    public User(string name){
        this.name = name;
        this.calendar = new Calendar();
    }
}

class Top{
    static void Main(){
        
    }
}