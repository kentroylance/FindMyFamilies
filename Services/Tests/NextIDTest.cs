///////////////////////////////////////////////////////////////////////////
// Description: Unit Test class for 'Next_ID'
// Generated by FindMyFamilies Generator
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Reflection;

using NUnit.Framework;
using XtUnit.Extensions.Royo;
using XtUnit.Framework;

using FindMyFamilies.Data;
using FindMyFamilies.Services;

namespace FindMyFamilies.Tests {

	/// <summary>
	/// Summary description for CommandTests.
	/// </summary>
	[TestFixture]
	public class NextIDTest : UnitTestBase    {

		private static readonly object logCategory = MethodBase.GetCurrentMethod().DeclaringType;

		public string[] validateFields = {
			NextIDDO.LANGUAGE_ID
		};

		public NextIDTest() {
			Init();
		}

		public NextIDTest(string testMethod) : base(testMethod) {
			Init();
		}

		private void Init() {
			this.DataFile = "NextIDData.xml";
			this.DataObjectType = "NextIDDO";
		
			if (this.TestDataArray.Count < 1) {
				this.LoadData();
			}
		}
		

		public void CreateNextID(ref NextIDDO inputData) {
			inputData = (NextIDDO) AdminServices.Instance().CreateNextID(inputData);
			if (inputData.Table_ID < 1) {
				throw new TestException("Failed: Cannot create a NextID. ", null);
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestCreateNextIDs() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					NextIDDO outputData = (NextIDDO) AdminServices.Instance().CreateNextID(inputData);
					if (outputData.Table_ID > 0) {
						if (testData.ReadDataObject != null) {
							((NextIDDO) testData.ReadDataObject).Table_ID = outputData.Table_ID;
						}
						if (testData.UpdateDataObject != null) {
							((NextIDDO) testData.UpdateDataObject).Table_ID = outputData.Table_ID;
						}
						outputData = AdminServices.Instance().ReadNextID(outputData);
					}
					if (outputData.Table_ID < 1) {
						throw new TestException();
					} else {
						Validate(validateFields, inputData, outputData);
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot create a NextID. ", ex);
				}
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestUpdateNextIDs() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					CreateNextID(ref inputData);
					if (inputData.Table_ID > 0) {
						NextIDDO nextIDInput = (NextIDDO) testData.UpdateDataObject;
						nextIDInput.GetSecurityData(testData.Username, testData.Password, this.Language);
						nextIDInput.Table_ID = inputData.Table_ID;
						AdminServices.Instance().UpdateNextID(nextIDInput);
						NextIDDO outputData = AdminServices.Instance().ReadNextID(nextIDInput);
						if (outputData.Table_ID > 0) {
							Validate(validateFields, nextIDInput, outputData);
						} else {
							throw new TestException("Failed: Cannot update a NextID. ", null);
						}
					} else {
						throw new TestException();
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot update a NextID. ", ex);
				}
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestDeleteNextIDs() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					CreateNextID(ref inputData);
					if (inputData.Table_ID > 0) {
						AdminServices.Instance().DeleteNextID(inputData);
						inputData = AdminServices.Instance().ReadNextID(inputData);
						if (inputData.Table_ID > 0) {
							throw new TestException("Failed: Cannot delete a NextID. ", null);
						}
					} else {
						throw new TestException();
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot delete a NextID. ", ex);
				}
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestReadNextID() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					CreateNextID(ref inputData);
					if (inputData.Table_ID > 0) {
						NextIDDO outputData = AdminServices.Instance().ReadNextID(inputData);
						if (outputData.IsEmpty()) {
							throw new TestException("Failed: Cannot read a NextID. ", null);
						} else {
							Validate(validateFields, inputData, outputData);
						}
					} else {
						throw new TestException("Failed: Cannot create a NextID. ", null);
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot read a NextID. ", ex);
				}
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestReadAllNextIDs() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					CreateNextID(ref inputData);
					if (inputData.Table_ID > 0) {
						ICollection results = (ICollection) AdminServices.Instance().ReadNextIDsAll(inputData);
						if (results.Count < 1) {
							throw new TestException();
						}
					} else {
						throw new TestException("Failed: Cannot create a NextID. ", null);
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot read all NextIDs. ", ex);
				}
			}
		}
		
		[Test, CustomRollBack, CustomTracing]
		public void TestReadNextIDsByPage() {
			for (this.Row = 1; this.Row < this.TestDataArray.Count; this.Row++) {
				try {
					TestData testData = (TestData) this.TestDataArray[this.Row];
					NextIDDO inputData = (NextIDDO) testData.ReadDataObject;
					inputData.GetSecurityData(testData.Username, testData.Password, this.Language);
					CreateNextID(ref inputData);
					if (inputData.Table_ID > 0) {
						inputData.Paging.PageIndex = 1;
						inputData.Paging.PageSize = 100;
						ICollection results = (ICollection) AdminServices.Instance().ReadNextIDsByPage(inputData);
						if (results.Count < 1) {
							throw new TestException();
						}
					} else {
						throw new TestException("Failed: Cannot create a NextID. ", null);
					}
				} catch (Exception ex) {
					throw new TestException("Failed: Cannot read NextIDs by page. ", ex);
				}
			}
		}
	}
}
