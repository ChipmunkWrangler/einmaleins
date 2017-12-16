[Ideas](Ideas)

# ToDo 

## v1.0.0 
* [ ] Make screenshots for shop, perhaps add them to web pages,
* [X] Mars Ho! is confusing
* [ ] Generate links for app store buttons
	* [ ] https://play.google.com/intl/en_us/badges/
	* [ ] https://developer.apple.com/library/content/qa/qa1633/_index.html 
* [ ] Store page https://developer.apple.com/app-store/categories/ and on android
	* [ ] Localized, up to date screen shots  
* [ ] Remove unnecessary permissions, e.g. Android.Internet aka "Other: full network access"
	* [ ] Note that in the .csproj files, there are defines like ENABLE_CLOUD_SERVICES_ADS 
* [ ] Test that it works with 512 Mb devices (https://hwstats.unity3d.com/mobile/os.html)
* [X] Change iOS build to Fast but no Exceptions option.
* [ ] Test: Turn off logging
* [ ] update OSX and XCode to be iPhoneX compatible
	* [ ] show status bar? 
	* [ ] Ensure that iPhone X divot at the top of screen doesn't mess up my layout https://forum.unity.com/threads/iphone-x-notch-in-screen.495028/
	* iPhone X overall dimensions 2436 x 1125 pixels
	* Overall safe area 2172 x 1062 pixels
	* Left & right insets (accounts for the notch and curved corners, plus a margin) 132 pixels each
	* Bottom inset (accounts for the bottom nav bar) 63 pixels
	* Top inset zero pixels
* Talk to Matthias S. again

### Credits

* Creative commons attribution 
	* spaceship by Jonathan Li from the Noun Project
	* Olga Prikhodko (exit arrow)
	* Numero Uno (Play Button)
	* Gauge By Roberto Chiaveri
	* Kuber (Back button)

# Bugs
* There is a pause on the first quiz question
* Reach mars without answering any questions after upgrade 1 : maybe tweak the planet heights?
* if a wrong question is shown but not answered (because you reach the planet), it is still no longer marked as wrong, hence in the next quiz is doesn't appear first
* [ ] ion stream flickers between in front of and behind the planets. But it is also kind of cool.
