# GenericFeetPlacement
Feet placement script using the Animation Rigging package. It works with generic rigs, including humanoids, animals, monsters, fish, etc
# Setup
1. Install Unity Animation Rigging package. <br>
![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/0d4907ce-eeef-4a79-9d48-74f43921366f)<br>

2. Separate the Model from the logic (Create a container for the Model)<br>
 ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/29329944-3b5b-4e11-a1d3-1f4acb571738)<br>
  
3. Add Rig to character (change local location to [0, 0, 0] if it's not already the case).<br>
   ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/38e31795-1c24-40f8-9f1e-18934cf9abb7)<br>

4. Add an Empty GameObject for each leg (don't do it if your model lacks legs or if they are so small that you don't care about them).<br>
  ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/a0474180-1338-456c-810d-7c5f24b713b8)<br>

5. Add ExtractTransformConstraint Component and TwoBoneIKConstraint (ExtractTransformConstraint HAS TO BE above TwoBoneIKConstraint because order matters)<br>
   ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/84fa4e91-c2a0-4deb-9a57-e86012265a6c)<br>

6. Add Targets for gameobjects (dont need to move them anywhere)<br>
   ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/f721df56-2f1f-4ad3-a4c5-52c549819a48)<br>

7. Assign bones (the tip in TwoBoneIKConstraint should be the same as bone the ExtractTransformConstraint).<br>
  ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/3beecf6b-c6d3-4306-a763-eb62b5696fb6)<br>

8. Add GenericFootIK Script to Model<br>
    ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/db0551bd-a7f3-4918-9b8f-9c153322548b)<br>

9. Assign the Root (a GameObject that's the parent of our model but doesn't have game logic / or just use our model) and the Targets (Targets and not GameObjects with TwoBoneIKConstraint component)<br>
    ![image](https://github.com/Kam-Tom/GenericFeetPlacement/assets/120057104/e987dac4-4100-4eb2-b093-a1ee1d2d2480)<br>
<br>
**Done**
Play with values or choice of bones if needed
