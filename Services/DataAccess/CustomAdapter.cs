using System;
using System.Data;
using System.Data.Common;

namespace  FindMyFamilies.DataAccess {
	public class CustomAdapter : DbDataAdapter {
		public int FillFromReader(DataTable dataTable, IDataReader dataReader) {
			return this.Fill(dataTable, dataReader);
		}

		protected override RowUpdatedEventArgs CreateRowUpdatedEvent(DataRow a, IDbCommand b, StatementType c, DataTableMapping d) {
			return (RowUpdatedEventArgs) new EventArgs();
		}

		protected override RowUpdatingEventArgs CreateRowUpdatingEvent(DataRow a, IDbCommand b, StatementType c, DataTableMapping d) {
			return (RowUpdatingEventArgs) new EventArgs();
		}

		protected override void OnRowUpdated(RowUpdatedEventArgs value) {
		}

		protected override void OnRowUpdating(RowUpdatingEventArgs value) {
		}
	}
}