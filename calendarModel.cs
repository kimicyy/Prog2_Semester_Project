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
    public int startYear; 
    public int startMonth;
    public int startDay
    public int startHour;
    public int startMin;
    public int startSec;
    public int endYear;
    public int endMonth;
    public int endHour;
    public int endMin;
    public int endSec;
    public string? sideNote; 
    public Dictionary<string, double> priorityStrToDouble = new Dictionary<string, double>{
        { "Urgent", 10.0 }, { "Normal", 5.0 }, { "Trivial", 1.0 }
    };

    public Event(string eventName, string priorityStr, int startYear, int startMonth, int startDay, 
                    int startHour, int startMin, int startSec, int endYear, int endMonth, int endDay, 
                    int endHour, int endMin, int endSec, string? sideNote = null){
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
    PriorityQueue<Event, double> events;
    List<Event> eventList;
    List<Event> eventListForSchedule;

    public Calendar(){
        this.events = new PriorityQueue<Event, double>();
        this.eventList = new List<Event>();
    }

    public void addEvent(Event eve){
        events.Enqueue(eve, eve.priority);
        eventList.Add(eve);
    }
    
}

class User{
    string name;
    Calendar calendar;
    public User(string name){
        this.name = name;
        this.calendar = new Calendar();
    }
}

class Top{
    static void Main(){
        
    }
}