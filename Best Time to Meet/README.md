# Best Time To Meet

This is the final project of CS50. This project aims to help crowded people to decide when to meet. There is a heatmap about which days are intensively chosen so that people can easily pick the best day to meet. 

You can see how app looks and how to use it by watching this **video: XXXXXXXX**


## How To Use

First of all, you need to create an account with your email address. No need to verify your address. This is just to track who is gonna be available on a selected day. 

After you create your account, you can log in and contribute an existing calendar to say that you are available on specific days. Or you can create a new calendar for your meeting. 

To open and contribute an existing calendar, you need to have the calendar ID and the password. If you choose to create a new calendar for people to contribute, you need to share the calendar ID and the password. 

After you open the calendar, you can simply select the days that you are available and save it. You can always change your selected days by opening the calendar and saving the new preferences. 

That's all! No need to worry about when to decide! Just use our application!



## How It Works

I've decided to use Google Firebase system as database infurstracture. There are 2 scenes and in both scenes there are 2 different UI Panels. 

### Scenes

The first scene is the **"LandingScene"**. Users will face this scene in the first place. This scene has Login and Register UI Canvases. In the Login canvas, users can log in and select the register option. If the users log in, they will face "PlanScene". If users select the register option, then Login canvas will disappear and Register canvas will appear. In this panel, users can create a new account. After creating the account, Register canvas will disappear and Login canvas will appear. So that the user can log in.

The second scene is the **"PlanScene"**. There are 2 canvases in this scene as well. The first canvas is the "Plan Landing" canvas which users can open an existing calendar or create a new one. After the decision, users will face with the "Plan Calendar" canvas which they will see the calendar and interact with it. 

### Objects And Classes

We have objects which do certain jobs and collabrate with each other to run the application. We have 3 main objects with their own classes and 1 more class for the users' interface's button objects.

#### **App Manager** 
Handles the navigational operations in the app. The only exceptional implementation in this object is the ability to create a new calendar ID and password and send it to the Calendar Manager object. **This object has its own class called AppManager**

#### **Calendar Manager**
Handles the user interaction with the calendar. Calendar Manager creates a calendar with given data and keeps the data. With every change in the calendar, this object creates or updates the heat map of the calendar. **This object has its own class called CalManager**

#### **Firebase Manager**
Handles the database connection and saving, loading operations. This object also handles login, logout, and registration operations. **This object has its own class called FirebaseManager**

#### **ButtonBehaviour""
This is just a class that handles user button interactions. When a button clicked it toggles the button color. So that the user can see which days he/she has selected. It also says which day selected by the Calendar Manager so that CalManager manages all the operations.