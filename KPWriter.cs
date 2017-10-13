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

using KeePassLib;
using KeePassLib.Security;

using CardFileRdr;

namespace CardFileKPPlugin {

	/// <summary>
	/// Write card data to Keepass
	/// </summary>
	public class KPWriter: Writer {

		private PwDatabase m_DB; // data target
		private PwGroup impGrp; // New group into which to import entries


		/// <summary>
		/// Construct a Writer object
		/// </summary>
		/// <param name="pDB">To where the object should be written</param>
		public KPWriter( PwDatabase pDB ) {
			if (pDB == null) {
				throw new ArgumentNullException( "CardFileKPPlugin.KPWriter(): null PwDB received" );
			}
			m_DB = pDB;
		}


		/// <summary>
		/// Write a header - For Keepass, create a new group into which to import
		/// </summary>
		public void WriteHeader() {
			string cfGroup = "ImpCardFile-" + DateTime.Now.ToString(); // new group into which to place imported entries
			impGrp = new PwGroup( true, true, cfGroup, PwIcon.Archive ); // A new group with a random icon
			m_DB.RootGroup.AddGroup( impGrp, true );
		}


		/// <summary>
		/// Write the equivalent of the card - but text only - contained objects are ignored
		/// </summary>
		/// <param name="card">The card data</param>
		public void WriteCard( Card card ) {
			if (card == null) {
				throw new ArgumentNullException( "CardFileKPPlugin.KPWriter.WriteCard(): null card received" );
			}

			if (!(String.IsNullOrEmpty( card.Title )) || !(String.IsNullOrEmpty( card.Data ))) {
				if (impGrp == null) {
					WriteHeader();
				}
				PwEntry ent = new PwEntry(true, true); // Create a new entry
				WriteTitle( ent, card.Title );
				WriteEntry( ent, card.Data );

				impGrp.AddEntry( ent, true ); // tell parent group it owns this entry
			}//has data
		}


		/// <summary>
		/// Write the equivalent of the card index title
		/// </summary>
		/// <param name="title">Card index title</param>
		private void WriteTitle( PwEntry ent, string title ) {
			if (title == null) { title = String.Empty; }
			ent.Strings.Set( PwDefs.TitleField, new ProtectedString( false, title ) );
		}


		/// <summary>
		/// Write the equivalent of the card data (text only)
		/// </summary>
		/// <param name="data">The card data</param>
		private void WriteEntry( PwEntry ent, string data ) {
			if (data == null) { data = String.Empty; }
			ent.Strings.Set( PwDefs.NotesField, new ProtectedString( false, data ) );
		}


		/// <summary>
		/// Close and dispose of the writable object (when appropriate)
		/// </summary>
		public void close() {
			if (impGrp == null) {
				throw new ExnCardFileRdr( "CardFileKPPlugin.KPWriter.close(): Plugin did not find any Cardfile text data to import" );
			}
		}

	}//class

}//nm
