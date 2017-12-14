
# Before deploying
 * Update MDVersion to handle previous version
 * Update build and/or version numbers (Android & iOS build settings + testflight & google play)
  * Changing the version number to something other than 0.1.x seems to cause an Invalid Signature complaint when uploaded
   * It complains that it is not a distribution certificate, which is true -- it is a developer certificate
   * Maybe 0.2 needs to be marked as "Test Flight" instead of "Release" somehow?  
 * use xARM to test the devices with the highest resolution, smallest & largest height & width, and extreme aspect ratios 16:9 and 4:3
 * Ensure that screenshots / video are up to date
  * Add grid button screenshots for small devices
 * Update Noun Project credits 
 * Test on device by changing bundle id to einmaleins2
 * Tag version in git
 * Change bundle id back to einmaleins

##Google
 * Enter passwords (build settings... publishing settings...keystore password) before building in Unity
 * Test in Simulator:
  * Start Simulator: ~/Library/Android/sdk/tools/emulator -avd Nexus_5X_API_26_x86
  * Drag apk onto simulator window
  * You might need to delete old version (Settings...App Info...einmaleins...uninstall)
 * New test: https://support.google.com/googleplay/android-developer/answer/3131213?hl=en

 * New release: https://support.google.com/googleplay/android-developer/answer/7159011
  * Play Console...Release Management... New Release...Browse Files

 * Publish
  * https://play.google.com/about/families/designed-for-families/program-requirements/  
##Apple
 * Build in unity
 * Upload in XCode
  * 1) choose "generic iOS device" (don't choose Ipad or any other devices connected or a simulator) 
  * 2) Go to Product --> Archive
  * 3) If everything is fine, it should open the archive in the Organizer. Click "validate" and then "Submit to App Store". 
 * When you get the first email ("processed"), go to iTunesConnect and
  * update compliance info at iTunesConnect
   * YOU USE CRYPTO NOW! 
  * Check "Test Information" under IOSBuilds...Test Details in testflight, in the right language -- see Texts below (https://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=build&subsection=testdetails&id=24416006)
  * Add groups to the build
##On major changes
 * Check Beta App Description, etc. in Testflight...Test Informationhttps://itunesconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/1296879662/testflight?section=testinformation
 * Ensure that Datenschutzerklärung is still true (e.g. if you add tracking or cross-branding). Model is https://www.carlsen.de/datenschutzerklaerung-apps 
###Test
 * Test starting with no players
 * Test leaving and reentering the character
 * Test getting answers wrong and ending quiz early
 * Test instamastery
 * Test fast answer that isn't instamastery
 * Test slow mastery
 * Test reaching Mars
 * Test upgrading
 * Test stats
 * Test reaching Pluto (all questions should be mastered, all upgrades done, gauntlet. Impossible to reach early)
 * Test post-gauntlet
