using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Each interaction you do with an object you should write to a Stack (similar to the list but more suitable for this kind of situation). Therefore you should create some class for it, name it "interaction" for example ;) 
//There can be some abstract class with a couple of methods or an interface. Then you have specific classes for different types of interactions, for example, translateInteraction, scaleInteraction, rotateInteraction, changeColorInteraction and so on.
//They all have a method called "Undo" for example, plus some state to which they should return, this state is registered before interaction occurs (there is an interaction manager or something, who trigger saving the state for each interaction). 
//You can store interaction stack inside the interaction manager for all interactions or separately for each object to manipulate depending on your need. Again, each time you interact with an object, the interaction manager creates new interaction instance object of a given type and store it into the stack, once you activate "undo" function, you pop the last element from the stack and use it for undo. If you use the single stack for all objects in the scene, your interaction records should also have a pointer to the object for which to apply the undo. 
public class Interaction
{
    
}
