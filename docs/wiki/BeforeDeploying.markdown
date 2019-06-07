* [o] Deploy
 * [X] Update MDVersion to handle previous version
 * [X] Update version number in iOS build settings
 * [ ] Update build and/or version numbers in Android build settings
 * [X] use xARM to test the devices with the highest resolution, smallest & largest height & width, and extreme aspect ratios 16:9 and 4:3
 * [X] Ensure that screenshots / video are up to date
  * [X] Add grid button screenshots for small devices
 * [O] Test on device IN GERMAN TOO 
  * [X] Test data version update
  * [X] Test starting with no players
  * [X] Text switching to grid layout
  * [X] Test leaving and reentering the character
  * [X] Test leaving and entering as another character (layout should be clock again)
  * [X] Test a division character
  * [X] Test getting answers wrong and ending quiz early
  * [X] Test launch codes
  * [X] Test "stop for the day"
  * [X] Test aborting quiz
  * [X] Test instamastery
  * [X] Test fast answer that isn't instamastery
  * [X] Test slow mastery
  * [X] Test reaching Mars (including message)
  * [X] Test new record (including message)
  * [X] Test upgrading (including particle effect and name)
  * [X] Test stats (multiplication only)
  * [ ] Test reaching Pluto (all questions should be mastered, all upgrades done, gauntlet. Impossible to reach early)
  * [ ] Test post-gauntlet
 * [ ] Ensure that Datenschutzerklärung is still true (e.g. if you add tracking or cross-branding). Model is https://www.carlsen.de/datenschutzerklaerung-apps 
 * [o] iOS Build
  * [X] Build in unity
  * [X] Upload in XCode
   * [X] choose "generic iOS device" (don't choose Ipad or any other devices connected or a simulator) 
   * [X] Use the search spyglass under the play button in Xcode to find "UIApplicationExitsOnSuspend" and click the minus sign to remove the key.
   * [X] Go to Product --> Archive
   * [X] it should open the archive in the Organizer. Click "validate" 
   * [X] Click "Distribute App" with default options. Optional: When you get to the second screen after Distribute App, choose Export instead of Upload, and give the resulting file to ApplicationLoader. This only works if your apple@ account is only signed up with one team, but it allows you (in settings... advanced) to turn off all protocols except DAV, which avoid the "Negotiating connection" freeze.
  * [O] When you get the first email ("processed"), go to iTunesConnect and
   * [X] update compliance info at iTunesConnect
   * [X] Check "Test Information" under IOSBuilds...Test Details in testflight, in the right language -- see Texts below (https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=build&subsection=testdetails&id=24416006)
   * [ ] Add groups to the build if it is for test flight
   * [X] Wait for Review
  * [ ] Check Beta App Description, etc. in Testflight...Test Information
   * https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=testinformation
  * [ ] Update Noun Project credits on store page 
 * [ ] Android Build
  * [ ] Enter passwords (build settings... publishing settings...keystore password) before building in Unity
  * [ ] Test in Simulator:
   * Start Simulator: ~/Library/Android/sdk/tools/emulator -avd Nexus_5X_API_26_x86
   * Drag apk onto simulator window
   * You might need to delete old version (Settings...App Info...einmaleins...uninstall)
   * Click on the little up arrow at the bottom of the screen
   * Click on Times Tables
  * [ ] New test: https://support.google.com/googleplay/android-developer/answer/3131213?hl=en
  * [ ] New release: https://support.google.com/googleplay/android-developer/answer/7159011
   * [ ] Play Console...Release Management... New Release...Browse Files
  * [ ] https://play.google.com/about/families/designed-for-families/program-requirements/  
  * [ ] update version number (?)
  * [ ] Update Noun Project credits on store page 
 * [X] Tag version in git
