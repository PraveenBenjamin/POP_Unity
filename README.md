# POP_Unity
POP! The plight of our peeps, UKEN challenge submission

DISCLAIMER :- THIS GAME DOES NOT REFLECT MY THOUGHTS OR OPINIONS. IT IS CREATED IN JEST AND THAT IS ITS ONLY INTENTION.
IT IS ONLY MEANT TO SHOWCASE MY EXPERIENCE WITH PROGRAMMING AND UNITY.

Music :- https://www.bensound.com/licensing
Images :- https://creativecommons.org/licenses/by-sa/2.0/deed.en
Fonts :- dafont.com


Concept :-

POP! the plight of our peeps, is a game about animal politics. the leaders of 5 parties are competing in an election, but who will win?
That is where the player comes in. It is your task to bring together the peeps of different sides to reach a consensus.

The game is a variant of the simple formula employed by traditional match 2 games.
The goal of the game is to match as many of the animals as one can by clicking them in sequence, but matches are only possible
if the animals are close together (including diagonals). If a match is not made, the clicked animals swap places.
So the gameplay consists of swapping the animals to the right positions before matching them. But be careful! one mistake and 
you wont be able to clear the grid!

But as is true with everything else, life moves on... and the animals will stand for election again!

The concept of the game is to show that, as with everything else, there is no clear choice. If you clear the grid, all the parties
will have an equal number of votes, and nobody wins. But if you ensure that your party wins, you are stacking the deck
against everyone else, and you are willfully preventing yourself from scoring... but isnt that true with actual politics?

And who is to say that your chosen candidate adheres to what is morally right? who will to turn to for clarity? the media? 
The media is obviously "influenced" by the competing parties right? Should you take what is told to you (in billboards i might add :P) at face value?

So what will you do? who will you be voting for? who will you let win? and is that truly the right candidate?

Or you can refrain from pondering about all those things and just play the game :P


To reviewers:-
If i were to pick any 2 classes out of the ones i wrote in this project for you to look at, i would ask that you 
looked at my implementation of the finiteStateMachine (called POP.Framework.FSM<T>) and my implementation of the
SingletonBehaviour (POP.Framework.SingletonBehaviour<T>)

I would like to state that I could have added many more things to the project to make it more scalable if i had the 
time, but as it stands, given the timeframe, I believe i did what was asked of me to specification, if not more.

Finally i realize that my code doesnt have too much documentation. But that is truly because of how simple it is to follow.
I made sure I documented the parts i felt were slightly less intuitive.


ColorPallete :-

InGame :-
Red = tru cat E20000
Green = putin lion 00CC00
Blue = un chi 314FFF
Black =modi do 6F6F6F
White = hill pen FFFFFF

UI :-
E0FFDA, A7B6FF


Commentary during development :- 13/09/2019, punched in at 8:00 am
9:27) Started out by adding overarching framework classes and setting up a skeleton architecture
10:34) adding/ removing and modifying said classes as the project progresses
12:38) Since im the only programmer im not really focused on logically seperating the development into git branches. 
12:38) It feels wonderful to be programming again :D
12:58) BTW, im writing everything from scratch, despite me having plenty of previously done work to draw from, so
that you guys can get a better feel for how i work and how long i take
2:20) Lunch
2:24) back at it, dealing with basetransition atm
4:21) Added the framework components for most of the menus, building the flow from the splashscreen->Mainmenu->LevelSelect->Gameplay
will begin writing gameplay next
5:51) started gameplay architecture about 20 mins back. Calculating positions of actors on the grid now.
7:05) almost done with the base architecture of the game, will be finalizing the flow before EOD
7:37) EOD, completed the basic architecture and overall flow of the game, tomorrow will be debugging the architecture and completing implementation sans UI

14/9/2019 :- punched in at 7:24
7:45) hard at work implementing and debugging yesterdays architecture
9:21) Breakfast break
9:37) Back at it, finalized baseTransitioner and moving onto LevelSelect implementation
10:18) Finished basic implementation of LevelSelectMenu
10:47) Finished basic implementation of InGameMenu and PauseMenu
12:39) Started gameplay implementation about 30 mins back, debugging and finalizing its flow now
2:15) Finished game flow. Will apply a tiny bit of polish before moving on
2:25) Lunch
2:52) Back at it
4:51) Spent the last 2 hours working on the design. Continuing to do so
6:59) Still in the design phase, but its coming along nicely, just picked out a background track
7:55) Making the billboard function now, the only thing after that is to replace the dev UI with a better one.
Then its off to deployment! woohoo!
10:07) trying to find the right assets :O, had plenty of luck but need plenty more
11:53) tbh, i dont wanna stop o.O this honestly is fun. Im just worried about not getting enough sleep and missing the deadline tomorrow :/
12:30) So close to finishing i can almost taste it. stuck waiting for a font atlas to generate :/
12:31) Finished most of the UI, only the gameOverscreen and pause screen to go.
2:39) Game completed. Documentation and polish are the only things left :)
3:25) fixing minor bugs till now. EOD.

15-09-2019 :- punched in at 7:23
Today is for bug fixing if any, deployment, cleanup and documentation
8:59) First build, fingers crossed o.O
11:17) started code documentation
11:48) Kinda dont like the font. Looking for a new one
12:52) Finalized the design. The only thing left is to get feedback from my peers.
1:12) building the apk now. will share with my family and friends and get feedback next.
3:09) Done. All that is left is to deploy one last time, test, and push
