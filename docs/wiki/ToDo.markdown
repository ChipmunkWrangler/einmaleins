[Ideas](Ideas)

# ToDo 
## v1.0.1
* [ ] Test stats with division
* [ ] multiplication
* [ ] Switch back to mult player
* [ ] Mult gauntlet
* [ ] Div gauntlet
* [ ] Rocket part in div
* [ ] Launch in div (no stats), switch to mult player, stats
* [ ] Switch between circle and square
* [ ] Fix first upgrade particle effects
* [ ] Test all particle effects
* [ ] Test all on device
* [ ] Upload version
* [ ] Email dude, tell him about testflight or have him wait
## v1.0.0 
* [o] Update web page
    * [o] Generate links for app store buttons and add them to web pages
	* [X] https://play.google.com/intl/en_us/badges/
	* [ ] https://developer.apple.com/library/content/qa/qa1633/_index.html 
    * [ ] check in and upload
* [ ] Do Apple legal stuff
 * [ ] Waiting for their reply to my enquiry of 19.1. They replied on 25.1. to say it has been noted and please wait.
* [ ] Do Google legal stuff
 * [ ] Waiting for them to transfer test money to my account
* [ ] Once iOS is Released:
 * [ ] Inform Alexander that it is available in the store, possibly meet with him after Buehne Frei
 * [ ] Get filkers and facebookers and ios testers to review once iOS is released (you have a draft email)
* [.] Improve funnel in Play Store: 
 * [X] We are running a sale from 17.1. through 24.1. See (on 26.1.) if that makes a difference relative to other weeks. Check visitors, not just downloads (download report from User Acquisition). Result: No more visitors due to sale (but higher conversion rate). Action: End sale, observe icon variants.
 * [ ] Split test icons in Play Store (this can work parallel to free test)
  * [ ] Wait for results of first test
 * [ ] [MarketingExperiments](MarketingExperiments)
### Credits

* Creative commons attribution 
	* example: "Tree” icon by Edward Boatman, from thenounproject.com.
	* "Rocket Ship" by Jonathan Li from the Noun Project
	* "Login" by LAFS, RU
	* "Play" by Numero Uno
	* "Gauge" by Roberto Chiaveri

# Bugs
* If you get a question wrong, but then right quickly, you can instamaster
 * And if you get it wrong, and get it right fast in the next quiz, you can instamaster? WTF? Does this happen because the first answer was nearly fast enough, so the average is low? Maybe increase time penalty for wrong answers? 
* There is a pause on the first quiz question
* Reach mars without answering any questions after upgrade 1 : maybe tweak the planet heights?
* if a wrong question is shown but not answered (because you reach the planet), it is still no longer marked as wrong, hence in the next quiz is doesn't appear first
* ion stream flickers between in front of and behind the planets. But it is also kind of cool.
* rocket parts size and colour not reset on quiz end / abort, so if you reach a planet while getting a rocket part, it stays enlarged.
* Answer is not cleared when you abort the quiz, so if you type at just the right moment, the answer appears over the planet, and then the "Auf zum Pluto" button appears on top of the answer
* rocket button in gauntlet looks odd because the button is faded, but not the rocket image
# Refactoring
Rebuild your player select scene with the entity architecture, then with Zenject, see what seems better
 Then try Entitas and or Data Bind For Unity
* Move config into prefabs and scriptable objects because then you have version control for every individual bit, not a huge scene file
* Add <summary> docs
* https://www.gamasutra.com/blogs/YankoOliveira/20180108/312617/A_UI_System_Architecture_and_Workflow_for_Unity.php
* Consider Dual Serialization pattern from Glorious Scriptable Object Revolution (see git repo, GameSettings class)
* Consider integration tests: make it easy to write a script that runs the game answering some percent of the questions correctly, and some fraction fast.
* Consider unit tests: make logic just logic (humble object?)
* Monkey test with android or iphone emulator
* SOLID :
    * If you can think of more than one motive for changing a class, then that class has more than one responsibility.
* Read C# book, incorporate new features as you grasp them
* Read Design Patterns, incorporate them as you grasp them
* Read Game Design Patterns, incorporate them as you grasp them

