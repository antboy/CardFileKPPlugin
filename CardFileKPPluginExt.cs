// CardFileKPPlugin - Copyright © 2016 John Oliver
// This program is free software: you can redistribute it and/or modify it under
// the terms of the GNU General Public License as published by the Free Software
// Foundation, either version 3 of the License, or (at your option) any later
// version.
// This program is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.


using KeePass.Plugins;

// The namespace name must be the same as the filename of the plugin without its extension.
// E.g. plugin SamplePlugin.dll namespace must be named 'SamplePlugin'.
namespace CardFileKPPlugin {

	/// <summary>
	/// A plugin for Keepass to import text data from a Microsoft Cardfile.
	///
	/// This is the main plugin class. It must be named exactly like the namespace suffixed with 'Ext'
	/// and must be derived from <c>KeePass.Plugins.Plugin</c>.
	/// </summary>
	public class CardFileKPPluginExt: Plugin {

		// KP data
		private IPluginHost  m_Host = null;

		// plugin custom data
		private CardFileFormatProvider  m_Importer;


		/// <summary>
		/// Override to give version file URL
		/// </summary>
		public override string UpdateUrl {
			get { return Defs.sUpdateUrl; }
		}


		/// <summary>
		/// Override Keepass method
		/// </summary>
		public override bool Initialize( IPluginHost host ) {
			if (host == null) { return false; }
			m_Host = host;
			KeePass.Util.UpdateCheckEx.SetFileSigKey( Defs.sUpdateUrl , Defs.RSAPublicKeyXml ); // version checker; associate version file with its public key

			m_Importer = new CardFileFormatProvider( m_Host ); // plugin's importer
			m_Host.FileFormatPool.Add( m_Importer ); // Add to list of importers

			return true;
		}//m()


		/// <summary>
		/// Override Keepass method
		/// </summary>
		public override void Terminate() {
			m_Host.FileFormatPool.Remove( m_Importer );
		}

	}//class

}//nm
