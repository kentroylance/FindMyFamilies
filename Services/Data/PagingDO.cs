using System;

namespace FindMyFamilies.Data {
    /// <summary>
    /// Summary description for PagingDO.
    /// </summary>
    public class PagingDO {

        // Declare Constants
		public const string READ_BY_PAGE = "ReadSortedPage";

		public const string PAGING_TABLE_NAME = "TableName";
		public const string PAGING_PRIMARY_KEY = "PrimaryKey";
		public const string PAGING_FIELDS = "Fields";
		public const string PAGING_JOIN = "Join";
		public const string PAGING_FILTER = "Filter";
		public const string PAGING_ORDER = "Order";
		public const string PAGING_PAGE_SIZE = "PageSize";
		public const string PAGING_PAGE_INDEX = "PageIndex";
		
		public const int PAGE_SIZE_DEFAULT = 0;
        public const int PAGE_INDEX = 0;

        // Declare Properties
        private int    m_PageIndex;  
        private int    m_PageSize;  
        private string m_Order;
        private string m_Filter;
		private string m_Join;
		private int    m_TotalRecords;
		private string m_Fields = "*";
		private string m_SearchField;

        public PagingDO() : base() {
        }

        public int PageIndex {
            get {
                if (m_PageIndex < 1) {
                    m_PageIndex = PAGE_INDEX;
                }
                return m_PageIndex;
            }
            set {
                m_PageIndex = value;
            }
        }

        public int PageSize {
            get {
                if (m_PageSize < 1) {
                    m_PageSize = PAGE_SIZE_DEFAULT;
                }
                return m_PageSize;
            }
            set {
                m_PageSize = value;
            }
        }

        public string Order {
            get {
                return m_Order;
            }
            set {
                m_Order = value;
            }
        }

        public string Filter 
		{
            get {
                return m_Filter;
            }
            set {
                m_Filter = value;
            }
        }

        public int TotalRecords {
            get {
                return m_TotalRecords;
            }
            set {
                m_TotalRecords = value;
            }
        }

    	public string Fields {
    		get {
    			return m_Fields;
    		}
    		set {
    			m_Fields = value;
    		}
    	}

    	public string Join {
    		get {
    			return m_Join;
    		}
    		set {
    			m_Join = value;
    		}
		}

    	public string SearchField {
    		get {
    			return m_SearchField;
    		}
    		set {
    			m_SearchField = value;
    		}
    	}
    }
}
