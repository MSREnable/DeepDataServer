using System;

namespace Microsoft.Research.EyeCatcher.Library
{
    /// <summary>
    /// Data recorded for an individual image.
    /// </summary>
    public interface IImageData
    {
        /// <summary>
        /// The sequence number of the capture for the position, zero-based. For a given position, if all images
        /// are successfully uploaded, you should expect contiguous indexes from zero to N-1, where N is the number
        /// of images captured. If two images in a position within the same sequence have the same index, they
        /// should be considered duplicates.
        /// </summary>
        int CaptureIndex { get; }

        /// <summary>
        /// The UTC time the image was taken.
        /// </summary>
        DateTimeOffset Timestamp { get; }

        /// <summary>
        /// The Base64 encoding of the MD5 hash of the file. This can be used to match metadata files
        /// to the corresponding images data, to identify duplicate images and to check the file
        /// stored in Azure blob storage is the correct file (the MD5 hash is exposed by Azure).
        /// </summary>
        string MD5Hash { get; }

        DateTimeOffset? BeforeTrackerTimestamp { get; }
        double? BeforeTrackerX { get; }
        double? BeforeTrackerY { get; }

        DateTimeOffset? AfterTrackerTimestamp { get; }
        double? AfterTrackerX { get; }
        double? AfterTrackerY { get; }

        /// <summary>
        /// Get the ICapturePosition object for this image.
        /// </summary>
        /// <returns></returns>
        IPositionData GetPosition();
    }
}