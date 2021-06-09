## Introduction Panel

The **IntroCanvas** prefab is used and customized in each scene to show the intro text. The main script on this prefab is **UIManager.cs**, which has code for proceeding and pulling up the notes.

The **NotesDisplay** prefab is used for the in-game notes.

The **Goals** prefab contains all entries in the checklist. The checklist entries are instances of the **GoalPrefab** prefab.

## WebSocket Handling

WebSocket connections are handled in **WebSocketHandler.cs**, which is attached to a **WebSocketHandler** prefab. This script starts up the connection defined by **SOCKET_URL** and has a SocketMessageEvent that is invoked after receiving a response. This event passes back a **SocketResponse** object which contains the scene name, action, and value of the response. The **SocketResponse** is created by parsing the JSON string coming in from the WebSocket.

**PlayCloudAudio.cs** listens to the SocketMessageEvent for "playaudio" actions then goes through the process of fetching the audio file and playing it along with an animation to show the character speaking.

Similarly, **ChangeSceneListener.cs** listens only for "changescene" actions and changes to the given scene. This is attached to a **ChangeSceneListener** GameObject in the scene.

## Intro Transitions

Intro transitions are achieved using Timelines and PlayableDirectors. The timelines are located in **Assets > Timelines**.

In the garage scene, the Camera object has the PlayableDirector and a script named **StartTimeline.cs**. The "TransitionStart" GameObject has a MouseDownHandler and BoxCollider, which is turned on when clicking on the door. This is achieved through the **MouseDownHandler.cs** script on the same object. MouseDownHandler.cs also contains an event that's invoked when clicking on a collider. In the garage scene, it plays the timeline then deactivates the TransitionStart object so it cannot play it again. The collider is initially activated through an event invoked when exiting the IntroCanvas, found in UIManager.cs.

The doctor scene has a similar setup. Here, the two objects controlling the transition are "Camera" and "TransitionStart".

When making intro transitions, ensure the object you're moving in the Timeline **is not marked static**. If it's marked static, it will not move in play mode or in a build.