# FlightMobileApp

Link to [Repository](https://github.com/kfiryehuda/FlightMobileApp)

## Intro
In this Project we will build an Android app that will control an airplane via the Flight Simulator. 
Our application will communicate via an HTTP protocol to an intermediary server, which will send our requests to the simulator. 
The broker server will connect to only one simulator, so it will only be able to serve one client. 
Its purpose is to enable standard communication between the client application and the server through known Web protocols. 
The Android app will be written in Kotlin language, and the intermediary server will be implemented in #C using ASP.NET Core technology.

## Android App
The application consist of two views, and work as well horizontally or vertically, depending on the user's wishes. 
When the application is opened, a login screen will be displayed, where we can enter the server information that we want to connect to. 
Once logged in, a control screen will be displayed that will include the main rudders of the aircraft (Throttle, Rudder, Aileron Elevator) as well as an image from the simulator screen.

## Login view
1) the view contains a field for entering the URL of the broker server, login button, And a list of unique server addresses that we have connected to in the past.
2) The list will hold a maximum of five different addresses from each other, and will be sorted in descending order according to the last login time of the app for each address.
3) Clicking on an address from the list of addresses will fill in the address field (shown at the bottom right) in the clicked address in the list, and will save the typing to the user.
4) Clicking the Login button will update the last usage time of the address selected to be the current time (even if the actual login itself failed).
5) The connection is successful when we first manage to get an image from the simulator through the intermediary server.
6) If the connection is successful, go to the next screen, the control screen. 
7) If the connection fails, a suitable message must be displayed to the user and remain on the login screen.

## The control display
1) The display is divided into two halves. In the upper half will appear an image from the simulator screen, and in the lower half the main steering wheels.
2) When rotating the device horizontally, make sure that the image and the rudders are in the same configuration, and that the steering wheel is responsible for the "up-down" movement when the device is in the vertical position, which is also the same rudder in the horizontal position.
3) The rudders for the aircraft are given in the image. 
4) In order to reduce the load on The server and the network consumption of the application, should be sent an appropriate command only when there is a significant change of one of the rudders. That is, one value (or more) of the parameters has changed by more than 1% of its allowable range. The change is measured from the last value sent to the server.
5) The joystick can be realized by drawing two circles, one inside the other. The inner circle is the "handle" of the joystick while the outer circle is the possible area of ​​action of the handle.
6) The joystick (inner circle) is at rest in the center of the outer circle. The center of the joystick can be dragged anywhere inside the outer circle.
7) The value of the steering wheel will be calculated as the relative distance traveled by the center of the joystick from the center of the outer circle. For example, if the center of the joystick is in the middle of the horizontal path between the center of the right circle and its right border, the aileron value will be 0.5
8) When we release the joystick a light animation will be activated that will put it back in place at rest to the center of the outer circuit, and of course send a message to the server with the new value of the rudders.
9) The picture should be updated every 0.1 seconds. When the display screen is not visible, stop updating the image and contact the server, and renew the update and refer to the server when the display screen returns to visibility.
10) If communication with the broker server runs into difficulties, or the broker server returns error messages
If there is a problem with the simulator itself, the user should be offered to go back to the login screen. This means that a distinction must be made between current problems such as an incorrect format, and different network modes, for example: that the server or simulator disconnects, or does not respond for more than 10 seconds.
11) Clicking "Back" on Android will take the user to the login screen.

## The server Api
1) The server responds to client requests using the HTTP protocol and is built according to the REST principles we have learned.
``` 
POST /api/command
```
We will define a single resource, a Command type object, and we will refer to it using the POST command when the message body is in Json Format.
``` 
￼￼￼￼￼￼{
"aileron": 1.0, "rudder": -1.0, "elevator": 0.5, "throttle": 0.3
}
```
2) The server response to the app will only be Appropriate Code Status (Success 200, or Failure of the appropriate status).
3) In addition, the server supports another command: GET / screenshot that returns the screenshot that is currently visible in the simulator in .jpg format
4) Assume that the simulator is running externally, and the server should not turn it on or off. 
5) In order for the simulator interface to be compatible with what you did in the first exercise, in the initial connection of The server to the simulator has to send the data command \ n The command will cancel the normal PROMPT, and return only the numeric value (encoded as a string at base 10) for get commands
6) The server addresses the simulator in the TCP channel with appropriate SET commands with the full path of the steering wheel required to be changed. For example for aileron the command should be sent in TCP channel:
``` 
set /controls/flight/aileron 0.5 \n
``` 
7) The server must verify that the value that exists in the simulator matches the value sent and that no fault occurs.
8) The value returned from the simulator in get commands is in the following format. Set commands will not return a value.
``` 
0.5
``` 

9) All strings to and from the simulator are encoded using ASCII
10) If there are slight changes in the format in front of the full simulator (FlightGear), the practitioner should be updated. 
11) The simulator runs an internal HTTP server which allows us to get the image from the screen by accessing 
``` 
get HOST:PORT/screenshot 30
``` 
12) The simulator uses 2 different ports for communication. One for the internal HTTP server and the other for listening To socket for set / get commands.
You must make sure that these two ports can be configured on your server, as well as the IP address of the host on which the simulator is running.
13) FlightGear documentation should be consulted to enable the simulator's internal HTTP server. 
14) You can make sure that your server works as expected if you refer in chrome to the address of your server for example:
``` 
localhost:5000/screenshot
``` 
and make sure that the browser displays an image on the client client from the running simulator In the background.

## Technical requirements
1) The server will be built on the basis of NET Core 3.1 WebAPI. 
2) The app will be written in Kotlin language. 
3) Use the room directory to store the URLs in the application's internal database. 
4) A retrofit library should be used to make HTTP calls. 
5) IO inquiries should be made using co-routines.

## Handling failure cases
Various failure cases should be prepared for.
If the application detects a failure case, a message should be displayed to the user that does not interfere with his experience.
