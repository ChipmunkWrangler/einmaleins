* [X] Deploy
 * [X] Update MDVersion to handle previous version
 * [X] Update build and/or version numbers in Android & iOS build settings
 * [X] use xARM to test the devices with the highest resolution, smallest & largest height & width, and extreme aspect ratios 16:9 and 4:3
 * [X] Ensure that screenshots / video are up to date
  * [X] Add grid button screenshots for small devices
 * [X] Test on device by changing bundle id to einmaleins2
  * [X] Test starting with no players
  * [X] Test leaving and reentering the character
  * [X] Test leaving and entering as another character 
  * [X] Test getting answers wrong and ending quiz early
  * [X] Test instamastery
  * [X] Test fast answer that isn't instamastery
  * [X] Test slow mastery
  * [X] Test reaching Mars
  * [X] Test upgrading
  * [X] Test stats
  * [X] Test reaching Pluto (all questions should be mastered, all upgrades done, gauntlet. Impossible to reach early)
  * [X] Test post-gauntlet
 * [X] Change bundle id back to einmaleins
 * [X] Ensure that Datenschutzerklärung is still true (e.g. if you add tracking or cross-branding). Model is https://www.carlsen.de/datenschutzerklaerung-apps 
 * [X] iOS Build
  * [X] Build in unity
  * [X] Upload in XCode
   * [X] 1) choose "generic iOS device" (don't choose Ipad or any other devices connected or a simulator) 
   * [X] 2) Go to Product --> Archive
   * [X] 3) it should open the archive in the Organizer. Click "validate" and then "Upload to App Store". 
  * [X] When you get the first email ("processed"), go to iTunesConnect and
   * [X] update compliance info at iTunesConnect
   * [X] Check "Test Information" under IOSBuilds...Test Details in testflight, in the right language -- see Texts below (https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=build&subsection=testdetails&id=24416006)
   * [X] Add groups to the build
  * [X] Check Beta App Description, etc. in Testflight...Test Information
   * https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=testinformation
  * [X] Update Noun Project credits on store page 
 * [X] Android Build
  * [X] Enter passwords (build settings... publishing settings...keystore password) before building in Unity
  * [X] Test in Simulator:
   * Start Simulator: ~/Library/Android/sdk/tools/emulator -avd Nexus_5X_API_26_x86
   * Drag apk onto simulator window
   * You might need to delete old version (Settings...App Info...einmaleins...uninstall)
   * Click on the little up arrow at the bottom of the screen
   * Click on Times Tables
  * [X] New test: https://support.google.com/googleplay/android-developer/answer/3131213?hl=en
  * [X] New release: https://support.google.com/googleplay/android-developer/answer/7159011
   * Play Console...Release Management... New Release...Browse Files
  * [X] https://play.google.com/about/families/designed-for-families/program-requirements/  
  * [X] update version number (?)
  * [X] Update Noun Project credits on store page 
 * [X] Tag version in git
