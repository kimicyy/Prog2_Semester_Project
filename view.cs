// using Gdk;
// using Gtk;
// using Window = Gtk.Window;
// using static Gtk.Orientation;
// using System;
// using System.Collections.Generic;
// using System.Text.RegularExpressions;
// using System.IO;
// using System.Linq;
// using static System.Console;
// using static System.Math;

// class UserNameWindow : Window{
//     public string userName = "";
//     public bool userNameEntered = false;
//     Entry userNameBox = new Entry();

//     public UserNameWindow() : base("Calendar: User Name") {
//         SetDefaultSize(320, 280);
//         Grid grid = new Grid();
//         grid.Attach(new Label("Welcome to Smart Calendar!"), 0, 0, 3, 10);
//         grid.Attach(new Label("User Name"), 0, 10, 1, 1);
//         grid.Attach(userNameBox, 1, 10, 1, 1);
//         grid.ColumnSpacing = 10;
//         grid.RowSpacing = 5;

//         Box row = new Box(Horizontal, 5);
//         Button enter = new Button("Enter");
//         row.Add(enter);
//         enter.Clicked += onEnter; 
//         Button quit = new Button("Quit");
//         row.Add(quit);
//         quit.Clicked += onQuit;

//         Box vbox = new Box(Vertical, 5);
//         vbox.Add(grid);
//         vbox.Add(row);
//         Add(vbox);
//         vbox.Margin = 5;
//     }

//     void onEnter(object? sender, EventArgs args) {
//         userName = userNameBox.Text;
//         if (userName.Length > 0){
//             userNameEntered = true;
//             var graphicalCalendar = new GraphicalCalendar(userName);
//             graphicalCalendar.ShowAll();
//             Destroy();
//         }
//         else{
//             userNameEntered = false;
//         }
//     }

//     void onQuit(object? sender, EventArgs args) {
//         Application.Quit();
//     }

//     protected override bool OnDeleteEvent(Event e) {
//         Application.Quit();
//         return true;
//     }
// }

// class GraphicalCalendar : Window{
//     public User user;

//     public GraphicalCalendar(string userName) : base($"{userName}'s Calendar"){
//         SetDefaultSize(320, 280);
//         Calendar calendar = new Calendar();
//         calendar.DaySelected += OnDaySelected;
//         Fixed fix = new Fixed();
//         fix.Put(calendar, 20, 20);
//         Add(fix);
//         this.user = new User(userName);
//     }

//     void OnDaySelected(object? sender, EventArgs args){
//         Calendar cal = (Calendar) sender;
//         using (AddEventDialog d = new AddEventDialog(this, cal.Month + 1 + "/" + cal.Day + "/" + cal.Year))
//             if (d.Run() == (int) ResponseType.Ok){
//                 ComboBoxText[] comboBoxes = {
//                     d.priorityBox,
//                     d.fromHourBox,
//                     d.fromMinuteBox,
//                     d.fromSecondBox,
//                     d.toHourBox,
//                     d.toMinuteBox,
//                     d.toSecondBox
//                 };
//                 if (d.eventNameEntry.Text.Length <= 0){
//                     return;
//                 }
//                 foreach (var box in comboBoxes){
//                     if (box.ActiveText == null){
//                         return;
//                     }
//                 }
//                 UserEvent e = new UserEvent(d.eventNameEntry.Text, d.priorityBox.ActiveText,
//                                             cal.Year, cal.Month + 1, cal.Day, 
//                                             int.Parse(d.fromHourBox.ActiveText),
//                                             int.Parse(d.fromMinuteBox.ActiveText),
//                                             int.Parse(d.fromSecondBox.ActiveText),
//                                             cal.Year, cal.Month + 1, cal.Day,
//                                             int.Parse(d.toHourBox.ActiveText),
//                                             int.Parse(d.toMinuteBox.ActiveText),
//                                             int.Parse(d.toSecondBox.ActiveText));
//                 // user.userCalendar.addUserEvent(e);
                
//             }
//     }

    
//     protected override bool OnDeleteEvent(Event e) {
//         Application.Quit();
//         return true;
//     }
// }

// class AddEventDialog : Dialog{
//     static string[] priorityLevel = {
//         "Urgent",
//         "Normal",
//         "Trivial"
//     };

//     public ComboBoxText priorityBox = new ComboBoxText();
//     public ComboBoxText fromHourBox = new ComboBoxText();
//     public ComboBoxText fromMinuteBox = new ComboBoxText();
//     public ComboBoxText fromSecondBox = new ComboBoxText();
//     public ComboBoxText toHourBox = new ComboBoxText();
//     public ComboBoxText toMinuteBox = new ComboBoxText();
//     public ComboBoxText toSecondBox = new ComboBoxText();
//     public Entry eventNameEntry = new Entry();

//     public AddEventDialog(GraphicalCalendar parent, string dateStr) : base(dateStr, parent,
//             DialogFlags.Modal, "Add", ResponseType.Ok, "Cancel", ResponseType.Cancel) {
//         SetDefaultSize(560, 300);
        
//         foreach (var priorityStr in priorityLevel) {
//             priorityBox.AppendText(priorityStr);
//         }
//         for (int i = 0; i < 24; i++){
//             string text = "";
//             if (i < 10){
//                 text = $"0{i}";
//             }
//             else{
//                 text = $"{i}";
//             }
//             fromHourBox.AppendText(text);
//             toHourBox.AppendText(text);
//         }
//         for (int i = 0; i < 60; i++){
//             string text = "";
//             if (i < 10){
//                 text = $"0{i}";
//             }
//             else{
//                 text = $"{i}";
//             }
//             fromMinuteBox.AppendText(text);
//             fromSecondBox.AppendText(text);
//             toMinuteBox.AppendText(text);
//             toSecondBox.AppendText(text);
//         }
//         Grid grid = new Grid();
//         grid.Attach(new Label("Event Name:"), 0, 0, 1, 1);
//         grid.Attach(eventNameEntry, 1, 0, 1, 1);
        
//         grid.Attach(new Label("Event Priority: "), 0, 1, 1, 1);
//         grid.Attach(priorityBox, 1, 1, 1, 1);

//         grid.Attach(new Label("From: "), 0, 2, 1, 1);
//         grid.Attach(fromHourBox, 1, 2, 1, 1);
//         grid.Attach(new Label(":"), 2, 2, 1, 1);
//         grid.Attach(fromMinuteBox, 3, 2, 1, 1);
//         grid.Attach(new Label(":"), 4, 2, 1, 1);
//         grid.Attach(fromSecondBox, 5, 2, 1, 1);

//         grid.Attach(new Label("To: "), 0, 3, 1, 1);
//         grid.Attach(toHourBox, 1, 3, 1, 1);
//         grid.Attach(new Label(":"), 2, 3, 1, 1);
//         grid.Attach(toMinuteBox, 3, 3, 1, 1);
//         grid.Attach(new Label(":"), 4, 3, 1, 1);
//         grid.Attach(toSecondBox, 5, 3, 1, 1);

//         grid.ColumnSpacing = 10;
//         grid.RowSpacing = 5;
//         grid.Margin = 5;
//         ContentArea.Add(grid);
//         ShowAll();
//     }
// }

// class View{
//     static void Main(){
//         Application.Init();
//         UserNameWindow w = new UserNameWindow();
//         w.ShowAll();
//         Application.Run();
//     }
// }