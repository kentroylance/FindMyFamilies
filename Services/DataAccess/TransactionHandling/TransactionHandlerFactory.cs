using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Collections;
using FindMyFamilies.Exceptions;
using FindMyFamilies.Transactions;
using FindMyFamilies.Util;

namespace  FindMyFamilies.TransactionHandling
{
    /// <summary>
    /// TransactionHandlerFactory returns TransactionHandler objects.
    /// Maintains/refreshes internal cache of TransactionHandlers.
    /// Config info retrieved from  FindMyFamilies.DataAccess.dll.config
    /// </summary>
    internal class TransactionHandlerFactory
    {
        private static Hashtable _transactionHandlers = null;
        private static ITransactionHandler _defaultTransactionHandler = null;

        private const string TRANSACTIONHANDLERS_ROOT_ELEMENT = "transactionHandlers";

        //only static methods
        private TransactionHandlerFactory() {}

        internal static ITransactionHandler GetHandler()
        {
            if(_transactionHandlers == null) 
            {
                ConfigManager.ProcessConfig(
                    TRANSACTIONHANDLERS_ROOT_ELEMENT, new LoadXMLHandler(LoadTransactionHandlers));

                if(_transactionHandlers == null)
                    throw new DataAccessException("No TransactionHandler config data found.");
            }

            ITransactionHandler th = _defaultTransactionHandler;

            if(th == null) 
            {
                //TODO: specialize the exception, perhaps DataSourceNotFoundException
                throw new DataAccessException("Default ITransactionHandler not found.");
            }

            return th;
        }

        private static void LoadTransactionHandlers(string xml) 
        {
            XmlTextReader xr = new XmlTextReader(new StringReader(xml));

            xr.WhitespaceHandling = WhitespaceHandling.None;
            xr.MoveToContent();

            Hashtable transactionHandlers = new Hashtable();
            string name = "";

            while(xr.Read())
            {
                switch(xr.NodeType) 
                {
                    case XmlNodeType.Element:
                    switch(xr.Name) 
                    {
                        case "transactionHandler":
                            name = xr.GetAttribute("name");
                            if(name == null || name == string.Empty)
                                throw new DataAccessException("No transactionHandler name specified.");

                            string strHandlerType = xr.GetAttribute("handlerType");
                            if(strHandlerType == null || strHandlerType == string.Empty)
                                throw new DataAccessException("No handlerType specified for transactionHandler " + name);
                            Type handlerType = Type.GetType(strHandlerType);
							if(handlerType == null)
                                throw new DataAccessException("Could not load handlerType for typeName " + strHandlerType + " for transactionHandler " + name);

                            string strDefault = xr.GetAttribute("default");
                            bool isDefault = false;
                            if(strDefault != null)
                                isDefault = bool.Parse(strDefault);

                            ITransactionHandler th = 
                                (ITransactionHandler)Activator.CreateInstance(handlerType);

                            transactionHandlers.Add(name, th);

                            if(isDefault) 
                            {
                                _defaultTransactionHandler = th;

                                //TODO: find a way to invoke the constructor at TransactionContextFactory init???
                                TransactionContextFactory.ContextCreated += new TCCreatedEventHandler(th.HandleTCCreated);
                            }
                            break;
                    }
                        break;
                }
            }
            xr.Close();

            _transactionHandlers = transactionHandlers;
        }
    }
}
