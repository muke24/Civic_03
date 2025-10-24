This project was for the headunit of my car.

I successfully added a police detector inspired from NFSMW 2005 which prompted me to make the real thing - https://github.com/muke24/Alert-Detector

This will work within the editor. The compass direction and location is simulated, meaning you can set it to where ever you like.

------------------------------

To use the police / alert detector:
- 1: Open the "Civic" scene.
- 2: Select the "Alerts" GameObject.
- 3: On that GameObject is the AlertReciever script, you can change: Max Distance, Check Interval (seconds), Location (Longitude, Latitude) and direction.
- 4: Click play! You can see ALL Waze alerts with their subtype within the "CurrentAlerts" array.

Keep in mind that you can rotate the camera by dragging the screen, this will change the offset direction that the arrow will point at if there is an Alert.

Unity 2019.2.21f1 is used as it was the newest version that would work on my Android 4.2 headunit.

------------------------------

<img width="1917" height="1031" alt="image" src="https://github.com/user-attachments/assets/0f4e6d6a-4c01-4366-badf-b51e2456e561" />
