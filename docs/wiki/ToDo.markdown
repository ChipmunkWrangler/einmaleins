[Ideas](Ideas)

# ToDo 

## v1.0.0 
* [X] update OSX and XCode to be iPhoneX compatible
	* [X] Ensure that iPhone X divot at the top of screen doesn't mess up my layout https://forum.unity.com/threads/iphone-x-notch-in-screen.495028/
		* [X] Test in simulator 
		* [X] Test on my ipad
	* iPhone X overall dimensions 2436 x 1125 pixels
	* Overall safe area 2172 x 1062 pixels
	* Left & right insets (accounts for the notch and curved corners, plus a margin) 132 pixels each => 600*132/1125=70
	* Bottom inset (accounts for the bottom nav bar) 63 pixels => 33
	* Top inset zero pixels => 55 
* [X] Make screenshots for shop
* [X] Grid mode screenshots
* [X] Consider using web page text for app store. Rich text?
* [o] Update web page
    * [X] Proofread German
    * [X] add screenshots
	* [X] English
	* [X] German
    * [o] Generate links for app store buttons and add them to web pages
	* [X] https://play.google.com/intl/en_us/badges/
	* [ ] https://developer.apple.com/library/content/qa/qa1633/_index.html 
    * [ ] check in and upload
* [X] Make video for app stores
    * [X] English
	* [X] iPad
	* [X] Android
    * [X] German
	* [X] iPad
	* [X] Android
* [X] Add Screenshot captions
    * [X] English
    * [X] German
* [X] Publish
    * [X] iOS
	* [X] Fix errors
	    * [X] Add contact page
	    * [X] Apple reqs
    * [X] Android
* [ ] "Managed Google Play" after publishing
* [X] Mars Ho! is confusing
* [X] Promo codes and reviews from filkers and flaregamers
* [X] E proofread web page
* [X] Store page https://developer.apple.com/app-store/categories/ and on android
	* [X] Localized, up to date screen shots  
* [ ] Test that it works with 512 Mb devices https://hwstats.unity3d.com/mobile/
* [X] Change iOS build to Fast but no Exceptions option.
* [X] Test: Turn off logging
* [ ] Talk to Matthias S. again

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
