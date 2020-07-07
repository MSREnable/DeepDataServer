using System.Collections.Generic;

namespace Microsoft.Research.EyeCatcher.Library
{
    /// <summary>
    /// A single gaze position for which several images will be taken.
    /// </summary>
    public interface IPositionData
    {
        /// <summary>
        /// The zero-based index of the positions recorded for this sequence. Several images will
        /// be taken for each position.
        /// </summary>
        int PositionIndex { get; }

        /// <summary>
        /// Number of images captured for position.
        /// </summary>
        int CaptureCount { get; }

        /// <summary>
        /// The raw horizontal pixel position, where zero is the leftmost pixel.
        /// </summary>
        double XRaw { get; }

        /// <summary>
        /// The raw vertical pixel position, where zero is the topmost pixel.
        /// </summary>
        double YRaw { get; }

        /// <summary>
        /// The logical horizontal pixel position where the user was intended to be
        /// looking. The application's logical position may be scaled.
        /// </summary>
        double X { get; }

        /// <summary>
        /// The logical vertical pixel position where the user was intended to be
        /// looking. The application's logical position may be scaled.
        /// </summary>
        double Y { get; }

        ISessionData GetSession();

        IEnumerable<IImageData> GetImages();
    }
}