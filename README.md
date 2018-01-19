.NET 4.6.1 C# SDK For Cogniac Public API

This client library provides access to most of the common functionality of the Cogniac public API. The main entry point is the Cogniac.Connection object.

The namespace Cogniac is used in this SDK, and all trivial types are nullable (E.G. long? ExpiresIn;).

# Class: Connection

__Cogniac.Connection(username, password, tenantId, token, urlPrefix)__

	Description: 			Create an authenticated Cogniac connection with known credentials.

	username (string):        	(Optional) The Cogniac account username (usually an email address).
					If username is not provided, then the contents of the
					COG_USER environment variable is used as the username.

	password (string):          	(Optional) The associated Cogniac account password.
					If password is not provided, then the contents of the
					COG_PASS environment variable is used as the username.

	tenantId (string):          	(Optional) tenant_id with which to assume credentials.
					This is only required if the user is a member of multiple 
					tenants. If tenant_id is not provided, and the user is a member 
					of multiple tenant then the contents of the COG_TENANT 
					environment variable is used as the tenant_id.

	token (string):             	(Optional) If a known API access token is provided, it can be 
					used instead of all other parameters. (This approach is recommended). 
	
	urlPrefix (string):         	(Optional) Do not use this parameter unless the API has 
					been relocated to a different address. The default value is always used.

The following are methods are members of the Cogniac.Connection object:

__GetAllAuthorizedTenants(username, password, urlPrefix)__
		
	Description: 			Static method that returns an AuthorizedTenants 
					object containing all tenants that a specific user
					is associated with. All the input parameters are 
					used in the same manner as creating a Connection object.
		
	Return:				Cogniac.Tenants - multi-member object.
				
__GetAuth()__
		
	Description:			Returns an object containing the authentication 
					information with the Cogniac API.
		
	Return:				Cogniac.Auth - multi-member object.
		
__UploadMedia(forceSet, fileName, mediaTimestamp, forceOverwrite, metaTags, isPublic, externalMediaId, 
		originalUrl, originalLandingUrl, license, authorProfileUrl, author, title, sourceUrl, 
		previewUrl, localGatewayUrl)__
		
	Description:			Uploads a media file to the Cogniac system.
		
	forceSet (string) 		(Optional) One of "training" or "validation", null otherwise.
					When it is null, it is random.
	
	fileName (string):		(Optional) The full path and file name of media item 
					to upload. If this is not provided, sourceUrl must be provided instead.
		
	mediaTimestamp (long):		(Optional) User-specified image timestamp.
	
	forceOverwrite (boolean):	(Optional) Overwrite any existing, identical media files and metadata.

	metaTags (string array):	(Optional) Other associated metadata.

	isPublic (boolean):		(Optional) Decides if the media is public or not, false if not provided.

	externalMediaId (string):	(Optional) A unique ID for this media from it's external data source.

	originalUrl (string):		(Optional) The original URL for this media.

	originalLandingUrl (string):	(Optional) The original landing URL for this media.

	license (string):		(Optional) License information about this media.

	authorProfileUrl (string):	(Optional) URL of the media owner.

	author (string):		(Optional) Name of the media owner.

	title (string):			(Optional) Title of this media.

	sourceUrl (string):		(Optional) Can pass an optional URL to the media to be created 
					instead of a file. If not provided, fileName must be provided instead.

	previewUrl (string):		(Optional) URL for media preview image for display.

	localGatewayUrl (string):	(Optional) URL to upload media to, this is used when a local gateway is installed.

	Return:				Cogniac.Media - multi-member object.

__DeleteMedia(mediaId, localGatewayUrl)__

	Description:			Deletes a specific media file from the Cogniac system.

	mediaId (string):		(Required) The media ID of the object to be deleted from the Cogniac system.

	Return:				Boolean - 'true' on success, 'false' otherwise.
	
__GetAllSubjects(tenantId)__

	Description:			Gets all subjects associated with a given tenant ID.
	
	tenantId (string):		(Required) The tenant ID to pass to the API.

	Return:				Cogniac.Subjects - multi-member object.
	
__GetSubject(subjectUid)__

	Description:			Gets a subject associated with a given subject UID.
	
	subjectUid (string):		(Required) The subject UID to pass to the API.

	Return:				Cogniac.Subject - multi-member object.
	
__GetAllApplications(tenantId)__

	Description:			Gets all applications associated with a given tenant ID.
	
	tenantId (string):		(Required) The tenant ID to pass to the API.

	Return:				Cogniac.Applications - multi-member object.
	
__GetApplication(applicationId)__

	Description:			Gets an application associated with a given application ID.
	
	applicationId (string):		(Required) The application ID to pass to the API.

	Return:				Cogniac.Application - multi-member object.
	
__GetTenant(tenantId)__

	Description:			Gets a tenant's information given a tenant ID.
	
	tenantId (string):		(Required) The tenant ID to pass to the API.

	Return:				Cogniac.Tenant - multi-member object.
	
__AssociateMediaToSubject(mediaId, subjectUid, forceFeedback)__

	Description:			Associates an uploaded media to a given subject.
	
	mediaId (string):		(Required) The unique ID of the media.

	subjectUid (string):		(Required) The unique subject UID to associate the media with.

	foreFeedback (bool):		(Optional) Forces the cogniac system to use the media for feedback.
					This value defaults to false if.

	Return:				Cogniac.Tenant - multi-member object.

__GetMedia(mediaId)__

	Description:			Gets a Cogniac.Media object from a given media ID.
	
	mediaId (string):		(Required) The unique ID of the media.

	Return:				Cogniac.Media - multi-member object.
	
__CreateSubject(name, description, publicRead, publicWrite)__

	Description:			Create a subject in the Cogniac system.
	
	name (string):			(Required) The name of the subject.
	
	description (string):		(Optional) The description of the subject.
	
	publicRead (boolean):		(Optional) Flag to select if the subject can be read publicly.
	
	publicWrite (boolean):		(Optional) Flag to select if the subject can be written to publicly.
	
	Return:				Cogniac.Subject - multi-member object.
	
__DeleteSubject(subjectUid)__

	Description:			Deletes a subject from the Cogniac system.
	
	subjectUid (string):		(Required) The unique ID of the subject to delete.
	
	Return:				Boolean - 'true' on success, 'false' otherwise.
	
__CreateApplication(name, type, description, inputSubjects, outputSubjects, releaseMetrics, detectionThresholds, detectionPostUrls, gatewayPostUrls, active, requestedFeedbackPerHour, refreshFeedback, appManagers)__

	Description:			Creates an application in the Cogniac system.
	
	name (string):			(Required) Application name.

	type (string):			(Required) Type of application 
					(See API docs for valid types).

	description (string):		(Optional) Application description.
	
	inputSubjects (string array):	(Optional) List of input subjects to use.
	
	outputSubjects (string array):	(Optional) List of output subjects to use.
	
	releaseMetrics (string):	(Optional) Release metrics string.
	
	detectionThresholds (Dict):	(Optional) String dictionary of detection thresholds.
	
	detectionPostUrls 		(Optional) URL's where model detections will be 
	(string array):			surfaced in addition to web and iOS interfaces.
					
	gatewayPostUrls (string array):	(Optional) A list of URL's where model detections 
					will be surfaced from the gateway.
					
	active (boolean):		(Optional) Controls if the the application is active or not.
	
	requestedFeedbackPerHour	(Optional) Override the target rate of feedback to 
	(integer):			surface per hour.
	
	refreshFeedback (boolean):	(Optional) Flag to control whether the images waiting 
					for user feedback should be re-evaluated by the new
					model when a new model is released.
					
	appManagers (string array):	(Optional) List of the application managers.

	Return:				Cogniac.Application - multi-member object.
	
__GetSubjectMediaAssociations(subjectUid)__

	Description:			Gets the subject media association given a subject UID.
	
	subjectUid (string):		(Required) The subject UID to pass to the API.

	Return:				Cogniac.SubjectMediaAssociations - multi-member object.
	
__GetMediaSubjects(mediaId)__

	Description:			Gets the subjects associated to a given media ID.
	
	mediaId (string):		(Required) The unique ID of the media.

	Return:				Cogniac.MediaSubjects - multi-member object.

The following are the objects used in the SDK and their members.
		
# Class: AuthorizedTenants

	Tenants (object array)		
		List of Tenant objects.
	
# Class: Tenant

	TenantId (string)	
		The tenant ID.

	Name (string)		
		The tenant's given name.
	
	Description (string)	
		The description of the tenant.
	
	CreatedAt (double)	
		The creation time of the tenant.
	
	ModifiedAt (double)	
		The time the tenant was last modified.
	
	CreatedBy (string)	
		String containing the ID of the tenant's creator.

# Class: Auth

	UserId (string)			
		The unique user ID from the Cogniac system.

	AccessToken (string)		
		The access token generated by the Cogniac system.

	TenantId (string)		
		The associated tenant ID.

	ExpiresIn (long)		
		The amount of seconds left for the token to expire.

	TokenType (string)		
		The type of the access token.

	TenantName (string)		
		The associated tenant's given name.

	UserEmail (string)		
		The associated user's email address.
		
# Class: Media

	SetAssignment (string)		
		Denotes whether the media will be used as training or validation.

	NumFrames (long)		
		Number of frames.

	Frame (long)			
		Frame value.

	Video (bool)			
		'True' if media is video.

	FrameDurations (object array)	
		Array containing duration of each frame.

	Duration (long)			
		Duration of the video file (if provided).

	Size (long)			
		Size of the media created.

	NetworkCameraId (string)	
		ID string for the network camera, if used.

	ResizeUrls (Dictionary)		
		A map containing all the resize URLs generated by the Cogniac system.

	OriginalUrl (string)		
		The original URL of the media.

	ImageWidth (long)		
		The width of the image uploaded.

	Filename (string)		
		The name of the file.

	OriginalLandingUrl (string)	
		The original landing URL.

	Fps (double)			
		Frames per second.

	UploadedByUser (string)		
		The user who uploaded the media.

	MediaTimestamp (long)		
		The provided media timestamp.

	MediaUrl (string)		
		The media URL.

	Status (string)			
		The upload status message.

	MediaId (string)		
		The unique Cogniac media ID. This is generated for anything uploaded.

	ExternalMediaId (string)	
		An external media ID provided by the user.

	TimeBase (object)		
		The time base of the media.

	SourceUrl (string)		
		The source URL of the media, if provided.

	AuthorProfileUrl (string)	
		The profile URL of the author.

	MediaSrc (string)		
		The source of the upload process of the media.

	ParentMediaId (string)		
		The parent media ID of the uploaded media, if available.

	Md5 (string)			
		Md5 hash of the media.

	ParentMediaIds (string array)	
		An array of strings containing the parent media IDs.

	MetaTags (string array)		
		An array containing all the uploaded media's meta-tags.

	License (string)		
		A string containing the uploaded media's license, if provided.

	TenantId (string)		
		The tenant ID associated with the media.

	CreatedAt (double)		
		Unix media creation timestamp.

	Author (string)			
		The author of the media uploaded, if provided.

	PreviewUrl (string)		
		The preview URL of the media uploaded.

	ImageHeight (long)		
		The image height of the media uploaded.

	MediaFormat (string)		
		The format of the media uploaded.

	Title (string)			
		The user defined title of the uploaded media.
	
# Class: Applications

	Application (object array)	
		Object array of type Cogniac.Application.
	
# Class: Application

	RefreshFeedback (bool)	
		Flag to refresh feedback.
	
	ApplicationId (string)
		The application's unique ID.
	
	ValidationDataCount (long)
		The validation data count.
	
	SystemFeedbackPerHour (long)	
		The feedback per hour value.
	
	AppManagers (string array)
		A list of the application managers.
	
	OutputSubjectsExternalIds (object) 
		An object representing a list of output subject external ID
		
# Class: CaptureId

	Id (string)
		A capture ID returned when a media is associated to a subject.
		
# Class: Subjects

	Subject (object array)
		Object array of type Cogniac.Subject.
		
# Class: Subject

	SubjectUid (string)
		The unique ID of a given subject.
		
	Description (string)
		The description string of a given subject.
		
	CreatedBy (string)
		The string containing the creator of the subject.
		
	ConsensusSummary (object array)
		Returns an array of Cogniac.ConsensusSummary objects.
		
	ExpiresIn (long)
		The time remaining for the subject to expire.
		
	TenantId (string)
		Tenant ID assocaited with the given subject.
		
	CreatedAt (double)
		Time of creation of the given subject.
		
	ModifiedAt (double)
		Last time when the given subject was modified.
		
	PublicWrite (bool)
		If the subject can be written to publicly, this flag is set to true.
		
	ExternalId (string)
		A string containing any useful external ID.
		
	PublicRead (bool)
		If the subject can be read publicly, this flag is set to true.
		
	Name (string)
		The given name of the subject.
		
	MediaId (string)
		Media ID property.
		
	Probability (double)
		Probability value.
		
	Focus (object)
		Focus data.
		
	AppDataType (string)
		Application data type.
		
	UpdatedAt (double)
		Updated at time.
		
	Consensus (string)
		Consensus data string.
		
	AppData (object)
		Application data object.
		
# Class: ConsensusSummary

	Count (long)
		A count of the consensus summary.
		
	Consensus (string)
		A string for the consensus.
		
	AppDataType (object)
		An object used to show the application data type.
		
# Class: SubjectMediaAssociations

	Paging (object)
		The object for paging.
	
	Data (object array)
		Data on the current page.
		
# Class: Datum

	OtherMedia (object array)
		Other media property
		
	Media (object)
		Media object associated.
		
	Focus (object)
		Focus data.
		
	MediaList (object array)
		Media objects list.
		
	Subject (object)
		Subject object associated.

# Class: AppDatum

	Box (object)
		Box data object.
		
	Probability (double)
		Probability value.
		
# Class: Box

	Y1 (long)
		Y1 value.
		
	Y0 (long)
		Y0 value.
		
	X1 (long)
		X1 value.
		
	X0 (long)
		X0 value.
		
# Class: Paging

	Next (string)
		Next page property value.

# Class: MediaSubjects

	Data (object)
		Data array containing media subjects
		
All classes can utilize the following methods:

__Cogniac.[OBJECT].FromJson(json)__

	Description:			Initializes any Cogniac object from a valid JSON string
	
	json (string):			(Required) The JSON string to use.

	Return:				Cogniac.[OBJECT] - multi-member Cogniac object.

__Cogniac.Serialize.ToJson(object)__

	Description:			Serializes any Cogniac.[OBJECT] to a valid JSON string.
	
	object (object):		(Required) The Cogniac.[OBJECT] to use.

	Return:				A valid JSON string.
	
# Utility: CogUpload.exe

Depends on all the "Release" DLLs output from the 'CogniacCSharpSDK' project.

	Usage: CogUpload [-OPTION1 [ARG1]] [-OPTION2 [ARG1] [ARG2] [ARG3] ...] [-OPTION3 [ARG1]] ...

	   * All options starts with '-' followed by a space after the option.
	   * Arguments to each option follow the option directly.
	   * If an option takes an array, the members are provided as space separated arguments.
	   * All options take a single string argument unless specified.

	List of available options:

	-fs   | -forceSet           Either 'training' or 'validation', don't provide it otherwise.
	-f    | -FileName           Full path and file name of meida.
	-d    | -Dirname            Directory of media files to process.
	-u    | -Username           Cogniac issued username.
	-p    | -Password           Cogniac issued password.
	-tid  | -TenantId           Valid Cogniac tenant ID.
	-tk   | -Token              Valid Cogniac access token.
	-up   | -UrlPrefix          URL prefix of the Cogniac API.
	-lgu  | -LocalGatewayUrl    Local gateway URL.
	-mt   | -MediaTimestamp     Time stamp of the media.
	-ff   | -ForceFeedback      ['True' or 'False' (default)] Force feedback after upload.
	-fow  | -ForceOverwrite     ['True' (default) or 'False'] Force overwrite of media.
	-mtg  | -MetaTags           [Array] List of meta tags of the media.
	-isp  | -IsPublic           ['True' or 'False' (default)] Set media to public.
	-emid | -ExternalMediaId    External media ID.
	-ou   | -OriginalUrl        Original media URL.
	-olu  | -OriginalLandingUrl Original landing URL.
	-l    | -License            License link or text of the media.
	-apu  | -AuthorProfileUrl   Author profile URL.
	-t    | -Title              Title of the media.
	-su   | -SourceUrl          Source URL of the media.
	-pu   | -PreviewUrl         Preview URL of the media.
	-suid | -SubjectUid         The Cogniac subject to associate the media with.
	-r    | -recursive          ['True' or 'False' (default)] Recursively upload files in 'DirName'.
	-h    | -Help               Displays this help message.

	   Note 1: 'TenantId' must always be provided unless 'Token' is used.
	   Note 2: Either use 'Token' or 'Username' and 'Password' but not both. If both are provided 'Token'
		   will be used and 'Username' and 'Password' will be ignored.
	   Note 3: 'SubjectUid' must always be provided. It applies to all 'FileName' and/or 'DirName' uploads.
	   Note 4: 'if 'DirName' is provided, all media within the directory will be uploaded recursively.
	   Note 5: 'MediaTimestamp', 'ExternalMediaId', 'SourceUrl', 'OriginalUrl', 'OriginalLandingUrl',
		   'Title', 'SourceUrl' and 'PreviewUrl' are single media file options only and cannot be
		   applied to an entire directory. The rest of the options apply to every media file within
		   the provided directory to process.
	   Note 6: If both 'FileName and 'DirName' are provided, the single file will process first
		   then the entire directory will process second.
	   Note 7: Spaces in any string feild are NOT permitted, please use '+' instead.
		   For example: '-f my image.png' is invalid, use '-f my+image.png' instead.
	   Note 8: Options are NOT case-sensitive.

	   Example 1:  CogUpload -f C:\Path\To\Image.png -u MYUSER -p MYPASSWORD -tid ABC123 -suid DEF456
	   Example 2:  CogUpload -dirName C:\Path\To\Images -u MYUSER -p MYPASSWORD -tid ABC123 -suid DEF456
	   Example 3:  CogUpload -fileName C:\Path\To\Image.png -token ABCDEF123456
		       -mtg John+Doe BlackBerry Android+7.1.1 -IsPublic True -ff True -suid DEF456
		       
# SDK Usage Examples

All the examples assume: 'using Cogniac;'

Connecting to Cogniac with username, password and tenant ID.

	var cc = new Connection("someUser@company.com", "MyPassword", "ValidTenantID");
	if (cc != null)
	{
		var ao = cc.GetAuthObject();
		if (ao != null)
		{
			// Get the token for later use
			string token = ao.AccessToken;
		}
	}

Connecting to Cogniac with a token.

	string token = "ValidTokenSequence";
	var cc = new Connection cc = new Connection(token: token);
	if (cc != null)
	{
		// The rest of the program
	}

Connecting to Cogniac with a username and password but no tenant ID. (username is assumed to have 1 tenant)

	var at = Connection.GetAllAuthorizedTenants("someUser@company.com", "MyPassword");
	if (at != null)
	{
		// Object 'at' will contain all the authorized tenants of this user, we use the first one
		var cc = new Connection("someUser@company.com", "MyPassword", at.Tenants[0].TenantId);
		var ao = cc.GetAuthObject();
		if (ao != null)
		{
			// Get the token for later use
			string token = ao.AccessToken;
		}	
	}

The following examples will assume a Cogniac.Connection object 'cc' has already been created properly.

Uploading a media item and associating it with a subject

	string subjectUid = "KnownSubjectUid";
	bool forceFeedback = true;
	string[] tags = new string[] {"Media Owner", "BlackBerry KeyOne", "Android 7.1.1"};
	string fullFileName = "Path\To\Image.jpg";
	var m = cc.UploadMedia(fileName: fullFileName, metaTags: tags, forceOverwrite: true, isPublic: false);
	if (m != null)
	{
		var ci = _con.AssociateMediaToSubject(m.MediaId, subjectUid, forceFeedback);
		if (ci != null)
		{
			Console.WriteLine($"Association successful. CaptureId: '{ci.Id}'");
		}
	}

Deleting a media from the Cogniac system

	string mediaId = "KnownMediaId";
	if (cc.DeleteMedia(mediaId))
	{
		Console.WriteLine("Media deleted");
	}
	else
	{
		Console.WriteLine("Error deleting media");
	}

Get subjects, subject, applications, application and tenant

	string tenantId = "KnownTenantId";
	string subjectUid = "KnownSubjectUid";
	string appId = "KnownApplicationId";

	var subjects = cc.GetAllSubjects(tenantId);
	var subject cc.GetSubject(subjectUid);
	var apps = cc.GetAllApplications(tenantId);
	var app = cc.GetApplication(appId);
	var t = cc.GetTenant(tenantId);

Create a Cogniac Application

	var app = cc.CreateApplication("TestApp", "classification");
	if (app != null)
	{
		// Application created properly, view it in JSON
		Console.WriteLine(Serialize.ToJson(app));
	}

Create a Cogniac Subject

	var sub = cc.CreateSubject("test", "this is a test subject");
	if (sub != null)
	{
		// Subject created properly, view it in JSON
		Console.WriteLine(Serialize.ToJson(sub));
	}

Get subject media associations

	string subjectUid = "KnownSubjectUid";
	var sma = cc.GetSubjectMediaAssociations(subjectUid);

Get media subjects

	string mediaId = "KnownMediaId";
	var ms = cc.GetMediaSubjects(mediaId);
