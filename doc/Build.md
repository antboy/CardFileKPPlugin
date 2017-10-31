## Building CardFileKPPlugin

If you wish your plugin to use a signed version file so that Keepass knows when a new version is available, firstly, add your public RSA public key in XML form to the Defs.cs C# sharp.  Also sign the version file with your private key (see [Keepass plugin update checking](http://keepass.info/help/v2_dev/plg_index.html#upd)) and add the location of the signed version file to Defs.cs.

Copy CardFileRdr.dll from [CardFileRdr](http://github.com/antboy/CardFileRdr) to the local build directory.  If using, for example, Visual Studio 2015, add the .dll as a reference to your project, otherwise link it in in the appropriate way for your build environment.  Follow the [Keepass plugin build directions](http://keepass.info/help/v2_dev/plg_index.html#plgx).
