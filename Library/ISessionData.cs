using System;
using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    public interface ISessionData
    {
        /// <summary>
        /// Identifier for installation instance. Different InstanceId should indicate different machine.
        /// Same InstanceId may imply same machine.
        /// </summary>
        Guid InstanceId { get; }

        /// <summary>
        /// Identifier for user instance. Different UserId should indicate different user. Same UserId
        /// may imply same user.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Identifier for session. SessionId should uniquely identify each session.
        /// </summary>
        Guid SessionId { get; }

        /// <summary>
        /// The content of the USERNAME environment variable, if available.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// The account name of the user.
        /// </summary>
        string AccountName { get; }

        string CurrentOrientation { get; }

        string PreviewRotation { get; }

        double? DiagonalSizeInInches { get; }
        double? Horizontal35mmEquivalentFocalLength { get; }
        int HorizontalPoints { get; }
        string HostManufacturer { get; }
        string HostModel { get; }
        string HostSku { get; }
        int ImageHeight { get; }
        int ImageWidth { get; }
        int Margin { get; }
        int MaxImagesPerPosition { get; }
        int MinImagesPerPosition { get; }

        string NativeOrientation { get; }

        double RawDpiX { get; }
        double RawDpiY { get; }
        double ScreenHeightInRawPixels { get; }
        double RawPixelsPerViewPixel { get; }
        double ScreenWidthInRawPixels { get; }
        DateTimeOffset SessionTimestamp { get; }
        double? Vertical35mmEquivalentFocalLength { get; }
        int VerticalPoints { get; }
        string VideoCaptureId { get; }
        string VideoCaptureName { get; }

        string VideoCapturePanel { get; }

        double WindowHeight { get; }
        double WindowWidth { get; }

        /// <summary>
        /// Background color red component.
        /// </summary>
        int BackgroundRed { get; }

        /// <summary>
        /// Background color green component.
        /// </summary>
        int BackgroundGreen { get; }

        /// <summary>
        /// Background color blue component.
        /// </summary>
        int BackgroundBlue { get; }

        /// <summary>
        /// Has the user manually set the camera position?
        /// </summary>
        bool IsCustomCameraPosition { get; }

        /// <summary>
        /// X part of raw pixel position where center of camera is.
        /// </summary>
        double CameraX { get; }

        /// <summary>
        /// Y part of raw pixel position where center of camera is.
        /// </summary>
        double CameraY { get; }

        /// <summary>
        /// CameraX scale to millimetres.
        /// </summary>
        double CameraMmX { get; }

        /// <summary>
        /// CameraY scaled to millimetres.
        /// </summary>
        double CameraMmY { get; }

        IEnumerable<IPositionData> GetPositions();
    }
}