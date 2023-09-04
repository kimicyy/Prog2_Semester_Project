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

// class MyWindow : Window {
//     public string userName = "";
//     public bool userNameEntered = false;
//     Entry userNameBox = new Entry();

//     public MyWindow() : base("Calendar: User Name") {
//         SetDefaultSize(1280, 720);
//         Grid grid = new Grid();
//         grid.Attach(new Label("User Name"), 0, 0, 1, 1);
//         grid.Attach(userNameBox, 1, 0, 1, 1);
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
//         }
//         else{
//             userNameEntered = false;
//         }
//         Application.Quit();
//     }

//     void onQuit(object? sender, EventArgs args) {
//         Application.Quit();
//     }

//     protected override bool OnDeleteEvent(Event e) {
//         Application.Quit();
//         return true;
//     }
// }

// class View{
//     static void Main() {
//         Application.Init();
//         MyWindow w = new MyWindow();
//         w.ShowAll();
//         Application.Run();
//         WriteLine(w.userName);
//         WriteLine(w.userNameEntered);
//     }
// }