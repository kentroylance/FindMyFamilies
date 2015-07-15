using System;
using System.Data;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Transactions;

namespace  FindMyFamilies.TransactionHandling
{
    /// <summary>
    /// HomeGrownTransactionHandler handles TransactionContext* events.
    /// Impementation using manual enlistment and control of data connections and transactions.
    /// Singlenton?
    /// </summary>
    public class HomeGrownTransactionHandler : ITransactionHandler
    {
        private const string THREAD_DATASESSIONS_HASHTABLE_KEY = "TDHK";

        public HomeGrownTransactionHandler()
        {
            ////TODO: find a way to invoke the constructor at TransactionContextFactory init???
            //TransactionContextFactory.ContextCreated += 
            //  new TCCreatedEventHandler(this.HandleTCCreated);
        }

        private static Hashtable dataSourceDataSessionsByTrCtx 
        {
            get 
            {
                Hashtable table = 
                    CallContext.GetData(THREAD_DATASESSIONS_HASHTABLE_KEY) as Hashtable;
                if(table == null) 
                {
                    table = new Hashtable();
                    CallContext.SetData(THREAD_DATASESSIONS_HASHTABLE_KEY, table);
                }
                return table;
            }
        }

        #region ITransactionHandler Members

        public void HandleTCCreated(object sender, TCCreatedEventArgs args)
        {
            //here attach to the event handlers of the newly created transaction context
            //(not in GetDataSession)
            TransactionContext trCtx = args.Context;
            trCtx.StateChanged += new TCStateChangedEventHandler(HandleTCStateChangedEvent);
        }

        public void HandleTCStateChangedEvent(object sender, TCStateChangedEventArgs args)
        {
            TransactionContext trCtx = (TransactionContext)sender;

            //here check the event & commit/rollback etc
            switch(trCtx.State) 
            {
                case TransactionContextState.Entered:
                    TransactionContext contrTrCtx = trCtx.GetControllingContext();
                    if(contrTrCtx != null && !dataSourceDataSessionsByTrCtx.Contains(contrTrCtx))
                        dataSourceDataSessionsByTrCtx.Add(contrTrCtx, new Hashtable());
                    break;

                case TransactionContextState.ToBeCommitted:
                    break;

                case TransactionContextState.ToBeRollbacked:
                    break;

                case TransactionContextState.Exitted:
                    if(dataSourceDataSessionsByTrCtx.Contains(trCtx)) 
                    {
                        switch(args.FromState)
                        {
                            case TransactionContextState.ToBeCommitted:
                                CommitTransactions(trCtx);
                                break;
                            case TransactionContextState.ToBeRollbacked:
                                RollbackTransactions(trCtx);
                                break;
                        }
                        dataSourceDataSessionsByTrCtx.Remove(trCtx);
                    }
                    break;

                default:
                    throw new Exception("Unexpected TransactionContextState:" + args.FromState.ToString());
            }       
        }

        private void CommitTransactions(TransactionContext trCtx) 
        {
            Hashtable dataSessionsByDataSourceToCommit = 
                dataSourceDataSessionsByTrCtx[trCtx] as Hashtable;
            if(dataSessionsByDataSourceToCommit != null) 
            {
                foreach(DictionaryEntry entry in dataSessionsByDataSourceToCommit) 
                {
                    DataSession dataSession = (DataSession)entry.Value;
                    dataSession.Transaction.Commit();
                    dataSession.Connection.Close();
                }
                dataSessionsByDataSourceToCommit.Clear();
            }
        }

        private void RollbackTransactions(TransactionContext trCtx) 
        {
            Hashtable dataSessionsByDataSourceToRollback = 
                dataSourceDataSessionsByTrCtx[trCtx] as Hashtable;
            if(dataSessionsByDataSourceToRollback != null) 
            {
                foreach(DictionaryEntry entry in dataSessionsByDataSourceToRollback) 
                {
                    DataSession dataSession = (DataSession)entry.Value;
                    dataSession.Transaction.Rollback();
                    dataSession.Connection.Close();
                }
                dataSessionsByDataSourceToRollback.Clear();
            }
        }

        public DataSession GetDataSession(IDataSource ds)
        {
            DataSession dataSession = null;

            //first get the current ***controlling*** context
            TransactionContext trCtx = TransactionContextFactory.GetCurrentContext();
            TransactionContext contrTrCtx = null;
            if(trCtx != null)
                contrTrCtx = trCtx.GetControllingContext();
            //if it's not null
            if(contrTrCtx != null) 
            {
                //get the Hashtable with DataSessions per DataSource
                Hashtable dataSessionsByDataSource = dataSourceDataSessionsByTrCtx[contrTrCtx] as Hashtable;
                //if not existing create :(
                if(dataSessionsByDataSource == null) 
                {
                    //add it to it
                    dataSessionsByDataSource = new Hashtable();
                    dataSourceDataSessionsByTrCtx.Add(contrTrCtx, dataSessionsByDataSource);
                    //and subscribe for it's events
                    contrTrCtx.StateChanged += new TCStateChangedEventHandler(HandleTCStateChangedEvent);
                }
                else 
                {
                    dataSession = dataSessionsByDataSource[ds] as DataSession;
                }

                //if not existing create new and add it
                if(dataSession == null) 
                {
                    IDbConnection con = ds.CreateConnection();
                    con.Open();
                    IsolationLevel isolationLevel = 
                        (IsolationLevel)Enum.Parse(typeof(IsolationLevel), contrTrCtx.IsolationLevel.ToString());
                    IDbTransaction tran = con.BeginTransaction(isolationLevel);
                    dataSession = new DataSession(con, tran);
                    dataSessionsByDataSource.Add(ds, dataSession);
                }
            }

            return dataSession;
        }

        #endregion
    }
}
