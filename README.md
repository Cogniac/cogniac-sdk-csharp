.NET 4.6.1 C# SDK For Cogniac Public API

This client library provides access to most of the common functionality of the Cogniac public API. The main entry point is the CogniacConnection object.

# H1 CogniacConnection

		__CogniacConnection(username, password, tenantId, token, urlPrefix)__

        Description: 					Create an authenticated Cogniac connection with known credentials.

        username (string):        		(Optional) The Cogniac account username (usually an email address).
										If username is not provided, then the contents of the
										COG_USER environment variable is used as the username.

        password (string):          	(Optional) The associated Cogniac account password.
										If password is not provided, then the contents of the
										COG_PASS environment variable is used as the username.

        tenantId (string):          	(Optional) tenant_id with which to assume credentials.
										This is only required if the user is a member of multiple tenants.
										If tenant_id is not provided, and the user is a member of multiple tenant
										then the contents of the COG_TENANT environment variable is used
										as the tenant_id.

        token (string):             	(Optional) If a known API access token is provided, it can be used instead of all
										other parameters. (This approach is recommended). 
                                      
        urlPrefix (string):         	(Optional) Do not use this parameter unless the API has been relocated to 
										a different address. The default value is always used.

The following are methods members of the CogniacConnection object:

		__GetAllAuthorizedTenants(username, password, urlPrefix)__
		
        Description: 					Static method that returns a CogniacTenantsObject containing all
										Tenants that a specific user is associated with. All the input parameters are 
										used exactly in the same manner as creating a CogniacConnection object.
		
		Return:							CogniacTenantsObject - multi-member object.
				
		__GetCogniacAuthObject()__
		
		Description:					Returns an object containing the authentication information with the Cogniac API.
		
		Return:							CogniacAuthObject - multi-member object.
		
		__UploadMedia(fileName, mediaTimestamp, parentMediaId, parentMediaIds, frame, forceOverwrite, metaTags, isPubli, 
					isVideo, externalMediaId, originalUrl, originalLandingUrl, license, authorProfileUrl, author, title, 
					sourceUrl, previewUrl, localGatewayUrl)__
		
		Description:					Uploads a media file to the Cogniac system.
		
		fileName (string):				(Optional) The full path and file name of media item to upload. If this is
										not provided, sourceUrl must be provided instead.
		
		mediaTimestamp (long):			(Optional) User-specified image timestamp.
		
		parentMediaId (string):			(Optional) Cogniac ID of the parent media to this item.
		
		parentMediaIds (string array):	(Optional) Array of parent media_ids from which the media was derived.
		
		frame (integer):				(Optional) The frame number of this media item in its parent video media.
		
		forceOverwrite (boolean):		(Optional) Overwrite any existing, identical media files and metadata.
		
		metaTags (string array):		(Optional) Other associated metadata.
		
		isPublic (boolean):				(Optional) Decides if the media is public or not, false if not provided.
		
		externalMediaId (string):		(Optional) A unique ID for this media from it's external data source.
		
		originalUrl (string):			(Optional) The original URL for this media.
		
		originalLandingUrl (string):	(Optional) The original landing URL for this media.
		
		license (string):				(Optional) License information about this media.
		
		authorProfileUrl (string):		(Optional) URL of the media owner.
		
		author (string):				(Optional) Name of the media owner.
		
		title (string):					(Optional) Title of this media.
		
		sourceUrl (string):				(Optional) Can pass an optional URL to the media to be created instead of a file.
										If this is not provided, fileName must be provided instead.
		
		previewUrl (string):			(Optional) URL for media preview image for display.
		
		localGatewayUrl (string):		(Optional) URL to upload media to, this is used when a local gateway is installed.
		
		Return:							CogniacCreateMediaObject - multi-member object.
		
		__DeleteMedia(mediaId, localGatewayUrl)__
		
		Description:					Deletes a specific media file from the Cogniac system.
		
		mediaId (string):				(Required) The media ID of the object to be deleted from the Cogniac system.
		
		Return:							bool - 'true' on success, 'false' otherwise.
		
The following are the objects used in the SDK and their members.
		
# H1 CogniacTenantsObject

		Tenants (object array)			List of Tenant objects.
		
		Tenants[].TenantId (string)		The tenant ID.
		
		Tenants[].Name (string)			The tenant's given name.

# H1 CogniacAuthObject

		UserId (string)					The unique user ID from the Cogniac system.
		
		AccessToken (string)			The access token generated by the Cogniac system.
		
		TenantId (string)				The associated tenant ID.
		
		ExpiresIn (long)				The amount of seconds left for the token to expire.
		
		TokenType (string)				The type of the access token.
		
		TenantName (string)				The associated tenant's given name.
		
		UserEmail (string)				The associated user's email address.
		
# H1 CogniacCreateMediaObject

		SetAssignment (string)			Denotes whether the media will be used as training or validation.
		
		NumFrames (object)				Number of frames.
		
		Frame (object)					Frame value.
		
		Video (bool)					'True' if media is video.
		
		FrameDurations (object array)	Array containing duration of each frame.
		
		Duration (object)				Duration of the video file (if provided).
		
		Size (long)						Size of the media created.
		
		NetworkCameraId (object)		ID object for the network camera, if used.
		
		ResizeUrls (Dictionary)			A map containing all the resize URLs generated by the Cogniac system.
		
		OriginalUrl (object)			The original URL of the media.
		
		ImageWidth (long)				The width of the image uploaded.
		
		Filename (string)				The name of the file.
		
		OriginalLandingUrl (object)		The original landing URL.
		
		Fps (object)					Frames per second.
		
		UploadedByUser (string)			The user who uploaded the media.
		
		MediaTimestamp (long)			The provided media timestamp.
		
		MediaUrl (string)				The media URL.
		
		Status (string)					The upload status message.
		
		MediaId (string)				The unique Cogniac media ID. This is generated for anything uploaded.
		
		ExternalMediaId (object)		An external media ID provided by the user.
		
		TimeBase (object)				The time base of the media.
		
		SourceUrl (object)				The source URL of the media, if provided.
		
		AuthorProfileUrl (object)		The profile URL of the author.
		
		MediaSrc (string)				The source of the upload process of the media.
		
		ParentMediaId (object)			The parent media ID of the uploaded media, if available.
		
		Md5 (string)					Md5 hash of the media.
		
		ParentMediaIds (object array)	An array of objects containing the parent media IDs.
		
		MetaTags (string array)			An array containing all the uploaded media's meta-tags.
		
		License (string)				A string containing the uploaded media's license, if provided.
		
		TenantId (string)				The tenant ID associated with the media.
		
		CreatedAt (double)				Unix media creation timestamp.
		
		Author (string)					The author of the media uploaded, if provided.
		
		PreviewUrl (object)				The preview URL of the media uploaded.
		
		ImageHeight (long)				The image height of the media uploaded.
		
		MediaFormat (string)			The format of the media uploaded.
		
		Title (string)					The user defined title of the uploaded media.