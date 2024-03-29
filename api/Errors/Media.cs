﻿using System.Net;

namespace api.Errors
{
    public class MediaTypeNotSupportedError : ApiError
    {
        public MediaTypeNotSupportedError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotImplemented;
            Error = "media-0001";
            ErrorMessage = "Type not supported";
            Detail = detail;
        }
    }
    
    public class MediaNotFoundForAdContainerError : ApiError
    {
        public MediaNotFoundForAdContainerError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "media-0002";
            ErrorMessage = "No media found for ad container";
            Detail = detail;
        }
    }
    
    public class MediaEngineNotImplementedError : ApiError
    {
        public MediaEngineNotImplementedError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotImplemented;
            Error = "media-0003";
            ErrorMessage = "Specified engine not implemented";
            Detail = detail;
        }
    }
    
    public class MediaNotSpecifiedError : ApiError
    {
        public MediaNotSpecifiedError(string detail = "")
        {
            StatusCode = HttpStatusCode.PartialContent;
            Error = "media-0004";
            ErrorMessage = "Media does not have a specified media";
            Detail = detail;
        }
    }
    
    public class MediaNotFoundError : ApiError
    {
        public MediaNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "media-0005";
            ErrorMessage = "Media not found";
            Detail = detail;
        }
    }
    
    public class MediaInsufficientRightsError : ApiError
    {
        public MediaInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "media-0006";
            ErrorMessage = "Insufficient user rights for action";
            Detail = detail;
        }
    }
    
    public class MediaMissing2DError : ApiError
    {
        public MediaMissing2DError(string detail = "")
        {
            StatusCode = HttpStatusCode.PartialContent;
            Error = "media-0007";
            ErrorMessage = "Media missing 2D Media";
            Detail = detail;
        }
    }
    
    public class MediaMissing3DError : ApiError
    {
        public MediaMissing3DError(string detail = "")
        {
            StatusCode = HttpStatusCode.PartialContent;
            Error = "media-0008";
            ErrorMessage = "Media missing 3D Media";
            Detail = detail;
        }
    }
    
    public class MediaNotValidError : ApiError
    {
        public MediaNotValidError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "media-0009";
            ErrorMessage = "Media not valid";
            Detail = detail;
        }
    }
    
    public class MediaTypeNotValidError : ApiError
    {
        public MediaTypeNotValidError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "media-0010";
            ErrorMessage = "Media type not valid";
            Detail = detail;
        }
    }
    
    public class MediaSaveError : ApiError
    {
        public MediaSaveError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "media-0011";
            ErrorMessage = "Cannot save media";
            Detail = detail;
        }
    }
    
    public class MediaMissingUnityMediaError : ApiError
    {
        public MediaMissingUnityMediaError(string detail = "")
        {
            StatusCode = HttpStatusCode.PartialContent;
            Error = "media-0012";
            ErrorMessage = "Media does not have an Unity media";
            Detail = detail;
        }
    }
    
    public class MediaAlreadyExistUnityMediaError : ApiError
    {
        public MediaAlreadyExistUnityMediaError(string detail = "")
        {
            StatusCode = HttpStatusCode.PartialContent;
            Error = "media-0013";
            ErrorMessage = "Media already have an Unity media";
            Detail = detail;
        }
    }
    
    public class MediaMissingAssetBundleOrStateError : ApiError
    {
        public MediaMissingAssetBundleOrStateError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "media-0014";
            ErrorMessage = "You need to specify at least an AssetBundle or a State";
            Detail = detail;
        }
    }
}