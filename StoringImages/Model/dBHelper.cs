using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Data.Common;

namespace StoringImages.Model
{
    class dBHelper { 
        private SQLiteConnection m_connection = null;
        private string m_connectionString = "";
        private SQLiteDataAdapter m_dataAdapter = null;
        private DataSet m_dataSet = null;
        private string m_fieldNameID = "";
        public DataSet DataSet
        {
            get { return m_dataSet; }
        }
        public dBHelper(string connectionString)
        {
            m_connectionString = connectionString;
        }
        public bool Load(string commandText, string fieldNameID)
        {
            m_fieldNameID = fieldNameID;
            try
            {

                m_connection = new SQLiteConnection(m_connectionString);
                m_connection.Open();

                m_dataAdapter = new SQLiteDataAdapter(commandText, m_connection);

                m_dataAdapter.RowUpdated += m_dataAdapter_RowUpdated;
                m_dataSet = new DataSet();
                if (!string.IsNullOrEmpty(fieldNameID))
                {
                    SQLiteCommandBuilder commandBuilder =
                   new SQLiteCommandBuilder(m_dataAdapter);
                    m_dataAdapter.InsertCommand =
                   commandBuilder.GetInsertCommand();
                    m_dataAdapter.DeleteCommand =
                   commandBuilder.GetDeleteCommand();
                    m_dataAdapter.UpdateCommand =
                   commandBuilder.GetUpdateCommand();
                }
                m_dataAdapter.Fill(m_dataSet);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                m_connection.Close();
            }
        }
        public bool Load(string commandText)
        {
            return Load(commandText, "");
        }
        public bool Save()
        {
            if (m_fieldNameID.Trim().Length == 0)
            {
                return false;
            }
            try
            {
                m_connection.Open();
                m_dataAdapter.Update(m_dataSet);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                m_connection.Close();
            }
        }
        void m_dataAdapter_RowUpdated(object sender,
        System.Data.Common.RowUpdatedEventArgs
       e)
        {
            if (e.StatementType == StatementType.Insert)
            {
                SQLiteCommand command = new SQLiteCommand
                ("SELECT last_insert_rowid() AS ID",m_connection);
                object newID = command.ExecuteScalar();
                if (newID == System.DBNull.Value == false)
                {
                    e.Row[m_fieldNameID] = Convert.ToInt32(newID);
                }
            }
        }
    }
}
