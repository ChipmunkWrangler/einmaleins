[ToDo](ToDo)
[StoreStuff](StoreStuff)


#Ideas 
* After successful Apple submission, submit another version with less debug stuff, step by step, as 1.1
* If a player enters e.g. 61 instead of 16, animate transposing the numbers instead of normal failure
* Keep track of longer-term question difficulty after all; if a question keeps getting answered wrong, show it several times in a row until it is answered right, then several times in the same quiz until it is answered right
* Play button is pixely in player selection screen
* Add bullet point list of cool things to your app store presence
* Add "Rate this app" popup
* Check texts with Pete or John & Angie
* Remove unnecessary permissions, e.g. Android.Internet aka "Other: full network access"
    * Note that in the .csproj files, there are defines like ENABLE_CLOUD_SERVICES_ADS 
* iPhone ad video https://stackoverflow.com/questions/25797990/capture-ios-simulator-video-for-app-preview
* Count three wrong answers as hitting the question mark button, maybe flash the button
 * Second time an answer is wrong twice, tutorial for question mark
* Ion stream doesn't look impressive
* Optimize: don't save all questions every time a question changes -- just the relevant question
* Make it free and charge for everything above Jupiter.
* Readd multiplier stars or some other kind of combo bonus
 * Maybe with new currency
 * Or just escalating particle effects
* Stop the quiz if the player accumulates 11 rocket parts
* At first, show "Press rocket button to launch" instead of Mars ho.
* Handle kids that only know 5x tables
* No one understands fuel
* Difficulty levels and or voice control due to motor control issues
* Make first rocket upgrade really fast
* Longer-term motivation
  * More involvement with rocket building
   * Collect currency for every correct question, regardless of speed
   * spend currency to:
    * Change rocket color again
    * Unlock and change flame color
    * Unlock and switch between alternate flame effects 
    * Unlock and switch between different rocket designs
     * Use Inkscape to split your rockets into pieces that you can build.
     * Colouring book for rocket
   * Existing players still start from 0 currency 
   * Upgrades are shown in advance for motivational reasons
  * random events to keep things interesting if there hasn't been a record or rocket part for a while
  * More collectibles
   * Introduce new elements gradually
   * Planet gallery 
   * Final congratulation with magic tree
   * "Stickers" for the rocket
* Add 0 and division.
  * That gives 55 + 90 + 10 = 155 questions, which is divisible by 5 and 31
  * We could add the sun, venus, mercury, the iss and the moon. However, that still means we average 15 questions per upgrade.
  * Add Ceres, Salacia (https://en.wikipedia.org/wiki/120347_Salacia), https://en.wikipedia.org/wiki/Eris_(dwarf_planet), https://en.wikipedia.org/wiki/Haumea , https://en.wikipedia.org/wiki/(225088)_2007_OR10, https://en.wikipedia.org/wiki/Makemake, https://en.wikipedia.org/wiki/50000_Quaoar, https://en.wikipedia.org/wiki/90377_Sedna, https://en.wikipedia.org/wiki/90482_Orcus. Some of these have no decent pictures, but there is https://en.wikipedia.org/wiki/File:EightTNOs.png . Then we have 19, for an average of 155/19 = 8.2 questions per upgrade. That sounds good.
* Competitive aspects
  * Show record lines of other players
  * Average answer speed 
  * Ghost of other player, either their last run, or how they did with your question set
* Music
  * Classical? 
* Use blender to fix texture or try 2d rocket
* Tutorial Tips
  * The first time that a player gets a question wrong twice, explain the give up ("hint") button
  * Show feedback on a wrong answer so the player knows it was wrong
   * Text
   * red flash
   * Tutorial tip initially, then nothing later on 
  * Show "the faster you answer, the higher you go", e.g. if players fail to break their record for three times, or perhaps answer no questions fast after the first quiz.
  * If the right answer is entered and OK isn't pressed in 5s, pulse it or show finger
* Algorithm 
  * Launch code also for normal wrong answers if there weren't any give ups
  * A new question each day
  * If a question is answered within allotted time, don't ask it again until tomorrow
  * If frustration is high, the hardest already mastered question is asked, or the easiest nonmastered if none are mastered.
   * If the easiest nonmastered has avg time > 15, ask the easiest new question instead. 
    * The current version tends to ask the same questions over and over, which may be sensible but is dull. This tweak might add a little more variety, as might decaying old answers (see below) 
  * Start with 1xN, the go to 2xN if two 1xNs in a row are answered quickly, etc. This may make progress steadier, and / or the beginning more rewarding (not too hard for beginners, not too boring for advanced)
  * Timestamp answers in the answer list and decay them 
   * This can cause a mastered question to become unmastered
   * But try not to make it frustrating
  * Add randomness to deep space question choice, since otherwise people may repeatedly get the same questions in a row
* Add unlimited accounts, or the ability to erase an account with parent mode
* Info-screen or overlay that explains fuel, etc.
  * Remove fuel and height indicators? 
* Serialization 
  * Add savegame per-player checksum, replace savegame with old copy if checksum fails because save was corrupted
  * Check where you call Save. Should happen at end of each quiz and on scene change, probably nowhere else.
  * Reduce Save() calls or find an alternative
  * On Android PlayerPrefs are stored in the app's private data folder. This folder gets deleted when the user uninstalls the app. They are stored as an XML file. Using PlayerPrefs.Save() to ensure they are commited to storage is a blocking call on the main thread. If you save more than a few bytes of data with that you'll get a noticable hang. You can't call this method from a different thread to avoid that. Apart from that, they are super reliable. 
* Ich wünsche mir Stimmeingabe, but others are opposed
* Make horizontal version with linear button array 
  * Not yet. Amalia is the only person to want horizontal so far, and Bert the only linear
  * Plus, linear arrangement of more than six buttons makes them too small. 
* "Keep going anyway" button at end of day?
* Shrink height numbers
  * Highlight the first few digits
  * Change unit (maybe write "Millionen km", or AU, or Mm
  * Or remove it entirely, since the height and record are also visible on the playfield
* Remove stats screen or make it more prominent
  * Add a big "View progress" above "Launch" button instead of little stats screen button 
  * Fade mastered cards one at a time, show flying "Mastered" text
* Avoid delay after answering first question by creating and saving questions asynch during rocket building
  * Also QuestionPicker only needs to save one question, not all questions, in OnAnswer 
* Cartoony - low poly: 
  * https://www.pinterest.de/pin/464081936581196391/
  * http://www.freeiconspng.com/img/40799
  * https://www.assetstore.unity3d.com/#!/content/72089
  * https://www.assetstore.unity3d.com/#!/content/34894
  * btw. könntest Du auch die Grafiken in Magica Voxel machen. Kannst Du dann auch sehr simple in Einzelteile zerlegen und Stück für Stück zusammenbauen
* Matthias wünscht sich Schrifteingabe
* x3 and x2 resolutions for backgrounds
* Add tracking of questions posed & answered to see how people play it
* Parent mode
  * Delete account
  * Reset account
  * Change difficulty level
   * With option to reset flash progress and upgrades, reimbursing rocket parts 
* Teach math, don't just drill (see Hints, Tricks and Number Sense) 
* bigger rocket on icon (use xcf in unusedassets as base)
* If you want to make it free and sell it after you have reached Mars: https://developer.apple.com/app-store/parental-gates/
* Explain the slow last round based on it being a cargo ship for the tree
* More education: See Hints
* More juice
  * https://www.reddit.com/r/Unity3D/comments/2i1281/adding_juice_a_question_about_squishing_and/
  * Highlight getting a record, keep it yellow
  * Split saturn into two layers (front of rings & (planet + back of rings) so you can go through the rings
   * Ensure that shadowed, missing part of ring is black, not transparent 
  * Rotate rocket as it rises
  * Fuel tank upgrade changes the shape of the rocket 
  * https://forum.unity.com/threads/juicebox-add-some-juice-to-your-game.267081/
  * Scene transition in OnPlay (rocket with name on the side?)
  * Celebrate the instant the record is broken in Launch mode: special flames?
  * Screen shake on launch, or on every thrust
  * Score header more visual 
   * Use a rocket as rocket parts progress bar instead of a number
    * And an engine for upgrades? 
   * Fuel is a bar rather than a number? 
   * But the images may visually dominate the height and record lines
* Switch from PlayerPrefs to another system https://unity3d.com/learn/tutorials/topics/scripting/persistence-saving-and-loading-data
* Optimize graphics:
   * https://gamedev.stackexchange.com/questions/75376/why-does-unity-in-2d-mode-employ-scaling-and-the-default-othographic-size-the-wa 
   * https://docs.unity3d.com/Manual/Profiler.html
   * http://answers.unity3d.com/questions/1009179/why-a-texture-always-consume-twice-ram-when-runnin.html
   * http://answers.unity3d.com/questions/401716/determining-texture-memory-usage.html
* Matthias B says: consider visual flow: ATM it starts at the question, then down to the buttons, then up to the answer, down to the rocket, up to the result... he thinks it would be better to go from top to bottom or vice versa

##Collectibles
* Get a new alien for each planet reached
  * Or a plant
  * Or at least have a collection screen for the planets themselves 
* Award a collectible animal when a skill is mastered
* Upgrade the animal or award another when a skill is flash mastered
* That is 100 - 400 animals (double if 6x8 is not 8x6 -- maybe cheat by adding the same animal in a different color)!
  * That is too many to appreciate anyway.
  * Maybe an animal for mastering all the x1s, all the x2s, etc. That would be 10 - 20.
  * And for each skill, it just lights up gold in the bingo card?
* After they get the animal, they have to feed the animal:
  * It starts to get hungry when they need to review its skills
  * They can't force the right exercise to come, but they know it will come soon after the animal gets hungry if they play. 
##Algorithm
* Consider adding a 15 minute cutoff regardless of number of errors
* Have the option to do extra review in LRU order.
* The intervals published in Pimsleur's paper were: 5 seconds, 25 seconds, 2 minutes, 10 minutes, 1 hour, 5 hours, 1 day, 5 days, 25 days, 4 months, and 2 years.
* If there are no cards left to drill, and there is a new card available, and (you have spent less than 5 minutes in the app or have fewer than three mistakes), add a new card.
##Presentation
* Present the questions both visually and orally
* Show untimed multiplication as a square of dots (or animals), maybe initially marching out in rows
  * http://www.mathplayground.com/tb_multiplication/thinking_blocks_multiplication_division.html
  * These blocks can be arranged on a 10x10 square, trying to fill it as much as possible 
  * Or pizzas with pepperonis
* Present the questions and answers in different formats and get children to say which are equivalent, e.g.
  * 6x7, 7x6, 56, a 7 by 6 square, 7 pizzas with six pepperonies each
  * maybe even 3x2x7, 7x7-7, 6x6+6, 5x6+2x6, 5x7+1x7 (and their visual equivalents) 
  * and 7 x ? = 42
  * for Nx8, also Nx10 - Nx2
* Clock solitaire http://www.timestableclock.com/index.html
* Show a bunch of numbers and say "which of these are multiples of N"
* Mnemonic stories: http://www.multiplication.com/student-manager/premium/video-player/84
##Games
* Maybe a minigame to play with the collected animals?
* Most math drill games are a game (some simple, some complex, like Tower Math) interrupted by a math question. Sometimes this feels more plausible than others (e.g. tower defense works better than shooter). Maybe you could have something where buying is a main mechanic, and the price is always given as N x M.
* The alternative is a freely playable game for which you win tickets by solving problems
* Is there anything better? 
  * A geometry game where knowing N x M is intrinsically important?
  * A lockpicking or safecracking game where the combination is the result
  * A hacking game where you have to disable drones before they hit your city. The sooner you do it, the less destruction.
  * Launch code for rocket
  * Coordinates of moon or stargate (could even offer a choice of problems (all due) to go different places or directions)
  * Alas, the best endless runners are based on flow, which you would interrupt with your questions. So maybe separate it: Play cube race or doodle jump for earned ticket; otherwise play booster rockets which is just solving the math (a glorified progress meter) for high scores. 
* General problem: You can only sensibly allow the game to be played when at least one fact is due. But then the game might be very short, or if you show all facts, encourage wasting time by reviewing things already known, probably at the expense of the other modes. 
###Battleship
Battleship where the grid has numbers for x and y and you make your attack by entering x times y and receive a shot by decording x times y.
agar.io where you can eat any equation smaller than you and run from those who are larger

###Doodle Jump
Doodle jump / cube racer thing where the powerups are digits. Maybe write the question on your ship. Make it easy to hit the digit you want (make it lane-based, slow ship down when digits come) -- if you want, you can include normal obstacles at normal speeds as well.
####Musings
* However, this means that the right answer must be repeated, making it recognizable unless all answers are repeated
   * Also, I don't really like multiple choice
    * Could make them fly through individual digits -- that would solve both problems 
   * And the player could hit the wrong answer through lack of coordination
    * Maybe instead of "this number or that" it could be "number or not", and it is easy to control, e.g. only two lanes and plenty of time to switch 
##Hints
* Always show 5x8 and ask for 40, but if the kid gets it wrong or takes too long, walk them through some tricks (or ask them how they did it)
* Base hints on player mastery -- if we know they know 6x6, use things based on 6x6.
##Tricks
* 5s end in 5 or 0
* 10s end in 0
* 9s digits add up to 9, and the first digit is one less than the multiplier (3x9 = {(3-1),(10-3)})
* 3s digits add up to 3, 6 or 9 (but this is more useful for other stuff)
* Songs or jingles?
* Focus on 1, 2, 3, 4, 9, 10. Then you only really have to memorize 6, 7, 8 by 6, 7, 8
* Hand trick 
* Take the nearest 5x and add
* Take the nearest 10x and subtract
* Remembering squares is useful because whenever you multiply numbers that are separated by two you can simply square the middle number and then subtract one to get the answer.  5x7=6x6-1
* Rhymes http://www.teacherweb.com/NY/Quogue/MrsLevy/MULTIPLICATION-RHYMES.pdf

##Number Sense
 Montessori Bretter aus Kuegelchen, "Angeregt von Montessori"

https://bhi61nm2cr3mkdgk1dtaov18-wpengine.netdna-ssl.com/wp-content/uploads/2017/03/FluencyWithoutFear-2015.pdf
https://www.mathsolutions.com/documents/NumberTalks_SParrish.pdf (for 2 digit multiplications)
##Skinning (Talea Ideas)
###Dinosaur
You are one of the mighty dinosaurs -- King Dinosaur -- and you have to travel across Pangea to get to a big volcano before it explodes, because if it explodes all the dinosaurs will die.
###Fairies & Unicorns
###Plants
When you reach Pluto, there should be a tree with a golden trunk and multicoloured leaves. You have to dig it out carefully to take it somewhere warmer where it can survive. It has magical powers that can save your grandmother or the king or someone.


#Competitors
https://www.youtube.com/watch?v=tDkn9bxr31U Allows lots of parental configuration (timer or not, lock stuff that is too easy, different modes (in order, got wrong, random, fill in the gaps, ...)
https://itunes.apple.com/us/app/rocket-math-multiplication-home/id1048024368?mt=8
