﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using static System.Console;
using static System.Math;

class UserEvent{
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

    public UserEvent(string eventName, string priorityStr, int startYear, int startMonth, int startDay, 
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

    public static bool eventsEqual(UserEvent e0, UserEvent e1){
        return (e0.eventName.Equals(e1.eventName) && e0.priorityStr.Equals(e1.priorityStr) &&
                e0.startYear.Equals(e1.startYear) && e0.startMonth.Equals(e1.startMonth) && 
                e0.startDay.Equals(e1.startDay) && e0.startHour.Equals(e1.startHour) &&
                e0.startMin.Equals(e1.startMin) && e0.startSec.Equals(e1.startSec) && 
                e0.endYear.Equals(e1.endYear) && e0.endMonth.Equals(e1.endMonth) && 
                e0.endDay.Equals(e1.endDay) && e0.endHour.Equals(e1.endHour) && 
                e0.endMin.Equals(e1.endMin) && e0.endSec.Equals(e1.endSec));
    }
}

class UserCalendar{
    public PriorityQueue<UserEvent, double> events;
    public List<UserEvent> eventList;

    public UserCalendar(){
        this.events = new PriorityQueue<UserEvent, double>();
        this.eventList = new List<UserEvent>();
        }

    public (bool eventAdded, List<UserEvent> eventsEffected) addUserEvent(UserEvent eve){
        var eveStartDatetime = Convert.ToDateTime($"{eve.startYear}-{eve.startMonth}-{eve.startDay} {eve.startHour}:{eve.startMin}:{eve.startSec}");
        var eveEndDatetime = Convert.ToDateTime($"{eve.endYear}-{eve.endMonth}-{eve.endDay} {eve.endHour}:{eve.endMin}:{eve.endSec}");
        bool eventAdded = false;
        var eventsEffected = new List<UserEvent>();
        var tempEvents = new PriorityQueue<UserEvent, double>();
        foreach (var e in eventList){
            tempEvents.Enqueue(e, e.priority);
        }
    
        while (tempEvents.Count > 0){
            var e = tempEvents.Dequeue();
            var eStartDatetime = Convert.ToDateTime($"{e.startYear}-{e.startMonth}-{e.startDay} {e.startHour}:{e.startMin}:{e.startSec}");
            var eEndDatetime = Convert.ToDateTime($"{e.endYear}-{e.endMonth}-{e.endDay} {e.endHour}:{e.endMin}:{e.endSec}");
            bool samePriority = false;
            if (UserEvent.eventsEqual(eve, e)){
                continue;
            }
            // overlapping detected
            if ((eveStartDatetime > eStartDatetime && eveStartDatetime < eEndDatetime && eveEndDatetime > eEndDatetime) ||
                (eveStartDatetime < eStartDatetime && eveEndDatetime > eStartDatetime && eveEndDatetime < eEndDatetime) ||
                (eveStartDatetime < eStartDatetime && eveEndDatetime > eEndDatetime) ||
                (eveStartDatetime > eStartDatetime && eveEndDatetime < eEndDatetime) ||
                (eveStartDatetime == eStartDatetime && eveEndDatetime == eEndDatetime) || 
                (eveStartDatetime == eStartDatetime && eveEndDatetime > eEndDatetime) ||
                (eveStartDatetime < eStartDatetime && eveEndDatetime == eEndDatetime) ||
                (eveStartDatetime > eStartDatetime && eveEndDatetime == eEndDatetime) ||
                (eveStartDatetime == eStartDatetime && eveEndDatetime < eEndDatetime)){
                    if (eve.priority < e.priority){
                        eventsEffected.Add(e);
                        eventList.Remove(e);
                        continue;
                    }
                    else if (eve.priority > e.priority){
                        return (eventAdded, eventsEffected);
                    }
                    else{
                        samePriority = true;
                    }
                if (samePriority){
                    return (eventAdded, eventsEffected);
                }
            }
        }
        eventList.Add(eve);
        eventAdded = true;
        return (eventAdded, eventsEffected);
    }

    public void deleteUserEvent(string eventToDeleteName){
        var eventToDelete = new UserEvent("Dummy", "Urgent", 1970, 1, 1, 1, 1, 1, 1970, 1, 1, 1, 1, 1);
        foreach (var e in eventList){
            if (e.eventName == eventToDeleteName){
                eventToDelete = e;
                break;
            }
        }
        for (int i = 0; i < eventList.Count; i++){
            if (UserEvent.eventsEqual(eventList[i], eventToDelete)){
                eventList.Remove(eventList[i]);
            }
        }
    }

    public List<UserEvent> getUserEvent(int year, int month, int day){
        var eventsFound = new List<UserEvent>();
        foreach (var e in eventList){
            if (e.startYear == year && e.startMonth == month && e.startDay == day){
                eventsFound.Add(e);
            }
        }
        return eventsFound;
    }

    public List<UserEvent> generateSchedule(){
        var eventsToReturn = new List<UserEvent>();
        foreach (var e in eventList){
            events.Enqueue(e, e.priority);
        }
        while (events.Count > 0){
            var eve = events.Dequeue();
            eventsToReturn.Add(eve);   
        }
        return eventsToReturn;
    }
}

class User{
    public string name;
    public UserCalendar userCalendar;
    public User(string name){
        this.name = name;
        this.userCalendar = new UserCalendar();
    }
}