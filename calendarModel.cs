using System;
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
                    // WriteLine($"UserEvent \"{eve.eventName}\" and Userevent \"{e.eventName}\" overlapping detected!");
                    if (eve.priority < e.priority){
                        // WriteLine($"UserEvent \"{eve.eventName}\": {eve.priorityStr}");
                        // WriteLine($"UserEvent \"{e.eventName}\": {e.priorityStr}");
                        // WriteLine($"UserEvent \"{e.eventName}\" will be deleted since it has lower priority");
                        eventsEffected.Add(e);
                        eventList.Remove(e);
                        continue;
                    }
                    else if (eve.priority > e.priority){
                        // WriteLine($"UserEvent \"{e.eventName}\": {e.priorityStr}");
                        // WriteLine($"UserEvent \"{eve.eventName}\": {eve.priorityStr}");
                        // WriteLine($"UserEvent \"{eve.eventName}\" will be deleted since it has lower priority");
                        return (eventAdded, eventsEffected);
                    }
                    else{
                        samePriority = true;
                    }
                if (samePriority){
                    // WriteLine($"UserEvent \"{eve.eventName}\": {eve.priorityStr}");
                    // WriteLine($"UserEvent \"{e.eventName}\": {e.priorityStr}");
                    // WriteLine($"UserEvent \"{eve.eventName}\" and UserEvent \"{e.eventName}\" have the same priority");
                    // WriteLine($"Please choose which event to delete! (1 for \"{eve.eventName}\", 2 for \"{e.eventName}\")");
                    return (eventAdded, eventsEffected);
                }
            }
        }
        eventList.Add(eve);
        eventAdded = true;
        return (eventAdded, eventsEffected);
    }

    public void deleteUserEvent(UserEvent eve){
        for (int i = 0; i < eventList.Count; i++){
            if (UserEvent.eventsEqual(eventList[i], eve)){
                eventList.Remove(eventList[i]);
            }
        }
    }

    public void generateSchedule(){
        foreach (var e in eventList){
            events.Enqueue(e, e.priority);
        }
        while (events.Count > 0){
            var eve = events.Dequeue();
            WriteLine($"UserEvent {eve.eventName}: {eve.priorityStr} ");
            WriteLine($"\tFrom: {eve.startHour:00}:{eve.startMin:00}:{eve.startSec:00} To: {eve.endHour:00}:{eve.endMin:00}:{eve.endSec:00}");
        }
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

class Top{
    static void Main(){
        var user1 = new User("Kimi");
        user1.userCalendar.addUserEvent(new UserEvent("Make Lunch", "Urgent", 2023, 8, 14, 11, 0, 0, 
        2023, 8, 14, 12, 0, 0));
        user1.userCalendar.addUserEvent(new UserEvent("Do Homework", "Trivial", 2023, 8, 14, 11, 0, 0, 
        2023, 8, 14, 12, 0, 0));
        user1.userCalendar.addUserEvent(new UserEvent("Make Dinner", "Urgent", 2023, 8, 14, 18, 0, 0, 
        2023, 8, 14, 19, 0, 0));
        // user1.userCalendar.deleteUserEvent(new UserEvent("Make Lunch", "Urgent", 2023, 8, 14, 11, 0, 0, 
        // 2023, 8, 14, 12, 0, 0));
        user1.userCalendar.generateSchedule();
    }
}