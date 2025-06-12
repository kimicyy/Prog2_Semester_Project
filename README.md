# Programming2 Semester Project

## Project Title: Smart Calendar

### Brief Description:
The Smart Calendar application is designed to help users manage their events more effectively by prioritizing them, categorizing them, and generating working plans based on these priorities. The application allows users to set custom priorities for each event and view a well-organized calendar.

### Problem Formalization:
Users can provide the following information for each event:
- **Event name**: A string representing the name of the event.
- **Priority**: A string representing the priority of the event (e.g., "important", "urgent", "normal").
- **Event category**: A string representing the user-defined category for the event.

### Problem-Solving Algorithm Design:
The core of the application is a calendar object that handles the organization and prioritization of events.

#### Data Members of Calendar Object:
- **Priority Dictionary**: A dictionary where the keys are priority strings (e.g., "important", "urgent") and the values are numeric values (doubles) that represent the priority level.
- **Priority Queue**: A priority queue that organizes events based on their priority (calculated using the priority dictionary).
- **Categories List**: A list of strings representing the different categories of events.

#### Workflow:
1. **Input**: When a user adds an event, the priority string is converted to a numeric value using the priority dictionary.
2. **Queue Management**: The event is then added to the priority queue based on its priority level.
3. **Plan Generation**: If the user wishes to generate a plan, the events are ordered according to the priority queue and displayed.
4. **Display**: The events and generated plan are shown using GTK3's model-view architecture.

### Input and Output Formats:
- **Input Format**:
  - **Event name**: String (e.g., "Team Meeting")
  - **Priority**: String (e.g., "important")
  - **Event category**: String (e.g., "Work")

- **Output Format**:
  - A calendar object reflecting the user's events, priorities, and categories. The calendar will display a plan if requested.

### User Interface:
- **Interface Type**: GTK3
- **Interface Description**:
  - A main window displaying the calendar and all events.
  - Buttons to input new events, generate the working plan, and update the calendar.
  - Toolboxes for managing event priorities and categories.

### Interactivity:
- Users can add, update, or delete events, and the calendar will automatically update to reflect these changes.
- The user can generate a plan based on the event priorities and categories.
