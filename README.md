# CardFileKPPlugin
Summary: A plugin for Keepass.  It enables importing a Microsoft Cardfile.

The plugin adheres to the Keepass 2.x interface and was written in C# v6.

The MS Cardfile is an old app from Windows NT and previous times which is however still in use today in a few cases.  The app was bundled in with the OS, and provided a simple GUI and 'database' in the manner of a manual indexed card file.  For those few using it, one approach was to use it to store login details and keep the resulting file inside an encrypted wrapper of some sort.  The MS app did not provide a means of exporting the data captured within it.  This plugin provides a means of importing the Cardfile data into Keepass.

At each import, the plugin creates a new Keepass group, whose name (ImpCardFile-<date/time>) includes the date and time, and puts all data read into the group, with each Card index text appearing as a an Entry Title, and the card contents appearing as an Entry Note.

This plugin uses the CardFileRdr.dll assembly, which does the actual reading of the MS Cardfile.  A related app is CardFileExporter which enables the Cardfile to be exported to XML instead, and which also uses that .dll.

A limitation of the CardFileRdr.dll is that it ignores contained OLEs and graphic objects such as bitmaps, which are theoretically allowed, so reads only the text contents, but it will process Unicode correctly.

The plugin uses Keepass's update check with an RSA public-key, providing notification of when new versions are available.  A SHA256 hash of the plugin is made available, and CardFileRdr.dll is signed with a self-signed X.509 certificate and has a similar hash provided too.

The plugin is made available as a Keepass .plgx file (CardFileKPPlugin.plgx) and is installed by simply copying it to the Keepass plugins directory.
