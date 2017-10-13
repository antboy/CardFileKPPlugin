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

using System;
using System.IO;
using System.Windows.Forms;

using KeePass.DataExchange;
using KeePassLib.Interfaces;
using KeePass.Plugins;

using CardFileRdr; // Cardfile reader


namespace CardFileKPPlugin {

	public class CardFileFormatProvider: FileFormatProvider {

		private IPluginHost m_Host;


		public CardFileFormatProvider( IPluginHost host ) {
			if (host == null) {
				throw new ArgumentNullException( "CardFileFormatProvider(): null host argument received" );
			}
			m_Host = host;
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes" )]
		public override void Import( KeePassLib.PwDatabase pwStorage, System.IO.Stream sInput, IStatusLogger slLogger ) {
			if (pwStorage == null) {
				throw new ArgumentNullException( "CardFileFormatProvider.Import(): null PwDatabase argument received" );
			}
			if (!pwStorage.IsOpen) {
				MessageBox.Show( "You first need to open a database!", "CardFileFormatProvider" );
				return;
			}
			if (sInput == null) {
				throw new ArgumentNullException( "CardFileFormatProvider.Import(): null Stream argument received" );
			}
			if (slLogger == null) {
				throw new ArgumentNullException( "CardFileFormatProvider.Import(): null IStatusLogger argument received" );
			}
			if (!(sInput.CanRead)) {
				throw new ArgumentException( "Input stream not readable" );
			}
			try {
				KPWriter kpWriter = new KPWriter(pwStorage); // The plugin's Keepass Writer
				CardFile crdfile = new CardFile(false, String.Empty, kpWriter); // The plugin's Importer, false = no logging

				slLogger.SetText( "Importing Cardfile ...", LogStatusType.Info );
				crdfile.process( sInput ); // read the cardfile & write to keepass

				m_Host.MainWindow.UpdateUI( false, null, true, m_Host.Database.RootGroup, true, null, true );
				slLogger.SetText( "Importing Cardfile completed", LogStatusType.Info );

			} catch (ExnCardFileRdr ex) {
				reportError( ex.Message, "Warning", slLogger, LogStatusType.Warning );

			} catch (FileNotFoundException ex) {
				reportError( ex.Message, "Warning", slLogger, LogStatusType.Warning );

			} catch (DirectoryNotFoundException ex) {
				reportError( ex.Message, "Warning", slLogger, LogStatusType.Warning );

			} catch (Exception ex) {
				reportError( ex.ToString(), "Error", slLogger, LogStatusType.Error );
			}//try
		}

		// report error details
		private void reportError( String msg, String heading, IStatusLogger slLogger, LogStatusType lst ) {
			MessageBox.Show( msg,
				heading,
				MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1 );

			slLogger.SetText( msg, lst );
		}

		public override bool SupportsImport {
			get { return true; }
		}

		public override bool SupportsExport {
			get { return false; }
		}

		public override string FormatName {
			get { return "Microsoft Cardfile"; }
		}

		public override string DefaultExtension {
			get { return "crd"; }
		}

		public override string ApplicationGroup {
			get { return KeePass.Resources.KPRes.PasswordManagers; }
		}

		public override bool ImportAppendsToRootGroupOnly {
			get { return false; }
		}

		public override bool RequiresFile {
			get { return true; }
		}


	}//class
}//nm
