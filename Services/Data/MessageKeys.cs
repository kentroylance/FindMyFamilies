using System;

namespace FindMyFamilies.Data {
	
	/*
	* Description: This interface holds the constants for the message keys. These messages keys correspond to
	* the message keys in the MessageResource.properties file.
	*
	*/
	public class MessageKeys : MessageKeysBase {
		//Generic Status Messages
		public readonly static String STATUS_CREATING = "StatusCreating";
		public readonly static String STATUS_FINISHED_CREATING = "StatusFinishedCreating";
		public readonly static String STATUS_DELETING = "StatusDeleting";
		public readonly static String STATUS_FINISHED_DELETING = "StatusFinishedDeleting";
		public readonly static String STATUS_UPDATING = "StatusUpdating";
		public readonly static String STATUS_FINISHED_UPDATING = "StatusFinishedUpdating";
		//Specific Status Messages
		//Generic Question Messages
		public readonly static String QUESTION_TITLE_SAVE = "QuestionTitleSave";
		public readonly static String QUESTION_SAVE_CHANGES = "QuestionSaveChanges";
		public readonly static String QUESTION_TITLE_DELETE = "QuestionTitleDelete";
		public readonly static String QUESTION_DELETE = "QuestionDelete";
		public readonly static String QUESTION_INSERT = "QuestionInsert";
		public readonly static String QUESTION_TITLE_INSERT = "QuestionTitleInsert";
		//Specific Question Messages
		public readonly static String QUESTION_DELETE_TRANSLATION_SYSTEM_DATA = "QuestionDeleteTranslationSystemData";
		//Generic Information Messages
		public readonly static String INFO_FIELD_REQUIRED = "InfoFieldRequired";
		public readonly static String INFO_TEXT_INVALID = "InfoTextInvalid";
		public readonly static String INFO_LIST_ITEM_ALREADY_EXIST = "InfoListItemAlreadyExist";
		public readonly static String INFO_LIST_ITEM_NOT_FOUND = "InfoListItemNotFound";
		public readonly static String INFO_DATE_INVALID_DATE = "InfoDateInvalidDate";
		public readonly static String INFO_NUMERIC_EXCEED_MAXIMUM = "InfoNumericExceedMaximum";
		public readonly static String INFO_NUMERIC_LESS_THEN_MINIMUM = "InfoNumericLessThenMinimum";
		public readonly static String INFO_INVALID_PARAMETER = "InfoInvalidParameter";
		public readonly static String INFO_EMPTY_PARAMETER = "InfoEmptyParameter";
		public readonly static String INFO_CANNOT_FIND_KEY = "InfoCannotFindKey";
		public readonly static String INFO_CANNOT_DELETE_KEY = "InfoCannotDeleteKey";
		public readonly static String INFO_CANNOT_LOAD_XML = "InfoCannotLoadXml";
		public readonly static String INFO_CANNOT_PERFORM_REFLECTION = "InfoCannotPerformReflection";
		public readonly static String INFO_INVALID_NUMBER_RANGE = "InfoInvalidNumberRange";
		public readonly static String INFO_INVALID_FORMAT = "InfoInvalidFormat";
		public readonly static String INFO_INVALID = "InfoInvalidSql";
		public readonly static String INFO_INVALID_PRIMARY_KEY = "InfoInvalidPrimaryKey";
		public readonly static String INFO_EMPTY_SECURITY = "InfoEmptySecurity";
		public readonly static String INFO_EMPTY_USER_NAME = "InfoEmptyUserName";
		public readonly static String INFO_EMPTY_PASSWORD = "InfoEmptyPassword";
		public readonly static String INFO_CANNOT_LOGIN = "InfoCannotLogin";
		public readonly static String INFO_CANNOT_LOAD_VALIDATION = "InfoCannotLoadValidation";
		public readonly static String INFO_CANNOT_LOAD_SERVICES = "InfoCannotLoadServices";
		public readonly static String INFO_CANNOT_LOAD_LISTS = "InfoCannotLoadLists";
		public readonly static String INFO_CANNOT_FIND_VALIDATION = "InfoCannotFindValidation";
		public readonly static String INFO_CANNOT_FIND_LIST = "InfoCannotFindList";
		public readonly static String INFO_EMPTY_REQUIRED_FIELD = "InfoEmptyRequiredField";
		public readonly static String INFO_EXCEEDS_FIELD_LENGTH = "InfoExceedsFieldLength";
		public readonly static String INFO_EMPTY_REQUEST = "InfoEmptyRequest";
		public readonly static String INFO_CANNOT_CHANGE_MAILING_ADDRESS = "InfoCannotChangeMailingAddress";
		public readonly static String INFO_CANNOT_DELETE_MAILING_ADDRESS = "InfoCannotDeleteMailingAddress";
		public readonly static String INFO_CANNOT_DELETE_MAILING_ADDRESS_DUE_TO_ASSOCIATION = "InfoCannotDeleteMailingAddressDueToAssociation";
		public readonly static String INFO_CANNOT_DELETE_MEMBER_DUE_TO_ASSOCIATION = "InfoCannotDeleteMemberDueToAssociation";
		public readonly static String INFO_MAILING_ADDRESS_FIRST = "InfoCreateMailingAddressFirst";
		public readonly static String INFO_LOGIN_ID_SHOULD_BE_UNIQUE = "InfoLoginIDShouldBeUnique";
		//Specific Information Messages
		//Error Messages
		public readonly static String CANNOT_CREATE_SYNCHEONIZE = "CannotCreateSynchronize";
		public readonly static String CANNOT_UPDATE_SYNCHEONIZE = "CannotUpdateSynchronize";
		public readonly static String CANNOT_DELETE_SYNCHEONIZE = "CannotDeleteSynchronize";
		public readonly static String CANNOT_READ_SYNCHEONIZE = "CannotReadSynchronize";
		public readonly static String CANNOT_READ_CONTACT = "CannotReadContact";
		public readonly static String CANNOT_DELETE_TRANSLATION_SYSTEM_DATA = "CannotDeleteTranslationSystemData";

		//Messages that need to be corrected
		public readonly static String CANNOT_READ_MEMBER_BY_CONTACT_ITEM = "MsgCannotReadMemberByContactItem";
		public readonly static String CANNOT_READ_CONTACT_ITEM_BY_MEMBER= "MsgCannotReadContactItemByMember";
		public readonly static String CANNOT_READ_CONTACT_ITEM_BY_CONTACT = "MsgCannotReadContactItemByContact";
		public readonly static String CANNOT_READ_MEMBER_BY_CONTACT_OWNER = "MsgCannotReadMemberContactByContactOwner";
		public readonly static String CANNOT_READ_MEMBER_BY_ADDRESS_AND_MEMBER = "MsgCannotReadMemberByAddressAndMember";
		public readonly static String CANNOT_READ_CONTACT_ITEM_MEMBER_BY_MEMBER = "MsgCannotReadContactItemMemberByMember";
		public readonly static String CANNOT_READ_CONTACT_ITEM_MEMBER_BY_CONTACT_ITEM = "MsgCannotReadContactItemMemberByContactItem";
		public readonly static String CANNOT_READ_CONTACT_ITEM_BY_FIRST_NAME_AND_CONTACT_OWNER= "MsgCannotReadContactItemByFirstNameAndContactOwner";
		public readonly static String CUSTOMFIELDS_READ_ALL_BY_CATEGORY_ERROR = "CustomfieldsReadAllByCategoryError";
		public readonly static String ISSUE_READ_ACTION_BY_ISSUE_ERROR = "IssueReadActionByIssueError";
		public readonly static String TRANSLATION_READ_MASTER_BY_TYPE_ERROR = "TranslationReadMasterByTypeError";
		public readonly static String TRANSLATION_READ_MASTER_BY_DISPLAY_NAME_ERROR = "TranslationReadMasterByDisplayNameError";
		public readonly static String TRANSLATION_READ_DETAIL_BY_DISPLAY_NAME_ERROR = "TranslationReadDetailByDisplayNameError";
		public readonly static String TRANSLATION_READ_BY_NAME_ERROR = "TranslationReadByNameError";
		public readonly static String SECURITY_CREATE_FORM_RIGHTS_BY_MEMBER_ERROR = "SecurityCreateFormRightsByMemberError";
		public readonly static String SECURITY_CREATE_FORM_RIGHTS_BY_GROUP_ERROR = "SecurityCreateFormRightsByGroupError";
		public readonly static String SECURITY_DELETE_CONTROL_RIGHTS_BY_FORM_RIGHT_ERROR = "SecurityDeleteControlRightsByFormRightError";
		public readonly static String SECURITY_DELETE_MEMBER_GROUP_BY_GROUP_ERROR = "SecurityDeleteMemberGroupByGroupError";
		public readonly static String SECURITY_DELETE_GROUP_FORM_BY_GROUP_ERROR = "SecurityDeleteGroupFormByGroupError";
		public readonly static String SECURITY_DELETE_ALL_GROUP_MENUS_BY_GROUP_ERROR = "SecurityDeleteAllGroupMenusByGroupError";
		public readonly static String SECURITY_READ_FORM_RIGHTS_BY_MEMBER_ERROR = "SecurityReadFormRightsByMemberError";
		public readonly static String SECURITY_READ_ALL_FORM_RIGHTS_BY_MEMBER_ERROR = "SecurityReadAllFormRightsByMemberError";
		public readonly static String SECURITY_READ_ALL_MENUS_BY_MEMBER_ERROR = "SecurityReadAllMenusByMemberError";
		public readonly static String SECURITY_READ_ALL_RIGHTS_ERROR = "SecurityReadAllRightsError";
		public readonly static String SECURITY_READ_ALL_GROUPS_ERROR = "SecurityReadAllGroupsError";
		public readonly static String SECURITY_READ_ALL_MENU_BY_GROUP_ERROR = "SecurityReadAllMenuByGroupError";
		public readonly static String SECURITY_READ_ALL_POPUP_MENU_ERROR = "SecurityReadAllPopupMenuError";
		public readonly static String SECURITY_READ_ALL_CONTROLS_BY_PARENT_CONTROL_ERROR = "SecurityReadAllControlsByParentControlError";
		public readonly static String SECURITY_READ_ALL_CONTROLS_BY_FORM_ERROR = "SecurityReadAllControlsByFormError";
		public readonly static String SECURITY_READ_ALL_MENUS_BY_PARENT_MENU_ERROR = "SecurityReadAllMenusByParentMenuError";
		public readonly static String SECURITY_READ_ALL_BY_MEMBER_MENU_ERROR = "SecurityReadAllByMemberMenuError";
		public readonly static String SECURITY_READ_GROUP_BY_MEMBER_ERROR = "SecurityReadGroupByMemberError";
		public readonly static String SECURITY_READ_CONTROL_DATA_BY_CONTROL_ERROR = "SecurityReadControlDataByControl";
		public readonly static String SECURITY_READ_BY_GROUP_MENU_ERROR = "SecurityReadByGroupMenuError";
		public readonly static String SECURITY_READ_BY_MENU_FORM_RIGHT_ERROR = "SecurityReadByMenuFormRightError";
		public readonly static String SECURITY_READ_FORM_RIGHT_BY_GROUP_ERROR = "SecurityReadFormRightByGroupError";
		public readonly static String BANK_READ_BY_MEMBER_ID_ERROR = "bankreadbymemberiderror";
		public readonly static String EVENT_READ_BY_DATE_ERROR = "EventReadByDateError";
		public readonly static String SECURITY_READ_BY_MEMBER_ERROR = "SecurityReadByMemberError";
		public readonly static String SECURITY_READ_BY_MEMBER_MENU_ERROR = "SecurityReadbymemberMenuError";
		public readonly static String SECURITY_READ_BY_CONTROL_ERROR = "SecurityReadByControlError";
		public readonly static String SECURITY_READ_ALL_BY_FORM_ERROR = "SecurityReadAllByFormError";
		public readonly static String SECURITY_READ_ALL_BY_PARENT_CONTROL_ERROR = "SecurityReadAllByParentControlError";
		public readonly static String SECURITY_READ_ALL_BY_MEMBER_ERROR = "SecurityReadAllByMemberError";
		public readonly static String SECURITY_READ_CONTROL_RIGHT_BY_MEMBER_FORM_ERROR = "SecurityReadControlRightByMemberAndFormNameError";
		public readonly static String EVENT_READ_BY_MEMBER_ERROR = "EventReadByMemberError";
		public readonly static String MSG_MEMBER_SAVE_CHANGES = "MsgMemberSaveChanges";
		public readonly static String MSGBOX_TITLE_SAVE_CHANGES = "MsgBoxTitleSaveChanges";
		public readonly static String MSG_MEMBER_SAVE_EDITED_CHANGES = "MsgMemberSaveEditedChanges";
		public readonly static String CANNOT_READ_ZIP_CODE = "cannotreadzipcode";
		public readonly static String MSG_PREPARE_ONLINE_MODE = "MsgPrepareOnlineMode";
		public readonly static String MSG_PREPARE_OFFLINE_MODE = "MsgPrepareOfflineMode";
		public readonly static String MSG_OPEN_SEARCH = "MsgOpenSearch";
		public readonly static String MSG_CLOSE_SEARCH = "MsgCloseSearch";
		public readonly static String MSG_ALREADY_PRESENT = "MsgThisIsAlreadyPresentInList";
		public readonly static String MSG_NO_FIELD_SEARCH = "MsgPleaseSelectAnyFieldToSearch";
		public readonly static String MSG_NO_TEXT_SEARCH = "MsgPleaseEnterTextToSearch";
		public readonly static String MSG_NO_ITEM = "MsgPleaseSelectAnyItemFromCombo";
		public readonly static String CANNOT_DELETE_PARENT_EVENT = "CannotDeleteParentEvent";
		public readonly static String MSG_NO_GUEST = "MsgNoGuestFound";
        public readonly static String MSG_BOOKING_ALREADY_CREATED = "MsgBookingAlreadyCreated";
		public readonly static String MSG_DELETE_GUEST = "MsgDeleteGuest";
		public readonly static String MSG_BOOKING_GUEST = "MsgBookingGuest";
		public readonly static String MSG_GUEST_ALREADY_ADDED = "MsgGuestAlreadyAdded";


		public readonly static String ERROR_INVALID_PARAMETER = "error.invalid.parameter";
		public readonly static String ERROR_FINDKEY = "error.findkey";
		public readonly static String ERROR_NUMBER_RANGE = "error.number.range";
		public readonly static String ERROR_USER_CREATE = "error.user.create";
		public readonly static String ERROR_USER_READ = "error.user.read";
		public readonly static String ERROR_USER_READ_ALL = "error.user.read.all";
		public readonly static String ERROR_USER_DELETE = "error.user.delete";
		public readonly static String ERROR_USER_UPDATE = "error.user.update";
		public readonly static String ERROR_LOAD_XML = "error.load.xml";
		public readonly static String ERROR_REFLECTION = "error.reflection";
		public readonly static String ERROR_INVALID_FORMAT = "error.invalid.format";
		public readonly static String ERROR_SQL = "error.sql";
		public readonly static String ERROR_INVALID_PRIMARYKEY = "error.invalid.primarykey";
		public readonly static String ERROR_SECURITY_EMPTY = "error.security.empty";
		public readonly static String ERROR_USERNAME_EMPTY = "error.username.empty";
		public readonly static String ERROR_PASSWORD_EMPTY = "error.password.empty";
		public readonly static String ERROR_CANNOT_LOGIN = "error.cannot.login";
		public readonly static String ERROR_LOADING_VALIDATION = "error.loading.validation";
		public readonly static String ERROR_LOADING_LISTS = "error.loading.lists";
		public readonly static String ERROR_FINDING_VALIDATION = "error.finding.validation";
		public readonly static String ERROR_FINDING_LIST = "error.finding.list";
		public readonly static String ERROR_REQUIRED_FIELD_EMPTY = "error.required.field.empty";
		public readonly static String ERROR_EXCEEDS_FIELD_LENGTH = "error.exceeds.field.length";
		public readonly static String ERROR_REQUEST_EMPTY = "error.request.empty";
		public readonly static String ERROR_ADDRESS_CREATE = "error.address.Create";
		public readonly static String ERROR_ADDRESS_READ = "error.address.read";
		public readonly static String ERROR_ADDRESS_READ_ALL = "error.address.read.all";
		public readonly static String ERROR_ADDRESS_UPDATE = "error.address.Update";
		public readonly static String ERROR_ADDRESS_DELETE = "error.address.Delete";
		public readonly static String ERROR_MEMBER_CREATE = "error.member.Create";
		public readonly static String ERROR_MEMBER_READ = "error.member.read";
		public readonly static String ERROR_MEMBER_READ_ALL = "error.member.read.all";
		public readonly static String ERROR_MEMBER_UPDATE = "error.member.Update";
		public readonly static String ERROR_MEMBER_DELETE = "error.member.Delete";
		public readonly static String ERROR_MEMBERGROUP_CREATE = "error.membergroup.Create";
		public readonly static String ERROR_MEMBERGROUP_READ = "error.membergroup.read";
		public readonly static String ERROR_MEMBERGROUP_READ_ALL = "error.membergroup.read.all";
		public readonly static String ERROR_MEMBERGROUP_DELETE = "error.membergroup.Delete";
		public readonly static String ERROR_MEMBERTYPE_CREATE = "error.membertype.Create";
		public readonly static String ERROR_MEMBERTYPE_READ = "error.membertype.read";
		public readonly static String ERROR_MEMBERTYPE_READ_ALL = "error.membertype.read.all";
		public readonly static String ERROR_MEMBERTYPE_UPDATE = "error.membertype.Update";
		public readonly static String ERROR_MEMBERTYPE_DELETE = "error.membertype.Delete";
		public readonly static String ERROR_RANK_CREATE = "error.rank.Create";
		public readonly static String ERROR_RANK_READ = "error.rank.read";
		public readonly static String ERROR_RANK_READ_ALL = "error.rank.read.all";
		public readonly static String ERROR_RANK_UPDATE = "error.rank.Update";
		public readonly static String ERROR_RANK_DELETE = "error.rank.Delete";
		public readonly static String ERROR_GROUP_CREATE = "error.group.Create";
		public readonly static String ERROR_GROUP_READ = "error.group.read";
		public readonly static String ERROR_GROUP_READ_ALL = "error.group.read.all";
		public readonly static String ERROR_GROUP_UPDATE = "error.group.Update";
		public readonly static String ERROR_GROUP_DELETE = "error.group.Delete";
		public readonly static String READ_REPORTS_BY_REPORT_BY = "error.report.read.report.by";
		public readonly static String READ_REPORTS_LIST = "error.report.read.list";
		public readonly static String EMAIL_CANNOT_READ_BY_EMAIL = "error.email.read.by.email";
		public readonly static String MEMBER_CANNOT_READ_BY_MEMBER = "error.member.read.by.member";
		public readonly static String MEMBER_CANNOT_READ_BY_PERSON = "error.member.read.by.person";
	}
}	
