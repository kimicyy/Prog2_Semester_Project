using Gdk;
using Gtk;
using Window = Gtk.Window;
using static Gtk.Orientation;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using static System.Console;
using static System.Math;

class UserNameWindow : Window{
    public string userName = "";
    public bool userNameEntered = false;
    Entry userNameBox = new Entry();

    public UserNameWindow() : base("Calendar: User Name") {
        SetDefaultSize(320, 280);
        Grid grid = new Grid();
        grid.Attach(new Label("Welcome to Smart Calendar!"), 0, 0, 3, 10);
        grid.Attach(new Label("User Name"), 0, 10, 1, 1);
        grid.Attach(userNameBox, 1, 10, 1, 1);
        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;

        Box row = new Box(Horizontal, 5);
        Button enter = new Button("Enter");
        row.Add(enter);
        enter.Clicked += onEnter; 
        Button quit = new Button("Quit");
        row.Add(quit);
        quit.Clicked += onQuit;

        Box vbox = new Box(Vertical, 5);
        vbox.Add(grid);
        vbox.Add(row);
        Add(vbox);
        vbox.Margin = 5;
    }

    void onEnter(object? sender, EventArgs args) {
        userName = userNameBox.Text;
        if (userName.Length > 0){
            userNameEntered = true;
            var graphicalCalendar = new GraphicalCalendar(userName);
            graphicalCalendar.ShowAll();
            Destroy();
        }
        else{
            userNameEntered = false;
        }
    }

    void onQuit(object? sender, EventArgs args) {
        Application.Quit();
    }

    protected override bool OnDeleteEvent(Event e) {
        Application.Quit();
        return true;
    }
}

class GraphicalCalendar : Window{
    public User user;
    public Button generate = new Button("Generate Schedule");

    public GraphicalCalendar(string userName) : base($"{userName}'s Calendar"){
        SetDefaultSize(320, 280);
        Calendar calendar = new Calendar();
        calendar.DaySelected += OnDaySelected;
        Fixed fix = new Fixed();
        fix.Put(calendar, 20, 20);
        fix.Put(generate, 20, 200);
        Add(fix);
        this.user = new User(userName);
        generate.Clicked += onGenerate;
    }

    void OnDaySelected(object? sender, EventArgs args){
        Calendar cal = (Calendar) sender;
        var eventsFound = user.userCalendar.getUserEvent(cal.Year, cal.Month + 1, cal.Day);
        ShowEventDialog sd = new ShowEventDialog(this, eventsFound, cal.Month + 1 + "/" + cal.Day + "/" + cal.Year);
        if (sd.Run() == (int) ResponseType.Ok){
            using (AddEventDialog d = new AddEventDialog(sd, cal.Month + 1 + "/" + cal.Day + "/" + cal.Year))
            if (d.Run() == (int) ResponseType.Ok){
                ComboBoxText[] comboBoxes = {
                    d.priorityBox,
                    d.fromHourBox,
                    d.fromMinuteBox,
                    d.fromSecondBox,
                    d.toHourBox,
                    d.toMinuteBox,
                    d.toSecondBox
                };
                if (d.eventNameEntry.Text.Length <= 0){
                    return;
                }
                foreach (var box in comboBoxes){
                    if (box.ActiveText == null){
                        return;
                    }
                }
                UserEvent eve = new UserEvent(d.eventNameEntry.Text, d.priorityBox.ActiveText,
                                            cal.Year, cal.Month + 1, cal.Day, 
                                            int.Parse(d.fromHourBox.ActiveText),
                                            int.Parse(d.fromMinuteBox.ActiveText),
                                            int.Parse(d.fromSecondBox.ActiveText),
                                            cal.Year, cal.Month + 1, cal.Day,
                                            int.Parse(d.toHourBox.ActiveText),
                                            int.Parse(d.toMinuteBox.ActiveText),
                                            int.Parse(d.toSecondBox.ActiveText));
                var result = user.userCalendar.addUserEvent(eve);
                // event successfully added
                if (result.eventAdded && result.eventsEffected.Count == 0){
                    createSimpleMessageDialog("Add event successfully!");
                }
                // event successfully added and delete the other events
                else if (result.eventAdded && result.eventsEffected.Count > 0){
                    foreach (var e in result.eventsEffected){
                        createSimpleMessageDialog($"Event \"{eve.eventName}\" and Event \"{e.eventName}\" overlapping detected!\n\n\tEvent \"{eve.eventName}\": {eve.priorityStr}\n\n\tEvent \"{e.eventName}\": {e.priorityStr}\n\nEvent \"{e.eventName}\" will be deleted since it has lower priority");
                    }
                }
                // event not added since there is another event that has higher or the same priority
                else{
                    createSimpleMessageDialog("Fail to add event since there is other event with higher or equal priority!");
                }
            }
            sd.Destroy();
        }
        if (sd.Run() == (int) ResponseType.Cancel){
            if (sd.eventSelectedName != null){
                user.userCalendar.deleteUserEvent(sd.eventSelectedName);
            }
            sd.Destroy();
        }
        
    }

    void createSimpleMessageDialog(string message){
        var sm = new SimpleMessageDialog(this, message);
        if (sm.Run() == (int) ResponseType.Ok){
            sm.Destroy();
        }
    }

    void onGenerate(object? sender, EventArgs e){
        var eventListGenerated = user.userCalendar.generateSchedule();
        var sc = new ShowScheduleDialog(this, eventListGenerated, "Calendar: Schedule");
        if (sc.Run() == (int) ResponseType.Ok){
            sc.Destroy();
        }
    }

    protected override bool OnDeleteEvent(Event e) {
        Application.Quit();
        return true;
    }
}

class AddEventDialog : Dialog{
    static string[] priorityLevel = {
        "Urgent",
        "Normal",
        "Trivial"
    };

    public ComboBoxText priorityBox = new ComboBoxText();
    public ComboBoxText fromHourBox = new ComboBoxText();
    public ComboBoxText fromMinuteBox = new ComboBoxText();
    public ComboBoxText fromSecondBox = new ComboBoxText();
    public ComboBoxText toHourBox = new ComboBoxText();
    public ComboBoxText toMinuteBox = new ComboBoxText();
    public ComboBoxText toSecondBox = new ComboBoxText();
    public Entry eventNameEntry = new Entry();

    public AddEventDialog(ShowEventDialog parent, string dateStr) : base(dateStr, parent,
            DialogFlags.Modal, "Add", ResponseType.Ok) {
        SetDefaultSize(560, 300);
        
        foreach (var priorityStr in priorityLevel) {
            priorityBox.AppendText(priorityStr);
        }
        for (int i = 0; i < 24; i++){
            string text = "";
            if (i < 10){
                text = $"0{i}";
            }
            else{
                text = $"{i}";
            }
            fromHourBox.AppendText(text);
            toHourBox.AppendText(text);
        }
        for (int i = 0; i < 60; i++){
            string text = "";
            if (i < 10){
                text = $"0{i}";
            }
            else{
                text = $"{i}";
            }
            fromMinuteBox.AppendText(text);
            fromSecondBox.AppendText(text);
            toMinuteBox.AppendText(text);
            toSecondBox.AppendText(text);
        }
        Grid grid = new Grid();
        grid.Attach(new Label("Event Name:"), 0, 0, 1, 1);
        grid.Attach(eventNameEntry, 1, 0, 1, 1);
        
        grid.Attach(new Label("Event Priority: "), 0, 1, 1, 1);
        grid.Attach(priorityBox, 1, 1, 1, 1);

        grid.Attach(new Label("From: "), 0, 2, 1, 1);
        grid.Attach(fromHourBox, 1, 2, 1, 1);
        grid.Attach(new Label(":"), 2, 2, 1, 1);
        grid.Attach(fromMinuteBox, 3, 2, 1, 1);
        grid.Attach(new Label(":"), 4, 2, 1, 1);
        grid.Attach(fromSecondBox, 5, 2, 1, 1);

        grid.Attach(new Label("To: "), 0, 3, 1, 1);
        grid.Attach(toHourBox, 1, 3, 1, 1);
        grid.Attach(new Label(":"), 2, 3, 1, 1);
        grid.Attach(toMinuteBox, 3, 3, 1, 1);
        grid.Attach(new Label(":"), 4, 3, 1, 1);
        grid.Attach(toSecondBox, 5, 3, 1, 1);

        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;
        grid.Margin = 5;
        ContentArea.Add(grid);
        ShowAll();
    }

    protected override bool OnDeleteEvent(Event e) {
        Destroy();
        return true;
    }
}

class SimpleMessageDialog : Gtk.MessageDialog{
    public SimpleMessageDialog(GraphicalCalendar parent, string message) : base(parent, 
                                                                                DialogFlags.Modal, 
                                                                                MessageType.Info, 
                                                                                ButtonsType.Ok, 
                                                                                "{0}", message){
        ShowAll();
    }
}

class ShowEventDialog : Dialog{
    public string? eventSelectedName = null;
    public ShowEventDialog(GraphicalCalendar parent, List<UserEvent> eventFound, 
                            string dateStr) : base(dateStr, parent, DialogFlags.Modal, 
                            "Add", ResponseType.Ok, "Delete", ResponseType.Cancel) {
        Grid grid = new Grid();
        var addButton = new RadioButton("Add New Event");
        addButton.Clicked += onClicked;
        if (eventFound.Count == 0){
            grid.Attach(new Label("No event"), 0, 0, 1, 1);
            grid.Attach(addButton, 0, 1, 1, 1);    
        }
        else{
            var buttons = new List<RadioButton>();
            for (int i = 0; i < eventFound.Count; i++){
                var b = new RadioButton(addButton, $"{eventFound[i].eventName}");
                grid.Attach(b, 0, i, 1, 1);
                grid.Attach(new Label($"\n\tFrom: {eventFound[i].startHour:00}:{eventFound[i].startMin:00}:{eventFound[i].startSec:00}\n\tTo:\t   {eventFound[i].endHour:00}:{eventFound[i].endMin:00}:{eventFound[i].endSec:00}"), 1, i, 1, 1);
                grid.Attach(new Label($"Priority: {eventFound[i].priorityStr}"), 2, i, 1, 1);
                buttons.Add(b);
                b.Clicked += onClicked;
            }
            grid.Attach(addButton, 0, eventFound.Count, 1, 1);
        }
        
        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;
        grid.Margin = 5;
        ContentArea.Add(grid);
        ShowAll();
    }
    
    void onClicked(object? sender, EventArgs e) {
        RadioButton b = (RadioButton) sender;
        if (b.Label != "Add New Event"){
            eventSelectedName = b.Label;
        }
    }

    protected override bool OnDeleteEvent(Event e) {
        Destroy();
        return true;
    }

}

class ShowScheduleDialog : Dialog{
    public ShowScheduleDialog(GraphicalCalendar parent, List<UserEvent> eventListGenerated, 
                            string title) : base(title, parent, DialogFlags.Modal, 
                            "Ok", ResponseType.Ok){
        var grid = new Grid();
        if (eventListGenerated.Count <= 0){
            grid.Attach(new Label("No event"), 0, 0, 1, 1);
        }
        else{
            for (int i = 0; i < eventListGenerated.Count; i++){
                grid.Attach(new Label($"{eventListGenerated[i].eventName}"), 0, i, 1, 1);
                grid.Attach(new Label($"\n\tFrom: {eventListGenerated[i].startHour:00}:{eventListGenerated[i].startMin:00}:{eventListGenerated[i].startSec:00}\n\tTo:\t   {eventListGenerated[i].endHour:00}:{eventListGenerated[i].endMin:00}:{eventListGenerated[i].endSec:00}"), 1, i, 1, 1);
                grid.Attach(new Label($"Priority: {eventListGenerated[i].priorityStr}"), 2, i, 1, 1);
            }
        }

        grid.ColumnSpacing = 10;
        grid.RowSpacing = 5;
        grid.Margin = 5;
        ContentArea.Add(grid);
        ShowAll();
    }
}

class View{
    static void Main(){
        Application.Init();
        UserNameWindow w = new UserNameWindow();
        w.ShowAll();
        Application.Run();
    }
}