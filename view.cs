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

class GraphicalCalendar : Window {
    public GraphicalCalendar(string userName) : base($"{userName}'s Calendar"){
        SetDefaultSize(320, 280);
        Calendar calendar = new Calendar();
        calendar.DaySelected += OnDaySelected;
        Fixed fix = new Fixed();
        fix.Put(calendar, 20, 20);
        Add(fix);
    }

    void OnDaySelected(object sender, EventArgs args){
        Calendar cal = (Calendar) sender;
        // label.Text = cal.Month + 1 + "/" + cal.Day + "/" + cal.Year;
    }

    protected override bool OnDeleteEvent(Event e) {
        Application.Quit();
        return true;
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